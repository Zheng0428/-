namespace MyNrf
{
    partial class MYPar_Set
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.myButton1 = new MyNrf.MyButton();
            this.par_Set1 = new MyNrf.Par_Set();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.button1.Location = new System.Drawing.Point(3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.button2.Location = new System.Drawing.Point(84, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "上一页";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.button3.Location = new System.Drawing.Point(165, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "下一页";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // myButton1
            // 
            this.myButton1.BackColor = System.Drawing.Color.Transparent;
            this.myButton1.BaseColor = System.Drawing.Color.Transparent;
            this.myButton1.ButtonStyle = MyNrf.MyButton.Style.Default;
            this.myButton1.ButtonText = null;
            this.myButton1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.myButton1.Location = new System.Drawing.Point(682, 1);
            this.myButton1.Name = "myButton1";
            this.myButton1.Size = new System.Drawing.Size(25, 25);
            this.myButton1.TabIndex = 6;
            this.myButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myButton1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myButton1_Click);
            // 
            // par_Set1
            // 
            this.par_Set1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.par_Set1.Location = new System.Drawing.Point(4, 36);
            this.par_Set1.Name = "par_Set1";
            this.par_Set1.Size = new System.Drawing.Size(700, 300);
            this.par_Set1.TabIndex = 5;
            // 
            // MYPar_Set
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 340);
            this.Controls.Add(this.myButton1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.par_Set1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MYPar_Set";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MYPar_Set";
            this.Load += new System.EventHandler(this.FrmConData_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MyPar_Set_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MyPar_Set_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private MyButton myButton1;
        private Par_Set par_Set1;
    }
}