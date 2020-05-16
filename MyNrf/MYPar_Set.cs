using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyNrf
{
    public partial class MYPar_Set : Form
    {
        private System.Drawing.Point mousePosition;
        public MYPar_Set()
        {
            InitializeComponent();
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 0), new Point(this.Width - 1, 0));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, this.Height - 1), new Point(0, this.Height - 1));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, this.Height - 1), new Point(0, 0));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 26), new Point(this.Width - 1, 26));
        }
        private void MyPar_Set_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePosition.X = e.X;
                this.mousePosition.Y = e.Y;
            }

        }
        private void MyPar_Set_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - mousePosition.Y - SystemInformation.FrameBorderSize.Height;
                this.Left = Control.MousePosition.X - mousePosition.X - SystemInformation.FrameBorderSize.Width;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
             MyNrf.Form1.ParValue = this.par_Set1.setListParValueList();
             if (MyNrf.Form1.mymode == MyMode.Nrf_CCD || MyNrf.Form1.mymode == MyMode.Nrf_Par || MyNrf.Form1.mymode == MyMode.Nrf_Pic || MyNrf.Form1.mymode == MyMode.Uart_CCD || MyNrf.Form1.mymode == MyMode.Uart_Par || MyNrf.Form1.mymode == MyMode.Uart_Pic)
             {
                 MyNrf.Form1.XmlFileWrite(new XmlInfo("参数选项", MyNrf.Form1.ParValue.Count.ToString()), true);
                 for (int i = 1; i <= MyNrf.Form1.ParValue.Count; i++)
                 {
                     MyNrf.Form1.XmlFileWrite(new XmlInfo("参数" + i.ToString() + "的名称", MyNrf.Form1.ParValue[i - 1].Name), false);
                     MyNrf.Form1.XmlFileWrite(new XmlInfo("参数" + i.ToString() + "的长度", MyNrf.Form1.ParValue[i - 1].LengthIndex.ToString()), false);
                     MyNrf.Form1.XmlFileWrite(new XmlInfo("参数" + i.ToString() + "的类型", MyNrf.Form1.ParValue[i - 1].TypeIndex.ToString()), false);
                     MyNrf.Form1.XmlFileWrite(new XmlInfo("参数" + i.ToString() + "的颜色", System.Drawing.ColorTranslator.ToHtml(MyNrf.Form1.ParValue[i - 1].WaveColor)), false);
                     MyNrf.Form1.XmlFileWrite(new XmlInfo("参数" + i.ToString() + "的缩放比例", MyNrf.Form1.ParValue[i - 1].WaveMulIndex.ToString()), false);
                     MyNrf.Form1.XmlFileWrite(new XmlInfo("参数" + i.ToString() + "的波形开关", MyNrf.Form1.ParValue[i - 1].WaveOn.ToString()), false);
                 }
             }
             this.Close();
        }

        private void button2_Click(object sender, EventArgs e)//上一页
        {
            this.par_Set1.PageLast();
        }

        private void button3_Click(object sender, EventArgs e)//下一页
        {
            this.par_Set1.PageNext();
        }

        private void FrmConData_Load(object sender, EventArgs e)
        {
            int i;
            for ( i = 0; i < MyNrf.Form1.ParValue.Count; i++)
            {
                this.par_Set1.Add(MyNrf.Form1.ParValue[i].Name, MyNrf.Form1.ParValue[i].TypeIndex, MyNrf.Form1.ParValue[i].LengthIndex, MyNrf.Form1.ParValue[i].WaveColor, MyNrf.Form1.ParValue[i].WaveMulIndex, MyNrf.Form1.ParValue[i].WaveOn);             
            }           
            this.par_Set1.Init();
        }

        private void myButton1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Dispose();
            }
        }





    }
}
