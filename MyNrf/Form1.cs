using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
using System.Net;
using System.Net.Sockets;
using MyNrf.Properties;
using MyNrf.这里写仿真程序;
using System.Management;


//using System.Collections.Specialized;　
namespace MyNrf
{
    public partial class Form1 : Form
    {
        #region 参数定义
        #region 串口类
        bool UART_OPEN = false;//串口开关
        public delegate void UpdateTextEventHandler();
        public UpdateTextEventHandler Update_Uart_Data;//用于委事件托
        private bool Listening = false;//用于监听串口
        byte flag = 0;
        public static string NrfChannel = "频道:001";
        private bool close = false;//串口是否正在关闭
        private bool waitcmd = false;
        private List<byte> requests = new List<byte>();//发送数据缓存
        private AutoResetEvent sendWaiter;//AutoResetEvent接收到信号之后自动清零 ManualResetEvent不清零
        private bool sendWaiterstop = false;
        private bool munkeyin = false;
        private bool minpoint = false;
        private int minpointcnt = 0;

        #endregion

        #region 示波器类
        Form frmAllScreen;//全屏显示时用到
        Pos WavePos = new Pos();//记录在窗体里的位置
        #endregion

        #region 数据采集类
        List<byte> Data_Image = new List<byte>();
        List<byte> Coefficient_Info = new List<byte>();
        List<byte> Parameter_Info = new List<byte>();
        ushort Pixels_height = 70;
        ushort Pixels_width = 200;
        ushort CCD_height = 3;
        ushort CCD_width = 128;
        ushort Coefficient_Num = 1; //系数
        ushort Parameter_Num = 1; //参数  
        ushort All_length = 0;
        Bitmap bmpReal, bmpFit;
        #endregion

        #region 数据存储类
        byte[,] Image_Fit_Pixels;//保存拟图
        byte[,] Pixels;//保存像素
        List<DataAll> DATA_Save = new List<DataAll>();
        bool Save_Flag = false;
        bool Read_Pic = true;
        byte[,] CCDD, CCDLRC;
        #endregion

        #region 中断类
        #endregion

        #region 参数选项类
        public static List<ClassParValue> ParValue = new List<ClassParValue>();
        public static List<ClassParValue> ConstValue = new List<ClassParValue>();
        List<CopyDataInfo> DataCopyList = new List<CopyDataInfo>();
        int Copy_Coefficient_Num = 0;
        int Copy_Parameter_Num = 0;
        int Copy_Local_Data_Num = 0;
        string MyClipboard = string.Empty;
        #endregion

        #region 配置文件
        static public MyXmlConfig My_Xml = new MyXmlConfig();
        #endregion

        #region 本地数据
        int Local_Data_Num = 0;
        SmartProcess MySmartProcess;//小车图像处理类
        CCDSmartProcess MyCCDSmartProcess = new CCDSmartProcess();
        bool OPENFILEFLAG = false;
        public static string VoiceString = string.Empty;
        #endregion

        #region 动画启动窗体
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        /*
        1. AW_SLIDE : 使用滑动类型, 默认为该类型. 当使用 AW_CENTER 效果时, 此效果被忽略
        2. AW_ACTIVE: 激活窗口, 在使用了 AW_HIDE 效果时不可使用此效果
        3. AW_BLEND: 使用淡入效果
        4. AW_HIDE: 隐藏窗口
        5. AW_CENTER: 与 AW_HIDE 效果配合使用则效果为窗口几内重叠,  单独使用窗口向外扩展.
        6. AW_HOR_POSITIVE : 自左向右显示窗口
        7. AW_HOR_NEGATIVE: 自右向左显示窗口
        8. AW_VER_POSITIVE: 自顶向下显示窗口
        9. AW_VER_NEGATIVE : 自下向上显示窗口
        */
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;
        public const Int32 AW_ACTIVATE = 0x00020000;
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;
        #endregion

        #region  其他参数
        private int trackbarvalue = 0;
        private System.Drawing.Point mousePosition;//记录鼠标坐标
        public static ClassFitColor FitColor = new ClassFitColor();//用用于保存拟图画板
        public static ClassCCDColor CCDColor = new ClassCCDColor();//用于保存光电组CCD个数据颜色
        public static MyMode mymode;//上位机模式
        string Open_File_Address = string.Empty;//打开文件地址
        static public string Save_File_Address = string.Empty;//存储文件地址
        string Child_Address = string.Empty;//文件内部地址
        public static MyResult MyGroup = MyResult.NULL;
        Thread ThreadWrite = null;//线程，防止写硬盘时界面卡死
        // Thread ThreadShow = null;//线程，防止写硬盘时界面卡死
        public static MyCursors MyCursor = new MyCursors();
        public static bool Pit_Map = false;//图像坐标轴辅助显示开关
        public static int Pit_Play_Speed = 25;//播放速度
        public static decimal WaveDataLength = 1000;
        private bool timerPlay2flag = false;
        private frmstatus tmpfrmAllScreen = frmstatus.frmdefault;
        public static float[,] my_value = new float[50,2000];
        public int my_value_arreng=0;
        //private int portNum = 8000;
        //TcpListener listener = new TcpListener(portNum);
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern Int32 WritePrivateProfileString(string Unicode, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string Unicode, string key, string
        def, StringBuilder retval, int size, string filePath);
        //注册热键api
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint control, Keys vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        public static bool PolyKey = false;
        public static int Polyfit = 3;
        public static bool LogKey = false;
        public static int LogLine = 255;
        public static MyNotifyPropertyClass wavexlable = new MyNotifyPropertyClass();
        public static MyNotifyPropertyClass waveylable = new MyNotifyPropertyClass();
        public static MyNotifyPropertyClass wavemode = new MyNotifyPropertyClass();
        #endregion

        #endregion
        #region 初始化
        //创建主窗体

        public Form1()
        {
            MySmartProcess = new SmartProcess(this);
            Control.CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //MessageBox.Show(MyMath.LaguerreL(8,0,1).ToString());
            AnimateWindow(this.Handle, 300, AW_BLEND + AW_CENTER);
            //MyComplex c1 = 2.3d;
            //c1 = c1 + 2.3;
            //MessageBox.Show(c1.ToString());

            #region 绑定数据
            mypaint1.Xlable.SetBinding(MyDendencyControl.ContentDependencyProperty, new MyBinding(wavexlable, "Value"));
            mypaint1.Ylable.SetBinding(MyDendencyControl.ContentDependencyProperty, new MyBinding(waveylable, "Value"));
            mypaint1.WaleMode.SetBinding(MyDendencyControl.ContentDependencyProperty, new MyBinding(wavemode, "Value"));
            wavexlable.SetValue(mypaint1.Xlable.Content);
            waveylable.SetValue(mypaint1.Ylable.Content);
            wavemode.SetValue(mypaint1.WaleMode.Content);
            #endregion

        }
        //打开主窗体触发事件
        private void FrmNRF_Load(object sender, EventArgs e)
        {
            this.Activate();
            this.Invalidate();
            //this.Cursor = MyCursor.Default;
            WavePos.Top = mypaint1.Top;
            WavePos.Left = mypaint1.Left;
            WavePos.Width = mypaint1.Width;
            WavePos.Height = mypaint1.Height;
            mypaint1.MouseDoubleClick += new MouseEventHandler(this.PaintWave_DoubleClick);
            Update_Uart_Data = new UpdateTextEventHandler(Serialreadata);
            mypaint1.MyWaveShow();
            XmlFileRead();
            timePlay.Interval = Pit_Play_Speed;
            timerPlay2.Interval = 1;
            myDataInfo1.MyShow();
            myDataInfo2.MyShow();
            mypaint1.MyWaveShow();
            Mode_Config();
            this.sendWaiter = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Send));//开启发送线程
            //注册热键
            UnregisterHotKey(this.Handle, 225);
            RegisterHotKey(this.Handle, 225, 0x0002 | 0x0001, Keys.Space);//ctrl 0x0002 alt 0x0001
            UnregisterHotKey(this.Handle, 226);
            RegisterHotKey(this.Handle, 226, 0x0002 | 0x0001, Keys.Enter);//ctrl 0x0002 alt 0x0001
            UnregisterHotKey(this.Handle, 227);
            RegisterHotKey(this.Handle, 227, 0x0002 | 0x0001, Keys.OemQuestion);//ctrl 0x0002 alt 0x0001

