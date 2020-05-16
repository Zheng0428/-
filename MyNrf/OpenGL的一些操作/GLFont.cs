using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using MyNrf.WIN32所需要使用的API;
using CsGL.OpenGL;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Globalization;
using System.Diagnostics;

namespace MyNrf.OpenGL的一些操作
{
    /// <summary>
    /// opengl文字显示   比较高效   支持3D
    /// </summary>
    public class GLFont
    {
        #region 私有字段
        private IntPtr hDC = Win32.GetDC(IntPtr.Zero);             //暂存与交换字体句柄
        private IntPtr m_hFont;         //新字体句柄
        private uint MAX_CHAR = 255;
        private uint lists = 1000;
        #endregion

        #region 构造函数

        public GLFont()
        {

        }

        #endregion

        #region  方法

        #region 初始化字体
        //初始化字体
        private bool InitFont()
        {
            try
            {
                m_hFont = Win32.CreateFont(16,
                                  0,
                                  0,
                                  0,
                                  Win32.FW_DONTCARE,
                                  0,
                                  0,
                                  0,
                                  Win32.DEFAULT_CHARSET,
                                  Win32.OUT_OUTLINE_PRECIS,
                                  Win32.CLIP_DEFAULT_PRECIS,
                                  Win32.CLEARTYPE_QUALITY,
                                  Win32.VARIABLE_PITCH,
                                  "Comic Sans MS");

                IntPtr hOldFont = Win32.SelectObject(hDC, m_hFont);//选择字体，得到老字体

                bool b = Win32.DeleteObject(hOldFont);//删除老字体(用新字体替换老字体),这里的bool b只是为了调试的时候监视一下，该不成功照样不成功- -!

                return b;
            }
            catch (Exception)
            { return false; }
        }

        #endregion

        #region 输出文字

        /// <summary>
        /// 只是输出文字，没有字体，不支持中文
        /// </summary>
        /// <param name="str">内容(英文)</param>
        /// <param name="locationX">x坐标</param>
        /// <param name="locationY">y坐标</param>
        /// <param name="locationZ">z坐标</param>
        public void Print(string str, float locationX, float locationY, float locationZ)
        {
            // 申请MAX_CHAR个连续的显示列表编号
            lists = GL.glGenLists((int)MAX_CHAR);
            // 把每个字符的绘制命令都装到对应的显示列表中

            bool b = Win32.wglUseFontBitmaps(hDC, 0, MAX_CHAR, lists); //这里的bool b只是为了调试的时候监视一下，该不成功照样不成功- -!

            GL.glRasterPos3f(locationX, locationY, locationZ);
            for (int i = 0; i < str.Length; i++)
            {
                GL.glCallList((lists + str[i]));
            }
        }

        #endregion

        /// <summary>
        /// 带字体样式输出，仅英文
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="font">字体</param>
        /// <param name="locationX">x坐标</param>
        /// <param name="locationY">y坐标</param>
        /// <param name="locationZ">z坐标</param>
        public void StylePrint(string str, Font font, float locationX, float locationY, float locationZ)
        {
            InitFont();
            Print(str, locationX, locationY, locationZ);
        }

        /// <summary>
        /// 输出中文并带字体样式
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="font">字体</param>
        /// <param name="locationX">x坐标</param>
        /// <param name="locationY">y坐标</param>
        /// <param name="locationZ">z坐标</param>
        public void PrintCN(string str, Font font, float locationX, float locationY, float locationZ)
        {
            //应用字体
            InitFont();
            lists = GL.glGenLists(1);
           
            GL.glRasterPos3f(locationX, locationY, locationZ);

            for (int i = 0; i < str.Length; i++)
            {
                //只是为了调试的bool b
                bool b = Win32.wglUseFontBitmapsW(hDC, (uint)(str[i]), 1, lists);//一定要注意这里调用的不一样
                GL.glCallList(lists);
            }
        }


        #endregion

    }

    class UnicodeChars
    {
        
