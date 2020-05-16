namespace MyNrf
{
    partial class MyMin
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
            this.Name = "MyMin";
            this.Size = new System.Drawing.Size(100, 32);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.VistaButton_Paint);//绘制
            this.MouseEnter += new System.EventHandler(this.VistaButton_MouseEnter);//鼠标在内
            this.MouseLeave += new System.EventHandler(this.VistaButton_MouseLeave);//鼠标离开
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(VistaButton_MouseUp);//鼠标在按钮的空间里放开
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VistaButton_MouseDown);//鼠标在按钮的空间里按下
        }

        #endregion
    }
}
