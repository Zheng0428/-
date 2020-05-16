namespace MyNrf
{
    partial class MyAGFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.世代数 = new System.Windows.Forms.NumericUpDown();
            this.变异概率 = new System.Windows.Forms.NumericUpDown();
            this.交叉概率 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.随机数种子 = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.运行次数 = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label14 = new MyNrf.label();
            this.button5 = new MyNrf.button();
            this.button4 = new MyNrf.button();
            this.button3 = new MyNrf.button();
            this.button2 = new MyNrf.button();
            this.button1 = new MyNrf.button();
            this.myAGWave1 = new MyNrf.MyAGWave();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.世代数)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.变异概率)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.交叉概率)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.随机数种子)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.运行次数)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // 世代数
            // 
            this.世代数.Location = new System.Drawing.Point(265, 462);
            this.世代数.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.世代数.Name = "世代数";
            this.世代数.Size = new System.Drawing.Size(90, 21);
            this.世代数.TabIndex = 3;
            this.世代数.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.世代数.ValueChanged += new System.EventHandler(this.世代数_ValueChanged);
            // 
            // 变异概率
            // 
            this.变异概率.DecimalPlaces = 6;
            this.变异概率.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.变异概率.Location = new System.Drawing.Point(94, 407);
            this.变异概率.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.变异概率.Name = "变异概率";
            this.变异概率.Size = new System.Drawing.Size(90, 21);
            this.变异概率.TabIndex = 4;
            this.变异概率.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.变异概率.ValueChanged += new System.EventHandler(this.变异概率_ValueChanged);
            // 
            // 交叉概率
            // 
            this.交叉概率.DecimalPlaces = 6;
            this.交叉概率.Increment = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.交叉概率.Location = new System.Drawing.Point(94, 434);
            this.交叉概率.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.交叉概率.Name = "交叉概率";
            this.交叉概率.Size = new System.Drawing.Size(90, 21);
            this.交叉概率.TabIndex = 5;
            this.交叉概率.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.交叉概率.ValueChanged += new System.EventHandler(this.交叉概率_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(190, 379);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "运行世代数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(190, 411);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "变异概率";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(190, 439);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "交叉概率";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(346, 379);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "初始个体染色体";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(346, 434);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "最佳个体染色体";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(346, 409);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 24;
            this.label6.Text = "个体染色体排列";
            // 
            // 随机数种子
            // 
            this.随机数种子.DecimalPlaces = 6;
            this.随机数种子.Increment = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.随机数种子.Location = new System.Drawing.Point(94, 462);
            this.随机数种子.Name = "随机数种子";
            this.随机数种子.Size = new System.Drawing.Size(90, 21);
            this.随机数种子.TabIndex = 41;
            this.随机数种子.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(190, 466);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 42;
            this.label12.Text = "随机数种子";
            // 
            // 运行次数
            // 
            this.运行次数.Location = new System.Drawing.Point(94, 375);
            this.运行次数.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.运行次数.Name = "运行次数";
            this.运行次数.Size = new System.Drawing.Size(90, 21);
            this.运行次数.TabIndex = 44;
            this.运行次数.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.运行次数.ValueChanged += new System.EventHandler(this.运行次数_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(361, 466);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 45;
            this.label15.Text = "个体数量";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(445, 466);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 46;
            this.label16.Text = "最佳个体值：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(521, 466);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(0, 12);
            this.label17.TabIndex = 47;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(671, 469);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(11, 12);
            this.label18.TabIndex = 48;
            this.label18.Text = "X";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(801, 469);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(11, 12);
            this.label19.TabIndex = 49;
            this.label19.Text = "Y";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(688, 469);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(0, 12);
            this.label20.TabIndex = 50;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(818, 469);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(0, 12);
            this.label21.TabIndex = 51;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 466);
            this.label14.MyText = "";
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(0, 12);
            this.label14.TabIndex = 43;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(265, 375);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 11;
            this.button5.Text = "暂停";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(265, 404);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "下一步";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 434);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "重置算法";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 404);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "开始计算";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 374);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "导入数据";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // myAGWave1
            // 
            this.myAGWave1.Location = new System.Drawing.Point(0, 0);
            this.myAGWave1.Name = "myAGWave1";
            this.myAGWave1.Size = new System.Drawing.Size(970, 368);
            this.myAGWave1.TabIndex = 0;
            this.myAGWave1.Text = "myAGWave1";
            this.myAGWave1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.myAGWave1_MouseMove);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(441, 375);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(517, 21);
            this.textBox1.TabIndex = 52;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(441, 403);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(517, 21);
            this.textBox2.TabIndex = 53;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(441, 431);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(517, 21);
            this.textBox3.TabIndex = 54;
            // 
            // MyAGFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 504);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.运行次数);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.随机数种子);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.交叉概率);
            this.Controls.Add(this.变异概率);
            this.Controls.Add(this.世代数);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.myAGWave1);
            this.Name = "MyAGFrom";
            this.Text = "MyAGFrom";
            this.Load += new System.EventHandler(this.MyAGFrom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.世代数)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.变异概率)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.交叉概率)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.随机数种子)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.运行次数)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MyAGWave myAGWave1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private button button1;
        private button button2;
        private System.Windows.Forms.NumericUpDown 世代数;
        private System.Windows.Forms.NumericUpDown 变异概率;
        private System.Windows.Forms.NumericUpDown 交叉概率;
        private button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private button button4;
        private button button5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown 随机数种子;
        private System.Windows.Forms.Label label12;
        private label label14;
        private System.Windows.Forms.NumericUpDown 运行次数;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
    }
}