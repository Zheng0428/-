using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace MyNrf
{
    public partial class MyDataToCopy : UserControl
    {
        int PageCount = 7;
        int PageNum = 0;
        int MaxPageNum = 0;
        const int TopSub = 10;
        const int WaveColorS = 20;
        const int WaveColorOnS = 14;
        public MyDataToCopy()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 35), new Point(this.Width - 1, 35));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 0), new Point(this.Width - 1, 0));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 0), new Point(0, this.Height - 1));

            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, this.Height - 1), new Point(this.Width - 1, this.Height - 1));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 265), new Point(this.Width - 1, 265));
        }
        public class ClassParControls
        {
            public Label lblNum = new Label();
            public TextBox txtName = new TextBox();
            public CheckBox cbxWaveOn = new CheckBox();
            public ClassParControls(int Num, string Name, bool WaveOn)
            {
                lblNum.Text = Num.ToString("0000");
                lblNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                lblNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                lblNum.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                lblNum.ForeColor = System.Drawing.SystemColors.WindowFrame;

                txtName.Text = Name;
                txtName.BorderStyle = System.Windows.Forms.BorderStyle.None;
                txtName.BackColor = System.Drawing.SystemColors.Control;
                txtName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                txtName.ForeColor = System.Drawing.SystemColors.WindowFrame;

                cbxWaveOn.Checked = WaveOn;


            }
        }

        public List<ClassParControls> ListConData = new List<ClassParControls>();

        public void Add(string Name, bool WaveOn)
        {
            ClassParControls ParCon = new ClassParControls(ListConData.Count + 1, Name, WaveOn);
            ListConData.Add(ParCon);
            PageNum = MaxPageNum;

            if (lblNum.Top + lblNum.Height + (TopSub + lblNum.Height) * (ListConData.Count - PageNum * PageCount) > llblPageNum.Top)
            {
                PageNum++;
                MaxPageNum = PageNum;
            }

            ParCon.lblNum.Width = lblNum.Width;
            ParCon.lblNum.Height = lblNum.Height;
            ParCon.lblNum.Left = lblNum.Left;
            ParCon.lblNum.Top = lblNum.Top + (TopSub + lblNum.Height) * (ListConData.Count - PageNum * PageCount);
            this.Controls.Add(ParCon.lblNum);

            ParCon.txtName.Width = lblName.Width;
            ParCon.txtName.Height = lblName.Height;
            ParCon.txtName.Left = lblName.Left;
            ParCon.txtName.Top = ParCon.lblNum.Top;
            this.Controls.Add(ParCon.txtName);

            ParCon.cbxWaveOn.Width = WaveColorOnS;
            ParCon.cbxWaveOn.Height = WaveColorOnS;
            ParCon.cbxWaveOn.Left = lblWaveOn.Left + lblWaveOn.Width / 2 - ParCon.cbxWaveOn.Width / 2;
            ParCon.cbxWaveOn.Top = ParCon.lblNum.Top + ParCon.lblNum.Height / 2 - ParCon.cbxWaveOn.Height / 2;
            ParCon.cbxWaveOn.Click += new EventHandler(LengthChange);
            this.Controls.Add(ParCon.cbxWaveOn);

        }

        public void PageShow()
        {
            int i = 0;
            for (i = 0; i < ListConData.Count; i++)
            {
                if (i >= PageCount * PageNum && i < PageCount * (PageNum + 1))
                {
                    ListConData[i].lblNum.Visible = true;
                    ListConData[i].txtName.Visible = true;
                    ListConData[i].cbxWaveOn.Visible = true;
                }
                else
                {
                    ListConData[i].lblNum.Visible = false;
                    ListConData[i].txtName.Visible = false;
                    ListConData[i].cbxWaveOn.Visible = false;
                }
            }

            LengthChange(this, new EventArgs());

        }

        private void LengthChange(object sender, EventArgs e)
        {

            llblPageNum.Text = "当前页码:" + (PageNum + 1).ToString("0000");
            llblCount.Text = "参数数目:" + ListConData.Count.ToString("0000");

        }
        private void UcParValue_Resize(object sender, EventArgs e)
        {
            llblPageNum.Top = this.Height - llblCount.Height;
            llblCount.Top = llblPageNum.Top;


            PageCount = (llblPageNum.Top - (lblNum.Height + lblNum.Top)) / (TopSub + lblNum.Height);
        }
        public void PageNext()
        {
            if (PageNum < MaxPageNum)
            {
                PageNum++;
                PageShow();
            }
        }

        public void PageLast()
        {
            if (PageNum > 0)
            {
                PageNum--;
                PageShow();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PageNext();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PageLast();
        }

        public void Init()
        {
            this.LengthChange(this, new EventArgs());
            this.PageNum = 0;
            this.Show();

        }

    }
}
