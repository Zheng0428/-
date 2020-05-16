using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNrf
{
    public partial class MyButton : UserControl
    {


        public enum Style
        {
            Default,
            Flat
        };

        private string mText; //按钮上显示的文字
        private Color mForeColor = Color.White;
        private Style mButtonStyle = Style.Default;
        private ContentAlignment mTextAlign = ContentAlignment.MiddleCenter;//居中显示 默认
        private Color mButtonColor = Color.Black;//按钮的底色部分
        private Color mGlowColor = Color.FromArgb(141, 189, 255);
        private Color mBaseColor = Color.White;//基础颜色
        private int line_weight = 1;
        public MyButton()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.MouseMove += new MouseEventHandler(MyButton_MouseMove);
            this.MouseLeave += new EventHandler(MyButton_MouseLeave);
            this.BackColor = Color.White;
           
        }
        private void MyButton_MouseMove(object sender, MouseEventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Choose;
        }
        private void MyButton_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Default;
        }
        public string ButtonText //按钮上显示的字
        {
            get { return mText; }
            set { mText = value; this.Invalidate(); }
        }

        public ContentAlignment TextAlign //文字
        {
            get { return mTextAlign; }
            set { mTextAlign = value; this.Invalidate(); }
        }
        public override Color ForeColor //修改字体颜色属性
        {
            get { return mForeColor; }
            set { mForeColor = value; this.Invalidate(); }
        }

        public Style ButtonStyle //当鼠标离开按钮时按钮显示
        {
            get { return mButtonStyle; }
            set { mButtonStyle = value; this.Invalidate(); }
        }

        public Color GlowColor //当鼠标在悬停按钮的部分
        {
            get { return mGlowColor; }
            set { mGlowColor = value; this.Invalidate(); }
        }

        public Color BaseColor  //按钮按下
        {
            get { return mBaseColor; }
            set { mBaseColor = value; this.Invalidate(); }
        }


        private void VistaButton_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            DrawBackground(e.Graphics);//绘制背景
            DrawText(e.Graphics);//绘制显示字
            DrawGlow(e.Graphics);

        }

        private StringFormat StringFormatAlignment(ContentAlignment textalign)//文字在按钮中的信息，位置
        {
            StringFormat sf = new StringFormat();
            switch (textalign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }
            switch (textalign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    break;
            }
            return sf;
        }
        private void DrawBackground(Graphics g)
        {
            return;
        }
        private void DrawText(Graphics g) //按钮的字样
        {
            StringFormat sf = StringFormatAlignment(this.TextAlign);
            Rectangle r = new Rectangle(8, 8, this.Width - 17, this.Height - 17);
            g.DrawString(this.ButtonText, this.Font, new SolidBrush(this.ForeColor), r, sf);
        }
        private void DrawGlow(Graphics g) 
        {
            Point P1=new Point();
            Point P2=new Point();
            Rectangle r = this.ClientRectangle;
            P1.X=r.X;
            P1.Y=r.Y;
            P2.X=r.Width;
            P2.Y=r.Height;
            g.DrawLine(new Pen(Color.LightSkyBlue, line_weight), P1, P2);
            P1.X = r.X;
            P1.Y = r.Height; 
            P2.X = r.Width;
            P2.Y = r.Y;
            g.DrawLine(new Pen(Color.LightSkyBlue, line_weight), P1, P2);
        }

        private void VistaButton_Resize(object sender, EventArgs e)
        {
            return;

        }

        private void VistaButton_MouseEnter(object sender, EventArgs e)//鼠标进入按钮
        {    
           // this.BackColor = SystemColors.ControlLight;
            line_weight = 2;
            this.Invalidate();
        }

        private void VistaButton_MouseLeave(object sender, EventArgs e)//鼠标离开按钮
        {
            this.BackColor = mBaseColor;
            line_weight = 1;
            this.Invalidate();
        }

        private void VistaButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
               // this.BackColor = SystemColors.ControlLight;
                this.Invalidate();
            }
        }
        private void VistaButton_MouseDown(object sender, MouseEventArgs e)//鼠标按下
        {
            if (e.Button == MouseButtons.Left)
            {
                this.BackColor = SystemColors.Control;
                line_weight = 1;
                this.Invalidate();
            }

        }




    }
}
