namespace pdd
{
    partial class Form3
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_content = new System.Windows.Forms.TextBox();
            this.textBox_size = new System.Windows.Forms.TextBox();
            this.textBox_caijinewgoods = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "采集关键字";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F);
            this.label2.Location = new System.Drawing.Point(40, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "采集数";
            // 
            // textBox_content
            // 
            this.textBox_content.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_content.Location = new System.Drawing.Point(113, 39);
            this.textBox_content.Name = "textBox_content";
            this.textBox_content.Size = new System.Drawing.Size(208, 26);
            this.textBox_content.TabIndex = 2;
            this.textBox_content.TextChanged += new System.EventHandler(this.textBox_content_TextChanged);
            this.textBox_content.Leave += new System.EventHandler(this.textBox_content_Leave);
            // 
            // textBox_size
            // 
            this.textBox_size.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_size.Location = new System.Drawing.Point(113, 85);
            this.textBox_size.Name = "textBox_size";
            this.textBox_size.Size = new System.Drawing.Size(100, 26);
            this.textBox_size.TabIndex = 3;
            this.textBox_size.TextChanged += new System.EventHandler(this.textBox_size_TextChanged);
            this.textBox_size.Leave += new System.EventHandler(this.textBox_size_Leave);
            // 
            // textBox_caijinewgoods
            // 
            this.textBox_caijinewgoods.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_caijinewgoods.Location = new System.Drawing.Point(113, 126);
            this.textBox_caijinewgoods.Name = "textBox_caijinewgoods";
            this.textBox_caijinewgoods.Size = new System.Drawing.Size(100, 26);
            this.textBox_caijinewgoods.TabIndex = 5;
            this.textBox_caijinewgoods.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10F);
            this.label3.Location = new System.Drawing.Point(40, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "新品数";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F);
            this.button1.Location = new System.Drawing.Point(113, 167);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 37);
            this.button1.TabIndex = 6;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 216);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_caijinewgoods);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_size);
            this.Controls.Add(this.textBox_content);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form3";
            this.Text = "采集设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_content;
        private System.Windows.Forms.TextBox textBox_size;
        private System.Windows.Forms.TextBox textBox_caijinewgoods;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}