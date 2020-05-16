using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNrf
{
    public partial class Const_Set : UserControl
    {

        int PageCount=7;   
        int PageNum = 0;
        int MaxPageNum = 0;
        const int TopSub = 10;
        const int WaveColorS = 20;
        const int WaveColorOnS = 14;
        public Const_Set()
        {
            InitializeComponent();
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 35), new Point(this.Width - 1, 35));
            //g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 70), new Point(this.Width - 1, 70));
            //g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 105), new Point(this.Width - 1, 105));
            //g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 140), new Point(this.Width - 1, 140));
            //g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 175), new Point(this.Width - 1, 175));
            //g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 210), new Point(this.Width - 1, 210));
            //g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 245), new Point(this.Width - 1, 245));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 270), new Point(this.Width - 1, 270));
        }
        public class ClassParControls 
        {
            public Label lblNum = new Label();
            public TextBox txtName = new TextBox();
            public ComboBox cobType = new ComboBox();
            public ComboBox cobLength = new ComboBox();
            public Mylabel lblWaveColor = new Mylabel();



            public ClassParControls(int Num, string Name, int TypeIndex, int LengthIndex, Color WaveColor, int WaveMul, bool WaveOn) 
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

                cobType.Items.AddRange(new object[] {
                    "无符号型",
                    "有符号型"
                    });
                    cobType.SelectedIndex=TypeIndex;
                cobType.Enabled=false;
                cobType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                cobType.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                cobType.ForeColor = System.Drawing.SystemColors.WindowFrame;
                     cobLength.Items.AddRange(new object[] {
                    "1个Byte",
                    "2个Byte"
                    });
                    cobLength.SelectedIndex=LengthIndex;
                cobLength.Enabled=false;
                cobLength.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                cobLength.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                lblWaveColor.MyColor = WaveColor;
                cobLength.ForeColor = System.Drawing.SystemColors.WindowFrame;



            }



        }

        public List<ClassParControls> ListConData = new List<ClassParControls>();

        public void Add(string Name, int TypeIndex, int LengthIndex, Color WaveColor, int WaveMulIndex, bool WaveOn)
        {
            ClassParControls ParCon = new ClassParControls(ListConData.Count + 1, Name, TypeIndex, LengthIndex, WaveColor, WaveMulIndex, WaveOn);
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

            ParCon.cobType.Width = lblType.Width;
            ParCon.cobType.Height = lblType.Height;
            ParCon.cobType.Left = lblType.Left;
            ParCon.cobType.Top = ParCon.lblNum.Top;
            this.Controls.Add(ParCon.cobType);

            ParCon.cobLength.Width = lblLength.Width;
            ParCon.cobLength.Height = lblLength.Height;
            ParCon.cobLength.Left = lblLength.Left;
            ParCon.cobLength.Top = ParCon.lblNum.Top;
            ParCon.cobLength.SelectedIndexChanged += new EventHandler(LengthChange);
            this.Controls.Add(ParCon.cobLength);

            ParCon.lblWaveColor.Width = WaveColorS;
            ParCon.lblWaveColor.Height = WaveColorS;
            ParCon.lblWaveColor.Left = lblWaveColor.Left + lblWaveColor.Width / 2 - ParCon.lblWaveColor.Width / 2;
            ParCon.lblWaveColor.Top = ParCon.lblNum.Top + ParCon.lblNum.Height / 2 - ParCon.lblWaveColor.Height / 2;
            this.Controls.Add(ParCon.lblWaveColor);






           
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
                    ListConData[i].cobType.Visible = true;
                    ListConData[i].cobLength.Visible = true;
                    ListConData[i].lblWaveColor.Visible = true;
 


                }
                else
                {
                    ListConData[i].lblNum.Visible = false;
                    ListConData[i].txtName.Visible = false;
                    ListConData[i].cobType.Visible = false;
                    ListConData[i].cobLength.Visible = false;
                    ListConData[i].lblWaveColor.Visible = false;



                }
            }
            
            LengthChange(this, new EventArgs());
           
        }

        private void LengthChange(object sender, EventArgs e)
        {

            llblPageNum.Text = "当前页码:" + (PageNum + 1).ToString("0000");
            llblCount.Text = "参数数目:" + ListConData.Count.ToString("0000");

            int Length = getDataLength();
            llblLength.Text = "参数长度:" + Length.ToString("0000");
            Length = 0;




        }


        public int getDataLength()//获取参数长度
        {
            int Length = 0;
            for (int i = 0; i < ListConData.Count; i++)
            {
                if (ListConData[i].cobLength.SelectedIndex == 0)
                {
                    Length += 1;
                }
                else if (ListConData[i].cobLength.SelectedIndex == 1)
                {
                    Length += 2;
                }
                else if (ListConData[i].cobLength.SelectedIndex == 2)
                {
                    Length += 4;
                }
            }
            return Length;
        }

        private void UcParValue_Resize(object sender, EventArgs e)
        {
            llblPageNum.Top = this.Height - llblCount.Height;
            llblCount.Top = llblPageNum.Top;
            llblLength.Top = llblCount.Top;



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

        public List<ClassParValue> setListParValueList()
        {
            List<ClassParValue> ParValue = new List<ClassParValue>();
            for (int i = 0; i < ListConData.Count; i++)
            {
                ClassParValue temp = new ClassParValue(ListConData[i].txtName.Text, ListConData[i].cobType.SelectedIndex, ListConData[i].cobLength.SelectedIndex, ListConData[i].lblWaveColor.MyColor, 1, false);
                ParValue.Add(temp);
            }
            return ParValue;
        }

        public void Init() 
        {           
            this.PageNum = 0;
            this.Show();
            
        }

    }
}
