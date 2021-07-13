namespace pdd
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox_ids = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.button_add = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox_pricesize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_verifyCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBox_deletefail = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_index = new System.Windows.Forms.Label();
            this.label_count = new System.Windows.Forms.Label();
            this.label_id = new System.Windows.Forms.Label();
            this.checkBox_verifyCode = new System.Windows.Forms.CheckBox();
            this.checkBox_autokucun = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_payorder = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox_ids
            // 
            this.richTextBox_ids.Location = new System.Drawing.Point(2, 34);
            this.richTextBox_ids.Name = "richTextBox_ids";
            this.richTextBox_ids.Size = new System.Drawing.Size(146, 349);
            this.richTextBox_ids.TabIndex = 1;
            this.richTextBox_ids.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(-1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "商品id，多个空格隔开";
            // 
            // textBox_log
            // 
            this.textBox_log.Location = new System.Drawing.Point(154, 34);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_log.Size = new System.Drawing.Size(601, 484);
            this.textBox_log.TabIndex = 129;
            // 
            // button_add
            // 
            this.button_add.Font = new System.Drawing.Font("宋体", 12F);
            this.button_add.Location = new System.Drawing.Point(2, 389);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(146, 33);
            this.button_add.TabIndex = 130;
            this.button_add.Text = "添加";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F);
            this.button1.Location = new System.Drawing.Point(2, 428);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 36);
            this.button1.TabIndex = 131;
            this.button1.Text = "启动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(166, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(126, 26);
            this.button2.TabIndex = 132;
            this.button2.Text = "删除批发商品";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(761, 34);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(172, 149);
            this.pictureBox1.TabIndex = 133;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // textBox_pricesize
            // 
            this.textBox_pricesize.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_pricesize.Location = new System.Drawing.Point(808, 469);
            this.textBox_pricesize.Name = "textBox_pricesize";
            this.textBox_pricesize.Size = new System.Drawing.Size(125, 26);
            this.textBox_pricesize.TabIndex = 136;
            this.textBox_pricesize.Text = "200000";
            this.textBox_pricesize.Leave += new System.EventHandler(this.textBox_pricesize_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(761, 474);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 137;
            this.label3.Text = "总价：";
            // 
            // textBox_verifyCode
            // 
            this.textBox_verifyCode.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_verifyCode.Location = new System.Drawing.Point(829, 248);
            this.textBox_verifyCode.Name = "textBox_verifyCode";
            this.textBox_verifyCode.ReadOnly = true;
            this.textBox_verifyCode.Size = new System.Drawing.Size(104, 26);
            this.textBox_verifyCode.TabIndex = 138;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(761, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 139;
            this.label4.Text = "验证码：";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(761, 280);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 23);
            this.button3.TabIndex = 140;
            this.button3.Text = "确认";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(761, 189);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(172, 53);
            this.pictureBox2.TabIndex = 141;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Font = new System.Drawing.Font("宋体", 10F);
            this.label5.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label5.Location = new System.Drawing.Point(764, 506);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 142;
            this.label5.Text = "清除日志";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // richTextBox_deletefail
            // 
            this.richTextBox_deletefail.Location = new System.Drawing.Point(774, 309);
            this.richTextBox_deletefail.Name = "richTextBox_deletefail";
            this.richTextBox_deletefail.Size = new System.Drawing.Size(146, 137);
            this.richTextBox_deletefail.TabIndex = 143;
            this.richTextBox_deletefail.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(806, 449);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 144;
            this.label2.Text = "异常批发商品id";
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("宋体", 12F);
            this.button4.Location = new System.Drawing.Point(2, 470);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(146, 36);
            this.button4.TabIndex = 145;
            this.button4.Text = "停止";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(733, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 146;
            this.label6.Text = "总数：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(834, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 148;
            this.label7.Text = "当前：";
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(881, 13);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(11, 12);
            this.label_index.TabIndex = 149;
            this.label_index.Text = "0";
            // 
            // label_count
            // 
            this.label_count.AutoSize = true;
            this.label_count.Location = new System.Drawing.Point(780, 13);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(11, 12);
            this.label_count.TabIndex = 147;
            this.label_count.Text = "0";
            // 
            // label_id
            // 
            this.label_id.AutoSize = true;
            this.label_id.Location = new System.Drawing.Point(12, 511);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(0, 12);
            this.label_id.TabIndex = 150;
            // 
            // checkBox_verifyCode
            // 
            this.checkBox_verifyCode.AutoSize = true;
            this.checkBox_verifyCode.Checked = true;
            this.checkBox_verifyCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_verifyCode.Location = new System.Drawing.Point(888, 284);
            this.checkBox_verifyCode.Name = "checkBox_verifyCode";
            this.checkBox_verifyCode.Size = new System.Drawing.Size(48, 16);
            this.checkBox_verifyCode.TabIndex = 151;
            this.checkBox_verifyCode.Text = "打码";
            this.checkBox_verifyCode.UseVisualStyleBackColor = true;
            this.checkBox_verifyCode.CheckedChanged += new System.EventHandler(this.checkBox_verifyCode_CheckedChanged);
            // 
            // checkBox_autokucun
            // 
            this.checkBox_autokucun.AutoSize = true;
            this.checkBox_autokucun.Checked = true;
            this.checkBox_autokucun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_autokucun.Location = new System.Drawing.Point(298, 9);
            this.checkBox_autokucun.Name = "checkBox_autokucun";
            this.checkBox_autokucun.Size = new System.Drawing.Size(96, 16);
            this.checkBox_autokucun.TabIndex = 152;
            this.checkBox_autokucun.Text = "库存自动修改";
            this.checkBox_autokucun.UseVisualStyleBackColor = true;
            this.checkBox_autokucun.CheckedChanged += new System.EventHandler(this.checkBox_autokucun_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label8.Font = new System.Drawing.Font("宋体", 10F);
            this.label8.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label8.Location = new System.Drawing.Point(881, 506);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 153;
            this.label8.Text = "清除缓存";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // comboBox_payorder
            // 
            this.comboBox_payorder.Font = new System.Drawing.Font("宋体", 12F);
            this.comboBox_payorder.FormattingEnabled = true;
            this.comboBox_payorder.Location = new System.Drawing.Point(637, 6);
            this.comboBox_payorder.Name = "comboBox_payorder";
            this.comboBox_payorder.Size = new System.Drawing.Size(90, 24);
            this.comboBox_payorder.TabIndex = 154;
            this.comboBox_payorder.SelectedIndexChanged += new System.EventHandler(this.comboBox_payorder_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(566, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 155;
            this.label9.Text = "支付处理：";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 530);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBox_payorder);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBox_autokucun);
            this.Controls.Add(this.checkBox_verifyCode);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label_index);
            this.Controls.Add(this.label_count);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox_deletefail);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox_verifyCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_pricesize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.textBox_log);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_ids);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form2";
            this.Text = "供货管理";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_ids;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBox_log;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox_pricesize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_verifyCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.Label label_id;
        private System.Windows.Forms.CheckBox checkBox_verifyCode;
        private System.Windows.Forms.CheckBox checkBox_autokucun;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_payorder;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.RichTextBox richTextBox_deletefail;
    }
}