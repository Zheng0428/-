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
    public partial class MyPictureBox : UserControl
    {
        public PictureBox Pic = new PictureBox();
        public Label lbl = new Label();
        int IMG_X, IMG_Y;
        int X, Y;
        public bool mousecheck = false;
        Color IMG_C;

        /// <summary>
        /// false拟图  true原图
        /// </summary>

        int mysub = 1;


        public int MySub
        {
            get
            {
                return mysub;
            }
            set
            {
                mysub = value;
                Pic.Invalidate();
            }
        }



        public Color MyColor
        {
            get
            {
                return this.BackColor;
            }
            set
            {
                this.BackColor = value;
            }
        }

        public Bitmap MyBackImage
        {
            get
            {
                return (Bitmap)this.Pic.Image;
            }
            set
            {
                 this.Pic.Image = value;
            }
        }



        public string MyTxt
        {
          
            set
            {

                lbl.Text = value;

            }
        }

        private int PenWidth = 1;

        public int MyPenWidth
        {
            get { return PenWidth; }
            set { PenWidth = value; }
        }




        public MyPictureBox()
        {
            InitializeComponent();

            this.Pic.Controls.Add(lbl);
            lbl.Top = lbl.Left = mysub;
            lbl.BackColor = Color.Transparent;
            lbl.BorderStyle = BorderStyle.None;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //if (mousecheck == true)
            //{

            //}
        }


        private void UcPictureBox_Load(object sender, EventArgs e)
        {
            Pic.Top = mysub;
            Pic.Left = mysub;
            Pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Controls.Add(Pic);
            IMG_X = IMG_Y = 0xFFF;
            IMG_C = Color.White;
        }




        private void UcPictureBox_Resize(object sender, EventArgs e)
        {
            lbl.Width = Pic.Width = this.Width - (mysub << 1);
            lbl.Height = Pic.Height = this.Height - (mysub << 1);
        }

        public void ImageLoad(string Path)
        {
            Pic.Load(Path);
        }

        public void InvalidateImage()
        {
            Pic.Invalidate();
        }

      

        public void getPos(int x, int y,double  XStep,double  YStep, Color backColor)
        {
            PIT_FLAG = false;
            X = x;
            Y = y;
            IMG_X =(int) (X / XStep);
            IMG_Y = (int )(Y / YStep);
            if (IMG_X < this.Pic.Image.Width && IMG_X >= 0 && IMG_Y < this.Pic.Image.Height && IMG_Y >= 0)
            {
             
                IMG_C = backColor;
                this.lbl.Paint += new PaintEventHandler(lbl_Paint);
               
            }
            lbl.Invalidate();
        }
        private bool PIT_FLAG = false;
        public void getPos(int x, int y, double XStep, double YStep, Color backColor,bool Pit_flag)
        {
            X = x;
            Y = y;
            PIT_FLAG = Pit_flag;
            IMG_X = (int)(X / XStep);
            IMG_Y = (int)(Y / YStep);
            if (IMG_X < this.Pic.Image.Width && IMG_X >= 0 && IMG_Y < this.Pic.Image.Height && IMG_Y >= 0)
            {

                IMG_C = backColor;
                this.lbl.Paint += new PaintEventHandler(lbl_Paint);

            }
            lbl.Invalidate();
        }

        private void lbl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (IMG_X < Pic.Image.Width && IMG_Y < Pic.Image.Height)
                {
                    Graphics g = e.Graphics;
                    SolidBrush P;
                    Color tmp_Color = this.BackColor;
                    if (PIT_FLAG == true)
                    {
                        tmp_Color = Color.CadetBlue;              
                    }


                    P = new SolidBrush(System.Drawing.SystemColors.WindowFrame);

                    //g.DrawString(string.Format("[ {0:D3},{1:D3} ]", Pic.Image.Width - 1 - IMG_X, Pic.Image.Height - 1 - IMG_Y), this.Font, P, 3, 3);
                    g.DrawLine(new Pen(tmp_Color, PenWidth), new Point(X, 0), new Point(X, lbl.Height));
                    g.DrawLine(new Pen(tmp_Color, PenWidth), new Point(0, Y), new Point(lbl.Width, Y));



                    this.lbl.Paint -= new PaintEventHandler(lbl_Paint);
                }
            }
            catch
            {

            }
        }
    }
}
