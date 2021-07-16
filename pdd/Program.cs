using HttpListenerPost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WechatRegster;
using WechatRegster.listenes;

namespace pdd
{
    static class Program
    {
        public static Dictionary<int, HttpListenerHandler> handlerMap = new Dictionary<int, HttpListenerHandler>();
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
            Logger.getLogger().close(true);
            if(File.Exists(Environment.CurrentDirectory + "\\MySQL5.7.26") && ini.Read("mysqld", "enter", "0",Environment.CurrentDirectory + "\\MySQL5.7.26\\my.ini").Equals("0"))
            {
                ini.Write("mysqld", "basedir", Environment.CurrentDirectory + "\\", Environment.CurrentDirectory + "\\MySQL5.7.26\\my.ini");
                ini.Write("mysqld", "datadir", Environment.CurrentDirectory + "\\" + "data\\", Environment.CurrentDirectory + "\\MySQL5.7.26\\my.ini");
                ini.Write("mysqld", "enter", "1", Environment.CurrentDirectory + "\\MySQL5.7.26\\my.ini");
            }
            //Util.startmysql();
            if ("笔记本".Equals(Environment.UserName))
                LinkService.dbConn = new DBConnection("Data Source=127.0.0.1;Database=pdd;User ID=root;Password=smartbi;Charset=utf8; Pooling=true;Allow User Variables=True");
            else
                LinkService.dbConn = new DBConnection("Data Source=127.0.0.1;Database=pdd;User ID=root;Password=root;Charset=utf8; Pooling=true;Allow User Variables=True");
            HttpListenerHandler handler = handlerMap.ContainsKey(Service.httpListenerLinkPort) ? handlerMap[Service.httpListenerLinkPort] : null;

            if (handler == null)
            {
                handler = new HttpListenerHandler(Service.httpListenerLinkPort);
                handlerMap.Add(Service.httpListenerLinkPort, handler);
            }
            handler.startListener(Service.httpListenerLinkPort);
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
    class caiji
    {
        public caiji(string content)
        {
            this.content = content;
        }
        public string content;
        public int caijisizeindex;//采集数
        public int caijinewgoodsindex;
    }

}
