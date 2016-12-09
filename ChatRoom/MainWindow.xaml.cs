using System.Windows;
using System.Net.Sockets;
using System.Net;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Media;
using System;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json.Linq;


namespace ChatRoom
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            dicClients = new Dictionary<string, TcpClient>();
            dicIPInfoToNickMap = new Dictionary<string, string>();
            setedInAccounts = new List<string>();

            updateListBoxDelegate = new UpdateListBoxDelegate(UpdateListBoxDelegateMethod);
            Logs = new ObservableCollection<string>();
            lbxLogs.ItemsSource = Logs;


            IPHostEntry ipe = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var item in ipe.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPEndPoint ipp = new IPEndPoint(item, 12016);
                    tcpServer = new TcpListener(ipp);
                    tcpAccountCheckOrSetUpServer = new TcpListener(item, 12017);
                    break;
                }
            }


        }
        private ObservableCollection<string> Logs;


        private TcpListener tcpServer;//聊天端口
        private TcpListener tcpAccountCheckOrSetUpServer;//账号密码检测和账号注册公用端口
        private TcpClient currentClient;
        private Dictionary<string, TcpClient> dicClients;
        private Dictionary<string, string> dicIPInfoToNickMap;
        private List<string> setedInAccounts;//已登录账号列表, 防止同一账号登录多次
        string currentAccount;
        private bool state = false;//服务器当前状态
        private string strJsonTemplate = "{\"type\":\"\",\"data\":{\"nickname\":\"\",\"isAccountRight\":\"\",\"msg\":\"\"}}";

        private delegate void UpdateListBoxDelegate(Dictionary<string, TcpClient> d);
        private UpdateListBoxDelegate updateListBoxDelegate;




        private void ServerListenerThread(object o)
        {
            try
            {

                while (state)
                {
                    if ((currentClient = tcpServer.AcceptTcpClient()) != null)
                    {
                        dicClients.Add(currentClient.Client.RemoteEndPoint.ToString(), currentClient);

                        lbxClients.Dispatcher.BeginInvoke(updateListBoxDelegate, new object[] { dicClients });
                        //new Thread(MsgListenerThread).Start(currentClient);
                        ThreadPool.QueueUserWorkItem(MsgListenerThread, currentClient);
                    }
                }

            }
            catch (Exception)
            {

                return;
            }

        }

        private void AccountCheckOrSetUpServerListenerThread(object o)
        {
            try
            {
                while (state)
                {
                    if ((currentClient = tcpAccountCheckOrSetUpServer.AcceptTcpClient()) != null)
                    {
                        ThreadPool.QueueUserWorkItem(HandleAccountCheckOrSetUpServerThread, currentClient);
                    }
                }
            }
            catch (Exception)
            {

                return;
            }

        }
        private string account;
        private void HandleAccountCheckOrSetUpServerThread(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            NetworkStream ns = client.GetStream();
            while (client.Connected)
            {
                if (client.Connected && client.Available > 0)
                {
                    byte[] bytes = new byte[1024];
                    ns.Read(bytes, 0, bytes.Length);
                    string jsonString = Encoding.Default.GetString(bytes).TrimEnd('\0');
                    JObject jObj = JObject.Parse(jsonString);


                    if (jObj["type"].ToString() == "check")
                    {
                        //检测账号区
                        account = jObj["data"]["account"].ToString();
                        string password = jObj["data"]["password"].ToString();


                        JObject jBack = JObject.Parse(strJsonTemplate);
                        jBack["type"] = "checkBack";



                        if (DBTools.IsAccountInfoRight(account, password) && !setedInAccounts.Contains(account))
                        {
                            jBack["data"]["isAccountRight"] = "True";
                            jBack["data"]["nickname"] = DBTools.GetNickNameByAccount(account);
                            ns.Write(Encoding.Default.GetBytes(jBack.ToString()), 0, jBack.ToString().Length);
                            ns.Close();
                            client.Close();
                            currentAccount = account;
                            setedInAccounts.Add(account);

                        }
                        else
                        {
                            jBack["data"]["isAccountRight"] = "False";
                            ns.Write(Encoding.Default.GetBytes(jBack.ToString()), 0, jBack.ToString().Length);
                            ns.Close();
                            client.Close();
                        }
                    }
                    else if (jObj["type"].ToString().Equals("setup"))
                    {
                        //注册账号区
                        //{\"type\":\"\",\"data\":{\"nickname\":\"\",\"isAccountRight\":\"\",\"msg\":\"\"}}"
                        string setUpAccount = jObj["data"]["account"].ToString();
                        string setUpPassword = jObj["data"]["password"].ToString();
                        string setUpNick = jObj["data"]["nickname"].ToString();

                        JObject jBack = JObject.Parse(strJsonTemplate);

                        if (DBTools.isAccountExist(setUpAccount))
                        {
                            jBack["type"] = "setupFault";
                            jBack["data"]["msg"] = "您要注册的账号已存在, 换个试试吧";
                            ns.Write(Encoding.Default.GetBytes(jBack.ToString()), 0, Encoding.Default.GetBytes(jBack.ToString()).Length);
                            ns.Close();
                            client.Close();
                            return;
                        }
                        else if (DBTools.SetUp(setUpNick, setUpAccount, setUpPassword))
                        {
                            jBack["type"] = "setupSuccess";//setupSuccess
                            jBack["data"]["msg"] = "注册成功, 赶快去登陆吧!";
                            ns.Write(Encoding.Default.GetBytes(jBack.ToString()), 0, Encoding.Default.GetBytes(jBack.ToString()).Length);
                            ns.Close();
                            client.Close();
                            

                            return;
                        }
                        else
                        {

                        }
                    }
                }
            }
        }

        private void MsgListenerThread(object o)//消息监听线程
        {

            TcpClient tcpClient = (TcpClient)o;
            string key = tcpClient.Client.RemoteEndPoint.ToString();
            NetworkStream ns = tcpClient.GetStream();
            bool tag = true;


            byte[] bytes;
            while (tcpClient.Connected)
            {
                if (tag)
                {

                    loginDelegate = Login;
                    Dispatcher.Invoke(loginDelegate, new object[] { tcpClient });

                    tag = false;
                }

                if (tcpClient.Connected && tcpClient.Available > 0)
                {
                    bytes = new byte[1024];
                    ns.Read(bytes, 0, bytes.Length);
                    JObject jobject = JObject.Parse(Encoding.Default.GetString(bytes));
                    jobject["data"]["nickname"] = DBTools.GetNickNameByAccount(jobject["data"]["account"].ToString());

                    foreach (var item in dicClients.Keys)
                    {
                        if (item.Equals(tcpClient.Client.RemoteEndPoint.ToString()))
                        {
                            continue;
                        }
                        dicClients[item].GetStream().Write(Encoding.Default.GetBytes(jobject.ToString()), 0, Encoding.Default.GetBytes(jobject.ToString()).Length);
                    }
                }

            }
            dicClients.Remove(key);
            setedInAccounts.Remove(currentAccount);
        }
        private delegate void LoginDelegate(TcpClient client);
        private LoginDelegate loginDelegate;
        private void Login(TcpClient client)
        {
            if (client != null)
            {
                dicIPInfoToNickMap.Add(client.Client.RemoteEndPoint.ToString(), DBTools.GetNickNameByAccount(account));
                Logs.Add(DateTime.Now.ToShortTimeString() + ": " + DBTools.GetNickNameByAccount(account) + " 已登录");
                if (Logs.Count > 0)
                {
                    lbxLogs.ScrollIntoView(lbxLogs.Items[lbxLogs.Items.Count - 1]);
                }
            }

        }
        private delegate void LogoutDelegate(TcpClient client);
        private LogoutDelegate logoutDelegate;
        private void Logout(TcpClient client)
        {
            if (client != null)
            {
                //dicAccountInfoToNickMap.Add(key, jobject["data"]["nickname"].ToString());
                Logs.Add(DateTime.Now.ToShortTimeString() + ": " + dicIPInfoToNickMap[client.Client.RemoteEndPoint.ToString()] + " 已下线");
                dicIPInfoToNickMap.Remove(client.Client.RemoteEndPoint.ToString());
                if (Logs.Count > 0)
                {
                    lbxLogs.ScrollIntoView(lbxLogs.Items[lbxLogs.Items.Count - 1]);
                }
            }

        }
        private static bool IsOnline(TcpClient c)
        {
            if (c == null)
            {
                return false;
            }
            return !((c.Client.Poll(1000, SelectMode.SelectRead) && (c.Client.Available == 0)) || !c.Client.Connected);
        }
        private void ClientsStateListener(object o)
        {
            if (dicClients.Count > 0)
            {
                try
                {
                    foreach (var item in dicClients.Keys)
                    {
                        TcpClient t = dicClients[item];
                        if (!IsOnline(t))
                        {
                            logoutDelegate = Logout;
                            Dispatcher.Invoke(logoutDelegate, new object[] { dicClients[item] });
                            dicClients[item].Close();
                            dicClients.Remove(item);
                            Dispatcher.Invoke(updateListBoxDelegate, new object[] { dicClients });
                        }
                    }
                }
                catch (System.Exception)
                {
                    return;
                }

            }

        }

        private void UpdateListBoxDelegateMethod(Dictionary<string, TcpClient> d)
        {
            lbxClients.ItemsSource = null;
            lbxClients.ItemsSource = d.Keys;
            if (dicClients.Count > 0)
            {
                lbxClients.ScrollIntoView(lbxClients.Items[lbxClients.Items.Count - 1]);
            }


        }

        private Timer ClientsStateListenerTimer;
        private void btnOpenOrClose_Click(object sender, RoutedEventArgs e)
        {
            if (!state)
            {
                state = true;
                tcpServer.Start();
                tcpAccountCheckOrSetUpServer.Start();
                ThreadPool.QueueUserWorkItem(ServerListenerThread);//处理聊天端口监听线程
                ThreadPool.QueueUserWorkItem(AccountCheckOrSetUpServerListenerThread);
                ClientsStateListenerTimer?.Dispose();
                ClientsStateListenerTimer = new Timer(ClientsStateListener, null, 0, 500);


                btnOpenOrClose.Content = "关闭服务器";
                rectState.Fill = new SolidColorBrush(Colors.Green);
                Logs.Add(DateTime.Now.ToShortTimeString() + "：服务器已启动！");
                if (Logs.Count > 0)
                {
                    lbxLogs.ScrollIntoView(lbxLogs.Items[lbxLogs.Items.Count - 1]);
                }
            }
            else
            {
                List<string> keys = new List<string>();
                foreach (var item in dicClients.Keys)
                {
                    keys.Add(item);
                }
                foreach (var item in keys)
                {
                    dicClients[item].Close();
                }
                keys = null;

                state = false;
                lbxClients.ItemsSource = null;
                lbxClients.ItemsSource = dicClients;
                ClientsStateListenerTimer?.Dispose();
                tcpServer.Stop();
                tcpAccountCheckOrSetUpServer.Stop();

                btnOpenOrClose.Content = "开启服务器";
                rectState.Fill = new SolidColorBrush(Colors.Red);

                Logs.Add(DateTime.Now.ToShortTimeString() + "：服务器已关闭，所有连接断开！");
                if (Logs.Count > 0)
                {
                    lbxLogs.ScrollIntoView(lbxLogs.Items[lbxLogs.Items.Count - 1]);
                }
            }
        }

    }
}
