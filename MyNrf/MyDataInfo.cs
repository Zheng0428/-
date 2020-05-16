using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO.Ports;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;
using CsGL.OpenGL;
using System.Drawing;
using System.Windows.Forms;
using MyNrf.WIN32所需要使用的API;
using MyNrf.OpenGL的一些操作;



namespace MyNrf
{
    class MyDataInfo : OpenGLControl
    {
        public List<LCR> Lcr = new List<LCR>();
        public List<ClassConstParValue> DataParValue = new List<ClassConstParValue>();
        public List<ClassConstParValue> DataConstValue = new List<ClassConstParValue>();
        private System.Drawing.Point mousePosition;
        private MyMouse MouseFlag = MyMouse.None;
        private int LcrEye_y = 0;
        private int ParEye_y = 0;
        private int ConstEye_y = 0;
        private int tmp_eye_y = 0;
        private bool MouseDown_Flag = false;
        private int repaint_cnt = 0;
        private bool Show_Pic_Info = true;
        private List<Rectangle> ConstParRect = new List<Rectangle>();
        public bool glfouse = false;
        public int constdatatabnum = 0;
        public DataInfoListupdatamsg UpDataInfoFrom = new DataInfoListupdatamsg();
        public float SendData = 0;
        ContextMenuStrip contextMenuStrip1;
        ToolStripMenuItem CopyLine;
        private System.ComponentModel.IContainer components = null;
        public MyDataInfo()
        {

            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopyLine = new ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.CopyLine });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 92);
            this.CopyLine.Text = "复制行距";
            this.CopyLine.Size = new System.Drawing.Size(154, 22);
            this.CopyLine.Click += new System.EventHandler(this.CopyLine_Click);
            this.contextMenuStrip1.ResumeLayout(false);



                for (int i = 0; i <70; i++)
                {
                    Lcr.Add(new LCR(255, 255, 255, 255, 255, 255, 255));
                }

                windowHandle = this.Handle;
                deviceContextHandle = Win32.GetDC(windowHandle);
                Win32.PIXELFORMATDESCRIPTOR pfd = new Win32.PIXELFORMATDESCRIPTOR();
                renderContextHandle = Win32.wglCreateContext(deviceContextHandle);

                this.MouseDown += new MouseEventHandler(MyDataInfo_MouseDown);
                this.MouseMove += new MouseEventHandler(MyDataInfo_MouseMove);
                this.MouseUp += new MouseEventHandler(MyDataInfo_MouseUp);
                this.MouseWheel += new MouseEventHandler(MyDataInfo_MouseWheel);
                this.ResizeRedraw = true;
                this.Resize += new EventHandler(MyDataInfo_Resize);
  
        }
        private void MyWaveUpdata()
        {
            if (MouseDown_Flag == true)
            {
                repaint_cnt++;
                if (repaint_cnt == 6)
                {
                    repaint_cnt = 0;
                    this.Invalidate();
                }

            }
            else
            {
                this.Invalidate();
            }

        }
        private void CopyLine_Click(object sender, EventArgs e)
        {
            if (Lcr.Count != 0)
            {
                string tmptext = "uint8_t fixValue[]=" + "\r\n" + "{";
                for (int i = 0; i < Lcr.Count; i++)
                {
                    if (i % 10 == 0)
                    {
                        tmptext += "\r\n"+"    ";
                    }
                    tmptext += (Lcr[i].LBlack - Lcr[i].RBlack).ToString() + ",";
                }
                tmptext += "\r\n" + "};";
                Clipboard.SetDataObject(tmptext);
            }
        }
        private void MyDataInfo_MouseDown(object sender, MouseEventArgs e) 
        {
            this.Focus();
            MouseDown_Flag = true;
            Rectangle rect1 = new Rectangle(360,  LcrEye_y, 25, (int)((float)this.Height  * 12 / 70));
            Rectangle rect2 = new Rectangle(660, ParEye_y, 25, (int)((float)this.Height * (Par_Num > 12 ? 12 : Par_Num) / Par_Num));
            Rectangle rect3 = new Rectangle(967, ConstEye_y, 25, (int)((float)this.Height * (Const_Num > 12 ? 12 : Const_Num) / Const_Num));
            Rectangle rect4 = new Rectangle(700,  30, 260, this.Height - 30);
            Rectangle rect5 = new Rectangle(0, 30, 260, this.Height - 30);
            if (e.Button == MouseButtons.Left)
            {
                if (rect1.Contains(e.Location))//滚动条
                {
                    //this.Cursor = Form1.MyCursor.Hand;
                    MouseFlag = MyMouse.LeftFlag1;//滚动条1
                    this.mousePosition.X = e.X;// Control.MousePosition.X - this.Location.X;
                    this.mousePosition.Y = e.Y;// Control.MousePosition.Y - this.Location.Y;
                    tmp_eye_y = LcrEye_y;
                }
                if (rect2.Contains(e.Location))//滚动条
                {
                    //this.Cursor = Form1.MyCursor.Hand;
                    MouseFlag = MyMouse.LeftFlag2;//滚动条2
                    this.mousePosition.X = e.X;// Control.MousePosition.X - this.Location.X;
                    this.mousePosition.Y = e.Y;// Control.MousePosition.Y - this.Location.Y;
                    tmp_eye_y = ParEye_y;
                }
                if (rect3.Contains(e.Location))//滚动条
                {
                    //this.Cursor = Form1.MyCursor.Hand;
                    MouseFlag = MyMouse.LeftFlag3;//滚动条3
                    this.mousePosition.X = e.X;// Control.MousePosition.X - this.Location.X;
                    this.mousePosition.Y = e.Y;// Control.MousePosition.Y - this.Location.Y;
                    tmp_eye_y = ConstEye_y;
                }
                glfouse = false;
                constdatatabnum = 0;
                if (rect4.Contains(e.Location))
                {
                    for (int i = 0; i < ConstParRect.Count; i++)
                    {
                        if (ConstParRect[i].Contains(e.Location))
                        {
                            constdatatabnum = i;
                            SendData = DataConstValue[i].Value;
                            glfouse = true;
                        }
                    }
                }
                UpDataInfoFrom.UDDrawConstChooseList = true;
                this.MyInvalidate();
            }
            else if (e.Button == MouseButtons.Right) 
            {
                if (rect5.Contains(e.Location)) 
                {
                    MouseFlag = MyMouse.RightFlag1;
                }
            }

        }
        private void MyDataInfo_MouseMove(object sender, MouseEventArgs e) 
        {
            if (MouseFlag == MyMouse.LeftFlag1) 
            {
                LcrEye_y = (e.Y - mousePosition.Y) + tmp_eye_y;
                if (LcrEye_y < 0) LcrEye_y = 0;
                if (LcrEye_y > this.Height - this.Height * 12 / 70) LcrEye_y = this.Height - this.Height * 12 / 70;
                UpDataInfoFrom.UDLcrScrollBarsList = true;
                MyWaveUpdata();
                return;
            }
            if (MouseFlag == MyMouse.LeftFlag2)
            {
                ParEye_y = (e.Y - mousePosition.Y) + tmp_eye_y;
                if (Par_Num < 12)
                {
                    ParEye_y = 0;
                }
                else
                {
                    if (ParEye_y < 0) ParEye_y = 0;
                    if (ParEye_y > this.Height - this.Height *12 / Par_Num) ParEye_y = this.Height - this.Height *12 / Par_Num;
                }
                UpDataInfoFrom.UDParScrollBarsList = true;
                MyWaveUpdata();
                return;
            }
            if (MouseFlag == MyMouse.LeftFlag3)
            {
                ConstEye_y = (e.Y - mousePosition.Y) + tmp_eye_y;
                if (Const_Num < 12)
                {
                    ConstEye_y = 0;
                }
                else
                {
                    if (ConstEye_y < 0) ConstEye_y = 0;
                    if (ConstEye_y > this.Height - this.Height * 12 / Const_Num) ConstEye_y = this.Height - this.Height * 12 / Const_Num;
                }
                UpDataInfoFrom.UDConstScrollBarsList = true;
                MyWaveUpdata();
                return;
            }

        }
        private void MyDataInfo_MouseUp(object sender, MouseEventArgs e) 
        {
            //this.Cursor = Form1.MyCursor.Default;
            MouseDown_Flag =false;
            if (MouseFlag == MyMouse.RightFlag1) 
            {
                contextMenuStrip1.Show(Control.MousePosition.X, Control.MousePosition.Y);
            }
            MouseFlag = MyMouse.None;
            
        }
        private void MyDataInfo_MouseWheel(object sender, MouseEventArgs e) 
        {
        
        }
        private void MyDataInfo_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        public void MyShow() 
        {
            Show_Pic_Info = true;
            this.Invalidate();
        }
        public void MyShow(bool IsPicShow)
        {
            Show_Pic_Info = IsPicShow;
            this.Invalidate();
        }
        public void MyInvalidate() 
        {
            this.Invalidate();
        }
        #region 初始化
        protected override void OnSizeChanged(EventArgs e)
        {
            //left_botton = (float)((float)125 / this.Height);
            GL.glViewport(0, 0, this.Width, this.Height);

            GL.glMatrixMode(GL.GL_PROJECTION);//0x1700
            MyGL_glLoadIdentity();
           // GL.gluPerspective(90.0f, (float)this.Width / (float)this.Height, 0.1f, 1000.0f);
            GL.gluOrtho2D(0.0, this.Width, 0.0, this.Height);

            GL.glMatrixMode(GL.GL_MODELVIEW);//0x1701
            this.Invalidate();
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            //glDraw();
            //ToDo:可加入自己的设计代码
        }
        protected override void InitGLContext()
        {
            base.InitGLContext();
           //GL.glEnable(GL.GL_DEPTH_TEST);                        // 启用深度测试。根据坐标的远近自动隐藏被遮住的图形（材料）
            //GL.glEnable(GL.GL_POLYGON_SMOOTH);                      //过虑图形（多边形）的锯齿
            GL.glEnable(GL.GL_POINT_SMOOTH);                      //执行后，过虑线点的锯齿
            GL.glHint(GL.GL_POINT_SMOOTH_HINT, GL.GL_NICEST);
            //GL.glEnable(GL.GL_LINE_SMOOTH);                      //执行后，过虑线段的锯齿
             //GL.glDepthFunc(GL.GL_LEQUAL);                        // The Type Of Depth Testing To Do
             //GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);    // Really Nice Perspective Calculations

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
           
            GL.glMatrixMode(GL.GL_PROJECTION);//视角和投影角设置模式


            MyGL_glLoadIdentity();
            //设置视角fovy 视角上面到下面角度，aspect 画面宽和高之比，zNear 画面到观测点最近距离，zfar 画面到观测点最远距离
            //GL.gluPerspective(90.0, ((double)(this.Width) / (double)(this.Height)), 1.0, 1000.0);//(90.0, ((double)(this.Width) / (double)(this.Height)), 1.0, 1000.0);//gluOrtho2D( 0.0,(GLdouble)800,0.0,(GLdouble)600);  
            GL.gluOrtho2D(0.0, this.Width,0.0 , this.Height);
            GL.glMatrixMode(GL.GL_MODELVIEW);//景物设置模式
            MyGL_glLoadIdentity();
            GL.glShadeModel(GL.GL_SMOOTH);//使用颜色过渡
        }


        public override void glDraw()//官方指定重绘函数
        {


            MyGL_glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT | GL.GL_ACCUM_BUFFER_BIT);  // 清理视窗颜色缓存、深度缓存、模板缓存以及累积缓存。    
    //        MakeCurrent();//应用高速渲染
            Get_Number();
            CreatList();        
            ShowList();

         //  Blit(deviceContextHandle);//双缓存
        }
        private void ShowList() //显示列表
        {
            //GL.glCallList(10);
            //GL.glCallList(1);
            //GL.glCallList(8);
            //GL.glCallList(2);
            //GL.glCallList(3);
            //GL.glCallList(4);
            //GL.glCallList(5);
            //GL.glCallList(6);
            //GL.glCallList(7);
            //GL.glCallList(9);
            GL.glCallList(9);
            GL.glCallList(7);
            GL.glCallList(6);
            GL.glCallList(5);
            GL.glCallList(4);
            GL.glCallList(3);
            GL.glCallList(2);
            GL.glCallList(8);
            GL.glCallList(1);
            GL.glCallList(10);
        }
        private void CreatList() //通过覆盖原有显存列表的方式 创建显存列表
        {
            if (UpDataInfoFrom.UDBaseList == true)
            {
                GL.glNewList(1, GL.GL_COMPILE);
                DrawBaseList();
                GL.glEndList();
                UpDataInfoFrom.UDBaseList = false;
            }
            if (UpDataInfoFrom.UDLcrList == true)
            {
                GL.glNewList(2, GL.GL_COMPILE);
                DrawLcrList();
                GL.glEndList();
                // UpDataInfoFrom.UDLcrList = false;//数据不断跟新
            }
            if (UpDataInfoFrom.UDParList == true)
            {
                GL.glNewList(3, GL.GL_COMPILE);
                DrawParList();
                GL.glEndList();
                // UpDataInfoFrom.UDParList = false;//数据不断跟新
            }
            if (UpDataInfoFrom.UDConstList == true)
            {
                GL.glNewList(4, GL.GL_COMPILE);
                DrawConstList();
                GL.glEndList();
                //UpDataInfoFrom.UDConstList = false;//数据不断跟新
            }
            if (UpDataInfoFrom.UDLcrScrollBarsList == true)
            {
                GL.glNewList(5, GL.GL_COMPILE);
                DrawLcrScrollBarsList();
                GL.glEndList();
                UpDataInfoFrom.UDLcrScrollBarsList = false;
            }
            if (UpDataInfoFrom.UDParScrollBarsList == true)
            {
                GL.glNewList(6, GL.GL_COMPILE);
                DrawParScrollBarsList();
                GL.glEndList();
                UpDataInfoFrom.UDParScrollBarsList = false;
            }
            if (UpDataInfoFrom.UDConstScrollBarsList == true)
            {
                GL.glNewList(7, GL.GL_COMPILE);
                DrawConstScrollBarsList();
                GL.glEndList();
                UpDataInfoFrom.UDConstScrollBarsList = false;
            }
            if (UpDataInfoFrom.UDFrameList == true)
            {
                GL.glNewList(8, GL.GL_COMPILE);
                DrawFrameList();
                GL.glEndList();
                UpDataInfoFrom.UDFrameList = false;
            }
            if (UpDataInfoFrom.UDBackGroundList == true)
            {
                GL.glNewList(9, GL.GL_COMPILE);
                DrawBackGroundList();
                GL.glEndList();
                UpDataInfoFrom.UDBackGroundList = false;
            }
            if (UpDataInfoFrom.UDDrawConstChooseList == true) 
            {
                GL.glNewList(10, GL.GL_COMPILE);
                DrawConstChooseList();
                GL.glEndList();
                UpDataInfoFrom.UDDrawConstChooseList = false;
            }
        }

        #endregion
        #region 绘图

        private void DrawBaseList() 
        {
            GL.glLineWidth(2.0f);//设置坐标轴粗细
            MyGL_glBegin(GL.GL_LINES);//画不闭合折线
            GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.Color.SkyBlue));

            MyGL_glVertex2i(50, this.Height - 30);
            MyGL_glVertex2i(350, this.Height - 30);

            MyGL_glVertex2i(400, this.Height - 30);
            MyGL_glVertex2i(650, this.Height - 30);

            MyGL_glVertex2i(700, this.Height - 30);
            MyGL_glVertex2i(960, this.Height - 30);

            GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.SystemColors.Control));

            MyGL_glVertex2i(390, 0);
            MyGL_glVertex2i(390, this.Height);

            MyGL_glVertex2i(690, 0);
            MyGL_glVertex2i(690, this.Height);
            MyGL_glEnd();
            Textrue.DrawText("左末", new Point(50, this.Height - 25), 23, Color.Green, 255.0f);
            Textrue.DrawText("左线", new Point(95, this.Height - 25), 23, Color.Black, 255.0f);
            Textrue.DrawText("左始", new Point(140, this.Height - 25), 23, Color.Blue, 255.0f);
            Textrue.DrawText("中线", new Point(185, this.Height - 25), 23, Color.Red, 255.0f);
            Textrue.DrawText("右始", new Point(230, this.Height - 25), 23, Color.Blue, 255.0f);
            Textrue.DrawText("右线", new Point(275, this.Height - 25), 23, Color.Black, 255.0f);
            Textrue.DrawText("右末", new Point(320, this.Height - 25), 23, Color.Green, 255.0f);

            Textrue.DrawText("基本参数", new Point(400, this.Height - 25), 25, Color.Black, 255.0f);
            Textrue.DrawText("控制固参", new Point(700, this.Height - 25), 25, Color.Black, 255.0f);
        }
        private void DrawLcrList() 
        {
            GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.Color.White));
            if (Show_Pic_Info == true)
            {
                for (int i = 0; i < Lcr.Count; i++)
                {

                    Textrue.DrawText(Lcr[i].LEnd.ToString(), new Point(50, this.Height - 50 - i * 20 + (int)((float)LcrEye_y * 70 / 12 * (this.Height - 28) / this.Height)), 20, Color.Green, 255.0f);
                    Textrue.DrawText(Lcr[i].LBlack.ToString(), new Point(95, this.Height - 50 - i * 20 + (int)((float)LcrEye_y * 70 / 12 * (this.Height - 28) / this.Height)), 20, Color.Black, 255.0f);
                    Textrue.DrawText(Lcr[i].LStart.ToString(), new Point(140, this.Height - 50 - i * 20 + (int)((float)LcrEye_y * 70 / 12 * (this.Height - 28) / this.Height)), 20, Color.Blue, 255.0f);
                    Textrue.DrawText(Lcr[i].Center.ToString(), new Point(185, this.Height - 50 - i * 20 + (int)((float)LcrEye_y * 70 / 12 * (this.Height - 28) / this.Height)), 20, Color.Red, 255.0f);
                    Textrue.DrawText(Lcr[i].RStart.ToString(), new Point(230, this.Height - 50 - i * 20 + (int)((float)LcrEye_y * 70 / 12 * (this.Height - 28) / this.Height)), 20, Color.Blue, 255.0f);
                    Textrue.DrawText(Lcr[i].RBlack.ToString(), new Point(275, this.Height - 50 - i * 20 + (int)((float)LcrEye_y * 70 / 12 * (this.Height - 28) / this.Height)), 20, Color.Black, 255.0f);
                    Textrue.DrawText(Lcr[i].REnd.ToString(), new Point(320, this.Height - 50 - i * 20 + (int)((float)LcrEye_y * 70 / 12 * (this.Height - 28) / this.Height)), 20, Color.Green, 255.0f);
                    Textrue.DrawText((i + 1).ToString(), new Point(10, this.Height - 50 - i * 20 + (int)((float)LcrEye_y * 70 / 12 * (this.Height - 28) / this.Height)), 20, Color.Black, 255.0f);
                }
            }
        }
        private void DrawParList() 
        {
            int j = 0;
            for (int i = 0; i < DataParValue.Count; i++)
            {
                if (DataParValue[i].ShutBool == true)
                {

                    Textrue.DrawText(DataParValue[i].Name, new Point(400, this.Height - 50 - j * 20 + (int)((float)ParEye_y * Par_Num / 12 * (this.Height - 28) / this.Height)), 20, DataParValue[i].color, 255.0f);
                    Textrue.DrawText(DataParValue[i].Value.ToString(), new Point(600, this.Height - 50 - j * 20 + (int)((float)ParEye_y * Par_Num / 12 * (this.Height - 28) / this.Height)), 20, DataParValue[i].color, 255.0f);
                    j++;
                }
            }
        }
        private void DrawConstList() 
        {
           int j = 0;
            for (int i = 0; i < DataConstValue.Count; i++)
            {
                if (DataConstValue[i].ShutBool == true)
                {
                    Textrue.DrawText(DataConstValue[i].Name, new Point(700, this.Height - 50 - j * 20 + (int)((float)ConstEye_y * Const_Num / 12 * (this.Height - 28) / this.Height)), 20, DataConstValue[i].color, 255.0f);
                    if (glfouse == true && constdatatabnum != 0 && j == constdatatabnum)
                    {
                        Textrue.DrawText(SendData.ToString(), new Point(900, this.Height - 50 - j * 20 + (int)((float)ConstEye_y * Const_Num / 12 * (this.Height - 28) / this.Height)), 20, DataConstValue[i].color, 255.0f);
                    }
                    else 
                    {
                        Textrue.DrawText(DataConstValue[i].Value.ToString(), new Point(900, this.Height - 50 - j * 20 + (int)((float)ConstEye_y * Const_Num / 12 * (this.Height - 28) / this.Height)), 20, DataConstValue[i].color, 255.0f);
                    }
                    
                    j++;
                }
                if(j > ConstParRect.Count)
                {
                    ConstParRect.Add(new Rectangle(700, 30 + (j - 1) * 20 - (int)((float)ConstEye_y * Const_Num / 12 * (this.Height - 28) / this.Height), 260, 20));
                }
                
            }
        }
        private void DrawLcrScrollBarsList() 
        {
            MyGL_glBegin(GL.GL_QUADS);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.Color.FromArgb(170, 170,170), 255));
            MyGL_glVertex2i(360, this.Height - LcrEye_y);
            MyGL_glVertex2i(360, (int)((float)this.Height - this.Height * 12 / 70 - LcrEye_y));
            MyGL_glVertex2i(385, (int)((float)this.Height - this.Height * 12 / 70 - LcrEye_y));
            MyGL_glVertex2i(385, this.Height - LcrEye_y);
            MyGL_glEnd();
        }
        private void DrawParScrollBarsList() 
        {
            MyGL_glBegin(GL.GL_QUADS);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.Color.FromArgb(170, 170, 170), 255));

            MyGL_glVertex2i(660, this.Height - ParEye_y);
            if (Par_Num > 12)
            {
                MyGL_glVertex2i(660, (int)((float)this.Height - this.Height * (Par_Num > 12 ? 12 : Par_Num) / Par_Num - ParEye_y));
                MyGL_glVertex2i(685, (int)((float)this.Height - this.Height * (Par_Num > 12 ? 12 : Par_Num) / Par_Num - ParEye_y));
            }
            else
            {
                MyGL_glVertex2i(660, 0);
                MyGL_glVertex2i(685, 0);
            }
            MyGL_glVertex2i(685, this.Height - ParEye_y);
            MyGL_glEnd();
        }
        private void DrawConstScrollBarsList() 
        {
            MyGL_glBegin(GL.GL_QUADS);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.Color.FromArgb(170, 170, 170), 255));

            MyGL_glVertex2i(967, this.Height - ConstEye_y);
            if (Const_Num > 12)
            {
                MyGL_glVertex2i(967, (int)((float)this.Height - this.Height * (Const_Num > 12 ? 12 : Const_Num) / Const_Num - ConstEye_y));
                MyGL_glVertex2i(992, (int)((float)this.Height - this.Height * (Const_Num > 12 ? 12 : Const_Num) / Const_Num - ConstEye_y));
            }
            else
            {
                MyGL_glVertex2i(967, 0);
                MyGL_glVertex2i(992, 0);
            }
            MyGL_glVertex2i(992, this.Height - ConstEye_y);

            MyGL_glEnd();
        }
        private void DrawFrameList() 
        {
            MyGL_glLoadIdentity();
            MyGL_glTranslatef(0, 0, 0);
            MyGL_glBegin(GL.GL_QUADS);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.Color.White, 255));
            MyGL_glVertex2i(0, this.Height);
            MyGL_glVertex2i(0, this.Height - 30);
            MyGL_glVertex2i(355, this.Height - 30);
            MyGL_glVertex2i(355, this.Height);

            MyGL_glVertex2i(400, this.Height);
            MyGL_glVertex2i(400, this.Height - 30);
            MyGL_glVertex2i(650, this.Height - 30);
            MyGL_glVertex2i(650, this.Height);

            MyGL_glVertex2i(700, this.Height);
            MyGL_glVertex2i(700, this.Height - 30);
            MyGL_glVertex2i(960, this.Height - 30);
            MyGL_glVertex2i(960, this.Height);
            MyGL_glEnd();
        }
        private void DrawConstChooseList()
        {

            GL.glPointSize(8.0f);
            MyGL_glBegin(GL.GL_POINTS);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.Color.FromArgb(150,50, 150), 255));

            MyGL_glVertex2i(696, this.Height - 40 - constdatatabnum * 20);

            MyGL_glEnd();
    
        }
        private void DrawBackGroundList()
        {
            MyGL_glBegin(GL.GL_QUADS);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.White, 255));
            MyGL_glVertex2i(this.Width, this.Height);
            MyGL_glVertex2i(0, this.Height);
            MyGL_glVertex2i(0, 0);
            MyGL_glVertex2i(this.Width, 0);
            MyGL_glEnd();
        }
        //GLFont glFont = new GLFont();//用来显示文字

        private TextrueUnicode Textrue = new TextrueUnicode();


        private int Par_Num = 0;
        public int Const_Num = 0;
        private void Get_Number() 
        {
            int tmp_cnt=0;
            for (int i = 0; i < DataParValue.Count; i++) 
            {
                if (DataParValue[i].ShutBool == true) 
                {
                    tmp_cnt++;
                }
            }
            Par_Num = tmp_cnt;
            tmp_cnt = 0;
            for (int i = 0; i < DataConstValue.Count; i++)
            {
                if (DataConstValue[i].ShutBool == true)
                {
                    tmp_cnt++;
                }
            }
            Const_Num = tmp_cnt;
        }
        #endregion 
        #region 为了提速,我也是蛮拼的。
        private IntPtr deviceContextHandle;
        private IntPtr renderContextHandle;

        public void MakeCurrent()
        {
            if (renderContextHandle != IntPtr.Zero)
                Win32.wglMakeCurrent(deviceContextHandle, renderContextHandle);
        }
        private IntPtr windowHandle;
        public void Blit(IntPtr hdc)
        {
            if (deviceContextHandle != IntPtr.Zero || windowHandle != IntPtr.Zero)
            {
                Win32.SwapBuffers(deviceContextHandle);
            }
        }
        #endregion
        #region 我的OpenGL绘图库
        /// <summary>
        /// <para>清除颜色缓冲以及深度缓冲</para>
        /// <para>可以使用以下标志位</para>
        /// <para>可以使用 | 运算符组合不同的缓冲标志位，表明需要清除的缓冲</para>
        /// <para>GL.GL_COLOR_BUFFER_BIT:    当前可写的颜色缓冲</para>    
        /// <para>GL.GL_DEPTH_BUFFER_BIT:    深度缓冲</para>
        /// <para>GL.GL_ACCUM_BUFFER_BIT:   累积缓冲</para>
        /// <para>GL.GL_STENCIL_BUFFER_BIT: 模板缓冲</para>
        /// </summary>
        /// <param name="mask"></param>
        private void MyGL_glClear(uint mask)
        {
            GL.glClear(mask);
        }

        /// <summary>
        /// <para>用一个4×4的单位矩阵来替换当前矩阵</para>
        /// <para>即对当前矩阵进行初始化</para>
        /// <para>X坐标轴从左至右，Y坐标轴从下至上，Z坐标轴从里至外</para>
        /// <para>屏幕中心的坐标值是X和Y轴上的0.0f点</para>
        /// <para>中心左面的坐标值是负值，右面是正值。移向屏幕顶端是正值，移向屏幕底端是负值。移入屏幕深处是负值，移出屏幕则是正值</para>
        /// </summary>
        private void MyGL_glLoadIdentity()
        {
            GL.glLoadIdentity();
        }
        /// <summary>
        /// <para>平移当前矩阵</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void MyGL_glTranslatef(float x, float y, float z)
        {
            GL.glTranslatef(x, y, z);
        }
        /// <summary>
        /// <para>旋转当前矩阵</para>
        /// <para>angle  旋转角度</para>
        /// <para>以坐标原点到(x,y,z)所连成的直线为旋转轴 </para>
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void MyGL_glRotatef(float angle, float x, float y, float z)
        {
            GL.glRotatef(angle, x, y, z);
        }
        /// <summary>
        /// <para>与MyGL_glEnd一起使用</para>
        /// <para>在glBegin()和glEnd()之间可调用的函数</para>
        /// <para>glVertex*() 设置顶点坐标</para>
        /// <para>glColor*() 设置当前颜色</para>
        /// <para>glIndex*() 设置当前颜色表 </para>
        /// <para>glNormal*() 设置法向坐标 </para>
        /// <para>glEvalCoord*() 产生坐标</para>
        /// <para>glCallList(),glCallLists() 执行显示列表 </para>
        /// <para>glTexCoord*() 设置纹理坐标 </para>
        /// <para>glEdgeFlag*() 控制边界绘制</para>
        /// <para>glMaterial*() 设置材质</para>
        /// <para>几何图元类型和说明</para>
        /// <para>GL_POINTS 单个顶点集</para>
        /// <para>GL_LINES 多组双顶点线段 </para>
        /// <para>GL_POLYGON 单个简单填充凸多边形</para>
        /// <para>GL_TRAINGLES 多组独立填充三角形</para>
        /// <para>GL_QUADS 多组独立填充四边形 </para>
        /// <para>GL_LINE_STRIP 不闭合折线</para>
        /// <para>GL_LINE_LOOP 闭合折线 </para>
        /// <para>GL_TRAINGLE_STRIP 线型连续填充三角形串</para>
        /// <para>GL_TRAINGLE_FAN 扇形连续填充三角形串 </para>
        /// <para>GL_QUAD_STRIP 连续填充四边形串</para>
        /// </summary>
        /// <param name="mode"></param>
        private void MyGL_glBegin(uint mode)
        {
            GL.glBegin(mode);
        }
        /// <summary>
        /// <para>GL.glEnd()</para>
        /// </summary>
        private void MyGL_glEnd()
        {
            GL.glEnd();
        }
        /// <summary>
        /// <para>设置颜色</para>
        /// <para>范围是[0.0,1.0]</para>
        /// <para>0.0  -- >  不使用颜色成分</para>
        /// <para>1.0  -- >  使用颜色的最大值</para>
        /// <para>例：</para>
        /// <para>glColor3f(0.0, 0.0, 0.0);  --> 黑色</para>
        /// <para>glColor3f(1.0, 1.0, 1.0);  --> 白色</para>
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        private void MyGL_glColor3f(float red, float green, float blue)
        {
            GL.glColor3f(red, green, blue);
        }
        /// <summary>
        /// <para>设置顶点坐标</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void MyGL_glVertex3f(float x, float y, float z)
        {
            GL.glVertex3f(x, y, z);
        }
        private void MyGL_glVertex2i(int x, int y)
        {
            GL.glVertex2i(x, y);
        }
        /// <summary>
        /// <para>opengl显示文字</para>
        /// <para>比较难看！！！效率也不高</para>
        /// <para>text 字</para>
        /// <para>font 字体格式</para>
        /// <para>test_Color 字体颜色</para>
        /// <para>black_Color 字体背景颜色</para>
        /// <para>x 横坐标</para>
        /// <para>y 纵坐标</para>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="test_Color"></param>
        /// <param name="black_Color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void BuildFont(string text, Font font, System.Drawing.Color test_Color, System.Drawing.Color black_Color, float x, float y)
        {
            SizeF m_size;
            byte[] pixBuffer;

            if (font == null)
                font = new Font("Arial", 12);
            System.Drawing.Color m_color = test_Color;

            Graphics g_ctrl = this.CreateGraphics();
            m_size = g_ctrl.MeasureString(text, font).ToSize() + new System.Drawing.Size(1, 0);
            g_ctrl.Dispose();


            Bitmap bitmap = new Bitmap((int)m_size.Width, (int)m_size.Height);
            Graphics g_bmp = Graphics.FromImage(bitmap);
            System.Drawing.Brush brush = new SolidBrush(m_color);

            g_bmp.Clear(black_Color);
            g_bmp.DrawString(text, font, brush, new Rectangle(0, 0, (int)m_size.Width, (int)m_size.Height));
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

            pixBuffer = new byte[(int)(m_size.Width * m_size.Height * 4)];

            Array.Copy(stream.ToArray(), 54, pixBuffer, 0, pixBuffer.Length);

            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);   // 取消材质绑定

            GL.glPushMatrix();
            GL.glLoadIdentity();

            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glPushMatrix();
            GL.glLoadIdentity();

            GL.gluOrtho2D(0.0, (double)this.Width, 0.0, (double)this.Height);

            GL.glRasterPos2f(x, (float)(this.Height - y - m_size.Height));

            GL.glDrawPixels((int)m_size.Width, (int)m_size.Height, GL.GL_BGRA, GL.GL_UNSIGNED_BYTE, pixBuffer);



            GL.glPopMatrix();

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glPopMatrix();



            stream.Dispose();
            brush.Dispose();
            g_bmp.Dispose();
            bitmap.Dispose();

        }



        #endregion
    }
    public class DataInfoListupdatamsg
    {
        public bool UDBaseList;
        public bool UDLcrList;
        public bool UDParList;
        public bool UDConstList;
        public bool UDLcrScrollBarsList;
        public bool UDParScrollBarsList;
        public bool UDConstScrollBarsList;
        public bool UDFrameList;
        public bool UDBackGroundList;
        public bool UDDrawConstChooseList;
        public DataInfoListupdatamsg()
        {
            UDBaseList = true;
            UDLcrList = true;
            UDParList = true;
            UDConstList = true;
            UDLcrScrollBarsList = true;
            UDParScrollBarsList = true;
            UDConstScrollBarsList = true;
            UDFrameList = true;
            UDBackGroundList = true;
            UDDrawConstChooseList = true;
        }

    }
}



