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
        public static ConcurrentQueue<string> goodslist = new ConcurrentQueue<string>();
        private void button_add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox_ids.Text))
            {
                string[] lines = richTextBox_ids.Lines;
                int index = 0;
                foreach (string line in lines)
                {
                    if (!goodsset.Contains(line))
                    {
                        index++;
                        goodsset.Add(line.Trim());
                        goodslist.Enqueue(line.Trim());
                    }
                }
                put("添加数：" + lines + ", 实际添加数：" + index + ",队列总数：" + goodslist.Count);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            form2.button1.Enabled = false;
            new Thread(delegate ()
            {
                string id = null;
                try
                {
                    while (!Form1.isEnd)
                    {
                        goodslist.TryDequeue(out id);
                        if (!string.IsNullOrEmpty(id) && pdd != null && !string.IsNullOrEmpty(pdd.cookie))
                        {
                            string cookie = pdd.cookie;
                            string result = pddhttp.milleaddGoods(id, 2, 97, cookie);
                            put("创建批发商品：" + result, true);
                            if (Util.StrTojson(result, out JObject jObject) && jObject["success"].ToObject<bool>())
                            {
                                Thread.Sleep(1000);
                                result = pddhttp.queryAmsterdamSku(id, cookie);
                                put("查询定向供货数据：" + result, true);
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
                                        Thread.Sleep(1000);
                                        result = pddhttp.createPreOrder(mille.goodid, mille.skuId, 52, cookie);
                                        put("创建定向供货订单："+result, true);
                                        if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                        string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                        {
                                            Thread.Sleep(1000);
                                            put("供货订单id：" + jObject["result"]["preOrderSn"]);
                                            pddhttp.sharePreOrder(jObject["result"]["preOrderSn"].ToString(), cookie);
                                            if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                            string.IsNullOrEmpty(jObject["errorMsg"] + "")) {
                                                put("二维码id：" + jObject["result"]["secretKey"]);
                                                pictureBox1.Image = Util.CreateQRCode("https://mai.pinduoduo.com/mobile-wholesale-ssr/confirm-order-csr?secret_key=" + jObject["result"]["secretKey"]);
                                            }
                                        }
                                    }
                                }
                            }
                           
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
                string result = pddhttp.milledeleteGoods(richTextBox_ids.Text.Trim(), pdd.cookie);
                put("删除批发商品数据："+result, true);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Util.CreateQRCode("https://mai.pinduoduo.com/mobile-wholesale-ssr/confirm-order-csr?secret_key=_RsQGzmAkG0WkkNXctXfOQD8HfgecWVVBRnH0D4JVQN8C5e2sRqNyG7qlbpFZqJ6");
           // pictureBox1.Image = Image.FromFile(LinkService.path + "1.jpg");
        }

        private void textBox_guige_Leave(object sender, EventArgs e)
        {
            int max = LinkService.ladderDiscount;
            if (!string.IsNullOrWhiteSpace(textBox_guige.Text) && int.TryParse(textBox_max.Text.Trim(), out int val))
            {
                LinkService.max = val;
            }
            else
            {
                textBox_max.Text = max + "";
                MessageBox.Show("设置失败");
            }
        }
    }
}
