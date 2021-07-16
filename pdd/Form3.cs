using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WechatRegster.listenes;

namespace pdd
{
   

    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void textBox_content_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox_size_TextChanged(object sender, EventArgs e)
        {
            int size = LinkService.caijisize;
            if (!string.IsNullOrWhiteSpace(textBox_size.Text) && int.TryParse(textBox_size.Text.Trim(), out int size1))
            {
                LinkService.caijisize = size1;
            }
            else
            {
                textBox_size.Text = size + "";
                MessageBox.Show("设置失败");
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            DataTable dataTable = LinkService.getsystemconfig(LinkService.macid);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                textBox_content.Text = dataTable.Rows[0][2] + "";
                textBox_size.Text = dataTable.Rows[0][3] + "";
                textBox_caijinewgoods.Text = dataTable.Rows[0][4] + "";
                string[] arr = (dataTable.Rows[0][2] + "").Split(' ');
                foreach (string s in arr)
                {
                    LinkService.caijicontent.Add(new caiji(s));
                }
                int.TryParse(dataTable.Rows[0][3] + "", out LinkService.caijisize);
                int.TryParse(dataTable.Rows[0][4] + "", out LinkService.caijinewgoods);
            }
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void textBox_content_Leave(object sender, EventArgs e)
        {
        }

        private void textBox_size_Leave(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LinkService.caijisize = int.Parse(textBox_size.Text.Trim());
            LinkService.caijinewgoods = int.Parse(textBox_size.Text.Trim());
            string[] arr = textBox_content.Text.Trim().Split(' ');
            LinkService.caijicontent = new List<caiji>();
            foreach (string s in arr)
            {
                if (!string.IsNullOrWhiteSpace(s))
                    LinkService.caijicontent.Add(new caiji(s));
            }
            LinkService.updatesystemconfig(LinkService.macid,string.Join(" ", LinkService.caijicontent) , LinkService.caijisize, LinkService.caijinewgoods);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
