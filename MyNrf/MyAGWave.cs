using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CsGL.OpenGL;
using System.Drawing;
using System.Drawing.Drawing2D; 
using System.Configuration;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Windows.Forms;
using MyNrf.WIN32所需要使用的API;
using MyNrf.OpenGL的一些操作;
namespace MyNrf
{

    /// <summary>
    /// opengl波形显示    基于重构官方模板
    /// </summary>
    class MyAGWave : OpenGLControl
    {
        #region 参数及子控件申请
        private System.ComponentModel.IContainer components = null;
        Timer Timer_GLupdate = new Timer();//  窗口重绘计时器
        //private bool Wave_Auto = true;
        private bool MouseDown_Flag=false;
        //private List<WaveInfoRect> datainforect = new List<WaveInfoRect>();
        /// <summary>
        /// 设置示波器流畅度,越小越流畅
        /// </summary>
        public int repaint_cnt = 0;
        private TextrueUnicode Textrue = new TextrueUnicode();
        //private int Data_Num = 0;
        //private MyMouse MouseFlag = MyMouse.None;
        public AGListupdatamsg UpDataWaveFrom = new AGListupdatamsg();
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        //private System.Drawing.Point mousePosition;
        //private System.Drawing.Point mouseLocation;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuchange1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuchangeDefult1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;

