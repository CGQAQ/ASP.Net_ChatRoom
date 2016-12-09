using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatRoomClient
{
    public class MyListboxTemplateSelector
        : DataTemplateSelector

    {
        //覆盖SelectTemplate函数
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Window win = Application.Current.MainWindow;
            //当前显示的是我的消息
            if (item != null && ((MainWindow.MsgFormat)item).Type == MainWindow.MsgFormat.MsgType.MyMsg)
            {
                return win.FindResource("MyMsg") as DataTemplate;
            }
            //当前显示的是他人消息
            else if (item != null && ((MainWindow.MsgFormat)item).Type == MainWindow.MsgFormat.MsgType.OtherMsg)
            {
                return win.FindResource("OtherMsg") as DataTemplate;
            }
            //当前显示的是系统消息
            else if (item != null && ((MainWindow.MsgFormat)item).Type == MainWindow.MsgFormat.MsgType.SystemMsg)
            {
                return win.FindResource("SysMsg") as DataTemplate;
            }
            return null;
        }
    }
}
