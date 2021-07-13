using HttpListenerPost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pdd;
using SufeiUtil;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

/// <summary>
/// 滑块地址的中转服务,用于与机器滑块对接
/// </summary>
namespace WechatRegster.listenes
{

    class LinkService : Service
    {
        public static string path = Environment.CurrentDirectory + "\\img\\";
        public static DBConnection dbConn = null;// new DBConnection("Data Source=127.0.0.1;Database=pdd;User ID=root;Password=smartbi;Charset=utf8; Pooling=true;Allow User Variables=True");
        private static object Singleton_Lock = new object();
        private static LinkService service;
        private static HttpListenerHandler httpListenerHandler = null;
        private bool isStart = false;
        public static int min = 0;
        public static int max = 100000;
        public static int plmin = -1;
        public static int plmax = -1;
        public static int sleep = 500;
        public static int size = 20;
        public static int ladderDiscount = 1;
        public static int pricesize = 200000;
        public static string verifyCode = null;
        public static string captchaId = null;
        public static bool autover = true;
        public static bool autokucun = true;
        public static int payorder = 0; // 支付处理
        public static LinkService getInstance(HttpListenerHandler httpListenerHandler = null)
        {
            if (service == null)
            {
                lock (Singleton_Lock)
                {
                    if (service == null)
                    {
                        service = new LinkService();
                    }
                }
            }
            if (LinkService.httpListenerHandler == null)
                LinkService.httpListenerHandler = httpListenerHandler;
            return service;
        }

