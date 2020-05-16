using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace MyNrf
{
    public partial class CCDPictureBox : UserControl
    {
        public List<CCDLCR> Item = new List<CCDLCR>();
        private bool Show_Map = false;
        private bool Show_Flag = false;
        private bool picmode = true;
        public Label lbl = new Label();
        public int lines = 1;
        private int X = 0, Y = 0;
        public bool PICMode
        {
            get
            {
                return picmode;
            }
            set
            {
                picmode = value;
            }
        }
        public string MyTxt
        {

            set
            {

                lbl.Text = value;

            }
        }
        public CCDPictureBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Controls.Add(lbl);
            this.lbl.Top = this.lbl.Left = 0;
            this.lbl.Width = this.Width;
            this.lbl.Height = this.Height;
            lbl.BackColor = Color.Transparent;
            lbl.BorderStyle = BorderStyle.None;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            this.lbl.Paint += new PaintEventHandler(this.lbl_Onpaint);

        }
        private void CCDPictureBox_Resize(object sender, EventArgs e)
        {
            this.lbl.Top = this.lbl.Left = 0;
            this.lbl.Width = this.Width;
            this.lbl.Height = this.Height;
        }
        public void getPos(int x, int y, bool IsShowMap)
        {
            this.Show_Map = IsShowMap;
            if (Show_Map == true)
            {
                this.X = x;
                this.Y = y;
                //this.lbl.Paint += new PaintEventHandler(this.lbl_Onpaint);
                this.lbl.Invalidate();
            }



        }
        private void lbl_Onpaint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            if (Show_Map == true)
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;//消除锯齿
                SolidBrush P = new SolidBrush(System.Drawing.SystemColors.WindowFrame);
                Color tmp_Color = Color.CadetBlue;
                int PenWidth = 1;
                g.DrawLine(new Pen(tmp_Color, PenWidth), new Point(X, 0), new Point(X, lbl.Height));
                g.DrawLine(new Pen(tmp_Color, PenWidth), new Point(0, Y), new Point(lbl.Width, Y));
                if (picmode == true)
                {
                    g.DrawString(string.Format("[ {0:D3},{1:D3} ]", X * 128 / this.Width, 255 - Y * 256 / this.Height), this.Font, P, 3, 3);
                }
                else if (picmode == false)
                {
                    g.DrawString(string.Format("[ {0:D3},{1:D3} ]", X * 128 / this.Width, Y * 50 / this.Height), this.Font, P, 3, 3);
                }
            }
            else
            {

            }

            //  this.lbl.Paint -= new PaintEventHandler(this.lbl_Onpaint);

        }
        public void Show_CCDPic()
        {
            this.Invalidate(false);
        }

        private void CCD_Paint(object sender, PaintEventArgs e)
        {
            //       MessageBox.Show("OK");
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;//消除锯齿

            if (picmode == true)
            {

                if (Item.Count == 0)
                {
                    return;
                }
                else
                {
                    if (Item.Count > lines)
                    {
                        Item.RemoveRange(lines, Item.Count - lines);
                    }
                    for (int i = 0; i < Item.Count; i++)
                    {
                        for (int j = 0; j < Item[i].Length - 1; j++)
                        {
                            if (i == 0)
                            {
                                g.DrawLine(new Pen(Item[i].Dcolor, 2), new Point(j * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[j] * this.Height / 255), new Point((j + 1) * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[j + 1] * this.Height / 255));
                            }
                            else
                            {
                                g.DrawLine(new Pen(Item[i].Dcolor, 2), new Point(j * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[j] * this.Height / 255 - i * 60), new Point((j + 1) * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[j + 1] * this.Height / 255 - i * 60));
                            }
                        }
                        if (Show_Flag == false)
                        {
                            if (Item[i].ccdpos.Left < 128)
                            {
                                // 对应空格为 横轴 起始x  ，起始y  
                                //  g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Left * (this.Width + 4) / Item[i].Length - 15, this.Height - Item[i].Data[Item[i].ccdpos.Left] * this.Height / 255), new Point(Item[i].ccdpos.Left * (this.Width + 4) / Item[i].Length + 15, this.Height - Item[i].Data[Item[i].ccdpos.Left] * this.Height / 255));//画横线
                                if (i == 0)
                                {
                                    g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Left * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Left] * this.Height / 255 - i * 60 + 15), new Point(Item[i].ccdpos.Left * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Left] * this.Height / 255 - i * 60 - 15));//画竖线
                                }
                                else
                                {
                                    g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Left * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Left] * this.Height / 255 - i * 60 + 15), new Point(Item[i].ccdpos.Left * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Left] * this.Height / 255 - i * 60 - 15));//画竖线
                                }
                            }
                            if (Item[i].ccdpos.Right < 128)
                            {
                                // g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Right * (this.Width + 4) / Item[i].Length - 15, this.Height - Item[i].Data[Item[i].ccdpos.Right] * this.Height / 255), new Point(Item[i].ccdpos.Right * (this.Width + 4) / Item[i].Length + 15, this.Height - Item[i].Data[Item[i].ccdpos.Right] * this.Height / 255));
                                if (i == 0)
                                {
                                    g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Right * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Right] * this.Height / 255 - i * 60 + 15), new Point(Item[i].ccdpos.Right * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Right] * this.Height / 255 - i * 60 - 15));
                                }
                                else
                                {
                                    g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Right * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Right] * this.Height / 255 - i * 60 + 15), new Point(Item[i].ccdpos.Right * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Right] * this.Height / 255 - i * 60 - 15));
                                }
                            }
                            if (Item[i].ccdpos.Center < 128)
                            {
                                //  g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Center * (this.Width + 4) / Item[i].Length - 15, this.Height - Item[i].Data[Item[i].ccdpos.Center] * this.Height / 255), new Point(Item[i].ccdpos.Center * (this.Width + 4) / Item[i].Length + 15, this.Height - Item[i].Data[Item[i].ccdpos.Center] * this.Height / 255));
                                if (i == 0)
                                {
                                    g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Center * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Center] * this.Height / 255 - i * 60 + 15), new Point(Item[i].ccdpos.Center * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Center] * this.Height / 255 - i * 60 - 15));
                                }
                                else
                                {
                                    g.DrawLine(new Pen(Form1.CCDColor.CCDP, 1), new Point(Item[i].ccdpos.Center * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Center] * this.Height / 255 - i * 60 + 15), new Point(Item[i].ccdpos.Center * (this.Width + 4) / Item[i].Length, this.Height - Item[i].Data[Item[i].ccdpos.Center] * this.Height / 255 - i * 60 - 15));
                                }
                            }

                        }

                    }
                    //画出分界线
                    g.DrawLine(new Pen(Form1.CCDColor.CCDP, 3), new Point(0, this.Height / 6 * 3), new Point(this.Width, this.Height / 6 * 3));


                }
            }
            else if (picmode == false)
            {
                if (Item.Count == 0)
                {
                    return;
                }
                else
                {
                    if (Item.Count > 50)
                    {
                        Item.RemoveRange(50, Item.Count - 50);
                    }
                    for (int i = 0; i < Item.Count - 1; i++)
                    {
                        if (Item[i + 1].ccdpos.Center < 128)
                        {
                            g.DrawLine(new Pen(Item[i + 1].ccdpos.C, 4), new Point(Item[i].ccdpos.Center * this.Width / Item[i].Length, i * this.Height / 50), new Point(Item[i + 1].ccdpos.Center * this.Width / Item[i].Length, (i + 1) * this.Height / 50));
                        }
                        if (Item[i + 1].ccdpos.Left < 128)
                        {
                            g.DrawLine(new Pen(Item[i + 1].ccdpos.L, 4), new Point(Item[i].ccdpos.Left * this.Width / Item[i].Length, i * this.Height / 50), new Point(Item[i + 1].ccdpos.Left * this.Width / Item[i].Length, (i + 1) * this.Height / 50));
                        }
                        if (Item[i + 1].ccdpos.Right < 128)
                        {
                            g.DrawLine(new Pen(Item[i + 1].ccdpos.R, 4), new Point(Item[i].ccdpos.Right * this.Width / Item[i].Length, i * this.Height / 50), new Point(Item[i + 1].ccdpos.Right * this.Width / Item[i].Length, (i + 1) * this.Height / 50));
                        }
                    }
                }
            }
            if (Show_Map == true)
            {

            }

        }
    }



}
