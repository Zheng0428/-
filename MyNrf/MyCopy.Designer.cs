namespace MyNrf
{
    partial class MyCopy
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
            this.myDataToCopy1 = new MyNrf.MyDataToCopy();
            this.myButton1 = new MyNrf.MyButton();
            this.SuspendLayout();
            // 
            // myDataToCopy1
            // 
            this.myDataToCopy1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.myDataToCopy1.Location = new System.Drawing.Point(1, 1);
            this.myDataToCopy1.Name = "myDataToCopy1";
            this.myDataToCopy1.Size = new System.Drawing.Size(321, 300);
            this.myDataToCopy1.TabIndex = 0;
            this.myDataToCopy1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MyPar_Set_MouseDown);
            this.myDataToCopy1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MyPar_Set_MouseMove);
            // 
            // myButton1
            // 
            this.myButton1.BackColor = System.Drawing.SystemColors.Control;
            this.myButton1.BaseColor = System.Drawing.SystemColors.Control;
            this.myButton1.ButtonStyle = MyNrf.MyButton.Style.Default;
            this.myButton1.ButtonText = null;
            this.myButton1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.myButton1.Location = new System.Drawing.Point(295, 3);
            this.myButton1.Name = "myButton1";
            this.myButton1.Size = new System.Drawing.Size(25, 25);
            this.myButton1.TabIndex = 5;
            this.myButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myButton1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myButton1_Click);
            // 
            // MyCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 303);
            this.Controls.Add(this.myButton1);
            this.Controls.Add(this.myDataToCopy1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MyCopy";
            this.Text = "MyCopy";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MyPar_Set_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MyPar_Set_MouseMove);
            this.Load += new System.EventHandler(this.FrmConData_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MyDataToCopy myDataToCopy1;
        private MyButton myButton1;
    }
}