using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WechatRegster.listenes;

namespace pdd
{
    static class Program
    {
        // <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new Form1());

            glExitApp = true;
        }
        static bool glExitApp = false;
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Service.put("CurrentDomain_UnhandledException: " + e.IsTerminating.ToString() + "    " + e.ExceptionObject, false);
            //LoginCheck.sendMsgEmail("link应用奔溃", "CurrentDomain_UnhandledException: " + e.IsTerminating.ToString() + "    " + e.ExceptionObject.ToString());
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Service.put("Application_ThreadException: " + e.Exception.StackTrace + "    " + e.Exception.Message, false);
            //LoginCheck.sendMsgEmail("link应用奔溃", "WechatRegster Application_ThreadException: " + e.Exception.StackTrace + "    " + e.Exception.Message);
        }
    }
    public class goods
    {
        public string id;
        public string goods_id;
        public string goods_name;
        public string image_url;
        public string goods_url;
        public double price;
        public int sales;
        public int create_time;
        public int details_status;
        public int details_time;
        public string pd;
        public string pl;
        public int pls;
        public int index;
        public int check;
        public int newgoods;
        public int num;
    }


}
