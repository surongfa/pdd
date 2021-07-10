using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WechatRegster
{
    class Logger
    {
        private static object Singleton_Lock = new object();
        private static Logger logger;
        public static Logger getLogger()
        {
            if (logger == null)
            {
                lock (Singleton_Lock)
                {
                    if (logger == null)
                    {
                        logger = new Logger();
                    }
                }
            }
            return logger;
        }
        //private string filePath = "/logger/";
        private FileStream fs;
        private StreamWriter sw;
        public void init()
        {
            try
            {
                lock (logger)
                {
                    string path = Environment.CurrentDirectory + "\\logger\\";
                    if (!Directory.Exists(path)) // 创建父目录
                    {
                        Directory.CreateDirectory(path);
                    }
                    path += DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".log";
                    if (!File.Exists(path))
                    //验证文件是否存在，有则追加，无则创建
                    {
                        FileStream fs1 = File.Create(path);
                        fs1.Close();
                    }
                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);
                }
            }
            catch (Exception e)
            {
                
            }

            // sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + "：" + strLog);

        }

        public void close(bool reset)
        {
            try
            {
                lock (logger)
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                    if (reset)
                    {
                        init();
                    }
                }
            }
            catch (Exception e)
            {
                
            }
        }


        public void appendText(string message)
        {
            try
            {
                lock (logger)
                {
                    if (fs != null && fs.Length > 2500000)
                    {
                        sw.Close();
                        sw = null;
                        fs.Close();
                        fs = null;
                        init();
                    }
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff") + "：" + message);
                    sw.Flush();
                }
            }
            catch (Exception e)
            {
                
            }
            
        }
    }
}
