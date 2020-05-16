using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace MyNrf
{
    public partial class MyAGFrom : Form
    {
        AForgeGenetic TPSAForeGenetic = new AForgeGenetic();
        private AutoResetEvent receiveWaiter;
        private bool StopFlag = false;
        public MyAGFrom()
        {
            InitializeComponent();
        }
        private void MyAGFrom_Load(object sender,EventArgs e) 
        {
            receiveWaiter = new AutoResetEvent(true);
        }

        private void InitAForgeGenetic()
        {
            TPSAForeGenetic.initdata((int)this.世代数.Value, 12, (double)this.交叉概率.Value, (double)this.变异概率.Value);
            TPSAForeGenetic.initAForgeGenetic(new double[12] { 0, 1, 0, 240, 0, 32.3, -26.2, 0, -4.83, 0, -17.9, 2.68 },
                new bool[12]{false,false,false,true,false,true,true,false,true,false,true,true});
            this.myAGWave1.datasize = (int)this.世代数.Value;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= (int)this.运行次数.Value; i++) 
            {
               Thread.Sleep(5);
                this.receiveWaiter.WaitOne();
                TPSAForeGenetic.bestindividual();
                TPSAForeGenetic.clear();
                TPSAForeGenetic.generation();

                InDataPoint[] tmp = new InDataPoint[(int)this.世代数.Value];
                TPSAForeGenetic.Result.DataPoint.CopyTo(tmp);
                this.myAGWave1.Data.DataPoint.AddRange(tmp);


                this.myAGWave1.MaxX = TPSAForeGenetic.Result.getMaxX() * 1.5;
                this.myAGWave1.MaxY = TPSAForeGenetic.Result.getMaxY() * 1.5;

                this.backgroundWorker1.ReportProgress(i,0);
                TPSAForeGenetic.updata();
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) 
        {
            MessageBox.Show("OK");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //this.BeginInvoke((EventHandler)(delegate{


                this.myAGWave1.MyWaveShow();

                this.textBox3.Text = TPSAForeGenetic.ToString();

                this.label14.Text = e.ProgressPercentage.ToString();
                this.label17.Text = TPSAForeGenetic.bestOne.varible.ToString();
                if (StopFlag == false)
                {
                    receiveWaiter.Set();
                }
            //}));
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
                MyMathXYData tmpdata = new MyMathXYData();
                while ((s = sr.ReadLine()) != null)
                {
                    //  解析数据
                    TPSAForeGenetic.Add(s);
                }
                InitAForgeGenetic();
                this.myAGWave1.Data = TPSAForeGenetic.Result;
                this.myAGWave1.MaxX = TPSAForeGenetic.Result.getMaxX()*1.5;
                this.myAGWave1.MaxY = TPSAForeGenetic.Result.getMaxY()*1.5;
                this.myAGWave1.MyWaveShow();
                sr.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy == false) 
            {
                 this.backgroundWorker1.RunWorkerAsync();       
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "暂停") 
            {
                      StopFlag = true;
                      button5.Text = "继续";
            }
            else if (button5.Text == "继续")
            {
                StopFlag = false;
                button5.Text = "暂停";
                receiveWaiter.Set();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (StopFlag == true)
            {
                receiveWaiter.Set();
            }
        }
        private void myAGWave1_MouseMove(object sender, MouseEventArgs e) 
        {
            this.label20.Text = (e.X  * 2 * myAGWave1.MaxX/ myAGWave1.Width - myAGWave1.MaxX).ToString();
            this.label21.Text = (myAGWave1.MaxY - e.Y * 2 * myAGWave1.MaxY / myAGWave1.Height).ToString();
        }

        private void 运行次数_ValueChanged(object sender, EventArgs e)
        {
            InitAForgeGenetic();
        }

        private void 变异概率_ValueChanged(object sender, EventArgs e)
        {
            InitAForgeGenetic();
        }

        private void 交叉概率_ValueChanged(object sender, EventArgs e)
        {
            InitAForgeGenetic();
        }

        private void 世代数_ValueChanged(object sender, EventArgs e)
        {
            InitAForgeGenetic();
        }


    }
}
