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
    /// <summary> 
    /// 保存控制参数的类
    /// Name 控制参数名
    /// LengthIndex 控制参数长度
    /// TypeIndex 控制参数类型
    /// Value 控制参数值
    /// ShutBool控制参数开关
    /// </summary>
    public class ClassConstParValue //保存控制参数的类
    {
        public string Name;
        public int LengthIndex;
        public int TypeIndex;
        public int SubIndex;
        public float Value;
        public bool ShutBool;
        public Color color;
        public bool waveon;

        public ClassConstParValue() { }

        public ClassConstParValue(string name, int type, int length, int sub, float value, bool shutbool, Color col)
        {
            Name = name;
            TypeIndex = type;
            LengthIndex = length;
            SubIndex = sub;
            Value = value;
            ShutBool = shutbool;
            color = col;
        }
    }
    public class CopyDataInfo 
    {
        public string Name;
        public bool ChooseOn;
        public CopyDataInfo(string name) 
        {
            this.Name = name;
            ChooseOn = false;
        }
    }
    /// <summary> 
    /// 保存控制参数的类
    /// Name 控制参数名
    /// LengthIndex 控制参数长度
    /// TypeIndex 控制参数类型
    /// WaveColor 控制参数显示颜色
    /// WaveMulIndex 控制参数显示比例增益
    /// WaveOn 控制参数显示开关
    /// Value 控制参数值
    /// </summary>
    public class ClassParValue //保存控制参数的类
    {
        public string Name;
        public int LengthIndex;
        public int TypeIndex;
        public Color WaveColor;
        public int WaveMulIndex;
        public bool WaveOn;
        public float Value = 0;

        public ClassParValue() { }

        public ClassParValue(string name, int typeIndex, int lengthIndex, Color wavecolor, int wavemulIndex, bool waveon)
        {
            Name = name;
            TypeIndex = typeIndex;
            LengthIndex = lengthIndex;
            WaveColor = wavecolor;
            WaveMulIndex = wavemulIndex;
            WaveOn = waveon;
        }
        public void ClassDataValueClear()
        {
            //Value.RemoveRange(0, Value.Count);
            Value = 0;
        }
    }
    /// <summary>
    /// 存储所有数据
    /// </summary>
    public class DataAll
    {
        public List<float> ParValue = new List<float>();//参数
        public List<float> CoeValue = new List<float>();//固参
        public Bitmap BmtFit;
        public Bitmap BmtReal;
        public string LStart;
        public string LEnd;
        public string LBlack;
        public string RStart;
        public string REnd;
        public string RBlack;
        public string Center;

        public string[] CCDD;

        public string[] CCDLRCD;



        public DataAll()
        { }
        /// <summary>
        /// 保存参数图像
        /// </summary>
        /// <param name="ParValue"></param>
        /// <param name="ParNum"></param>
        /// <param name="CoeValue"></param>
        /// <param name="CoeNum"></param>
        /// <param name="BmtFit"></param>
        /// <param name="BmtReal"></param>
        /// <param name="L"></param>
        public DataAll(List<ClassParValue> ParValue, int ParNum, List<ClassParValue> CoeValue, int CoeNum, Bitmap BmtFit, Bitmap BmtReal,List<LCR> L)
        {

            for (int i = 0; i < ParNum; i++)
            {
                this.ParValue.Add(ParValue[i].Value);
            }
            for (int i = 0; i < CoeNum; i++)
            {
                this.CoeValue.Add(CoeValue[i].Value);
            }
            this.BmtFit = new Bitmap(BmtFit);
            this.BmtReal =  new Bitmap(BmtReal);
            for (int i = 0; i < 70; i++) 
            {
                LStart += L[i].LStart.ToString()+" ";
                LEnd += L[i].LEnd.ToString() + " ";
                LBlack += L[i].LBlack.ToString() + " ";
                RStart += L[i].RStart.ToString() + " ";
                REnd += L[i].REnd.ToString() + " ";
                RBlack += L[i].RBlack.ToString() + " ";
                Center += L[i].Center.ToString() + " ";
            }
        }
        /// <summary>
        /// 只保存参数
        /// </summary>
        /// <param name="ParValue"></param>
        /// <param name="ParNum"></param>
        /// <param name="CoeValue"></param>
        /// <param name="CoeNum"></param>
        public DataAll(List<ClassParValue> ParValue, int ParNum, List<ClassParValue> CoeValue, int CoeNum)
        {

            for (int i = 0; i < ParNum; i++)
            {
                this.ParValue.Add(ParValue[i].Value);
            }
            for (int i = 0; i < CoeNum; i++)
            {
                this.CoeValue.Add(CoeValue[i].Value);
            }

        }

        public DataAll(byte CCDWidth, byte CCDHeight, byte[,] CCD, byte[,] CCDLRC)
        {
            CCDD = new string[CCDHeight];
            CCDLRCD = new string[CCDHeight + 1];
            for (int i = 0; i < CCDHeight; i++)
            {
                for (int j = 0; j < CCDWidth; j++)
                {
                    CCDD[i] += CCD[i, j].ToString() + " ";
                }
            }
            for (int i = 0; i < CCDHeight+1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CCDLRCD[i] += CCDLRC[i, j].ToString() + " ";
                }
            }
        }
        public DataAll(byte CCDWidth, byte CCDHeight, byte[,] CCD, byte[,] CCDLRC, List<ClassParValue> ParValue, int ParNum, List<ClassParValue> CoeValue, int CoeNum)
        {
            CCDD = new string[CCDHeight];
            CCDLRCD = new string[CCDHeight + 1];
            for (int i = 0; i < CCDHeight; i++)
            {
                for (int j = 0; j < CCDWidth; j++)
                {
                    CCDD[i] += CCD[i, j].ToString() + " ";
                }
            }
            for (int i = 0; i < CCDHeight + 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CCDLRCD[i] += CCDLRC[i, j].ToString() + " ";
                }
            }
            for (int i = 0; i < ParNum; i++)
            {
                this.ParValue.Add(ParValue[i].Value);
            }
            for (int i = 0; i < CoeNum; i++)
            {
                this.CoeValue.Add(CoeValue[i].Value);
            }
        }
        
    }
    public class ListDataNode
    {
        public float Value = 0;
        public int MulIndex = 1;
        public ListDataNode(float value, int mulindex)
        {
            Value = value;
            MulIndex = mulindex;
        }

    }

    public class WaveData
    {
        public int Num = 0;
        public String Name = "huang";
        public Color Color = Color.Red;
        public int Width = 1; 
        public bool waveon=false;
        public List<ListDataNode> ListData = new List<ListDataNode>();
        public WaveData(int num, String name, Color color, int width)
        {

            Num = num;
            Name = name;
            Color = color;
            Width = width;
            ListData.Clear();
        }
        public void AddNode(ListDataNode Node)
        {
            ListData.Insert(0, Node);
        }
    }
    public class ClassGetObject
    {
        public static uint getKeyEnum(int index)
        {
            switch (index)
            {
                case 0: return 0x0004;
                case 1: return 0x0002;
                case 2: return 0x0001;
                case 3: return 0x0004 | 0x0002;
                case 4: return 0x0004 | 0x0001;
                default: return 0x0002 | 0x0001;
            }
        }




        public static Keys getKeys(int index)
        {
            if (index >= 0 && index < 26)
            {
                return (Keys)(index + 65);
            }
            else if (index >= 26 && index < 38)
            {
                return (Keys)(index - 26 + 112);
            }
            else
            {
                switch (index)
                {
                    case 38: return Keys.Space;
                    case 39: return Keys.Enter;
                    default: return Keys.Space;
                }
            }
        }


        public static NrfMode getNrfStartMode(int index)
        {
            return (NrfMode)index;
        }

        public static int getParDiv(int index)
        {
            return index + 1;
        }

        public static int getXYMul(int index)
        {
            return index + 1;
        }

        public static bool getConstrParTypeBool(int index)
        {
            return index == 0 ? false : true;
        }

        public static int getChannel(int Index)
        {
            return (Index + 1);
        }

        public static int getTimeInterval(int Index)
        {
            switch (Index)
            {
                case 0: return 1;
                case 1: return 10;
                case 2: return 50;
                case 3: return 200;
                case 4: return 1000;
                default: return 1000;
            }
        }

        public static int getValueSub(ref List<ClassConstParValue> ConstParValue, int i)
        {
            switch (ConstParValue[i].SubIndex)
            {
                case 0: return 1;
                case 1: return 2;
                case 2: return 5;
                case 3: return 10;
                case 4: return 20;
                case 5: return 50;
                case 6: return 100;
                case 7: return 200;
                case 8: return 500;
                case 9: return 1000;
                default: return 1;
            }
        }

        public static int getMaxValue(ref List<ClassConstParValue> ConstParValue, int i)
        {
            if (ConstParValue[i].TypeIndex == 0)
            {
                if (ConstParValue[i].LengthIndex == 0)
                {
                    return (1 << 8) - 1;
                }
                else if (ConstParValue[i].LengthIndex == 1)
                {

                    return (1 << 16) - 1;
                }
            }
            else
            {
                if (ConstParValue[i].LengthIndex == 0)
                {
                    return (1 << 7) - 1;
                }
                else if (ConstParValue[i].LengthIndex == 1)
                {

                    return (1 << 15) - 1;
                }

            }
            return 0;
        }

        public static int getMinValue(ref List<ClassConstParValue> ConstParValue, int i)
        {
            if (ConstParValue[i].TypeIndex == 0)
            {
                return 0;
            }
            else
            {
                if (ConstParValue[i].LengthIndex == 0)
                {
                    return -(1 << 7);
                }
                else if (ConstParValue[i].LengthIndex == 1)
                {

                    return -(1 << 15);
                }

            }
            return 0;
        }






        public static int getAllLength(ref List<ClassParValue> ConstParValue)
        {
            int Count = 0;
            for (int i = 0; i < ConstParValue.Count; i++)
            {
                if (ConstParValue[i].LengthIndex == 0)
                {
                    Count += 1;
                }
                else if (ConstParValue[i].LengthIndex == 1)
                {
                    Count += 2;
                }
                else
                {
                    Count += 4;
                }
            }
            return Count;
        }



        public static string getType(int index)
        {
            switch (index)
            {
                case 0: return "否";
                case 1: return "是";
                default: return "否";

            }

        }

        public static string getLength(int index)
        {
            switch (index)
            {
                case 0: return "1字节";
                case 1: return "2字节";
                case 2: return "4字节";
                default: return "1字节";

            }

        }
        public static string getSub(int Index)
        {
            switch (Index)
            {
                case 0: return "0001";
                case 1: return "0002";
                case 2: return "0005";
                case 3: return "0010";
                case 4: return "0020";
                case 5: return "0050";
                case 6: return "0100";
                case 7: return "0200";
                case 8: return "0500";
                case 9: return "1000";
                default: return "0001";
            }
        }

        public static float getMulValue(int MulIndex)
        {
            switch (MulIndex)
            {
                case 0: return (float)0.1;
                case 1: return (float)0.2;
                case 2: return (float)0.5;
                case 3: return (float)1;
                case 4: return (float)2;
                case 5: return (float)3;
                case 6: return (float)5;
                case 7: return (float)10;
                case 8: return (float)20;
                case 9: return (float)50;
                case 10: return (float)100;
                default: return (float)1;
            }
        }
    }

    public class LCR
    {
        /// <summary>
        /// 左搜索域开始
        /// </summary>
        public byte LStart;
        /// <summary>
        /// 左线
        /// </summary>
        public byte LBlack;
        /// <summary>
        /// 左搜索域结束
        /// </summary>
        public byte LEnd;
        /// <summary>
        /// 中线
        /// </summary>
        public byte Center;
        /// <summary>
        /// 右搜索域开始
        /// </summary>
        public byte RStart;
        /// <summary>
        /// 右线
        /// </summary>
        public byte RBlack;
        /// <summary>
        /// 右搜索域结束
        /// </summary>
        public byte REnd;
        /// <summary>
        /// 回归线
        /// </summary>
        public byte CenterBack;
        public LCR()
        {
            LStart = LBlack = LEnd = Center = RStart = RStart = REnd = CenterBack= 255;
        }
        public LCR(byte LEnd, byte REnd, byte LStart, byte RStart, byte LBlack, byte RBlack, byte Center)
        {
            this.LEnd = LEnd;
            this.REnd = REnd;
            this.LStart = LStart;
            this.RStart = RStart;
            this.LBlack = LBlack;
            this.RBlack = RBlack;
            this.Center = Center;
        }
        public void LCR_Clear()
        {
            LStart = LBlack = LEnd = Center = RStart = RStart = REnd =CenterBack = 255;
        }
    }

    public class CCDPOS 
    {
        public byte Left;
        public byte Right;
        public byte Center;
        public Color L;
        public Color R;
        public Color C;
        public CCDPOS() 
        {
            ;
        }
       public CCDPOS(byte left, byte right, byte center) 
        {
            Left = left;
            Right = right;
            Center = center;
        }
       public void Setpos(byte left, byte right, byte center)
       {
           Left = left;
           Right = right;
           Center = center;
       }
       public void LCR_Clear()
       {
           Left = Right = Center = 255;
       }
    }
    public class CCDLCR 
    {
        public byte[] Data;
        public int Length;
        public CCDPOS ccdpos=new CCDPOS();
        public Color Dcolor;
        public CCDLCR(byte[] Value)
        {
            Data = Value;           
        }
        public CCDLCR(List<byte> Value, int length, Color C)
        {
            Dcolor = C;
            Data = new byte[length];
            Length = length;
            for (int i = 0; i < length; i++) 
            {
                Data[i] = Value[i];
            }
        }

        public CCDLCR(byte[,] Value,int chose, int length,Color C)
        {
            Dcolor = C;
            Data = new byte[length];
            Length = length;
            for (int i = 0; i < length; i++)
            {
                Data[i] = Value[chose,i];
            }
        }
        public CCDLCR(byte left, byte right, byte center, Color L, Color R, Color C, int length)
        {
            Length = length;
            ccdpos.Left = left;
            ccdpos.Right = right;
            ccdpos.Center = center;
            ccdpos.L = L;
            ccdpos.R = R;
            ccdpos.C = C;
        }
        public void SetCCDLcr(List<byte> Value, int length,Color C) 
        {
            Dcolor = C;
            Data = new byte[length];
            Length = length;
            for (int i = 0; i < length; i++)
            {
                Data[i] = Value[i];
            }
        }
    }
    public class ClassFitColor //用于设定拟图线颜色
    {
        public Color Back;
        public Color LS;
        public Color LL;
        public Color LE;
        public Color CL;
        public Color RS;
        public Color RL;
        public Color RE;
        public Color Text;

        public ClassFitColor()
        {
        }

        public ClassFitColor(Color back, Color ls, Color ll, Color le, Color cl, Color rs, Color rl, Color re, Color text)
        {
            Back = back;
            LS = ls;
            LL = ll;
            LE = le;
            CL = cl;
            RS = rs;
            RL = rl;
            RE = re;
            Text = text;

        }
    }

    public class ClassCCDColor 
    {
        public Color Back;
        public Color CCD1;
        public Color CCD2;
        public Color CCD3;
        public Color CCDC;
        public Color CCDL;
        public Color CCDR;
        public Color CCDP;
        public Color Text;
        public ClassCCDColor()
        {
        }
        public ClassCCDColor(Color back, Color C1, Color C2, Color C3, Color CC, Color CL, Color CR, Color CP, Color text)
        {
            Back = back;
            CCD1 = C1;
            CCD2 = C2;
            CCD3 = C3;
            CCDC = CC;
            CCDL = CL;
            CCDR = CR;
            CCDP = CP;
            Text = text;

        }
    }
    public class Pos
    {
        public int Top = 0;
        public int Left = 0;
        public int Width = 0;
        public int Height = 0;

    }

    public class ConvertClass
    {
        public static byte[,] getPixels(Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (imageIn != null)
                {
                    Bitmap bmp = new Bitmap(imageIn);
                    bmp.Save(ms, imageIn.RawFormat);
                }
                byte[] Temp = ms.ToArray();

                int length = 54;

                byte[,] Pixels = new byte[imageIn.Height, imageIn.Width];


                for (int R = 0; R < imageIn.Height; R++)
                {
                    for (int C = imageIn.Width - 1; C >= 0; C--)
                    {
                        Pixels[R, C] = Temp[length];
                        length += 4;
                    }
                }

                return Pixels;
            }
        }
        public static byte[] GetBitPixel(List<byte> Data, int width, int height)
        {
            byte[] Pixel = new byte[width * height];
            int count = (width * height) >> 3;
            int i = 0;
            for (int k = 0; k < count; k++)
            {
                for (int j = 7; j >= 0; j--)
                {
                    Pixel[i] = (byte)((Data[k] >> j) & 0x01);
                    i++;
                }
            }
            return Pixel;

        }//二值图像数据解压
        public static byte[] GetSimPixel(List<byte> Data, int width, int height)
        {
            byte[] Pixel = new byte[width * height];
            int count = width * height;
            for (int i = 0; i < count; i++)
            {
                Pixel[i] = (byte)Data[i];
            }

            return Pixel;
        }//模拟图像数据解压
        /// <summary> 

        /// 将一个字节数组转换为8bit灰度位图 

        /// </summary> 

        /// <param name="rawValues">显示字节数组</param> 

        /// <param name="width">Image_Width</param> 

        /// <param name="height">Image_Height</param> 

        /// <returns>位图</returns> 
        public static Bitmap ToGrayBitmapReal(byte[] rawValues, int width, int height)
        {

            //// 申请目标位图的变量，并将其内存区域锁定 

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),

                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);



            //// 获取图像参数 

            int stride = bmpData.Stride;　 // 扫描线的宽度 

            int offset = stride - width;　 // 显示宽度与扫描线宽度的间隙 

            IntPtr iptr = bmpData.Scan0;　 // 获取bmpData的内存起始位置 

            int scanBytes = stride * height;　　 // 用stride宽度，表示这是内存区域的大小 



            //// 下面把原始的显示大小字节数组转换为内存中实际存放的字节数组 

            int posScan = 0, posReal = 0;　　 // 分别设置两个位置指针，指向源数组和目标数组 

            byte[] pixelValues = new byte[scanBytes];　 //为目标数组分配内存 



            for (int x = 0; x < height; x++)
            {

                //// 下面的循环节是模拟行扫描 

                for (int y = 0; y < width; y++)
                {

                    pixelValues[posScan++] = rawValues[posReal++];

                }

                posScan += offset;　 //行扫描结束，要将目标位置指针移过那段“间隙” 

            }



            //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中 

            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);

            bmp.UnlockBits(bmpData);　 // 解锁内存区域 



            //// 下面的代码是为了修改生成位图的索引表，从伪彩修改为灰度 

            ColorPalette tempPalette;

            using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {

                tempPalette = tempBmp.Palette;

            }
            for (int i = 0; i < 256; i++)
            {

                tempPalette.Entries[i] = Color.FromArgb(i, i, i);

            }
            tempPalette.Entries[1] = Color.FromArgb(255, 255, 255);


            bmp.Palette = tempPalette;



            //// 算法到此结束，返回结果 

            return bmp;

        }
        public static Bitmap ToGrayBitmapFit(ref  byte[,] rawValues, int width, int height, ref  ClassFitColor FitColor)
        {

            //// 申请目标位图的变量，并将其内存区域锁定 

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),

                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);



            //// 获取图像参数 

            int stride = bmpData.Stride;　 // 扫描线的宽度 

            int offset = stride - width;　 // 显示宽度与扫描线宽度的间隙 

            IntPtr iptr = bmpData.Scan0;　 // 获取bmpData的内存起始位置 

            int scanBytes = stride * height;　　 // 用stride宽度，表示这是内存区域的大小 



            //// 下面把原始的显示大小字节数组转换为内存中实际存放的字节数组 

            int posScan = 0;　　 // 分别设置两个位置指针，指向源数组和目标数组 

            byte[] pixelValues = new byte[scanBytes];　 //为目标数组分配内存 



            for (int x = 0; x < height; x++)
            {

                //// 下面的循环节是模拟行扫描 

                    for (int y = 0; y < width; y++)
                    {

                        pixelValues[posScan++] = rawValues[x, y];

                    }

                    posScan += offset;　 //行扫描结束，要将目标位置指针移过那段“间隙” 
                
            }



            //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中 

            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);

            bmp.UnlockBits(bmpData);　 // 解锁内存区域 



            //// 下面的代码是为了修改生成位图的索引表，从伪彩修改为灰度 

            ColorPalette tempPalette;

            using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {

                tempPalette = tempBmp.Palette;

            }

            tempPalette.Entries[0] = FitColor.Back;
            tempPalette.Entries[1] = FitColor.LE;
            tempPalette.Entries[2] = FitColor.RE;
            tempPalette.Entries[3] = FitColor.LS;
            tempPalette.Entries[4] = FitColor.RS;
            tempPalette.Entries[5] = FitColor.LL;
            tempPalette.Entries[6] = FitColor.RL;
            tempPalette.Entries[7] = FitColor.CL;
            tempPalette.Entries[8] = Color.Yellow;



            bmp.Palette = tempPalette;



            //// 算法到此结束，返回结果 

            return bmp;

        }
        public static Bitmap ToGrayBitmapCCDReal(byte[] rawValues, int width, int height) 
        {
            int length = 64;

            Bitmap bmp = new Bitmap(width, length, PixelFormat.Format8bppIndexed);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, length),

                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int stride = bmpData.Stride;　 // 扫描线的宽度 

            int offset = stride - width;　 // 显示宽度与扫描线宽度的间隙 

            IntPtr iptr = bmpData.Scan0;　 // 获取bmpData的内存起始位置 

            int scanBytes = stride * length;　　 // 用stride宽度，表示这是内存区域的大小 



            //// 下面把原始的显示大小字节数组转换为内存中实际存放的字节数组 

          //  int posScan = 0;　　 // 分别设置两个位置指针，指向源数组和目标数组 

            byte[] pixelValues = new byte[scanBytes];　 //为目标数组分配内存 
            for (int j = 0; j < height; j++)
            {
                    for (int i = 0; i < width; i++)
                    {
                        pixelValues[((uint)rawValues[j * width + i]>>2) *(offset + width ) +i] = 1;
                    }
            }
                //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中 

                System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);

            bmp.UnlockBits(bmpData);　 // 解锁内存区域 



            //// 下面的代码是为了修改生成位图的索引表，从伪彩修改为灰度 

            ColorPalette tempPalette;

            using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {

                tempPalette = tempBmp.Palette;

            }
            tempPalette.Entries[0] = Color.White;
            tempPalette.Entries[1] = Color.Black;


            bmp.Palette = tempPalette;

            //// 算法到此结束，返回结果 

            return bmp;
        }
        public static Bitmap ToGrayBitmapCCDFit(ref byte[,] rawValues, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),

                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int stride = bmpData.Stride;　 // 扫描线的宽度 

            int offset = stride - width;　 // 显示宽度与扫描线宽度的间隙 

            IntPtr iptr = bmpData.Scan0;　 // 获取bmpData的内存起始位置 

            int scanBytes = stride * height;　　 // 用stride宽度，表示这是内存区域的大小 



            //// 下面把原始的显示大小字节数组转换为内存中实际存放的字节数组 

              int posScan = 0;　　 // 分别设置两个位置指针，指向源数组和目标数组 

            byte[] pixelValues = new byte[scanBytes];　 //为目标数组分配内存 
            for (int x = 0; x < height; x++)
            {
                //// 下面的循环节是模拟行扫描 

                for (int y = 0; y < width; y++)
                {

                    pixelValues[posScan++] = rawValues[x, y];

                }

                posScan += offset;　 //行扫描结束，要将目标位置指针移过那段“间隙” 

            }
            //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中 

            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);

            bmp.UnlockBits(bmpData);　 // 解锁内存区域 



            //// 下面的代码是为了修改生成位图的索引表，从伪彩修改为灰度 

            ColorPalette tempPalette;

            using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {

                tempPalette = tempBmp.Palette;

            }
            tempPalette.Entries[0] = Color.White;
            tempPalette.Entries[1] = Color.Black;
            tempPalette.Entries[2] = Color.Black;
            tempPalette.Entries[3] = Color.Black;

            bmp.Palette = tempPalette;

            //// 算法到此结束，返回结果 

            return bmp;
        }
    }

    public class String_Data_INI
    {
        private List<string> string_data = new List<string>();

        public List<string> String_Data
        {
            get { return string_data; }
            set { string_data = value; }
        }
        public String_Data_INI() { }
        public string Get_Value(string target)
        {
            int tmp_i = 0, tmp_j = 0;
            for (tmp_i = 0; tmp_i < string_data.Count; tmp_i++)
            {
                tmp_j = string_data[tmp_i].IndexOf(target);
                if (tmp_j != -1)
                {
                    break;
                }
            }
            if (tmp_j == -1)
            {
                MessageBox.Show("没有找到关键字");
                return default(string);
            }
            tmp_j = string_data[tmp_i].IndexOf("=");

            string tmp = string_data[tmp_i].Substring(tmp_j + 1);

            return tmp;
        }

    }

    public class DataChange
    {
        public float u32tofloat(UInt32 Value)
        {
            float Datatmp = 0;
            int bias = 0;
            double fraction = 0;

            bias = (int)(((Value >> 23) & 0xFF) - 0x7F);

            fraction += ((double)((Value & 0x007FFFFF) / Math.Pow(2, 23)));

            fraction += 1.0;

            Datatmp = (float)(fraction * Math.Pow(2, bias));

            if (((Value >> 31) & 0x01) == 1)
            {
                Datatmp = -(float)Datatmp;
            }
            if (Math.Abs(Datatmp) < 0.000001) 
            {
                Datatmp = 0;
            }
            return Datatmp;
        }
        public byte[] floattou32byte(float Value) 
        {
            byte[] Datatmp = new byte[4];
            UInt32 tmp=0;
            byte biascnt=0;
            UInt64 dmp = (UInt64)((double)(Value * Math.Pow(2, 32)));
            if (Value < 0) 
            {
                tmp += (UInt32)1 << 31;
            }
            while (dmp >= (UInt32)(1 << 24)) 
            {
                biascnt++;
                dmp = dmp / 2;
            }
            biascnt = (byte)(biascnt - 32+127+24);
            tmp += (UInt32)(biascnt << 23);
            tmp += (UInt32)(dmp - (1 << 24));
            Datatmp[3] = (byte)(tmp & 0xff);
            Datatmp[2] = (byte)((tmp >> 8) & 0xff);
            Datatmp[1] = (byte)((tmp >> 16) & 0xff);
            Datatmp[0] = (byte)((tmp >> 24) & 0xff);
            return Datatmp;
        }
    }

    public enum NrfMode
    {
        GetAllInfo,
        GetParInfo,
        SendParInfo,
    };
    public enum AllScreen
    {
        Null,
        Wave,
    };
    public enum MyResult
    {
        NULL,
        光电组,
        摄像头组,
        四轴飞行器组,
        通用模式,
    };
    public enum MyMode
    {
        Nrf_Par,
        Nrf_Pic,
        Uart_Par,
        Uart_Pic,
        Local_Sim,
        Local_Debug,
        //光电组特有模式
        Nrf_CCD,
        Uart_CCD,
        Local_CCD,
        Local_CCD_Debug,
        Uart_PicGray//串口灰度图像

    };
    public class Getmode
    {
      //  private string p;

        public Getmode()
        {
            ;
        }
        public static MyMode getmode(string value) 
        {
            if (value == "Nrf_Par") 
            {
                return MyMode.Nrf_Par;
            }
            else if (value == "Nrf_Pic") 
            {
                return MyMode.Nrf_Pic;
            }
            else if (value == "Uart_Par")
            {
                return MyMode.Uart_Par;
            }
            else if (value == "Uart_Pic")
            {
                return MyMode.Uart_Pic;
            }
            else if (value == "Local_Sim")
            {
                return MyMode.Local_Sim;
            }
            else if (value == "Local_Debug")
            {
                return MyMode.Local_Debug;
            }
            else if (value == "Nrf_CCD")
            {
                return MyMode.Nrf_CCD;
            }
            else if (value == "Uart_CCD")
            {
                return MyMode.Uart_CCD;
            }
            else if (value == "Local_CCD")
            {
                return MyMode.Local_CCD;
            }
            else if (value == "Local_CCD_Debug")
            {
                return MyMode.Local_CCD_Debug;
            }
            else if(value == "Uart_PicGray")
            {
                return MyMode.Uart_PicGray;
            }
            else
            {
                return MyMode.Uart_Par;
            }
        }
    }
    public enum MyMouse
    {
        None,
        Right,
        Left,
        Middle,
        Button1,
        Button2,
        RightFlag1,
        RightFlag2,
        RightFlag3,
        RightFlag4,
        RightFlag5,
        LeftFlag1,
        LeftFlag2,
        LeftFlag3,
        LeftFlag4,
        LeftFlag5,
    }

    /***************************通讯指令定义********************************/
    public enum CmdMode
    {
        Waitting = 0x00,//输出数据
        Write_Data = 0x01,///输出数据
        Read_Par = 0x02,//读入参数
        InitParLen = 0x03,//初始化参数  
        Write_Par = 0x04,//输出参数
        Stop_Car = 0x05, //停车
        UcStop_Car = 0x06,//自定义停车
        RstChip = 0x07, //重启主机
        NrfError = 0x08,
        ReadCmd = 0x09,
        Command10 = 0x0A,
        Command11 = 0x0B,
        Command12 = 0x0C,
        Command13 = 0x0D,
        Command14 = 0x0E,
        Command15 = 0x0F
    };

    public class MyCursors
    {
        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("user32.dll")]
        private static extern IntPtr SetCursor(IntPtr cursorHandle);

        [DllImport("user32.dll")]
        private static extern uint DestroyCursor(IntPtr cursorHandle);

        private  IntPtr colorCursorHandle = new IntPtr();

        public  Cursor Default = new Cursor(Cursor.Current.Handle);

        public Cursor Cross = new Cursor(Cursor.Current.Handle);

        public Cursor Move = new Cursor(Cursor.Current.Handle);

        public Cursor Choose = new Cursor(Cursor.Current.Handle);

        public Cursor Hand = new Cursor(Cursor.Current.Handle);

        public Cursor Text = new Cursor(Cursor.Current.Handle);
        public MyCursors()
        {
            colorCursorHandle = LoadCursorFromFile("../../MyCursors\\01.cur");//鼠标图标路径  
            Default.GetType().InvokeMember("handle", BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.SetField, null, Default,
            new object[] { colorCursorHandle });

            colorCursorHandle = LoadCursorFromFile("../../MyCursors\\05.cur");//鼠标图标路径  
            Cross.GetType().InvokeMember("handle", BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.SetField, null, Cross,
            new object[] { colorCursorHandle });

            colorCursorHandle = LoadCursorFromFile("../../MyCursors\\13.cur");//鼠标图标路径  
            Move.GetType().InvokeMember("handle", BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.SetField, null, Move,
            new object[] { colorCursorHandle });

            colorCursorHandle = LoadCursorFromFile("../../MyCursors\\03.cur");//鼠标图标路径  
            Choose.GetType().InvokeMember("handle", BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.SetField, null, Choose,
            new object[] { colorCursorHandle });

            colorCursorHandle = LoadCursorFromFile("../../MyCursors\\15.cur");//鼠标图标路径  
            Hand.GetType().InvokeMember("handle", BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.SetField, null, Hand,
            new object[] { colorCursorHandle });

            colorCursorHandle = LoadCursorFromFile("../../MyCursors\\06.cur");//鼠标图标路径  
            Text.GetType().InvokeMember("handle", BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.SetField, null, Text,
            new object[] { colorCursorHandle });

        }
    }

    public class MyMathXYData
    {
        public List<InDataPoint> DataPoint;
        //public int X_Count;
        //public int Y_Count;
        private InDataPoint MaxValue;
        private InDataPoint MinValue;


        public MyMathXYData()
        {
            //X_Count = 0;
            //Y_Count = 0;
            MaxValue = new InDataPoint();
            MinValue = new InDataPoint();
            DataPoint = new List<InDataPoint>();
        }
        public void Clear()
        {
            //X_Count = 0;
            //Y_Count = 0;
            DataPoint.Clear();
        }
        public void Add(string Value)
        {
            double tmpx, tmpy;
            try
            {
                string[] tmpValue = Value.Split('=');
                tmpx = double.Parse(tmpValue[0]);
                tmpy = double.Parse(tmpValue[1]);
            }
            catch
            {
                return;
            }
            DataPoint.Add(new InDataPoint(tmpx, tmpy));
        }
        private bool Xsort()
        {
            InDataPoint tmpx1 = new InDataPoint();
            InDataPoint tmpx2 = new InDataPoint();
            if (DataPoint.Count <= 2)
            {
                return false;
            }
            for (int i = 0; i < DataPoint.Count; i++)
            {
                tmpx1 = DataPoint[i];
                for (int j = i; j < DataPoint.Count; j++)
                {
                    tmpx2 = DataPoint[j];
                    if (tmpx1.X_Value > tmpx2.X_Value)
                    {
                        DataPoint[i]=tmpx2;
                        DataPoint[j] = tmpx1;
                        tmpx1 = DataPoint[i];
                    }
                }
            }
            MaxValue = DataPoint[DataPoint.Count - 1];
            MinValue = DataPoint[0];
            return true;
        }
        /// <summary>
        /// X轴线性滤波
        /// </summary>
        public List<InDataPoint> XLinearFilter(int cnt)
        {
            if (cnt <= 0)
            {
                MessageBox.Show("输入参数有误");
                return default(List<InDataPoint>);
            }
            if (DataPoint.Count == 0)
            {
                MessageBox.Show("输入数据有误");
                return default(List<InDataPoint>);
            }
            if (this.Xsort()==false) 
            {
                MessageBox.Show("输入数据有误");
                return default(List<InDataPoint>);
            }
            List<InDataPoint> tmpdata = new List<InDataPoint>();
            InDataPoint tmpx1 = new InDataPoint();
            InDataPoint tmpx2 = new InDataPoint();
            InDataPoint tmpx3 = MinValue;
            double ysub;
            double xsub = (double)(MaxValue.X_Value - MinValue.X_Value) / cnt;
            tmpdata.Add(MinValue);
            for (int i = 0; i < DataPoint.Count - 1; )
            {
                tmpx1 = DataPoint[i];
                do
                {
                    i++;
                }
                while (i < DataPoint.Count-1 && (DataPoint[i].X_Value - tmpx1.X_Value) < xsub);

                tmpx2 = DataPoint[i];
                ysub = (double)(tmpx2.Y_Value - tmpx1.Y_Value) * xsub / (tmpx2.X_Value - tmpx1.X_Value);
                while (tmpdata[tmpdata.Count-1].X_Value <= tmpx2.X_Value)
                {
                    tmpdata.Add(new InDataPoint(tmpx3.X_Value + xsub, tmpx3.Y_Value + ysub));
                    tmpx3 = tmpdata[tmpdata.Count - 1];
                }
            }

            return tmpdata;
        }
        /// <summary>
        /// 获取X最大值
        /// </summary>
        /// <returns></returns>
        public double getMaxX()
        {
            if (DataPoint.Count == 0)
            {
                return default(double);
            }
            double tmpx;
            tmpx = DataPoint[0].X_Value;
            for (int i = 1; i < DataPoint.Count; i++)
            {
                if (DataPoint[i].X_Value > tmpx)
                {
                    tmpx = DataPoint[i].X_Value;
                }
            }
            return tmpx;
        }
        /// <summary>
        /// 获取X最小值
        /// </summary>
        /// <returns></returns>
        public double getMinX()
        {
            if (DataPoint.Count == 0)
            {
                return default(double);
            }
            double tmpx;
            tmpx = DataPoint[0].X_Value;
            for (int i = 1; i < DataPoint.Count; i++)
            {
                if (DataPoint[i].X_Value < tmpx)
                {
                    tmpx = DataPoint[i].X_Value;
                }
            }
            return tmpx;
        }
        /// <summary>
        /// 获取Y最大值
        /// </summary>
        /// <returns></returns>
        public double getMaxY()
        {
            if (DataPoint.Count == 0)
            {
                return default(double);
            }
            double tmpy;
            tmpy = DataPoint[0].Y_Value;
            for (int i = 1; i < DataPoint.Count; i++)
            {
                if (DataPoint[i].Y_Value > tmpy)
                {
                    tmpy = DataPoint[i].Y_Value;
                }
            }
            return tmpy;
        }

        /// <summary>
        /// 获取Y最小值
        /// </summary>
        /// <returns></returns>
        public double getMinY()
        {
            if (DataPoint.Count == 0)
            {
                return default(double);
            }
            double tmpy;
            tmpy = DataPoint[0].Y_Value;
            for (int i = 1; i < DataPoint.Count; i++)
            {
                if (DataPoint[i].Y_Value < tmpy)
                {
                    tmpy = DataPoint[i].Y_Value;
                }
            }
            return tmpy;
        }

        public double[] XToArray()
        {
            double[] tmp = new double[DataPoint.Count];
            for (int i = 0; i < DataPoint.Count; i++) 
            {
                tmp[i] = DataPoint[i].X_Value;
            }
            return tmp;
        }
        public double[] YToArray()
        {
            double[] tmp = new double[DataPoint.Count];
            for (int i = 0; i < DataPoint.Count; i++)
            {
                tmp[i] = DataPoint[i].Y_Value;
            }
            return tmp;
        }

        public void setdata(double[] X_Value, double[] Y_Value) 
        {
            DataPoint.Clear();
            for (int i = 0; i < X_Value.Length&&i<Y_Value.Length; i++) 
            {
                DataPoint.Add(new InDataPoint(X_Value[i], Y_Value[i]));
            }
        }
    }
    public class InDataPoint
    {
        public double X_Value;
        public double Y_Value;
        public InDataPoint()
        {
            X_Value = 0;
            Y_Value = 0;
        }
        public InDataPoint(double X,double Y)
        {
            X_Value = X;
            Y_Value = Y;
        }
    }


    /// <summary>
    /// 遗传算法每个个体
    /// </summary>
    public class individual
    {
        /// <summary>
        /// 染色体
        /// </summary>
        public List<double> chrom = new List<double>();
        /// <summary>
        /// 适应度
        /// </summary>
        public double fitness = 0;
        /// <summary>
        /// 父个体
        /// </summary>
        public int[] parent = new int[2];
        /// <summary>
        /// 交叉位置
        /// </summary>
        public int xsite;
        /// <summary>
        /// 个体对应的变量值
        /// </summary>
        public double varible;

        public individual() 
        {
            fitness = 0;
            xsite = 0;
            varible = 0;
        }
        public individual(double[] chrom)
        {
            this.chrom.AddRange(chrom);
        }
        public individual(int num)
        {
            double[] tmp = new double[num];
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = 0;
            }
            this.chrom.AddRange(tmp);
        }
        public List<double> Getchrom() 
        {
            List<double> tmp = new List<double>(chrom);
            return tmp;
        }
    }
    public enum frmstatus 
    {
            frmdefault,
            frmall,
            frmfast,
    }


}
