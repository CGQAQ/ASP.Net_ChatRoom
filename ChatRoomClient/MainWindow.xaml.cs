using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace ChatRoomClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string NickName { get; set; }
        public string Account { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        public class MsgFormat
        {
            public enum MsgType
            {
                MyMsg,
                OtherMsg,
                SystemMsg
            }
            public MsgFormat(MsgType type, string str, string nickname = null)
            {
                this.Type = type;
                if (type == MsgType.MyMsg)
                {
                    LeftMsg = "";
                    RightMsg = str;
                    SysMsg = "";
                }
                else if (type == MsgType.OtherMsg)
                {
                    LeftMsg = str;
                    this.NickName = nickname+":  ";
                    RightMsg = "";
                    SysMsg = "";
                }
                else
                {
                    LeftMsg = "";
                    RightMsg = "";
                    SysMsg = str;
                }
            }
            public string LeftMsg {
                get;
                set;
            }
            public string NickName{
                get;
                set;
            }
            public string RightMsg {
                get;
                set;
            }
            public string SysMsg {
                get;
                set;
            }
            public MsgType Type {
                set;
                get;
            }
        }

        private ObservableCollection<MsgFormat> msgs;
        private string strJsonTemplate = "{\"type\":\"\",\"data\":{\"nickname\":\"\",\"account\":\"\",\"isAccountRight\":\"\",\"msg\":\"\"}}";
        private TcpClient client;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = NickName;
            msgs = new ObservableCollection<MsgFormat>();
            lbxMsgs.ItemsSource = msgs;
            timer = new Timer(TimerCallback, null, 1000, 500);
            

            client = new TcpClient();
            try
            {
                IPHostEntry iphe = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var item in iphe.AddressList)
                {
                    if (item.AddressFamily == AddressFamily.InterNetwork)
                    {
                        client.Connect(item, 12016);
                        break;
                    }
                }
            }
            catch (System.Exception )
            {
                MessageBox.Show(this, "连接聊天服务器失败,请重启本软件!", "未知错误");
            }
            if (client.Connected)
            {
                ThreadPool.QueueUserWorkItem(receiveMsgThread);
            }
            else 
            {
                MessageBox.Show(this, "连接聊天服务器失败,请重启本软件!", "未知错误");
            }
        }

        private delegate void ReceiveMsgDelegate();
        private JObject jReceiveMsg;
        private Timer timer;
        private void ReceiveMsgDelegateMethod()
        {
            if (jReceiveMsg != null)
            {
                msgs.Add(new MsgFormat(MsgFormat.MsgType.OtherMsg, jReceiveMsg["data"]["msg"].ToString(), jReceiveMsg["data"]["nickname"].ToString()));
            }           
        }
        private void receiveMsgThread(object o)
        {
            while (client.Connected)
            {
                NetworkStream ns = client.GetStream();
                if (ns!=null&&client.Available>0)
                {
                    byte[] bytes = new byte[1024];
                    ns.Read(bytes, 0, bytes.Length);
                    jReceiveMsg = JObject.Parse(Encoding.Default.GetString(bytes).TrimEnd('\0'));
                    if (jReceiveMsg["type"].ToString().Equals("msg"))
                    {
                        ReceiveMsgDelegate rmd = ReceiveMsgDelegateMethod;
                        Dispatcher.Invoke(rmd);
                    }
                    //stateCode = JObject.Parse(Encoding.Default.GetString(bytes).TrimEnd('\0'))["data"]["isAccountRight"].ToString();
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

        private void TimerCalledMethod()
        {
            MessageBox.Show(this, "连接错误, 按确定关闭软件", "错误");
            this.Close();
        }

        private void TimerCallback(object o)
        {
            if (!IsOnline(client))
            {
                Dispatcher.Invoke(TimerCalledMethod);

            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (!IsOnline(client))
            {
                TimerCalledMethod();
            }
            else {
                string msg = tbxMsg.Text.Trim();
                if (msg.Length > 0)
                {
                    msgs.Add(new MsgFormat(MsgFormat.MsgType.MyMsg, msg));
                    JObject jObj = JObject.Parse(strJsonTemplate);
                    jObj["type"] = "msg";
                    jObj["data"]["msg"] = msg;
                    jObj["data"]["account"] = Account;
                    if (client.Connected)
                    {
                        NetworkStream ns = client.GetStream();
                        ns.Write(Encoding.Default.GetBytes(jObj.ToString()), 0, Encoding.Default.GetBytes(jObj.ToString()).Length);
                        ns.Flush();
                    }
                    tbxMsg.Text = "";
                }
                tbxMsg.Text = "";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            client?.Close();
            timer?.Dispose();
        }
    }
}
