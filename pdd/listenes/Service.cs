using pdd;
using System;
using System.Windows.Forms;

/// <summary>
/// 滑块地址的中转服务,用于与机器滑块对接
/// </summary>
namespace WechatRegster.listenes
{
    public class Service
    {
        //public const int httpListenerAuxiliaryPort = 58588;
        public static int httpListenerLinkPort = 8080;
        public static Form1 form1;
        public static bool debugModel = false;
        public static int version = 1;

        public virtual void readLineEnd(Array array = null)
        {

        }

        public virtual void monitorCallBack(Array array = null)
        {

        }

        public static void put(string message, Boolean isClient = true)
        {
            if (isClient)
            {
                Logger.getLogger().appendText(message);
                if (isClient)
                {
                    form1.BeginInvoke(new MethodInvoker(() =>
                    {
                        form1.textBox_log.AppendText(message + "\r\n");
                    }));
                }
            }
           
        }

    }
}
