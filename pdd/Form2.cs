using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
            this.ControlBox = false;
            ArrayList array = new ArrayList();
            array.Add(new System.Web.UI.WebControls.ListItem("手动扫码", "0"));
            array.Add(new System.Web.UI.WebControls.ListItem("自动自付", "1"));
            array.Add(new System.Web.UI.WebControls.ListItem("自动他付", "2"));
            comboBox_payorder.DataSource = array;
            comboBox_payorder.DropDownStyle = ComboBoxStyle.DropDownList;
            string path = AppDomain.CurrentDomain.BaseDirectory + "anicontent.js";
            //加载js文件的所有内容
            Service.anicontentcode = System.IO.File.ReadAllText(path);
            button_add.Focus();
            richTextBox_ids.Text = "商品id，多个空格隔开或者换行";
            richTextBox_ids.ForeColor = Color.Gray;

            DataTable dataTable = LinkService.getsystemconfig(LinkService.macid);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                textBox_pdduserid.Text = dataTable.Rows[0][0] + "";
                textBox_paypdduserid.Text = dataTable.Rows[0][1] + "";
            }
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
        public static Pdd paypdd = null;
        public static bool stop = false;
        public static HashSet<string> goodsset = new HashSet<string>();
        public static ConcurrentQueue<string> goodslist = new ConcurrentQueue<string>();
        public static int exeindex = 0;
        public static int execount = 0;
        public static string pdduserid;
        public static string paypdduserid;
        private void button_add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox_ids.Text))
            {
                string[] lines = richTextBox_ids.Lines;
                int index = 0;
                foreach (string line in lines)
                {
                    string[] idstemp = line.Trim().Split(' ');
                    foreach (string id in idstemp)
                    {
                        if (!string.IsNullOrWhiteSpace(id))
                        {

                            if (!goodsset.Contains(id) && id.Length > 11)
                            {
                                if (int.TryParse(id.Substring(0, id.Length - 10), out int val) && val > 20)
                                {
                                    index++;
                                    goodsset.Add(id);
                                    pddhttp.goodskucunlist.Enqueue(id);
                                    execount++;
                                }
                            }
                            else
                            {
                                put("商品id已存在：" + id);
                            }
                        }
                    }
                }
                label_count.Text = execount.ToString();
                put("总数：" + pddhttp.goodskucunlist.Count + "，添加数：" + index);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pdd == null || string.IsNullOrEmpty(pdd.pdduserid))
            {
                MessageBox.Show("请输入并在浏览器登录供货管理店铺");
                return;
            }
            if (LinkService.payorder == 2 && (paypdd == null || string.IsNullOrWhiteSpace(paypdd.pdduserid)))
            {
                MessageBox.Show("请输入并在浏览器登录自动他付店铺");
                return;
            }

            Form2.stop = false;
            form2.button1.Enabled = false;
            checkBox_autokucun.Enabled = false;
            button2.Enabled = false;
            new Thread(pddhttp.executeautokucun).Start();
            new Thread(delegate ()
            {
                List<Mille> millelist = new List<Mille>();
                string cookie = null, userAgent = null, result = null, tempid = null, id = null, skuid = null, addressid = null;
                try
                {

                    int tempquantity = 0;
                    while (!Form1.isEnd && !stop)
                    {
                        if (pddhttp.goodskucunlist.Count > 0 && goodslist.Count < 10) // 加载够10行
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        this.pictureBox2.Image = null;
                        textBox_verifyCode.Text = "";
                        foreach (Mille mille in millelist)
                        {
                            richTextBox_deletefail.AppendText("异常商品id：" + mille.goodid + "\r\n");
                        }
                        int pricesize = 0;
                        millelist = new List<Mille>();
                        cookie = pdd.cookie;
                        userAgent = pdd.userAgent;
                        addressid = pdd.addressid;
                        bool end = false;
                        int index = 0;
                        while (!end && (tempid != null || goodslist.TryDequeue(out id)))
                        {
                            label_id.Text = id;
                            if (tempid != null)
                                id = tempid;
                            else
                            {
                                tempquantity = 0;
                                exeindex++;
                                label_index.Text = exeindex.ToString();
                            }
                            if (id != null && pdd != null && !string.IsNullOrEmpty(cookie))
                            {
                                tempid = null;
                                pictureBox1.Image = null;
                                pictureBox2.Image = null;
                                result = pddhttp.pageQueryLegalGoods(cookie, "{ \"pageNum\": 1, \"goodsIds\": [ " + id + " ], \"pageSize\": 50, \"bizId\": 1 }", userAgent);
                                put("批发商品数据：" + result, true);
                                if (Util.StrTojson(result, out JObject jObject) && jObject["success"].ToObject<bool>())
                                {
                                    string json = "{\"activityGoodsConfigs\":[{\"goodsId\": " + id + ",\"goodsLadderDiscounts\": [{\"ladderStartValue\": 2,\"ladderDiscount\": 90}]}],\"bizId\":1}";
                                    if (jObject["result"].ToObject<JObject>()["list"].ToObject<JArray>()[0].ToObject<JObject>()["enrollEnable"].ToObject<bool>())
                                    {
                                        result = pddhttp.milleaddGoods(json, cookie, userAgent);
                                        put("创建批发商品：" + result, false);
                                        Util.StrTojson(result, out jObject);
                                        if (jObject != null && !jObject.ContainsKey("success") && jObject["error_code"].ToObject<int>() == 54001)
                                        {
                                            put("创建批发商品：" + result);
                                            MessageBox.Show("创建批发商品出现验证码");
                                        }
                                        //{"error_msg":"操作太过频繁，请稍后再试！","error_code":54001,"result":{"verifyAuthToken":"ljehOAVugGFS-dharObFqQ784dab020028a14f0"}}
                                        if (jObject != null && jObject["success"].ToObject<bool>())
                                        {
                                            Thread.Sleep(1000);
                                            //List<Mille> list = new List<Mille>();
                                            result = pddhttp.queryAmsterdamSku(id, cookie, userAgent);
                                            if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                                jObject["result"].ToObject<JObject>().ContainsKey("goodsId"))
                                            {
                                                jObject = jObject["result"].ToObject<JObject>();
                                                JObject goodsLadderDiscounts = jObject["goodsLadderDiscounts"].ToObject<JArray>()[0].ToObject<JObject>();
                                                JArray jArray = jObject["skuInfos"].ToObject<JArray>();
                                                for (int i = 0; i < jArray.Count; i++)
                                                {
                                                    JObject skuInfos = jArray[i].ToObject<JObject>();
                                                    if ((skuid != null && skuInfos["skuId"].ToObject<string>().Equals(skuid)) || skuInfos["quantity"].ToObject<int>() > 100000)
                                                    {
                                                        Mille mille = new Mille(jObject["goodsId"].ToString(), goodsLadderDiscounts["ladderStartValue"].ToObject<int>()
                                                        , 1, skuInfos["skuId"].ToObject<string>()
                                                        , skuInfos["groupPrice"].ToObject<int>(), skuInfos["quantity"].ToObject<int>());
                                                        int quantity = mille.quantity - tempquantity - 250;
                                                        result = pddhttp.milledeleteGoods(id, cookie, userAgent); // 先删除批发商品
                                                        put("删除9折批发商品数据：" + result, false);
                                                        if (!Util.StrTojson(result, out jObject) || !jObject["success"].ToObject<bool>() ||
                                                               !string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                                        {
                                                            put("删除9折批发商品失败：" + id, true);
                                                            richTextBox_deletefail.AppendText("删除失败：" + id + "\r\n");
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            index++;
                                                            put(quantity + " " + (mille.groupPrice / 100) + " " + mille.ladderDiscount + " " + pricesize + " " + LinkService.pricesize * 100, false);
                                                            if (quantity * (mille.groupPrice / 100) * mille.ladderDiscount + pricesize < LinkService.pricesize * 100)
                                                            {
                                                                pricesize += quantity * (mille.groupPrice / 100) * mille.ladderDiscount;
                                                                mille.quantity = quantity;
                                                                millelist.Add(mille);
                                                                tempquantity = 0;
                                                            }
                                                            else
                                                            {
                                                                put(quantity + " " + LinkService.pricesize * 100 + " " + pricesize + " " + mille.groupPrice + " " + mille.ladderDiscount, false);
                                                                int quantity1 = (LinkService.pricesize * 100 - pricesize) / (mille.groupPrice / 100 * mille.ladderDiscount);
                                                                if (quantity - quantity1 < 250)
                                                                    quantity1 = quantity - 250;
                                                                mille.quantity = quantity1;
                                                                tempquantity = mille.quantity;
                                                                millelist.Add(mille);
                                                                tempid = id;
                                                                skuid = mille.skuId;
                                                                end = true;
                                                            }
                                                            if (index > 9)
                                                                end = true;
                                                        }
                                                        break;
                                                    }
                                                    else if (i == jArray.Count - 1)
                                                    {
                                                        put("批发商品没有sku库存大于10万：" + id, true);
                                                        richTextBox_deletefail.AppendText("sku库存不满足：" + id + "\r\n");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                put("查询供货商品规格失败：" + result, true);
                                                richTextBox_deletefail.AppendText("查询供货商品规格失败：" + id + "\r\n");
                                            }
                                        }
                                        else
                                        {
                                            put("创建批发商品失败：" + id + result, true);
                                            richTextBox_deletefail.AppendText("创建批发商品失败：" + id + "\r\n");
                                        }
                                    }
                                    else if (!jObject["result"].ToObject<JObject>()["list"].ToObject<JArray>()[0].ToObject<JObject>()["enrollEnable"].ToObject<bool>())
                                    {
                                        richTextBox_deletefail.AppendText("商品已参与活动：" + id + "\r\n");
                                    }
                                    else
                                    {
                                        put("查询批发商品失败：" + id + result, true);
                                        richTextBox_deletefail.AppendText("查询批发商品失败：" + id + "\r\n");
                                    }
                                }
                            }
                        }
                        if (millelist.Count > 0)
                        {
                            JObject obj = new JObject();
                            JArray arr = new JArray();
                            for (int i = 0; i < millelist.Count; i++)
                            {
                                Util.StrTojson("{\"goodsId\": " + millelist[i].goodid + ",\"goodsLadderDiscounts\": [{\"ladderStartValue\": 2,\"ladderDiscount\": " + LinkService.ladderDiscount + "}]}", out JObject o);
                                arr.Add(o);
                            }
                            obj.Add("activityGoodsConfigs", arr);
                            obj.Add("bizId", 1);
                            bool isflag = true;
                            if (arr.Count > 0)
                            {
                                //"{\"activityGoodsConfigs\": [{\"goodsId\": " + goodsId + ",\"goodsLadderDiscounts\": [{\"ladderStartValue\": 2,\"ladderDiscount\": " + LinkService.ladderDiscount + "}]}],\"bizId\": 1 }";
                                result = pddhttp.milleaddGoods(obj.ToString(), cookie, userAgent);
                                put("创建批发商品1：" + result, false);
                                Util.StrTojson(result, out JObject jObject);
                                while (jObject != null && jObject.ContainsKey("success") && !jObject["success"].ToObject<bool>())
                                {
                                    LinkService.verifyCode = null;
                                    textBox_verifyCode.Text = "";
                                    put("重新输入验证码：" + jObject["result"].ToObject<JObject>()["sign"], true);
                                    string base64 = jObject["result"].ToObject<JObject>()["picture"].ToString().Replace("data:image/jpeg;base64,", "");
                                    this.pictureBox2.Image = Util.Base64StringToImage(base64);
                                    if (LinkService.autover)
                                    {
                                        new Task(() =>
                                        {
                                            Util.StrTojson(Util.verCode(base64), out JObject jObject1);
                                            if (jObject1 != null && jObject1["code"].ToObject<int>() == 0)
                                            {
                                                LinkService.captchaId = jObject1["data"].ToObject<JObject>()["captchaId"] + "";
                                                if ((jObject1["data"].ToObject<JObject>()["recognition"] + "").Length > 4)
                                                    LinkService.verifyCode = (jObject1["data"].ToObject<JObject>()["recognition"] + "").Substring(0, 4);
                                                else
                                                    LinkService.verifyCode = jObject1["data"].ToObject<JObject>()["recognition"] + "";
                                                textBox_verifyCode.Text = LinkService.verifyCode;
                                            }
                                            else if (jObject1 != null)
                                                put("打码返回结果：" + jObject1.ToString());
                                        }).Start();
                                    }

                                    for (int i = 0; i < 60; i++)
                                    {
                                        Thread.Sleep(1000);
                                        if (!string.IsNullOrWhiteSpace(LinkService.verifyCode))
                                            break;
                                    }
                                    obj["sign"] = jObject["result"].ToObject<JObject>()["sign"].ToString();
                                    obj["verifyCode"] = LinkService.verifyCode;
                                    result = pddhttp.milleaddGoods(obj.ToString(), cookie, userAgent);
                                    Util.StrTojson(result, out jObject);
                                    put(result, false);
                                    if (LinkService.autover && jObject != null && jObject.ContainsKey("success") && !jObject["success"].ToObject<bool>())
                                        Util.reporterror(LinkService.captchaId);
                                }

                                isflag = jObject != null && jObject["success"].ToObject<bool>();
                                if (isflag)
                                {
                                    Thread.Sleep(1000);
                                    pricesize = 0;
                                    obj = new JObject();
                                    arr = new JArray();

                                    foreach (Mille mille in millelist)
                                    {
                                        result = pddhttp.calMmsSkuPrice(mille.goodid, mille.ladderStartValue, mille.ladderDiscount, mille.skuId, mille.quantity, mille.groupPrice, cookie, userAgent);
                                        put("确认定向供货：" + result, true);
                                        if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                            string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                        {
                                            Util.StrTojson("{ \"goodsId\": " + mille.goodid + ", \"skuId\": " + mille.skuId + ", \"skuNum\": " + mille.quantity + " }", out JObject o);
                                            arr.Add(o);
                                        }
                                        else
                                            put("确认定向供货失败", true);
                                        //"{ \"orderItems\": [ { \"goodsId\": " + goodsId + ", \"skuId\": " + skuId + ", \"skuNum\": " + piece + " } ] }"
                                    }
                                    obj.Add("orderItems", arr);
                                    put("创建定向供货订单数据：" + obj.ToString(), false);
                                    result = pddhttp.createPreOrder(obj.ToString(), cookie, userAgent);
                                    put("创建定向供货订单：" + result, false);
                                    if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                    string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                    {
                                        Thread.Sleep(1000);
                                        put("供货订单id：" + jObject["result"]["preOrderSn"], false);
                                        result = pddhttp.sharePreOrder(jObject["result"]["preOrderSn"].ToString(), cookie, userAgent);
                                        if (Util.StrTojson(result, out jObject) && jObject["success"].ToObject<bool>() &&
                                        string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                        {
                                            string secretKey = jObject["result"]["secretKey"].ToString();
                                            put("二维码id：" + secretKey);
                                            pictureBox1.Image = Util.CreateQRCode("https://mai.pinduoduo.com/mobile-wholesale-ssr/confirm-order-csr?secret_key=" + jObject["result"]["secretKey"]);
                                            for (int i = 0; i < 300 && !stop; i++)
                                            {
                                                Thread.Sleep(1000);
                                                result = pddhttp.queryQrCodeStatus(secretKey, cookie, userAgent);
                                                put("扫码心跳" + i + "：" + result);
                                                Util.StrTojson(result, out jObject);
                                                int status = jObject != null && jObject["success"].ToObject<bool>() ? jObject["result"].ToObject<JObject>()["qrCodeStatus"].ToObject<int>() : 0;
                                                if (status == 2)
                                                {
                                                    put("扫码成功");
                                                    break;
                                                }
                                                if (i == 299 || status == 3)
                                                    put("扫码失败");
                                                if (i >= 3 && LinkService.payorder != 0)
                                                {
                                                    string paycookie = LinkService.payorder == 1 ? cookie : paypdd.cookie;

                                                    if (string.IsNullOrEmpty(addressid))
                                                    {
                                                        result = pddhttp.pddaddress(secretKey, paycookie, userAgent);
                                                        if (Util.StrTojson(result, out jObject) && jObject["error_code"].ToObject<int>() == 1000000)
                                                        {
                                                            pdd.addressid = jObject["result"].ToObject<JObject>()["address_id"].ToString();
                                                            addressid = pdd.addressid;
                                                            put("addressid：" + pdd.addressid);
                                                        }
                                                        else
                                                            put("用户地址获取失败：" + result);
                                                    }
                                                    if (!string.IsNullOrEmpty(addressid))
                                                    {
                                                        result = pddhttp.createOrder(secretKey, addressid, Util.getanicontent(), paycookie, userAgent);
                                                        if (Util.StrTojson(result, out jObject) && jObject["errorCode"].ToObject<int>() == 7000027)
                                                            put("支付成功：" + result);
                                                        else
                                                            put("支付失败：" + result);
                                                    }
                                                    else
                                                        put("支付失败，获取不到地址：" + result);
                                                    break;
                                                }
                                            }
                                            Thread.Sleep(1000);
                                            foreach (Mille mille in millelist)
                                            {
                                                result = pddhttp.milledeleteGoods(mille.goodid, cookie, userAgent);
                                                put("删除批发商品数据：" + result, true);
                                                if (!Util.StrTojson(result, out jObject) || !jObject["success"].ToObject<bool>() ||
                                                       !string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                                {
                                                    richTextBox_deletefail.AppendText("删除失败：" + mille.goodid + "\r\n");
                                                }
                                            }
                                            put("删除批发商品结束：" + id, true);
                                            millelist = new List<Mille>();
                                        }
                                        else
                                            put("分享订单失败：" + result, true);
                                    }
                                    else
                                        put("创建定向供货订单失败：" + result, true);
                                }
                            }
                        }
                    }

                }
                catch (Exception e1)
                {
                    stop = true;
                    put(e1.StackTrace + e1.Message);
                    Thread.Sleep(1000);
                    foreach (Mille mille in millelist)
                    {
                        result = pddhttp.milledeleteGoods(mille.goodid, cookie, userAgent);
                        put("程序出现异常，删除批发商品数据：" + result, true);
                        if (!Util.StrTojson(result, out JObject jObject) || !jObject["success"].ToObject<bool>() ||
                               !string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                        {
                            richTextBox_deletefail.AppendText(mille.goodid + "\r\n");
                        }
                    }
                    put("删除批发商品结束", true);
                }
                finally
                {
                    form2.button1.Enabled = true;
                    checkBox_autokucun.Enabled = true;
                    button2.Enabled = true;
                    foreach (Mille mille in millelist)
                    {
                        richTextBox_deletefail.AppendText("异常商品id：" + mille.goodid + "\r\n");
                    }
                    put("程序执行结束，最后一个商品id：" + id);
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
                new Thread(delegate ()
                {
                    string[] lines = richTextBox_ids.Lines;
                    foreach (string line in lines)
                    {
                        string[] idstemp = line.Trim().Split(' ');
                        foreach (string id in idstemp)
                        {
                            if (!string.IsNullOrWhiteSpace(id))
                            {
                                string result = pddhttp.milledeleteGoods(id, pdd.cookie, pdd.userAgent);
                                put("删除批发商品数据：" + result, true);
                                if (!Util.StrTojson(result, out JObject jObject) || !jObject["success"].ToObject<bool>() ||
                                       !string.IsNullOrEmpty(jObject["errorMsg"] + ""))
                                {
                                    put("删除批发商品失败：" + id, true);
                                }
                                else
                                {
                                    goodsset.Remove(id);
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

            //pictureBox2.Image = Image.FromFile("C:\\Users\\笔记本\\Desktop\\gaitubao_下载_jpg.jpg");
            //textBox_verifyCode.Text = Util.verCode();
            /*if (!textBox_verifyCode.Text.Equals("pkex"))
            {
                Util.ReportError(id);
            }*/
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.textBox_log.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定停止？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                stop = true;
                pddhttp.goodskucunlist = new ConcurrentQueue<string>();
                goodslist = new ConcurrentQueue<string>();
            }

        }

        private void checkBox_verifyCode_CheckedChanged(object sender, EventArgs e)
        {
            LinkService.autover = checkBox_verifyCode.Checked;
            textBox_verifyCode.ReadOnly = LinkService.autover;
        }

        private void checkBox_autokucun_CheckedChanged(object sender, EventArgs e)
        {
            LinkService.autokucun = checkBox_autokucun.Checked;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定清除缓存？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                pddhttp.goodskucunlist = new ConcurrentQueue<string>();
                goodslist = new ConcurrentQueue<string>();
                goodsset = new HashSet<string>();
            }


        }

        private void comboBox_payorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            LinkService.payorder = int.Parse(((System.Web.UI.WebControls.ListItem)comboBox_payorder.SelectedItem).Value);
            if (LinkService.payorder == 2)
            {
                textBox_log.AppendText("自动他付需要在当前电脑另开浏览器并登录账号");
                textBox_paypdduserid.Visible = true;
            }
            else
                textBox_paypdduserid.Visible = false;
        }

        private void richTextBox_ids_Enter(object sender, EventArgs e)
        {
            if ("商品id，多个空格隔开或者换行".Equals(richTextBox_ids.Text))
            {
                richTextBox_ids.Text = "";
                richTextBox_ids.ForeColor = Color.Black;
                form2.button1.Enabled = true;
            }
        }

        private void textBox_pdduserid_TextChanged(object sender, EventArgs e)
        {
            pdduserid = textBox_pdduserid.Text.Trim();
        }

        private void textBox_paypdduserid_TextChanged(object sender, EventArgs e)
        {
            paypdduserid = textBox_paypdduserid.Text.Trim();
        }

        private void textBox_pdduserid_Leave(object sender, EventArgs e)
        {
            LinkService.updatesystemconfig(LinkService.macid, pdduserid, paypdduserid);
        }

        private void textBox_paypdduserid_Leave(object sender, EventArgs e)
        {
            LinkService.updatesystemconfig(LinkService.macid, pdduserid, paypdduserid);
        }
        Thread updateoriderprice = null;
        private void button5_Click(object sender, EventArgs e)
        {
            if (updateoriderprice == null)
            {
                DialogResult dr = MessageBox.Show("确定修改所有订单价格？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    button_updateorderprice.Text = "停止修改价格";
                    updateoriderprice = new Thread(delegate ()
                    {
                        int fail = 0, success = 0;
                        try
                        {
                            int index = 1;
                            while (!Form1.isEnd && updateoriderprice != null)
                            {
                                string result = pddhttp.unpaidOrders(index, pdd.cookie);
                                Util.StrTojson(result, out JObject jObject);
                                put("商家订单列表：" + result, false);
                                if (jObject != null && jObject["total"].ToObject<int>() > 0 && jObject["orders"].ToObject<JArray>().Count > 0)
                                {
                                    if (index == 0)
                                        put("订单总数："+ jObject["total"].ToObject<int>());
                                    index++;
                                    JArray jArray = jObject["orders"].ToObject<JArray>();
                                    for (int i = 0; i < jArray.Count; i++)
                                    {
                                        JObject obj = jArray[i].ToObject<JObject>();
                                        if (obj["manualDiscount"].ToObject<int>() == 0)// 未改价
                                        {
                                            string id = obj["orderSn"].ToString();
                                            result = pddhttp.sendOrderUrge("{\"orderSn\": \"" + id + "\",\"urgeTypes\": [1],\"goodsDiscount\": \"" + (obj["orderAmount"].ToObject<int>() - 1) + "\" }", pdd.cookie);
                                            put("修改订单价格："+result, false);
                                            if (Util.StrTojson(result, out obj) && obj["success"].ToObject<bool>() && obj["errorCode"].ToObject<int>() == 1000000)
                                            {
                                                success++;
                                                put("订单修改成功"+ i + "：" + obj["result"].ToObject<JObject>()["orderSn"].ToString());
                                            }
                                            else
                                            {
                                                fail++;
                                                put("订单修改失败" + i + "：" + id);
                                            }
                                        }
                                    }
                                }
                                else if (jObject == null)
                                {
                                    put("获取商家订单失败：" + result);
                                    break;
                                }
                                else
                                {
                                    put("执行结束，page：" + index);
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                        }
                        catch (Exception e1)
                        {
                            put("修改订单价格异常：" + e1.Message + e1.StackTrace);
                        }
                        finally
                        {
                            put("执行结束,修改成功：" + success + "，修改失败：" + fail);
                            button_updateorderprice.Text = "启动修改价格";
                            updateoriderprice = null;
                        }

                    });
                    updateoriderprice.Start();
                }
            }
            else
            {
                DialogResult dr = MessageBox.Show("确定停止修改订单价格？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    updateoriderprice = null;
                }
            }

        }
    }
}