        public void stoptListener(bool end = false)
        {
            isStart = false;
            if (httpListenerHandler != null)
            {
                httpListenerHandler.closeListener();
            }
        }
        public void httpPostRequestHandle()
        {
            HttpListenerContext requestContext = null;
            isStart = true;
            try
            {
                while (isStart)
                {
                    try
                    {
                        requestContext = httpListenerHandler.getHttpPostRequest().GetContext();
                    }
                    catch (Exception e)
                    {
                        if (isStart)
                            Service.put("License服务出现异常, 监听已经停止,请查找原因:" + e.StackTrace + e.Message, true);
                        else
                            Service.put("License服务已经停止", true);
                        break;
                    }
                    requestContext.Request.Headers.Add("currentTime", DateTime.Now.ToString());
                    ThreadPool.QueueUserWorkItem(new WaitCallback(delegate (object requestcontext)
                    {
                        string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { ret = false, msg = "未知请求" });
                        HttpListenerContext request = (HttpListenerContext)requestcontext;
                        string rawUrl = request.Request.RawUrl;
                        //Service.put("请求url:" + request.Request.RawUrl, true);
                        goods goods = new goods(); JArray jarray = null; string data = null, cookie = null;
                        string step = "开始";
                        try
                        {
                            if (rawUrl.StartsWith("/goods/add") || rawUrl.StartsWith("/goods/get_details") || rawUrl.StartsWith("/goods/set_details")
                            || rawUrl.StartsWith("/mille")) // 授权验证 || rawUrl.StartsWith("/goods/stock") 
                            {
                                bool isPost = false;
                                List<HttpListenerValue> lst = null;
                                if (request.Request.HttpMethod.ToUpper() == "GET")
                                {
                                    lst = new List<HttpListenerValue>();
                                    NameValueCollection collection = request.Request.QueryString;
                                    for (int i = 0; i < collection.Count; i++)
                                    {
                                        HttpListenerValue value = new HttpListenerValue();
                                        value.name = collection.GetKey(i);
                                        value.value = collection.Get(i);
                                        lst.Add(value);
                                    }
                                }
                                else
                                {
                                    isPost = true;
                                    try
                                    {
                                        lst = HttpListenerPostParaHelper.GetHttpListenerValue(request);
                                    }
                                    catch (Exception e)
                                    {
                                        result = Newtonsoft.Json.JsonConvert.SerializeObject(new { ret = false, msg = "请求失败,数据格式可能存在问题" });
                                        Service.put("数据格式不符,请查找原因,url:" + request.Request.RawUrl + "," + e.StackTrace + e.Message, true);
                                        lst = null;
                                    }
                                }

                                if (lst != null)
                                {


                                    //使用方法
                                    foreach (var key in lst)
                                    {
                                        if (key.type == 0)
                                        {
                                            string value = key.value;
                                            switch (key.name)
                                            {
                                                case "JArray":
                                                    Console.WriteLine(value);
                                                    jarray = JArray.Parse(value);
                                                    break;
                                                case "goods_id":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        goods.goods_id = Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value));
                                                    else
                                                        goods.goods_id = value;
                                                    break;
                                                case "goods_name":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        goods.goods_name = Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value));
                                                    else
                                                        goods.goods_name = value;
                                                    break;
                                                case "image_url":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        goods.image_url = Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value));
                                                    else
                                                        goods.image_url = value;
                                                    break;
                                                case "price_info":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        goods.price = double.Parse(Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value)));
                                                    else
                                                        goods.price = double.Parse(value);
                                                    break;
                                                case "sales":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        goods.sales = int.Parse(Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value)));
                                                    else
                                                        goods.sales = int.Parse(value);
                                                    break;
                                                case "pd":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        goods.pd = Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value));
                                                    else
                                                        goods.pd = value;
                                                    break;
                                                case "pl":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        goods.pl = Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value));
                                                    else
                                                        goods.pl = value;
                                                    break;
                                                case "data":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        data = Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value));
                                                    else
                                                        data = value;
                                                    break;
                                                case "cookie":
                                                    if (!isPost && request.Request.ContentEncoding != Encoding.UTF8)
                                                        cookie = Encoding.GetEncoding("UTF-8").GetString(request.Request.ContentEncoding.GetBytes(value));
                                                    else
                                                        cookie = value;
                                                    break;
                                            }
                                        }
                                    }
                                    if (request.Request.RawUrl.StartsWith("/goods/stock"))
                                    {
                                        if (string.IsNullOrEmpty(cookie))
                                        {
                                            result = JsonConvert.SerializeObject(new
                                            {
                                                code = -1,
                                                msg = "cookie获取不到",
                                                data = ""
                                            });
                                            put("cookie获取不到");
                                            return;
                                        }
                                        Util.StrTojson(data, out JObject jObject);
                                        if (jObject != null)
                                        {
                                            int start = cookie.LastIndexOf(" PASS_ID=");
                                            string userid = null;
                                            put("cookie:" + cookie, false);
                                            if (start > 0)
                                            {
                                                userid = cookie.Substring(start, cookie.IndexOf("; ", start) - start);
                                                userid = userid.Substring(userid.LastIndexOf("_") + 1);
                                                //put("userid:"+ userid);
                                            }
                                            /*string[] cookies = cookie.Split(';');
                                            HashSet<string> set = new HashSet<string>();
                                            foreach (string s in  cookies)
                                            {
                                                set.Add(s);
                                            }
                                            Console.WriteLine(string.Join(";", set.ToArray()));*/
                                            //Service.put(data);
                                            pddhttp.executeincrease(new Pdd(userid, cookie, request.Request.UserAgent), jObject);
                                            result = JsonConvert.SerializeObject(new
                                            {
                                                code = 1,
                                                msg = "保存成功",
                                                data = ""
                                            }); ;
                                        }
                                        else
                                        {
                                            result = JsonConvert.SerializeObject(new
                                            {
                                                code = -1,
                                                msg = "解析出错",
                                                data = ""
                                            });
                                        }
                                    }
                                    else if (request.Request.RawUrl.StartsWith("/goods/add") && jarray != null)
                                    {
                                        int i = 0;
                                        foreach (JObject jObject in jarray)
                                        {
                                            goods = new goods();
                                            if (jObject["goods_id"] != null)
                                            {
                                                goods.goods_id = jObject["goods_id"] + "";
                                                goods.goods_name = jObject["goods_name"] + "";
                                                goods.image_url = jObject["image_url"] + "";
                                                goods.goods_url = "http://mobile.pinduoduo.com/goods.html?goods_id=" + jObject["goods_id"] + "";
                                                goods.price = jObject["price_info"].ToObject<double>(); ;
                                                goods.sales = int.Parse(jObject["sales"] + "");
                                                goods.newgoods = jObject["newgoods"].ToObject<int>();
                                                insertgoods(goods);
                                                i++;
                                                new Thread(delegate ()
                                                {
                                                    getimg(goods.goods_id, goods.image_url);
                                                }).Start();
                                            }

                                        }
                                        result = JsonConvert.SerializeObject(new
                                        {
                                            code = 1,
                                            msg = "保存成功",
                                            data = i
                                        });

                                    }
                                    else if (request.Request.RawUrl.StartsWith("/goods/get_details"))
                                    {
                                        result = JsonConvert.SerializeObject(new
                                        {
                                            code = -1,
                                            msg = "暂无需要处理数据",
                                            data = ""
                                        });
                                        DataTable dataTable = get_details(min, max);
                                        if (dataTable != null && dataTable.Rows.Count > 0)
                                        {
                                            result = JsonConvert.SerializeObject(new
                                            {
                                                code = 1,
                                                msg = "获取成功",
                                                data = dataTable.Rows[0][0]
                                            });
                                        }

                                    }
                                    else if (request.Request.RawUrl.StartsWith("/goods/set_details") && !string.IsNullOrWhiteSpace(goods.goods_id))
                                    {
                                        result = JsonConvert.SerializeObject(new
                                        {
                                            code = 1,
                                            msg = "保存成功",
                                            data = ""
                                        });
                                        if (!string.IsNullOrWhiteSpace(goods.pl))
                                        {
                                            if (goods.pl.Contains("万"))
                                            {
                                                double.TryParse(goods.pl.Replace("商品评价(", "").Replace("万)", ""), out double pls);
                                                goods.pls = (int)(pls * 10000);
                                            }
                                            else
                                                int.TryParse(goods.pl.Replace("商品评价(", "").Replace(")", ""), out goods.pls);
                                        }
                                        set_details(goods);
                                    }
                                    else if (request.Request.RawUrl.StartsWith("/mille") && Form1.form2 != null && !string.IsNullOrWhiteSpace(cookie))
                                    {
                                        if (string.IsNullOrEmpty(cookie))
                                        {
                                            result = JsonConvert.SerializeObject(new
                                            {
                                                code = -1,
                                                msg = "会话获取不到",
                                                data = ""
                                            });
                                            put("会话获取不到");
                                            return;
                                        }
                                        int start = cookie.IndexOf("PASS_ID=");
                                        string userid = null;
                                        Form2.put("会话:" + cookie);
                                        if (start > 0)
                                        {
                                            userid = cookie.Substring(start, cookie.IndexOf("; ", start) - start);
                                            userid = userid.Substring(userid.LastIndexOf("_") + 1);
                                        }
                                        Form2.pdd = new Pdd(userid, cookie, request.Request.UserAgent);
                                        result = JsonConvert.SerializeObject(new
                                        {
                                            code = 1,
                                            msg = "保存成功",
                                            data = ""
                                        }); ;
                                    }

                                }


                            }
                            else
                                Service.put("请求url:" + request.Request.RawUrl, false);
                        }
                        catch (Exception e)
                        {
                            Service.put("请求监听线程出错0:" + e.Message + ", " + e.StackTrace + ",url:" + request.Request.RawUrl, true);
                        }
                        finally
                        {
                            try
                            {
                                request.Response.StatusCode = 200;
                                request.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                                request.Response.ContentType = "application/json";
                                requestContext.Response.ContentEncoding = Encoding.UTF8;
                                byte[] buffer = Encoding.UTF8.GetBytes(result);
                                request.Response.ContentLength64 = buffer.Length;
                                var output = request.Response.OutputStream;
                                if (output != null && output.CanWrite)
                                {
                                    output.Write(buffer, 0, buffer.Length);
                                    output.Close();
                                }
                                if (request != null && request.Response != null)
                                    request.Response.Close();
                            }
                            catch (Exception)
                            {
                                if (request != null && request.Response != null)
                                    request.Response.StatusCode = 500;
                                Service.put("请求响应失败", false);
                            }

                        }
                    }), requestContext);
                    //Thread threadsub = new Thread(new ParameterizedThreadStart((requestcontext) =>
                    //{

                    //}));
                    //threadsub.Start(requestContext);
                }
            }
            catch (Exception e)
            {
                Service.put("请求监听线程出错2:" + e.Message + ", " + e.StackTrace, true);
            }
        }
        public static void getimg(string id, string url)
        {
            HttpItem item = new HttpItem()
            {
                URL = url,
                Method = "get",
                ContentType = "application/x-www-form-urlencoded",
                Accept = "*",
                ResultType = ResultType.Byte
            };
            byte[] b = new HttpHelper().GetHtml(item).ResultByte;
            if (b != null && b.Length > 2000)
            {
                try
                {
                    if (!File.Exists(LinkService.path + id + ".jpg"))
                        System.IO.File.WriteAllBytes(LinkService.path + id + ".jpg", b);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
        public static string increase(string data, string cookie = "api_uid=Ck625GDhjqkMpQBmJv18Ag==; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625635798684; JSESSIONID=6636B56A40DDDD7F54CED5AF17281372")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/vodka/v2/mms/edit/quantity/increase",
                Method = "post",
                Postdata = "{\"data\":[{\"beforeQuantity\":8,\"goodsId\":259233666708,\"quantity\":-1,\"skuId\":892866508592}],\"userId\":95786252,\"sourceId\":1}",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "ApiPOST Runtime +https://www.apipost.cn"
                //UserAgent= "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;

        }
        public static int insertgoods(goods goods)
        {
            try
            {
                return dbConn.executeUpdate("insert into pdd_goods(goods_id,goods_name,image_url,goods_url,price,sales,newgoods,create_time)  values(@goods_id,@goods_name,@image_url,@goods_url, @price, @sales, @newgoods, unix_timestamp(now())) ON DUPLICATE KEY UPDATE goods_name=@goods_name1, image_url=@image_url1,goods_url=@goods_url1, price=@price1, sales=@sales1, newgoods=@newgoods1", goods.goods_id, goods.goods_name, goods.image_url, goods.goods_url, goods.price, goods.sales, goods.newgoods, goods.goods_name, goods.image_url, goods.goods_url, goods.price, goods.sales, goods.newgoods);
            }
            catch (Exception e)
            {
                Service.put("数据库操作错误,insertgoods: " + e.Message + e.StackTrace, true);
            }
            return -1;
        }
        public static DataTable get_details(int min, int max)
        {
            try
            {
                return dbConn.getData("select goods_url from pdd_goods where details_status=0 and sales >= " + min + " and sales<=" + max + " order by create_time limit 1");
            }
            catch (Exception e)
            {
                Service.put("数据库操作错误,get_details: " + e.Message + e.StackTrace, true);
            }
            return null;
        }

        public static int set_details(goods goods)
        {
            try
            {
                return dbConn.executeUpdate("update pdd_goods set pd=@pd, pl = @pl,pls=@pls, details_status=1 , details_time = unix_timestamp(now()) where goods_id=@goods_id", goods.pd, goods.pl, goods.pls, goods.goods_id);
            }
            catch (Exception e)
            {
                Service.put("数据库操作错误,set_details," + goods.goods_id + ": " + e.Message + e.StackTrace, true);
            }
            return -1;
        }
        public static int updatepls()
        {
            try
            {
                dbConn.executeUpdate("update pdd_goods set pls = case pl when '商品评价(10万+)' then 100000  else pls end WHERE pls = 0");
                dbConn.executeUpdate("update pdd_goods set pls =  case  when LOCATE('万)', pl)>0 then(REPLACE(REPLACE(pl,'万)',''),'商品评价(','') +0.0)*10000  else pls end WHERE pls = 0");
                dbConn.executeUpdate("update pdd_goods set pls = case  when LOCATE('商品评价(', pl)>0 then CONVERT(REPLACE(REPLACE(pl,'商品评价(','') ,')',''), SIGNED)  else pls end  WHERE pls = 0");
                return 1;
            }
            catch (Exception e)
            {
                Service.put("数据库操作错误,updatepls," + ": " + e.Message + e.StackTrace, true);
            }
            return -1;
        }



        public static int updatecheck(string id, int check)
        {
            try
            {
                return dbConn.executeUpdate("update pdd_goods set checked=@checked where goods_id=@goods_id", check, id);
            }
            catch (Exception e)
            {
                Service.put("数据库操作错误,updatecheck," + id + ": " + e.Message + e.StackTrace, true);
            }
            return -1;
        }
        public static int getcount(string sql)
        {
            try
            {
                DataTable dataTable = dbConn.getData(sql);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return int.Parse(dataTable.Rows[0][0].ToString());
                }
            }
            catch (Exception e1)
            {
                Service.put("数据库操作错误,getcount: " + e1.Message + e1.StackTrace + sql, true);
            }

            return -1;
        }
        public static DataTable getgoods(int page, out int count, DateTime dateTime, string find = "", string order = "id", int rows = 10, double s = -1, double e = -1,
            int min = -1, int max = -1, double plmin = -1, double plmax = -1, int check = -1, int newgoods = -1)
        {
            count = 0;
            try
            {
                string where = "";
                if (s > -1)
                    where = " and price>=" + s;
                if (e > -1)
                    where = where + " and price<=" + e;
                if (min > -1)
                    where = where + " and sales>=" + min;
                if (max > -1)
                    where = where + " and sales<=" + max;
                if (plmin > -1)
                    where = where + " and pls>=" + plmin;
                if (plmax > -1)
                    where = where + " and pls<=" + plmax;
                if (dateTime.Year != 1970)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    where = where + " and create_time>=" + (dateTime - startTime).TotalSeconds;
                }
                if (check > -1)
                    where = where + " and checked=" + check;
                if (newgoods > -1)
                    where = where + " and newgoods=" + newgoods;
                if (string.IsNullOrEmpty(find))
                {
                    count = getcount("select count(*) from pdd_goods where 1=1 " + where);
                    return dbConn.getData("select goods_id,goods_name,image_url,goods_url,price,sales,create_time,pd,pl,checked, newgoods from pdd_goods where 1=1 " + where + " order by " + order + " limit " + page * rows + ", " + rows);
                }
                count = getcount("select count(*) from pdd_goods where goods_name like '%" + find + "%' or goods_url ='" + find + "'");
                return dbConn.getData("select goods_id,goods_name,image_url,goods_url,price,sales,create_time,pd,pl,checked, newgoods from pdd_goods where goods_name like '%" + find + "%' or goods_url =@find1  " + " order by " + order + " limit " + page * rows + ", " + rows, find);
            }
            catch (Exception e1)
            {
                Service.put("数据库操作错误,getgoods: " + e1.Message + e1.StackTrace, true);
            }
            return null;
        }
        public static DataTable getgoodsbyindex(int index, DateTime dateTime, string find = "", string order = "id", int rows = 1, double s = -1, double e = -1,
            int min = -1, int max = -1, int plmin = -1, int plmax = -1, int newgoods = -1)
        {
            try
            {
                string where = "";
                if (s > -1)
                    where = " and price>=" + s;
                if (e > -1)
                    where = where + " and price<=" + e;
                if (min > -1)
                    where = where + " and sales>=" + min;
                if (max > -1)
                    where = where + " and sales<=" + max;
                if (plmin > -1)
                    where = where + " and pls>=" + plmin;
                if (plmax > -1)
                    where = where + " and pls<=" + plmax;
                if (newgoods > -1)
                    where = where + " and newgoods=" + newgoods;
                if (dateTime.Year != 1970)
                {
                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    where = where + " and create_time>=" + (dateTime - startTime).TotalSeconds;
                }
                if (string.IsNullOrEmpty(find))
                {
                    return dbConn.getData("select goods_id,goods_name,image_url,goods_url,price,sales,create_time,pd,pl,checked,newgoods,id from pdd_goods where 1=1 " + where + " order by " + order + " limit " + index + ", " + rows);
                }
                return dbConn.getData("select goods_id,goods_name,image_url,goods_url,price,sales,create_time,pd,pl,checked,newgoods,id from pdd_goods where goods_name like '%" + find + "%' or goods_url =@find1  " + " order by " + order + " limit " + index + ", " + rows, find);
            }
            catch (Exception e1)
            {
                Service.put("数据库操作错误,getgoodsbyindex: " + e1.Message + e1.StackTrace, true);
            }
            return null;
        }
        public static int delete(double time)
        {
            try
            {
                dbConn.executeUpdate("delete from pdd_goods where create_time<" + time);
            }
            catch (Exception e1)
            {
                Service.put("数据库操作错误,delete: " + e1.Message + e1.StackTrace, true);
            }

            return -1;
        }
    }
}
