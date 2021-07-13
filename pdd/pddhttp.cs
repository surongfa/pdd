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
        public Pdd(string userid, string cookie, string userAgent)
        {
            this.userid = userid;
            this.cookie = cookie;
            this.userAgent = userAgent;
        }
        public string userAgent;
        public string userid;
        public string cookie;
        public string addressid;
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
    public class Sku
    {
        public Sku(string goodid, string skuId, int groupPrice, int quantity)
        {
            this.goodid = goodid;
            this.skuId = skuId;
            this.groupPrice = groupPrice; // 单价
            this.quantity = quantity;  //库存总量
        }
        public string goodid;
        public string skuId;
        public int groupPrice;
        public int quantity;
    }

    class pddhttp
    {
        private static Dictionary<string, Pdd> users = new Dictionary<string, Pdd>();
        public static Dictionary<string, JObject> goodslist = new Dictionary<string, JObject>();
        private static ConcurrentQueue<JObject> goodslistrun = new ConcurrentQueue<JObject>();
        public static ConcurrentQueue<string> goodskucunlist = new ConcurrentQueue<string>();
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
                                        obj["sku_list"].ToObject<JArray>()[0]["skuId"].ToString(), pdd1.cookie, pdd1.userAgent);
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

        public static void executeautokucun()
        {
            while (!Form1.isEnd && LinkService.autokucun && !Form2.stop)
            {
                while (goodskucunlist.TryDequeue(out string goodsid) && !string.IsNullOrEmpty(goodsid))
                {
                    string anicontent = Util.executescript(Service.anicontentcode, "getEncode()");
                    string result = goodsone(new string[] { goodsid }, Form2.pdd.cookie, anicontent);
                    if (Util.StrTojson(result, out JObject jObject) && jObject["success"].ToObject<bool>() &&
                        jObject["errorCode"].ToObject<int>() == 1000000 && jObject["result"].ToObject<JObject>()["goods_list"].ToObject<JArray>().Count > 0)
                    {
                        jObject = jObject["result"].ToObject<JObject>()["goods_list"].ToObject<JArray>()[0].ToObject<JObject>();
                        JArray jArray = jObject["sku_list"].ToObject<JArray>();
                        for (int i = 0; i < jArray.Count; i++)
                        {
                            int skuQuantity = jArray[i]["skuQuantity"].ToObject<int>();
                            if (skuQuantity > 100000)
                            {
                                Form2.put("商品：" + goodsid + "库存已满足");
                                Form2.goodslist.Enqueue(goodsid);
                                break;
                            }
                            else if (i == jArray.Count - 1)
                            {
                                result = increase(Form2.pdd.userid, goodsid, skuQuantity, 100000, jArray[0]["skuId"].ToString(), Form2.pdd.cookie, Form2.pdd.userAgent);
                                if (result != null && result.Contains("100000"))
                                {
                                    Form2.put("商品：" + goodsid + "库存修改成功");
                                    new Thread(delegate (object value)
                                    {
                                        Thread.Sleep(1000);
                                        Form2.goodslist.Enqueue(value.ToString());
                                    }).Start(goodsid);
                                }
                                else
                                {
                                    Form2.put("商品：" + goodsid + "库存修改失败：" + result);
                                    Form2.form2.richTextBox_deletefail.AppendText("异常商品id：" + goodsid + "\r\n");
                                }
                                    
                            }
                        }
                    }
                    else
                        Form2.form2.richTextBox_deletefail.AppendText("异常商品id：" + goodsid + "\r\n");
                }
                Thread.Sleep(1000);
            }
            if (!LinkService.autokucun)
            {
                while (goodskucunlist.TryDequeue(out string goodsid))
                {
                    Form2.goodslist.Enqueue(goodsid);
                }
            }
        }

        //修改库存
        public static string increase(string userId, string goodsId, int beforeQuantity, int quantity, string skuId, string cookie = "", string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/vodka/v2/mms/edit/quantity/increase",
                Method = "post",
                Postdata = "{\"data\":[{\"beforeQuantity\":" + beforeQuantity + ",\"goodsId\":" + goodsId + ",\"quantity\":" + quantity + ",\"skuId\":" + skuId + "}],\"userId\":" + userId + ",\"sourceId\":1}",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                UserAgent = userAgent,
            };
            LinkService.put(item.Postdata, false);
            return new HttpHelper().GetHtml(item).Html;

        }

        //{"pre_sale_type":4,"page":1,"is_onsale":1,"sold_out":0,"size":10}
        //{"goods_id_list":["260898652768"],"pre_sale_type":4,"page":1,"is_onsale":1,"sold_out":0,"size":10}
        // is_onsale 1：在售 0售罄  sold_out 1：下架 0上架
        public static string goodsList(int page, string cookie, string anicontent)
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
        public static string goodsone(string[] ids, string cookie, string anicontent)
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/vodka/v2/mms/query/display/mall/goodsList",
                Method = "post",
                Postdata = "{ \"goods_id_list\":[\"" + string.Join("\",\"", ids) + "\"],\"pre_sale_type\":4,\"page\":1,\"is_onsal\":1,\"sold_out\":0,\"size\":100}",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36",
            };
            item.Header.Add("Anti-Content", anicontent);
            return new HttpHelper().GetHtml(item).Html;

        }

        // 定向供货

        public static string pageQueryLegalGoods(string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
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
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string pageQueryLegalGoods(string cookie, string Postdata, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
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
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }

        public static string milleaddGoods(string Postdata, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
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
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;

        }

        public static string queryAmsterdamSku(string goodsId, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
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
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string milledeleteGoods(string goodsId, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
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
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string calMmsSkuPrice(string goodsId, int ladderStartValue, int ladderDiscount, string skuId, int piece, int groupPrice, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
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
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string createPreOrder(string Postdata, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
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
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string sharePreOrder(string preOrderSn, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/amsterdam/sharePreOrder",
                Method = "post",
                Postdata = "{ \"preOrderSn\": " + preOrderSn + " }",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                KeepAlive = true,
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }
        public static string queryQrCodeStatus(string secretKey, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/mille/amsterdam/queryQrCodeStatus",
                Method = "post",
                Postdata = "{ 	\"secretKey\": \"" + secretKey + "\" }",
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }

        //获取用户pdd地址信息
        public static string pddaddress(string secretKey, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/api/tanya/tob/addr/default/address_for_pf",
                Method = "post",
                Postdata = "{\"scene_id\":\"80002\",\"secret_key\":\""+ secretKey + "\",\"shipping_type\":1}",  //80002服务编号
                ContentType = "application/json;charset=UTF-8",
                Accept = "*",
                Cookie = cookie,
                UserAgent = UserAgent,
            };
            return new HttpHelper().GetHtml(item).Html;
        }

        //{"success":false,"errorCode":7000027,"errorMsg":"发起微信支付失败","result":null}
        public static string createOrder(string secretKey, string addressid, string anicontent, string cookie, string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36")
        {
            HttpItem item = new HttpItem()
            {
                URL = "https://mms.pinduoduo.com/pifa/bapp/order/createOrder",
                Method = "post",
                Postdata = "{\"payType\":0,\"frontEnv\":7,\"payMethodInfo\":\"{\\\"payMethod\\\":\\\"WEIXIN\\\"}\",\"secretKey\":\""+ secretKey + "\",\"orderType\":3,\"addressId\":\""+ addressid + "\",\"promotionType\":null,\"promotionId\":null,\"pageId\":\"\"}",  //80002服务编号
                ContentType = "application/json;charset=UTF-8",
                Accept = "application/json, text/javascript, */*; q=0.01",
                Cookie = cookie,
                UserAgent = UserAgent,
            };
            item.Header.Add("anti-content", anicontent);
            item.Header.Add("Origin", "https://mms.pinduoduo.com");  //关键头
            return new HttpHelper().GetHtml(item).Html;
        }
        //{"payType":0,"frontEnv":7,"payMethodInfo":"{\"payMethod\":\"WEIXIN\"}","secretKey":"opC40bqVT7qcXSuG4OYonXuhleZdpLBoQN8wHoXQNydt_82t5GwlTRGBZ3HCp2bK","orderType":3,"addressId":"23102662059","promotionType":null,"promotionId":null,"pageId":""}
    }
}
