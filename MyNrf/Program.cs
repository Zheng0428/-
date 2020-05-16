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

namespace MyNrf
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Login MyLogin = new Login();
            //MyLogin.ShowDialog();//显示登陆窗体
            //if (MyLogin.Result != MyResult.NULL)
            //{
            Form1.MyGroup = MyResult.摄像头组;
            Application.Run(new Form1());
            //}
            //MyLogin.Dispose();
        }
    }
}
