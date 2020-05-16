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
using System.Configuration;
using System.Threading;
using MyNrf.Properties;
using System.Drawing.Drawing2D;

namespace MyNrf
{

    public partial class MyRichTextBox : UserControl
    {
       // public event EventHandler PropertyChanged;
        private string text = String.Empty;
      
        //private void OnPropertyChanged(EventArgs eventArgs)
        //{
        //    if (this.PropertyChanged != null)//判断事件是否有处理函数 
        //    {
        //        this.PropertyChanged(this, eventArgs);
        //    }
        //}
        public MyRichTextBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
           // base.BackColor = Color.FromArgb(_alpha, Color.Transparent);
         //   this.BackgroundImage = global::MyNrf.Properties.Resources.Transrichbox1;
            
        }



        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //0x20;  // 开启 WS_EX_TRANSPARENT,使控件支持透明
                return cp;
            }
        }

          private int _alpha = 125;
          public int Alpha
          {
              get
              {
                  return _alpha;
              }
              set
              {
                  _alpha = value;
                  this.Invalidate();
              }
          }


         private Font LineFont = new Font("隶书", 10, FontStyle.Bold);

        public Font MyLineFont
        {
            get { return LineFont; }
            set { LineFont = value; }
        }
        private Color text_color = Color.Black;
        public Color Text_Color 
        {
            get { return text_color;}
            set { text_color = value; }
        }

        public string MyText
        {
            get
            {
                return text;
            }
            set
            {

                text=value;
                ct.Location = new Point(0, 0);
                this.Invalidate();
                //Visible = false;
                //Visible = true;
               // this.OnPropertyChanged(new EventArgs());//值改变时触发事件
            }
        }



        private void MyRichTextBox_Load(object sender, EventArgs e)
        {
            Graphics graphics = CreateGraphics();  
            this.Paint += new PaintEventHandler(RichTextBox_Paint);
            //this.MyText = "";
            ct.Location = new Point(0, 0);
            ct.Height = (int)Math.Ceiling(graphics.MeasureString("幽魂", LineFont).Height);

            this.Invalidate();
        }



        private void MyRichTextBox_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }


        //private void MyRichTextBox_PropertyChanged(object sender, EventArgs e) 
        //{
        //    lbl.Invalidate();
        //}

        private Rectangle ct = new Rectangle();


        private void RichTextBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            List<string> write_data = new List<string>();
            int begin = 0, end = 0;
            int tmp_ctx = ct.X;
            for (int Index = 0; Index < text.Length; Index++)
            {
                end = Index;
                if (Index == text.Length - 1)
                {
                    write_data.Add(text.Substring(begin));
                }
                else if (end + 1 < text.Length && text.Substring(end, 2) == "\r\n")
                {
                    write_data.Add(text.Substring(begin, end - begin));
                    end = Index += 2;
                    begin = end;
                }
                else if (g.MeasureString(text.Substring(begin, end - begin + 1), LineFont).Width > (this.Width - tmp_ctx))
                {
                    write_data.Add(text.Substring(begin, end - begin));
                    begin = end;
                }
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            SolidBrush P = new SolidBrush(text_color);
            for (int i = 0; i < write_data.Count; i++)
            {
                g.DrawString(write_data[i], LineFont, P, ct.Location, sf);
                if (i < write_data.Count - 1)
                {
                    Point_New_Range();
                }
            }
            //Brush br = new SolidBrush(Color.FromArgb(_alpha, Color.Transparent));
            //GraphicsPath bb = new GraphicsPath();

            //bb.AddLine(0, 0, this.Width, 0);
            //bb.AddLine(this.Width, 0, this.Width, this.Height);
            //bb.AddLine(this.Width, this.Height, 0, this.Height);
            //bb.AddLine(0, this.Height, 0, 0);
            //g.FillPath(br, bb);
            //this.ForeColor = Color.FromArgb(_alpha, Color.Transparent);
            //this.BackColor = Color.FromArgb(_alpha, Color.Transparent);
        }


        public void AppendText(string text)
        {
            ct.Width = this.Width - ct.X;
            ct.Y = 0;
            this.text+=text;
            this.Invalidate();
            Visible = false;
            Visible = true;
        }

        private void Point_New_Range() 
        {
            ct.X = 0;
            ct.Width = this.Width;
            ct.Y += ct.Height;
        }
        public void Clear() 
        {
            this.text = "";
            this.Invalidate();
            Visible = false;
            Visible = true;
        }
    }
}
