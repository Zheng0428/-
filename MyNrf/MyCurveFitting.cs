using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; 
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;//引用DLL申明
using MyNrf.WIN32所需要使用的API;

namespace MyNrf
{
    public partial class MyCurveFitting : Form
    {
        private System.Drawing.Point mousePosition;//记录鼠标坐标
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }        //DLL申明      
        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);        //DLL申明       
        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern bool DwmIsCompositionEnabled();        //直接添加代码   

        MyMathXYData Data = new MyMathXYData();
        MyMathXYData Result = new MyMathXYData();
        protected override void OnLoad(EventArgs e)
        {
            if (DwmIsCompositionEnabled())
            {
                MARGINS margins = new MARGINS();
                margins.Right = -1;
                margins.Left = -1;
                margins.Top = -1;
                margins.Bottom = -1;
                DwmExtendFrameIntoClientArea(this.Handle, ref margins);
            }
            base.OnLoad(e);
        }       

        private void MyCurveFitting_Resize(object sender, EventArgs e)
        {
            waveshow();
        }
        public MyCurveFitting()
        {
            InitializeComponent();
            this.Width = 1000;
            this.Height = 600;
            //this.BackColor = Color.FromArgb(50, 50, 70);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.BackColor = System.Drawing.Color.FromArgb(0, 0, 0);
            waveshow();
            this.Invalidate();
        }
        //鼠标左键单击窗体事件
        private void Top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePosition.X = e.X + 10;
                this.mousePosition.Y = e.Y + 30;
            }


        }
        //鼠标左键单击窗体并拖动事件
        private void Top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - mousePosition.Y;
                this.Left = Control.MousePosition.X - mousePosition.X;
            }
        }
        private void MyCurveFitting_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Pen p = new Pen(Color.FromArgb(0, 0, 0), 1);
            drawwave(p, g);

        }
        private float Y_Max = 1000;
        private float X_Max = 1000;
        private float Y_Min = 0;
        private float X_Min = 0;
        private void drawcalibration(Pen p, Graphics g)
        {


            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Near;
            stringFormat.Alignment = StringAlignment.Far;
            for (int i = 0; i <= 10; i++)
            {
                g.DrawString(((Y_Max - Y_Min) * (10 - i) / 10 + Y_Min).ToString(), this.Font, Brushes.Black, new PointF(70, 45 + (this.Height - 230) * i / 10 - (this.Height - 230) / 60), stringFormat);
                g.DrawString(((X_Max - X_Min) * (10 - i) / 10 + X_Min).ToString(), this.Font, Brushes.Black, new PointF(75 + (this.Width - 100) * (10 - i) / 10, this.Height - 180), stringFormat);
            }



        }

        private void drawwave(Pen p, Graphics g)
        {
            if (Data.DataPoint.Count < 2)
            {
                return;
            }
            else
            {
                Point[] point = new Point[Data.DataPoint.Count];
                for (int i = 0; i < Data.DataPoint.Count; i++)
                {
                    point[i].X = (int)((Data.DataPoint[i].X_Value - X_Min) * (this.Width - 100) / (X_Max - X_Min) + 70);
                    point[i].Y = (int)((Y_Max - Data.DataPoint[i].Y_Value) * (this.Height - 230) / (Y_Max - Y_Min) + 50);
                }
                g.DrawLines(p, point);

            }
            if (Result.DataPoint.Count < 2)
            {
                return;
            }
            else
            {
                Point[] point = new Point[Result.DataPoint.Count];
                for (int i = 0; i < Result.DataPoint.Count; i++)
                {
                    point[i].X = (int)((Result.DataPoint[i].X_Value - X_Min) * (this.Width - 100) / (X_Max - X_Min) + 70);
                    point[i].Y = (int)((Y_Max - Result.DataPoint[i].Y_Value) * (this.Height - 230) / (Y_Max - Y_Min) + 50);
                }
                try
                {
                    p.Color = Color.Blue;
                    g.DrawLines(p, point);
                }
                catch
                {
                    MessageBox.Show("数据错误");
                    return;                   
                }

            }
        }
        private void waveshow()
        {
            if (DwmIsCompositionEnabled())
            {
                MARGINS margins = new MARGINS();
                margins.Right = -1;
                margins.Left = -1;
                margins.Top = -1;
                margins.Bottom = -1;
                DwmExtendFrameIntoClientArea(this.Handle, ref margins);
            }
            Bitmap backgroundimage = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(backgroundimage);
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle rec1 = new Rectangle(70, 50, this.Width - 100, this.Height - 230);
            Pen p = new Pen(Color.FromArgb(0, 0, 0), 1);
            LinearGradientBrush lgb1 = new LinearGradientBrush(
              new System.Drawing.Point(29, 0),
              new System.Drawing.Point(70, 0),
              Color.FromArgb(0, Color.Transparent),
              Color.FromArgb(180, 255, 255, 255));

            LinearGradientBrush lgb2 = new LinearGradientBrush(
             new System.Drawing.Point(0, 40),
             new System.Drawing.Point(0, 50),
            Color.FromArgb(0, Color.Transparent),
            Color.FromArgb(180, 255, 255, 255));

            LinearGradientBrush lgb3 = new LinearGradientBrush(
             new System.Drawing.Point(0, this.Height - 160),
             new System.Drawing.Point(0, this.Height - 180),
            Color.FromArgb(0, Color.Transparent),
            Color.FromArgb(180, 255, 255, 255));

            LinearGradientBrush lgb4 = new LinearGradientBrush(
            new System.Drawing.Point(this.Width - 20, 0),
            new System.Drawing.Point(this.Width - 30, 0),
            Color.FromArgb(0, Color.Transparent),
            Color.FromArgb(180, 255, 255, 255));



            Rectangle rec2 = new Rectangle(30, 50, 40, this.Height - 230);
            Rectangle rec3 = new Rectangle(70, 40, this.Width - 100, 10);
            Rectangle rec4 = new Rectangle(70, this.Height - 180, this.Width - 100, 20);
            Rectangle rec5 = new Rectangle(this.Width - 30, 50, 10, this.Height - 230);



            g.FillRectangle(new SolidBrush(Color.FromArgb(180, 255, 255, 255)), rec1);
            g.FillRectangle(lgb1, rec2);
            g.FillRectangle(lgb2, rec3);
            g.FillRectangle(lgb3, rec4);
            g.FillRectangle(lgb4, rec5);

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(30, 40, 80, 20);
            PathGradientBrush pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = Color.FromArgb(180, 255, 255, 255);
            pthGrBrush.SurroundColors = new Color[] { Color.FromArgb(0, Color.Transparent) };
            g.FillPie(pthGrBrush, 30, 40, 80, 20, 180, 90);

            path = new GraphicsPath();
            path.AddEllipse(30, this.Height - 200, 80, 40);
            pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = Color.FromArgb(180, 255, 255, 255);
            pthGrBrush.SurroundColors = new Color[] { Color.FromArgb(0, Color.Transparent) };
            g.FillPie(pthGrBrush, 30, this.Height - 200, 80, 40, 90, 90);

            path = new GraphicsPath();
            path.AddEllipse(this.Width - 40, this.Height - 200, 20, 40);
            pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = Color.FromArgb(180, 255, 255, 255);
            pthGrBrush.SurroundColors = new Color[] { Color.FromArgb(0, Color.Transparent) };
            g.FillPie(pthGrBrush, this.Width - 40, this.Height - 200, 20, 40, 0, 90);

            path = new GraphicsPath();
            path.AddEllipse(this.Width - 40, 40, 20, 20);
            pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = Color.FromArgb(180, 255, 255, 255);
            pthGrBrush.SurroundColors = new Color[] { Color.FromArgb(0, Color.Transparent) };
            g.FillPie(pthGrBrush, this.Width - 40, 40, 20, 20, 270, 90);


            g.DrawLine(p, 70, 50, 70, this.Height - 180);
            g.DrawLine(p, 70, this.Height - 180, this.Width - 30, this.Height - 180);


            p.Color = Color.FromArgb(150, 150, 100);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            p.DashPattern = new float[] { 5f, 5f };
            for (int i = 0; i < 10; i++)
            {
                g.DrawLine(p, 70, 50 + (this.Height - 230) * i / 10, this.Width - 30, 50 + (this.Height - 230) * i / 10);
            }
            for (int i = 0; i < 10; i++)
            {
                g.DrawLine(p, 70 + (this.Width - 100) * (i + 1) / 10, 50, 70 + (this.Width - 100) * (i + 1) / 10, this.Height - 180);
            }
            p.DashPattern = new float[] { 2f, 2f };
            for (int i = 0; i < 10; i++)
            {
                g.DrawLine(p, 70, 50 + (this.Height - 230) * (2 * i + 1) / 20, this.Width - 30, 50 + (this.Height - 230) * (2 * i + 1) / 20);
            }
            for (int i = 0; i < 10; i++)
            {
                g.DrawLine(p, 70 + (this.Width - 100) * (2 * i + 1) / 20, 50, 70 + (this.Width - 100) * (2 * i + 1) / 20, this.Height - 180);
            }
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            drawcalibration(p, g);          
            this.BackgroundImage = backgroundimage;

            GC.Collect();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(fileDialog.FileName);
                StreamReader sr = file.OpenText();
                string s;
                MyMathXYData tmpdata=new MyMathXYData();
                while ((s = sr.ReadLine()) != null)
                {
                    //  解析数据
                    tmpdata.Add(s);
                }
              //  Data.DataPoint = tmpdata.XLinearFilter(1000);
                Data.DataPoint = tmpdata.DataPoint;
                X_Max = (float)Data.getMaxX() + 3;
                X_Min = (float)Data.getMinX() - 3;
                Y_Max = (float)Data.getMaxY() + 3;
                Y_Min = (float)Data.getMinY() - 3;
                waveshow();
                this.Invalidate();
                sr.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            double[] tmpx = Data.XToArray();
            double[] tmpy = Data.YToArray();

            MyMathXYData tmpdata = new MyMathXYData();

            //MyMath.Filter.ButtordFilter(ref tmpy, 2, 5, 10);

            //double[] pfit = MyMath.Filter.polyfit(tmpx, tmpy, 25);
            //for (int i = 0; i < tmpy.Length; i++)
            //{
            //    tmpy[i] = MyMath.Filter.MyPolyVal(pfit, tmpx[i]);
            //}



            double[] tmpyl = new double[tmpy.Length / 2];
            double[] tmpyh = new double[tmpy.Length / 2];
            for (int i = 0; i < tmpyh.Length; i++)
            {
                tmpyh[tmpyh.Length - i - 1] = tmpy[i];
            }
            for (int i = 0; i < tmpyl.Length; i++)
            {
                tmpyl[tmpyl.Length - i - 1] = tmpy[tmpy.Length - 1 - i];
            }
            MyMath.Filter.PolyFilter(ref tmpyh, 8, 1);
            MyMath.Filter.PolyFilter(ref tmpyl, 8, 1);
            for (int i = 0; i < tmpyh.Length; i++)
            {
                tmpy[i] = tmpyh[tmpyh.Length - i - 1];
            }
            for (int i = 0; i < tmpyl.Length; i++)
            {
                tmpy[tmpy.Length - 1 - i] = tmpyl[tmpyl.Length - i - 1];
            }




            tmpdata.setdata(tmpx, tmpy);
            Result.DataPoint = tmpdata.XLinearFilter(1000);
            X_Max = ((float)Result.getMaxX() + 3) > X_Max ? (float)Result.getMaxX() + 3 : X_Max;
            X_Min = ((float)Result.getMinX() - 3) < X_Min ? (float)Result.getMinX() - 3 : X_Min;
            Y_Max = ((float)Result.getMaxY() + 3) > Y_Max ? (float)Result.getMaxY() + 3 : Y_Max;
            Y_Min = ((float)Result.getMinY() - 3) < Y_Min ? (float)Result.getMinY() - 3 : Y_Min;
            waveshow();
            this.Invalidate();


            //OpenFileDialog fileDialog = new OpenFileDialog();
            //fileDialog.Multiselect = true;
            //fileDialog.Title = "请选择文件参数文件";
            //fileDialog.Filter = "所有文件(*.*)|*.*";
            //if (fileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    FileInfo file = new FileInfo(fileDialog.FileName);
            //    StreamReader sr = file.OpenText();
            //    string s;
            //    MyMathXYData par = new MyMathXYData();
            //    while ((s = sr.ReadLine()) != null)
            //    {
            //        //  解析数据
            //        par.Add(s);
            //    }
            //    double[] az = par.XToArray();
            //    double[] bz = par.YToArray();
            //    MyMath.Filter.FilterFilter(ref tmpy, az, bz);
            //    tmpdata.setdata(tmpx, tmpy);
            //    Result.DataPoint = tmpdata.XLinearFilter(1000);
            //    X_Max = ((float)Result.getMaxX() + 1) > X_Max ? (float)Result.getMaxX() + 1 : X_Max;
            //    X_Min = ((float)Result.getMinX() - 1) < X_Min ? (float)Result.getMinX() - 1 : X_Min;
            //    Y_Max = ((float)Result.getMaxY() + 1) > Y_Max ? (float)Result.getMaxY() + 1 : Y_Max;
            //    Y_Min = ((float)Result.getMinY() - 1) < Y_Min ? (float)Result.getMinY() + 1 : Y_Min;
            //    this.Invalidate();
            //    sr.Close();
            //}


        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {

        }













    }
}
