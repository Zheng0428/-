using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNrf
{
    public partial class MySetup : Form
    {
        private System.Drawing.Point mousePosition;

        public MySetup()
        {
            InitializeComponent();
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 0), new Point(this.Width - 1, 0));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, this.Height - 1), new Point(0, this.Height - 1));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, this.Height - 1), new Point(0, 0));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 37), new Point(this.Width - 1, 37));

        }
        private void MySetup_Load(object sender, EventArgs e)
        {
            textBox1.Text = MyNrf.Form1.Save_File_Address;
            folderBrowserDialog1.ShowNewFolderButton = true;
            this.checkBox1.Checked = MyNrf.Form1.Pit_Map;
            this.checkBox2.Checked = MyNrf.Form1.PolyKey;
            this.numericUpDown2.Value = MyNrf.Form1.Polyfit;
            trackBar1.Maximum = 500;
            trackBar1.Minimum = 1;
            trackBar1.TickFrequency =50;
            trackBar1.Value = MyNrf.Form1.Pit_Play_Speed;
            trackBar1.Enabled = true;
            comboBox1.Text = MyNrf.Form1.NrfChannel;
            this.numericUpDown1.Value = MyNrf.Form1.WaveDataLength;
            for (int i = 1; i <= 126; i++)
            {
                this.comboBox1.Items.Add("频道:" + i.ToString("000"));
            }
            label3.Text = "↓↓↓播放速度" + trackBar1.Value.ToString() + "ms/f";
            for (int i = 0; i < MyNrf.Form1.ParValue.Count; i++) 
            {
                this.comboBox2.Items.Add(i.ToString()+":"+MyNrf.Form1.ParValue[i].Name);            
            }
            if (MyNrf.Form1.LogLine < this.comboBox2.Items.Count) 
            {
                this.comboBox2.Text = this.comboBox2.Items[MyNrf.Form1.LogLine].ToString();
            }
            this.textBox2.Text = (string)Form1.wavexlable.Value;
            this.textBox3.Text = (string)Form1.waveylable.Value;
            this.checkBox4.Checked = (bool)Form1.wavemode.Value;
        }

        private void myButton1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MyNrf.Form1.Pit_Map = this.checkBox1.Checked;
                MyNrf.Form1.XmlFileWrite(new XmlInfo("图形坐标轴辅助显示开关", MyNrf.Form1.Pit_Map.ToString()), false);
                MyNrf.Form1.Pit_Play_Speed = trackBar1.Value;
                MyNrf.Form1.XmlFileWrite(new XmlInfo("图像播放速度", MyNrf.Form1.Pit_Play_Speed.ToString()), false);
                MyNrf.Form1.XmlFileWrite(new XmlInfo("无线频道", MyNrf.Form1.NrfChannel), false);
                MyNrf.Form1.XmlFileWrite(new XmlInfo("波形数据长度", this.numericUpDown1.Value.ToString()), false);
                MyNrf.Form1.WaveDataLength = this.numericUpDown1.Value;
                 MyNrf.Form1.PolyKey= this.checkBox2.Checked;
                 MyNrf.Form1.Polyfit = (int)this.numericUpDown2.Value;
                 MyNrf.Form1.LogKey = this.checkBox3.Checked;
                 MyNrf.Form1.XmlFileWrite(new XmlInfo("中线回归开关", MyNrf.Form1.PolyKey.ToString()), false);
                 MyNrf.Form1.XmlFileWrite(new XmlInfo("中线回归次数", this.numericUpDown2.Value.ToString()), false);
                 MyNrf.Form1.XmlFileWrite(new XmlInfo("打脚开关", MyNrf.Form1.LogKey.ToString()), false);
                 MyNrf.Form1.XmlFileWrite(new XmlInfo("打脚行", MyNrf.Form1.LogLine.ToString()), false);
                 MyNrf.Form1.XmlFileWrite(new XmlInfo("横坐标名称", this.textBox2.Text), false);
                 MyNrf.Form1.XmlFileWrite(new XmlInfo("纵坐标名称", this.textBox3.Text), false);
                 MyNrf.Form1.XmlFileWrite(new XmlInfo("波形从左向右开关",this.checkBox4.Checked.ToString()), false);
                 Form1.wavexlable.Value=this.textBox2.Text;
                 Form1.waveylable.Value=this.textBox3.Text;
                 Form1.wavemode.Value =this.checkBox4.Checked ;
                 this.Close();
            }
        }

        private void MySetup_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePosition.X = e.X;
                this.mousePosition.Y = e.Y;
            }

        }
        private void MySetup_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - mousePosition.Y - SystemInformation.FrameBorderSize.Height;
                this.Left = Control.MousePosition.X - mousePosition.X - SystemInformation.FrameBorderSize.Width;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "请选择输出目录";
            folderBrowserDialog1.SelectedPath = MyNrf.Form1.Save_File_Address;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                MyNrf.Form1.Save_File_Address=folderBrowserDialog1.SelectedPath;
                textBox1.Text = MyNrf.Form1.Save_File_Address;
                MyNrf.Form1.XmlFileWrite(new XmlInfo("保存路径", MyNrf.Form1.Save_File_Address), false);
                
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = checkBox1.Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = "↓↓↓播放速度" + trackBar1.Value.ToString() + "ms/f";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyNrf.Form1.NrfChannel = comboBox1.Text;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = checkBox2.Checked;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {  
            string[] tmpValue =this.comboBox2.Text.Split(':');
            MyNrf.Form1.LogLine = int.Parse(tmpValue[0]);      
        }


    }
}
