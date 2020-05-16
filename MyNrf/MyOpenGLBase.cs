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
    class MyOpenGLBase : OpenGLControl
    {
        #region 参数及子控件申请
        private System.ComponentModel.IContainer components = null;
        Timer Timer_GLupdate = new Timer();//  窗口重绘计时器
        //private float rtri = 0;                                                    // Angle For The Triangle
        //private float rquad = 0;
        private int eye_x = 0;
        private int eye_y = 0;
        private int eye_z = 0;
        private int tmp_eye_x = 0;
        private int tmp_eye_y = 0;
        private int tmp_eye_z = 0;
        private float Sub_X = 500;
        private float Sub_Y = 60000;
        public int Data_Length=1000;
        private bool Wave_Auto = true;
        private bool MouseDown_Flag=false;
        private List<WaveInfoRect> datainforect = new List<WaveInfoRect>();
        /// <summary>
        /// 设置示波器流畅度,越小越流畅
        /// </summary>
        public int repaint_cnt = 0;
        private float Max_x = 500;
        public float Max_X 
        {
            get { return Max_x; }
            set { Max_x = value; }
        }
        private float Min_x = 0;
        public float Min_X
        {
            get { return Min_x; }
            set { Min_x = value; }
        }
        private float Max_y = 30000;
        public float Max_Y
        {
            get { return Max_y; }
            set { Max_y = value; }
        }
        private float Min_y=-30000;
        public float Min_Y
        {
            get { return Min_y; }
            set { Min_y = value; }
        }
        private Color test_color;
        public Color Test_Color 
        {
            get { return test_color; }
            set { test_color = value; }
        }
        private Color black_color;
        public Color Black_Color
        {
            get { return black_color; }
            set { black_color = value; }
        }
        private Color frame_color;
        public Color Frame_Color
        {
            get { return frame_color; }
            set { frame_color = value; }
        }
        private Color line_color;
        public Color Line_Color
        {
            get { return line_color; }
            set { line_color = value; }
        }
        private int Data_Num = 0;
        private MyMouse MouseFlag = MyMouse.None;
        public WaveListupdatamsg UpDataWaveFrom = new WaveListupdatamsg();

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        public MyDendencyControl Xlable = new MyDendencyControl("t/s");
        public MyDendencyControl Ylable = new MyDendencyControl("a/db");
        public MyDendencyControl WaleMode = new MyDendencyControl(true);

        private System.Drawing.Point mousePosition;
        private System.Drawing.Point mouseLocation;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuchange1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuchangeDefult1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;

        #endregion
        //创建opengl控件
        public MyOpenGLBase()
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

            this.Xlable.DependencyPropertyChanged += new EventHandler(Base_DependencyPropertyChanged);
            this.Ylable.DependencyPropertyChanged += new EventHandler(Base_DependencyPropertyChanged);
            
            base.BackColor = Color.White;

            left_botton = 100;
            right_botton = 80;
           
            Sub_X = Max_x - Min_x;
            Sub_Y = Max_y - Min_y;
            Textrue.JianJu = 0.1;//设置字体间距jianju.....
      
        }
        private void Base_DependencyPropertyChanged(object sender, EventArgs e) 
        {
            UpDataWaveFrom.UDrawXYlabelList = true;
            MyWaveShow();
        }
        //鼠标进入控件区域触发事件
        private void contextMenu_MouseMove(object sender, MouseEventArgs e)
         {
            this.Cursor=Form1.MyCursor.Choose;
         }
        //鼠标离开控件区域触发事件
        private void contextMenu_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Default;
        }
        //启动定时器
        public void Timer_Start() 
        {
            //this.Timer_GLupdate.Start();
        }
        //定时器触发的方法
        private void Timer_GLupdate_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        //重绘触发的方法1  一直触发
        public void MyWaveShow() 
        {
            UpDataWaveFrom.UDDataList = true;
            this.Invalidate();
        }
        //重绘触发的方法2  分时复用
        private void MyWaveUpdata()
        {
            if (MouseDown_Flag == true || ToolStripMenuItem1.Checked == true)
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
                repaint_cnt = 0;
                this.Invalidate();
            }
            
        }
        #region 一些动作
        //鼠标按下触发事件
        private void OpenGLBase_MouseDown(object sender, MouseEventArgs e) 
        {
            this.Focus();
            MouseDown_Flag = true;
            Rectangle rect1 = new Rectangle(this.Width - right_botton, 0, right_botton, this.Height);
            Rectangle rect3 = new Rectangle(left_botton, 5, this.Width - left_botton - right_botton, this.Height - 40);
            if (e.Button == MouseButtons.Right)
            {

                 if (rect3.Contains(e.Location)) //鼠标滚轮及缩放
                {
                    //this.Cursor = Form1.MyCursor.Hand;
                    this.mousePosition.X = e.X;
                    this.mousePosition.Y = e.Y;

                    change_subx = (float)((float)Min_x + (((float)mousePosition.X - this.Height * left_botton / 2) * (float)Sub_X / ((float)this.Width - this.Height * (left_botton + 0.35) / 2)));
                    change_suby = (float)((float)Max_y - (((float)mousePosition.Y + ((float)this.Height * 0.925 - (float)mousePosition.Y) * 0.015 - (float)this.Height * 0.025) * (float)Sub_Y / ((float)this.Height - this.Height * 0.15 / 2)));//坐标误差加上补偿

                    MouseFlag = MyMouse.RightFlag2;
                }
                
            }
            if (e.Button == MouseButtons.Left) 
            {
                if (rect1.Contains(e.Location))//参数
                {
                    //this.Cursor = Form1.MyCursor.Hand;
                    this.mousePosition.X = e.X;// Control.MousePosition.X - this.Location.X;
                    this.mousePosition.Y = e.Y;// Control.MousePosition.Y - this.Location.Y;

                    tmp_eye_x = eye_x;
                    tmp_eye_y = eye_y;
                    tmp_eye_z = eye_z;
                    MouseFlag = MyMouse.LeftFlag1;
                }

                if (rect3.Contains(e.Location)) //波形位置
                {
                    //this.Cursor = Form1.MyCursor.Move;
                    this.mousePosition.X = e.X;// Control.MousePosition.X - this.Location.X;
                    this.mousePosition.Y = e.Y;// Control.MousePosition.Y - this.Location.Y;
                    change_x = Max_x;
                    change_y = Max_y;
                    MouseFlag = MyMouse.LeftFlag2;
                }

                for (int i = 0; i < datainforect.Count; i++) 
                {
                    if (datainforect[i].rect1.Contains(e.Location))
                    {
                        datainforect[i].tmpwaveon = !datainforect[i].tmpwaveon;                    
                        UpDataWaveFrom.UDInfoRectList = true;
                        break;
                    }
                }
            }

            
        }
        //两临时变量记录鼠标坐标改变量
        private float change_x, change_y;
        private int WireData = 0;
        //鼠标移动触发事件
        private void OpenGLBase_MouseMove(object sender, MouseEventArgs e) 
        {
            mouseLocation.X = e.X;
            mouseLocation.Y = e.Y;



            if (MouseFlag == MyMouse.RightFlag2 || MouseFlag == MyMouse.RightFlag3)
            {
                if (contextMenuchange1.Checked == true) 
                {

                    MyWaveUpdata();
                }
                UpDataWaveFrom.UDRectList = true;
                MouseFlag = MyMouse.RightFlag3;
            }
            if (MouseFlag == MyMouse.LeftFlag1) 
            {
                if (Data_Num * 38 < this.Height)
                {
                    eye_y = 0;
                    return; 
                }
                else
                {
                    eye_y = e.Y - mousePosition.Y + tmp_eye_y;
                    if (eye_y < this.Height - 38 * Data_Num) eye_y = this.Height - 38 * Data_Num;
                    if (eye_y > 0) eye_y = 0;
                    UpDataWaveFrom.UDInfoRectList = true;
                    MyWaveUpdata();
                    return;
                }
            }
            if(MouseFlag == MyMouse.LeftFlag2)
            {
                Wave_Auto=false;
                float tmpx = (float)(Max_x - Min_x) / (this.Width - left_botton - right_botton);
                float tmpy = (float)(Max_y - Min_y) / (this.Height -40);
                Max_x = -(float)((float)((float)e.X - mousePosition.X) * tmpx) + change_x;
                Max_y = (float)((float)((float)e.Y - mousePosition.Y) * tmpy) + change_y;
                if (Max_x > Data_Length) Max_x = Data_Length;
                if (Max_X - Sub_X < 0) Max_x = 0+Sub_X;
                if (Max_y > 60000) Max_y = 60000;
                if (Max_y - Sub_Y < -60000) Max_y = -60000 + Sub_Y;
                Min_x = Max_x - Sub_X;
                Min_y = Max_y - Sub_Y;
                UpDataWaveFrom.UDCoordinateList = true;
                UpDataWaveFrom.UDDataList = true;
                MyWaveUpdata();
            }
            if (ToolStripMenuItem1.Checked == true)
            {
                UpDataWaveFrom.UDMousePointList = true;
               // this.Invalidate();
                MyWaveUpdata();
                return;
            }
            WireData = 0;
            for (int i = 0; i < datainforect.Count; i++)
            {
                if (datainforect[i].rect2.Contains(e.Location))
                {
                    WireData = i + 1;
                    UpDataWaveFrom.UDInfoRectList = true;
                    break;
                }
            }
        }
        //鼠标按键弹起触发事件
        private void OpenGLBase_MouseUp(object sender, MouseEventArgs e) 
        {
            if (MouseFlag == MyMouse.RightFlag2) 
            {
                contextMenuStrip1.Show(Control.MousePosition.X, Control.MousePosition.Y);
            }
            if (MouseFlag == MyMouse.RightFlag3)
            {
                UpDataWaveFrom.UDCoordinateList = true;
            }
            //this.Cursor = Form1.MyCursor.Default;
            MouseFlag = MyMouse.None;
            if (Menuchangeflag == true) 
            {
                Menuchangeflag = false;
                if (Menuchangex1 - Menuchangex2 > 10 || Menuchangex2 - Menuchangex1 > 10)
                {
                    Max_x = (Menuchangex1 > Menuchangex2) ? Menuchangex1 : Menuchangex2;
                    Min_x = (Menuchangex1 < Menuchangex2) ? Menuchangex1 : Menuchangex2;
                }
                if (Menuchangey1 - Menuchangey2 > 10 || Menuchangey2 - Menuchangey1 > 10)
                {
                    Max_y = (Menuchangey1 > Menuchangey2) ? Menuchangey1 : Menuchangey2;
                    Min_y = (Menuchangey1 < Menuchangey2) ? Menuchangey1 : Menuchangey2;
                }
                Sub_X = Max_x - Min_x;
                Sub_Y = Max_y - Min_y;
            }
            MouseDown_Flag = false;
            UpDataWaveFrom.UDRectList = true;
            MyWaveShow();
        }
        //两临时变量记录鼠标步进改变量
        private float change_subx, change_suby;
        //鼠标滚轮滚动触发事件
        private void OpenGLBase_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((MouseFlag == MyMouse.RightFlag2 || MouseFlag == MyMouse.RightFlag3) && contextMenuchange1.Checked == false)
            {
                MouseFlag = MyMouse.RightFlag3;
                if (e.Delta  > 0)
                {
                    float tmps = Max_x - Min_x;
                    Sub_X = (float)((float)Sub_X * 1.01 + 1);
                    if (Sub_X < 10) Sub_X = 10;
                    if (Sub_X > Data_Length) Sub_X = Data_Length;
                    Max_x = (float)(change_subx + Sub_X * ((float)Max_x - change_subx) / tmps);
                    if (Max_x > Data_Length) Max_x = Data_Length;
                    Min_x = (float)(change_subx - Sub_X * ((float)change_subx - Min_x) / tmps);
                    if (Min_x < 0) Min_x = 0;
                    Sub_X = Max_x - Min_x;

                    tmps = Max_y - Min_y;
                    Sub_Y = (float)((float)Sub_Y * 1.01 + 1);
                    if (Sub_Y < 10) Sub_Y = 10;
                    if (Sub_Y > 120000) Sub_Y = 120000;
                    Max_y = (float)(change_suby + Sub_Y * ((float)Max_y - change_suby) / tmps);
                    if (Max_x > 60000) Max_x = 60000;
                    Min_y = (float)(change_suby - Sub_Y * ((float)change_suby - Min_y) / tmps);
                    if (Min_y < -60000) Min_y = -60000;
                    Sub_Y = Max_y - Min_y;
                    UpDataWaveFrom.UDCoordinateList = true;
                }
                else if (e.Delta  < 0)
                {

                    float tmps = Max_x - Min_x;
                    Sub_X = (float)((float)Sub_X * 0.99 - 1);
                    if (Sub_X < 10) Sub_X = 10;
                    if (Sub_X > 1000) Sub_X = 1000;
                    Max_x = (float)(change_subx + Sub_X * ((float)Max_x - change_subx) / tmps);
                    if (Max_x > 1000) Max_x = 1000;
                    Min_x = (float)(change_subx - Sub_X * ((float)change_subx - Min_x) / tmps);
                    if (Min_x < 0) Min_x = 0;
                    Sub_X = Max_x - Min_x;

                    tmps = Max_y - Min_y;
                    Sub_Y = (float)((float)Sub_Y * 0.99 - 1);
                    if (Sub_Y < 10) Sub_Y = 10;
                    if (Sub_Y > 120000) Sub_Y = 120000;
                    Max_y = (float)(change_suby + Sub_Y * ((float)Max_y - change_suby) / tmps);
                    if (Max_x > 60000) Max_x = 60000;
                    Min_y = (float)(change_suby - Sub_Y * ((float)change_suby - Min_y) / tmps);
                    if (Min_y < -60000) Min_y = -60000;
                    Sub_Y = Max_y - Min_y;
                    UpDataWaveFrom.UDCoordinateList = true;
                }


                MyWaveShow();
            }

        }
        //键盘按键按下触发事件
        private void OpenGLBase_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.D:
            //        eye_x+=2; break;
            //    case Keys.A:
            //        eye_x-=2;break;
            //    case Keys.W:
            //        eye_y+=2;if (eye_y > (float)(((float)Data_Num - 13) * 0.15)) eye_y = (int)(((float)Data_Num - 13) * 0.15);break;
            //    case Keys.S:
            //        eye_y-=2; if(eye_y<0)eye_y=0;break;
            //    default:
            //        break;
            //}
            //this.Invalidate();
        }
        //控件大小改变时触发事件，使控件及控件内部元素实时适应新的大小
        private void OpenGLBase_Resize(object sender, EventArgs e) 
        {

            MyWaveShow();

            GC.Collect();
        }
        //鼠标右键菜单“坐标辅助显示”按下触发事件
        private void 坐标辅助显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyWaveUpdata();
            ToolStripMenuItem1.Checked = !ToolStripMenuItem1.Checked;
            UpDataWaveFrom.UDMousePointList = true;
            MyWaveShow();
        }
        //鼠标右键菜单“默认坐标范围”按下触发事件
        private void tcmsDefult_Click(object sender, EventArgs e)
        {
            Max_y = 30000;
            Min_y = -30000;
            Max_x = Data_Length;
            Min_x = 0;
            Sub_X = Max_x - Min_x;
            Sub_Y = Max_y - Min_y;
            Wave_Auto = true;
            contextMenuchange1.Checked = false;
            UpDataWaveFrom.UDCoordinateList = true;
            MyWaveShow();

        }
        //鼠标右键菜单“放大选取功能”按下触发事件
        private void tcmsChage_Click(object sender, EventArgs e)
        {
            Wave_Auto = false;
            this.contextMenuchange1.Checked = !this.contextMenuchange1.Checked;
        }
        //两临时变量记录纵坐标范围
        private float y_max=21428,y_min=-21428;
        //鼠标右键菜单“波形Auto”按下触发事件
        private void ToolStripMenuItem2_Click(object sender, EventArgs e) 
        {
            bool flag = true;
            Wave_Auto=true;
            UpDataWaveFrom.UDCoordinateList = true;
            if (!(Item.Count > 0))
            {
                Max_y = 30000;
                Min_y = -30000;
                Max_x = 500;
                Min_x = 0;
                Sub_X = Max_x - Min_x;
                Sub_Y = Max_y - Min_y;
                contextMenuchange1.Checked = false;
                MyWaveShow();
                return;
            }
            else 
            {
                for (int i = 0; i < Item.Count && i < datainforect.Count; i++) 
                {
                    if (Item[i].waveon == true && datainforect[i].tmpwaveon==true)
                    {
                        for (int j = 0; j < Item[i].ListData.Count; j++)
                        {

                            if (j == 0 && flag==true)
                            {
                                y_max = Item[i].ListData[j].Value * ClassGetObject.getMulValue(Item[i].ListData[j].MulIndex);
                                y_min = Item[i].ListData[j].Value * ClassGetObject.getMulValue(Item[i].ListData[j].MulIndex);
                                flag = false;
                            }
                            else
                            {
                                if (Item[i].ListData[j].Value * ClassGetObject.getMulValue(Item[i].ListData[j].MulIndex) > y_max)
                                {
                                    y_max = Item[i].ListData[j].Value * ClassGetObject.getMulValue(Item[i].ListData[j].MulIndex);
                                }
                                if (Item[i].ListData[j].Value * ClassGetObject.getMulValue(Item[i].ListData[j].MulIndex) < y_min)
                                {
                                    y_min = Item[i].ListData[j].Value * ClassGetObject.getMulValue(Item[i].ListData[j].MulIndex);
                                }
                            }
                        }
                    }
                }
                if (y_max < y_min) 
                {
                    y_max = 21428;
                    y_min=-21428;
                }
                Max_y = (float)(y_max + (y_max - y_min) * 0.2 + 5.0);
                Min_y = (float)(y_min - (y_max - y_min) * 0.2 - 5.0);
                Max_x = 500;
                Min_x = 0;
                Sub_X = Max_x - Min_x;
                Sub_Y = Max_y - Min_y;
                contextMenuchange1.Checked = false;
                MyWaveShow();
                return;       
            }

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

            MyWaveShow();
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
         //   GL.glEnable(GL.GL_DEPTH_TEST);                        // 启用深度测试。根据坐标的远近自动隐藏被遮住的图形（材料）
            //GL.glEnable(GL.GL_POLYGON_SMOOTH);                      //过虑图形（多边形）的锯齿
            //GL.glHint(GL.GL_POLYGON_SMOOTH_HINT, GL.GL_NICEST);

            GL.glEnable(GL.GL_POINT_SMOOTH);                      //执行后，过虑线点的锯齿
            GL.glHint(GL.GL_POINT_SMOOTH_HINT, GL.GL_NICEST);

            GL.glEnable(GL.GL_LINE_SMOOTH);                      //执行后，过虑线段的锯齿
            GL.glHint(GL.GL_LINE_SMOOTH_HINT, GL.GL_NICEST);
            GL.glEnable(GL.GL_BLEND);   //启用混合模式
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
         //   GL.glDepthFunc(GL.GL_LESS);                        // 深度测试

        //    GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);    // 高质量显示抗锯齿
            GL.glMatrixMode(GL.GL_PROJECTION);//视角和投影角设置模式
            MyGL_glLoadIdentity();
            //设置视角fovy 视角上面到下面角度，aspect 画面宽和高之比，zNear 画面到观测点最近距离，zfar 画面到观测点最远距离
         //   GL.gluPerspective(90.0, ((double)(this.Width) / (double)(this.Height)), 1.0, 1000.0);//(90.0, ((double)(this.Width) / (double)(this.Height)), 1.0, 1000.0);//gluOrtho2D( 0.0,(GLdouble)800,0.0,(GLdouble)600);  
            GL.gluOrtho2D(0.0, this.Width, 0.0, this.Height);

            GL.glMatrixMode(GL.GL_MODELVIEW);//景物设置模式
            MyGL_glLoadIdentity();
            GL.glShadeModel(GL.GL_SMOOTH);//使用颜色过渡
            
        }

        private TextrueUnicode Textrue = new TextrueUnicode();

      //  GLFont glFont = new GLFont();//用来显示文字
        public List<WaveData> Item = new List<WaveData>();

        public override void glDraw()//官方指定重绘函数
        {

            MyGL_glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT |GL.GL_STENCIL_BUFFER_BIT | GL.GL_ACCUM_BUFFER_BIT);  // 清理视窗颜色缓存、深度缓存、模板缓存以及累积缓存。   
            WaveAutoAct();

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
            if (UpDataWaveFrom.UDBoxList == true)
            {
                GL.glNewList(1, GL.GL_COMPILE);
                DrawBoxList();
                GL.glEndList();
                UpDataWaveFrom.UDBoxList = false;
            }
            if (UpDataWaveFrom.UDCoordinateList == true)
            {
                GL.glNewList(2, GL.GL_COMPILE);
                DrawCoordinateList();
                GL.glEndList();
                UpDataWaveFrom.UDCoordinateList = false;
            }
            if (UpDataWaveFrom.UDInfoList == true)
            {
                GL.glNewList(3, GL.GL_COMPILE);
                DrawInfoList();
                GL.glEndList();
                //UpDataWaveFrom.UDInfoList = false;//数据不断跟新
            }
            if (UpDataWaveFrom.UDFillList == true)
            {
                GL.glNewList(4, GL.GL_COMPILE);
                DrawFillList(Color.White);
                GL.glEndList();
                UpDataWaveFrom.UDFillList = false;
            }
            if (UpDataWaveFrom.UDDataList == true)
            {
                GL.glNewList(5, GL.GL_COMPILE);
                DrawDataList();
                GL.glEndList();
                UpDataWaveFrom.UDDataList = false;//数据不断跟新
            }
            if (UpDataWaveFrom.UDMousePointList == true)
            {
                GL.glNewList(6, GL.GL_COMPILE);
                DrawMousePointList();
                GL.glEndList();
                UpDataWaveFrom.UDMousePointList = false;
            }
            if (UpDataWaveFrom.UDRectList == true)
            {
                GL.glNewList(7, GL.GL_COMPILE);
                DrawRectList();
                GL.glEndList();
                UpDataWaveFrom.UDRectList = false;
            }
            if (UpDataWaveFrom.UDBackGroundList == true)
            {
                GL.glNewList(8, GL.GL_COMPILE);
                DrawBackGroundList();
                GL.glEndList();
                UpDataWaveFrom.UDBackGroundList = false;
            }
            if (UpDataWaveFrom.UDStippleList == true)
            {
                GL.glNewList(9, GL.GL_COMPILE);
                DrawStippleList();
                GL.glEndList();
                UpDataWaveFrom.UDStippleList = false;
            }
            if (UpDataWaveFrom.UDInfoRectList == true)
            {
                GL.glNewList(10, GL.GL_COMPILE);
                DrawInfoRectList();
                GL.glEndList();
                UpDataWaveFrom.UDInfoRectList = false;
            }
            if (UpDataWaveFrom.UDrawXYlabelList == true)
            {
                GL.glNewList(11, GL.GL_COMPILE);
                DrawXYlabelList();
                GL.glEndList();
                UpDataWaveFrom.UDrawXYlabelList = false;
            }
            
        }
        private void ShowList() 
        {
        
            //GL.glCallList(1);
            //GL.glCallList(11);
            //GL.glCallList(2);
            //GL.glCallList(10);
            //GL.glCallList(3);
            //GL.glCallList(4);
            //GL.glCallList(5);
            //GL.glCallList(9);
            //GL.glCallList(7);
            //GL.glCallList(6);
            //GL.glCallList(8);

            GL.glCallList(8);
            GL.glCallList(6);
            GL.glCallList(7);
            GL.glCallList(9);
            GL.glCallList(5);
            GL.glCallList(4);
            GL.glCallList(3);
            GL.glCallList(10);
            GL.glCallList(2);
            GL.glCallList(11);
            GL.glCallList(1);
        }

        #endregion
         

        #region 画波形
        //四个临时变量记录矩形选取框位置
        private float Menuchangex1, Menuchangex2, Menuchangey1, Menuchangey2;
        //临时变量记录波形缩放矩形选取框大小是否合理
        private bool Menuchangeflag = false;

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
        private void DrawBoxList() 
        {
            GL.glLineWidth(2.0f);//设置坐标轴粗细
            MyGL_glBegin(GL.GL_LINES);//画不闭合折线
            GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.DimGray));//设置坐标轴颜色

            MyGL_glVertex2i(left_botton, this.Height - 5);
            MyGL_glVertex2i(left_botton, 30);

            MyGL_glVertex2i(left_botton - 5, 35);
            MyGL_glVertex2i(this.Width - right_botton, 35);

            MyGL_glVertex2i(this.Width - right_botton, 35);
            MyGL_glVertex2i(this.Width - right_botton, this.Height - 5);

            MyGL_glVertex2i(this.Width - right_botton, this.Height - 5);
            MyGL_glVertex2i(left_botton - 5, this.Height - 5);

            for (int i = 0; i < 10; i++)
            {
                MyGL_glVertex2i(left_botton + 5, (this.Height - 40) * i / 10 + 35);
                MyGL_glVertex2i(left_botton - 5, (this.Height - 40) * i / 10 + 35);

                MyGL_glVertex2i((this.Width - left_botton - right_botton) * i / 10 + left_botton, 40);
                MyGL_glVertex2i((this.Width - left_botton - right_botton) * i / 10 + left_botton, 30);
            }

            MyGL_glEnd();
        }//框
        private void DrawCoordinateList() 
        {

            for (int i = 0; i < 11; i++)
            {
                Textrue.DrawText(((Max_y - Min_y) / 10 * i + Min_y).ToString(), new Point(30, (this.Height - 40) * i / 10 + 25), 18, Color.Black, 255.0f);
                if (i != 10)
                {
                    Textrue.DrawText(((Max_x - Min_x) / 10 * i + Min_x).ToString("G5"), new Point((this.Width - left_botton - right_botton) * i / 10 + left_botton - 4, 15), 18, Color.Black, 255.0f);
                }
                else
                {
                    Textrue.DrawText(((Max_x - Min_x) / 10 * i + Min_x).ToString("G4"), new Point((this.Width - left_botton - right_botton) * i / 10 + left_botton - 34, 15), 18, Color.Black, 255.0f);
                }

            }
        }//坐标轴
        private void DrawXYlabelList() 
        {
            Textrue.DrawText(Xlable.Content.ToString(), new Point((this.Width + left_botton - right_botton) / 2, 0), 18, Color.Black, 255.0f);
            Textrue.DrawTextL(Ylable.Content.ToString(), new Point(5, this.Height / 2), 18, Color.Black, 255.0f);
        }
        private void WaveAutoAct() 
        {
            if (!(Item.Count > 0))
            {
                return;
            }
            int tmp_num = 0;
            for (int i = 0; (i < 500 && i < Item.Count); i++)
            {
                if (Wave_Auto == true && Item[i].ListData.Count > 1 && Item[i].waveon == true && i < datainforect.Count && datainforect[i].tmpwaveon == true)
                {
                    if (Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex) > Max_y)
                    {
                        Max_y = (int)(Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex));
                        UpDataWaveFrom.UDCoordinateList = true;
                    }
                    if (Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex) < Min_y)
                    {
                        Min_y = (int)(Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex));
                        UpDataWaveFrom.UDCoordinateList = true;
                    }
                    if (Max_y > 60000) Max_y = 60000;
                    if (Min_y < -60000) Min_y = -60000;
                    Sub_Y = Max_y - Min_y;
                }
                if (Item[i].waveon == true)
                {
                    tmp_num++;
                }
            }
            Data_Num = tmp_num;
        }//自适应坐标
        private void DrawInfoRectList() 
        {
            if (!(Item.Count > 0))
            {
                return;
            }
            int j = 0;
            for (int i = 0; i < Item.Count; i++)
            {
                if (Item[i].ListData.Count > 1 && Item[i].waveon == true)
                {
                    if (datainforect[j].tmpwaveon==true)
                    {
                        MyGL_glBegin(GL.GL_QUADS);
                        GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Item[i].Color));


                        MyGL_glVertex2i(this.Width - right_botton + 5, this.Height - 5 - eye_y - j * 38);
                        MyGL_glVertex2i(this.Width - right_botton + 5, this.Height - 20 - eye_y - j * 38);
                        MyGL_glVertex2i(this.Width - right_botton + 20, this.Height - 20 - eye_y - j * 38);
                        MyGL_glVertex2i(this.Width - right_botton + 20, this.Height - 5 - eye_y - j * 38);
                    }
                    else
                    {
                        GL.glLineWidth(1.0f);
                        MyGL_glBegin(GL.GL_LINE_LOOP);
                        GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Item[i].Color));


                        MyGL_glVertex2i(this.Width - right_botton + 5, this.Height - 5 - eye_y - j * 38);
                        MyGL_glVertex2i(this.Width - right_botton + 5, this.Height - 20 - eye_y - j * 38);


                        MyGL_glVertex2i(this.Width - right_botton + 20, this.Height - 20 - eye_y - j * 38);
                        MyGL_glVertex2i(this.Width - right_botton + 20, this.Height - 5 - eye_y - j * 38);



                    }


                    datainforect[j].rect1 = new Rectangle(this.Width - right_botton + 5, 5 + eye_y + j * 38, 15, 15);
                    datainforect[j].rect2 = new Rectangle(this.Width - right_botton + 5, 5 + eye_y + j * 38, right_botton, 38);
                    MyGL_glEnd();
                    j++;
                }
            }
        }
        private void DrawInfoList() 
        {
            if (!(Item.Count > 0))
            {
                return;
            }

            int j = 0;
            for (int i = 0; i < Item.Count; i++)
            {
                if (Item[i].ListData.Count > 1 && Item[i].waveon == true)
                {

                    //MyGL_glBegin(GL.GL_QUADS);
                    //GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Item[i].Color));


                    //MyGL_glVertex2i(this.Width - left_botton + 5, this.Height - 5 - eye_y - j * 38);
                    //MyGL_glVertex2i(this.Width - left_botton + 5, this.Height - 20 - eye_y - j * 38);
                    //MyGL_glVertex2i(this.Width - left_botton + 20, this.Height - 20 - eye_y - j * 38);
                    //MyGL_glVertex2i(this.Width - left_botton + 20, this.Height - 5 - eye_y - j * 38);

                    //MyGL_glEnd();
                    Textrue.DrawText(Item[i].Name, new Point(this.Width - right_botton + 25, this.Height - 20 - eye_y - j * 38), 18, Item[i].Color, 255.0f);
                    Textrue.DrawText((Item[i].ListData[0].Value * ClassGetObject.getMulValue(Item[i].ListData[0].MulIndex)).ToString(), new Point(this.Width - right_botton + 5, this.Height - 40 - eye_y - j * 38), 20, Item[i].Color, 255.0f);

                    j++;
                }
            }
            if (datainforect.Count != j)
            {
                if (datainforect.Count > j) 
                {
                    datainforect.RemoveRange(j, datainforect.Count-j);
                }
                if (datainforect.Count < j)
                {
                    for (int i = datainforect.Count; i < j; i++)
                    {
                        datainforect.Add(new WaveInfoRect());
                    }
                }
                for (int i = 0; i < datainforect.Count; i++) 
                {
                    datainforect[i].tmpwaveon = true;
                }
                UpDataWaveFrom.UDInfoRectList = true;
            }
        }//右侧信息栏
        private void DrawFillList(Color color) //填充区域
        {
            MyGL_glBegin(GL.GL_QUADS);
            //GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(color, 255));
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.White, 255));
            MyGL_glVertex2i(0, 0);
            MyGL_glVertex2i(left_botton, 0);
            MyGL_glVertex2i(left_botton, this.Height);
            MyGL_glVertex2i(0, this.Height);


            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(color, 255));

            MyGL_glVertex2i(0, 0);
            MyGL_glVertex2i(this.Width, 0);
            MyGL_glVertex2i(this.Width, 35);
            MyGL_glVertex2i(0, 35);


            MyGL_glVertex2i(this.Width, 0);
            MyGL_glVertex2i(this.Width, this.Height);
            MyGL_glVertex2i(this.Width - right_botton, this.Height);
            MyGL_glVertex2i(this.Width - right_botton, 0);


            MyGL_glVertex2i(this.Width, this.Height);
            MyGL_glVertex2i(0, this.Height);
            MyGL_glVertex2i(0, this.Height - 5);
            MyGL_glVertex2i(this.Width, this.Height - 5);
            MyGL_glEnd();
        }
        private void DrawDataList() 
        {
            if ((bool)WaleMode.Content == true)
            {


                if (!(Item.Count > 0))
                {
                    return;
                }
                int Count=0;


                int k = 0;
                for (int i = 0; i < Item.Count; i++)
                {
                    if (Max_x - Min_x < 1000)
                    {
                        GL.glLineWidth(2.0f);
                    }
                    else
                    {
                        GL.glLineWidth(1.0f);
                    }
                    if (k != WireData - 1)
                    {
                        DataLeftToRightConfig(i, Count, ref k);
                    }
                    else 
                    {
                        k++;
                    }
                  
                }
                if (WireData != 0)
                {
                    k = WireData - 1;
                    int j = 0, n = WireData;
                    foreach (WaveData tmp in Item)
                    {
                        j++;
                        if (tmp.waveon == true)
                        {
                            n--;
                            if (n == 0)
                            {
                                break;
                            }
                        }
                    }
                    GL.glLineWidth(5.0f);
                    DataLeftToRightConfig(j - 1, Count, ref k);
                }
            }
            else 
            {
                if (!(Item.Count > 0))
                {
                    return;
                }
                int Count=0;


                int k = 0;
                for (int i = 0; i < Item.Count; i++)
                {
                    if (Max_x - Min_x < 1000)
                    {
                        GL.glLineWidth(2.0f);
                    }
                    else
                    {
                        GL.glLineWidth(1.0f);
                    }

                    if (k != WireData - 1)
                    {
                        DataRightToLeftConfig(i, Count, ref k);
                    }
                    else
                    {
                        k++;
                    }
                    
                }
                if (WireData != 0) 
                {
                    k = WireData - 1;
                    int j = 0, n = WireData;
                    foreach (WaveData tmp in Item) 
                    {
                        j++;
                        if (tmp.waveon == true) 
                        {
                            n--;
                            if (n == 0) 
                            {
                                break;
                            }
                        }
                    }
                   GL.glLineWidth(5.0f);
                   DataRightToLeftConfig(j-1, Count, ref k);
                }
            }
        }

        private void DataLeftToRightConfig(int i,int Count, ref int  k)
        {
            MyGL_glBegin(GL.GL_LINE_STRIP);//画不闭合折线
            GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Item[i].Color));
            try
            {
                Count = (int)(Item[i].ListData.Count - Data_Length - 1);//先默认保存1000组数据
                if (Count > 0)
                {
                    Item[i].ListData.RemoveRange(Item[i].ListData.Count - Count, Count);
                }
                if (Item[i].ListData.Count > 1 && Item[i].waveon == true)
                {

                    if (datainforect[k].tmpwaveon == true)
                    {
                        for (int j = (int)(Min_x); j < Item[i].ListData.Count - 1; j++)
                        {

                            MyGL_glVertex2i(left_botton + (int)((j - Min_x) * (this.Width - right_botton) / (Max_x - Min_x)), (int)(40 + (Item[i].ListData[j].Value * ClassGetObject.getMulValue(Item[i].ListData[j].MulIndex) - Min_y) * (this.Height - 40) / (Max_y - Min_y)));
                            MyGL_glVertex2i(left_botton + (int)((j + 1 - Min_x) * (this.Width - right_botton) / (Max_x - Min_x)), (int)(40 + (Item[i].ListData[j + 1].Value * ClassGetObject.getMulValue(Item[i].ListData[j + 1].MulIndex) - Min_y) * (this.Height - 40) / (Max_y - Min_y)));
                            if (j + 1 > Max_x)
                            {
                                break;
                            }
                        }
                    }
                    k++;
                }
            }
            catch
            {
                ;
            }
            MyGL_glEnd();
        }

        private void DataRightToLeftConfig(int i, int Count, ref int k)
        {
            MyGL_glBegin(GL.GL_LINE_STRIP);//画不闭合折线
            GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Item[i].Color));
            try
            {
                Count = (int)(Item[i].ListData.Count - Data_Length - 1);//先默认保存1000组数据
                if (Count > 0)
                {
                    Item[i].ListData.RemoveRange(Item[i].ListData.Count - Count, Count);
                }
                if (Item[i].ListData.Count > 1 && Item[i].waveon == true)
                {
                    if (datainforect[k].tmpwaveon == true)
                    {
                        for (int j = (int)(Min_x); j < Item[i].ListData.Count - 1; j++)
                        {

                            MyGL_glVertex2i(left_botton + (int)((j - Min_x) * (this.Width - right_botton) / (Max_x - Min_x)), (int)(40 + (Item[i].ListData[Item[i].ListData.Count - 1 - j].Value * ClassGetObject.getMulValue(Item[i].ListData[Item[i].ListData.Count - 1 - j].MulIndex) - Min_y) * (this.Height - 40) / (Max_y - Min_y)));
                            MyGL_glVertex2i(left_botton + (int)((j + 1 - Min_x) * (this.Width - right_botton) / (Max_x - Min_x)), (int)(40 + (Item[i].ListData[Item[i].ListData.Count - 2 - j].Value * ClassGetObject.getMulValue(Item[i].ListData[Item[i].ListData.Count - 2 - j].MulIndex) - Min_y) * (this.Height - 40) / (Max_y - Min_y)));
                            if (j + 1 > Max_x)
                            {
                                break;
                            }
                        }
                    }
                    k++;
                }
            }
            catch
            {
                ;
            }
            MyGL_glEnd();
        }
        private void DrawMousePointList() 
        {
            if (ToolStripMenuItem1.Checked == true)
            {

                GL.glLineWidth(1.0f);//设置线粗细
                MyGL_glBegin(GL.GL_LINES);
                GL.glColor3fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.Blue));


                MyGL_glVertex2i(mouseLocation.X, this.Height);
                MyGL_glVertex2i(mouseLocation.X, 0);
                MyGL_glVertex2i(0, this.Height - mouseLocation.Y);
                MyGL_glVertex2i(this.Width, this.Height - mouseLocation.Y);
                MyGL_glEnd();

                float tmpx = (float)(Min_x + (mouseLocation.X - left_botton) * (Max_x - Min_x) / (this.Width - left_botton - right_botton));
                float tmpy = (float)(Max_y - (mouseLocation.Y - 5) * (Max_y - Min_y) / (this.Height - 40));
                Textrue.DrawText("X:" + tmpx.ToString() + " , " + "Y:" + tmpy.ToString(), new Point(mouseLocation.X, this.Height - mouseLocation.Y), 18, Color.Black, 255.0f);

            }
        }
        private void DrawRectList() 
        {
            if (contextMenuchange1.Checked == true && (MouseFlag == MyMouse.RightFlag2 || MouseFlag == MyMouse.RightFlag3))
            {

                GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.FromArgb(0, 230, 230), 50));
                MyGL_glBegin(GL.GL_QUADS);


                MyGL_glVertex2i(mousePosition.X, this.Height - mousePosition.Y);
                MyGL_glVertex2i(mousePosition.X, this.Height - mouseLocation.Y);
                MyGL_glVertex2i(mouseLocation.X, this.Height - mouseLocation.Y);
                MyGL_glVertex2i(mouseLocation.X, this.Height - mousePosition.Y);

                MyGL_glEnd();

                Menuchangex1 = (float)(Min_x + (mousePosition.X - left_botton) * (Max_x - Min_x) / (this.Width - left_botton - right_botton));
                Menuchangex2 = (float)(Min_x + (mouseLocation.X - left_botton) * (Max_x - Min_x) / (this.Width - left_botton - right_botton));
                Menuchangey1 = (float)(Max_y - (mousePosition.Y - 5) * (Max_y - Min_y) / (this.Height - 40));
                Menuchangey2 = (float)(Max_y - (mouseLocation.Y - 5) * (Max_y - Min_y) / (this.Height - 40));
                if (Menuchangex1 < 0) 
                {
                    Menuchangex1 = 0;
                }
                if (Menuchangex2 < 0) 
                {
                    Menuchangex2 = 0;
                }
                Menuchangeflag = true;

            }


        }
        private void DrawStippleList() //画虚线
        {

            GL.glPointSize(1.0f);
            MyGL_glBegin(GL.GL_POINTS);
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.FromArgb(0, 0, 0), 255));
            for (int i = 1; i < 10; i++)
            {
                for (int j = 0; j <= (this.Width - left_botton - right_botton) / 3; j++)
                {
                    MyGL_glVertex2i(left_botton + j * 3, (this.Height - 40) * i / 10 + 34);
                }

                for (int j = 0; j <= (this.Height - 40) / 3; j++)
                {
                    MyGL_glVertex2i((this.Width - left_botton - right_botton) * i / 10 + left_botton, 35 + j * 3);
                }

            }
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(Color.FromArgb(0, 0, 255), 255));
            MyGL_glVertex2i(201, 201);
            MyGL_glEnd();

        }

        private int left_botton,right_botton;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
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
    public class WaveListupdatamsg 
    {
        public bool UDBoxList;
        public bool UDCoordinateList;
        public bool UDInfoList;
        public bool UDFillList;
        public bool UDDataList;
        public bool UDMousePointList;
        public bool UDRectList;
        public bool UDBackGroundList;
        public bool UDStippleList;
        public bool UDInfoRectList;
        public bool UDrawXYlabelList;
        public WaveListupdatamsg()
        {
            UDBoxList = true;
            UDCoordinateList = true;
            UDInfoList = true;
            UDFillList = true;
            UDDataList = true;
            UDMousePointList = true;
            UDRectList = true;
            UDBackGroundList = true;
            UDStippleList = true;
            UDInfoRectList = true;
            UDrawXYlabelList = true;
        }

    }
    public class WaveInfoRect 
    {
        public Rectangle rect1 = new Rectangle();
        public Rectangle rect2 = new Rectangle();
        public bool tmpwaveon = true;

        public WaveInfoRect() 
        {
        
        }
    }

    
}
