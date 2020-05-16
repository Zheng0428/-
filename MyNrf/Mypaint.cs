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
    public partial class Mypaint : UserControl
    {
        public Mypaint()
        {
            InitializeComponent();
        }
        bool MouseDownBool = false;
        bool WaveMul = false;
        bool WaveAuto = true;
        Point lastPoint = new Point();
        Point NowPoint = new Point();

        public class Demo : Label  
        {
            public Demo()
            {
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
                SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲


              
            }
        }

       public  Demo lblWave = new Demo();

      

        int boderWidth = 2;//边框大小

        public int MyBoder
        {
            get { return boderWidth; }
            set { boderWidth = value; }
        }

        Color boderColor = Color.Lime;//边框颜色

        public Color MyBoderColor
        {
            get { return boderColor; }
            set { boderColor = value; }
        }

        Color fontColor = Color.Blue;//字体颜色

        public Color MyFontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }

        Color WaveBacKColor = Color.Black;

        public Color MyWaveBackColor
        {
            get { return WaveBacKColor; }
            set { WaveBacKColor = value; }
        }



        int LSub = 55;//左边距

        public int MyLSub
        {
            get { return LSub; }
            set { LSub = value; }
        }

        int RSub = 20;//右边距

        public int MyRSub
        {
            get { return RSub; }
            set { RSub = value; }
        }

        int TSub = 10;//上边距

        public int MyTSub
        {
            get { return TSub; }
            set { TSub = value; }
        }

        int BSub = 20;//下边距

        public int MyBSub
        {
            get { return BSub; }
            set { BSub = value; }
        }

        int XSub = 5;//X偏差

        public int MyXSub
        {
            get { return XSub; }
            set { XSub = value; }
        }

        int YSub = 5;//Y偏差

        public int MyYSub
        {
            get { return YSub; }
            set { YSub = value; }
        }

        int KSub = 5;//刻度半长

        public int MyKSub
        {
            get { return KSub; }
            set { KSub = value; }
        }

        int MaxY = 100;//Y轴最大值
        int MaxY_Temp = 100;//Y轴最大值缓存值

        public int MyMaxY
        {
            get { return MaxY; }
            set { MaxY = value; MaxY_Temp = MaxY; }
        }

        int MinY = -100;//Y轴最小值
        int MinY_Temp = -100;//Y轴最小值缓存值

        public int MyMinY
        {
            get { return MinY; }
            set { MinY = value; MinY_Temp = MinY; }
        }


        int MaxX = 500;//X轴最大值
        int MaxX_Temp = 500;//X轴最大值缓存值


        public int MyMaxX
        {
            get { return MaxX; }
            set { MaxX = value; MaxX_Temp = MaxX; }
        }

        int MinX = 0;//X轴最小值
        int MinX_Temp = 0;//X轴最小值缓存值

        public int MyMinX
        {
            get { return MinX; }
            set { MinX = value; MinX_Temp = MinX; }
        }

        int XDiv = 10;//X分度

        public int MyXDiv
        {
            get { return XDiv; }
            set { XDiv = value; }
        }

        int YDiv = 10;//Y分度

        public int MyYDiv
        {
            get { return YDiv; }
            set { YDiv = value; }
        }

        int F_XLSub = 10;//X轴上的字体左偏度

        public int MyF_XLSub
        {
            get { return F_XLSub; }
            set { F_XLSub = value; }
        }

        int F_XTSub = 8;//X轴上的字体上偏度

        public int MyF_XTSub
        {
            get { return F_XTSub; }
            set { F_XTSub = value; }
        }


        int F_YLSub = 10;//Y轴上的字体左偏度

        public int MyF_YSub
        {
            get { return F_YLSub; }
            set { F_YLSub = value; }
        }

        int F_YTSub = 0;//Y轴上的字体上偏度

        public int MyF_YTSub
        {
            get { return F_YTSub; }
            set { F_YTSub = value; }
        }

        Size ParSize = new Size(15, 15);//波形参数字体颜色

        public Size MyParSize
        {
            get { return ParSize; }
            set { ParSize = value; }
        }

        int ParTop = 4;//波形参数字体上边距

        public int MyParTop
        {
            get { return ParTop; }
            set { ParTop = value; }
        }

        Font ParFont = new Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));//波形参数字体

        public Font MyParFont
        {
            get { return ParFont; }
            set { ParFont = value; }
        }

        Color ParNameColor = Color.White ;//波形参数名称显示颜色

        public Color MyParNameColor
        {
            get { return ParNameColor; }
            set { ParNameColor = value; }
        }

        Color ParValueColor = Color.YellowGreen ;//波形参数数字显示颜色

        public Color MyParValueColor
        {
            get { return ParValueColor; }
            set { ParValueColor = value; }
        }


        bool LogoBool = false;

        public bool MyLogo
        {
            get { return LogoBool; }
            set { LogoBool = value; }
        }

        Font LogoFont = new Font("隶书", 100);

        public Font MyLogoFont
        {
            get { return LogoFont; }
            set { LogoFont = value; }
        }

        string LogoText = "幽魂的示波器";

        public string MyLogoText
        {
            get { return LogoText; }
            set { LogoText = value; }
        }

        TextureBrush LogoBrush = new TextureBrush(MyNrf.Properties.Resources.金属纹理);

        public TextureBrush MyLogoBrush
        {
            get { return LogoBrush; }
            set { LogoBrush = value; }
        }

        Pen PenLine = new Pen(Color.Green, 1);

        public Pen MyPenLine
        {
            get { return PenLine; }
            set { PenLine = value; }
        }




        SolidBrush SBrushLineFontColor = new SolidBrush(Color.White);

        public SolidBrush MySBrushLineFontColor
        {
            get { return SBrushLineFontColor; }
            set { SBrushLineFontColor = value; }
        }



        Font LineFont = new Font("隶书", 10, FontStyle.Bold);

        public Font MyLineFont
        {
            get { return LineFont; }
            set { LineFont = value; }
        }

      




       


        public List<WaveData> Item = new List<WaveData>();


        Graphics g_back;


       
        public void DrawLogo()
        {
            LogoBool = true;
            lblWave.Invalidate(); 
        }

        public void CanelLogo()
        {
            LogoBool = false;
            lblWave.Invalidate(); 
        }

        public void DrawWave()
        {
            if (!(Item.Count > 0))
            {
                return;
            }

            g_back.Clear(BackColor);
            int Count;
            g_back.SmoothingMode = SmoothingMode.AntiAlias;
            g_back.PixelOffsetMode = PixelOffsetMode.HighQuality;
            float YSub = (MaxY - MinY);
            

            for (int i = 0; i < Item.Count; i++)
            {
                Count = (int)(Item[i].ListData.Count - MaxX - 1);
                if (Count > 0)
                {
                    Item[i].ListData.RemoveRange(Item[i].ListData.Count - Count, Count);
                }
                PointF[] point = new PointF[Item[i].ListData.Count];
                for (int j = 0; j < Item[i].ListData.Count; j++)
                {

                    point[j].X = lblWave.Image.Width * j / (MaxX);
                    point[j].Y = lblWave.Image.Height - (lblWave.Image.Height * (Item[i].ListData[j].Value * ClassGetObject.getMulValue(Item[i].ListData[j].MulIndex) - MinY) / YSub);
                }
                if (Item[i].ListData.Count >= 2)
                {
                    g_back.DrawLines(new Pen(Item[i].Color, Item[i].Width), point);
                }
                   
            }

            try
            {

                int LabelWidth = 0;
                int WidthTemp=0;
                for (int i = 0; i < Item.Count; i++)//寻找出宽度最大的
                {
                    WidthTemp = (int)g_back.MeasureString(Item[i].Name, ParFont).Width ;
                    if (LabelWidth < WidthTemp)
                    {
                        LabelWidth = WidthTemp;
                    }
                }

                LabelWidth += (int)(g_back.MeasureString(":" + "0".PadLeft(6), ParFont).Width );

                LabelWidth += ParSize.Width + 12;

                int R_Label_Count = 1;
  

                for (R_Label_Count = 1; ; R_Label_Count++)//统计一行需要多少个标签
                {
                    if (R_Label_Count * LabelWidth >= lblWave.Width)
                    {
                        R_Label_Count--;
                        if (R_Label_Count > Item.Count)
                        {
                            R_Label_Count = Item.Count;
                        }
                        break;
                    }
                }

                int StartX = ((lblWave.Width - R_Label_Count * LabelWidth) >> 1);

                int TopTemp = 0;
                int LeftTemp = 0;

                for (int i = 0; i < Item.Count; i++)
                {
                    if (Item[i].ListData.Count > 0)
                    {
                        LeftTemp = StartX + (i % R_Label_Count) * LabelWidth;
                        TopTemp = ParTop + (i / R_Label_Count) * (ParTop + ParSize.Width);

                        g_back.FillRectangle(new SolidBrush(Item[i].Color), new Rectangle(new Point(LeftTemp, TopTemp), ParSize));
                        LeftTemp += ParSize.Width + 2;
                        g_back.DrawString(Item[i].Name + ":", ParFont, new SolidBrush(ParNameColor), new PointF(LeftTemp, TopTemp));

                        LeftTemp += (int)(g_back.MeasureString(Item[i].Name + ":", ParFont).Width + 2);

                        g_back.DrawString((Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[i].MulIndex)).ToString("00000").PadLeft(6), ParFont, new SolidBrush(ParValueColor), new PointF(LeftTemp, TopTemp));
                    }
                }



                
            }
            catch 
            {
            
            }



            if (WaveAuto)
            {
                int Now_MaxY = 0;
                int Now_MinY = 0;
                for (int i = 0; i < Item.Count; i++)
                {
                    if (Item[i].ListData.Count > 0)
                    {
                        if (Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex)*5 > MaxY)
                        {
                            Now_MaxY = (int)((Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex)));
                        }
                        else if (Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex)*5 < MinY)
                        {
                            Now_MinY = (int)(Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex));
                        }
                    }
                }

                if (Now_MaxY > MaxY || Now_MinY < MinY)
                {
                    if (Now_MaxY > MaxY) MaxY = Now_MaxY;
                    if (Now_MinY < MinY) MinY = Now_MinY;

                    this.UcWaveShow_Resize(this, new EventArgs());
                    DrawWave();
                }
             
            }
            


            lblWave.Invalidate(); 


        }


        public void MyRefreshXY()
        {
            this.UcWaveShow_Resize(this, new EventArgs());
        }

        public void Paint_Inti() 
        {
            InitializeComponent();
        }

        private void UcWaveShow_Resize(object sender, EventArgs e)
        {
            Pen PLine = new Pen(boderColor, boderWidth);

            Bitmap BackGroundImasge = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(BackGroundImasge);

            //----------------------框-----------------------------
            g.DrawLine(PLine, LSub, TSub, BackGroundImasge.Width - RSub, TSub);
            g.DrawLine(PLine, LSub, BackGroundImasge.Height - BSub, BackGroundImasge.Width - RSub, BackGroundImasge.Height - BSub);
            g.DrawLine(PLine, LSub, TSub, LSub, BackGroundImasge.Height - BSub);
            g.DrawLine(PLine, BackGroundImasge.Width - RSub, TSub, BackGroundImasge.Width - RSub, BackGroundImasge.Height - BSub);

            int width = BackGroundImasge.Width - LSub - RSub;
            int height = BackGroundImasge.Height - BSub - TSub;

            g.FillRectangle(new SolidBrush(WaveBacKColor), new Rectangle(new Point(LSub + 1, TSub + 1), new Size(width - 2, height - 2)));



            //---------------------刻度------------------------------



            for (int i = 0; i <= XDiv; i++)
            {
                g.DrawLine(PLine, new Point(LSub + i * width / XDiv, height + TSub - KSub), new Point(LSub + i * width / XDiv, height + TSub + KSub));
                g.DrawString((i * MaxX / XDiv).ToString().PadLeft(3), lblWave.Font, new SolidBrush(fontColor), new Point(LSub + i * width / XDiv - F_XLSub, height + TSub + F_XTSub));
            }


            float Y = (MaxY - MinY);

            for (int i = 0; i <= MyYDiv; i++)
            {
                g.DrawLine(PLine, new Point(LSub - KSub, TSub + height * i / MyYDiv), new Point(LSub + KSub, TSub + height * i / MyYDiv));
                g.DrawString(((MyYDiv - i) * Y / YDiv + MinY).ToString().PadLeft(6), lblWave.Font, new SolidBrush(fontColor), new Point(F_YTSub, TSub + height * i / MyYDiv - F_XTSub));
            }

            lblWave.Size = new Size(width - 2, height - 2);
            lblWave.Top = TSub + 1;
            lblWave.Left = LSub + 1;
            lblWave.Image = new Bitmap(width - 2, height - 2);
            lblWave.BackColor = Color.Transparent;
            this.Controls.Add(lblWave);

            this.BackgroundImage = BackGroundImasge;

            lblWave.Image = new Bitmap((int)(lblWave.Image.Width), (int)(lblWave.Image.Height));
          
            g_back = Graphics.FromImage(lblWave.Image);

            GC.Collect();
        }

      

        private void UcWaveShow_MouseDown(object sender, MouseEventArgs e)
        {
                if (e.Button == MouseButtons.Right)
                {
                    Point point=this.PointToScreen(lblWave.Location);
                    contextMenuStrip1.Show(e.X + point.X, e.Y + point.Y);
                }
          
                else if (e.Button == MouseButtons.Left)
                {
                    if (contextMenuchange1.Checked)
                    {
                        if (MouseDownBool == false)
                        {
                            MouseDownBool = true;
                            NowPoint.X = 0;
                            NowPoint.Y = 0;
                            lastPoint.X = e.X;
                            lastPoint.Y = e.Y;
                        }
                    }

                }
        }

        private void UcWaveShow_Load(object sender, EventArgs e)
        {
            lblWave.MouseDown += new MouseEventHandler(UcWaveShow_MouseDown);
            lblWave.MouseMove += new MouseEventHandler(UcWaveShow_MouseMove);
            lblWave.MouseUp += new MouseEventHandler(UcWaveShow_MouseUp);
            lblWave.Paint += new PaintEventHandler(UcWaveShow_Paint);
            lblWave.MouseLeave += new EventHandler(UcWaveShow_MouseLeave);
        }

        private void tcmsChage_Click(object sender, EventArgs e)
        {
            this.contextMenuchange1.Checked = !this.contextMenuchange1.Checked;
        }

        public Point et = new Point();

        private void UcWaveShow_MouseMove(object sender, MouseEventArgs e)
        {
            et.X = e.X ;
            et.Y = e.Y;
            if (MouseDownBool)
            {
                NowPoint.X = e.X;
                NowPoint.Y = e.Y;
                if (NowPoint.X - lastPoint.X > 10 && NowPoint.Y - lastPoint.Y > 10)
                {
                    WaveMul = true;
                    lblWave.Invalidate();
                    return;
                }
            }
            if (ToolStripMenuItem1.Checked == true)
            {
                lblWave.Invalidate();
            }
            
           
           
        }
        //public bool LineBool = false;

        private void UcWaveShow_MouseLeave(object sender, EventArgs e)
        {
            et.X=et.Y  = -1;
            if (ToolStripMenuItem1.Checked == true)
            {
                lblWave.Invalidate();
            }
        }

       

        private void UcWaveShow_Paint(object sender, PaintEventArgs e)
        {
           if (LogoBool)
            {
                //使用图像填充文字线条  
               
                Graphics g = e.Graphics;
                SizeF temp=  g.MeasureString(LogoText, LogoFont );
                g.DrawString(LogoText, LogoFont, LogoBrush, new PointF((lblWave.Width - temp.Width) / 2, (lblWave.Height  - temp.Height ) / 2)); 
            }
            if (MouseDownBool)
            {
                Graphics g = e.Graphics;
                g.DrawRectangle(new Pen(Color.Yellow, 1), lastPoint.X, lastPoint.Y, NowPoint.X - lastPoint.X, NowPoint.Y - lastPoint.Y);
                g.FillRectangle(new SolidBrush(Color.FromArgb(90, 0, 255, 0)), new Rectangle(lastPoint, new Size(NowPoint.X - lastPoint.X, NowPoint.Y - lastPoint.Y)));
            }
            if (ToolStripMenuItem1.Checked == true)
            {
                if (et.X >= 0 && et.X < lblWave.Width && et.Y >= 0 && et.Y < lblWave.Height)
                {
                    Graphics g = e.Graphics;
                    g.DrawLine(PenLine, 0, et.Y, lblWave.Width, et.Y);
                    g.DrawLine(PenLine, et.X, 0, et.X, lblWave.Height);
                    string Str=(((et.X + 1) * (MaxX - MinX) / (lblWave.Width * 1.0) + MinX).ToString("00000.0") + "," + ((lblWave.Height - et.Y) * (MaxY - MinY) / (lblWave.Height * 1.0) + MinY).ToString("00000.0"));
                    SizeF size=g.MeasureString(Str, LineFont);

                    if (et.X + size.Width < lblWave.Width - 2 && et.Y - size.Height > 2)
                    {
                        g.DrawString(Str, LineFont, SBrushLineFontColor, et.X + 1, (int)(et.Y - size.Height - 2));
                    }
                    else if (et.X + size.Width > lblWave.Width - 2 && et.Y - size.Height > 2)
                    {
                        g.DrawString(Str, LineFont, SBrushLineFontColor, et.X - size.Width-1, (int)(et.Y - size.Height - 2));
                    }
                    else if (et.X + size.Width > lblWave.Width - 2 && et.Y - size.Height < 2)
                    {
                        g.DrawString(Str, LineFont, SBrushLineFontColor, et.X - size.Width - 1, (int)(et.Y + 2));
                    }
                    else
                    {
                        g.DrawString(Str, LineFont, SBrushLineFontColor, et.X + 1, (int)(et.Y + 2));
                    }
                    
                }
            }
           
        }

        private void UcWaveShow_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDownBool = false;
            if (WaveMul)
            {
                WaveMul = false;
                int Y_SUB_Temp = MaxY - MinY;
                if (Y_SUB_Temp > 10)
                {

                    MaxY = Y_SUB_Temp * (lblWave.Height-lastPoint.Y )/ lblWave.Height + MinY;
                    MinY = Y_SUB_Temp * (lblWave.Height- NowPoint.Y)/ lblWave.Height + MinY;
                    WaveAuto = false;
                }

                int XMax_Temp = MaxX * NowPoint.X  / lblWave.Width;
               
               
                if (XMax_Temp > 10)
                {
                    try
                    {
                        int CountSub;
                        for (int i = 0; i < Item.Count; i++)
                        {
                            CountSub = Item[i].ListData.Count - XMax_Temp;
                            if (CountSub > 0)
                            {
                                Item[i].ListData.RemoveRange(XMax_Temp, CountSub);
                            }
                        }
                    }
                    catch 
                    {
                    }
                    MaxX = XMax_Temp;
                    WaveAuto = false;
                }
                this.UcWaveShow_Resize(this, new EventArgs());
                DrawWave();
            }

        }

        private void tcmsDefult_Click(object sender, EventArgs e)
        {
            MaxX = MaxX_Temp;
            MinX = MinX_Temp;
            MaxY = MaxY_Temp;
            MinY = MinY_Temp;
            WaveAuto = true;
            contextMenuchange1.Checked = false;
            this.UcWaveShow_Resize(this, new EventArgs());
            DrawWave();

        }

        private void 坐标辅助显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem1.Checked = !ToolStripMenuItem1.Checked;
        }


    }
}
