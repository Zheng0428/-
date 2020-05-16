using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MyNrf
{
    public partial class MyTrackBar : UserControl
    {
        private int barValue = 0;
        private int maxBarValue;
        bool tracing = false;

        private Color darker1 = Color.FromArgb(11, 32, 160);
        public Color Darker1 
        {
            get { return darker1; }
            set { darker1 = value; }
        }
        private Color lighter1 = Color.FromArgb(221, 210, 227);
        public Color Lighter1
        {
            get { return lighter1; }
            set { lighter1 = value; }
        }
        private Color darker2 = Color.FromArgb(203, 83, 247);
        public Color Darker2
        {
            get { return darker2; }
            set { darker2 = value; }
        }
        private Color lighter2 = Color.FromArgb(238, 227, 242);
        public Color Lighter2
        {
            get { return lighter2; }
            set { lighter2 = value; }
        }
        private Color dark = Color.FromArgb(0, 20, 255);
        public Color Dark
        {
            get { return dark; }
            set { dark = value; }
        }

        private Pen blackPen = new Pen(Color.Black, 2);

        private Color pencolor = Color.Black;
        public Color PenColor 
        {
            get { return pencolor; }
            set { pencolor = value; }
        }
        private float penwidth =2;
        public float PenWidth
        {
            get { return penwidth; }
            set { penwidth = value; }
        }

        Point lUpPoint;
        Point mickeyMouse;

        Color trackerColor1;
        Color trackerColor2;

        public int clientHeight;
        private int clientWidth;

        public MyTrackBar()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Name = "TrackBar";
            this.Size = new System.Drawing.Size(100, 38);
            this.ResumeLayout(false);
            this.blackPen.Color = pencolor;
            this.blackPen.Width = penwidth;
            trackerColor1 = darker1;
            trackerColor2 = lighter1;

            clientHeight = this.ClientRectangle.Height;
            clientWidth = this.ClientRectangle.Width;

            maxBarValue = clientWidth - clientHeight / 2;
        }

        public int BarValue   //bar 的当前值 
        {
            get
            {
                return barValue;
            }
            set
            {
                barValue = value;
            }
        }

        public int MaxBarValue  //bar 的最大值 
        {
            get
            {
                return maxBarValue;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            g.DrawLine(blackPen, clientWidth / 8, 0, 7 * clientWidth/8,0);
            g.DrawLine(blackPen, clientWidth / 8, clientHeight, 7 * clientWidth / 8, clientHeight);
            g.DrawLine(blackPen, clientWidth/2, 0, clientWidth / 2, clientHeight);  

           // ///画外框            
           // Rectangle outRect = new Rectangle(0, clientHeight / 3, clientWidth, clientHeight / 3);
           // LinearGradientBrush brush = new LinearGradientBrush(outRect, darker1, lighter1,
           //     LinearGradientMode.Vertical);
           // g.FillRectangle(brush, outRect);
           // g.DrawRectangle(blackPen, outRect);

           // ///画内框上边 
           // Rectangle uInRect = new Rectangle(clientHeight / 8, 3 * clientHeight / 8, clientWidth - clientHeight / 4,
           //     clientHeight / 8);
           // LinearGradientBrush brush2 = new LinearGradientBrush(uInRect, lighter1, darker1,
           //    LinearGradientMode.Vertical);
           // g.FillRectangle(brush2, uInRect);
           // g.DrawRectangle(blackPen, uInRect);
           // ///画内框下边 
           // Rectangle dInRect = new Rectangle(clientHeight / 8, clientHeight / 2, clientWidth - clientHeight / 4,
           //     clientHeight / 8);
           // LinearGradientBrush brush3 = new LinearGradientBrush(dInRect, darker1, lighter1,
           //     LinearGradientMode.Vertical);
           // g.FillRectangle(brush3, dInRect);
           //g.DrawRectangle(blackPen, dInRect);

           // brush.Dispose();
           // brush2.Dispose();
           // brush3.Dispose();
           // ///画游码 
            lUpPoint = new Point(clientHeight / 8 + barValue, clientHeight / 6);
            DrawTracker(g, BarValue);
        }

        private void DrawTracker(Graphics g, int nValue)
        {
            Point[] p=new Point[6];
            p[0].X=0;
            p[0].Y = clientHeight - nValue - 5;
            p[1].X = clientWidth / 4;
            p[1].Y = clientHeight - nValue-2;
            p[2].X = 3*clientWidth / 4;
            p[2].Y = clientHeight - nValue-2;
            p[3].X = clientWidth ;
            p[3].Y = clientHeight - nValue - 5;
            p[4].X = 3 * clientWidth / 4;
            p[4].Y = clientHeight - nValue - 8;
            p[5].X = clientWidth / 4;
            p[5].Y = clientHeight - nValue - 8;

            g.DrawPolygon(blackPen, p);

            //Rectangle rect = new Rectangle(clientHeight / 8 + nValue, clientHeight / 6,
            //    clientHeight / 4, 2 * clientHeight / 3);
            //LinearGradientBrush brush = new LinearGradientBrush(rect, trackerColor1, trackerColor2,
            //    LinearGradientMode.Vertical);
 
            //g.FillRectangle(brush, rect);
            //g.DrawRectangle(blackPen, rect);

            //brush.Dispose();

        }


        private void TrackBar_Resize(object sender, EventArgs e) 
        {
            this.blackPen.Color = pencolor;
            this.blackPen.Width = penwidth;
            trackerColor1 = darker1;
            trackerColor2 = lighter1;
            Invalidate();
        }

        ///  
        /// 鼠标键放开 
        ///  
        ///  
        ///  
        private void TrackBar_MouseClick(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            trackerColor1 = darker1;
            trackerColor2 = lighter1;
            tracing = false;
            Invalidate();
        }

        ///  
        /// 鼠标键点下 
        ///  
        ///  
        ///  
        private void TrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            mickeyMouse = e.Location;
            Rectangle rect1 = new Rectangle(lUpPoint.X, lUpPoint.Y, clientHeight / 4, 2 * clientHeight / 3);
            Rectangle rect2 = new Rectangle(0, clientHeight/3, clientWidth, clientHeight/3);
            if (rect1.Contains(e.Location))
            {
                Cursor = Cursors.Hand;
                trackerColor1 = darker2;
                trackerColor2 = lighter2;
                tracing = true;
                this.Invalidate();
            }
            else if (rect2.Contains(e.Location))
            {
                Cursor = Cursors.Hand;
                BarValue = mickeyMouse.X - clientHeight/4;
                this.Invalidate();
            }
        }

        ///  
        /// 鼠标拖动 
        ///  
        ///  
        ///  
        private void TrackBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && tracing)
            {
                Point mousePos = e.Location;
                int temp = BarValue + (mousePos.X - mickeyMouse.X);
                if (temp >= 0 && temp <= clientWidth - clientHeight / 2)
                {
                    BarValue += (mousePos.X - mickeyMouse.X);
                    mickeyMouse.X = mousePos.X;
                }
                this.Invalidate();
            }
        }

        ///  
        /// 鼠标离开 
        ///  
        ///  
        ///  
        private void TrackBar_MouseLeave(object sender, EventArgs e)
        {
            trackerColor1 = darker1;
            trackerColor2 = lighter1;
            Invalidate();
        }

        private void TrackBar_SizeChanged(object sender, EventArgs e)
        {
            clientHeight = this.ClientRectangle.Height;
            clientWidth = this.ClientRectangle.Width;
            maxBarValue = clientWidth - clientHeight / 2;
        }
    }
}
