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
            this.textBox_guige = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_pricesize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_verifyCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
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
            this.richTextBox_ids.Text = "1\n2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "商品id，多个换行";
            // 
            // textBox_log
            // 
            this.textBox_log.Location = new System.Drawing.Point(166, 34);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_log.Size = new System.Drawing.Size(589, 484);
            this.textBox_log.TabIndex = 129;
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(2, 389);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(146, 23);
            this.button_add.TabIndex = 130;
            this.button_add.Text = "添加";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2, 418);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 23);
            this.button1.TabIndex = 131;
            this.button1.Text = "启动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(166, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(146, 23);
            this.button2.TabIndex = 132;
            this.button2.Text = "删除商品";
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
            // textBox_guige
            // 
            this.textBox_guige.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_guige.Location = new System.Drawing.Point(808, 449);
            this.textBox_guige.Name = "textBox_guige";
            this.textBox_guige.Size = new System.Drawing.Size(125, 26);
            this.textBox_guige.TabIndex = 134;
            this.textBox_guige.Leave += new System.EventHandler(this.textBox_guige_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(761, 454);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 135;
            this.label2.Text = "规格：";
            // 
            // textBox_pricesize
            // 
            this.textBox_pricesize.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_pricesize.Location = new System.Drawing.Point(808, 481);
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
            this.label3.Location = new System.Drawing.Point(761, 486);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 137;
            this.label3.Text = "总价：";
            // 
            // textBox_verifyCode
            // 
            this.textBox_verifyCode.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_verifyCode.Location = new System.Drawing.Point(829, 241);
            this.textBox_verifyCode.Name = "textBox_verifyCode";
            this.textBox_verifyCode.Size = new System.Drawing.Size(104, 26);
            this.textBox_verifyCode.TabIndex = 138;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(761, 246);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 139;
            this.label4.Text = "验证码：";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(774, 273);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(146, 23);
            this.button3.TabIndex = 140;
            this.button3.Text = "确认";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(761, 189);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(172, 45);
            this.pictureBox2.TabIndex = 141;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label5.Location = new System.Drawing.Point(107, 506);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 142;
            this.label5.Text = "清除日志";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 530);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox_verifyCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_pricesize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_guige);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.textBox_log);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_ids);
            this.Name = "Form2";
            this.Text = "Form2";
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
        private System.Windows.Forms.TextBox textBox_guige;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_pricesize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_verifyCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label5;
    }
}