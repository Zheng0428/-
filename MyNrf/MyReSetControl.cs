using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using MyNrf.Properties;
using System.Reflection;

namespace MyNrf
{
    public class button : Button
    {
        public button()
        {
            this.MouseMove += new MouseEventHandler(Button_MouseMove);
            this.MouseLeave += new EventHandler(Button_MouseLeave);

        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {

            //this.Cursor = Form1.MyCursor.Choose;
        }
        private void Button_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Default;
        }
    }
    public class menustrip : MenuStrip 
    {
        public menustrip()
        {
            this.MouseMove += new MouseEventHandler(menustrip_MouseMove);
            this.MouseLeave += new EventHandler(menustrip_MouseLeave);
        }

        private void menustrip_MouseMove(object sender, MouseEventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Choose;
        }
        private void menustrip_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Default;
        }
    }

    public class combobox : ComboBox 
    {
        public combobox()
        {
            this.MouseMove += new MouseEventHandler(combobox_MouseMove);
            this.MouseLeave += new EventHandler(combobox_MouseLeave);

        }
        private void combobox_MouseMove(object sender, MouseEventArgs e)
        {

            //this.Cursor = Form1.MyCursor.Choose;
        }
        private void combobox_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Default;
        }
    }

    public class label : Label
    {
        public string mytext = String.Empty;
        /// <summary>
        /// 当Mytext值改变时发生
        /// </summary>
        //public event EventHandler MytextChanged=null;
        public string MyText
        {
            get
            {
                return mytext;
            }
            set
            {

                mytext = value;
                this.OnMytextChanged(new EventArgs());//值改变时触发事件
            }
        }
        public label()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.MouseMove += new MouseEventHandler(label_MouseMove);
            this.MouseLeave += new EventHandler(label_MouseLeave);
        }
        private void OnMytextChanged(EventArgs eventArgs)
        {
            //if (this.PropertyChanged != null)//判断事件是否有处理函数 
            //{
            //    this.MytextChanged(this, eventArgs);
            //}
            this.Text = mytext;
        }
        private void label_MouseMove(object sender, MouseEventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Default;
        }
        private void label_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Form1.MyCursor.Default;
        }
        public void AppendText(string value_text)
        {
            this.Text += value_text;
        }
        public void Clear() 
        {
            this.Text = string.Empty;
        }

    }

    public class TestRichTextBox :RichTextBox
    {
        private const int WM_SETFOCUS = 0x7;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        public bool Down = false;
        private bool Down_flag = false;
        public TestRichTextBox()
        {
            //this.Cursor = Form1.MyCursor.Default;//设置鼠标样式  
            this.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
        }


        //protected override void WndProc(ref Message m)
        //{
        //    if (Down == true && Down_flag == false)
        //    {
        //        Down_flag = true;
        //        m.Msg = WM_SETFOCUS;
        //        base.WndProc(ref m);
        //        return;
        //    }
        //    if (m.Msg == WM_SETFOCUS && Down == false)
        //    {
        //        return;
        //    }
        //    base.WndProc(ref m);
        //}
    }




}
