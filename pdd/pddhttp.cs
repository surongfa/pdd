using Newtonsoft.Json.Linq;
using SufeiUtil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WechatRegster.listenes;

namespace pdd
{
    public class Pdd
    {
        public Pdd(string userid, string cookie)
        {
            this.userid = userid;
            this.cookie = cookie;
        }
        public string userid;
        public string cookie;
    }
    public class Mille
    {
        public Mille(string goodid, int ladderStartValue, int ladderDiscount, string skuId, int groupPrice, int quantity)
        {
            this.goodid = goodid;
            this.ladderStartValue = ladderStartValue;
            this.ladderDiscount = ladderDiscount; // 折扣
            this.skuId = skuId;
            this.groupPrice = groupPrice; // 单价
            this.quantity = quantity;  //库存总量
        }
        public string goodid;
        public int ladderStartValue;
        public int ladderDiscount;
        public string skuId;
        public int groupPrice;
        public int quantity;
    }
    class pddhttp
    {
        public static Dictionary<string, Pdd> users = new Dictionary<string, Pdd>();
        public static Dictionary<string, JObject> goodslist = new Dictionary<string, JObject>();
        public static ConcurrentQueue<JObject> goodslistrun = new ConcurrentQueue<JObject>();
        private static object Singleton_Lock = new object();
        public static void executeincrease(Pdd pdd, JObject jobject)
        {
            lock (Singleton_Lock)
            {
                if (jobject != null)
                {
                    if (jobject != null && jobject.ContainsKey("result"))
                    {
                        JArray jArray = jobject["result"].ToObject<JObject>()["goods_list"].ToObject<JArray>();
                        int index = 0;
                        foreach (JObject obj in jArray)
                        {
                            if (!goodslist.ContainsKey(obj["id"].ToString()))
                            {
                                if (obj["sold_quantity"].ToObject<int>() < 100000 && obj["sku_list"].ToObject<JArray>()[0]["skuQuantity"].ToObject<int>() < 100000)
                                    index++;
                                goodslistrun.Enqueue(obj);
                                goodslist.Add(obj["id"].ToString(), obj);
                            }
                        }
                        Service.put("新增：" + jArray.Count + "，需要修改数：" + index, true);
                    }
                }
                if (users.ContainsKey(pdd.userid))
                {
                    users[pdd.userid] = pdd;
                    return;
                }
                else
                {
                    users[pdd.userid] = pdd;
                    Service.put(pdd.userid + "：" + pdd.cookie, false);
                }

                new Thread(delegate (object o)
                {
                    Pdd pdd1 = (Pdd)o;
                    Service.put("线程启动：" + pdd1.userid);
                    int success = 0, fail = 0;
                    try
                    {
                        while (!Form1.isEnd)
                        {
                            if (goodslistrun.Count > 0)
                            {
                                while (goodslistrun.Count > 0)
                                {
                                    goodslistrun.TryDequeue(out JObject obj);
                                    if (obj != null && obj["sold_quantity"].ToObject<int>() < 100000 && obj["sku_list"].ToObject<JArray>()[0]["skuQuantity"].ToObject<int>() < 100000)
                                    {
                                        DateTime start = DateTime.Now;
                                        string result = increase(pdd1.userid, obj["id"].ToString(), obj["quantity"].ToObject<int>(), 100000,
                                        obj["sku_list"].ToObject<JArray>()[0]["skuId"].ToString(), pdd1.cookie);
                                        //{"success":true,"errorCode":1000000,"errorMsg":null,"result":[259932882294]}
                                        if (result != null && result.Contains("1000000"))
                                            success++;
                                        else
                                            fail++;
                                        Service.put((result.Contains("1000000") ? "成功" : "失败") + ", " + obj["id"].ToString() + ",耗时：" + (DateTime.Now - start).Milliseconds + ", " + result);
                                    }
                                    Thread.Sleep(LinkService.sleep);
                                }
                                Service.put("执行结束，成功：" + success + "失败：" + fail);
                            }
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception e)
                    {
                        Service.put("executeincrease异常：" + e.Message + e.StackTrace);
                    }
                    finally
                    {
                        users.Remove(pdd1.userid);
                    }
                }).Start(pdd);
            }


        }


        public static string increase(string userId, string goodsId, int beforeQuantity, int quantity, string skuId, string cookie = "")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/vodka/v2/mms/edit/quantity/increase",
                Method = "post",
                Postdata = "{\"data\":[{\"beforeQuantity\":" + beforeQuantity + ",\"goodsId\":" + goodsId + ",\"quantity\":" + quantity + ",\"skuId\":" + skuId + "}],\"userId\":" + userId + ",\"sourceId\":1}",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;

        }

        //{"pre_sale_type":4,"page":1,"is_onsale":1,"sold_out":0,"size":10}
        public static string goodsList(int page, string cookie)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/vodka/v2/mms/query/display/mall/goodsList",
                Method = "post",
                Postdata = "{ \"pre_sale_type\":4,\"page\":" + page + ",\"is_onsal\":1,\"sold_out\":0,\"size\":100}",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;

        }


        // 定向供货

        public static string pageQueryLegalGoods(string cookie)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/mms/goods/pageQueryLegalGoods",
                Method = "post",
                Postdata = "{\"pageNum\": 1,\"pageSize\": 50,\"bizId\": 1}",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string pageQueryLegalGoods(string cookie, string Postdata)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/mms/goods/pageQueryLegalGoods",
                Method = "post",
                Postdata = Postdata,
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        
        public static string milleaddGoods(string Postdata, string cookie = "")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/mms/goods/addGoods",
                Method = "post",
                Postdata = Postdata, //"{\"activityGoodsConfigs\": [{\"goodsId\": " + goodsId + ",\"goodsLadderDiscounts\": [{\"ladderStartValue\": " + ladderStartValue + ",\"ladderDiscount\": " + ladderDiscount + "}]}],\"bizId\": 1 }",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;

        }

        public static string queryAmsterdamSku(string goodsId, string cookie = "")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/amsterdam/queryAmsterdamSku",
                Method = "post",
                Postdata = "{\"goodsId\": " + goodsId + " }",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string milledeleteGoods(string goodsId, string cookie)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/mms/goods/deleteGoods",
                Method = "post",
                Postdata = "{\"goodsId\": " + goodsId + ",\"bizId\":1 }",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string calMmsSkuPrice(string goodsId, int ladderStartValue, int ladderDiscount, string skuId, int piece, int groupPrice, string cookie)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/mms/common/calMmsSkuPrice",
                Method = "post",
                Postdata = "{\"goodsId\":" + goodsId + ",\"goodsLadderDiscounts\":[{\"ladderStartValue\":" + ladderStartValue + ",\"ladderDiscount\":" + ladderDiscount + "}],\"skuInfos\":[{\"skuId\":" + skuId + ",\"piece\":" + piece + ",\"groupPrice\":" + groupPrice + "}]}",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string createPreOrder(string Postdata, string cookie)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/amsterdam/createPreOrder",
                Method = "post",
                Postdata = Postdata,//"{ \"orderItems\": [ { \"goodsId\": " + goodsId + ", \"skuId\": " + skuId + ", \"skuNum\": " + piece + " } ] }",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string sharePreOrder(string preOrderSn, string cookie)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/amsterdam/sharePreOrder",
                Method = "post",
                Postdata = "{ \"preOrderSn\": "+ preOrderSn + " }",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            return new HttpHelper().GetHtml(item).Html;
        }
       
    }
}