        private UnicodeChar[] chars = new UnicodeChar[65536];
        int cNumber = 0;
        uint[] texture = new uint[1];
        byte[] data;
        string FontName = "微软雅黑";
        public CharInfo GetChar(ushort UnCode)
        {
            int W = 32, H = 32;
            if (chars[UnCode] != null)
            {
                if (!chars[UnCode].Textrue.Used)
                {
                    GL.glBindTexture(GL.GL_TEXTURE_2D, chars[UnCode].Textrue.TextrueNum);
                    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGBA, W, H, 0, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, chars[UnCode].Textrue.date);
                    GL.glTexParameterf(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
                    GL.glTexParameterf(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
                    chars[UnCode].Textrue.Used = true;
                }
                return chars[UnCode].Textrue;
            }
            chars[UnCode] = new UnicodeChar();
            uint[] texture = new uint[1];
            GL.glGenTextures(1, texture);//位纹理分配纹理内存
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            //drawingContext.DrawRectangle(new SolidColorBrush(Colors.White), new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 1), new System.Windows.Rect(0,0,W,H));
            // Draw a formatted text string into the DrawingContext.  
            byte[] ch = BitConverter.GetBytes(UnCode);
            string s = ASCIIEncoding.Unicode.GetString(ch);
            FormattedText formtext = new FormattedText(s, CultureInfo.GetCultureInfo("zh-CHS"), System.Windows.FlowDirection.LeftToRight,
                new Typeface(FontName), 24, System.Windows.Media.Brushes.White);

            drawingContext.DrawText(formtext, new System.Windows.Point(0, 0));
            // Persist the drawing content.  
            drawingContext.Close();
            RenderTargetBitmap rtb = new RenderTargetBitmap(W, H, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);
            data = new byte[4 * W * H];

           rtb.CopyPixels(data, 4 * W, 0);
           for (int m = 0; m < W; m++)//为了得到白色背景  将显存中数据调出进行RGBA逆转
           {
               for (int n = 0; n < W; n++)
               {
                   data[(m * H + n) * 4 + 3] =(byte)( (data[(m * H + n) * 4] + data[(m * H + n) * 4 + 1] + data[(m * H + n) * 4 + 2])/3);//先让背景变透明
                   data[(m * H + n) * 4] = (byte)(255);//R
                   data[(m * H + n) * 4 + 1] = (byte)(255);//G
                   data[(m * H + n) * 4 + 2] = (byte)(255);//B
               }
           }
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture[0]);//绑定纹理
            GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGBA, W, H, 0, GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, data);//为当前纹理设置像素数据
            GL.glTexParameterf(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);//线性滤波
            GL.glTexParameterf(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);//线性滤波

            chars[UnCode].Textrue.Used = true;

            chars[UnCode].Textrue.TextrueNum = texture[0];
            chars[UnCode].Textrue.Width = (float)formtext.Width;
            chars[UnCode].Textrue.Height = (float)formtext.Height;
            cNumber++;

