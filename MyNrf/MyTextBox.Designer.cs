namespace MyNrf
{
    partial class MyTextBox
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
            this.richTextBox1 = new MyNrf.TestRichTextBox();
            this.myButton1 = new MyNrf.MyButton();
            this.label1 = new MyNrf.label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.richTextBox1.Location = new System.Drawing.Point(1, 29);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(829, 398);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.GotFocus += new System.EventHandler(this.richTextBox1_GotFocus);
            this.richTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
            this.richTextBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseUP);
            //this.richTextBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseWheel);
         //   this.richTextBox1.Cursor = Form1.MyCursor.Default;
            // 
            // myButton1
            // 
            this.myButton1.BackColor = System.Drawing.Color.Transparent;
            this.myButton1.BaseColor = System.Drawing.Color.Transparent;
            this.myButton1.ButtonStyle = MyNrf.MyButton.Style.Default;
            this.myButton1.ButtonText = null;
            this.myButton1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.myButton1.Location = new System.Drawing.Point(806, 2);
            this.myButton1.Name = "myButton1";
            this.myButton1.Size = new System.Drawing.Size(25, 25);
            this.myButton1.TabIndex = 1;
            this.myButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myButton1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myButton1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.MyText = "";
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "第十三届恩智浦智能车     杭电智能车队";
            // 
            // MyTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 429);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.myButton1);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "MyTextBox";
            this.Text = "MyTextBox";
            this.Load += new System.EventHandler(this.MyTextBox_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MyTextBox_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MyTextBox_MouseMove);
            this.Resize += new System.EventHandler(this.MyTextBox_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TestRichTextBox richTextBox1;
        private MyButton myButton1;
        private label label1;
    }
}