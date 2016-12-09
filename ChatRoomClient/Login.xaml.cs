using System.Text;
using System.Windows;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ChatRoomClient
{



    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private XmlDocument xmldoc;

        public Login()
        {
            InitializeComponent();
            Config();
            myClient = new TcpClient();
           
        }
        private XmlNode account = null;
        private XmlNode password = null;
        private void Config()
        {
            xmldoc = new XmlDocument();
            if (File.Exists(@"data.xml"))
            {
                xmldoc.Load(@"data.xml");

                //初始化界面
                XmlNode accountCheck = xmldoc.SelectSingleNode("/root/settings/isRememberAccount");
                XmlNode passwordCheck = xmldoc.SelectSingleNode("/root/settings/isRememberPassword");
                account = xmldoc.SelectSingleNode("/root/data/account");
                password = xmldoc.SelectSingleNode("/root/data/password");

                //根据配置文件 初始化accountcheckbox
                if (accountCheck.InnerText.Equals("False"))
                {
                    cbxRememberAccount.IsChecked = false;
                }
                else
                {
                    cbxRememberAccount.IsChecked = true;
                }
                //根据配置文件 初始化passwordcheckbox
                if (passwordCheck.InnerText.Equals("False"))
                {
                    cbxRememberPassWord.IsChecked = false;
                }
                else
                {
                    cbxRememberPassWord.IsChecked = true;
                }

                if (cbxRememberAccount.IsChecked == true)
                {
                    tbxAccount.Text = account.InnerText;
                }
                if (cbxRememberPassWord.IsChecked == true)
                {
                    pbxPassWord.Password = password.InnerText;
                }

            }
            else
            {
                XmlDeclaration dec = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                xmldoc.AppendChild(dec);
                XmlElement root = xmldoc.CreateElement("root");
                xmldoc.AppendChild(root);

                XmlElement theSettings = null, theData = null, theElement = null;

                //设置Settings节点
                theSettings = xmldoc.CreateElement("settings");
                theElement = xmldoc.CreateElement("isRememberAccount");
                theElement.InnerText = isRememberAccountChecked.ToString();
                theSettings.AppendChild(theElement);
                theElement = xmldoc.CreateElement("isRememberPassword");
                theElement.InnerText = isRememberPassWordChecked.ToString();
                theSettings.AppendChild(theElement);
                root.AppendChild(theSettings);

                //设置data节点
                theData = xmldoc.CreateElement("data");
                theElement = xmldoc.CreateElement("account");
                theElement.InnerText = "";
                theData.AppendChild(theElement);
                theElement = xmldoc.CreateElement("password");
                theElement.InnerText = "";
                theData.AppendChild(theElement);
                root.AppendChild(theData);

                xmldoc.Save("data.xml");
            }
        }

        #region 记住密码和记住账号
        private bool isRememberAccountChecked = false;
        private bool isRememberPassWordChecked = false;

        private void cbxRememberAccount_Checked(object sender, RoutedEventArgs e)
        {
            isRememberAccountChecked = true;
            XmlNode accountCheck = xmldoc.SelectSingleNode("/root/settings/isRememberAccount");
            accountCheck.InnerText = cbxRememberAccount.IsChecked.ToString();
        }

        private void cbxRememberAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            isRememberAccountChecked = false;
            if (cbxRememberPassWord.IsChecked == true)
            {
                cbxRememberPassWord.IsChecked = false;
                isRememberPassWordChecked = false;
            }
            XmlNode accountCheck = xmldoc.SelectSingleNode("/root/settings/isRememberAccount");
            accountCheck.InnerText = cbxRememberAccount.IsChecked.ToString();
        }

        private void cbxRememberPassWord_Checked(object sender, RoutedEventArgs e)
        {
            isRememberPassWordChecked = true;
            if (cbxRememberAccount.IsChecked == false)
            {
                cbxRememberAccount.IsChecked = true;
                isRememberAccountChecked = true;
            }
            XmlNode accountCheck = xmldoc.SelectSingleNode("/root/settings/isRememberPassword");
            accountCheck.InnerText = cbxRememberAccount.IsChecked.ToString();
        }

        private void cbxRememberPassWord_Unchecked(object sender, RoutedEventArgs e)
        {
            isRememberPassWordChecked = false;
            XmlNode accountCheck = xmldoc.SelectSingleNode("/root/settings/isRememberPassword");
            accountCheck.InnerText = cbxRememberAccount.IsChecked.ToString();
        }
        #endregion

        private void HandleCheckThread(Object stateInfo)
        {

            byte[] bytes;
            while (myClient.Connected)
            {
                if (myClient.Connected && myClient.Available > 0)
                {
                    bytes = new byte[1024];
                    ns.Read(bytes, 0, bytes.Length);
                    string s = Encoding.Default.GetString(bytes).TrimEnd('\0');
                    JObject jobj = JObject.Parse(Encoding.Default.GetString(bytes).TrimEnd('\0'));
                    stateCode = JObject.Parse(Encoding.Default.GetString(bytes).TrimEnd('\0'))["data"]["isAccountRight"].ToString();
                    nickName = JObject.Parse(Encoding.Default.GetString(bytes).TrimEnd('\0'))["data"]["nickname"].ToString();
                    setUpState = JObject.Parse(Encoding.Default.GetString(bytes).TrimEnd('\0'))["type"].ToString();
                    msg = JObject.Parse(Encoding.Default.GetString(bytes).TrimEnd('\0'))["data"]["msg"].ToString();
                    break;
                }
            }
        }

        private delegate void BingoCallback(string msg);
        private delegate void FailCallback(string msg);
        private void BingoCallbackMethod(string msg)
        {
            if (!isSetUp)
            {
                timer.Dispose();
                MessageBox.Show(this, "欢迎回来！" + nickName, "提示");
                MainWindow m = new MainWindow();
                m.NickName = nickName;
                m.Account = tbxAccount.Text;
                Application.Current.MainWindow = m;
                m.Show();
                this.Close();
                btnSetIn.IsEnabled = true;
            }
            else
            {
                timer.Dispose();
                MessageBox.Show(this, $"恭喜你, 注册成功!\n服务器端返回消息：\n{msg}", "提示");
                btnSetIn.IsEnabled = true;
                btnSetUp.IsEnabled = true;
                setUpState = "";

            }
            
        }
        private void FailCallbackMethod(string msg)
        {
            if (!isSetUp)
            {
                timer.Dispose();
                MessageBox.Show(this, "账号或密码错误！", "警告");
                btnSetIn.IsEnabled = true;
            }
            else
            {
                timer.Dispose();
                MessageBox.Show(this, $"很遗憾,注册失败!\n服务器端返回消息：\n{msg}", "提示");
                btnSetIn.IsEnabled = true;
                btnSetUp.IsEnabled = true;
                setUpState = "";
            }
        }

        private void TimerThread(object state)
        {

            if (!isSetUp)
            {
                if (stateCode.Equals("False"))
                {
                    FailCallback f = FailCallbackMethod;
                    Dispatcher.Invoke(f, new object[] { "" });
                }
                else if (stateCode.Equals("True"))
                {

                    BingoCallback callback = BingoCallbackMethod;
                    Dispatcher.Invoke(callback, new object[] { "" });

                }
            }
            else
            {
                if (setUpState.Equals("setupSuccess"))
                {
                    BingoCallback callback = BingoCallbackMethod;
                    Dispatcher.Invoke(callback, new object[] { msg });
                }
                else if (setUpState.Equals("setupFault"))
                {
                    FailCallback f = FailCallbackMethod;
                    Dispatcher.Invoke(f, new object[] { msg });
                }  
            }
                
                

        }

        private TcpClient myClient = null;
        //private Thread thread = null, timer = null;
        private Timer timer;
        private NetworkStream ns = null;
        private string stateCode = "";
        private string nickName = "";
        private string setUpState = "";
        private string msg = "";

        private void btnSetIn_Click(object sender, RoutedEventArgs e)
        {
            Regex regAccount = new Regex(@"^[0-9a-zA-Z_]{4,10}$");
            Regex regPassword = new Regex(@"^[0-9a-zA-Z_$+.]{6,16}$");
            Regex regNick = new Regex(@"^.{1,6}$");
            if (!isSetUp)
            {
                btnSetIn.IsEnabled = false;

                myClient.Close();
                myClient = new TcpClient();

                string strOAccount = tbxAccount.Text.Trim();
                string strOPassword = pbxPassWord.Password.Trim();
                

                //本地验证与服务器验证双重验证
                //本地验证
                if (!regAccount.IsMatch(strOAccount))
                {
                    FailCallbackMethod("");
                    return;
                }

                if (!regPassword.IsMatch((strOPassword)))
                {
                    FailCallbackMethod("");
                    return;
                }
                //连接验证服务器
                if (!myClient.Connected)
                {

                    IPHostEntry ipe = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var item in ipe.AddressList)
                    {
                        if (item.AddressFamily == AddressFamily.InterNetwork)
                        {
                            IPEndPoint ipp = new IPEndPoint(item, 12017);
                            try
                            {
                                myClient.Connect(ipp);
                                ns = myClient.GetStream();
                            }
                            catch (System.Exception)
                            {
                                MessageBox.Show(this, "连接服务器失败!", "警告!");
                                btnSetIn.IsEnabled = true;
                                break;
                            }

                            break;
                        }
                    }
                }
                string jsonTemplateString = "{\"type\":\"\",\"data\":{\"account\":\"\",\"password\":\"\",\"nickname\":\"\"}}";

                JObject jObj = JObject.Parse(jsonTemplateString);
                jObj["type"] = "check";
                jObj["data"]["account"] = strOAccount;
                jObj["data"]["password"] = strOPassword;

                string jsonString = jObj.ToString();
                if (ns != null)
                {
                    ns.Write(Encoding.Default.GetBytes(jsonString), 0, jsonString.Length);
                    ns.Flush();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(HandleCheckThread), null);
                    timer?.Dispose();
                    timer = new Timer(TimerThread, null, 0, 300);


                }


                //--------------骄傲的分割线----------------------------


                if (isRememberAccountChecked)
                {
                    //如果用户想记住账号
                    if (xmldoc != null)
                    {
                        XmlNode account = xmldoc.SelectSingleNode("/root/data/account");
                        account.InnerText = strOAccount;
                        xmldoc.Save("data.xml");
                    }
                }
                else
                {
                    //如果用户不想记住账号，清空已记住的账号
                    if (xmldoc != null)
                    {
                        XmlNode account = xmldoc.SelectSingleNode("/root/data/account");
                        account.InnerText = "";
                        xmldoc.Save("data.xml");
                    }
                }


                if (isRememberPassWordChecked)
                {
                    //如果用户想记住密码
                    if (xmldoc != null)
                    {
                        XmlNode account = xmldoc.SelectSingleNode("/root/data/password");
                        account.InnerText = strOPassword;
                        xmldoc.Save("data.xml");
                    }
                }
                else
                {
                    //如果用户不想记住密码，清空已记住的密码
                    if (xmldoc != null)
                    {
                        XmlNode account = xmldoc.SelectSingleNode("/root/data/password");
                        account.InnerText = "";
                        xmldoc.Save("data.xml");
                    }
                }
            }
            else
            {
                myClient.Close();
                myClient = new TcpClient();

                if (!regAccount.IsMatch(tbxAccount.Text))
                {
                    MessageBox.Show(this, "注册账号格式错误!\n格式应为: 只包含数字、大小写字母和下划线.的4到10位的字符串", "错误");//@"^[0-9a-zA-Z_]{4,10}$"
                    return;
                }

                if (!regPassword.IsMatch((pbxPassWord.Password)))
                {
                    MessageBox.Show(this, "注册账号的密码格式错误!\n格式应为: 只包含数字、大小写字母、下划线、$、+和.的6到16位字符串", "错误");//@"^[0-9a-zA-Z_$+.]{6,16}$"
                    return;
                }
                if (!regNick.IsMatch(tbxNick.Text))
                {
                    MessageBox.Show(this, "注册账号的昵称格式错误!\n格式应为: 包含任意字符的1到6位字符串", "错误");//@"^.{1,6}$"
                    return;
                }
                MessageBoxResult mr = MessageBox.Show(this, $"账号:  {tbxAccount.Text}\n密码:  { pbxPassWord.Password}\n昵称:  {tbxNick.Text}\n确认注册吗?", "请确认你的账号信息是否填错", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.Yes)
                {
                    string jsonTemplateString = "{\"type\":\"\",\"data\":{\"account\":\"\",\"password\":\"\",\"nickname\":\"\"}}";

                    JObject jObj = JObject.Parse(jsonTemplateString);
                    jObj["type"] = "setup";
                    jObj["data"]["account"] = tbxAccount.Text;
                    jObj["data"]["password"] = pbxPassWord.Password;
                    jObj["data"]["nickname"] = tbxNick.Text;
                    string form = jObj.ToString();//注册字符串

                    if (!myClient.Connected)
                    {

                        IPHostEntry ipe = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (var item in ipe.AddressList)
                        {
                            if (item.AddressFamily == AddressFamily.InterNetwork)
                            {
                                IPEndPoint ipp = new IPEndPoint(item, 12017);
                                try
                                {
                                    myClient.Connect(ipp);
                                    ns = myClient.GetStream();
                                }
                                catch (System.Exception)
                                {
                                    MessageBox.Show(this, "连接服务器失败!", "警告!");
                                    btnSetIn.IsEnabled = true;
                                    break;
                                }

                                break;
                            }
                        }
                    }
                    if (ns != null)
                    {
                        ns.Write(Encoding.Default.GetBytes(form), 0, form.Length);
                        ns.Flush();

                        ThreadPool.QueueUserWorkItem(new WaitCallback(HandleCheckThread), null);
                        timer?.Dispose();
                        timer = new Timer(TimerThread, null, 0, 300);


                    }

                    btnSetIn.IsEnabled = false;
                    btnSetUp.IsEnabled = false;


                    return;
                }
                else
                {
                    return;
                }
            }
        }
        private bool isSetUp = false;
        private void btnSetUp_Click(object sender, RoutedEventArgs e)
        {
            if (!isSetUp)
            {
                tbxAccount.Text = "";
                pbxPassWord.Password = "";
                cbxRememberAccount.Visibility = Visibility.Collapsed;
                cbxRememberPassWord.Visibility = Visibility.Collapsed;
                lblNick.Visibility = Visibility.Visible;
                tbxNick.Visibility = Visibility.Visible;
                btnSetUp.Content = "返回";
                btnSetIn.Content = "注册";
                isSetUp = true;
            }
            else
            {
                if (isRememberAccountChecked)
                {
                    tbxAccount.Text = account.InnerText;
                }
                else
                {
                    tbxAccount.Text = "";
                }
                if (isRememberPassWordChecked)
                {
                    pbxPassWord.Password = password.InnerText;
                }
                else
                {
                    pbxPassWord.Password = "";
                }
                tbxNick.Text = "";
                cbxRememberAccount.Visibility = Visibility.Visible;
                cbxRememberPassWord.Visibility = Visibility.Visible;
                lblNick.Visibility = Visibility.Collapsed;
                tbxNick.Visibility = Visibility.Collapsed;
                btnSetUp.Content = "注册";
                btnSetIn.Content = "登录";
                isSetUp = false;
            }
            
        }
    }
}
