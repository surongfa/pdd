using HttpListenerPost;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WechatRegster;
using WechatRegster.listenes;

namespace pdd
{
    public partial class Form1 : Form
    {
        public static bool isEnd = false;
        public static bool isClose = false;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            x = this.Width;
            y = this.Height;
            setTag(this);
            if (!Directory.Exists(LinkService.path)) // 创建父目录
                Directory.CreateDirectory(LinkService.path);
        }
        private Dictionary<int, HttpListenerHandler> handlerMap = new Dictionary<int, HttpListenerHandler>();
        private void Form1_Load(object sender, EventArgs e)
        {
            isEnd = false;
            LinkService.form1 = this;
            Logger.getLogger().close(true);
            if("笔记本".Equals(Environment.UserName))
                LinkService.dbConn = new DBConnection("Data Source=127.0.0.1;Database=pdd;User ID=root;Password=smartbi;Charset=utf8; Pooling=true;Allow User Variables=True");
            else
                LinkService.dbConn = new DBConnection("Data Source=127.0.0.1;Database=pdd;User ID=root;Password=root;Charset=utf8; Pooling=true;Allow User Variables=True");
            LinkService.min = int.Parse(textBox_min.Text.Trim());
            LinkService.max = int.Parse(textBox_max.Text.Trim());
            LinkService.size = int.Parse(textBox_size.Text.Trim());
            LinkService.updatepls();
            new Thread(delegate ()
            {
                while (!isEnd)
                {
                    label_ncount.Text = LinkService.getcount("select count(*) from pdd_goods where details_status=0 and sales >= " + LinkService.min + " and sales<=" + LinkService.max).ToString();
                    Thread.Sleep(5000);
                }
            }).Start();
            HttpListenerHandler handler = handlerMap.ContainsKey(Service.httpListenerLinkPort) ? handlerMap[Service.httpListenerLinkPort] : null;

            if (handler == null)
            {
                handler = new HttpListenerHandler(Service.httpListenerLinkPort);
                handlerMap.Add(Service.httpListenerLinkPort, handler);
            }
            handler.startListener(Service.httpListenerLinkPort);

            dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.RowTemplate.Height = 200;
            getgoods();
            //this.dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);//自动调整列宽
            //this.dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);

        }
        public goods getgoodsbyindex(int index = 0, string order = null)
        {
            order = order ?? "id";
            double s = string.IsNullOrEmpty(textBox_prices.Text.Trim()) ? -1 : double.Parse(textBox_prices.Text.Trim());
            double e = string.IsNullOrEmpty(textBox_pricee.Text.Trim()) ? -1 : double.Parse(textBox_pricee.Text.Trim());
            DataTable dataTable = LinkService.getgoodsbyindex(index, DateTime.Parse(dateTimePicker_start.Text), textBox_find.Text.Trim(), order, 1, s, e, LinkService.min, LinkService.max, LinkService.plmin, LinkService.plmax);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                goods goods = new goods();
                goods.goods_id = dataTable.Rows[0][0] + "";
                goods.goods_name = dataTable.Rows[0][1] + "";
                goods.goods_url = dataTable.Rows[0][3] + "";
                goods.price = double.Parse(dataTable.Rows[0][4] + "");
                goods.sales = (int)dataTable.Rows[0][5];
                goods.create_time = (int)dataTable.Rows[0][6];
                goods.pd = dataTable.Rows[0][7] + "";
                goods.pl = dataTable.Rows[0][8] + "";
                goods.check = (int)dataTable.Rows[0][9];
                if (!File.Exists(LinkService.path + dataTable.Rows[0][0] + ".jpg"))
                {
                    LinkService.getimg(dataTable.Rows[0][0] + "", dataTable.Rows[0][2] + "");
                }
                return goods;
            }
            return null;
        }
        public void getgoods(int page = 0, string order = null)
        {
            order = order ?? "id";
            double s = string.IsNullOrEmpty(textBox_prices.Text.Trim()) ? -1 : double.Parse(textBox_prices.Text.Trim());
            double e = string.IsNullOrEmpty(textBox_pricee.Text.Trim()) ? -1 : double.Parse(textBox_pricee.Text.Trim());
            DataTable dataTable = LinkService.getgoods(page, out int count, DateTime.Parse(dateTimePicker_start.Text), textBox_find.Text.Trim(), order, LinkService.size, s, e, LinkService.min, LinkService.max, LinkService.plmin, LinkService.plmax);
            label_count.Text = count.ToString();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                label1.Visible = false;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int index = dataGridView1.Rows.Add();
                    //if((int)dataTable.Rows[i][9] == 1)
                    //    this.dataGridView1.Rows[index].HeaderCell.InheritedStyle.BackColor = Color.Red;
                    this.dataGridView1.Rows[index].HeaderCell.Value = (index + 1).ToString();
                    //this.dataGridView1.Rows[index].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    this.dataGridView1.Rows[index].Cells[0].Value = dataTable.Rows[i][0];
                    this.dataGridView1.Rows[index].Cells[1].Value = dataTable.Rows[i][1];
                    this.dataGridView1.Rows[index].Cells[2].Value = dataTable.Rows[i][3];
                    this.dataGridView1.Rows[index].Cells[3].Value = dataTable.Rows[i][4];
                    this.dataGridView1.Rows[index].Cells[4].Value = dataTable.Rows[i][5];
                    this.dataGridView1.Rows[index].Cells[5].Value = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddSeconds((int)dataTable.Rows[i][6]).ToString();
                    this.dataGridView1.Rows[index].Cells[6].Value = dataTable.Rows[i][7];
                    this.dataGridView1.Rows[index].Cells[7].Value = dataTable.Rows[i][8];
                    if (!File.Exists(LinkService.path + dataTable.Rows[i][0] + ".jpg"))
                    {
                        LinkService.getimg(dataTable.Rows[i][0] + "", dataTable.Rows[i][2] + "");
                    }
                    this.dataGridView1.Rows[index].Cells[8].Value = Image.FromFile(LinkService.path + dataTable.Rows[i][0] + ".jpg");
                    this.dataGridView1.Rows[index].Cells[9].Value = (int)dataTable.Rows[i][9] == 1;
                }

            }
            else
                label1.Visible = true;
        }

        #region 控件大小随窗体大小等比例缩放
        private float x;//定义当前窗体的宽度
        private float y;//定义当前窗体的高度
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (x != 0)
            {
                float newx = (this.Width) / x;
                float newy = (this.Height) / y;
                setControls(newx, newy, this);
            }
        }
        #endregion

        public static int page;
        public static string orderby;
        private void button_next_Click(object sender, EventArgs e)
        {
            page++;
            getgoods(page, orderby);
        }

        private void button_find_Click(object sender, EventArgs e)
        {
            string[] cookies = "nullacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636630933; JSESSIONID=DA30F5D8CB8FE6646B2C3879769130CBacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636681281; JSESSIONID=471F5625276B2C9BD7DE52A85D177FF6acid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636798545; JSESSIONID=F6BB2ECF6D690E75439CB70C8F7F152Eacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636851977; JSESSIONID=C2A95F1178D029481B88498FD584B382acid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636891290; JSESSIONID=661ABE30FAE86BD4609A3897ACFD7C2Aacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; JSESSIONID=E8F1137AE8C389235B14D08E9F1B2E27; PASS_ID=1-KutZLR7HJKe09BBFK84ujLPSnmPB+LxgVoU+7DNyW9PwzFAI6V25WGRoxZyBlmcAqC89vb6Zn0GYzrI7ZUD8MQ_840969999_95786252; x-visit-time=1625715569662acid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-KutZLR7HJKe09BBFK84ujLPSnmPB+LxgVoU+7DNyW9PwzFAI6V25WGRoxZyBlmcAqC89vb6Zn0GYzrI7ZUD8MQ_840969999_95786252; x-visit-time=1625716404316; JSESSIONID=3AD0529A97BFDCD29DBF1E89496EB4FFacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-KutZLR7HJKe09BBFK84ujLPSnmPB+LxgVoU+7DNyW9PwzFAI6V25WGRoxZyBlmcAqC89vb6Zn0GYzrI7ZUD8MQ_840969999_95786252; x-visit-time=1625716476381; JSESSIONID=73BD6C0826162490625D540301D5ED6D".Split(';');
            string cookie = "nullacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636630933; JSESSIONID=DA30F5D8CB8FE6646B2C3879769130CBacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636681281; JSESSIONID=471F5625276B2C9BD7DE52A85D177FF6acid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636798545; JSESSIONID=F6BB2ECF6D690E75439CB70C8F7F152Eacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636851977; JSESSIONID=C2A95F1178D029481B88498FD584B382acid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-CVNbXRUzfBbQ4AUzPqxF4B0XEVfvOWdCbbEl6IOFzvwIpmQtD/DtCQs7meraBrbtxSEOIkyEX2xiOPHcmxAJcw_840969999_95786252; x-visit-time=1625636891290; JSESSIONID=661ABE30FAE86BD4609A3897ACFD7C2Aacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; JSESSIONID=E8F1137AE8C389235B14D08E9F1B2E27; PASS_ID=1-KutZLR7HJKe09BBFK84ujLPSnmPB+LxgVoU+7DNyW9PwzFAI6V25WGRoxZyBlmcAqC89vb6Zn0GYzrI7ZUD8MQ_840969999_95786252; x-visit-time=1625715569662acid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac; _a42=4457d935-ccfa-4af4-92e3-4bd0054d56b0; rcgk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; rckk=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; ru1k=cc529979-a6e4-47f6-9248-0bff137f15ac; ru2k=4457d935-ccfa-4af4-92e3-4bd0054d56b0; mms_b84d1838=3414,120,3397,3434,3432,1202,1203,1204,1205; PASS_ID=1-KutZLR7HJKe09BBFK84ujLPSnmPB+LxgVoU+7DNyW9PwzFAI6V25WGRoxZyBlmcAqC89vb6Zn0GYzrI7ZUD8MQ_840969999_95786252; x-visit-time=1625716404316; JSESSIONID=3AD0529A97BFDCD29DBF1E89496EB4FFacid=9e423077f5b9db7a5f4828666c2b5af5; api_uid=Ck625GDhjqkMpQBmJv18Ag==; _nano_fp=XpExnpXbnpPaX0Xbl9_0KXP3X3vtEgCXhsHgYPEs; jrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; njrpl=VND7mYWPVpsnyARHJSICnSwVMRJ3g8Oi; dilx=f64XYN8OrGepJVbqmTdSi; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F86.0.4240.198%20Safari%2F537.36; webp=1; PDDAccessToken=5F2EHL6L52T32FBUEQVGQ6DNSFMHWMWSABEMM3FM26UHGE5QO3KQ112ff7c; pdd_user_id=2700180776703; pdd_user_uin=E27CR7EFCNPPVS54FJAHSWKSJY_GEXDA; JSESSIONID=A5141EBD4DEE8564ABAC7881A0933904; pdd_vds=gaxLlInPsyLLLIxaxQltENOmImsLoiIiltwtILLaOboyxtsEEEmQnmLPniyN; _nano_fp=XpExnpdjn0XoXqTjX9_YJrIl~sKdVDBOOmM4fHEu; _nano_fp=XpExnpdjn0XxX5Ebn9_vxhSwWvwiiXmDjl_SYwAK; _crr=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _bee=IEQKICaIUsqxsZ4yaHRp7USuTb1J58Nu; _f77=cc529979-a6e4-47f6-9248-0bff137f15ac;";
            int start = cookie.IndexOf("PASS_ID=");
            if (start > 0)
            {
                string passid = cookie.Substring(start, cookie.IndexOf("; ", start) - start);
                passid = passid.Substring(passid.LastIndexOf("_")+1);
                Console.WriteLine(passid);
            }
            Console.WriteLine(Environment.UserName);
            page = 0;
            dataGridView1.Rows.Clear();
            getgoods();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            page = 0;
            dataGridView1.Rows.Clear();
            SortOrder sortOrder = dataGridView1.SortOrder;
            string order = sortOrder == SortOrder.Descending ? "desc" : "";
            DataGridViewColumn sortColumn = this.dataGridView1.SortedColumn;
            string key = null;
            if (sortColumn != null)
            {
                if (sortColumn.Index == 3)
                    key = "price " + order;
                else if (sortColumn.Index == 4)
                    key = "sales " + order;
                else if (sortColumn.Index == 5)
                    key = "create_time " + order;
                else if (sortColumn.Index == 7)
                    key = "pls " + order;
            }
            orderby = key;
            getgoods(page, key);


            /*
                        DataGridViewColumnSortMode mode = dataGridView1.Columns[3].SortMode;
                        if (mode == DataGridViewColumnSortMode.Automatic)
                            key = "price,";
                        else if (mode == DataGridViewColumnSortMode.Programmatic)
                            key = "price desc,";
                        mode = dataGridView1.Columns[4].SortMode;
                        if (mode == DataGridViewColumnSortMode.Automatic)
                            key = key + "sales,";
                        else if (mode == DataGridViewColumnSortMode.Programmatic)
                            key = key + "sales desc,";
                        mode = dataGridView1.Columns[5].SortMode;
                        if (mode == DataGridViewColumnSortMode.Automatic)
                            key = key + "create_time,";
                        else if (mode == DataGridViewColumnSortMode.Programmatic)
                            key = key + "create_time desc,";

                        if (key == null)
                            getgoods(page);
                        else
                            getgoods(page, key.Substring(0, key.Length - 1));
            */

        }

        private void textBox_size_Leave(object sender, EventArgs e)
        {
            int size = LinkService.size;
            if (!string.IsNullOrWhiteSpace(textBox_size.Text) && int.TryParse(textBox_size.Text.Trim(), out int size1))
            {
                page = 0;
                LinkService.size = size1;
            }
            else
            {
                textBox_size.Text = size + "";
                MessageBox.Show("设置失败");
            }
        }

        private void textBox_min_TextChanged(object sender, EventArgs e)
        {
            int min = LinkService.min;
            if (!string.IsNullOrWhiteSpace(textBox_min.Text) && int.TryParse(textBox_min.Text.Trim(), out int val))
            {
                LinkService.min = val;
            }
            else
            {
                textBox_min.Text = min + "";
                MessageBox.Show("设置失败");
            }
        }

        private void textBox_max_TextChanged(object sender, EventArgs e)
        {
            int max = LinkService.max;
            if (!string.IsNullOrWhiteSpace(textBox_max.Text) && int.TryParse(textBox_max.Text.Trim(), out int val))
            {
                LinkService.max = val;
            }
            else
            {
                textBox_max.Text = max + "";
                MessageBox.Show("设置失败");
            }
        }

        private void button_cleardata_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定清空所选日期前所有数据吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                LinkService.delete((DateTime.Parse(dateTimePicker_start.Text) - startTime).TotalSeconds);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClose = true;
            isEnd = true;
            this.Controls.Clear();
            HttpListenerHandler handler = handlerMap.ContainsKey(Service.httpListenerLinkPort) ? handlerMap[Service.httpListenerLinkPort] : null;

            if (handler != null)
                handler.stoptListener(Service.httpListenerLinkPort);
            Logger.getLogger().close(false);
        }
        public goods select = null;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8)
            {
                panel1.Visible = true;
                select = getgoodsbyindex(e.RowIndex, orderby);
                if (select != null)
                {
                    select.index = e.RowIndex;
                    pictureBox1.Image = Image.FromFile(LinkService.path + select.goods_id + ".jpg");
                    setpanel();
                }
            }
            else if (e.ColumnIndex == 9)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                LinkService.updatecheck(id, (bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue ? 1 : 0);
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            button2.Enabled = false;
            if (select != null)
            {
                goods goods = getgoodsbyindex(select.index + 1, orderby);
                if (goods != null)
                {
                    goods.index = select.index + 1;
                    pictureBox1.Image = Image.FromFile(LinkService.path + goods.goods_id + ".jpg");
                    select = goods;
                    setpanel();
                }
                else
                    MessageBox.Show("已经是最后一条数据");
            }
            button2.Enabled = true;
        }
        public void setpanel()
        {
            label_name.Text = select.goods_name;
            label_price.Text = select.price.ToString();
            label_sales.Text = select.sales.ToString();
            label_pl.Text = select.pl;
            label_id.Text = select.goods_id;
            checkBox1.Checked = select.check == 1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (select != null)
            {
                if (select.index == 0)
                    MessageBox.Show("当前为第一条数据");
                else
                {
                    goods goods = getgoodsbyindex(select.index - 1, orderby);
                    if (goods != null)
                    {
                        goods.index = select.index - 1;
                        pictureBox1.Image = Image.FromFile(LinkService.path + goods.goods_id + ".jpg");
                        select = goods;
                        setpanel();
                    }
                }
            }
            button1.Enabled = true;
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(select.goods_url);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            select.check = checkBox1.Checked ? 1 : 0;
            LinkService.updatecheck(select.goods_id, select.check);
        }

        private void button3_Click(object sender, EventArgs e1)
        {
            string order = orderby ?? "id";
            double s = string.IsNullOrEmpty(textBox_prices.Text.Trim()) ? -1 : double.Parse(textBox_prices.Text.Trim());
            double e = string.IsNullOrEmpty(textBox_pricee.Text.Trim()) ? -1 : double.Parse(textBox_pricee.Text.Trim());
            DataTable dataTable = LinkService.getgoods(0, out int count, DateTime.Parse(dateTimePicker_start.Text), textBox_find.Text.Trim(), order, 1000000, s, e, LinkService.min, LinkService.max, LinkService.plmin, LinkService.plmax);
            export(dataTable, "查询数据");
        }

        public void export(DataTable dataTable, string filename)
        {
            try
            {
                string importExcelPath = Environment.CurrentDirectory + "\\import.xlsx";
                string exportExcelPath = Environment.CurrentDirectory + "\\logger\\" + filename + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".xlsx";
                IWorkbook workbook = WorkbookFactory.Create(importExcelPath);
                ISheet sheet = workbook.GetSheetAt(0);//获取第一个工作薄
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    IRow row = sheet.CreateRow(0);
                    row.CreateCell(0).SetCellValue("编号");
                    row.CreateCell(1).SetCellValue("商品标题");
                    row.CreateCell(2).SetCellValue("图片链接");
                    row.CreateCell(3).SetCellValue("商品链接");
                    row.CreateCell(4).SetCellValue("价格");
                    row.CreateCell(5).SetCellValue("销量");
                    row.CreateCell(6).SetCellValue("拼单");
                    row.CreateCell(7).SetCellValue("评价数");
                    row.CreateCell(8).SetCellValue("创建时间");
                    row.CreateCell(9).SetCellValue("是否标记");
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        row = sheet.CreateRow(i + 1);
                        //IRow row = (IRow)sheet.GetRow(i);//获取第一行
                        row.CreateCell(0).SetCellValue(dataTable.Rows[i][0] + "");
                        row.CreateCell(1).SetCellValue(dataTable.Rows[i][1] + "");
                        row.CreateCell(2).SetCellValue(dataTable.Rows[i][2] + "");
                        row.CreateCell(3).SetCellValue(dataTable.Rows[i][3] + "");
                        row.CreateCell(4).SetCellValue(double.Parse(dataTable.Rows[i][4] + ""));
                        row.CreateCell(5).SetCellValue((int)dataTable.Rows[i][5]);
                        row.CreateCell(6).SetCellValue(dataTable.Rows[i][7] + "");
                        row.CreateCell(7).SetCellValue(dataTable.Rows[i][8] + "");
                        row.CreateCell(8).SetCellValue(TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddSeconds((int)dataTable.Rows[i][6]).ToString());
                        row.CreateCell(9).SetCellValue((int)dataTable.Rows[i][9] == 1 ? "是" : "否");
                    }
                    //导出excel
                    FileStream fs = new FileStream(exportExcelPath, FileMode.Create, FileAccess.ReadWrite);
                    workbook.Write(fs);
                    fs.Close();
                    MessageBox.Show("导出成功，路径："+exportExcelPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e1)
        {
            string order = orderby ?? "id";
            double s = string.IsNullOrEmpty(textBox_prices.Text.Trim()) ? -1 : double.Parse(textBox_prices.Text.Trim());
            double e = string.IsNullOrEmpty(textBox_pricee.Text.Trim()) ? -1 : double.Parse(textBox_pricee.Text.Trim());
            DataTable dataTable = LinkService.getgoods(0, out int count, DateTime.Parse(dateTimePicker_start.Text), textBox_find.Text.Trim(), order, 1000000, s, e, LinkService.min, LinkService.max, LinkService.plmin, LinkService.plmax, 0);
            export(dataTable, "非标记数据");
        }

        private void textBox_plmin_TextChanged(object sender, EventArgs e)
        {
            int min = LinkService.plmin;
            if (string.IsNullOrWhiteSpace(textBox_plmin.Text))
            {
                LinkService.plmin = -1;
                return;
            }
            if (!string.IsNullOrWhiteSpace(textBox_plmin.Text) && int.TryParse(textBox_plmin.Text.Trim(), out int val))
            {
                LinkService.plmin = val;
            }
            else
            {
                textBox_plmin.Text = min + "";
                MessageBox.Show("设置失败");
            }
        }

        private void textBox_plmax_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_plmax.Text))
            {
                LinkService.plmax = -1;
                return;
            }
            int plmax = LinkService.plmax;
            if (!string.IsNullOrWhiteSpace(textBox_plmax.Text) && int.TryParse(textBox_plmax.Text.Trim(), out int val))
            {
                LinkService.plmax = val;
            }
            else
            {
                textBox_plmax.Text = plmax + "";
                MessageBox.Show("设置失败");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox_log.Visible = !textBox_log.Visible;
        }

        private void textBox_sleep_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_sleep.Text))
            {
                LinkService.sleep = 500;
                return;
            }
            int sleep = LinkService.sleep;
            if (!string.IsNullOrWhiteSpace(textBox_sleep.Text) && int.TryParse(textBox_sleep.Text.Trim(), out int val))
            {
                LinkService.sleep = val;
            }
            else
            {
                textBox_sleep.Text = sleep + "";
                MessageBox.Show("设置失败");
            }
            
        }
    }
}
