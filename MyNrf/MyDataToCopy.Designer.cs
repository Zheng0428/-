namespace MyNrf
{
    partial class MyDataToCopy
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNum = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblWaveOn = new System.Windows.Forms.Label();
            this.llblCount = new System.Windows.Forms.LinkLabel();
            this.llblPageNum = new System.Windows.Forms.LinkLabel();
            this.button2 = new MyNrf.button();
            this.button1 = new MyNrf.button();
            this.SuspendLayout();
            // 
            // lblNum
            // 
            this.lblNum.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNum.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lblNum.Location = new System.Drawing.Point(8, 9);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(93, 23);
            this.lblNum.TabIndex = 0;
            this.lblNum.Text = "数据编号";
            this.lblNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum.Enabled = false;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lblName.Location = new System.Drawing.Point(108, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(93, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "数据名称";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblName.Enabled = false;
            // 
            // lblWaveOn
            // 
            this.lblWaveOn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWaveOn.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lblWaveOn.Location = new System.Drawing.Point(208, 9);
            this.lblWaveOn.Name = "lblWaveOn";
            this.lblWaveOn.Size = new System.Drawing.Size(93, 23);
            this.lblWaveOn.TabIndex = 0;
            this.lblWaveOn.Text = "选中参数";
            this.lblWaveOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblWaveOn.Enabled = false;
            // 
            // llblCount
            // 
            this.llblCount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.llblCount.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.llblCount.LinkArea = new System.Windows.Forms.LinkArea(5, 4);
            this.llblCount.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llblCount.LinkColor = System.Drawing.SystemColors.WindowFrame;
            this.llblCount.Location = new System.Drawing.Point(108, 270);
            this.llblCount.Name = "llblCount";
            this.llblCount.Size = new System.Drawing.Size(97, 23);
            this.llblCount.TabIndex = 1;
            this.llblCount.TabStop = true;
            this.llblCount.Text = "参数数目:0000";
            this.llblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llblCount.UseCompatibleTextRendering = true;
            this.llblCount.Enabled = false;
            // 
            // llblPageNum
            // 
            this.llblPageNum.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.llblPageNum.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.llblPageNum.LinkArea = new System.Windows.Forms.LinkArea(5, 4);
            this.llblPageNum.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.llblPageNum.LinkColor = System.Drawing.SystemColors.WindowFrame;
            this.llblPageNum.Location = new System.Drawing.Point(8, 270);
            this.llblPageNum.Name = "llblPageNum";
            this.llblPageNum.Size = new System.Drawing.Size(97, 23);
            this.llblPageNum.TabIndex = 1;
            this.llblPageNum.TabStop = true;
            this.llblPageNum.Text = "当前页码:0000";
            this.llblPageNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llblPageNum.UseCompatibleTextRendering = true;
            this.llblPageNum.Enabled = false;
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.button2.Location = new System.Drawing.Point(262, 271);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(51, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Last";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.button1.Location = new System.Drawing.Point(208, 271);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Next";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MyDataToCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.llblPageNum);
            this.Controls.Add(this.llblCount);
            this.Controls.Add(this.lblWaveOn);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblNum);
            this.Name = "MyDataToCopy";
            this.Size = new System.Drawing.Size(321, 300);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblWaveOn;
        private System.Windows.Forms.LinkLabel llblPageNum;
        private System.Windows.Forms.LinkLabel llblCount;
        #endregion
        private button button1;
        private button button2;
    }
}
