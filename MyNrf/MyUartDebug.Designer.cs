namespace MyNrf
{
    partial class MyUartDebug
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
            this.components = new System.ComponentModel.Container();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.发送命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waittingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readParToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.initParLenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeParToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopCarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ucStopCarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rstChipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nrfErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readCmdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label7 = new MyNrf.label();
            this.label6 = new MyNrf.label();
            this.label5 = new MyNrf.label();
            this.label4 = new MyNrf.label();
            this.button6 = new MyNrf.button();
            this.button5 = new MyNrf.button();
            this.button4 = new MyNrf.button();
            this.button3 = new MyNrf.button();
            this.button2 = new MyNrf.button();
            this.button1 = new MyNrf.button();
            this.myButton1 = new MyNrf.MyButton();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(12, 33);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(976, 527);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";         
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textBox1.Location = new System.Drawing.Point(12, 566);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(976, 17);
            this.textBox1.TabIndex = 5;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoCheck = false;
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton1.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton1.Location = new System.Drawing.Point(829, 593);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(78, 19);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Hex显示";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton1_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "杭电第十届智能车串口调试助手";
            // 
            // comboBox1
            // 
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "9600",
            "115200",
            "256000",
            "640000",
            "750000",
            "960000",
            "1152000",
            "1500000",
            "2250000"});
            this.comboBox1.Location = new System.Drawing.Point(151, 587);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(120, 25);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Text = "640000";
            // 
            // comboBox2
            // 
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16",
            "COM17",
            "COM18"});
            this.comboBox2.Location = new System.Drawing.Point(151, 617);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(120, 25);
            this.comboBox2.TabIndex = 10;
            this.comboBox2.Text = "COM1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.发送命令ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(356, 586);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(123, 28);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 发送命令ToolStripMenuItem
            // 
            this.发送命令ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.发送命令ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.waittingToolStripMenuItem,
            this.writeDataToolStripMenuItem,
            this.readParToolStripMenuItem,
            this.initParLenToolStripMenuItem,
            this.writeParToolStripMenuItem,
            this.stopCarToolStripMenuItem,
            this.ucStopCarToolStripMenuItem,
            this.rstChipToolStripMenuItem,
            this.nrfErrorToolStripMenuItem,
            this.readCmdToolStripMenuItem});
            this.发送命令ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.发送命令ToolStripMenuItem.Name = "发送命令ToolStripMenuItem";
            this.发送命令ToolStripMenuItem.Size = new System.Drawing.Size(111, 24);
            this.发送命令ToolStripMenuItem.Text = "无线主机指令";
            // 
            // waittingToolStripMenuItem
            // 
            this.waittingToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.waittingToolStripMenuItem.Name = "waittingToolStripMenuItem";
            this.waittingToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.waittingToolStripMenuItem.Text = "Waitting";
            this.waittingToolStripMenuItem.Click += new System.EventHandler(this.waittingToolStripMenuItem_Click);
            // 
            // writeDataToolStripMenuItem
            // 
            this.writeDataToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.writeDataToolStripMenuItem.Name = "writeDataToolStripMenuItem";
            this.writeDataToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.writeDataToolStripMenuItem.Text = "Write_Data";
            this.writeDataToolStripMenuItem.Click += new System.EventHandler(this.writeDataToolStripMenuItem_Click);
            // 
            // readParToolStripMenuItem
            // 
            this.readParToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.readParToolStripMenuItem.Name = "readParToolStripMenuItem";
            this.readParToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.readParToolStripMenuItem.Text = "Read_Par";
            this.readParToolStripMenuItem.Click += new System.EventHandler(this.readParToolStripMenuItem_Click);
            // 
            // initParLenToolStripMenuItem
            // 
            this.initParLenToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.initParLenToolStripMenuItem.Name = "initParLenToolStripMenuItem";
            this.initParLenToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.initParLenToolStripMenuItem.Text = "InitParLen";
            this.initParLenToolStripMenuItem.Click += new System.EventHandler(this.initParLenToolStripMenuItem_Click);
            // 
            // writeParToolStripMenuItem
            // 
            this.writeParToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.writeParToolStripMenuItem.Name = "writeParToolStripMenuItem";
            this.writeParToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.writeParToolStripMenuItem.Text = "Write_Par";
            this.writeParToolStripMenuItem.Click += new System.EventHandler(this.writeParToolStripMenuItem_Click);
            // 
            // stopCarToolStripMenuItem
            // 
            this.stopCarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.stopCarToolStripMenuItem.Name = "stopCarToolStripMenuItem";
            this.stopCarToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.stopCarToolStripMenuItem.Text = "Stop_Car";
            this.stopCarToolStripMenuItem.Click += new System.EventHandler(this.stopCarToolStripMenuItem_Click);
            // 
            // ucStopCarToolStripMenuItem
            // 
            this.ucStopCarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.ucStopCarToolStripMenuItem.Name = "ucStopCarToolStripMenuItem";
            this.ucStopCarToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.ucStopCarToolStripMenuItem.Text = "UcStop_Car";
            this.ucStopCarToolStripMenuItem.Click += new System.EventHandler(this.ucStopCarToolStripMenuItem_Click);
            // 
            // rstChipToolStripMenuItem
            // 
            this.rstChipToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.rstChipToolStripMenuItem.Name = "rstChipToolStripMenuItem";
            this.rstChipToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.rstChipToolStripMenuItem.Text = "RstChip";
            this.rstChipToolStripMenuItem.Click += new System.EventHandler(this.rstChipToolStripMenuItem_Click);
            // 
            // nrfErrorToolStripMenuItem
            // 
            this.nrfErrorToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.nrfErrorToolStripMenuItem.Name = "nrfErrorToolStripMenuItem";
            this.nrfErrorToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.nrfErrorToolStripMenuItem.Text = "NrfError";
            this.nrfErrorToolStripMenuItem.Click += new System.EventHandler(this.nrfErrorToolStripMenuItem_Click);
            // 
            // readCmdToolStripMenuItem
            // 
            this.readCmdToolStripMenuItem.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.readCmdToolStripMenuItem.Name = "readCmdToolStripMenuItem";
            this.readCmdToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.readCmdToolStripMenuItem.Text = "ReadCmd";
            this.readCmdToolStripMenuItem.Click += new System.EventHandler(this.readCmdToolStripMenuItem_Click);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textBox2.Location = new System.Drawing.Point(482, 593);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(330, 17);
            this.textBox2.TabIndex = 15;
            this.textBox2.Text = "请选择命令";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(93, 593);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "波特率";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(93, 622);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "串口号";
            // 
            // comboBox3
            // 
            this.comboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox3.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(913, 621);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(75, 20);
            this.comboBox3.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(615, 622);
            this.label7.MyText = "";
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 22;
            this.label7.Text = "命令参数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(840, 623);
            this.label6.MyText = "";
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 21;
            this.label6.Text = "无线频道";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(437, 623);
            this.label5.MyText = "";
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "无";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(359, 623);
            this.label4.MyText = "";
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "指令说明：";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(535, 617);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 14;
            this.button6.Text = "发送指令↑";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(278, 618);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "保存";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(278, 589);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "清空";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(913, 589);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "发送数据↑";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 618);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "打开串口";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 589);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "检测串口";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // myButton1
            // 
            this.myButton1.BackColor = System.Drawing.SystemColors.Control;
            this.myButton1.BaseColor = System.Drawing.SystemColors.Control;
            this.myButton1.ButtonStyle = MyNrf.MyButton.Style.Default;
            this.myButton1.ButtonText = null;
            this.myButton1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.myButton1.Location = new System.Drawing.Point(974, 2);
            this.myButton1.Name = "myButton1";
            this.myButton1.Size = new System.Drawing.Size(25, 25);
            this.myButton1.TabIndex = 0;
            this.myButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myButton1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myButton1_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 4;
            this.numericUpDown1.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDown1.Location = new System.Drawing.Point(732, 619);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(102, 24);
            this.numericUpDown1.TabIndex = 24;
            this.numericUpDown1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.numericUpDown2.Location = new System.Drawing.Point(688, 619);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(38, 24);
            this.numericUpDown2.TabIndex = 25;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // MyUartDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 648);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.myButton1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MyUartDebug";
            this.Text = "MyUartDebug";
            this.Load += new System.EventHandler(this.MyUartDebug_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Top_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Top_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MyButton myButton1;
        private button button1;
        private button button2;
        private button button3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.IO.Ports.SerialPort serialPort1;
        private button button4;
        private button button5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 发送命令ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waittingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem writeDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readParToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem initParLenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem writeParToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopCarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ucStopCarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rstChipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nrfErrorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readCmdToolStripMenuItem;
        private button button6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private label label4;
        private label label5;
        private System.Windows.Forms.ComboBox comboBox3;
        private label label6;
        private label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
    }
}