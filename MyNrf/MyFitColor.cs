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
    public partial class MyFitColor : Form
    {

        public MyFitColor()
        {
            InitializeComponent();


        }

      // private ClassFitColor fitcolor = new ClassFitColor();
       private System.Drawing.Point mousePosition;

       protected override void OnPaint(PaintEventArgs e)
       {
           Graphics g = e.Graphics;
           g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 0), new Point(this.Width - 1, 0));
           g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height - 1));
           g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(this.Width - 1, this.Height - 1), new Point(0, this.Height - 1));
           g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, this.Height - 1), new Point(0, 0));
           g.DrawLine(new Pen(Color.SkyBlue, 1), new Point(0, 30), new Point(this.Width - 1, 30));
       }
       private void MyFitColor_Resize(object sender,EventArgs e) 
       {
            
       }
       private void MyFitColor_Load(object sender, EventArgs e)
       {

           if (MyNrf.Form1.MyGroup == MyResult.摄像头组)
           {
               this.Controls.Add(this.lbl_back);
               this.Controls.Add(this.lbl_Text);
               this.Controls.Add(this.lbl_LS);
               this.Controls.Add(this.lbl_LL);
               this.Controls.Add(this.lbl_LE);
               this.Controls.Add(this.lbl_CL);
               this.Controls.Add(this.lbl_RS);
               this.Controls.Add(this.lbl_RL);
               this.Controls.Add(this.lbl_RE);
               this.label1.Text = "背景";
               this.label2.Text = "左始";
               this.label3.Text = "左线";
               this.label4.Text = "左末";
               this.label5.Text = "中线";
               this.label6.Text = "右始";
               this.label7.Text = "右线";
               this.label8.Text = "右末";
               this.label9.Text = "文字";
               lbl_back.MyColor = Form1.FitColor.Back;
               lbl_LS.MyColor = Form1.FitColor.LS;
               lbl_LL.MyColor = Form1.FitColor.LL;
               lbl_LE.MyColor = Form1.FitColor.LE;
               lbl_CL.MyColor = Form1.FitColor.CL;
               lbl_RS.MyColor = Form1.FitColor.RS;
               lbl_RL.MyColor = Form1.FitColor.RL;
               lbl_RE.MyColor = Form1.FitColor.RE;
               lbl_Text.MyColor = Form1.FitColor.Text;
           }
           if (MyNrf.Form1.MyGroup == MyResult.光电组) 
           {
               this.Controls.Add(this.lbl_back);
               this.Controls.Add(this.lbl_Text);
               this.Controls.Add(this.lbl_CCD1);
               this.Controls.Add(this.lbl_CCD2);
               this.Controls.Add(this.lbl_CCD3);
               this.Controls.Add(this.lbl_CCDC);
               this.Controls.Add(this.lbl_CCDL);
               this.Controls.Add(this.lbl_CCDR);
               this.Controls.Add(this.lbl_CCDP);
               this.label1.Text = "背景";
               this.label2.Text = "C1";
               this.label3.Text = "C2";
               this.label4.Text = "C3";
               this.label5.Text = "中线";
               this.label6.Text = "左线";
               this.label7.Text = "右线";
               this.label8.Text = "标记";
               this.label9.Text = "文字";
               lbl_back.MyColor = Form1.CCDColor.Back;
               lbl_Text.MyColor = Form1.CCDColor.Text;
               lbl_CCD1.MyColor = Form1.CCDColor.CCD1;
               lbl_CCD2.MyColor = Form1.CCDColor.CCD2;
               lbl_CCD3.MyColor = Form1.CCDColor.CCD3;
               lbl_CCDC.MyColor = Form1.CCDColor.CCDC;
               lbl_CCDL.MyColor = Form1.CCDColor.CCDL;
               lbl_CCDR.MyColor = Form1.CCDColor.CCDR;
               lbl_CCDP.MyColor = Form1.CCDColor.CCDP;
           }
           this.Invalidate();
       }
       private void myButton1_Click(object sender, MouseEventArgs e)
       {
           if (e.Button == MouseButtons.Left)
           {
               if (MyNrf.Form1.MyGroup == MyResult.摄像头组)
               {
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图颜色", "TRUE"), true);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图背景颜色", System.Drawing.ColorTranslator.ToHtml(lbl_back.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图左线开始颜色", System.Drawing.ColorTranslator.ToHtml(lbl_LS.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图左线颜色", System.Drawing.ColorTranslator.ToHtml(lbl_LL.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图左线结束颜色", System.Drawing.ColorTranslator.ToHtml(lbl_LE.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图中线颜色", System.Drawing.ColorTranslator.ToHtml(lbl_CL.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图右线开始颜色", System.Drawing.ColorTranslator.ToHtml(lbl_RS.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图右线颜色", System.Drawing.ColorTranslator.ToHtml(lbl_RL.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图右线结束颜色", System.Drawing.ColorTranslator.ToHtml(lbl_RE.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("拟图文字颜色", System.Drawing.ColorTranslator.ToHtml(lbl_Text.MyColor)), false);
                   Form1.FitColor.Back = lbl_back.MyColor;
                   Form1.FitColor.LS = lbl_LS.MyColor;
                   Form1.FitColor.LL = lbl_LL.MyColor;
                   Form1.FitColor.LE = lbl_LE.MyColor;
                   Form1.FitColor.CL = lbl_CL.MyColor;
                   Form1.FitColor.RS = lbl_RS.MyColor;
                   Form1.FitColor.RL = lbl_RL.MyColor;
                   Form1.FitColor.RE = lbl_RE.MyColor;
                   Form1.FitColor.Text = lbl_Text.MyColor;
               }
               if (MyNrf.Form1.MyGroup == MyResult.光电组) 
               {
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD颜色设置", "TRUE"), true);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD背景颜色", System.Drawing.ColorTranslator.ToHtml(lbl_back.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD1颜色", System.Drawing.ColorTranslator.ToHtml(lbl_CCD1.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD2颜色", System.Drawing.ColorTranslator.ToHtml(lbl_CCD2.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD3颜色", System.Drawing.ColorTranslator.ToHtml(lbl_CCD3.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD中线颜色", System.Drawing.ColorTranslator.ToHtml(lbl_CCDC.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD左线颜色", System.Drawing.ColorTranslator.ToHtml(lbl_CCDL.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD右线颜色", System.Drawing.ColorTranslator.ToHtml(lbl_CCDR.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD标记颜色", System.Drawing.ColorTranslator.ToHtml(lbl_CCDP.MyColor)), false);
                   MyNrf.Form1.XmlFileWrite(new XmlInfo("CCD文字颜色", System.Drawing.ColorTranslator.ToHtml(lbl_Text.MyColor)), false);

                   Form1.CCDColor.Back = lbl_back.MyColor;
                   Form1.CCDColor.Text = lbl_Text.MyColor;
                   Form1.CCDColor.CCD1 = lbl_CCD1.MyColor;
                   Form1.CCDColor.CCD2 = lbl_CCD2.MyColor;
                   Form1.CCDColor.CCD3 = lbl_CCD3.MyColor;
                   Form1.CCDColor.CCDC = lbl_CCDC.MyColor;
                   Form1.CCDColor.CCDL = lbl_CCDL.MyColor;
                   Form1.CCDColor.CCDR = lbl_CCDR.MyColor;
                   Form1.CCDColor.CCDP = lbl_CCDP.MyColor;
               }
               this.Dispose();
           }
       }

       private void MyPar_Set_MouseDown(object sender, MouseEventArgs e)
       {
           if (e.Button == MouseButtons.Left)
           {
               Point Mouse_point=new Point();
                Mouse_point.X=Control.MousePosition.X-this.Location.X;
                Mouse_point.Y=Control.MousePosition.Y-this.Location.Y;
                this.mousePosition.X = Mouse_point.X-4;
                this.mousePosition.Y = Mouse_point.Y-4;

                   //this.mousePosition.X = e.X;
                   //this.mousePosition.Y = e.Y;

           }

       }
       private void MyPar_Set_MouseMove(object sender, MouseEventArgs e)
       {

           if (e.Button == MouseButtons.Left)
           {
               this.Top = Control.MousePosition.Y - mousePosition.Y -SystemInformation.FrameBorderSize.Height;
               this.Left = Control.MousePosition.X - mousePosition.X -SystemInformation.FrameBorderSize.Width;

           }
       }


    }
}