            return chars[UnCode].Textrue;
        }

    }

    public class CharInfo
    {
        public uint TextrueNum;
        public float Width;
        public float Height;
        public byte[] date;
        public bool Used = false;
    }
    class UnicodeChar
    {
        public CharInfo Textrue = new CharInfo();
    }

    public class Po3D
    {
        public double x, y, z;

        public Po3D(double X, double Y, double Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }
    public class TextrueUnicode
    {

        UnicodeChars chars;
        public System.Drawing.Point Start = new System.Drawing.Point(400, 300);     //更改开始位置
        public float Height = 40;       //更改字体大小
        double Nowx, Nowy;//Nowz;
        /// <summary>
        ///  更改行距
        /// </summary>
        public double HangJu = 10;   
        /// <summary>
        /// 更改字间距
        /// </summary>
        public double JianJu = 2;      

        public TextrueUnicode()
        {
            chars = new UnicodeChars();
        }

        private void DrawChar(ushort c)
        {
            byte[] ch = BitConverter.GetBytes(c);
            string s = ASCIIEncoding.Unicode.GetString(ch);
            if (s == "\n")
            {
                Nowy = Nowy - Height - HangJu;
                Nowx = Start.X;
                return;
            }
            else if (s == " ")
            {
                Nowx += Height / 2;
            }
            CharInfo ci = chars.GetChar(c);
            double dx = ci.Width / 32;
            double dy = ci.Height / 32;
            double Width = Height * dx / dy;

            Nowx += Width;
             
            GL.glBindTexture(GL.GL_TEXTURE_2D, ci.TextrueNum);

            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2d(dx, 0.0f); GL.glVertex2d(Nowx, Height + Nowy); 	// Bottom Left Of The Texture and Quad
            GL.glTexCoord2d(dx, dy); GL.glVertex2d(Nowx, Nowy); 	// Bottom Right Of The Texture and Quad
            GL.glTexCoord2d(0.0f, dy); GL.glVertex2d(Nowx - Width, Nowy);	// Top Right Of The Texture and Quad
            GL.glTexCoord2d(0.0f, 0.0f); GL.glVertex2d(Nowx - Width, Height + Nowy);	// Top Left Of The Texture and Quad
            GL.glEnd();
            Nowx += JianJu;
        }
        private void DrawCharL(ushort c)
        {
            //byte[] ch = BitConverter.GetBytes(c);
            //string s = ASCIIEncoding.Unicode.GetString(ch);
            //if (s == "\n")
            //{
            //    Nowy = Nowy - Height - HangJu;
            //    Nowx = Start.X;
            //    return;
            //}
            //else if (s == " ")
            //{
            //    Nowy += Height / 2;
            //}
            //CharInfo ci = chars.GetChar(c);
            //double dx = ci.Width / 32;
            //double dy = ci.Height / 32;
            //double Width = Height * dx / dy;

            //Nowy += Width;

            //GL.glBindTexture(GL.GL_TEXTURE_2D, ci.TextrueNum);

            //GL.glBegin(GL.GL_QUADS);
            //GL.glTexCoord2d(dx, 0.0f); GL.glVertex2d(Nowx - Height, Width + Nowy);	// Top Left Of The Texture and Quad
            //GL.glTexCoord2d(dx, dy); GL.glVertex2d(Nowx, Width + Nowy); 	// Bottom Left Of The Texture and Quad
            //GL.glTexCoord2d(0.0f, dy); GL.glVertex2d(Nowx, Nowy); 	// Bottom Right Of The Texture and Quad
            //GL.glTexCoord2d(0.0f, 0.0f); GL.glVertex2d(Nowx - Height, Nowy);	// Top Right Of The Texture and Quad
            //GL.glEnd();
            //Nowy += JianJu;
            byte[] ch = BitConverter.GetBytes(c);
            string s = ASCIIEncoding.Unicode.GetString(ch);
            if (s == "\n")
            {
                Nowx = Nowx - Height - HangJu;
                Nowx = Start.X;
                return;
            }
            else if (s == " ")
            {
                Nowy += Height / 2;
            }
            CharInfo ci = chars.GetChar(c);
            double dx = ci.Width / 32;
            double dy = ci.Height / 32;
            double Width = Height * dx / dy;

            Nowy += Width;

            GL.glBindTexture(GL.GL_TEXTURE_2D, ci.TextrueNum);

            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2d(dx, 0.0f);GL.glVertex2d(Nowx, Nowy); 	// Bottom Right Of The Texture and Quad
            GL.glTexCoord2d(dx, dy); GL.glVertex2d(Nowx + Height, Nowy);	// Top Right Of The Texture and Quad
            GL.glTexCoord2d(0.0f, dy);  GL.glVertex2d(Nowx + Height, -Width + Nowy);	// Top Left Of The Texture and Quad
            GL.glTexCoord2d(0.0f, 0.0f); GL.glVertex2d(Nowx, -Width + Nowy); 	// Bottom Left Of The Texture and Quad
            GL.glEnd();
            Nowy += JianJu;
        }
        //绘制字符：Unicode字符，包括空格、回车，可设置起始位置、颜色、行距、字距、字体大小
        //性能分析：每10ms绘制2000字 每秒20，0000字
        public void DrawText(string Text, System.Drawing.Point sPoint, System.Drawing.Color color, float alpha)
        {
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(color, alpha));
            GL.glEnable(GL.GL_TEXTURE_2D);
            Start = sPoint;
            Nowx = Start.X;
            Nowy = Start.Y;
            foreach (ushort ct in Text)
            {
                DrawChar(ct);
            }
            GL.glDisable(GL.GL_TEXTURE_2D);
            Start.X = (int)Nowx + 10;
            Start.Y = (int)Nowy;
        }

        //绘制字符：Unicode字符，包括空格、回车，可设置起始位置、颜色、行距、字距、字体大小
        //性能分析：每10ms绘制2000字 每秒20，0000字
        public void DrawText(string Text, System.Drawing.Point sPoint, float TextHeight, System.Drawing.Color color, float alpha)
        {
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(color, alpha));
            GL.glEnable(GL.GL_TEXTURE_2D);
            Start = sPoint;
            Height = TextHeight;
            Nowx = Start.X;
            Nowy = Start.Y;
            foreach (ushort ct in Text)
            {
                DrawChar(ct);
            }
            GL.glDisable(GL.GL_TEXTURE_2D);
            Start.X = (int)Nowx + 10;
            Start.Y = (int)Nowy;
        }
        public void DrawTextL(string Text, System.Drawing.Point sPoint, float TextHeight, System.Drawing.Color color, float alpha )
        {
            GL.glColor4fv(OpenGLPublicFunction.GetGLColorFromDotNetColor(color, alpha));
            GL.glEnable(GL.GL_TEXTURE_2D);
            Start = sPoint;
            Height = TextHeight;
            Nowx = Start.X;
            Nowy = Start.Y;
            foreach (ushort ct in Text)
            {
                DrawCharL(ct);
            }
            GL.glDisable(GL.GL_TEXTURE_2D);
            Start.X = (int)Nowx + 10;
            Start.Y = (int)Nowy;
        }
        //确定要绘制的字符串的宽、高信息
        public System.Drawing.Rectangle VisulDraw(string Text)
        {
            int Sx = 0, Sy = 0, Ex = 0, Ey = 0;

            foreach (ushort c in Text)
            {
                byte[] ch = BitConverter.GetBytes(c);
                string s = ASCIIEncoding.Unicode.GetString(ch);
                if (s == " ")
                {
                    Ex += (int)(Height / 2);
                }
                CharInfo ci = chars.GetChar(c);
                double dx = ci.Width / 32;
                double dy = ci.Height / 32;
                double Width = Height * dx / dy;

                Ex += (int)Width;
                Ex += (int)JianJu;
            }
            Ey = (int)Height;
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle(Sx, Sy, Ex, Ey);
            return rec;
        }
    }






}
