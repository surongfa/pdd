namespace pdd
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.goodid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.goodurl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.img = new System.Windows.Forms.DataGridViewImageColumn();
            this.check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.button_next = new System.Windows.Forms.Button();
            this.textBox_find = new System.Windows.Forms.TextBox();
            this.button_find = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_prices = new System.Windows.Forms.TextBox();
            this.textBox_pricee = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_count = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_max = new System.Windows.Forms.TextBox();
            this.textBox_min = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_size = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label_ncount = new System.Windows.Forms.Label();
            this.dateTimePicker_start = new System.Windows.Forms.DateTimePicker();
            this.button_cleardata = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_id = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label_pl = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button_copy = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label_sales = new System.Windows.Forms.Label();
            this.label_price = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_plmax = new System.Windows.Forms.TextBox();
            this.textBox_plmin = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox_sleep = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.goodid,
            this.name,
            this.goodurl,
            this.price,
            this.sale,
            this.createtime,
            this.pd,
            this.pl,
            this.img,
            this.check});
            this.dataGridView1.Location = new System.Drawing.Point(12, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 60;
            this.dataGridView1.RowTemplate.Height = 100;
            this.dataGridView1.Size = new System.Drawing.Size(1167, 557);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            // 
            // goodid
            // 
            this.goodid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.goodid.HeaderText = "goodid";
            this.goodid.Name = "goodid";
            this.goodid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "名称";
            this.name.Name = "name";
            this.name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // goodurl
            // 
            this.goodurl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.goodurl.HeaderText = "商品链接";
            this.goodurl.Name = "goodurl";
            this.goodurl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // price
            // 
            this.price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.price.HeaderText = "价格";
            this.price.Name = "price";
            // 
            // sale
            // 
            this.sale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sale.HeaderText = "销量";
            this.sale.Name = "sale";
            // 
            // createtime
            // 
            this.createtime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.createtime.HeaderText = "创建时间";
            this.createtime.Name = "createtime";
            // 
            // pd
            // 
            this.pd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pd.HeaderText = "拼单";
            this.pd.Name = "pd";
            this.pd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pl
            // 
            this.pl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pl.HeaderText = "评价数";
            this.pl.Name = "pl";
            // 
            // img
            // 
            this.img.HeaderText = "图片";
            this.img.Name = "img";
            this.img.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.img.Width = 200;
            // 
            // check
            // 
            this.check.HeaderText = "标记";
            this.check.Name = "check";
            this.check.Width = 50;
            // 
            // button_next
            // 
            this.button_next.Location = new System.Drawing.Point(586, 600);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(91, 29);
            this.button_next.TabIndex = 1;
            this.button_next.Text = "下一页";
            this.button_next.UseVisualStyleBackColor = true;
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // textBox_find
            // 
            this.textBox_find.Location = new System.Drawing.Point(15, 4);
            this.textBox_find.Multiline = true;
            this.textBox_find.Name = "textBox_find";
            this.textBox_find.Size = new System.Drawing.Size(171, 31);
            this.textBox_find.TabIndex = 2;
            // 
            // button_find
            // 
            this.button_find.Location = new System.Drawing.Point(424, 4);
            this.button_find.Name = "button_find";
            this.button_find.Size = new System.Drawing.Size(75, 31);
            this.button_find.TabIndex = 3;
            this.button_find.Text = "查询";
            this.button_find.UseVisualStyleBackColor = true;
            this.button_find.Click += new System.EventHandler(this.button_find_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(694, 608);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "查询不到数据";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "价格";
            // 
            // textBox_prices
            // 
            this.textBox_prices.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_prices.Location = new System.Drawing.Point(227, 4);
            this.textBox_prices.Name = "textBox_prices";
            this.textBox_prices.Size = new System.Drawing.Size(75, 31);
            this.textBox_prices.TabIndex = 6;
            // 
            // textBox_pricee
            // 
            this.textBox_pricee.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_pricee.Location = new System.Drawing.Point(343, 4);
            this.textBox_pricee.Name = "textBox_pricee";
            this.textBox_pricee.Size = new System.Drawing.Size(72, 30);
            this.textBox_pricee.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(308, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "——";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(946, 609);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "总数：";
            // 
            // label_count
            // 
            this.label_count.AutoSize = true;
            this.label_count.Location = new System.Drawing.Point(993, 609);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(11, 12);
            this.label_count.TabIndex = 10;
            this.label_count.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(640, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "—";
            // 
            // textBox_max
            // 
            this.textBox_max.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_max.Location = new System.Drawing.Point(657, 5);
            this.textBox_max.Name = "textBox_max";
            this.textBox_max.Size = new System.Drawing.Size(72, 30);
            this.textBox_max.TabIndex = 13;
            this.textBox_max.Text = "80000";
            this.textBox_max.TextChanged += new System.EventHandler(this.textBox_max_TextChanged);
            // 
            // textBox_min
            // 
            this.textBox_min.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_min.Location = new System.Drawing.Point(564, 4);
            this.textBox_min.Name = "textBox_min";
            this.textBox_min.Size = new System.Drawing.Size(75, 31);
            this.textBox_min.TabIndex = 12;
            this.textBox_min.Text = "5000";
            this.textBox_min.TextChanged += new System.EventHandler(this.textBox_min_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(505, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "采集范围";
            // 
            // textBox_size
            // 
            this.textBox_size.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_size.Location = new System.Drawing.Point(523, 600);
            this.textBox_size.Name = "textBox_size";
            this.textBox_size.Size = new System.Drawing.Size(57, 29);
            this.textBox_size.TabIndex = 15;
            this.textBox_size.Text = "20";
            this.textBox_size.Leave += new System.EventHandler(this.textBox_size_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1047, 608);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "未采集数：";
            // 
            // label_ncount
            // 
            this.label_ncount.AutoSize = true;
            this.label_ncount.Location = new System.Drawing.Point(1118, 609);
            this.label_ncount.Name = "label_ncount";
            this.label_ncount.Size = new System.Drawing.Size(11, 12);
            this.label_ncount.TabIndex = 17;
            this.label_ncount.Text = "0";
            // 
            // dateTimePicker_start
            // 
            this.dateTimePicker_start.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dateTimePicker_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_start.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dateTimePicker_start.Location = new System.Drawing.Point(1030, 9);
            this.dateTimePicker_start.Name = "dateTimePicker_start";
            this.dateTimePicker_start.Size = new System.Drawing.Size(158, 21);
            this.dateTimePicker_start.TabIndex = 18;
            this.dateTimePicker_start.Value = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            // 
            // button_cleardata
            // 
            this.button_cleardata.Location = new System.Drawing.Point(951, 8);
            this.button_cleardata.Name = "button_cleardata";
            this.button_cleardata.Size = new System.Drawing.Size(75, 23);
            this.button_cleardata.TabIndex = 19;
            this.button_cleardata.Text = "清空数据";
            this.button_cleardata.UseVisualStyleBackColor = true;
            this.button_cleardata.Click += new System.EventHandler(this.button_cleardata_Click);
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label_id);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label_pl);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.button_copy);
            this.panel1.Controls.Add(this.button_close);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label_sales);
            this.panel1.Controls.Add(this.label_price);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label_name);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(189, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(751, 575);
            this.panel1.TabIndex = 20;
            this.panel1.UseWaitCursor = true;
            this.panel1.Visible = false;
            // 
            // label_id
            // 
            this.label_id.AutoSize = true;
            this.label_id.Font = new System.Drawing.Font("宋体", 12F);
            this.label_id.Location = new System.Drawing.Point(3, 14);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(32, 16);
            this.label_id.TabIndex = 13;
            this.label_id.Text = "...";
            this.label_id.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_id.UseWaitCursor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F);
            this.label9.Location = new System.Drawing.Point(443, 551);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 16);
            this.label9.TabIndex = 12;
            this.label9.Text = "评价数：";
            this.label9.UseWaitCursor = true;
            // 
            // label_pl
            // 
            this.label_pl.AutoSize = true;
            this.label_pl.Font = new System.Drawing.Font("宋体", 12F);
            this.label_pl.Location = new System.Drawing.Point(518, 551);
            this.label_pl.Name = "label_pl";
            this.label_pl.Size = new System.Drawing.Size(16, 16);
            this.label_pl.TabIndex = 11;
            this.label_pl.Text = "0";
            this.label_pl.UseWaitCursor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(705, 213);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "标记";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.UseWaitCursor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button_copy
            // 
            this.button_copy.Location = new System.Drawing.Point(663, 544);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(85, 27);
            this.button_copy.TabIndex = 9;
            this.button_copy.Text = "复制商品链接";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.UseWaitCursor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(673, 3);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 27);
            this.button_close.TabIndex = 1;
            this.button_close.Text = "关闭";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.UseWaitCursor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 12F);
            this.label11.Location = new System.Drawing.Point(301, 551);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 16);
            this.label11.TabIndex = 8;
            this.label11.Text = "销量：";
            this.label11.UseWaitCursor = true;
            // 
            // label_sales
            // 
            this.label_sales.AutoSize = true;
            this.label_sales.Font = new System.Drawing.Font("宋体", 12F);
            this.label_sales.Location = new System.Drawing.Point(366, 551);
            this.label_sales.Name = "label_sales";
            this.label_sales.Size = new System.Drawing.Size(16, 16);
            this.label_sales.TabIndex = 7;
            this.label_sales.Text = "0";
            this.label_sales.UseWaitCursor = true;
            // 
            // label_price
            // 
            this.label_price.AutoSize = true;
            this.label_price.Font = new System.Drawing.Font("宋体", 12F);
            this.label_price.Location = new System.Drawing.Point(232, 551);
            this.label_price.Name = "label_price";
            this.label_price.Size = new System.Drawing.Size(16, 16);
            this.label_price.TabIndex = 6;
            this.label_price.Text = "0";
            this.label_price.UseWaitCursor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F);
            this.label8.Location = new System.Drawing.Point(176, 551);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 16);
            this.label8.TabIndex = 5;
            this.label8.Text = "价格：";
            this.label8.UseWaitCursor = true;
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Font = new System.Drawing.Font("宋体", 12F);
            this.label_name.Location = new System.Drawing.Point(117, 14);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(32, 16);
            this.label_name.TabIndex = 4;
            this.label_name.Text = "...";
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_name.UseWaitCursor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.Location = new System.Drawing.Point(712, 235);
            this.button2.Name = "button2";
            this.button2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button2.Size = new System.Drawing.Size(25, 73);
            this.button2.TabIndex = 3;
            this.button2.Text = ">";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.UseWaitCursor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.Location = new System.Drawing.Point(15, 235);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button1.Size = new System.Drawing.Size(25, 73);
            this.button1.TabIndex = 2;
            this.button1.Text = "<";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.UseWaitCursor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(50, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(653, 503);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.UseWaitCursor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 606);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "导出查询数据";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(109, 606);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 23);
            this.button4.TabIndex = 22;
            this.button4.Text = "导出非标记数据";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(859, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 26;
            this.label10.Text = "—";
            // 
            // textBox_plmax
            // 
            this.textBox_plmax.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_plmax.Location = new System.Drawing.Point(876, 5);
            this.textBox_plmax.Name = "textBox_plmax";
            this.textBox_plmax.Size = new System.Drawing.Size(72, 30);
            this.textBox_plmax.TabIndex = 25;
            this.textBox_plmax.TextChanged += new System.EventHandler(this.textBox_plmax_TextChanged);
            // 
            // textBox_plmin
            // 
            this.textBox_plmin.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_plmin.Location = new System.Drawing.Point(783, 5);
            this.textBox_plmin.Name = "textBox_plmin";
            this.textBox_plmin.Size = new System.Drawing.Size(75, 31);
            this.textBox_plmin.TabIndex = 24;
            this.textBox_plmin.TextChanged += new System.EventHandler(this.textBox_plmin_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(736, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 23;
            this.label12.Text = "评价数";
            // 
            // textBox_log
            // 
            this.textBox_log.Location = new System.Drawing.Point(232, 126);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_log.Size = new System.Drawing.Size(726, 384);
            this.textBox_log.TabIndex = 128;
            this.textBox_log.Visible = false;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(220, 601);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 27);
            this.button5.TabIndex = 129;
            this.button5.Text = "日志";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.UseWaitCursor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox_sleep
            // 
            this.textBox_sleep.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_sleep.Location = new System.Drawing.Point(301, 600);
            this.textBox_sleep.Name = "textBox_sleep";
            this.textBox_sleep.Size = new System.Drawing.Size(57, 29);
            this.textBox_sleep.TabIndex = 130;
            this.textBox_sleep.Text = "500";
            this.textBox_sleep.TextChanged += new System.EventHandler(this.textBox_sleep_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 636);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label_ncount);
            this.Controls.Add(this.textBox_sleep);
            this.Controls.Add(this.textBox_log);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox_plmax);
            this.Controls.Add(this.textBox_plmin);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button_cleardata);
            this.Controls.Add(this.dateTimePicker_start);
            this.Controls.Add(this.textBox_size);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox_max);
            this.Controls.Add(this.textBox_min);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label_count);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_pricee);
            this.Controls.Add(this.textBox_prices);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_find);
            this.Controls.Add(this.textBox_find);
            this.Controls.Add(this.button_next);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.TextBox textBox_find;
        private System.Windows.Forms.Button button_find;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_prices;
        private System.Windows.Forms.TextBox textBox_pricee;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_max;
        private System.Windows.Forms.TextBox textBox_min;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_size;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_ncount;
        private System.Windows.Forms.DateTimePicker dateTimePicker_start;
        private System.Windows.Forms.Button button_cleardata;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label_sales;
        private System.Windows.Forms.Label label_price;
        private System.Windows.Forms.Button button_copy;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridViewTextBoxColumn goodid;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn goodurl;
        private System.Windows.Forms.DataGridViewTextBoxColumn price;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale;
        private System.Windows.Forms.DataGridViewTextBoxColumn createtime;
        private System.Windows.Forms.DataGridViewTextBoxColumn pd;
        private System.Windows.Forms.DataGridViewTextBoxColumn pl;
        private System.Windows.Forms.DataGridViewImageColumn img;
        private System.Windows.Forms.DataGridViewCheckBoxColumn check;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label_pl;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_plmax;
        private System.Windows.Forms.TextBox textBox_plmin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label_id;
        public System.Windows.Forms.TextBox textBox_log;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox_sleep;
    }
}

