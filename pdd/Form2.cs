using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WechatRegster.listenes;

namespace pdd
{
    public partial class Form2 : Form
    {
        public static Form2 form2 = null;
        public Form2()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            form2 = this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public static void put(string message, bool isClient = true)
        {
            LinkService.put(message, false);
            if (isClient)
            {
                form2.BeginInvoke(new MethodInvoker(() =>
                {
                    form2.textBox_log.AppendText(message + "\r\n");
                }));
            }

        }
        public static Pdd pdd = null;
        public static HashSet<string> goodsset = new HashSet<string>();
        public static ConcurrentQueue<string[]> goodslist = new ConcurrentQueue<string[]>();
        private void button_add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox_ids.Text))
            {
                string[] lines = richTextBox_ids.Lines;
                int index = 0;
                foreach (string line in lines)
                {
                    /*if (!goodsset.Contains(line))
                    {
                        index++;
                        
                    }*/
                    string[] idstemp = line.Trim().Split(' ');
                    List<string> list = new List<string>();
                    foreach (string id in idstemp)
                    {
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            list.Add(id);
                        }
                    }
                    goodslist.Enqueue(list.ToArray());
                }
                put("添加数：" + lines + ",队列总数：" + goodslist.Count);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            form2.button1.Enabled = false;
            new Thread(delegate ()
            {
                string[] ids = null;
                try
                {
                    while (!Form1.isEnd)
                    {
                        goodslist.TryDequeue(out ids);
                        if (ids != null && ids.Length > 0 && pdd != null && !string.IsNullOrEmpty(pdd.cookie))
                        {
                            string cookie = pdd.cookie;
                            string result = pddhttp.pageQueryLegalGoods(cookie, "{ \"pageNum\": 1, \"goodsIds\": [ " + string.Join(",", ids) + " ], \"pageSize\": 50, \"bizId\": 1 }");
                            put("批发商品数据：" + result, true);
                            if (Util.StrTojson(result, out JObject jObject) && jObject["success"].ToObject<bool>())
                            //&& jObject["result"].ToObject<JObject>()["list"].ToObject<JArray>().Count!= ids.Length
                            {
                                JArray jArray = jObject["result"].ToObject<JObject>()["list"].ToObject<JArray>();
                                JObject obj = new JObject();
                                JArray arr = new JArray();
                                for(int i=0; i< jArray.Count; i++)
                                {
                                    jObject = jArray[i].ToObject<JObject>();
                                    if (jObject["enrollEnable"].ToObject<bool>())
                                    {
                                        Util.StrTojson("{\"goodsId\": " + jObject["goodsId"].ToString() + ",\"goodsLadderDiscounts\": [{\"ladderStartValue\": 2,\"ladderDiscount\": " + LinkService.ladderDiscount + "}]}", out JObject o);
                                        arr.Add(o);
                                    }
                                   
                                }
                                obj.Add("activityGoodsConfigs", arr);
                                obj.Add("bizId", 1);
                                bool isflag = true;
                                if (arr.Count > 0)
                                {
                                    //"{\"activityGoodsConfigs\": [{\"goodsId\": " + goodsId + ",\"goodsLadderDiscounts\": [{\"ladderStartValue\": 2,\"ladderDiscount\": " + LinkService.ladderDiscount + "}]}],\"bizId\": 1 }";
                                    result = pddhttp.milleaddGoods(obj.ToString(), cookie);
                                    put("创建批发商品：" + result, true);
                                    Util.StrTojson(result, out jObject);
                                    while (jObject != null && !jObject["success"].ToObject<bool>())
                                    {
                                        LinkService.verifyCode = null;
                                        put("出现验证码：" + jObject["result"].ToObject<JObject>()["sign"], true);
                                        this.pictureBox2.Image = Util.Base64StringToImage(jObject["result"].ToObject<JObject>()["picture"].ToString());
                                        for (int i = 0; i < 60; i++)
                                        {
                                            Thread.Sleep(1000);
                                            if (!string.IsNullOrWhiteSpace(LinkService.verifyCode))
                                                break;
                                        }
                                        obj.Add("sign", jObject["result"].ToObject<JObject>()["sign"].ToString());
                                        obj.Add("verifyCode", LinkService.verifyCode);
                                        result = pddhttp.milleaddGoods(obj.ToString(), cookie);
                                        Util.StrTojson(result, out jObject);
                                    }
                                    isflag = jObject != null && jObject["success"].ToObject<bool>();
                                }
                                
                                if (isflag)
                                {
                                    Thread.Sleep(1000);
                                    bool confirm = true;
                                    List<Mille> list = new List<Mille>();
                                    foreach (string id in ids)
                                    {
                                        result = pddhttp.queryAmsterdamSku(id, cookie);
                                        if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                            jObject["result"].ToObject<JObject>().ContainsKey("goodsId"))
                                        {
                                            jObject = jObject["result"].ToObject<JObject>();
                                            JObject goodsLadderDiscounts = jObject["goodsLadderDiscounts"].ToObject<JArray>()[0].ToObject<JObject>();
                                            JObject skuInfos = jObject["skuInfos"].ToObject<JArray>()[0].ToObject<JObject>();
                                            Mille mille = new Mille(jObject["goodsId"].ToString(), goodsLadderDiscounts["ladderStartValue"].ToObject<int>()
                                                , goodsLadderDiscounts["ladderDiscount"].ToObject<int>(), skuInfos["skuId"].ToObject<string>()
                                                , skuInfos["groupPrice"].ToObject<int>(), skuInfos["quantity"].ToObject<int>());
                                            list.Add(mille);
                                            Thread.Sleep(100);
                                        }
                                        else
                                        {
                                            confirm = false;
                                        }
                                    }
                                    if(!confirm)
                                    {
                                        put("确认定向供货数据失败：" + result, true);
                                        continue;
                                    }
                                    int pricesize = 0;
                                    obj = new JObject();
                                    arr = new JArray();

                                    foreach (Mille mille1 in list)
                                    {
                                        int quantity = 20000;
                                        if (mille1.quantity < 20000)
                                            quantity = mille1.quantity;
                                       
                                        if (quantity * mille1.groupPrice * mille1.ladderDiscount + pricesize < LinkService.pricesize * 100)
                                        {
                                            pricesize += quantity * mille1.groupPrice * mille1.ladderDiscount;
                                            Util.StrTojson("{ \"goodsId\": " + mille1.goodid + ", \"skuId\": " + mille1.skuId + ", \"skuNum\": " + quantity + " }", out JObject o);
                                            arr.Add(o);
                                        }
                                        else
                                        {
                                            quantity = LinkService.pricesize * 100 / (mille1.groupPrice * mille1.ladderDiscount);
                                            Util.StrTojson("{ \"goodsId\": " + mille1.goodid + ", \"skuId\": " + mille1.skuId + ", \"skuNum\": " + quantity + " }", out JObject o);
                                            arr.Add(o);
                                            break;
                                        }
                                        //"{ \"orderItems\": [ { \"goodsId\": " + goodsId + ", \"skuId\": " + skuId + ", \"skuNum\": " + piece + " } ] }"
                                    }
                                    obj.Add("orderItems", arr);
                                    Thread.Sleep(1000);
                                    result = pddhttp.createPreOrder(obj.ToString(), cookie);
                                    put("创建定向供货订单：" + result, true);
                                    if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                    string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                    {
                                        Thread.Sleep(1000);
                                        put("供货订单id：" + jObject["result"]["preOrderSn"]);
                                        result = pddhttp.sharePreOrder(jObject["result"]["preOrderSn"].ToString(), cookie);
                                        if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                        string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                        {
                                            put("二维码id：" + jObject["result"]["secretKey"]);
                                            pictureBox1.Image = Util.CreateQRCode("https://mai.pinduoduo.com/mobile-wholesale-ssr/confirm-order-csr?secret_key=" + jObject["result"]["secretKey"]);
                                        }
                                    }
                                    /*put("查询定向供货数据：" + result, true);
                                    if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                    jObject["result"].ToObject<JObject>().ContainsKey("goodsId"))
                                    {
                                        Thread.Sleep(1000);
                                        jObject = jObject["result"].ToObject<JObject>();
                                        JObject goodsLadderDiscounts = jObject["goodsLadderDiscounts"].ToObject<JArray>()[0].ToObject<JObject>();
                                        JObject skuInfos = jObject["skuInfos"].ToObject<JArray>()[0].ToObject<JObject>();
                                        Mille mille = new Mille(jObject["goodsId"].ToString(), goodsLadderDiscounts["ladderStartValue"].ToObject<int>()
                                            , goodsLadderDiscounts["ladderDiscount"].ToObject<int>(), skuInfos["skuId"].ToObject<string>()
                                            , skuInfos["groupPrice"].ToObject<int>(), skuInfos["quantity"].ToObject<int>());
                                        result = pddhttp.calMmsSkuPrice(mille.goodid, mille.ladderStartValue, mille.ladderDiscount, mille.skuId, 52, 20000, cookie);
                                        put("确认定向供货：" + result, true);
                                        if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                            string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                        {
                                            
                                        }
                                    }*/
                                }
                            }
                            else
                                put("输入的商品不存在，流程失败：" + string.Join(" ", ids), true);
                           
                            
                           
                        }
                        else
                        {
                            put("执行结束：" + DateTime.Now.ToString());
                            break;
                        }
                    }

                }
                catch (Exception e1)
                {
                    put(e1.StackTrace + e1.Message);
                }
                finally
                {
                    form2.button1.Enabled = true;
                }



            }).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pdd == null || string.IsNullOrEmpty(pdd.cookie))
            {
                MessageBox.Show("没有会话,先浏览器访问供货管理");
                return;
            }
            DialogResult dr = MessageBox.Show("确定删除批发商品数据吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                new Thread(delegate() {
                    string[] lines = richTextBox_ids.Lines;
                    foreach (string line in lines)
                    {
                        string[] idstemp = line.Trim().Split(' ');
                        foreach (string id in idstemp)
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                            {
                                string result = pddhttp.milledeleteGoods(id, pdd.cookie);
                                put("删除批发商品数据：" + result, true);
                                if (!Util.StrTojson(result, out JObject jObject) || !jObject["success"].ToObject<bool>() ||
                                       !string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                {
                                    put("删除批发商品失败：" + id, true);
                                }
                            }
                        }
                    }

                }).Start();
                
                
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //this.pictureBox1.Image = Util.Base64StringToImage("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");
            pictureBox1.Image = Util.CreateQRCode("https://mai.pinduoduo.com/mobile-wholesale-ssr/confirm-order-csr?secret_key=_RsQGzmAkG0WkkNXctXfOQD8HfgecWVVBRnH0D4JVQN8C5e2sRqNyG7qlbpFZqJ6");
            // pictureBox1.Image = Image.FromFile(LinkService.path + "1.jpg");
        }

        private void textBox_guige_Leave(object sender, EventArgs e)
        {
            
        }

        private void textBox_pricesize_Leave(object sender, EventArgs e)
        {
            int pricesize = LinkService.pricesize;
            if (!string.IsNullOrWhiteSpace(textBox_pricesize.Text) && int.TryParse(textBox_pricesize.Text.Trim(), out int val))
            {
                LinkService.pricesize = val;
            }
            else
            {
                textBox_pricesize.Text = pricesize + "";
                MessageBox.Show("设置失败");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LinkService.verifyCode = textBox_verifyCode.Text.Trim();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.textBox_log.Text = "";
        }
    }
}
