namespace MyNrf
{
    partial class Mypaint
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuchange1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuchangeDefult1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuchange1,
            this.contextMenuchangeDefult1,
            this.ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 92);
            // 
            // tcmsChage
            // 
            this.contextMenuchange1.Name = "contextMenuchange1";
            this.contextMenuchange1.Size = new System.Drawing.Size(154, 22);
            this.contextMenuchange1.Text = "放大选取框功能";
            this.contextMenuchange1.Click += new System.EventHandler(this.tcmsChage_Click);
            // 
            // tcmsDefult
            // 
            this.contextMenuchangeDefult1.Name = "contextMenuchangeDefult1";
            this.contextMenuchangeDefult1.Size = new System.Drawing.Size(154, 22);
            this.contextMenuchangeDefult1.Text = "默认坐标范围";
            this.contextMenuchangeDefult1.Click += new System.EventHandler(this.tcmsDefult_Click);
            // 
            // 坐标辅助显示ToolStripMenuItem
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.ToolStripMenuItem1.Text = "坐标辅助显示";
            this.ToolStripMenuItem1.Click += new System.EventHandler(this.坐标辅助显示ToolStripMenuItem_Click);

            // 
            // UcWaveShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Mypaint";
            this.Load += new System.EventHandler(this.UcWaveShow_Load);
            this.Resize += new System.EventHandler(this.UcWaveShow_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuchange1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuchangeDefult1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
    }
}