        public double MaxX=200;
        public double MaxY = 200;
        public MyMathXYData Data = new MyMathXYData();
        private System.Drawing.Point mouseLocation;
        public int datasize = 0;
        #endregion
        //创建opengl控件
        public MyAGWave()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuchange1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuchangeDefult1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuchange1,
            this.contextMenuchangeDefult1,
            this.ToolStripMenuItem1,
            this.ToolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 92);
            this.contextMenuStrip1.MouseMove += new MouseEventHandler(contextMenu_MouseMove);
            this.contextMenuStrip1.MouseLeave += new EventHandler(contextMenu_MouseLeave);
            // 
            // tcmsChage
            // 
            this.contextMenuchange1.Name = "contextMenuchange1";
            this.contextMenuchange1.Size = new System.Drawing.Size(154, 22);
            this.contextMenuchange1.Text = "放大选取框功能";
            this.contextMenuchange1.Click += new System.EventHandler(this.tcmsChage_Click);
            this.contextMenuchange1.MouseMove += new MouseEventHandler(contextMenu_MouseMove);
            this.contextMenuchange1.MouseLeave += new EventHandler(contextMenu_MouseLeave);
            // 
            // tcmsDefult
            // 
            this.contextMenuchangeDefult1.Name = "contextMenuchangeDefult1";
            this.contextMenuchangeDefult1.Size = new System.Drawing.Size(154, 22);
            this.contextMenuchangeDefult1.Text = "默认坐标范围";
            this.contextMenuchangeDefult1.Click += new System.EventHandler(this.tcmsDefult_Click);
            this.contextMenuchangeDefult1.MouseMove += new MouseEventHandler(contextMenu_MouseMove);
            this.contextMenuchangeDefult1.MouseLeave += new EventHandler(contextMenu_MouseLeave);
            // 
            // 坐标辅助显示ToolStripMenuItem
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.ToolStripMenuItem1.Text = "坐标辅助显示";
            this.ToolStripMenuItem1.Click += new System.EventHandler(this.坐标辅助显示ToolStripMenuItem_Click);
            this.ToolStripMenuItem1.MouseMove += new MouseEventHandler(contextMenu_MouseMove);
            this.ToolStripMenuItem1.MouseLeave += new EventHandler(contextMenu_MouseLeave);
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(154, 22);
            this.ToolStripMenuItem2.Text = "波形Auto";
            this.ToolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2_Click);
            this.ToolStripMenuItem2.MouseMove += new MouseEventHandler(contextMenu_MouseMove);
            this.ToolStripMenuItem2.MouseLeave += new EventHandler(contextMenu_MouseLeave);

            this.contextMenuStrip1.ResumeLayout(false);

            this.SetStyle(ControlStyles.Selectable, true);
            this.Timer_GLupdate.Tick += new EventHandler(Timer_GLupdate_Tick);
            this.Timer_GLupdate.Interval = 10;   
            this.KeyDown += new KeyEventHandler(OpenGLBase_KeyDown);
            this.MouseDown += new MouseEventHandler(OpenGLBase_MouseDown);
            this.MouseMove += new MouseEventHandler(OpenGLBase_MouseMove);
            this.MouseUp += new MouseEventHandler(OpenGLBase_MouseUp);
            this.MouseWheel += new MouseEventHandler(OpenGLBase_MouseWheel);
            this.ResizeRedraw = true;
            this.Resize += new EventHandler(OpenGLBase_Resize);


            Textrue.JianJu = 0.1;//设置字体间距jianju.....
      
        }
        //鼠标进入控件区域触发事件
        private void contextMenu_MouseMove(object sender, MouseEventArgs e)
         {
            //this.Cursor=Form1.MyCursor.Choose;
         }
        //鼠标离开控件区域触发事件
        private void contextMenu_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Default;
        }
        //启动定时器
        public void Timer_Start() 
        {
            this.Timer_GLupdate.Start();
        }
        //定时器触发的方法
        private void Timer_GLupdate_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        //重绘触发的方法1  一直触发
        public void MyWaveShow() 
        {
            UpDataWaveFrom.UPDrawDataPointList = true;
            this.Invalidate();
        }
        //重绘触发的方法2  分时复用
        private void MyWaveUpdata()
        {

                repaint_cnt++;
                if (repaint_cnt == 6) 
                {
                    repaint_cnt = 0;
                    this.Invalidate();
                }


            
        }
        #region 一些动作
        //鼠标按下触发事件
        private void OpenGLBase_MouseDown(object sender, MouseEventArgs e) 
        {
   

            
        }

        //鼠标移动触发事件
        private void OpenGLBase_MouseMove(object sender, MouseEventArgs e) 
        {
            mouseLocation.X = e.X;
            mouseLocation.Y = e.Y;
            UpDataWaveFrom.UPDrawMousePointList = true;
            MyWaveUpdata();
        }
        //鼠标按键弹起触发事件
        private void OpenGLBase_MouseUp(object sender, MouseEventArgs e) 
        {
  
        }

        private void OpenGLBase_MouseWheel(object sender, MouseEventArgs e)
        {
 
        }
        //键盘按键按下触发事件
        private void OpenGLBase_KeyDown(object sender, KeyEventArgs e)
        {
 
        }
        //控件大小改变时触发事件，使控件及控件内部元素实时适应新的大小
        private void OpenGLBase_Resize(object sender, EventArgs e) 
        {

            this.Invalidate();

            GC.Collect();
        }
        //鼠标右键菜单“坐标辅助显示”按下触发事件
        private void 坐标辅助显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //鼠标右键菜单“默认坐标范围”按下触发事件
        private void tcmsDefult_Click(object sender, EventArgs e)
        {


        }
        //鼠标右键菜单“放大选取功能”按下触发事件
        private void tcmsChage_Click(object sender, EventArgs e)
        {

            this.contextMenuchange1.Checked = !this.contextMenuchange1.Checked;
        }

        //鼠标右键菜单“波形Auto”按下触发事件
        private void ToolStripMenuItem2_Click(object sender, EventArgs e) 
        {
            //Wave_Auto=true;

                contextMenuchange1.Checked = false;

        }
        #endregion
        #region 初始化
        /// <summary>
        /// 重写窗体大小 改变方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {


            GL.glViewport(0, 0, this.Width, this.Height);

            GL.glMatrixMode(GL.GL_PROJECTION);//0x1700
            MyGL_glLoadIdentity();
            GL.gluOrtho2D(0.0, this.Width, 0.0, this.Height);
         //   GL.gluPerspective(90.0f, (float)this.Width / (float)this.Height, 0.1f, 1000.0f);
            GL.glMatrixMode(GL.GL_MODELVIEW);//0x1701

            this.Invalidate();
        }
        /// <summary>
        /// OnPaint方法处理Paint事件
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent)
        {
            
            base.OnPaint(pevent);
            //glDraw();

            //ToDo:可加入自己的设计代码
        }


        /// <summary>
        /// OpenGL初始化场景
        /// </summary>
        protected override void InitGLContext()
        {
            base.InitGLContext();
            //GL.glEnable(GL.GL_DEPTH_TEST);                        // 启用深度测试。根据坐标的远近自动隐藏被遮住的图形（材料）

            //GL.glEnable(GL.GL_POLYGON_SMOOTH);                      //过虑图形（多边形）的锯齿
            //GL.glHint(GL.GL_POLYGON_SMOOTH_HINT, GL.GL_NICEST);

            GL.glEnable(GL.GL_POINT_SMOOTH);                      //执行后，过虑线点的锯齿
            GL.glHint(GL.GL_POINT_SMOOTH_HINT, GL.GL_NICEST);

            GL.glEnable(GL.GL_LINE_SMOOTH);                      //执行后，过虑线段的锯齿
            GL.glHint(GL.GL_LINE_SMOOTH_HINT, GL.GL_NICEST);
            GL.glEnable(GL.GL_BLEND);   //启用混合模式
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            //GL.glDepthFunc(GL.GL_LESS);                        // 深度测试

            //GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);    // 高质量显示抗锯齿
            GL.glMatrixMode(GL.GL_PROJECTION);//视角和投影角设置模式
            MyGL_glLoadIdentity();
            //设置视角fovy 视角上面到下面角度，aspect 画面宽和高之比，zNear 画面到观测点最近距离，zfar 画面到观测点最远距离
         //   GL.gluPerspective(90.0, ((double)(this.Width) / (double)(this.Height)), 1.0, 1000.0);//(90.0, ((double)(this.Width) / (double)(this.Height)), 1.0, 1000.0);//gluOrtho2D( 0.0,(GLdouble)800,0.0,(GLdouble)600);  
            GL.gluOrtho2D(0.0, this.Width, 0.0, this.Height);

            GL.glMatrixMode(GL.GL_MODELVIEW);//景物设置模式
            MyGL_glLoadIdentity();
            GL.glShadeModel(GL.GL_SMOOTH);//使用颜色过渡

        }



      //  GLFont glFont = new GLFont();//用来显示文字


        public override void glDraw()//官方指定重绘函数
        {

            MyGL_glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT |GL.GL_STENCIL_BUFFER_BIT | GL.GL_ACCUM_BUFFER_BIT);  // 清理视窗颜色缓存、深度缓存、模板缓存以及累积缓存。   
            MyGL_glLoadIdentity();
            CreatList();
            ShowList();



            

            #region 一些3D形状
            //MyGL_glLoadIdentity();
            //MyGL_glTranslatef(-1.5f + eye_x, 0.0f + eye_y, -6.0f + eye_z);                                            // Move Left 1.5 Units And Into The Screen 

            //MyGL_glRotatef(rtri, 0.0f, 1.0f, 0.0f);                                            // 绕Y轴旋转金字塔  rtri是角度
            //MyGL_glBegin(GL.GL_TRIANGLES);                                                        // Drawing Using Triangles
            ////正对的面
            //MyGL_glColor3f(1.0f, 0.0f, 0.0f);                                            // Red
            //MyGL_glVertex3f(0.0f, 1.0f, 0.0f);                                            // Top Of Triangle (Front)
            //MyGL_glColor3f(0.0f, 1.0f, 0.0f);                                            // Green
            //MyGL_glVertex3f(-1.0f, -1.0f, 1.0f);                                            // Left Of Triangle (Front)
            //MyGL_glColor3f(0.0f, 0.0f, 1.0f);                                            // Blue
            //MyGL_glVertex3f(1.0f, -1.0f, 1.0f);                                            // Right Of Triangle (Front)
            ////2
            //MyGL_glColor3f(1.0f, 0.0f, 0.0f);                                        // Red
            //MyGL_glVertex3f(0.0f, 1.0f, 0.0f);                                        // Top Of Triangle (Right)
            //MyGL_glColor3f(0.0f, 0.0f, 1.0f);                                        // Blue
            //MyGL_glVertex3f(1.0f, -1.0f, 1.0f);                                        // Left Of Triangle (Right)
            //MyGL_glColor3f(0.0f, 1.0f, 0.0f);                                        // Green
            //MyGL_glVertex3f(1.0f, -1.0f, -1.0f);                                        // Right Of Triangle (Right)
            ////3
            //MyGL_glColor3f(1.0f, 0.0f, 0.0f);                                        // Red
            //MyGL_glVertex3f(0.0f, 1.0f, 0.0f);                                        // Top Of Triangle (Back)
            //MyGL_glColor3f(0.0f, 1.0f, 0.0f);                                        // Green
            //MyGL_glVertex3f(1.0f, -1.0f, -1.0f);                                        // Left Of Triangle (Back)
            //MyGL_glColor3f(0.0f, 0.0f, 1.0f);                                        // Blue
            //MyGL_glVertex3f(-1.0f, -1.0f, -1.0f);                                        // Right Of Triangle (Back)
            ////4
            //MyGL_glColor3f(1.0f, 0.0f, 0.0f);                                        // Red
            //MyGL_glVertex3f(0.0f, 1.0f, 0.0f);                                        // Top Of Triangle (Left)
            //MyGL_glColor3f(0.0f, 0.0f, 1.0f);                                        // Blue
            //MyGL_glVertex3f(-1.0f, -1.0f, -1.0f);                                        // Left Of Triangle (Left)
            //MyGL_glColor3f(0.0f, 1.0f, 0.0f);                                        // Green
            //MyGL_glVertex3f(-1.0f, -1.0f, 1.0f);                                        // Right Of Triangle (Left)
            //MyGL_glEnd();                                                                // Finished Drawing The Pyramid

            //MyGL_glLoadIdentity();                                                            // Reset The Current Modelview Matrix,回到中心位置
            //MyGL_glTranslatef(1.5f + eye_x, 0.0f + eye_y, -7.0f + eye_z);                                            // Move Right 1.5 Units And Into The Screen 7.0

            //MyGL_glRotatef(rquad, 1.0f, 1.0f, 1.0f);                                            // Rotate The Quad On The X, Y, And Z Axes
            //MyGL_glBegin(GL.GL_QUADS);                                                            // Draw A Quad
            ////1
            //MyGL_glColor3f(0.0f, 1.0f, 0.0f);                                            // Set The Color To Green
            //MyGL_glVertex3f(1.0f, 1.0f, -1.0f);                                            // Top Right Of The Quad (Top)
            //MyGL_glVertex3f(-1.0f, 1.0f, -1.0f);                                            // Top Left Of The Quad (Top)
            //MyGL_glVertex3f(-1.0f, 1.0f, 1.0f);                                            // Bottom Left Of The Quad (Top)
            //MyGL_glVertex3f(1.0f, 1.0f, 1.0f);                                            // Bottom Right Of The Quad (Top)
            ////2
            //MyGL_glColor3f(1.0f, 0.5f, 0.0f);                                        // Set The Color To Orange
            //MyGL_glVertex3f(1.0f, -1.0f, 1.0f);                                        // Top Right Of The Quad (Bottom)
            //MyGL_glVertex3f(-1.0f, -1.0f, 1.0f);                                        // Top Left Of The Quad (Bottom)
            //MyGL_glVertex3f(-1.0f, -1.0f, -1.0f);                                        // Bottom Left Of The Quad (Bottom)
            //MyGL_glVertex3f(1.0f, -1.0f, -1.0f);                                        // Bottom Right Of The Quad (Bottom)
            ////3
            //MyGL_glColor3f(1.0f, 0.0f, 0.0f);                                            // Set The Color To Red
            //MyGL_glVertex3f(1.0f, 1.0f, 1.0f);                                            // Top Right Of The Quad (Front)
            //MyGL_glVertex3f(-1.0f, 1.0f, 1.0f);                                            // Top Left Of The Quad (Front)
            //MyGL_glVertex3f(-1.0f, -1.0f, 1.0f);                                            // Bottom Left Of The Quad (Front)
            //MyGL_glVertex3f(1.0f, -1.0f, 1.0f);                                            // Bottom Right Of The Quad (Front)
            ////4
            //MyGL_glColor3f(1.0f, 1.0f, 0.0f);                                        // Set The Color To Yellow
            //MyGL_glVertex3f(1.0f, -1.0f, -1.0f);                                        // Bottom Left Of The Quad (Back)
            //MyGL_glVertex3f(-1.0f, -1.0f, -1.0f);                                        // Bottom Right Of The Quad (Back)
            //MyGL_glVertex3f(-1.0f, 1.0f, -1.0f);                                        // Top Right Of The Quad (Back)
            //MyGL_glVertex3f(1.0f, 1.0f, -1.0f);                                        // Top Left Of The Quad (Back)
            ////5
            //MyGL_glColor3f(0.0f, 0.0f, 1.0f);                                        // Set The Color To Blue
            //MyGL_glVertex3f(-1.0f, 1.0f, 1.0f);                                        // Top Right Of The Quad (Left)
            //MyGL_glVertex3f(-1.0f, 1.0f, -1.0f);                                        // Top Left Of The Quad (Left)
            //MyGL_glVertex3f(-1.0f, -1.0f, -1.0f);                                        // Bottom Left Of The Quad (Left)
            //MyGL_glVertex3f(-1.0f, -1.0f, 1.0f);                                        // Bottom Right Of The Quad (Left)
            ////6
            //MyGL_glColor3f(1.0f, 0.0f, 1.0f);                                                    // Set The Color To Violet
            //MyGL_glVertex3f(1.0f, 1.0f, -1.0f);                                            // Top Right Of The Quad (Right)
            //MyGL_glVertex3f(1.0f, 1.0f, 1.0f);                                            // Top Left Of The Quad (Right)
            //MyGL_glVertex3f(1.0f, -1.0f, 1.0f);                                            // Bottom Left Of The Quad (Right)
            //MyGL_glVertex3f(1.0f, -1.0f, -1.0f);                                            // Bottom Right Of The Quad (Right)
            //MyGL_glEnd();              // Done Drawing The Cube

            //MyGL_glLoadIdentity();
            //MyGL_glTranslatef(0.0f + eye_x, 1.0f + eye_y, -1.0f + eye_z);
            //GL.glLineWidth(5.0f);
            //MyGL_glBegin(GL.GL_LINE_STRIP);
            //MyGL_glColor3f(0.0f, 0.0f, 1.0f);
            //MyGL_glVertex3f(0.0f, 0.0f, 0.0f);
            //MyGL_glVertex3f(-1.0f, -1.0f, 0.0f);
            //MyGL_glColor3f(1.0f, 0.0f, 0.0f);
            //MyGL_glVertex3f(1.0f, -1.0f, 0.0f);
            //MyGL_glVertex3f(0.0f, -0.0f, 0.0f);
            //MyGL_glEnd();

            //rtri += 0.6f;                     // Increase The Rotation Variable For The Triangle
            //rquad += 0.6f;            // Decrease The Rotation Variable For The Quad,单位是角度

            #endregion

        }

        private void CreatList() //通过覆盖原有显存列表的方式 创建显存列表
        {
            if (UpDataWaveFrom.UPDrawXYlineList == true) 
            {
                GL.glNewList(1, GL.GL_COMPILE);
                DrawXYline();
                GL.glEndList();
                UpDataWaveFrom.UPDrawXYlineList = false;
            }
            if (UpDataWaveFrom.UPDrawDataPointList == true)
            {
                GL.glNewList(2, GL.GL_COMPILE);
                DrawDataPoint();
                GL.glEndList();
                UpDataWaveFrom.UPDrawDataPointList = false;
            }
            if (UpDataWaveFrom.UPDrawMousePointList == true)
            {
                GL.glNewList(3, GL.GL_COMPILE);
                DrawMousePointList();
                GL.glEndList();
                UpDataWaveFrom.UPDrawMousePointList = false;
            }
            
        }
        private void ShowList() 
        {

            GL.glCallList(3);
            GL.glCallList(2);
            GL.glCallList(1);
            //GL.glCallList(10);
            //GL.glCallList(3);
            //GL.glCallList(4);
            //GL.glCallList(5);
            //GL.glCallList(9);
            //GL.glCallList(7);
            //GL.glCallList(6);
            //GL.glCallList(8);
        }

        #endregion


        #region 画波形
        //四个临时变量记录矩形选取框位置

        private void DrawXYline() 
        {
            MyGL_glBegin(GL.GL_LINES);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.White, 255));
            MyGL_glVertex2i(0, this.Height/2);
            MyGL_glVertex2i(this.Width, this.Height/2);
            MyGL_glVertex2i(this.Width / 2, 0);
            MyGL_glVertex2i(this.Width / 2, this.Height);
            MyGL_glEnd();
        }
        private void DrawDataPoint() 
        {
            GL.glPointSize(7.0f);
            MyGL_glBegin(GL.GL_POINTS);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(System.Drawing.Color.FromArgb(150, 50, 150), 255));
            if (Data.DataPoint.Count > datasize) 
            {
                Data.DataPoint.RemoveRange(0, Data.DataPoint.Count - datasize); 
            }
            if (Data.DataPoint.Count != 0)
            {
                for (int i = 0; i < Data.DataPoint.Count; i++)
                {
                    MyGL_glVertex2i((int)(Data.DataPoint[i].X_Value * this.Width / MaxX / 2 + this.Width / 2), (int)(this.Height / 2 - Data.DataPoint[i].Y_Value * this.Height / 2 / MaxY));
                }
            }

            MyGL_glEnd();
        }
        private void DrawMousePointList()
        {

                GL.glLineWidth(1.0f);//设置线粗细
                MyGL_glBegin(GL.GL_LINES);
                GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.Blue));


                MyGL_glVertex2i(mouseLocation.X, this.Height);
                MyGL_glVertex2i(mouseLocation.X, 0);
                MyGL_glVertex2i(0, this.Height - mouseLocation.Y);
                MyGL_glVertex2i(this.Width, this.Height - mouseLocation.Y);
                MyGL_glEnd();

                float tmpx = (float)(MaxX + (mouseLocation.X) * (MaxX * 2) / (this.Width));
                float tmpy = (float)(MaxY - (mouseLocation.Y ) * (MaxY) / (this.Height));
                     
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
        private void MyGL_glTranslatef(float x,float y,float z) 
        {
            GL.glTranslatef(x,y,z);
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
        private void MyGL_glRotatef(float angle,float x, float y, float z)
        {
            GL.glRotatef(angle,x,y,z);
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
            GL.glColor3f(red,green,blue);
        }
        /// <summary>
        /// <para>设置顶点坐标</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void MyGL_glVertex3f(float x, float y, float z) 
        {
            GL.glVertex3f(x,y,z);
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
        private void BuildFont(string text, Font font,Color test_Color,Color black_Color,float x,float y)
        {
            SizeF m_size;
            byte[] pixBuffer;

            if (font == null)
                font = new Font("Arial", 12);
            Color m_color = test_Color;

            Graphics g_ctrl = this.CreateGraphics();
            m_size = g_ctrl.MeasureString(text, font).ToSize() + new Size(1, 0);
            g_ctrl.Dispose();


            Bitmap bitmap = new Bitmap((int)m_size.Width, (int)m_size.Height);
            Graphics g_bmp = Graphics.FromImage(bitmap);
            Brush brush = new SolidBrush(m_color);

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
    public class AGListupdatamsg 
    {
        public bool UPDrawXYlineList;
        public bool UPDrawDataPointList;
        public bool UPDrawMousePointList;
        public AGListupdatamsg()
        {
            UPDrawXYlineList = true;
            UPDrawDataPointList = true;
            UPDrawMousePointList = true;
        }

    }

    
}

