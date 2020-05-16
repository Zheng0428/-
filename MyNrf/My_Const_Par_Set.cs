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
    public partial class My_Const_Par_Set : Form
    {

                private System.Drawing.Point mousePosition;

        public My_Const_Par_Set()
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
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 27), new Point(this.Width - 1, 27));
        }
        private void My_Const_Par_Set_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePosition.X = e.X;
                this.mousePosition.Y = e.Y;
            }

        }
        private void My_Const_Par_Set_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - mousePosition.Y - SystemInformation.FrameBorderSize.Height;
                this.Left = Control.MousePosition.X - mousePosition.X - SystemInformation.FrameBorderSize.Width;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MyNrf.Form1.ConstValue = this.Const_Set1.setListParValueList();
            MyNrf.Form1.XmlFileWrite(new XmlInfo("控制固参", MyNrf.Form1.ConstValue.Count.ToString()), true);
            for (int i = 1; i <= MyNrf.Form1.ConstValue.Count; i++)
            {
                MyNrf.Form1.XmlFileWrite(new XmlInfo("固参" + i.ToString() + "的名称", MyNrf.Form1.ConstValue[i - 1].Name), false);
                MyNrf.Form1.XmlFileWrite(new XmlInfo("固参" + i.ToString() + "的长度", MyNrf.Form1.ConstValue[i - 1].LengthIndex.ToString()), false);
                MyNrf.Form1.XmlFileWrite(new XmlInfo("固参" + i.ToString() + "的类型", MyNrf.Form1.ConstValue[i - 1].TypeIndex.ToString()), false);
                MyNrf.Form1.XmlFileWrite(new XmlInfo("固参" + i.ToString() + "的颜色", System.Drawing.ColorTranslator.ToHtml(MyNrf.Form1.ConstValue[i - 1].WaveColor)), false);
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)//上一页
        {
            this.Const_Set1.PageLast();
        }

        private void button3_Click(object sender, EventArgs e)//下一页
        {
            this.Const_Set1.PageNext();
        }
        private void myButton1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Dispose();
            }
        }


        private void My_Const_Par_Set_Load(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < MyNrf.Form1.ConstValue.Count; i++)
            {
                this.Const_Set1.Add(MyNrf.Form1.ConstValue[i].Name, MyNrf.Form1.ConstValue[i].TypeIndex, MyNrf.Form1.ConstValue[i].LengthIndex, MyNrf.Form1.ConstValue[i].WaveColor, MyNrf.Form1.ConstValue[i].WaveMulIndex, MyNrf.Form1.ConstValue[i].WaveOn);
            }
            this.Const_Set1.Init();
        }

        private void Const_Set1_Load(object sender, EventArgs e)
        {

        }
    }
}
