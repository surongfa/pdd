using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WechatRegster;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web;
using WechatRegster.listenes;
using pdd;

namespace HttpListenerPost
{
    /// <summary>
    /// HttpListenner监听Post请求参数值实体
    /// </summary>
    public class HttpListenerValue
    {
        /// <summary>
        /// 0=> 参数
        /// 1=> 文件
        /// </summary>
        public int type = 0;
        public string name;
        public byte[] datas;
        public string value;
    }

    /// <summary>
    /// 获取Post请求中的参数和值帮助类
    /// </summary>
    public class HttpListenerPostParaHelper
    {
        private static bool CompareBytes(byte[] source, byte[] comparison)
        {
            try
            {
                int count = source.Length;
                if (source.Length != comparison.Length)
                    return false;
                for (int i = 0; i < count; i++)
                    if (source[i] != comparison[i])
                        return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static byte[] ReadLineAsBytes(Stream SourceStream)
        {
            var resultStream = new MemoryStream();
            while (true)
            {
                int data = SourceStream.ReadByte();
                resultStream.WriteByte((byte)data);
                if (data == 10)
                    break;
            }
            resultStream.Position = 0;
            byte[] dataBytes = new byte[resultStream.Length];
            resultStream.Read(dataBytes, 0, dataBytes.Length);
            return dataBytes;
        }

        /// <summary>
        /// 获取Post过来的参数和数据
        /// </summary>
        /// <returns></returns>
        public static List<HttpListenerValue> GetHttpListenerValue(HttpListenerContext request)
        {
            List<HttpListenerValue> HttpListenerValueList = new List<HttpListenerValue>();
            try
            {
                Stream SourceStream = request.Request.InputStream;
                if (!string.IsNullOrWhiteSpace(request.Request.ContentType) &&  request.Request.ContentType.Length > 20 && string.Compare(request.Request.ContentType.Substring(0, 20), "multipart/form-data;", true) == 0)
                {
                    string[] HttpListenerValue = request.Request.ContentType.Split(';').Skip(1).ToArray();
                    string boundary = string.Join(";", HttpListenerValue).Replace("boundary=", "").Trim();
                    byte[] ChunkBoundary = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
                    byte[] EndBoundary = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");
                    byte[] rn = Encoding.UTF8.GetBytes("\r\n");
                    var resultStream = new MemoryStream();
                    bool CanMoveNext = true;
                    HttpListenerValue data = null;
                    bool hasDisposition = false;
                    while (CanMoveNext)
                    {
                        byte[] currentChunk = ReadLineAsBytes(SourceStream);
                        Console.WriteLine(Encoding.UTF8.GetString(currentChunk));
                        if (!Encoding.UTF8.GetString(currentChunk).Equals("\r\n"))
                            resultStream.Write(currentChunk, 0, currentChunk.Length);
                        if (CompareBytes(ChunkBoundary, currentChunk))
                        {
                            byte[] result = new byte[resultStream.Length - ChunkBoundary.Length];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            CanMoveNext = true;
                            if (result.Length > 0)
                                data.datas = result;
                            data = new HttpListenerValue();
                            HttpListenerValueList.Add(data);
                            resultStream.Dispose();
                            resultStream = new MemoryStream();

                        }
                        else if (Encoding.UTF8.GetString(currentChunk).Contains("Content-Disposition"))
                        {
                            hasDisposition = true;
                            byte[] result = new byte[resultStream.Length - 2];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            CanMoveNext = true;
                            data.name = Encoding.UTF8.GetString(result).Replace("Content-Disposition: form-data; name=\"", "").Replace("\"", "").Split(';')[0];
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (Encoding.UTF8.GetString(currentChunk).Contains("Content-Type"))
                        {
                            CanMoveNext = true;
                            data.type = 1;
                            resultStream.Dispose();
                            resultStream = new MemoryStream();
                        }
                        else if (CompareBytes(EndBoundary, currentChunk))
                        {
                            byte[] result = new byte[resultStream.Length - EndBoundary.Length - 2];
                            resultStream.Position = 0;
                            resultStream.Read(result, 0, result.Length);
                            data.datas = result;
                            resultStream.Dispose();
                            CanMoveNext = false;
                        }
                        else if (!CompareBytes(currentChunk, rn) && hasDisposition)
                        {
                            byte[] result = currentChunk.Skip(0).Take(currentChunk.Length - 2).ToArray();
                            data.value = Encoding.UTF8.GetString(result);

                        }
                    }
                }
                else 
                { // 请求数据流既是全部,如触控精灵的post
                    string strLength = request.Request.Headers.GetValues("Content-Length")[0];
                    byte[] buffer1 = new byte[Convert.ToInt64(strLength)];
                    //读取用户发送过来的正文
                    int jsonLength = SourceStream.Read(buffer1, 0, buffer1.Length);

                    if (jsonLength > 0)
                    {
                        string reqData = Encoding.UTF8.GetString(buffer1, 0, jsonLength);
                        if (!string.IsNullOrEmpty(reqData))
                        {
                            if (reqData.StartsWith("{") && reqData.EndsWith("}"))
                            {
                                JObject jobj = (JObject)JsonConvert.DeserializeObject(reqData);
                                foreach (var item in jobj)
                                {
                                    HttpListenerValue value = new HttpListenerValue();
                                    value.name = HttpUtility.UrlDecode(item.Key);
                                    if (item.Value != null)
                                        value.value = HttpUtility.UrlDecode(item.Value.ToString());
                                    HttpListenerValueList.Add(value);
                                }
                            }
                            else if (reqData.StartsWith("[") && reqData.EndsWith("]"))
                            {
                                
                                HttpListenerValue value = new HttpListenerValue();
                                value.name = "JArray";
                                value.value = reqData;

                                HttpListenerValueList.Add(value);
                            }
                            else // 数据为url格式
                            {
                                ParseUrl(reqData, HttpListenerValueList);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Service.put("DIDX转发的短信数据格式不符,请查找原因,url:" + request.Request.RawUrl + "," + e.StackTrace + e.Message, true);
                return null;
            }
            return HttpListenerValueList;
        }

        private static void ParseUrl(string url, List<HttpListenerValue> HttpListenerValueList)
        {
            if (string.IsNullOrEmpty(url))
                return;

            int questionMarkIndex = url.IndexOf('?');
        
            if (questionMarkIndex == url.Length - 1)
                return;
            string ps = url.Substring(questionMarkIndex + 1);

            // 开始分析参数对  
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);

            foreach (Match m in mc)
            {
                HttpListenerValue value = new HttpListenerValue();
                value.name = HttpUtility.UrlDecode(m.Result("$2"));
                value.value = HttpUtility.UrlDecode(m.Result("$3"));
                HttpListenerValueList.Add(value);
            }
        }
    }
    

    public class HttpListenerHandler
    {
        private HttpListener httpPostRequest = new HttpListener();

        private static object Singleton_Lock = new object();

        //private HttpListenerHandler()
        //{
        //}

        //public static HttpListenerHandler getInstance()
        //{
        //    if (handler == null)
        //    {
        //        lock (Singleton_Lock)
        //        {
        //            if (handler == null)
        //            {
        //                handler = new HttpListenerHandler();
        //            }
        //        }
        //    }
        //    return handler;
        //}

        public HttpListener getHttpPostRequest()
        {
            return httpPostRequest;
        }

        public void startListener(int port)
        {
            if (!httpPostRequest.IsListening)
            {
                httpPostRequest.Start();
            }
            if (httpPostRequest.IsListening)
            {
                if(port == Service.httpListenerLinkPort)
                {
                    Thread threadHttpAuxiliaryRequest = new Thread(new ThreadStart(LinkService.getInstance(this).httpPostRequestHandle));
                    threadHttpAuxiliaryRequest.Start();
                }
               
            }
        }

        public void closeListener()
        {
            httpPostRequest.Stop();
            httpPostRequest.Close();
        }

        public HttpListenerHandler(string url)
        {
            httpPostRequest.Prefixes.Add(url);
        }

        public HttpListenerHandler(int port)
        {
            httpPostRequest.Prefixes.Add("http://+:"+port+"/");
        }

        public void stoptListener(int port)
        {
            if (port == Service.httpListenerLinkPort)
                LinkService.getInstance(null).stoptListener();
            
        }

    }
}