            if (MyGroup == MyResult.摄像头组)
            {
                this.PicFit.lbl.MouseMove += new MouseEventHandler(PicFit_MouseMove);
                this.PicReal.lbl.MouseMove += new MouseEventHandler(PicReal_MouseMove);
                this.myPictureBox1.lbl.MouseMove += new MouseEventHandler(myPictureBox1_MouseMove);
                this.PicReal.lbl.MouseLeave += new EventHandler(Pic_MouseLeave);
                this.PicFit.lbl.MouseLeave += new EventHandler(Pic_MouseLeave);
                this.myPictureBox1.lbl.MouseLeave += new EventHandler(Pic_MouseLeave);

                this.Controls.Remove(CCDPicReal);
                this.Controls.Remove(CCDPicFit);
                Read_Pic = true;
                return;
            }
            else if (MyGroup == MyResult.光电组)
            {
                this.CCDPicFit.lbl.MouseMove += new MouseEventHandler(PicFit_MouseMove);
                this.CCDPicReal.lbl.MouseMove += new MouseEventHandler(PicReal_MouseMove);
                this.CCDPicReal.lbl.MouseLeave += new EventHandler(Pic_MouseLeave);
                this.CCDPicFit.lbl.MouseLeave += new EventHandler(Pic_MouseLeave);
                this.Controls.Remove(PicReal);
                this.Controls.Remove(PicFit);
                Read_Pic = false;

                return;
            }

        }
        //主窗体重绘触发事件
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 25), new Point(this.Width, 25));

        }
        //模式匹配
        private void Mode_Config()
        {
            if (MyGroup == MyResult.摄像头组)
            {
                //
                this.模式ToolStripMenuItem.DropDownItems.Remove(this.光电串口模式ToolStripMenuItem);
                this.模式ToolStripMenuItem.DropDownItems.Remove(this.光电图像模式ToolStripMenuItem);
                this.模式ToolStripMenuItem.DropDownItems.Remove(this.光电无线模式ToolStripMenuItem);
                this.模式ToolStripMenuItem.DropDownItems.Remove(this.光电仿真模式ToolStripMenuItem);
                //这里分流

                //
                if (mymode == MyMode.Uart_Par)
                {
                    Cur_Mode.MyText = "串口参数模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    timePlay.Enabled = false;
                    trackBar1.Enabled = false;
                    OPENFILEFLAG = false;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("摄像头组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Uart_Pic)
                {
                    Cur_Mode.MyText = "串口图像模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = false;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("摄像头组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Uart_PicGray)
                {
                    Cur_Mode.MyText = "串口灰度图像模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = false;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("摄像头组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Nrf_Pic)
                {
                    Cur_Mode.MyText = "无线图像模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = false;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("摄像头组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Nrf_Par)
                {
                    Cur_Mode.MyText = "无线参数模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = false;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("摄像头组开启模式", mymode.ToString()), false);
                    //   MessageBox.Show(mymode.ToString());
                    XmlFileRead();
                }
                else if (mymode == MyMode.Local_Sim)
                {
                    Cur_Mode.MyText = "本地模式";
                    this.打开.Enabled = true;
                    this.上一场.Enabled = true;
                    this.下一场.Enabled = true;
                    this.播放.Enabled = true;
                    this.button1.Enabled = false;
                    this.button2.Enabled = false;
                    this.comboBox1.Enabled = false;
                    this.comboBox2.Enabled = false;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = true;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = false;
                    this.写入数据.Enabled = false;
                    this.清空缓存.Enabled = false;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("摄像头组开启模式", mymode.ToString()), false);
                }
                else if (mymode == MyMode.Local_Debug)
                {
                    Cur_Mode.MyText = "图像仿真模式";
                    this.打开.Enabled = true;
                    this.上一场.Enabled = true;
                    this.下一场.Enabled = true;
                    this.播放.Enabled = true;
                    this.button1.Enabled = false;
                    this.button2.Enabled = false;
                    this.comboBox1.Enabled = false;
                    this.comboBox2.Enabled = false;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = true;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = false;
                    this.写入数据.Enabled = false;
                    this.清空缓存.Enabled = false;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("摄像头组开启模式", mymode.ToString()), false);
                }
            }
            else if (MyGroup == MyResult.光电组)
            {
                this.模式ToolStripMenuItem.DropDownItems.Remove(this.图像模式ToolStripMenuItem);
                this.模式ToolStripMenuItem.DropDownItems.Remove(this.图像仿真ToolStripMenuItem);
                this.模式ToolStripMenuItem.DropDownItems.Remove(this.本地模式ToolStripMenuItem);
                this.模式ToolStripMenuItem.DropDownItems.Remove(this.串口图像模式ToolStripMenuItem);
                if (mymode == MyMode.Uart_Par)
                {
                    Cur_Mode.MyText = "串口参数模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    timePlay.Enabled = false;
                    trackBar1.Enabled = false;
                    OPENFILEFLAG = false;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("光电组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Nrf_Par)
                {
                    Cur_Mode.MyText = "无线参数模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    timePlay.Enabled = false;
                    trackBar1.Enabled = false;
                    OPENFILEFLAG = false;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("光电组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Uart_CCD)
                {
                    Cur_Mode.MyText = "光电串口模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = false;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("光电组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Nrf_CCD)
                {
                    Cur_Mode.MyText = "光电无线模式";
                    this.打开.Enabled = false;
                    this.上一场.Enabled = false;
                    this.下一场.Enabled = false;
                    this.播放.Enabled = false;
                    this.button1.Enabled = true;
                    this.button2.Enabled = true;
                    this.comboBox1.Enabled = true;
                    this.comboBox2.Enabled = true;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = false;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = true;
                    this.写入数据.Enabled = true;
                    this.清空缓存.Enabled = true;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("光电组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Local_CCD_Debug)
                {
                    Cur_Mode.MyText = "光电仿真模式";
                    this.打开.Enabled = true;
                    this.上一场.Enabled = true;
                    this.下一场.Enabled = true;
                    this.播放.Enabled = true;
                    this.button1.Enabled = false;
                    this.button2.Enabled = false;
                    this.comboBox1.Enabled = false;
                    this.comboBox2.Enabled = false;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = true;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = false;
                    this.写入数据.Enabled = false;
                    this.清空缓存.Enabled = false;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("光电组开启模式", mymode.ToString()), false);
                    XmlFileRead();
                }
                else if (mymode == MyMode.Local_CCD)
                {
                    Cur_Mode.MyText = "光电本地模式";
                    this.打开.Enabled = true;
                    this.上一场.Enabled = true;
                    this.下一场.Enabled = true;
                    this.播放.Enabled = true;
                    this.button1.Enabled = false;
                    this.button2.Enabled = false;
                    this.comboBox1.Enabled = false;
                    this.comboBox2.Enabled = false;
                    timePlay.Enabled = false;
                    OPENFILEFLAG = false;
                    trackBar1.Enabled = true;
                    Save_Flag = false;
                    this.存储开关.Text = "写入缓存";
                    this.存储开关.Enabled = false;
                    this.写入数据.Enabled = false;
                    this.清空缓存.Enabled = false;
                    myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                    myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                    myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                    myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                    mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                    ParValue.RemoveRange(0, ParValue.Count);
                    ConstValue.RemoveRange(0, ConstValue.Count);
                    XmlFileWrite(new XmlInfo("光电组开启模式", mymode.ToString()), false);
                }

            }
        }
        //热键
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:    //这个是window消息定义的注册的热键消息 
                    if (m.WParam.ToString().Equals("225")) //ctrl+alt+space
                    {
                        if (serialPort1.IsOpen)
                        {
                            if (mymode == MyMode.Nrf_Pic || mymode == MyMode.Nrf_CCD || mymode == MyMode.Nrf_Par)
                            {
                                byte[] Buff = new byte[] { (byte)CmdMode.UcStop_Car };
                                Monitor.Enter(this.requests);
                                requests.AddRange(Buff);
                                Monitor.Exit(this.requests);
                                sendWaiter.Set();
                                timerPlay2.Enabled = true;
                            }
                        }

                    }
                    else if (m.WParam.ToString().Equals("226"))//ctrl+alt+enter
                    {
                        if (serialPort1.IsOpen)
                        {
                            if (mymode == MyMode.Nrf_Pic || mymode == MyMode.Nrf_CCD || mymode == MyMode.Nrf_Par)
                            {
                                byte[] Buff = new byte[] { (byte)CmdMode.Stop_Car };
                                Monitor.Enter(this.requests);
                                requests.AddRange(Buff);
                                Monitor.Exit(this.requests);
                                sendWaiter.Set();
                                timerPlay2.Enabled = true;
                            }
                        }
                    }
                    else if (m.WParam.ToString().Equals("227"))//ctrl+alt+rightshift
                    {
                        if (tmpfrmAllScreen == frmstatus.frmdefault)
                        {
                            frmAllScreen = new MyForm();
                            frmAllScreen.Name = "Wave";
                            frmAllScreen.Width = mypaint1.Width;
                            frmAllScreen.Height = mypaint1.Height + 200;
                            frmAllScreen.Controls.Add(mypaint1);
                            frmAllScreen.Resize += new EventHandler(frmAllScreen_Resize);
                            frmAllScreen.Show();
                            tabControl1.TabPages.Remove(tabPage1);
                            tabControl1.SelectedIndex = 1;
                            frmAllScreen.KeyPreview = true;
                            mypaint1.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(PaintWave_DoubleClick);
                            mypaint1.UpDataWaveFrom = new WaveListupdatamsg();
                            mypaint1.MyWaveShow();

                            tmpfrmAllScreen = frmstatus.frmfast;
                            break;
                        }
                        if (tmpfrmAllScreen == frmstatus.frmfast)
                        {

                            mypaint1.Top = WavePos.Top;
                            mypaint1.Left = WavePos.Left;
                            mypaint1.Height = WavePos.Height;
                            mypaint1.Width = WavePos.Width;
                            this.tabPage1.Controls.Add(mypaint1);
                            mypaint1.UpDataWaveFrom = new WaveListupdatamsg();
                            tabControl1.TabPages.Insert(0, tabPage1);
                            mypaint1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(PaintWave_DoubleClick);
                            tmpfrmAllScreen = frmstatus.frmdefault;
                            frmAllScreen.Resize -= new EventHandler(frmAllScreen_Resize);
                            tmpfrmAllScreen = frmstatus.frmdefault;
                            frmAllScreen.Close();
                            frmAllScreen.Dispose();
                            GC.Collect();
                            break;
                        }

                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
        }

        #endregion
        #region 基本逻辑
        //窗体最小化时触发事件，窗体最小化到状态栏
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == false)
                this.Visible = true;
            notifyIcon1.Icon = this.Icon;
        }
        //鼠标左键单击窗体事件
        private void Top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePosition.X = e.X;
                this.mousePosition.Y = e.Y;
            }
        }
        //显示类型
        private void element_show()
        {
            //element.Text = "-十字-" + SmartProcess.Element_Cross + "-坡道-" + SmartProcess.Ramp_Flag + "-环阶段-" + SmartProcess.Ring_State_Flag + "-起跑线-" + SmartProcess.Starting_Line_Flag + "-障碍-" + SmartProcess.L_Obstacle_Flag + SmartProcess.R_Obstacle_Flag;
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
        //鼠标在拟图区域内并移动触发事件
        private void PicFit_MouseMove(object sender, MouseEventArgs e)
        {
            //this.Cursor = MyCursor.Cross;
            if (this.Listening == true)
            {
                return;
            }
            if (MyGroup == MyResult.摄像头组)
            {
                if (Pit_Map == false)
                {
                    this.PicFit.getPos(e.X, e.Y, (double)this.PicFit.Width / 201, (double)this.PicFit.Height / 71, Color.Black);
                    return;
                }
                //else
                //{
                //    this.PicReal.getPos(e.X, e.Y, (double)this.PicReal.Width / 201, (double)this.PicReal.Height / 71, Color.Black, Pit_Map);
                //    this.PicFit.getPos(e.X, e.Y, (double)this.PicFit.Width / 201, (double)this.PicFit.Height / 71, Color.Black, Pit_Map);
                //    this.myPictureBox1.getPos(e.X, e.Y, (double)this.myPictureBox1.Width / (Pixels_width + 1)/*201*/, (double)this.myPictureBox1.Height / 71, Color.Black, Pit_Map);
                //    //显示当前鼠标指向的灰度值
                //    //label1.text储存
                //    byte image_x = (byte)(e.X / ((double)this.PicReal.Width / (Pixels_width + 2)));
                //    byte image_y = (byte)(e.Y / ((double)this.PicReal.Height / 72));
                //    byte grayValue = this.PicReal.MyBackImage.GetPixel(image_x, image_y).B;
                //    label1.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                //    label2.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                //    label3.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                //    return;
                //}
            }
            else if (MyGroup == MyResult.光电组)
            {
                if (Pit_Map == false)
                {
                    this.CCDPicFit.getPos(e.X, e.Y, Pit_Map);
                }
                else
                {
                    // this.CCDPicReal.getPos(e.X, e.Y, Pit_Map);
                    this.CCDPicFit.getPos(e.X, e.Y, Pit_Map);
                }
            }
        }
        //鼠标在原图区域内并移动触发事件
        private void PicReal_MouseMove(object sender, MouseEventArgs e)
        {
            //this.Cursor = MyCursor.Cross;
            if (this.Listening == true)
            {
                return;
            }
            if (MyGroup == MyResult.摄像头组)
            {
                if (Pit_Map == false)
                {
                    this.PicReal.getPos(e.X, e.Y, (double)this.PicReal.Width / 201, (double)this.PicReal.Height / 71, Color.Black);
                    return;
                }
                else
                {
                    this.PicReal.getPos(e.X, e.Y, (double)this.PicReal.Width / (Pixels_width + 1)/*201*/, (double)this.PicReal.Height / 71, Color.Black, Pit_Map);
                    this.PicFit.getPos(e.X, e.Y, (double)this.PicFit.Width / (Pixels_width + 1)/*201*/, (double)this.PicFit.Height / 71, Color.Black, Pit_Map);
                    this.myPictureBox1.getPos(e.X, e.Y, (double)this.myPictureBox1.Width / (Pixels_width + 1)/*201*/, (double)this.myPictureBox1.Height / 71, Color.Black, Pit_Map);
                    //显示当前鼠标指向的灰度值
                    //label1.text储存
                    byte image_x = (byte)(e.X / ((double)this.PicReal.Width / (Pixels_width + 2)));
                    byte image_y = (byte)(e.Y / ((double)this.PicReal.Height / 72));
                    byte grayValue = this.PicReal.MyBackImage.GetPixel(image_x, image_y).B;
                    label1.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                    label2.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                    label3.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                    //Color tmp_Color = new Color();//= this.BackColor;
                    ////if (PIT_FLAG == true)
                    ////{
                    ////    tmp_Color = Color.CadetBlue;
                    ////}
                    //PaintEventArgs e2 = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle); ;
                    //Graphics g = e2.Graphics;
                    //g.DrawLine(new Pen(tmp_Color, 1), new Point(e.X, 0), new Point(e.X, this.PicReal.Height));
                    //g.DrawLine(new Pen(tmp_Color, 1), new Point(0, e.Y), new Point(this.PicReal.Width, e.Y));
                    return;
                }
            }
            else if (MyGroup == MyResult.光电组)
            {
                if (Pit_Map == false)
                {
                    this.CCDPicReal.getPos(e.X, e.Y, Pit_Map);
                }
                else
                {
                    this.CCDPicReal.getPos(e.X, e.Y, Pit_Map);
                    //this.CCDPicFit.getPos(e.X, e.Y, Pit_Map);
                }
            }
        }
        private void myPictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //this.Cursor = MyCursor.Cross;
            if (this.Listening == true)
            {
                return;
            }
            if (MyGroup == MyResult.摄像头组)
            {
                if (Pit_Map == false)
                {
                    this.myPictureBox1.getPos(e.X, e.Y, (double)this.myPictureBox1.Width / 201, (double)this.myPictureBox1.Height / 71, Color.Black);
                    return;
                }
                else
                {
                    this.PicReal.getPos(e.X, e.Y, (double)this.PicReal.Width / (Pixels_width + 1)/*201*/, (double)this.PicReal.Height / 71, Color.Black, Pit_Map);
                    this.PicFit.getPos(e.X, e.Y, (double)this.PicFit.Width / (Pixels_width + 1)/*201*/, (double)this.PicFit.Height / 71, Color.Black, Pit_Map);
                    this.myPictureBox1.getPos(e.X, e.Y, (double)this.myPictureBox1.Width / (Pixels_width + 1)/*201*/, (double)this.myPictureBox1.Height / 71, Color.Black, Pit_Map);
                    //显示当前鼠标指向的灰度值
                    //label1.text储存
                    byte image_x = (byte)(e.X / ((double)this.myPictureBox1.Width / (Pixels_width + 2)));
                    byte image_y = (byte)(e.Y / ((double)this.myPictureBox1.Height / 72));
                    byte grayValue = this.myPictureBox1.MyBackImage.GetPixel(image_x, image_y).B;
                    label1.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                    label2.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                    label3.Text = "[" + (Pixels_width - image_x - 1) + "," + (Pixels_height - image_y - 1) + "] " + grayValue;
                    //Color tmp_Color = new Color();//= this.BackColor;
                    ////if (PIT_FLAG == true)
                    ////{
                    ////    tmp_Color = Color.CadetBlue;
                    ////}
                    //PaintEventArgs e2 = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle); ;
                    //Graphics g = e2.Graphics;
                    //g.DrawLine(new Pen(tmp_Color, 1), new Point(e.X, 0), new Point(e.X, this.PicReal.Height));
                    //g.DrawLine(new Pen(tmp_Color, 1), new Point(0, e.Y), new Point(this.PicReal.Width, e.Y));
                    return;
                }
            }
        }
        //鼠标离开拟图和原图区域触发事件
        private void Pic_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = MyCursor.Default;
        }
        //鼠标在主窗体内部并移动触发事件
        private void Nomal_MouseMouse(object sender, MouseEventArgs e)
        {
            //this.Cursor = MyCursor.Choose;
        }
        //鼠标离开主窗体触发事件
        private void Nomal_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = MyCursor.Default;
        }
        //单击菜单“示波工具”触发事件
        private void 示波工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close = true;

            this.Visible = false;//主窗体最小化到托盘

            notifyIcon1.Icon = this.Icon;

            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            MyUartDebug myuartdebug = new MyUartDebug();

            myuartdebug.ShowDialog();

            this.Visible = true;//主窗体恢复

            close = false;
        }
        //单击菜单“更新日志”触发事件
        private void 更新日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyTextBox MyText = new MyTextBox();
            string tmp_data = "";
            tmp_data += "by高源辰（十三届）  2018/4/17\r\n更新日志：\r\n";
            tmp_data += "1.增加串口传灰度图模式。\r\n";
            tmp_data += "by黄明  2114/12/17\r\n更新日志：\r\n";
            tmp_data += "1.界面优化。\r\n";
            tmp_data += "2.改进无线初始化逻辑。\r\n";
            tmp_data += "3.提高运行流畅度。\r\n";
            tmp_data += "4.增加最小二乘法中线回归功能。\r\n";
            tmp_data += "5.新增功能函数库“MyMath”\r\n";
            tmp_data += "6.新增曲线拟合工具（还没有完善）\r\n";
            tmp_data += "7.增加串口调试助手（内涵无线模块调试功能）\r\n";
            tmp_data += "8.增加光电组识别位置标记和中点标记。\r\n";
            tmp_data += "9.增加光电组拟图显示。\r\n";
            tmp_data += "10.修复了一些会发生错误的bug。\r\n";
            tmp_data += "11.更新了一下配置文件的读写方法，防止读写发生错误。\r\n";
            MyText.MyTextShow(tmp_data, false);
            MyText.Show();

        }
        //单击菜单“使用帮助”触发事件
        private void 使用帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyTextBox MyText = new MyTextBox();
            string tmp_data = "";
            tmp_data += "使用帮助：\r\n";
            tmp_data += "1.若程序跑死，重启一下程序（目前只能这么办）。\r\n";
            tmp_data += "2.若跳出。。。错误！！！请继续之类的警告，请检查一下’MyNrf.exe.config‘这个文件。\r\n";
            tmp_data += "3.快捷键ctrl+alt+space用户自定义停车，ctrl+alt+enter停车。（若无效则长按）\r\n";
            tmp_data += "若实在无法解决，请拷贝一下默认配置。" + "\r\n";
            tmp_data += "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "\r\n";
            tmp_data += "<configuration>" + "\r\n";
            tmp_data += "    <connectionStrings>" + "\r\n";
            tmp_data += "        <clear />" + "\r\n";
            tmp_data += "        <add name=\"配置文件\" connectionString=\"第十三届恩智浦智能车\" providerName=\"杭电智能车\" />" + "\r\n";
            tmp_data += "    </connectionStrings>" + "\r\n";
            tmp_data += "    <appSettings>" + "\r\n";
            tmp_data += "        <clear />" + "\r\n";
            tmp_data += "        <add key=\"幽魂\" value=\"参数选项|拟图颜色|控制固参|CCD颜色设置|\" />" + "\r\n";
            tmp_data += "        <add key=\"参数选项\" value=\"0\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图颜色\" value=\"TRUE\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图背景颜色\" value=\"White\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图左线开始颜色\" value=\"#FFFF80\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图左线颜色\" value=\"#80FF80\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图左线结束颜色\" value=\"#80FFFF\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图中线颜色\" value=\"#0080FF\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图右线开始颜色\" value=\"#FF80C0\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图右线颜色\" value=\"#FF80FF\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图右线结束颜色\" value=\"Red\" />" + "\r\n";
            tmp_data += "        <add key=\"拟图文字颜色\" value=\"Black\" />" + "\r\n";
            tmp_data += "        <add key=\"存图路径\" value=\"E:\\存图\\2014.12.6 18.25.38\" />" + "\r\n";
            tmp_data += "        <add key=\"保存路径\" value=\"E:\\存图\" />" + "\r\n";
            tmp_data += "        <add key=\"控制固参\" value=\"0\" />" + "\r\n";
            tmp_data += "        <add key=\"图形坐标轴辅助显示开关\" value=\"True\" />" + "\r\n";
            tmp_data += "        <add key=\"图像播放速度\" value=\"30\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD颜色设置\" value=\"TRUE\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD背景颜色\" value=\"White\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD1颜色\" value=\"#FF80FF\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD2颜色\" value=\"Aqua\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD3颜色\" value=\"#FF8000\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD中线颜色\" value=\"Red\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD左线颜色\" value=\"Blue\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD右线颜色\" value=\"Blue\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD标记颜色\" value=\"#408080\" />" + "\r\n";
            tmp_data += "        <add key=\"CCD文字颜色\" value=\"Black\" />" + "\r\n";
            tmp_data += "        <add key=\"光电组开启模式\" value=\"Local_CCD\" />" + "\r\n";
            tmp_data += "        <add key=\"摄像头组开启模式\" value=\"Local_Sim\" />" + "\r\n";
            tmp_data += "    </appSettings>" + "\r\n";
            tmp_data += "</configuration>" + "\r\n";

            MyText.MyTextShow(tmp_data, false);
            MyText.Show();
        }

        private string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }

                }
            }
            return result;
        }

        private string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }

            }
            return result;
        }

        private void 我的设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyTextBox MyText = new MyTextBox();
            string tmp_data = "设备信息：\r\n";

            tmp_data += "CPUID：" + identifier("Win32_Processor", "UniqueId") + "\r\n";  //CPUID   
            tmp_data += "处理器ID：" + identifier("Win32_Processor", "ProcessorId") + "\r\n";
            tmp_data += "处理器名称：" + identifier("Win32_Processor", "Name") + "\r\n";
            tmp_data += "处理器制造商：" + identifier("Win32_Processor", "Manufacturer") + "\r\n";
            tmp_data += "最大时钟频率：" + identifier("Win32_Processor", "MaxClockSpeed") + "\r\n";

            tmp_data += "BIOS制造商名称：" + identifier("Win32_BIOS", "Manufacturer") + "\r\n";  //CPUID   
            tmp_data += "：" + identifier("Win32_BIOS", "SMBIOSBIOSVersion") + "\r\n";
            tmp_data += "：" + identifier("Win32_BIOS", "IdentificationCode") + "\r\n";
            tmp_data += "BIOS序列号：" + identifier("Win32_BIOS", "SerialNumber") + "\r\n";
            tmp_data += "出厂日期：" + identifier("Win32_BIOS", "ReleaseDate") + "\r\n";
            tmp_data += "版本号：" + identifier("Win32_BIOS", "Version") + "\r\n";

            tmp_data += "硬盘名称：" + identifier("Win32_DiskDrive", "Name") + "\r\n";
            tmp_data += "硬盘模式：" + identifier("Win32_DiskDrive", "Model") + "\r\n";
            tmp_data += "硬盘制造商：" + identifier("Win32_DiskDrive", "Manufacturer") + "\r\n";
            tmp_data += "硬盘签名：" + identifier("Win32_DiskDrive", "Signature") + "\r\n";
            tmp_data += "硬盘扇区头：" + identifier("Win32_DiskDrive", "TotalHeads") + "\r\n";


            MyText.MyTextShow(tmp_data, false);
            MyText.Show();
        }

        private void 算法实现ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyTextBox MyText = new MyTextBox();
            string tmp_data = "";
            tmp_data += MyMath.FunC.Function_To_In();
            tmp_data += MyMath.FunC.Function_To_Sqrt();
            tmp_data += MyMath.FunC.Function_To_sin();
            tmp_data += MyMath.FunC.Function_To_cos();
            MyText.MyTextShow(tmp_data, false);
            MyText.Show();
        }
        //单击菜单“参数选项”触发事件
        private void 参数选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MYPar_Set myParSet = new MYPar_Set();
            myParSet.ShowDialog();

            mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
            myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
            myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
            for (int i = 0; i < ParValue.Count && i < Coefficient_Num; i++)
            {
                mypaint1.Item.Add(new WaveData(1, ParValue[i].Name, ParValue[i].WaveColor, 2));
                myDataInfo1.DataParValue.Add(new ClassConstParValue(ParValue[i].Name, 0, 4, 1, 0, true, ParValue[i].WaveColor));
                myDataInfo2.DataParValue.Add(new ClassConstParValue(ParValue[i].Name, 0, 4, 1, 0, true, ParValue[i].WaveColor));
                if (ParValue[i].WaveOn == true)
                {
                    mypaint1.Item[i].waveon = true;
                }
            }
        }
        //单击菜单“控制固参”触发事件
        private void 控制固参ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            My_Const_Par_Set Const_Par_Form = new My_Const_Par_Set();
            Const_Par_Form.ShowDialog();
            myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
            myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
            for (int i = 0; i < ConstValue.Count && i < Parameter_Num; i++)
            {
                myDataInfo1.DataConstValue.Add(new ClassConstParValue(ConstValue[i].Name, 0, 4, 1, 0, true, ConstValue[i].WaveColor));
                myDataInfo2.DataConstValue.Add(new ClassConstParValue(ConstValue[i].Name, 0, 4, 1, 0, true, ConstValue[i].WaveColor));
            }
        }
        //单击菜单“拟图颜色”触发事件
        private void 拟图颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyFitColor FitColorSet = new MyFitColor();
            FitColorSet.ShowDialog();
        }
        //单击菜单“曲线拟合”触发事件
        private void 曲线拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCurveFitting CurFit = new MyCurveFitting();
            CurFit.Show();
        }

        private void 遗传算法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyAGFrom myagfrom = new MyAGFrom();
            this.Visible = false;//主窗体最小化到托盘
            notifyIcon1.Icon = this.Icon;
            myagfrom.ShowDialog();
            this.Visible = true;//主窗体恢复
        }

        private void 截取数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file1 = new OpenFileDialog();
            file1.InitialDirectory = Open_File_Address;
            if (!File.Exists(Open_File_Address))
            {
                file1.Filter = Save_File_Address + "|*.hdu";
            }
            else
            {
                file1.Filter = Open_File_Address + "|*.hdu";
            }
            if (file1.ShowDialog() == DialogResult.OK)
            {
                Open_File_Address = file1.FileName;
                Open_File_Address = Open_File_Address.Remove(Open_File_Address.LastIndexOf("\\"));
                Copy_Local_Data_Num = ProjectReadInt("工程参数", "场数量", file1.FileName);
                Copy_Coefficient_Num = (ushort)ProjectReadInt("控制参数", "参数个数", file1.FileName);
                Copy_Parameter_Num = (ushort)ProjectReadInt("固定参数", "参数个数", file1.FileName);
                MyCopy mycopy = new MyCopy();

                for (int i = 0; i < Copy_Coefficient_Num; i++)
                {
                    DataCopyList.Add(new CopyDataInfo(ProjectReadString("控制参数信息", "参数名称" + i.ToString(), file1.FileName)));
                }
                for (int i = 0; i < Copy_Parameter_Num; i++)
                {
                    DataCopyList.Add(new CopyDataInfo(ProjectReadString("控制固参信息", "参数名称" + i.ToString(), file1.FileName)));
                }
                mycopy.data = DataCopyList;
                mycopy.ShowDialog();
                DataCopyList = mycopy.data;
                if (backgroundWorker4.IsBusy == false)
                {
                    backgroundWorker4.RunWorkerAsync();
                }
            }

        }
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            string ThisPath = string.Empty;
            string tmpdata = string.Empty;
            for (int i = 0; i < Copy_Local_Data_Num; i++)
            {
                ThisPath = Open_File_Address + "\\数据\\" + (i + 1).ToString() + ".txt";
                if (File.Exists(ThisPath))
                {
                    for (int j = 0; j < Copy_Coefficient_Num; j++)
                    {
                        if (DataCopyList[j].ChooseOn == true)
                        {
                            tmpdata += ProjectReadIfloat("反馈数据", j.ToString(), ThisPath).ToString();
                            tmpdata += "=";
                        }
                    }

                    for (int j = 0; j < Copy_Parameter_Num; j++)
                    {
                        if (DataCopyList[j + Copy_Coefficient_Num].ChooseOn == true)
                        {
                            tmpdata += ProjectReadIfloat("控制固参", j.ToString(), ThisPath).ToString();
                            tmpdata += "=";
                        }
                    }
                    if (tmpdata != null || tmpdata != string.Empty)
                    {
                        tmpdata = tmpdata.Remove(tmpdata.LastIndexOf("="));
                        tmpdata += "\r\n";
                    }

                }
                this.backgroundWorker4.ReportProgress((i + 1) * 10000 / Copy_Local_Data_Num, 0);
            }
            MyClipboard = tmpdata;
        }
        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.richTextBox1.Text = "复制数据进度：" + ((double)e.ProgressPercentage / 100).ToString() + "%";
        }
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Clipboard.SetDataObject(MyClipboard);
            MyClipboard = string.Empty;
            this.richTextBox1.Text = "数据已经复制到剪贴板";
        }
        //关闭主窗体时触发事件  使用操作系统来结束进程，而不用程序正常退出逻辑，从而使主窗体快速推出
        private void Nrf_FormClosed(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 200, AW_SLIDE + AW_HIDE + AW_CENTER);
            Process.GetCurrentProcess().Kill();
            //Environment.Exit(Environment.ExitCode);
            this.Dispose();
        }
        //关闭主窗体
        private void OnFormClosing()
        {
            //注销热键
            UnregisterHotKey(this.Handle, 225);
            UnregisterHotKey(this.Handle, 226);
            UnregisterHotKey(this.Handle, 227);
            if (serialPort1.IsOpen == true)
            {

                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();

                this.Close();
            }
            else
            {

                this.Close();

            }
        }
        //鼠标单击最小化按钮时触发事件
        private void myMin1_Click(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                AnimateWindow(this.Handle, 200, AW_VER_NEGATIVE + AW_CENTER);
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                AnimateWindow(this.Handle, 200, AW_VER_POSITIVE + AW_HIDE);
                this.WindowState = FormWindowState.Minimized;
            }
            this.Visible = true;
        }
        //鼠标单击关闭按钮时触发事件
        private void myButton1_Click(object sender, MouseEventArgs e)
        {
            sendWaiterstop = true;//关闭线程
            if (e.Button == MouseButtons.Left)
            {
                OnFormClosing();
            }
        }
        //读取配置文件
        private void XmlFileRead() //读取配置文件
        {
            //先匹配参数选项
            myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
            myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
            myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
            myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
            mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
            ParValue.RemoveRange(0, ParValue.Count);
            ConstValue.RemoveRange(0, ConstValue.Count);
            string ParNum = My_Xml.GetValue("参数选项");

            if (ParNum != "" || ParNum != "0")
            {
                for (int i = 1; i <= int.Parse(ParNum); i++)
                {
                    ParValue.Add(new ClassParValue(My_Xml.GetValue("参数" + i.ToString() + "的名称"), int.Parse(My_Xml.GetValue("参数" + i.ToString() + "的长度")), int.Parse(My_Xml.GetValue("参数" + i.ToString() + "的类型")), System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("参数" + i.ToString() + "的颜色")), int.Parse(My_Xml.GetValue("参数" + i.ToString() + "的缩放比例")), bool.Parse(My_Xml.GetValue("参数" + i.ToString() + "的波形开关"))));
                    myDataInfo1.DataParValue.Add(new ClassConstParValue(ParValue[i - 1].Name, 0, 4, 1, 0, false, ParValue[i - 1].WaveColor));
                    myDataInfo2.DataParValue.Add(new ClassConstParValue(ParValue[i - 1].Name, 0, 4, 1, 0, false, ParValue[i - 1].WaveColor));
                }
            }
            string ConstNum = My_Xml.GetValue("控制固参");
            if (ConstNum != "" || ConstNum != "0")
            {
                for (int i = 1; i <= int.Parse(ConstNum); i++)
                {
                    ConstValue.Add(new ClassParValue(My_Xml.GetValue("固参" + i.ToString() + "的名称"), int.Parse(My_Xml.GetValue("固参" + i.ToString() + "的长度")), int.Parse(My_Xml.GetValue("固参" + i.ToString() + "的类型")), System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("固参" + i.ToString() + "的颜色")), 1, false));
                    myDataInfo1.DataConstValue.Add(new ClassConstParValue(ConstValue[i - 1].Name, 0, 4, 1, 0, false, ConstValue[i - 1].WaveColor));
                    myDataInfo2.DataConstValue.Add(new ClassConstParValue(ConstValue[i - 1].Name, 0, 4, 1, 0, false, ConstValue[i - 1].WaveColor));
                }
            }
            string FitColorSet = My_Xml.GetValue("拟图颜色");
            if (FitColorSet == "TRUE")
            {
                FitColor.Back = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图背景颜色"));
                FitColor.LS = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图左线开始颜色"));
                FitColor.LL = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图左线颜色"));
                FitColor.LE = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图左线结束颜色"));
                FitColor.CL = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图中线颜色"));
                FitColor.RS = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图右线开始颜色"));
                FitColor.RL = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图右线颜色"));
                FitColor.RE = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图右线结束颜色"));
                FitColor.Text = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("拟图文字颜色"));
            }
            else
            {
                FitColor.Back = Color.White;
                FitColor.LS = Color.Yellow;
                FitColor.LL = Color.Blue;
                FitColor.LE = Color.Brown;
                FitColor.CL = Color.Red;
                FitColor.RS = Color.Yellow;
                FitColor.RL = Color.Blue;
                FitColor.RE = Color.Brown;
                FitColor.Text = Color.Black;
            }
            string CCDColorSet = My_Xml.GetValue("CCD颜色设置");
            if (CCDColorSet == "TRUE")
            {
                CCDColor.Back = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD背景颜色"));
                CCDColor.CCD1 = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD1颜色"));
                CCDColor.CCD2 = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD2颜色"));
                CCDColor.CCD3 = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD3颜色"));
                CCDColor.CCDC = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD中线颜色"));
                CCDColor.CCDL = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD左线颜色"));
                CCDColor.CCDR = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD右线颜色"));
                CCDColor.CCDP = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD标记颜色"));
                CCDColor.Text = System.Drawing.ColorTranslator.FromHtml(My_Xml.GetValue("CCD文字颜色"));
            }
            else
            {
                CCDColor.Back = Color.White;
                CCDColor.CCD1 = Color.Blue;
                CCDColor.CCD2 = Color.Yellow;
                CCDColor.CCD3 = Color.Blue;
                CCDColor.CCDC = Color.Blue;
                CCDColor.CCDL = Color.Blue;
                CCDColor.CCDR = Color.Blue;
                CCDColor.CCDP = Color.Blue;
                CCDColor.Text = Color.Black;
            }

            Open_File_Address = My_Xml.GetValue("存图路径");
            Save_File_Address = My_Xml.GetValue("保存路径");
            for (int i = 0; i < ParValue.Count; i++)
            {
                mypaint1.Item.Add(new WaveData(1, ParValue[i].Name, ParValue[i].WaveColor, 2));
            }
            Pit_Map = bool.Parse(My_Xml.GetValue("图形坐标轴辅助显示开关"));

            Pit_Play_Speed = int.Parse(My_Xml.GetValue("图像播放速度"));
            timePlay.Interval = Pit_Play_Speed;
            timerPlay2.Interval = Pit_Play_Speed;
            if (MyGroup == MyResult.摄像头组)
            {
                mymode = Getmode.getmode(My_Xml.GetValue("摄像头组开启模式"));
            }
            if (MyGroup == MyResult.光电组)
            {
                mymode = Getmode.getmode(My_Xml.GetValue("光电组开启模式"));
            }
            if (My_Xml.GetValue("无线频道") == "")
            {
                NrfChannel = "频道:001";
            }
            else
            {
                NrfChannel = My_Xml.GetValue("无线频道");
            }
            string wavedatalength = My_Xml.GetValue("波形数据长度");
            if (wavedatalength != "")
            {
                WaveDataLength = decimal.Parse(wavedatalength);
            }
            PolyKey = bool.Parse(My_Xml.GetValue("中线回归开关"));
            string polyfitnum = My_Xml.GetValue("中线回归次数");
            if (polyfitnum != "")
            {
                Polyfit = int.Parse(polyfitnum);
            }
            LogKey = bool.Parse(My_Xml.GetValue("打脚开关"));
            string tmplognum = My_Xml.GetValue("打脚行");
            if (tmplognum != "")
            {
                LogLine = int.Parse(tmplognum);
            }
            wavexlable.Value = My_Xml.GetValue("横坐标名称");
            waveylable.Value = My_Xml.GetValue("纵坐标名称");
            wavemode.Value = bool.Parse(My_Xml.GetValue("波形从左向右开关"));
        }
        //写入配置文件
        static public void XmlFileWrite(XmlInfo XmlData, bool Flag) //写入配置文件
        {
            My_Xml.WriteXml(XmlData, Flag);
        }
        //向窗体文本区域输出字符串
        private void output(string Data)
        {
            try
            {
                richTextBox1.AppendText(Data);
            }
            catch
            {
                return;
            }
        }
        //检测串口按钮
        private void button1_Click(object sender, EventArgs e)
        {
            int i = 1;
            richTextBox1.Text = "";
            if (button1.Text == "刷新串口")
            {
                close = true;
                try
                {
                    serialPort1.DiscardInBuffer();
                }
                catch
                {
                    MessageBox.Show("清空缓存异常");
                    UART_OPEN = false;
                    return;
                }
                richTextBox1.Clear();
                //if (serialPort1.IsOpen && (mymode == MyMode.Nrf_Pic || mymode == MyMode.Nrf_Par || mymode == MyMode.Nrf_CCD))
                //{
                //    byte[] buff = new byte[3] {(byte)CmdMode.InitParLen,
                //       byte.Parse(NrfChannel.Substring(NrfChannel.IndexOf(":")+1, 3)),
                //       (byte)mymode
                //     };
                //    serialPort1.Write(buff, 0, buff.Length);
                //    richTextBox1.Text = "无线重置完毕……" + "\r\n" + "重置信息……" + mymode.ToString()
                //        + "\r\n" + "无线频道：" + NrfChannel + "   " + "(无线频道尽量和别人的不一样，否则会造成干扰！)";
                //    serialPort1.Write(new byte[1] { (byte)CmdMode.Write_Data }, 0, 1);
                //    //timerPlay2.Start();
                //    //waitcmd = true;
                //}
                close = false;
            }
            else
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    serialPort1.Dispose();
                }
                else
                {
                    if (comboBox2.Text != "没有检测到串口")
                    {
                        serialPort1.PortName = "COM" + i.ToString();
                        serialPort1.BaudRate = Int32.Parse(comboBox1.Text);
                        try
                        {
                            serialPort1.Open();
                        }
                        catch { }
                    }
                    while (serialPort1.IsOpen == false)
                    {
                        serialPort1.PortName = "COM" + i.ToString();
                        serialPort1.BaudRate = Int32.Parse(comboBox1.Text);
                        i++;
                        if (i > 20)
                        {
                            comboBox2.Text = "没有检测到串口";
                            UART_OPEN = false;
                            return;
                        }
                        try
                        {
                            serialPort1.Open();
                        }
                        catch
                        {
                            continue;
                        }

                    }

                }
                comboBox2.Text = "检测到" + serialPort1.PortName;
                this.comboBox1.Text = serialPort1.BaudRate.ToString();


                serialPort1.Close();
                serialPort1.ReceivedBytesThreshold = 1;//设置串口触发速度
                //   serialPort1.ReadBufferSize = 50000;
                button2.Text = "开始读取";
                UART_OPEN = true;
                this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);

            }
        }
        //打开/关闭串口按钮
        private void button2_Click(object sender, EventArgs e)
        {
            if (UART_OPEN == true)
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.BaudRate = Int32.Parse(comboBox1.Text);
                    try
                    {
                        comboBox1.Enabled = false;
                        comboBox2.Enabled = false;
                        button2.Text = "关闭读取";
                        button1.Text = "刷新串口";
                        output("开始接收数据：" + "\r\n");
                        清空缓存.Enabled = false;
                        serialPort1.Open();
                        if (serialPort1.IsOpen && (mymode == MyMode.Nrf_Pic || mymode == MyMode.Nrf_Par || mymode == MyMode.Nrf_CCD))//初始化无线
                        {
                            byte[] buff = new byte[3] {(byte)CmdMode.InitParLen,
                                 byte.Parse(NrfChannel.Substring(NrfChannel.IndexOf(":")+1, 3)),
                                 (byte)mymode
                                };
                            serialPort1.Write(buff, 0, buff.Length);
                            richTextBox1.Text = "无线重置完毕……" + "\r\n" + "重置信息……" + mymode.ToString()
                                + "\r\n" + "无线频道：" + NrfChannel + "   " + "(无线频道尽量和别人的不一样，否则会造成干扰！)";
                            //serialPort1.Write(new byte[1] { (byte)CmdMode.Write_Data }, 0, 1);
                            //timerPlay2.Start();
                            //waitcmd = true;
                        }

                        if (mymode == MyMode.Nrf_Pic || mymode == MyMode.Nrf_CCD)
                        {
                            if (serialPort1.IsOpen == true)
                            {
                                //waitcmd = true;
                                serialPort1.Write(new byte[1] { (byte)CmdMode.Write_Data }, 0, 1);
                            }
                        }
                        else if (mymode == MyMode.Nrf_Par)
                        {
                            if (serialPort1.IsOpen == true)
                            {
                                //waitcmd = true;
                                serialPort1.Write(new byte[1] { (byte)CmdMode.Write_Data }, 0, 1);
                            }
                        }
                    }
                    catch
                    {

                        MessageBox.Show(serialPort1.PortName + "已经断开");
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;
                        button2.Text = "开始读取";
                        button1.Text = "重置串口";
                        清空缓存.Enabled = true;
                        UART_OPEN = false;
                        return;
                    }
                }
                else
                {
                    close = true;
                    while (Listening == true)
                    {
                        Application.DoEvents();
                    }
                    serialPort1.Close();
                    清空缓存.Enabled = true;
                    button2.Text = "开始读取";
                    button1.Text = "检测串口";
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    close = false;
                }
            }
            else
            {
                清空缓存.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                button2.Text = "打开串口";
                button1.Text = "检测串口";
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            close = true;
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Write(new byte[1] { (byte)CmdMode.RstChip }, 0, 1);
            }
            //waitcmd = true;

            if (serialPort1.IsOpen && (mymode == MyMode.Nrf_Pic || mymode == MyMode.Nrf_Par || mymode == MyMode.Nrf_CCD))//初始化无线
            {
                byte[] buff = new byte[3] {(byte)CmdMode.InitParLen,
                       byte.Parse(NrfChannel.Substring(NrfChannel.IndexOf(":")+1, 3)),
                       (byte)mymode
                     };
                serialPort1.Write(buff, 0, buff.Length);
                richTextBox1.Text = "无线重置完毕……" + "\r\n" + "重置信息……" + mymode.ToString()
                    + "\r\n" + "无线频道：" + NrfChannel + "   " + "(无线频道尽量和别人的不一样，否则会造成干扰！)";
                serialPort1.Write(new byte[1] { (byte)CmdMode.Write_Data }, 0, 1);
                //timerPlay2.Start();
                //waitcmd = true;
            }

            if (mymode == MyMode.Nrf_Pic || mymode == MyMode.Nrf_CCD)
            {
                if (serialPort1.IsOpen == true)
                {
                    //waitcmd = true;
                    serialPort1.Write(new byte[1] { (byte)CmdMode.Write_Data }, 0, 1);
                }
            }
            else if (mymode == MyMode.Nrf_Par)
            {
                if (serialPort1.IsOpen == true)
                {
                    //waitcmd = true;
                    serialPort1.Write(new byte[1] { (byte)CmdMode.Write_Par }, 0, 1);
                }
            }
            close = false;
        }

        //单击菜单“无线图像模式”触发事件 模式转换为“无线图像模式”
        private void 无线图像模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Nrf_Pic;
            Mode_Config();

        }
        //单击菜单“无线参数模式”触发事件 模式转换为“无线参数模式”
        private void 无线参数模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Nrf_Par;
            Mode_Config();
            //   MessageBox.Show(mymode.ToString());
        }
        //单击菜单“图像仿真模式”触发事件 模式转换为“图像仿真模式”
        private void 图像仿真模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Local_Debug;
            Mode_Config();

        }
        //单击菜单“本地模式”触发事件 模式转换为“本地模式”
        private void 本地模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }

            mymode = MyMode.Local_Sim;
            Mode_Config();

        }
        //单击菜单“串口图像模式”触发事件 模式转换为“串口图像模式”
        private void 串口图像模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Uart_Pic;
            Mode_Config();

        }
        //单击菜单“串口灰度图像模式”触发事件 模式转换为“串口灰度图像模式”
        private void 串口灰度图像模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Uart_PicGray;
            Mode_Config();
        }

        //单击菜单“串口参数模式”触发事件 模式转换为“串口参数模式”
        private void 串口参数模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Uart_Par;
            Mode_Config();


        }
        //单击菜单“光电串口模式”触发事件 模式转换为“光电串口模式”
        private void 光电串口模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Uart_CCD;
            Mode_Config();
        }
        //单击菜单“光电图像模式”触发事件 模式转换为“光电图像模式”
        private void 光电图像模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Local_CCD;
            Mode_Config();
        }

        private void 光电无线模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Nrf_CCD;
            Mode_Config();
        }

        private void 光电仿真模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                while (Listening == true)
                {
                    Application.DoEvents();
                }
                serialPort1.Close();
            }
            mymode = MyMode.Local_CCD_Debug;
            Mode_Config();
        }

        //单击菜单“配置”触发事件 模式转换为“配置”
        private void 配置ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MySetup setup = new MySetup();
            setup.ShowDialog();
            mypaint1.Data_Length = (int)(WaveDataLength);
            timePlay.Interval = Pit_Play_Speed;
        }
        //打开按钮
        private void 打开_Click(object sender, EventArgs e)
        {
            timePlay.Enabled = false;
            OpenFileDialog file1 = new OpenFileDialog();
            file1.InitialDirectory = Open_File_Address;
            if (!File.Exists(Open_File_Address))
            {
                file1.Filter = Save_File_Address + "|*.hdu";
            }
            else
            {
                file1.Filter = Open_File_Address + "|*.hdu";
            }

            if (file1.ShowDialog() == DialogResult.OK)
            {
                Open_File_Address = file1.FileName;//记录路径，方便下次打开
                Open_File_Address = Open_File_Address.Remove(Open_File_Address.LastIndexOf("\\"));
                XmlFileWrite(new XmlInfo("存图路径", Open_File_Address), false);//写入配置文件
                if (MyGroup == MyResult.光电组)
                {
                    if (ProjectReadString("工程参数", "组别", file1.FileName) == "光电组")
                    {
                        CCD_height = (ushort)ProjectReadInt("数据长度参数", "图像高度", file1.FileName);
                        CCD_width = (ushort)ProjectReadInt("数据长度参数", "图像宽度", file1.FileName);
                        CCDPicReal.lines = CCD_height;
                    }
                    else
                    {
                        MessageBox.Show("警告！组别不对应，请确认打开文件是否为光电组");
                    }
                }
                if (MyGroup == MyResult.摄像头组)
                {
                    if (ProjectReadString("工程参数", "组别", file1.FileName) == "摄像头组")
                    {
                        Pixels_height = (ushort)ProjectReadInt("数据长度参数", "图像高度", file1.FileName);
                        Pixels_width = (ushort)ProjectReadInt("数据长度参数", "图像宽度", file1.FileName);
                    }
                    else
                    {
                        MessageBox.Show("警告！组别不对应，请确认打开文件是否为摄像头组");
                    }
                    if (ProjectReadString("工程参数", "存入模式", file1.FileName) == "Uart_Par")
                    {
                        Read_Pic = false;
                    }
                    else
                    {
                        Read_Pic = true;
                    }
                }
                Local_Data_Num = ProjectReadInt("工程参数", "场数量", file1.FileName);
                Coefficient_Num = (ushort)ProjectReadInt("控制参数", "参数个数", file1.FileName);
                Parameter_Num = (ushort)ProjectReadInt("固定参数", "参数个数", file1.FileName);
                myDataInfo1.DataParValue.RemoveRange(0, myDataInfo1.DataParValue.Count);
                myDataInfo2.DataParValue.RemoveRange(0, myDataInfo2.DataParValue.Count);
                myDataInfo1.DataConstValue.RemoveRange(0, myDataInfo1.DataConstValue.Count);
                myDataInfo2.DataConstValue.RemoveRange(0, myDataInfo2.DataConstValue.Count);
                mypaint1.Item.RemoveRange(0, mypaint1.Item.Count);
                ParValue.RemoveRange(0, ParValue.Count);
                ConstValue.RemoveRange(0, ConstValue.Count);

                string tmp_name = string.Empty;
                Color tmp_Color = new Color();

                for (int i = 0; i < Coefficient_Num; i++)
                {
                    tmp_name = ProjectReadString("控制参数信息", "参数名称" + i.ToString(), file1.FileName);//读取信息
                    tmp_Color = System.Drawing.ColorTranslator.FromHtml(ProjectReadString("控制参数信息", "参数颜色" + i.ToString(), file1.FileName));//读取信息
                    myDataInfo1.DataParValue.Add(new ClassConstParValue(tmp_name, 0, 4, 1, 0, true, tmp_Color));
                    myDataInfo2.DataParValue.Add(new ClassConstParValue(tmp_name, 0, 4, 1, 0, true, tmp_Color));
                    ParValue.Add(new ClassParValue(tmp_name, 1, 1, tmp_Color, 3, true));
                    mypaint1.Item.Add(new WaveData(1, myDataInfo1.DataParValue[i].Name, myDataInfo1.DataParValue[i].color, 2));
                }


                for (int i = 0; i < Parameter_Num; i++)
                {
                    tmp_name = ProjectReadString("控制固参信息", "参数名称" + i.ToString(), file1.FileName);//读取信息
                    tmp_Color = System.Drawing.ColorTranslator.FromHtml(ProjectReadString("控制固参信息", "参数颜色" + i.ToString(), file1.FileName));//读取信息
                    myDataInfo1.DataConstValue.Add(new ClassConstParValue(tmp_name, 0, 4, 1, 0, true, tmp_Color));
                    myDataInfo2.DataConstValue.Add(new ClassConstParValue(tmp_name, 0, 4, 1, 0, true, tmp_Color));
                    ConstValue.Add(new ClassParValue(tmp_name, 1, 1, tmp_Color, 3, false));
                }
            }
            else
            {
                MessageBox.Show("文件打开失败");
                return;
            }

            trackBar1.Maximum = Local_Data_Num;
            trackBar1.Minimum = 1;
            trackBar1.TickFrequency = Local_Data_Num / 10;
            trackBar1.Value = 1;
            trackBar1.Enabled = true;
            if (mymode == MyMode.Local_Debug)
            {
                if (MySmartProcess.Lcr.Count < Pixels_height)
                {
                    for (int i = MySmartProcess.Lcr.Count; i < Pixels_height; i++)
                        MySmartProcess.Lcr.Add(new LCR(255, 255, 255, 255, 255, 255, 255));
                }
            }
            if (mymode == MyMode.Local_CCD_Debug)
            {
                if (MyCCDSmartProcess.Lcr.Count <= CCD_height)
                {
                    for (int i = MyCCDSmartProcess.Lcr.Count; i <= CCD_height; i++)
                        MyCCDSmartProcess.Lcr.Add(new CCDPOS(255, 255, 255));
                }
            }
            OPENFILEFLAG = true;
            MyShow();
        }
        //上一场按钮
        private void 上一场_Click(object sender, EventArgs e)
        {
            if (OPENFILEFLAG == false)
            {
                return;
            }
            if (trackBar1.Value > 1)
            {
                trackBar1.Value--;
            }
            element_show();
        }
        //下一场按钮
        private void 下一场_Click(object sender, EventArgs e)
        {
            if (OPENFILEFLAG == false)
            {
                return;
            }
            if (trackBar1.Value < Local_Data_Num)
            {
                trackBar1.Value++;
            }
            element_show();
        }
        //播放按钮
        private void 播放_Click(object sender, EventArgs e)
        {
            if (OPENFILEFLAG == false)
            {
                return;
            }
            if (播放.Text == "播放")
            {
                //timerPlay2.Stop();
                timePlay.Enabled = true;
                //timerPlay2.Enabled = true;
                播放.Text = "暂停";
                this.上一场.Enabled = false;
                this.下一场.Enabled = false;
            }
            else if (播放.Text == "暂停")
            {
                timePlay.Enabled = false;
                //timerPlay2.Enabled = false;
                播放.Text = "播放";
                this.上一场.Enabled = true;
                this.下一场.Enabled = true;
            }
            element_show();
        }
        //存储开关按钮
        private void 存储开关_Click(object sender, EventArgs e)
        {
            if (Save_Flag == false && (mymode == MyMode.Nrf_Pic || mymode == MyMode.Uart_Pic || mymode == MyMode.Uart_PicGray || mymode == MyMode.Uart_CCD || mymode == MyMode.Nrf_CCD || mymode == MyMode.Nrf_Par || mymode == MyMode.Uart_Par))
            {
                if (DATA_Save.Count > 1) //最后一个数据不完整 
                {
                    DATA_Save.RemoveRange(DATA_Save.Count - 1, 1);
                }
                Save_Flag = true;
                存储开关.Text = "关闭存储";
            }
            else if (Save_Flag == true)
            {
                Save_Flag = false;
                存储开关.Text = "写入缓存";
            }



        }
        //写入数据按钮
        private void 写入数据_Click(object sender, EventArgs e)
        {
            while (Listening == true)//先关闭串口
            {
                Application.DoEvents();
            }
            if (!(DATA_Save.Count > 0))
            {

                MessageBox.Show("警告!缓存区没有任何数据");
                return;
            }
            if (Save_Flag == true)
            {
                Save_Flag = false;
                存储开关.Text = "写入缓存";

            }
            serialPort1.Close();
            button2.Text = "开始读取";
            button1.Text = "检测串口";
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            if (Save_File_Address == "") //如果路径错误，那就选择路径
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                folderBrowserDialog1.Description = "请选择输出目录";
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    Save_File_Address = folderBrowserDialog1.SelectedPath;
                    XmlFileWrite(new XmlInfo("保存路径", Save_File_Address), false);
                }
            }
            //这里开始存储数据
            //创建文件
            Child_Address = Save_File_Address + "\\" + DateTime.Now.ToString().Replace(@"/", ".").Replace(":", ".");//创建文件
            if (mymode == MyMode.Uart_Pic || mymode == MyMode.Nrf_Pic || mymode == MyMode.Uart_PicGray)
            {
                System.IO.Directory.CreateDirectory(Child_Address + "\\原图\\");//创建文件
                System.IO.Directory.CreateDirectory(Child_Address + "\\拟图\\");//创建文件
                System.IO.Directory.CreateDirectory(Child_Address + "\\数据\\");//创建文件
                Child_Address = Child_Address + "\\Project.hdu";//创建文件
                ProjectWrite("工程参数", "组别", MyGroup.ToString(), Child_Address);
                ProjectWrite("工程参数", "存入模式", mymode.ToString(), Child_Address);
                ProjectWrite("工程参数", "场数量", DATA_Save.Count.ToString(), Child_Address);
                WritePrivateProfileString("数据长度参数", "图像高度", Pixels_height.ToString(), Child_Address);//写入信息
                WritePrivateProfileString("数据长度参数", "图像宽度", Pixels_width.ToString(), Child_Address);//写入信息
            }
            else if (mymode == MyMode.Uart_CCD || mymode == MyMode.Nrf_CCD)
            {
                System.IO.Directory.CreateDirectory(Child_Address + "\\数据\\");//创建文件
                Child_Address = Child_Address + "\\Project.hdu";//创建文件
                ProjectWrite("工程参数", "组别", MyGroup.ToString(), Child_Address);
                ProjectWrite("工程参数", "存入模式", mymode.ToString(), Child_Address);
                ProjectWrite("工程参数", "场数量", DATA_Save.Count.ToString(), Child_Address);
                WritePrivateProfileString("数据长度参数", "图像高度", CCD_height.ToString(), Child_Address);//写入信息
                WritePrivateProfileString("数据长度参数", "图像宽度", CCD_width.ToString(), Child_Address);//写入信息
            }
            else if (mymode == MyMode.Uart_Par || mymode == MyMode.Nrf_Par)
            {
                System.IO.Directory.CreateDirectory(Child_Address + "\\数据\\");//创建文件
                Child_Address = Child_Address + "\\Project.hdu";//创建文件
                ProjectWrite("工程参数", "组别", MyGroup.ToString(), Child_Address);
                ProjectWrite("工程参数", "存入模式", mymode.ToString(), Child_Address);
                ProjectWrite("工程参数", "场数量", DATA_Save.Count.ToString(), Child_Address);
                WritePrivateProfileString("数据长度参数", "图像高度", (0).ToString(), Child_Address);//写入信息
                WritePrivateProfileString("数据长度参数", "图像宽度", (0).ToString(), Child_Address);//写入信息
            }
            else
            {
                MessageBox.Show("模式错误");
                return;
            }
            WritePrivateProfileString("控制参数", "参数个数", DATA_Save[0].ParValue.Count.ToString(), Child_Address);//写入信息
            WritePrivateProfileString("固定参数", "参数个数", DATA_Save[0].CoeValue.Count.ToString(), Child_Address);//写入信息
            for (int i = 0; i < DATA_Save[0].ParValue.Count; i++)
            {
                WritePrivateProfileString("控制参数信息", "参数名称" + i.ToString(), ParValue[i].Name, Child_Address);//写入信息
                WritePrivateProfileString("控制参数信息", "参数颜色" + i.ToString(), System.Drawing.ColorTranslator.ToHtml(ParValue[i].WaveColor), Child_Address);//写入信息
            }
            for (int i = 0; i < DATA_Save[0].CoeValue.Count; i++)
            {
                WritePrivateProfileString("控制固参信息", "参数名称" + i.ToString(), ConstValue[i].Name, Child_Address);//写入信息
                WritePrivateProfileString("控制固参信息", "参数颜色" + i.ToString(), System.Drawing.ColorTranslator.ToHtml(ConstValue[i].WaveColor), Child_Address);//写入信息
            }
            ThreadWrite = new Thread(new ThreadStart(IniCfgSave));//开始存储数据
            ThreadWrite.IsBackground = true;//后台执行
            ThreadWrite.Start();//开始执行

        }
        //清空缓存按钮
        private void 清空缓存_Click(object sender, EventArgs e)
        {
            if (Save_Flag == true)
            {
                Save_Flag = false;
                存储开关.Text = "写入缓存";
            }
            DialogResult result = MessageBox.Show("确定要删除缓存区数据？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (!(DATA_Save.Count > 0))
                {
                    return;
                }
                DATA_Save.RemoveRange(0, DATA_Save.Count);
            }
            if (result == DialogResult.No)
            {
                return;
            }

        }
        //定时器1触发事件
        private void timePlay_Tick(object sender, EventArgs e)
        {

            if (trackBar1.Value < Local_Data_Num)
            {
                //timePlay.Stop();
                //timerPlay2.Start();
                下一场_Click(this, new EventArgs());
            }
            else
            {
                timePlay.Enabled = false;
                播放.Text = "播放";
                this.上一场.Enabled = true;
                this.下一场.Enabled = true;
            }

        }
        //定时器2触发事件
        private void timerPlay2_Tick(object sender, EventArgs e)
        {
            if (timerPlay2flag == false)
            {
                timerPlay2flag = true;
            }
            else
            {
                byte[] Buff;
                if (mymode == MyMode.Nrf_Pic || mymode == MyMode.Nrf_CCD)
                {
                    Buff = new byte[] { (byte)CmdMode.Write_Data };
                    Monitor.Enter(this.requests);
                    requests.AddRange(Buff);
                    Monitor.Exit(this.requests);
                    sendWaiter.Set();
                }
                else if (mymode == MyMode.Nrf_Par)
                {
                    Buff = new byte[] { (byte)CmdMode.Write_Data };
                    Monitor.Enter(this.requests);
                    requests.AddRange(Buff);
                    Monitor.Exit(this.requests);
                    sendWaiter.Set();
                }
                timerPlay2flag = false;
                timerPlay2.Enabled = false;
            }
        }
        //进度条进度变化触发事件
        private void trackBar1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.backgroundWorker2.IsBusy == false)
            {
                richTextBox1.MyText = trackBar1.Value.ToString() + "/" + trackBar1.Maximum.ToString() + "\r\n" + "内存消耗：" + GC.GetTotalMemory(true).ToString();
                trackbarvalue = trackBar1.Value;
                this.backgroundWorker2.RunWorkerAsync();
            }
            //element.Text = "-十字-" + SmartProcess.Element_Cross + "-坡道-" + SmartProcess.Ramp_Flag + "-环阶段-" + SmartProcess.Ring_State_Flag + "-起跑线-" + SmartProcess.Starting_Line_Flag + "-障碍-" + SmartProcess.L_Obstacle_Flag + SmartProcess.R_Obstacle_Flag;
            //MyShow();
        }

        private void myDataInfo1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (myDataInfo1.glfouse == true && myDataInfo1.constdatatabnum != 0)
                    {
                        if (serialPort1.IsOpen == true)
                        {
                            munkeyin = false;
                            minpoint = false;
                            minpointcnt = 0;
                            DataChange tmpchange = new DataChange();
                            byte[] tmpdata = tmpchange.floattou32byte(myDataInfo1.SendData);
                            byte[] SendData = new byte[8];
                            SendData[0] = tmpdata[3];
                            SendData[1] = tmpdata[2];
                            SendData[2] = tmpdata[1];
                            SendData[3] = tmpdata[0];
                            SendData[4] = (byte)~tmpdata[3];
                            SendData[5] = (byte)~tmpdata[2];
                            SendData[6] = (byte)~tmpdata[1];
                            SendData[7] = (byte)~tmpdata[0];
                            Monitor.Enter(this.requests);
                            requests.AddRange(new byte[1] { (byte)CmdMode.Read_Par });
                            requests.AddRange(SendData);
                            requests.AddRange(new byte[1] { (byte)myDataInfo1.constdatatabnum });
                            Monitor.Exit(this.requests);
                            sendWaiter.Set();
                            timerPlay2.Enabled = true;

                        }
                    }
                    break;
                case Keys.S:

                    if (myDataInfo1.constdatatabnum < myDataInfo1.Const_Num - 1)
                    {
                        munkeyin = false;
                        minpoint = false;
                        minpointcnt = 0;
                        myDataInfo1.constdatatabnum++;
                        myDataInfo1.SendData = myDataInfo1.DataConstValue[myDataInfo1.constdatatabnum].Value;
                        myDataInfo1.UpDataInfoFrom.UDDrawConstChooseList = true;
                        myDataInfo1.glfouse = true;
                    }

                    break;
                case Keys.W:

                    if (myDataInfo1.constdatatabnum > 0)
                    {
                        munkeyin = false;
                        minpoint = false;
                        minpointcnt = 0;
                        myDataInfo1.constdatatabnum--;
                        myDataInfo1.SendData = myDataInfo1.DataConstValue[myDataInfo1.constdatatabnum].Value;
                        myDataInfo1.UpDataInfoFrom.UDDrawConstChooseList = true;
                        myDataInfo1.glfouse = true;
                    }

                    break;
                case Keys.A:
                    if (myDataInfo1.constdatatabnum != 0)
                    {
                        munkeyin = false;
                        minpoint = false;
                        minpointcnt = 0;
                        myDataInfo1.SendData--;
                        myDataInfo1.glfouse = true;
                    }
                    break;
                case Keys.D:
                    if (myDataInfo1.constdatatabnum != 0)
                    {
                        munkeyin = false;
                        minpoint = false;
                        minpointcnt = 0;
                        myDataInfo1.SendData++;
                        myDataInfo1.glfouse = true;
                    }
                    break;
                case Keys.D0:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 0;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(0 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D1:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 1;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(1 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D2:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 2;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(2 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D3:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 3;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(3 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D4:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 4;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(4 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D5:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 5;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(5 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D6:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 6;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(6 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D7:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 7;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(7 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D8:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 8;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(8 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.D9:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        if (minpoint == false)
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData * 10 + 9;
                        }
                        else
                        {
                            myDataInfo1.SendData = myDataInfo1.SendData + (float)(9 * Math.Pow(10, -minpointcnt));
                            minpointcnt++;
                        }
                    }
                    break;
                case Keys.OemPeriod:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (minpoint == false)
                        {
                            minpoint = true;
                            minpointcnt = 1;
                        }
                    }
                    break;
                case Keys.Back:
                    if (myDataInfo1.glfouse == true)
                    {
                        myDataInfo1.SendData = 0;
                        minpoint = false;
                        minpointcnt = 0;
                    }
                    break;
                case Keys.OemMinus:
                    if (myDataInfo1.glfouse == true)
                    {
                        if (munkeyin == false)
                        {
                            myDataInfo1.SendData = 0;
                            munkeyin = true;
                        }
                        myDataInfo1.SendData = -myDataInfo1.SendData;
                    }
                    break;
                default: break;
            }

        }
        #endregion
        #region 串口及显示
        //显示数据
        private void MyShow()
        {

            string ThisPath = string.Empty;
            #region 摄像头数据查看
            if (mymode == MyMode.Local_Sim)
            {

                if (Read_Pic == true)
                {
                    ThisPath = Open_File_Address + "\\原图\\" + trackbarvalue.ToString() + ".bmp";
                    if (File.Exists(ThisPath))
                    {
                        bmpReal = new Bitmap(ThisPath);
                        this.PicReal.MyBackImage = bmpReal;

                    }
                    ThisPath = Open_File_Address + "\\拟图\\" + trackbarvalue.ToString() + ".bmp";
                    if (File.Exists(ThisPath))
                    {
                        bmpFit = new Bitmap(ThisPath);
                        this.PicFit.MyBackImage = bmpFit;
                    }
                }
                ThisPath = Open_File_Address + "\\数据\\" + trackbarvalue.ToString() + ".txt";
                if (File.Exists(ThisPath))
                {
                    int i = 0;
                    if (Read_Pic == true)
                    {
                        string[] temp = ProjectReadString("拟线数据", "左始", ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (i = 0; i < temp.Length; i++)
                        {
                            myDataInfo1.Lcr[i].LStart = (byte)Convert.ToInt32(temp[i]);
                        }
                        temp = ProjectReadString("拟线数据", "左线", ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (i = 0; i < temp.Length; i++)
                        {
                            myDataInfo1.Lcr[i].LBlack = (byte)Convert.ToInt32(temp[i]);
                        }
                        temp = ProjectReadString("拟线数据", "左末", ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (i = 0; i < temp.Length; i++)
                        {
                            myDataInfo1.Lcr[i].LEnd = (byte)Convert.ToInt32(temp[i]);
                        }
                        temp = ProjectReadString("拟线数据", "中线", ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (i = 0; i < temp.Length; i++)
                        {
                            myDataInfo1.Lcr[i].Center = (byte)Convert.ToInt32(temp[i]);
                        }
                        temp = ProjectReadString("拟线数据", "右始", ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (i = 0; i < temp.Length; i++)
                        {
                            myDataInfo1.Lcr[i].RStart = (byte)Convert.ToInt32(temp[i]);
                        }
                        temp = ProjectReadString("拟线数据", "右线", ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (i = 0; i < temp.Length; i++)
                        {
                            myDataInfo1.Lcr[i].RBlack = (byte)Convert.ToInt32(temp[i]);
                        }
                        temp = ProjectReadString("拟线数据", "右末", ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (i = 0; i < temp.Length; i++)
                        {
                            myDataInfo1.Lcr[i].REnd = (byte)Convert.ToInt32(temp[i]);
                        }
                    }

                    for (i = 0; i < myDataInfo1.DataParValue.Count; i++)
                    {
                        myDataInfo1.DataParValue[i].Value = ProjectReadIfloat("反馈数据", i.ToString(), ThisPath);
                    }

                    for (i = 0; i < myDataInfo1.DataConstValue.Count; i++)
                    {
                        myDataInfo1.DataConstValue[i].Value = ProjectReadIfloat("控制固参", i.ToString(), ThisPath);
                    }
                    for (i = 0; i < ParValue.Count; i++)
                    {
                        if (ParValue[i].WaveOn == true)
                        {
                            mypaint1.Item[i].waveon = true;
                            mypaint1.Item[i].AddNode(new ListDataNode(myDataInfo1.DataParValue[i].Value, ParValue[i].WaveMulIndex));
                        }
                    }
                    //mypaint1.MyWaveShow();
                    //myDataInfo1.MyShow(Read_Pic);
                }
            }
            #endregion
            #region 摄像头图像仿真
            else if (mymode == MyMode.Local_Debug)
            {
                //图像
                ThisPath = Open_File_Address + "\\原图\\" + trackbarvalue.ToString() + ".bmp";
                if (File.Exists(ThisPath))
                {
                    bmpReal = new Bitmap(ThisPath);
                    this.PicReal.MyBackImage = bmpReal;

                }
                this.Invoke((EventHandler)(delegate
                {
                    Pixels = ConvertClass.getPixels(PicReal.Pic.Image);
                }));
                //数据
                ThisPath = Open_File_Address + "\\数据\\" + trackbarvalue.ToString() + ".txt";
                if (File.Exists(ThisPath))
                {
                    int i = 0;
                    for (i = 0; i < myDataInfo1.DataParValue.Count; i++)
                    {
                        myDataInfo1.DataParValue[i].Value = ProjectReadIfloat("反馈数据", i.ToString(), ThisPath);
                        MyNrf.这里写仿真程序.SmartProcess.MyDataRead[i] = myDataInfo1.DataParValue[i].Value;
                    }
                    //mypaint1.MyWaveShow();
                    //myDataInfo1.MyShow(Read_Pic);
                }

                MyImageSmartProcess();//链接用户图像处理类



                Image_Fit_Pixels = new byte[Pixels_height, Pixels_width];

                int IMG_Height = Pixels_height - 1;
                int IMG_Width = Pixels_width - 1;

                for (int i = 0; i < Pixels_height; i++)
                {
                    if (MySmartProcess.Lcr[i].LStart < Pixels_width) { Image_Fit_Pixels[IMG_Height - i, IMG_Width - MySmartProcess.Lcr[i].LStart] = 3; }
                    if (MySmartProcess.Lcr[i].LBlack < Pixels_width) { Image_Fit_Pixels[IMG_Height - i, IMG_Width - MySmartProcess.Lcr[i].LBlack] = 5; }
                    if (MySmartProcess.Lcr[i].LEnd < Pixels_width) { Image_Fit_Pixels[IMG_Height - i, IMG_Width - MySmartProcess.Lcr[i].LEnd] = 1; }
                    if (MySmartProcess.Lcr[i].Center < Pixels_width) { Image_Fit_Pixels[IMG_Height - i, IMG_Width - MySmartProcess.Lcr[i].Center] = 7; }
                    if (MySmartProcess.Lcr[i].RStart < Pixels_width) { Image_Fit_Pixels[IMG_Height - i, IMG_Width - MySmartProcess.Lcr[i].RStart] = 4; }
                    if (MySmartProcess.Lcr[i].RBlack < Pixels_width) { Image_Fit_Pixels[IMG_Height - i, IMG_Width - MySmartProcess.Lcr[i].RBlack] = 6; }
                    if (MySmartProcess.Lcr[i].REnd < Pixels_width) { Image_Fit_Pixels[IMG_Height - i, IMG_Width - MySmartProcess.Lcr[i].REnd] = 2; }
                    if (MySmartProcess.Lcr[i].CenterBack < Pixels_width) { Image_Fit_Pixels[IMG_Height - i, IMG_Width - MySmartProcess.Lcr[i].CenterBack] = 8; }
                    myDataInfo1.Lcr[i].LStart = MySmartProcess.Lcr[i].LStart;
                    myDataInfo1.Lcr[i].LBlack = MySmartProcess.Lcr[i].LBlack;
                    myDataInfo1.Lcr[i].LEnd = MySmartProcess.Lcr[i].LEnd;
                    myDataInfo1.Lcr[i].Center = MySmartProcess.Lcr[i].Center;
                    myDataInfo1.Lcr[i].RStart = MySmartProcess.Lcr[i].RStart;
                    myDataInfo1.Lcr[i].RBlack = MySmartProcess.Lcr[i].RBlack;
                    myDataInfo1.Lcr[i].REnd = MySmartProcess.Lcr[i].REnd;
                }

           
               


                this.Invoke((EventHandler)(delegate
                {
                    bmpFit = ConvertClass.ToGrayBitmapFit(ref Image_Fit_Pixels, Pixels_width, Pixels_height, ref FitColor);//合成拟图
                }));
                this.myPictureBox1.MyBackImage = bmpFit;

                ThisPath = Open_File_Address + "\\拟图\\" + trackbarvalue.ToString() + ".bmp";
                if (File.Exists(ThisPath))
                {
                    bmpFit = new Bitmap(ThisPath);
                    this.PicFit.MyBackImage = bmpFit;
                }
                // this.PicFit.MyTxt = VoiceString;

                //myDataInfo1.MyShow();

            }
            #endregion
            #region 光电数据查看
            else if (mymode == MyMode.Local_CCD)
            {
                ThisPath = Open_File_Address + "\\数据\\" + trackbarvalue.ToString() + ".txt";
                if (File.Exists(ThisPath))
                {
                    CCDD = new byte[CCD_height, CCD_width];
                    CCDLRC = new byte[CCD_height + 1, 3];
                    List<Color> tmp_Color = new List<Color>();
                    tmp_Color.Add(CCDColor.CCD1);
                    tmp_Color.Add(CCDColor.CCD2);
                    tmp_Color.Add(CCDColor.CCD3);
                    for (int i = CCD_height - 1; i >= 0; i--)
                    {
                        string[] temp = ProjectReadString("CCD原图", "CCD" + i.ToString(), ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < temp.Length; j++)
                        {
                            CCDD[i, j] = (byte)Convert.ToInt32(temp[j]);
                        }
                        CCDPicReal.Item.Insert(0, new CCDLCR(CCDD, i, CCD_width, tmp_Color[CCD_height - i - 1]));
                    }
                    for (int i = 0; i < CCD_height + 1; i++)
                    {
                        string[] temp = ProjectReadString("CCD拟图", "CCDLRC" + i.ToString(), ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        CCDLRC[i, 0] = (byte)Convert.ToInt32(temp[0]);
                        CCDLRC[i, 1] = (byte)Convert.ToInt32(temp[1]);
                        CCDLRC[i, 2] = (byte)Convert.ToInt32(temp[2]);
                        if (i >= 1)
                        {
                            CCDPicReal.Item[i - 1].ccdpos.Left = CCDLRC[i, 0];
                            CCDPicReal.Item[i - 1].ccdpos.Right = CCDLRC[i, 1];
                            CCDPicReal.Item[i - 1].ccdpos.Center = CCDLRC[i, 2];
                        }
                    }
                    CCDPicFit.Item.Insert(0, new CCDLCR(CCDLRC[0, 0], CCDLRC[0, 1], CCDLRC[0, 2], CCDColor.CCDL, CCDColor.CCDR, CCDColor.CCDC, CCD_width));
                    for (int i = 0; i < myDataInfo1.DataParValue.Count; i++)
                    {
                        myDataInfo1.DataParValue[i].Value = ProjectReadIfloat("反馈数据", i.ToString(), ThisPath);
                    }

                    for (int i = 0; i < myDataInfo1.DataConstValue.Count; i++)
                    {
                        myDataInfo1.DataConstValue[i].Value = ProjectReadIfloat("控制固参", i.ToString(), ThisPath);
                    }
                    for (int i = 0; i < ParValue.Count; i++)
                    {
                        if (ParValue[i].WaveOn == true)
                        {
                            mypaint1.Item[i].waveon = true;
                            mypaint1.Item[i].AddNode(new ListDataNode(myDataInfo1.DataParValue[i].Value, ParValue[i].WaveMulIndex));
                        }
                    }

                    //mypaint1.MyWaveShow();
                    //myDataInfo1.MyShow(Read_Pic);
                    //CCDPicFit.Show_CCDPic();
                    //CCDPicReal.Show_CCDPic();

                }

            }
            #endregion
            #region 光电仿真
            else if (mymode == MyMode.Local_CCD_Debug)
            {
                ThisPath = Open_File_Address + "\\数据\\" + trackbarvalue.ToString() + ".txt";
                if (File.Exists(ThisPath))
                {
                    CCDD = new byte[CCD_height, CCD_width];
                    CCDLRC = new byte[CCD_height + 1, 3];
                    List<Color> tmp_Color = new List<Color>();
                    tmp_Color.Add(CCDColor.CCD1);
                    tmp_Color.Add(CCDColor.CCD2);
                    tmp_Color.Add(CCDColor.CCD3);
                    for (int i = CCD_height - 1; i >= 0; i--)
                    {
                        string[] temp = ProjectReadString("CCD原图", "CCD" + i.ToString(), ThisPath).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < temp.Length; j++)
                        {
                            CCDD[CCD_height - i - 1, j] = (byte)Convert.ToInt32(temp[j]);
                        }
                        CCDPicReal.Item.Insert(0, new CCDLCR(CCDD, CCD_height - i - 1, CCD_width, tmp_Color[CCD_height - i - 1]));
                    }
                    MyImageSmartProcess();
                    for (int i = 0; i < CCD_height + 1; i++)
                    {

                        if (i >= 1)
                        {
                            CCDPicReal.Item[i - 1].ccdpos.Left = MyCCDSmartProcess.Lcr[i].Left;
                            CCDPicReal.Item[i - 1].ccdpos.Right = MyCCDSmartProcess.Lcr[i].Right;
                            CCDPicReal.Item[i - 1].ccdpos.Center = MyCCDSmartProcess.Lcr[i].Center;
                        }
                    }


                    CCDPicFit.Item.Insert(0, new CCDLCR(MyCCDSmartProcess.Lcr[0].Left, MyCCDSmartProcess.Lcr[0].Right, MyCCDSmartProcess.Lcr[0].Center, CCDColor.CCDL, CCDColor.CCDR, CCDColor.CCDC, CCD_width));
                    // this.PicFit.MyTxt = VoiceString;
                }
            }
            #endregion
            //Application.DoEvents();
            //System.Threading.Thread.Sleep(20);
        }
        //用户图像处理类
        void MyImageSmartProcess()
        {
            if (mymode == MyMode.Local_Debug)
            {
                VoiceString = "";
                MySmartProcess.Pixels = Pixels;
                MySmartProcess.Image_Height = Pixels_height;
                MySmartProcess.Image_Width = Pixels_width;
                this.Invoke((EventHandler)(delegate
                {
                    MySmartProcess.FieldCount = trackBar1.Value;
                }));

                MySmartProcess.SignalProcess();//图像处理
                if (PolyKey == true)
                {
                    MySmartProcess.PolyCal();
                }
            }
            else if (mymode == MyMode.Local_CCD_Debug)
            {
                VoiceString = "";
                MyCCDSmartProcess.CCDData = CCDD;
                MyCCDSmartProcess.CCDHeight = (byte)CCD_height;
                MyCCDSmartProcess.CCDWeight = (byte)CCD_width;
                this.Invoke((EventHandler)(delegate
                {
                    MyCCDSmartProcess.Count = trackBar1.Value;
                }));
                MyCCDSmartProcess.SignalProcess();//图像处理
            }
        }
        //后台进程2
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            MyShow();
        }
        //后台进程2完成事件
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (mymode == MyMode.Local_Sim)
            {
                mypaint1.MyWaveShow();
                myDataInfo1.MyShow(Read_Pic);
                myDataInfo2.MyShow(Read_Pic);
            }
            else if (mymode == MyMode.Local_Debug)
            {
                this.PicFit.MyTxt = VoiceString;
                myDataInfo1.MyShow();
                myDataInfo2.MyShow();
            }
            else if (mymode == MyMode.Local_CCD)
            {
                mypaint1.MyWaveShow();
                myDataInfo1.MyShow(Read_Pic);
                CCDPicFit.Show_CCDPic();
                CCDPicReal.Show_CCDPic();
            }
            else if (mymode == MyMode.Local_CCD_Debug)
            {
                this.CCDPicFit.MyTxt = VoiceString;
                CCDPicFit.Show_CCDPic();
                CCDPicReal.Show_CCDPic();
            }
            GC.Collect();
        }
        //串口接收到数据触发事件
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen == false || close == true)
            {
                return;
            }
            //  this.serialPort1.DataReceived -= new SerialDataReceivedEventHandler(SerialPort1_DataReceived);

            if (Listening == true)
            {
                return;
            }
            try
            {
                Listening = true;





                int n = serialPort1.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致

                byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据

                try
                {
                    serialPort1.Read(buf, 0, n);//读取缓冲数据   
                }
                catch
                {
                    return;
                }
                try
                {
                    Data_Image.AddRange(buf);
                }
                catch
                {
                    return;
                }

                //if (waitcmd == true)
                //{
                //    if (mymode == MyMode.Nrf_Pic&&Data_Image[0] == (byte)CmdMode.Write_Data)
                //    {
                //        waitcmd = false;
                //        this.Invoke((EventHandler)(delegate
                //        {
                //        richTextBox1.Text = "nrf主机初始化成功！";
                //        }));
                //    }
                //    if (mymode == MyMode.Nrf_Par&&Data_Image[0] == (byte)CmdMode.Write_Par) 
                //    {
                //        waitcmd = false;
                //        this.Invoke((EventHandler)(delegate
                //        {
                //            richTextBox1.Text = "nrf主机初始化成功！";
                //        }));
                //    }
                //    if (Data_Image[0] == (byte)CmdMode.Write_Par) 
                //    {
                //        this.Invoke((EventHandler)(delegate
                //        {
                //            richTextBox1.Text = "nrf主机初始化失败！";
                //        }));
                //    }
                //    else
                //    {
                //        return;
                //    }
                //    Data_Image.RemoveRange(0, Data_Image.Count);
                //}




                if (this.backgroundWorker1.IsBusy == false)
                {
                    this.backgroundWorker1.RunWorkerAsync();
                }
                //this.Invoke((EventHandler)(delegate
                //{
                //    Serialreadata();
                //}));
                //this.Invoke(Update_Uart_Data);//数据刷新较慢 
                // this.BeginInvoke(Update_Uart_Data);//高速刷新数据，但发送太快会使Data_Image数组溢出  
            }
            finally
            {
                //  this.serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived);
                Listening = false;
            }
        }
        //处理串口接收到的数据
        private void Serialreadata()
        {
            #region 接收数据
            #region 接收验证信息
            if (flag == 0) //接收数据校验
            {
                if (Data_Image.Count > 5)
                {
                    int Index;
                    bool IndexBool = false;
                    for (Index = 0; Index < Data_Image.Count - 4; Index++)
                    {
                        if (Data_Image[Index] == 1 && Data_Image[Index + 1] == 2 && Data_Image[Index + 2] == 3 && Data_Image[Index + 3] == 4 && Data_Image[Index + 4] == 5)
                        {
                            IndexBool = true;
                            break;

                        }
                    }
                    if (IndexBool)
                    {
                        Data_Image.RemoveRange(0, Index + 5);
                        if (MyGroup == MyResult.摄像头组)
                        {
                            flag = 1;
                        }
                        if (MyGroup == MyResult.光电组)
                        {
                            flag = 21;
                        }

                    }
                    else
                    {
                        Data_Image.Clear();
                    }
                }
            }
            #endregion
            #region 接收图像及参数及控制固参信息
            if (flag == 1)//接收图像及参数信息   先接收长度 在宽度   如 长200 宽70//摄像头组用
            {
                if (Data_Image.Count > 4)
                {
                    int Index = 0;
                    Pixels_width = (ushort)(Data_Image[Index]);
                    Index += 1;
                    Pixels_height = (ushort)(Data_Image[Index]);
                    Index += 1;
                    Coefficient_Num = (ushort)(Data_Image[Index]);
                    Index += 1;
                    Parameter_Num = (ushort)(Data_Image[Index]);
                    Index += 1;
                    Data_Image.RemoveRange(0, Index);
                    //if (Pixels_width != 200 || Pixels_height != 70)
                    //{
                    //    flag = 0;
                    //}
                    //else
                    //{
                    if (mymode == MyMode.Uart_Pic || mymode == MyMode.Nrf_Pic || mymode == MyMode.Uart_PicGray)
                    {
                        flag = 2;
                    }
                    else if (mymode == MyMode.Uart_Par || mymode == MyMode.Nrf_Par)
                    {
                        flag = 11;
                    }
                    //}
                }
            }
            #endregion
            #region 接收参数和图像
            #region 接收普通参数
            if (flag == 2) //普通参数采集
            {
                if (Data_Image.Count > Coefficient_Num << 3)
                {
                    UInt32[] tmp = new UInt32[Coefficient_Num << 1];
                    for (int i = 0; i < Coefficient_Num; i++)
                    {
                        tmp[i * 2] = (UInt32)((Data_Image[0] << 0) + (Data_Image[1] << 8) + (Data_Image[2] << 16) + (Data_Image[3] << 24));//接收数据
                        tmp[i * 2 + 1] = (UInt32)((Data_Image[4] << 0) + (Data_Image[5] << 8) + (Data_Image[6] << 16) + (Data_Image[7] << 24));//接收校验


                        if (ParValue.Count != Coefficient_Num)
                        {
                            if (ParValue.Count < Coefficient_Num)
                            {
                                if (ParValue.Count <= i)
                                {
                                    ParValue.Add(new ClassParValue("Value" + (i + 1).ToString(), 1, 1, Color.White, 0, false));
                                    myDataInfo1.DataParValue.Add(new ClassConstParValue(ParValue[i].Name, 0, 4, 1, 0, false, ParValue[i].WaveColor));
                                }
                            }
                            else
                            {
                                //MessageBox.Show("参数不批配");
                            }
                        }
                        if (mypaint1.Item.Count <= i)
                        {
                            mypaint1.Item.Add(new WaveData(1, ParValue[i].Name, ParValue[i].WaveColor, 2));
                        }
                        if ((UInt32)((tmp[i * 2] + tmp[i * 2 + 1]) & 0xFFFFFFFF) != 0xFFFFFFFF)
                        {
                            //数据校验错误
                        }
                        else
                        {
                            DataChange datatmp = new DataChange();
                            ParValue[i].Value = ((float)datatmp.u32tofloat(tmp[i * 2]));
                            myDataInfo1.DataParValue[i].ShutBool = true;
                            myDataInfo1.DataParValue[i].Value = ParValue[i].Value;
                            if (ParValue[i].WaveOn == true)
                            {
                                mypaint1.Item[i].AddNode(new ListDataNode(ParValue[i].Value, ParValue[i].WaveMulIndex));
                                mypaint1.Item[i].waveon = true;
                            }
                        }
                        Data_Image.RemoveRange(0, 8);
                    }

                    flag = 3;
                }
            }
            #endregion
            #region 接收控制固参
            if (flag == 3)
            {
                if (Data_Image.Count > Parameter_Num << 3)
                {
                    UInt32[] tmp = new UInt32[Parameter_Num << 1];
                    for (int i = 0; i < Parameter_Num; i++)
                    {
                        tmp[i * 2] = (UInt32)((Data_Image[0] << 0) + (Data_Image[1] << 8) + (Data_Image[2] << 16) + (Data_Image[3] << 24));//接收数据
                        tmp[i * 2 + 1] = (UInt32)((Data_Image[4] << 0) + (Data_Image[5] << 8) + (Data_Image[6] << 16) + (Data_Image[7] << 24));//接收校验

                        if (ConstValue.Count != Parameter_Num)
                        {
                            if (ConstValue.Count < Parameter_Num)
                            {
                                if (ConstValue.Count <= i)
                                {
                                    ConstValue.Add(new ClassParValue("Value" + (i + 1).ToString(), 1, 1, Color.White, 0, false));
                                    myDataInfo1.DataConstValue.Add(new ClassConstParValue(ConstValue[i].Name, 0, 4, 1, 0, false, ConstValue[i].WaveColor));
                                }
                            }
                            else
                            {
                                //MessageBox.Show("参数不批配");
                            }
                        }
                        if ((UInt32)((tmp[i * 2] + tmp[i * 2 + 1]) & 0xFFFFFFFF) != 0xFFFFFFFF)
                        {
                            //数据校验错误
                        }
                        else
                        {
                            DataChange datatmp = new DataChange();
                            ConstValue[i].Value = ((float)datatmp.u32tofloat(tmp[i * 2]));
                            myDataInfo1.DataConstValue[i].ShutBool = true;
                            myDataInfo1.DataConstValue[i].Value = ConstValue[i].Value;
                        }
                        Data_Image.RemoveRange(0, 8);
                    }

                    flag = 4;
                }

            }
            #endregion
            #region 接收拟图
            if (flag == 4) //拟图采集
            {
                if (Data_Image.Count > Pixels_height * 7)
                {
                    byte Image_Fit_Pixels_Pos;//用于保存点偏移位置
                    int Index = 0, i = 0;
                    Image_Fit_Pixels = new byte[Pixels_height, Pixels_width];
                    for (Index = 0; Index < Pixels_height; Index++)
                    {
                        for (i = 0; i < 7; i++)
                        {
                            Image_Fit_Pixels_Pos = Data_Image[Index * 7 + i];//缓存数据点位置
                            if (Image_Fit_Pixels_Pos < Pixels_width)
                            {
                                Image_Fit_Pixels[Index, Image_Fit_Pixels_Pos] = (byte)(i + 1);//植入并标记颜色
                            }
                            #region 打脚行
                            if (LogKey == true)
                            {

                                if (LogLine < ParValue.Count)
                                {
                                    if (Index == (int)ParValue[LogLine].Value && i == 0)
                                    {
                                        int x, y = 0;
                                        for (int j = 0; j < 5; j++)
                                        {

                                            if (Image_Fit_Pixels_Pos + j < 200 && Image_Fit_Pixels_Pos - j > 0 && Index + j < 70 && Index - j > 0)
                                            {
                                                Image_Fit_Pixels[Index, Image_Fit_Pixels_Pos + j] = 9;
                                                Image_Fit_Pixels[Index, Image_Fit_Pixels_Pos - j] = 9;
                                                Image_Fit_Pixels[Index + j, Image_Fit_Pixels_Pos] = 9;
                                                Image_Fit_Pixels[Index - j, Image_Fit_Pixels_Pos] = 9;
                                            }

                                        }

                                    }
                                }
                            }
                            #endregion

                        }

                        myDataInfo1.Lcr[Index].LStart = Data_Image[Index * 7 + 2];
                        myDataInfo1.Lcr[Index].LBlack = Data_Image[Index * 7 + 4];
                        myDataInfo1.Lcr[Index].LEnd = Data_Image[Index * 7 + 0];
                        myDataInfo1.Lcr[Index].Center = Data_Image[Index * 7 + 6];
                        myDataInfo1.Lcr[Index].RStart = Data_Image[Index * 7 + 3];
                        myDataInfo1.Lcr[Index].RBlack = Data_Image[Index * 7 + 5];
                        myDataInfo1.Lcr[Index].REnd = Data_Image[Index * 7 + 1];

                    }

                    bmpFit = ConvertClass.ToGrayBitmapFit(ref Image_Fit_Pixels, Pixels_width, Pixels_height, ref FitColor);//合成拟图
                    bmpFit.RotateFlip(RotateFlipType.Rotate180FlipNone);//旋转180度




                    Data_Image.RemoveRange(0, Pixels_height * 7);
                    flag = 5;
                }
            }
            #endregion
            #region 接收图像
            if (flag == 5) //图像采集
            {
                if (mymode == MyMode.Uart_Pic || mymode == MyMode.Nrf_Pic)//传输二值化图像
                {
                    if (Data_Image.Count > ((Pixels_height * Pixels_width) >> 3))
                    {

                        bmpReal = ConvertClass.ToGrayBitmapReal(ConvertClass.GetBitPixel(Data_Image, Pixels_width, Pixels_height), Pixels_width, Pixels_height);
                        bmpReal.RotateFlip(RotateFlipType.Rotate180FlipNone);//旋转180度
                        Data_Image.RemoveRange(0, (Pixels_width * Pixels_height) >> 3);

                        //MessageBox.Show("正在接收图像");
                        flag = 100;
                    }
                }
                else if (mymode == MyMode.Uart_PicGray)//传输灰度图像
                {
                    if (Data_Image.Count > (Pixels_height * Pixels_width))
                    {
                        //从list中读取灰度图像
                        bmpReal = ConvertClass.ToGrayBitmapReal(ConvertClass.GetSimPixel(Data_Image, Pixels_width, Pixels_height), Pixels_width, Pixels_height);
                        bmpReal.RotateFlip(RotateFlipType.Rotate180FlipNone);//旋转180度
                        Data_Image.RemoveRange(0, (Pixels_width * Pixels_height));

                        //MessageBox.Show("正在接收图像");
                        flag = 100;
                    }
                }


            }
            #endregion
            #endregion
            #region 只接收参数
            #region 接收普通参数
            if (flag == 11)//无规则数据信息接收
            {
                if (Data_Image.Count > Coefficient_Num << 3)
                {
                    UInt32[] tmp = new UInt32[Coefficient_Num << 1];
                    for (int i = 0; i < Coefficient_Num; i++)
                    {
                        tmp[i * 2] = (UInt32)((Data_Image[0] << 0) + (Data_Image[1] << 8) + (Data_Image[2] << 16) + (Data_Image[3] << 24));//接收数据
                        tmp[i * 2 + 1] = (UInt32)((Data_Image[4] << 0) + (Data_Image[5] << 8) + (Data_Image[6] << 16) + (Data_Image[7] << 24));//接收校验


                        if (ParValue.Count != Coefficient_Num)
                        {
                            if (ParValue.Count < Coefficient_Num)
                            {
                                if (ParValue.Count <= i)
                                {
                                    ParValue.Add(new ClassParValue("Value" + (i + 1).ToString(), 1, 1, Color.White, 0, false));
                                    myDataInfo1.DataParValue.Add(new ClassConstParValue(ParValue[i].Name, 0, 4, 1, 0, false, ParValue[i].WaveColor));
                                }
                            }
                            else
                            {
                                //MessageBox.Show("参数不批配");
                            }
                        }
                        if (mypaint1.Item.Count <= i)
                        {
                            mypaint1.Item.Add(new WaveData(1, ParValue[i].Name, ParValue[i].WaveColor, 2));
                        }
                        if ((UInt32)((tmp[i * 2] + tmp[i * 2 + 1]) & 0xFFFFFFFF) != 0xFFFFFFFF)
                        {

                        }
                        else
                        {
                            DataChange datatmp = new DataChange();
                            ParValue[i].Value = ((float)datatmp.u32tofloat(tmp[i * 2]));
                            myDataInfo1.DataParValue[i].ShutBool = true;
                            myDataInfo1.DataParValue[i].Value = ParValue[i].Value;
                            if (ParValue[i].WaveOn == true)
                            {
                                mypaint1.Item[i].AddNode(new ListDataNode(ParValue[i].Value, ParValue[i].WaveMulIndex));
                                mypaint1.Item[i].waveon = true;
                            }
                        }
                        Data_Image.RemoveRange(0, 8);
                    }
                    flag = 12;
                }
            }
            #endregion
            #region 接收控制固参
            if (flag == 12)
            {
                if (Data_Image.Count > Parameter_Num << 3)
                {
                    UInt32[] tmp = new UInt32[Parameter_Num << 1];
                    for (int i = 0; i < Parameter_Num; i++)
                    {
                        tmp[i * 2] = (UInt32)((Data_Image[0] << 0) + (Data_Image[1] << 8) + (Data_Image[2] << 16) + (Data_Image[3] << 24));//接收数据
                        tmp[i * 2 + 1] = (UInt32)((Data_Image[4] << 0) + (Data_Image[5] << 8) + (Data_Image[6] << 16) + (Data_Image[7] << 24));//接收校验

                        if (ConstValue.Count != Parameter_Num)
                        {
                            if (ConstValue.Count < Parameter_Num)
                            {
                                if (ConstValue.Count <= i)
                                {
                                    ConstValue.Add(new ClassParValue("Value" + (i + 1).ToString(), 1, 1, Color.White, 0, false));
                                    myDataInfo1.DataConstValue.Add(new ClassConstParValue(ConstValue[i].Name, 0, 4, 1, 0, false, ConstValue[i].WaveColor));
                                }
                            }
                            else
                            {
                                //MessageBox.Show("参数不批配");
                            }
                        }
                        if ((UInt32)((tmp[i * 2] + tmp[i * 2 + 1]) & 0xFFFFFFFF) != 0xFFFFFFFF)
                        {
                            //数据校验错误
                        }
                        else
                        {
                            DataChange datatmp = new DataChange();
                            ConstValue[i].Value = ((float)datatmp.u32tofloat(tmp[i * 2]));
                            myDataInfo1.DataConstValue[i].ShutBool = true;
                            myDataInfo1.DataConstValue[i].Value = ConstValue[i].Value;
                        }
                        Data_Image.RemoveRange(0, 8);
                    }

                    flag = 100;
                }
            }
            #endregion
            #endregion
            #region 接收光电组信息
            #region 接收图像及参数及控制固参信息
            if (flag == 21)
            {
                if (Data_Image.Count > 4)
                {
                    int Index = 0;
                    CCD_width = (ushort)(Data_Image[Index]);
                    Index += 1;
                    CCD_height = (ushort)(Data_Image[Index]);
                    Index += 1;
                    Coefficient_Num = (ushort)(Data_Image[Index]);
                    Index += 1;
                    Parameter_Num = (ushort)(Data_Image[Index]);
                    Index += 1;
                    Data_Image.RemoveRange(0, Index);
                    if (mymode == MyMode.Uart_CCD || mymode == MyMode.Nrf_CCD)
                    {
                        flag = 22;
                    }
                    else if (mymode == MyMode.Uart_Par || mymode == MyMode.Nrf_Par)
                    {
                        flag = 11;
                    }

                }
            }
            #endregion
            #region 接收普通参数
            if (flag == 22)
            {
                if (Data_Image.Count > Coefficient_Num << 3)
                {
                    UInt32[] tmp = new UInt32[Coefficient_Num << 1];
                    for (int i = 0; i < Coefficient_Num; i++)
                    {
                        tmp[i * 2] = (UInt32)((Data_Image[0] << 0) + (Data_Image[1] << 8) + (Data_Image[2] << 16) + (Data_Image[3] << 24));//接收数据
                        tmp[i * 2 + 1] = (UInt32)((Data_Image[4] << 0) + (Data_Image[5] << 8) + (Data_Image[6] << 16) + (Data_Image[7] << 24));//接收校验


                        if (ParValue.Count != Coefficient_Num)
                        {
                            if (ParValue.Count < Coefficient_Num)
                            {
                                if (ParValue.Count <= i)
                                {
                                    ParValue.Add(new ClassParValue("Value" + (i + 1).ToString(), 1, 1, Color.White, 0, false));
                                    myDataInfo1.DataParValue.Add(new ClassConstParValue(ParValue[i].Name, 0, 4, 1, 0, false, ParValue[i].WaveColor));
                                }
                            }
                            else
                            {
                                //MessageBox.Show("参数不批配");
                            }
                        }
                        if (mypaint1.Item.Count <= i)
                        {
                            mypaint1.Item.Add(new WaveData(1, ParValue[i].Name, ParValue[i].WaveColor, 2));
                        }
                        if ((UInt32)((tmp[i * 2] + tmp[i * 2 + 1]) & 0xFFFFFFFF) != 0xFFFFFFFF)
                        {
                            //数据校验错误
                        }
                        else
                        {
                            DataChange datatmp = new DataChange();
                            ParValue[i].Value = ((float)datatmp.u32tofloat(tmp[i * 2]));
                            myDataInfo1.DataParValue[i].ShutBool = true;
                            myDataInfo1.DataParValue[i].Value = ParValue[i].Value;
                            if (ParValue[i].WaveOn == true)
                            {
                                mypaint1.Item[i].AddNode(new ListDataNode(ParValue[i].Value, ParValue[i].WaveMulIndex));
                                mypaint1.Item[i].waveon = true;
                            }
                        }
                        Data_Image.RemoveRange(0, 8);
                    }

                    flag = 23;
                }
            }
            #endregion
            #region 接收控制固参
            if (flag == 23)
            {
                if (Data_Image.Count > Parameter_Num << 3)
                {
                    UInt32[] tmp = new UInt32[Parameter_Num << 1];
                    for (int i = 0; i < Parameter_Num; i++)
                    {
                        tmp[i * 2] = (UInt32)((Data_Image[0] << 0) + (Data_Image[1] << 8) + (Data_Image[2] << 16) + (Data_Image[3] << 24));//接收数据
                        tmp[i * 2 + 1] = (UInt32)((Data_Image[4] << 0) + (Data_Image[5] << 8) + (Data_Image[6] << 16) + (Data_Image[7] << 24));//接收校验

                        if (ConstValue.Count != Parameter_Num)
                        {
                            if (ConstValue.Count < Parameter_Num)
                            {
                                if (ConstValue.Count <= i)
                                {
                                    ConstValue.Add(new ClassParValue("Value" + (i + 1).ToString(), 1, 1, Color.White, 0, false));
                                    myDataInfo1.DataConstValue.Add(new ClassConstParValue(ConstValue[i].Name, 0, 4, 1, 0, false, ConstValue[i].WaveColor));
                                }
                            }
                            else
                            {
                                //MessageBox.Show("参数不批配");
                            }
                        }
                        if ((UInt32)((tmp[i * 2] + tmp[i * 2 + 1]) & 0xFFFFFFFF) != 0xFFFFFFFF)
                        {
                            //数据校验错误
                        }
                        else
                        {
                            DataChange datatmp = new DataChange();
                            ConstValue[i].Value = ((float)datatmp.u32tofloat(tmp[i * 2]));
                            myDataInfo1.DataConstValue[i].ShutBool = true;
                            myDataInfo1.DataConstValue[i].Value = ConstValue[i].Value;
                        }
                        Data_Image.RemoveRange(0, 8);
                    }

                    flag = 24;
                }
            }
            #endregion
            #region 接收CCD拟图
            if (flag == 24)
            {
                if (Data_Image.Count > (CCD_height + 1) * 3)
                {
                    //byte Image_Fit_Pixels_Pos;//用于保存点偏移位置
                    int Index = 0;
                    CCDLRC = new byte[CCD_height + 1, 3];
                    for (Index = 0; Index < CCD_height + 1; Index++)
                    {
                        CCDLRC[Index, 0] = Data_Image[2 + Index * 3];
                        CCDLRC[Index, 1] = Data_Image[1 + Index * 3];
                        CCDLRC[Index, 2] = Data_Image[0 + Index * 3];
                        if (Index == 0)
                        {
                            CCDPicFit.Item.Insert(0, new CCDLCR(Data_Image[2], Data_Image[1], Data_Image[0], CCDColor.CCDL, CCDColor.CCDR, CCDColor.CCDC, CCD_width));
                        }

                    }

                    Data_Image.RemoveRange(0, (CCD_height + 1) * 3);
                    flag = 25;
                }
            }
            #endregion
            #region 接收CCD原图
            if (flag == 25)
            {
                if (Data_Image.Count > (CCD_height * CCD_width))
                {
                    CCDD = new byte[CCD_height, CCD_width];
                    CCDPicReal.lines = CCD_height;
                    List<Color> tmp_Color = new List<Color>();
                    tmp_Color.Add(CCDColor.CCD1);
                    tmp_Color.Add(CCDColor.CCD2);
                    tmp_Color.Add(CCDColor.CCD3);
                    for (int i = 0; i < CCD_height; i++)
                    {
                        CCDPicReal.Item.Insert(0, new CCDLCR(Data_Image, CCD_width, tmp_Color[CCD_height - i - 1]));
                        CCDPicReal.Item[0].ccdpos.Left = CCDLRC[i + 1, 0];
                        CCDPicReal.Item[0].ccdpos.Right = CCDLRC[i + 1, 1];
                        CCDPicReal.Item[0].ccdpos.Center = CCDLRC[i + 1, 2];
                        for (int j = 0; j < CCD_width; j++)
                        {
                            CCDD[i, j] = Data_Image[j];
                        }
                        Data_Image.RemoveRange(0, CCD_width);
                    }
                    flag = 100;
                }

            }
            #endregion
            #endregion


            #region 数据接收完成后续处理
            //this.Invoke((EventHandler)(delegate
            //{
            if (flag == 100)
            {
                flag = 101;
                if (Save_Flag == true)
                {
                    if (mymode == MyMode.Uart_Pic || mymode == MyMode.Nrf_Pic || mymode == MyMode.Uart_PicGray)
                    {
                        DATA_Save.Add(new DataAll(ParValue, Coefficient_Num, ConstValue, Parameter_Num, bmpFit, bmpReal, myDataInfo1.Lcr));
                    }
                    else if (mymode == MyMode.Uart_Par || mymode == MyMode.Nrf_Par)
                    {
                        DATA_Save.Add(new DataAll(ParValue, Coefficient_Num, ConstValue, Parameter_Num));
                    }
                    else if (mymode == MyMode.Uart_CCD || mymode == MyMode.Nrf_CCD)
                    {
                        DATA_Save.Add(new DataAll((byte)CCD_width, (byte)CCD_height, CCDD, CCDLRC, ParValue, Coefficient_Num, ConstValue, Parameter_Num));
                    }
                }

                //    if (All_length == 0)
                //    {
                //        if (mymode == MyMode.Uart_Pic)
                //        {
                //            All_length = (ushort)(5 + Pixels_height * Pixels_width / 8 + Coefficient_Num * 8 + Parameter_Num * 8 + 4 + 490);
                //        }
                //        if (mymode == MyMode.Uart_Par)
                //        {
                //            All_length = (ushort)(5 + Coefficient_Num * 8 + Parameter_Num * 8 + 4);
                //        }
                //    }
                if (mymode == MyMode.Uart_Pic || mymode == MyMode.Nrf_Pic || mymode == MyMode.Uart_PicGray)
                {
                    PicFit.MyBackImage = bmpFit;
                    PicReal.MyBackImage = bmpReal;
                }
                //    if (mymode == MyMode.Uart_CCD) 
                //    {
                //   //    MessageBox.Show(PicReal.Item.Count.ToString());
                //        CCDPicFit.Show_CCDPic();
                //        CCDPicReal.Show_CCDPic();
                //    }
                //    mypaint1.MyWaveShow();
                //    myDataInfo1.MyShow(Read_Pic);
                //    Coefficient_Info.Clear();
                //    Parameter_Info.Clear();

                //    try
                //    {
                //        richTextBox1.MyText = "开始接收数据:" + "     " + "缓存区大小：" + Data_Image.Count.ToString("0000") + "     " + "数据总长度：" + All_length.ToString("00000") + "\r\n" + "参数数量：" + Coefficient_Num.ToString("0000") + "     " + "参数长度：" + (Coefficient_Num * 4).ToString("0000") + "     " + "固参数量：" + Parameter_Num.ToString("0000") + "     " + "固参长度：" + (Parameter_Num * 4).ToString("0000") + "\r\n" + DATA_Save.Count.ToString();
                //    }
                //    catch
                //    {

                //        return;
                //    }
                //    for (int i = 0; i < Coefficient_Num; i++)
                //    {
                //        ParValue[i].ClassDataValueClear();
                //    }
                //    for (int i = 0; i < Parameter_Num; i++)
                //    {
                //        ConstValue[i].ClassDataValueClear();
                //    }
                //Pixels_height = 0;
                //Pixels_width = 0;
                //Coefficient_Num = 0; //系数数量
                //Coefficient_length = 0;
                //Parameter_Num = 0; //参数
                //Parameter_length = 0;
            }
            //}));

            #endregion
            #endregion
        }//接收数据
        private void Send(object obj)
        {
            while (true)
            {
                this.sendWaiter.WaitOne();
                if (sendWaiterstop == true)
                {
                    break;
                }
                Monitor.Enter(this.requests);
                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Write(requests.ToArray(), 0, requests.Count);
                    requests.Clear();
                }
                Monitor.Exit(this.requests);
            }

        }//发送数据(这个函数单独一个线程)
        //后台进程1
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Serialreadata();
        }
        //后台进程1完成事件
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (flag == 101)
            {
                flag = 0;
                //if (Save_Flag == true)
                //{
                //    if (mymode == MyMode.Uart_Pic)
                //    {
                //        DATA_Save.Add(new DataAll(ParValue, Coefficient_Num, ConstValue, Parameter_Num, bmpFit, bmpReal, myDataInfo1.Lcr));
                //    }
                //    else if (mymode == MyMode.Uart_Par)
                //    {
                //        DATA_Save.Add(new DataAll(ParValue, Coefficient_Num, ConstValue, Parameter_Num));
                //    }
                //}
                if (All_length == 0)
                {
                    if (mymode == MyMode.Uart_Pic || mymode == MyMode.Nrf_Pic)
                    {
                        All_length = (ushort)(5 + Pixels_height * Pixels_width / 8 + Coefficient_Num * 8 + Parameter_Num * 8 + 4 + 490);
                    }
                    if (mymode == MyMode.Uart_Par || mymode == MyMode.Nrf_Par)
                    {
                        All_length = (ushort)(5 + Coefficient_Num * 8 + Parameter_Num * 8 + 4);
                    }
                    if (mymode == MyMode.Uart_CCD || mymode == MyMode.Nrf_CCD)
                    {
                        All_length = (ushort)(5 + Coefficient_Num * 8 + Parameter_Num * 8 + 4 + CCD_height * CCD_width);
                    }
                }
                //if (mymode == MyMode.Uart_Pic)
                //{
                //    this.BeginInvoke((EventHandler)(delegate
                //    {
                //        PicFit.MyBackImage = bmpFit;
                //        PicReal.MyBackImage = bmpReal;
                //    }));
                //}
                if (mymode == MyMode.Uart_CCD || mymode == MyMode.Nrf_CCD)
                {
                    //    MessageBox.Show(PicReal.Item.Count.ToString());
                    CCDPicFit.Show_CCDPic();
                    CCDPicReal.Show_CCDPic();
                }
                mypaint1.MyWaveShow();
                myDataInfo1.MyShow(Read_Pic);
                myDataInfo2.MyShow(Read_Pic);
                Coefficient_Info.Clear();
                Parameter_Info.Clear();

                //try
                //{
                this.Invoke((EventHandler)(delegate
                {
                    richTextBox1.MyText = "开始接收数据:" + "     " + "缓存区大小：" + Data_Image.Count.ToString("00000") + "     " + "数据总长度：" + All_length.ToString("00000") + "\r\n" + "参数数量：" + Coefficient_Num.ToString("0000") + "     " + "参数长度：" + (Coefficient_Num * 4).ToString("0000") + "     " + "固参数量：" + Parameter_Num.ToString("0000") + "     " + "固参长度：" + (Parameter_Num * 4).ToString("0000") + "\r\n" + DATA_Save.Count.ToString();
                    close = true;
                    if (Data_Image.Count > All_length)
                    {
                        if (this.backgroundWorker1.IsBusy == false)
                        {
                            this.backgroundWorker1.RunWorkerAsync();
                        }


                    }
                    close = false;
                }));

                //}
                //catch
                //{

                //    return;
                //}
                //for (int i = 0; i < Coefficient_Num; i++)
                //{
                //    ParValue[i].ClassDataValueClear();
                //}
                //for (int i = 0; i < Parameter_Num; i++)
                //{
                //    ConstValue[i].ClassDataValueClear();
                //}
            }
        }
        #endregion
        #region 全屏操作
        //双击退出全屏
        private void frmExit_DoubleClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                mypaint1.Top = WavePos.Top;
                mypaint1.Left = WavePos.Left;
                mypaint1.Height = WavePos.Height;
                mypaint1.Width = WavePos.Width;
                mypaint1.UpDataWaveFrom = new WaveListupdatamsg();

                this.tabPage1.Controls.Add(mypaint1);
                tabControl1.TabPages.Insert(0, tabPage1);
                mypaint1.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(frmExit_DoubleClick);
                mypaint1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(PaintWave_DoubleClick);
                tmpfrmAllScreen = frmstatus.frmdefault;
                frmAllScreen.Dispose();
                GC.Collect();
            }
            else
            {
                return;
            }
        }
        //按ESC退出全屏
        private void frm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

                mypaint1.Top = WavePos.Top;
                mypaint1.Left = WavePos.Left;
                mypaint1.Height = WavePos.Height;
                mypaint1.Width = WavePos.Width;
                this.tabPage1.Controls.Add(mypaint1);
                mypaint1.UpDataWaveFrom = new WaveListupdatamsg();
                tabControl1.TabPages.Insert(0, tabPage1);
                mypaint1.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(frmExit_DoubleClick);
                mypaint1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(PaintWave_DoubleClick);
                tmpfrmAllScreen = frmstatus.frmdefault;
                (sender as Form).Dispose();
                GC.Collect();
            }


        }
        //双击波形窗口
        private void PaintWave_DoubleClick(object sender, MouseEventArgs e)
        {
            if (tmpfrmAllScreen != frmstatus.frmdefault)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                frmAllScreen = new Form();
                frmAllScreen.ShowIcon = false;
                frmAllScreen.ShowInTaskbar = false;
                frmAllScreen.Name = "Wave";
                frmAllScreen.BackColor = Color.Black;
                frmAllScreen.Width = 0;
                frmAllScreen.Height = 0;
                frmAllScreen.TopMost = true;
                frmAllScreen.FormBorderStyle = FormBorderStyle.None;
                frmAllScreen.ShowInTaskbar = false;
                if (Screen.AllScreens.Length >= 2)
                {

                    DialogResult result = MessageBox.Show("系统检测到你有多个显示器，是否将全屏显示到第二个个屏幕上?", "提示", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        frmAllScreen.Show();
                        frmAllScreen.DesktopLocation = Screen.AllScreens[1].Bounds.Location;
                        frmAllScreen.Width = Screen.AllScreens[1].Bounds.Size.Width;
                        frmAllScreen.Height = Screen.AllScreens[1].Bounds.Size.Height;
                        tmpfrmAllScreen = frmstatus.frmall;

                    }
                    else if (result == DialogResult.No)
                    {
                        frmAllScreen.Show();
                        frmAllScreen.DesktopLocation = Screen.AllScreens[0].Bounds.Location;
                        frmAllScreen.Width = Screen.AllScreens[0].Bounds.Size.Width;
                        frmAllScreen.Height = Screen.AllScreens[0].Bounds.Size.Height;
                        tmpfrmAllScreen = frmstatus.frmall;
                    }
                    else
                    {
                        frmAllScreen.Dispose();
                        return;
                    }
                }
                else
                {
                    frmAllScreen.Show();
                    frmAllScreen.DesktopLocation = Screen.AllScreens[0].Bounds.Location;
                    frmAllScreen.Width = Screen.AllScreens[0].Bounds.Size.Width;
                    frmAllScreen.Height = Screen.AllScreens[0].Bounds.Size.Height;
                    tmpfrmAllScreen = frmstatus.frmall;
                }
                mypaint1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(frmExit_DoubleClick);
                frmAllScreen.Controls.Add(mypaint1);
                mypaint1.Left = 0;
                mypaint1.Top = 0;
                mypaint1.Height = frmAllScreen.Height;
                mypaint1.Width = frmAllScreen.Width;
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.SelectedIndex = 1;
                frmAllScreen.KeyPreview = true;
                frmAllScreen.KeyUp += new KeyEventHandler(frm_KeyUp);
                mypaint1.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(PaintWave_DoubleClick);
                mypaint1.UpDataWaveFrom = new WaveListupdatamsg();
                mypaint1.MyWaveShow();
            }
            else
            {
                return;
            }
        }

        private void frmAllScreen_Resize(object sender, EventArgs e)
        {
            mypaint1.Top = 0;
            mypaint1.Left = 0;
            mypaint1.Height = frmAllScreen.Height;
            mypaint1.Width = frmAllScreen.Width;
            mypaint1.UpDataWaveFrom = new WaveListupdatamsg();
            mypaint1.MyWaveShow();
        }
        #endregion
        #region 数据读写
        //存储数据
        public void IniCfgSave()
        {
            string address = Child_Address.Remove(Child_Address.LastIndexOf("\\"));
            this.BeginInvoke((EventHandler)(delegate //访问UI来更新界面
            {
                richTextBox1.MyText = "硬盘写入中…… ";
            }));
            int num = DATA_Save.Count;
            string Path;
            if (mymode == MyMode.Uart_Pic || mymode == MyMode.Nrf_Pic || mymode == MyMode.Uart_PicGray)
            {

                for (int i = 0; i < num; i++)
                {
                    this.BeginInvoke((EventHandler)(delegate
                    {
                        richTextBox1.MyText = "硬盘写入中…… " + "\r\n" + (num - i).ToString();
                    }));
                    if (DATA_Save[i].BmtReal != null)
                    {

                        DATA_Save[i].BmtReal.Save(address + "\\原图\\" + (i + 1).ToString() + ".bmp", ImageFormat.Bmp);

                    }

                    if (DATA_Save[i].BmtFit != null)
                    {
                        DATA_Save[i].BmtFit.Save(address + "\\拟图\\" + (i + 1).ToString() + ".bmp", ImageFormat.Bmp);

                    }

                    Path = address + "\\数据\\" + (i + 1).ToString() + ".txt";


                    ProjectWrite("拟线数据", "左始", DATA_Save[i].LStart.ToString(), Path);

                    ProjectWrite("拟线数据", "左线", DATA_Save[i].LBlack.ToString(), Path);

                    ProjectWrite("拟线数据", "左末", DATA_Save[i].LEnd.ToString(), Path);

                    ProjectWrite("拟线数据", "中线", DATA_Save[i].Center.ToString(), Path);

                    ProjectWrite("拟线数据", "右始", DATA_Save[i].RStart.ToString(), Path);

                    ProjectWrite("拟线数据", "右线", DATA_Save[i].RBlack.ToString(), Path);

                    ProjectWrite("拟线数据", "右末", DATA_Save[i].REnd.ToString(), Path);

                    for (int j = 0; j < DATA_Save[i].ParValue.Count; j++)
                    {
                        ProjectWrite("反馈数据", j.ToString(), DATA_Save[i].ParValue[j].ToString(), Path);
                    }

                    for (int j = 0; j < DATA_Save[i].CoeValue.Count; j++)
                    {
                        ProjectWrite(" 控制固参", j.ToString(), DATA_Save[i].CoeValue[j].ToString(), Path);
                    }


                }
            }
            else if (mymode == MyMode.Uart_Par || mymode == MyMode.Nrf_Par)
            {
                for (int i = 0; i < num; i++)
                {
                    this.BeginInvoke((EventHandler)(delegate
                    {
                        richTextBox1.MyText = "硬盘写入中…… " + "\r\n" + (num - i).ToString();
                    }));
                    Path = address + "\\数据\\" + (i + 1).ToString() + ".txt";
                    for (int j = 0; j < DATA_Save[i].ParValue.Count; j++)
                    {
                        ProjectWrite("反馈数据", j.ToString(), DATA_Save[i].ParValue[j].ToString(), Path);
                    }

                    for (int j = 0; j < DATA_Save[i].CoeValue.Count; j++)
                    {
                        ProjectWrite(" 控制固参", j.ToString(), DATA_Save[i].CoeValue[j].ToString(), Path);
                    }
                }
            }
            else if (mymode == MyMode.Uart_CCD || mymode == MyMode.Nrf_CCD)
            {

                for (int i = 0; i < num; i++)
                {
                    this.BeginInvoke((EventHandler)(delegate
                    {
                        richTextBox1.MyText = "硬盘写入中…… " + "\r\n" + (num - i).ToString();
                    }));
                    Path = address + "\\数据\\" + (i + 1).ToString() + ".txt";
                    for (int j = 0; j < DATA_Save[i].CCDD.Length; j++)
                    {
                        ProjectWrite("CCD原图", "CCD" + j.ToString(), DATA_Save[i].CCDD[j], Path);
                    }

                    for (int j = 0; j < DATA_Save[i].CCDLRCD.Length; j++)
                    {
                        ProjectWrite(" CCD拟图", "CCDLRC" + j.ToString(), DATA_Save[i].CCDLRCD[j], Path);
                    }

                    for (int j = 0; j < DATA_Save[i].ParValue.Count; j++)
                    {
                        ProjectWrite("反馈数据", j.ToString(), DATA_Save[i].ParValue[j].ToString(), Path);
                    }

                    for (int j = 0; j < DATA_Save[i].CoeValue.Count; j++)
                    {
                        ProjectWrite(" 控制固参", j.ToString(), DATA_Save[i].CoeValue[j].ToString(), Path);
                    }
                }
            }

            DATA_Save.RemoveRange(0, DATA_Save.Count);
            this.Invoke((EventHandler)(delegate
            {
                richTextBox1.MyText = "写入完成…… ";
            }));
            GC.Collect();
            if (ThreadWrite != null)
            {
                if (ThreadWrite.IsAlive)
                {
                    ThreadWrite.Abort();
                }
            }
        }
        //以字符串形式读取
        private string ProjectReadString(string Section, string key, string Path)
        {
            StringBuilder temp = new StringBuilder(4096);
            GetPrivateProfileString(Section, key, "", temp, 4096, Path);
            return temp.ToString();
        }
        //以整型形式读取
        private int ProjectReadInt(string Section, string key, string Path)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, Path);
            return Convert.ToInt32(temp.ToString());
        }

        private void dssgsgsdgToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //MySmartProcess.podao_process = Convert.ToByte(textBox1.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            MySmartProcess.ClearFlag();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void settext(object msg)
        {
            listBox1.Items.Add(msg);
        }

        public void cleartxt()
        {
            listBox1.Items.Clear();
        }

        private void mypaint1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PicReal_Load(object sender, EventArgs e)
        {

        }

        private void CCDPicReal_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            MySmartProcess.LeftBlock();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MySmartProcess.RightBlock();
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            MySmartProcess.BrkRoadAddLine();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            MySmartProcess.Anulus0();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MySmartProcess.Anulus1();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MySmartProcess.Anulus2();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MySmartProcess.Anulus3();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MySmartProcess.BrkRoadNotAddLine();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MySmartProcess.StopCross();
        }

        private void PicFit_Load(object sender, EventArgs e)
        {

        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void myDataInfo1_Click(object sender, EventArgs e)
        {

        }

        private void myButton1_Load(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void myPictureBox1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }


        //以浮点型形式读取
        private float ProjectReadIfloat(string Section, string key, string Path)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, Path);
            return (float)Convert.ToDouble(temp.ToString());
        }
        //写入数据
        private void ProjectWrite(string section, string key, string val, string filepath)
        {
            WritePrivateProfileString(section, key, val, filepath);
        }
        #endregion
    }
}
