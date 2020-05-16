using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
namespace MyNrf
{
    public partial class MyUartDebug : Form
    {
        private System.Drawing.Point mousePosition;//记录鼠标坐标
        private bool WirtePort = false;
        private bool SerialPortIsReady = false;
        private bool Listening = false;
        private AutoResetEvent sendWaiter;//AutoResetEvent接收到信号之后自动清零 ManualResetEvent不清零
        private AutoResetEvent receiveWaiter;
        private bool sendWaiterstop = false;
        private bool receiveWaiterstop = false;
        private List<byte> responses;//接收数据缓存
        private List<byte> requests;//发送数据缓存
        private byte TargetPar=1;
        private byte[] SendPar=new byte[8];
        private class ShelvesDataReceivedEventArgs : EventArgs
        {
            private string realtime;

            public ShelvesDataReceivedEventArgs(string realtime)
            {
                this.realtime = realtime;
            }

            public string Realtime
            {
                get
                {
                    return this.realtime;
                }
            }
        }
        private delegate void UpdateDelegate(string realtime);
        byte[] tmpvalue = new byte[4];
        DataChange changetmpvalue = new DataChange();
        public MyUartDebug()
        {
            InitializeComponent();
        }
        private void MyUartDebug_Load(object sender, EventArgs e) 
        {
            this.requests = new List<byte>();
            this.responses = new List<byte>();
            this.sendWaiter = new AutoResetEvent(false);
            this.receiveWaiter = new AutoResetEvent(false);
            sendWaiterstop = false;
            receiveWaiterstop = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(Send));//开启发送线程
            ThreadPool.QueueUserWorkItem(new WaitCallback(Received));//开启接收线程
            for (int i = 1; i <= 126; i++)
            {
                this.comboBox3.Items.Add("频道:" + i.ToString("000"));
            }
            this.comboBox3.Text = MyNrf.Form1.NrfChannel;
            tmpvalue=changetmpvalue.floattou32byte((float)this.numericUpDown1.Value);
            SendPar[0] = tmpvalue[3];
            SendPar[1] = tmpvalue[2];
            SendPar[2] = tmpvalue[1];
            SendPar[3] = tmpvalue[0];
            SendPar[4] = (byte)~tmpvalue[3];
            SendPar[5] = (byte)~tmpvalue[2];
            SendPar[6] = (byte)~tmpvalue[1];
            SendPar[7] = (byte)~tmpvalue[0];
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
        //鼠标左键单击窗体并拖动事件
        private void Top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - mousePosition.Y;
                this.Left = Control.MousePosition.X - mousePosition.X;
            }
        }
        private void myButton1_Click(object sender, MouseEventArgs e) 
        {
            while (Listening == true)
            {
                Application.DoEvents();
            }
            serialPort1.Close();
            sendWaiterstop = true;//关闭线程
            receiveWaiterstop = true;//关闭线程
            sendWaiter.Set();
            receiveWaiter.Set();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "刷新串口")
            {
                if (serialPort1.IsOpen == true)
                {
                    WirtePort = true;
                    try
                    {
                        serialPort1.DiscardInBuffer();
                    }
                    catch
                    {
                        MessageBox.Show("清空缓存异常");
                        return;
                    }
                    WirtePort = false;
                }
                else 
                {
                    MessageBox.Show("串口以断开");
                    button1.Text = "检测串口";
                    button2.Text = "开始读取";
                    SerialPortIsReady = false;

                }
            }
            else 
            {
                    int i=1;
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
                            SerialPortIsReady = false;
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
                    comboBox2.Text = "检测到" + serialPort1.PortName;
                    comboBox1.Text = serialPort1.BaudRate.ToString();
                    serialPort1.Close();
                    serialPort1.ReceivedBytesThreshold = 1;//设置串口触发速度
                    button2.Text = "开始读取";
                    SerialPortIsReady = true;
                    this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort1_DataReceived);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SerialPortIsReady == true)
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
                        serialPort1.Open();
                    }
                    catch
                    {
                        MessageBox.Show(serialPort1.PortName + "已经断开");
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;
                        button2.Text = "开始读取";
                        button1.Text = "检测串口";
                        SerialPortIsReady = false;
                    }
                    return;
                }
                else
                {
                    WirtePort = true;
                    while (Listening == true)
                    {
                        Application.DoEvents();
                    }
                    serialPort1.Close();
                    button2.Text = "开始读取";
                    button1.Text = "检测串口";
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    WirtePort = false;
                }
            }
            else 
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                button2.Text = "打开串口";
                button1.Text = "检测串口";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Monitor.Enter(this.requests);
            requests.AddRange(Encoding.Default.GetBytes(this.textBox1.Text));
            Monitor.Exit(this.requests);
            sendWaiter.Set();
            this.textBox1.Text = "";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }
        private void button5_Click(object sender, EventArgs e)
        {

        }
        private void button6_Click(object sender, EventArgs e)
        {
            Monitor.Enter(this.requests);
            requests.Clear();
            switch (textBox2.Text)
            {
                case "Waitting": requests.AddRange(new byte[1] { (byte)CmdMode.Waitting }); sendWaiter.Set(); break;
                case "WriteData": requests.AddRange(new byte[1] { (byte)CmdMode.Write_Data }); sendWaiter.Set(); break;
                case "ReadPar": requests.AddRange(new byte[1] { (byte)CmdMode.Read_Par }); requests.AddRange(SendPar); requests.AddRange(new byte[1] { TargetPar }); sendWaiter.Set(); break;
                case "InitParLen": requests.AddRange(new byte[3] { (byte)CmdMode.InitParLen, byte.Parse(this.comboBox3.Text.Substring(this.comboBox3.Text.IndexOf(":") + 1, 3)),default(byte)}); sendWaiter.Set(); break;
                case "WritePar": requests.AddRange(new byte[1] { (byte)CmdMode.Write_Par }); sendWaiter.Set(); break;
                case "StopCar": requests.AddRange(new byte[1] { (byte)CmdMode.Stop_Car }); sendWaiter.Set(); break;
                case "UcStopCar": requests.AddRange(new byte[1] { (byte)CmdMode.UcStop_Car }); sendWaiter.Set(); break;
                case "RstChip": requests.AddRange(new byte[1] { (byte)CmdMode.RstChip }); sendWaiter.Set(); break;
                case "NrfError": requests.AddRange(new byte[1] { (byte)CmdMode.NrfError }); sendWaiter.Set(); break;
                case "ReadCmd": requests.AddRange(new byte[1] { (byte)CmdMode.ReadCmd }); sendWaiter.Set(); break;
                default: break;
            }
            Monitor.Exit(this.requests);
        }

        private void textBox2_TextChanged(object sender, EventArgs e) 
        {    
             switch (textBox2.Text) 
            {
                case "请选择命令": this.label5.Text = "无"; break;
                case "Waitting": this.label5.Text = "等待状态"; break;
                case "WriteData": this.label5.Text = "输出数据"; break;
                case "ReadPar": this.label5.Text = "读入参数"; break;
                case "InitParLen": this.label5.Text = "初始化参数"; break;
                case "WritePar": this.label5.Text = "输出参数信息"; break;
                case "StopCar": this.label5.Text = "停车"; break;
                case "UcStopCar": this.label5.Text = "自定义停车"; break;
                case "RstChip": this.label5.Text = "复位芯片"; break;
                case "NrfError": this.label5.Text = "主机错误";  break;
                case "ReadCmd": this.label5.Text = "读取主机状态"; break;
                default: this.label5.Text = "无效命令"; break;
            }
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            this.radioButton1.Checked = !this.radioButton1.Checked;
        }

        private void waittingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "Waitting";
        }

        private void writeDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "WriteData";         
        }

        private void readParToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "ReadPar";     
        }

        private void initParLenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "InitParLen";           
        }

        private void writeParToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "WritePar";     
        }

        private void stopCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "StopCar";           
        }

        private void ucStopCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "UcStopCar";          
        }

        private void rstChipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "RstChip";            
        }

        private void nrfErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "NrfError";           
        }

        private void readCmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "ReadCmd";            
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            TargetPar = (byte)this.numericUpDown2.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            tmpvalue = changetmpvalue.floattou32byte((float)this.numericUpDown1.Value);
            SendPar[0] = tmpvalue[3];
            SendPar[1] = tmpvalue[2];
            SendPar[2] = tmpvalue[1];
            SendPar[3] = tmpvalue[0];
            SendPar[4] = (byte)~tmpvalue[3];
            SendPar[5] = (byte)~tmpvalue[2];
            SendPar[6] = (byte)~tmpvalue[1];
            SendPar[7] = (byte)~tmpvalue[0];
        }
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e) 
        {
            if (serialPort1.IsOpen == false || WirtePort == true||Listening == true)
            {
                return;
            }
            try
            {
                Listening = true;
                if (this.serialPort1.BytesToRead > 0)
                {
                    byte[] buffer = new byte[this.serialPort1.BytesToRead];
                    int readCount = this.serialPort1.Read(buffer, 0, buffer.Length);
                    Monitor.Enter(this.responses);
                    this.responses.AddRange(buffer);
                    Monitor.Exit(this.responses);
                    receiveWaiter.Set();//发送同步信号激活接收线程
                }

            }
            finally 
            {
                Listening = false;
            }


        }


        private void Received(object obj) 
        {
            while (true) 
            {
                this.receiveWaiter.WaitOne();
                if (receiveWaiterstop == true) 
                {
                    break;
                }
                Monitor.Enter(this.responses);//允许访问和改动responses
                while (responses.Count > 2)
                {
                    int index;
                    byte[] buf=new byte[(int)(responses.Count>>1)*2];
                    for ( index= 0; index <(int)(responses.Count >> 1)*2; index++) 
                    {
                        buf[index] = responses[index];
                    }
                    responses.RemoveRange(0, index);
                    string realtime=string.Empty;
                    if (this.radioButton1.Checked == false)
                    {
                        realtime = System.Text.Encoding.GetEncoding("GB2312").GetString(buf);
                    }
                    else 
                    {
                        for (int i = 0; i < buf.Length; i++) 
                        {
                            realtime+= buf[i].ToString("X2")+" ";
                        }
                            
                    }
                    this.ShelvesDataReceived(this, new ShelvesDataReceivedEventArgs(realtime));
                }
                Monitor.Exit(this.responses);//禁止访问和改动responses
            }
         
        }

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
                    string realtime = "\r\n已发送数据：";
                    for (int i = 0; i < requests.Count; i++)
                    {
                        realtime += requests[i].ToString("X2") + " ";
                    }
                    realtime += "\r\n";
                    this.ShelvesDataReceived(this, new ShelvesDataReceivedEventArgs(realtime));
                    requests.Clear();
                }
                Monitor.Exit(this.requests);
            }
            
        }

        private void ShelvesDataReceived(object sender, ShelvesDataReceivedEventArgs e)
        {
            this.Invoke(new UpdateDelegate(this.Update), new object[] { e.Realtime });
        }

        private void Update(string realtime)//更新界面
        {
            this.richTextBox1.Text += realtime;
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }






    }
}
