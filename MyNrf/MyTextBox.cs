using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace MyNrf
{

    public partial class MyTextBox : Form
    {
        private System.Drawing.Point mousePosition;
        //[DllImport("user32", EntryPoint = "HideCaret")]
        //private static extern bool HideCaret(IntPtr hWnd);
        public MyTextBox()
        {
            InitializeComponent();
        }

        private void MyTextBox_Load(object sender, EventArgs e) 
        {
            this.Invalidate();
            this.Cursor = Form1.MyCursor.Default;//设置鼠标样式  
            this.richTextBox1.Down = true;
        }
        private void MyTextBox_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        public void MyTextShow(string value, bool mode) 
        {
            if (mode == true)
            {
                richTextBox1.AppendText(value);
            }
            else 
            {
                richTextBox1.Text =value;
            }

        }
        protected override void OnPaint(PaintEventArgs e) 
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 28), new Point(this.Width, 28));
            g.DrawLine(new Pen(Color.SkyBlue,1),new Point(0,0),new Point(this.Width,0));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, this.Height - 1), new Point(0, this.Height - 1));
            g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, this.Height - 1), new Point(0, 0));
        }

        private void myButton1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            {
                this.Close();
            }
        }
        private void MyTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePosition.X = e.X;
                this.mousePosition.Y = e.Y;
            }

        }
        private void MyTextBox_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - mousePosition.Y;
                this.Left = Control.MousePosition.X - mousePosition.X;
            }
        }
        private void richTextBox1_MouseDown(object sender, MouseEventArgs e) 
        {
            num = this.richTextBox1.SelectionStart;
             //HideCaret(((RichTextBox)sender).Handle);
           //  this.Cursor = Form1.MyCursor.Text;//设置鼠标样式  
        }

        private void richTextBox1_GotFocus(object sender, EventArgs e) 
        {
          //  HideCaret(((RichTextBox)sender).Handle);
        }
        private void richTextBox1_MouseUP(object sender, MouseEventArgs e) 
        {
           // this.Cursor = Form1.MyCursor.Default;//设置鼠标样式  
        }
        private  int num=0;
        private bool num_flag = false;
        //private void richTextBox1_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    if (e.Delta > 0)
        //    {
        //        if (num_flag == true) 
        //        {
        //            num -= 150;
        //            num_flag = false;  
        //        }
        //        num -= 50;
        //    }
        //    if (e.Delta < 0)
        //    {
        //        if (num_flag == false) 
        //        {
        //            num += 150;
        //            num_flag = true;
        //        }
        //        num += 50;
        //    }
        //    if (num < 0) { num = 0; }
        //    if (num > this.richTextBox1.Text.Length) { num = this.richTextBox1.Text.Length; }
        //    //this.richTextBox1.ScrollBars();// = num;
        //    //this.richTextBox1.SelectionStart = num;
        //   // HideCaret(((RichTextBox)sender).Handle);
        //}
    }
}
