using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNrf
{
    /// <summary>
    /// 实数后缀+d   虚数后缀+m
    /// </summary>
    public struct MyComplex
    {
        #region 数据成员
        public double Real_part;//实部
        public double Imag_Part;//虚部
        #endregion
        #region 构造函数
        public MyComplex(double real, double imag)
        {
            this.Real_part = real;
            this.Imag_Part = imag;
        }
        #endregion
        #region 运算符重载
        public static MyComplex operator +(MyComplex c1, MyComplex c2)
        {
            return new MyComplex(c1.Real_part + c2.Real_part, c1.Imag_Part + c2.Imag_Part);
        }
        public static MyComplex operator -(MyComplex c1, MyComplex c2)
        {
            return new MyComplex(c1.Real_part - c2.Real_part, c1.Imag_Part - c2.Imag_Part);
        }
        public static MyComplex operator *(MyComplex c1, MyComplex c2)
        {
            return new MyComplex(c1.Real_part * c2.Real_part - c1.Imag_Part * c2.Imag_Part,
             c1.Real_part * c2.Imag_Part + c1.Imag_Part * c2.Real_part);
        }
        public static MyComplex operator /(MyComplex c1, MyComplex c2)
        {
            return new MyComplex(-c1.Real_part * c2.Real_part +
               c1.Imag_Part * c2.Imag_Part, -c1.Real_part * c2.Imag_Part + c1.Imag_Part * c2.Real_part);
        }
        #endregion
        #region 类型转换
        public static implicit operator MyComplex(double c1)//隐式转换 
        {
            MyComplex c2 = new MyComplex();
            c2.Real_part = c1;
            return c2;
        }

        public static implicit operator MyComplex(decimal c1)//隐式转换 
        {
            MyComplex c2 = new MyComplex();
            c2.Imag_Part = (double)c1;
            return c2;
        }
        #endregion
        #region 角度转换
        public double getangle()
        {
            double tmpangle;
            tmpangle = Math.Atan(this.Real_part / this.Imag_Part);
            return tmpangle;
        }
        #endregion
        public override string ToString()
        {
            return (String.Format("{0} + {1}i", Real_part, Imag_Part));
        }
    }
    #region uint16_t类型
    public class uint16_t
    {
        private UInt16 Value;
        #region int
        //public static explicit operator int(uint16_t c1) //显式转换
        //{
        //    return c1.Value;
        //}

        public static implicit operator int(uint16_t c1) //隐式转换
        {
            return c1.Value;
        }
        //public static explicit operator uint16_t(int c1)//显式转换
        //{
        //    uint16_t c2 = new uint16_t();
        //    c2.Value = (UInt16)c1;
        //    return c2;
        //}

        public static explicit operator uint16_t(int c1)//显式转换
        {
            uint16_t c2 = new uint16_t();
            c2.Value = (UInt16)c1;
            return c2;
        }
        #endregion
        #region UInt16
        public static implicit operator UInt16(uint16_t c1) //隐式转换
        {
            return c1.Value;
        }

        public static implicit operator uint16_t(UInt16 c1)//隐式转换
        {
            uint16_t c2 = new uint16_t();
            c2.Value = (UInt16)c1;
            return c2;
        }
        #endregion
        #region float
        public static implicit operator float(uint16_t c1) //隐式转换
        {
            return c1.Value;
        }

        public static explicit operator uint16_t(float c1)//显式转换
        {
            uint16_t c2 = new uint16_t();
            c2.Value = (UInt16)c1;
            return c2;
        }
        #endregion
        #region Byte
        public static explicit operator Byte(uint16_t c1) //显式转换
        {
            return (Byte)c1.Value;
        }

        public static implicit operator uint16_t(Byte c1)//隐式转换
        {
            uint16_t c2 = new uint16_t();
            c2.Value = (UInt16)c1;
            return c2;
        }
        #endregion
        #region uint8_t
        public static explicit operator uint8_t(uint16_t c1) //显式转换
        {
            return (uint8_t)c1.Value;
        }

        public static implicit operator uint16_t(uint8_t c1)//隐式转换
        {
            uint16_t c2 = new uint16_t();
            c2.Value = (UInt16)c1;
            return c2;
        }
        #endregion
        #region ToString
        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion
    }
    #endregion
    #region uint8_t类型
    public class uint8_t
    {
        private Byte Value;
        #region int
        //public static explicit operator int(uint8_t c1) //显式转换
        //{
        //    return c1.Value;
        //}

        public static implicit operator int(uint8_t c1) //隐式转换
        {
            return c1.Value;
        }
        //public static explicit operator uint16_t(int c1)//显式转换
        //{
        //    uint16_t c2 = new uint16_t();
        //    c2.Value = (UInt16)c1;
        //    return c2;
        //}

        public static explicit operator uint8_t(int c1)//显式转换
        {
            uint8_t c2 = new uint8_t();
            c2.Value = (Byte)c1;
            return c2;
        }
        #endregion
        #region UInt16
        public static implicit operator UInt16(uint8_t c1) //隐式转换
        {
            return c1.Value;
        }

        public static explicit operator uint8_t(UInt16 c1)//显式转换
        {
            uint8_t c2 = new uint8_t();
            c2.Value = (Byte)c1;
            return c2;
        }
        #endregion
        #region float
        public static implicit operator float(uint8_t c1) //隐式转换
        {
            return c1.Value;
        }

        public static explicit operator uint8_t(float c1)//显式转换
        {
            uint8_t c2 = new uint8_t();
            c2.Value = (Byte)c1;
            return c2;
        }
        #endregion
        #region Byte
        public static implicit operator Byte(uint8_t c1) //隐式转换
        {
            return c1.Value;
        }

        public static implicit operator uint8_t(Byte c1)//隐式转换
        {
            uint8_t c2 = new uint8_t();
            c2.Value = (Byte)c1;
            return c2;
        }
        #endregion
        #region uint16_t
        public static implicit operator uint16_t(uint8_t c1) //隐式转换
        {
            return c1.Value;
        }

        public static explicit operator uint8_t(uint16_t c1)//显式转换
        {
            uint8_t c2 = new uint8_t();
            c2.Value = (Byte)c1;
            return c2;
        }
        #endregion
        #region ToString
        public override string ToString()
        {
            return Value.ToString();
        }
        #endregion
    }
    #endregion
    class MyMath
    {
        /// <summary>
        /// 表示圆周率
        /// </summary>
        public const double PI = Math.PI;
        /// <summary>
        /// 表示自然数
        /// </summary>
        public const double E = Math.E;
        /// <summary>
        /// 求最大值
        /// </summary>
        /// <param name="Value1"></param>
        /// <param name="Value2"></param>
        /// <returns></returns>
        public static double Max(double Value1, double Value2)
        {
            if (Value1 > Value2)
            {
                return Value1;
            }
            else
            {
                return Value2;
            }
        }
        /// <summary>
        /// 求最小值
        /// </summary>
        /// <param name="Value1"></param>
        /// <param name="Value2"></param>
        /// <returns></returns>
        public static double Min(double Value1, double Value2)
        {
            if (Value1 < Value2)
            {
                return Value1;
            }
            else
            {
                return Value2;
            }
        }
        /// <summary>
        /// 求绝对值
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double Abs(double Value)
        {
            if (Value > 0)
            {
                return Value;
            }
            else
            {
                return -Value;
            }
        }
        /// <summary>
        /// 符号函数
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int Sign(double Value)
        {
            if (Value == 0)
            {
                return 0;
            }
            else if (Value > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 斜坡函数
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="sita"></param>
        /// <returns></returns>
        public static double Sat(double Value, double sita)
        {
            if (MyMath.Abs(Value) < sita)
            {
                return (double)(Value / sita);
            }
            else
            {
                return (double)(Sign(Value));
            }
        }
        /// <summary>
        /// 返回指定数组的指定幂
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static double pow(double Value, double index)
        {
            return Math.Pow(Value, index);
        }
        /// <summary>
        /// 返回指定数的平方根
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double sqrt(double Value)
        {
            return Math.Sqrt(Value);
        }
        /// <summary>
        /// 限幅函数
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static double limit(double Value, double index)
        {
            if (index < 0)
            {
                return Value;
            }
            if (Value > index)
            {
                return index;
            }
            else if (Value < -index)
            {
                return -index;
            }
            else
            {
                return Value;
            }
        }
        /// <summary>
        /// 求平均值
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double average(double[] Value)
        {
            double sum = 0;
            for (int i = 0; i < Value.Length; i++)
            {
                sum += Value[i];
            }
            return (double)(sum / Value.Length);
        }
        /// <summary>
        /// 求余弦
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double cos(double Value)
        {
            return Math.Cos(Value);
        }
        /// <summary>
        /// 求正弦
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double sin(double Value)
        {
            return Math.Sin(Value);
        }
        /// <summary>
        /// 求正切
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double tan(double Value)
        {
            return Math.Tan(Value);
        }
        /// <summary>
        /// 浮点数求整(进位方式)
        /// </summary>
        /// <param name="Value"></param>
        /// <returns>as</returns>
        public static int Ceil(double Value)
        {
            if (Value != (int)Value)
            {
                return ((int)Value) + 1;
            }
            else
            {
                return ((int)Value);
            }
        }
        /// <summary>
        /// 求对数
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="newBase"></param>
        /// <returns></returns>
        public static double log(double Value, double newBase)
        {
            return Math.Log(Value, newBase);
        }
        /// <summary>
        /// 求10的对数
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double log10(double Value)
        {
            return Math.Log10(Value);
        }
        /// <summary>
        /// 求ln的对数
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double ln(double Value)
        {
            return Math.Log(E, Value);
        }
        /// <summary>
        /// 秦九昭算法求拉盖尔多项式的值
        /// <para>N：阶数</para>   
        /// <para>alpha：α系数</para>  
        /// <para>Value：输入参数</para>  
        /// </summary>
        /// <param name="n"></param>
        /// <param name="alpha"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double LaguerreL(int N, double alpha, double Value)
        {
            double Lagu = 1, Bin = 1;
            for (int i = N; i > 0; i--)//使用秦九昭算法
            {
                Bin = Bin * (alpha + i) / (N + 1 - i);
                Lagu = Bin - Value * Lagu / i;
            }
            return Lagu;
        }
        /// <summary>
        /// 递归法求勒让德多项式值
        /// </summary>
        /// <param name="n"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double LegendreP(int n, double Value)
        {
            if (n == 0)
            {
                return 1;
            }
            else if (n == 1)
            {
                return Value;
            }
            else
            {
                return (double)((2 * n - 1) * Value * LegendreP(n - 1, Value) - (n - 1) * LegendreP(n - 2, Value)) / n;
            }
        }
        /// <summary>
        /// 递归法求第一类切比雪夫多项式值
        /// </summary>
        /// <param name="n"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double ChebyshevF(int n, double Value)
        {
            if (n == 0)
            {
                return 1;
            }
            else if (n == 1)
            {
                return Value;
            }
            else
            {
                return (double)(2 * Value * ChebyshevF(n - 1, Value) - ChebyshevF(n - 2, Value));
            }
        }
        /// <summary>
        /// 递归法求第二类切比雪夫多项式值
        /// </summary>
        /// <param name="n"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double ChebyshevS(int n, double Value)
        {
            if (n == 0)
            {
                return 1;
            }
            else if (n == 1)
            {
                return 2 * Value;
            }
            else
            {
                return (double)(2 * Value * ChebyshevS(n - 1, Value) - ChebyshevS(n - 2, Value));
            }
        }

        /// <summary>
        /// 各种滤波
        /// </summary>
        public class Filter
        {
            /// <summary>
            /// 快速卡尔曼滤波 q,r为滤波系数
            /// </summary>
            /// <param name="Value"></param>
            /// <param name="q"></param>
            /// <param name="r"></param>
            public static void FastKalmanFilter(double[] Value, double q, double r)
            {
                double[] tmp1 = Value;
                double p1 = 0, p2 = 0, kg = 0, c1 = 0;
                for (int i = 0; i < 1000; i++)//消去开头
                {
                    p2 = p1 + q;
                    kg = p2 * p2 / (p2 * p2 + r * r);
                    p1 = (1 - kg) / p2;
                }
                for (int i = 0; i < tmp1.Length; i++)//消去开头
                {
                    Value[i] = c1 + kg * (tmp1[i] - c1);
                    c1 = Value[i];
                }

            }
            /// <summary>
            /// 计算滤波器系数
            /// <para>N：滤波器阶数</para>   
            /// <para>Ws：起始频率</para>  
            /// <para>As：衰减</para>  
            /// </summary>
            /// <param name="N"></param>
            /// <param name="sa"></param>
            /// <param name="sb"></param>
            /// <param name="az"></param>
            /// <param name="bz"></param>
            /// <returns></returns>
            private static void Bilinear(int N, double[] sa, double[] sb, ref double[] az, ref double[] bz)
            {
                int Count = 0, Count_1 = 0, Count_2 = 0, Count_Z = 0;
                double[] Res = new double[N + 1];
                double[] Res_Save = new double[N + 1];

                for (Count_Z = 0; Count_Z <= N; Count_Z++)
                {
                    az[Count_Z] = 0;
                    bz[Count_Z] = 0;
                }
                for (Count = 0; Count <= N; Count++)
                {
                    for (Count_Z = 0; Count_Z <= N; Count_Z++)
                    {
                        Res[Count_Z] = 0;
                        Res_Save[Count_Z] = 0;
                    }
                    Res_Save[0] = 1;
                    for (Count_1 = 0; Count_1 < N - Count; Count_1++)
                    {
                        for (Count_2 = 0; Count_2 <= Count_1 + 1; Count_2++)
                        {
                            if (Count_2 == 0)
                            {
                                Res[Count_2] += Res_Save[Count_2];
                            }
                            else if ((Count_2 == (Count_1 + 1)) && (Count_1 != 0))
                            {
                                Res[Count_2] += -Res_Save[Count_2 - 1];
                            }
                            else
                            {
                                Res[Count_2] += Res_Save[Count_2] - Res_Save[Count_2 - 1];
                            }
                        }
                        for (Count_Z = 0; Count_Z <= N; Count_Z++)
                        {
                            Res_Save[Count_Z] = Res[Count_Z];
                            Res[Count_Z] = 0;
                        }
                    }
                    for (Count_1 = (N - Count); Count_1 < N; Count_1++)
                    {
                        for (Count_2 = 0; Count_2 <= Count_1 + 1; Count_2++)
                        {
                            if (Count_2 == 0)
                            {
                                Res[Count_2] += Res_Save[Count_2];
                            }
                            else if ((Count_2 == (Count_1 + 1)) && (Count_1 != 0))
                            {
                                Res[Count_2] += Res_Save[Count_2 - 1];
                            }
                            else
                            {
                                Res[Count_2] += Res_Save[Count_2] + Res_Save[Count_2 - 1];
                            }
                        }
                        for (Count_Z = 0; Count_Z <= N; Count_Z++)
                        {
                            Res_Save[Count_Z] = Res[Count_Z];
                            Res[Count_Z] = 0;
                        }
                    }
                    for (Count_Z = 0; Count_Z <= N; Count_Z++)
                    {
                        az[Count_Z] += MyMath.pow(2, N - Count) * (sa[Count]) * Res_Save[Count_Z];
                        bz[Count_Z] += (sb[Count]) * Res_Save[Count_Z];
                    }
                }
                for (Count_Z = N; Count_Z >= 0; Count_Z--)
                {
                    bz[Count_Z] = (bz[Count_Z]) / az[0];
                    az[Count_Z] = (az[Count_Z]) / az[0];
                }
            }
            /// <summary>
            /// 计算 sa与sb(算子)
            /// <para>Wc：截止频率</para>  
            /// </summary>
            /// <param name="N"></param>
            /// <param name="Wc"></param>
            /// <param name="sa"></param>
            /// <param name="sb"></param>
            private static void MyBuffer(int N, double Wc, ref double[] sa, ref double[] sb)
            {
                double dk = 0;
                int k = 0;
                int count = 0, count_1 = 0;
                MyComplex[] poles = new MyComplex[N];
                MyComplex[] Res = new MyComplex[N + 1];
                MyComplex[] Res_Save = new MyComplex[N + 1];

                if ((N % 2) == 0)
                {
                    dk = 0.5;
                }
                else
                {
                    dk = 0;
                }
                for (k = 0; k <= ((2 * N) - 1); k++)
                {
                    if (Wc * MyMath.cos((k + dk) * (MyMath.PI / N)) < 0)
                    {
                        poles[count].Real_part = -Wc * MyMath.cos((k + dk) * (MyMath.PI / N));
                        poles[count].Imag_Part = -Wc * MyMath.sin((k + dk) * (MyMath.PI / N));
                        count++;
                        if (count == N) break;
                    }
                }
                Res[0].Real_part = poles[0].Real_part;
                Res[0].Imag_Part = poles[0].Imag_Part;
                Res[1].Real_part = 1;
                Res[1].Imag_Part = 0;
                for (count_1 = 0; count_1 < N - 1; count_1++)
                {
                    for (count = 0; count <= count_1 + 2; count++)
                    {
                        if (count == 0)
                        {
                            Res_Save[count] = Res[count] * poles[count_1 + 1];
                        }
                        else if ((count_1 + 2) == count)
                        {
                            Res_Save[count].Real_part += Res[count - 1].Real_part;
                            Res_Save[count].Imag_Part += Res[count - 1].Imag_Part;
                        }
                        else
                        {
                            Res_Save[count] = Res[count] * poles[count_1 + 1];
                            Res_Save[count].Real_part += Res[count - 1].Real_part;
                            Res_Save[count].Imag_Part += Res[count - 1].Imag_Part;
                        }
                    }
                    for (count = 0; count <= N; count++)
                    {
                        Res[count].Real_part = Res_Save[count].Real_part;
                        Res[count].Imag_Part = Res_Save[count].Imag_Part;

                        sa[N - count] = Res[count].Real_part;
                    }
                }
                if (N == 1)
                {
                    sa[0] = 1;
                }
                sb[N] = sa[N];

            }
            /// <summary>
            /// 求最优阶数
            /// </summary>
            /// <param name="Wc"></param>
            /// <param name="Ws"></param>
            /// <param name="As"></param>
            /// <param name="N"></param>
            private static void Buttord(double Wc, double Ws, double As, ref int N)
            {
                N = MyMath.Ceil((double)0.5 * (MyMath.log10(MyMath.pow(10, As / 10) - 1) / MyMath.log10(Ws / Wc)));
            }
            /// <summary>
            /// 巴斯沃特滤波
            /// <para>As：衰减</para>  
            /// <para>Ws：起始频率</para>
            /// <para>Wc：截止频率</para> 
            /// <para>As：衰减</para>  
            /// </summary>
            /// <param name="dataIn"></param>
            /// <param name="Wc"></param>
            /// <param name="Ws"></param>
            /// <param name="As"></param>
            public static void ButtordFilter(ref double[] data, double Wc, double Ws, double As)
            {
                int N = 0;
                Buttord(Wc, Ws, As, ref N);
                double[] sa = new double[N + 1];
                double[] sb = new double[N + 1];
                double[] az = new double[N + 1];
                double[] bz = new double[N + 1];
                for (int i = 0; i <= N; i++)
                {
                    sa[i] = 0;
                    sb[i] = 0;
                    az[i] = 0;
                    bz[i] = 0;
                }
                MyBuffer(N, Wc, ref sa, ref sb);
                Bilinear(N, sa, sb, ref az, ref bz);
                double[] tmpdata = data;
                double[] Ycbuffer = new double[N + 1];
                double[] Y = new double[N + 1];
                for (int i = 0; i < tmpdata.Length; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        Ycbuffer[j] = Ycbuffer[j + 1];
                        Y[j] = Y[j + 1];
                    }
                    Ycbuffer[N] = tmpdata[i];
                    Y[N] = Ycbuffer[N] * bz[N];
                    for (int j = 0; j < N; j++)
                    {
                        Y[N] += bz[j] * Ycbuffer[j] - Y[j] * az[N - j];
                    }
                    data[i] = Y[N];
                }
            }
            /// <summary>
            /// 数字滤波
            /// az,bz 位滤波器参数
            /// az在分子 bz在分母
            /// </summary>
            /// <param name="data"></param>
            /// <param name="az"></param>
            /// <param name="bz"></param>
            public static void FilterFilter(ref double[] data, double[] az, double[] bz)
            {
                int N = az.Length - 1;
                double[] tmpdata = data;
                double[] Ycbuffer = new double[N + 1];
                double[] Y = new double[N + 1];
                for (int i = 0; i < tmpdata.Length; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        Ycbuffer[j] = Ycbuffer[j + 1];
                        Y[j] = Y[j + 1];
                    }
                    Ycbuffer[N] = tmpdata[i];
                    Y[N] = Ycbuffer[N] * az[N];
                    for (int j = 0; j < N; j++)
                    {
                        Y[N] += az[j] * Ycbuffer[j] - Y[j] * bz[N - j];
                    }
                    data[i] = Y[N];
                }
            }
            /// <summary>
            /// 最小二乘法计算系数
            /// Poly阶数
            /// </summary>
            /// <param name="data"></param>
            /// <param name="Poly"></param>
            public static double[] polyfit(double[] x, double[] y, int Poly)
            {
                int n = x.Length;
                double[] tempx = new double[n];
                double[] sumxx = new double[Poly * 2 + 1];
                double[] tempy = new double[n];
                double[] sumxy = new double[Poly + 1];
                double[] ata = new double[(Poly + 1) * (Poly + 1)];

                for (int i = 0; i < n; i++)
                {
                    tempx[i] = 1;
                    tempy[i] = y[i];
                }
                for (int i = 0; i < 2 * Poly + 1; i++)
                {
                    sumxx[i] = 0;
                    for (int j = 0; j < n; j++)
                    {
                        sumxx[i] += tempx[j];
                        tempx[j] *= x[j];
                    }
                }
                for (int i = 0; i < Poly + 1; i++)
                {
                    sumxy[i] = 0;
                    for (int j = 0; j < n; j++)
                    {
                        sumxy[i] += tempy[j];
                        tempy[j] *= x[j];
                    }
                }
                for (int i = 0; i < Poly + 1; i++)
                {
                    for (int j = 0; j < Poly + 1; j++)
                    {
                        ata[i * (Poly + 1) + j] = sumxx[i + j];
                    }
                }
                double[] p = gauss_solve(Poly + 1, ata, sumxy);
                double temp;
                for (int i = 0; i < p.Length / 2; i++)
                {
                    temp = p[i];
                    p[i] = p[p.Length - 1 - i];
                    p[p.Length - 1 - i] = temp;
                }
                return p;
            }
            /// <summary>
            /// 高斯消元法求多项式值
            /// </summary>
            /// <param name="n"></param>
            /// <param name="A"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            private static double[] gauss_solve(int n, double[] A, double[] b)
            {
                int i, j, k, r;
                double[] x = new double[n];
                double max;
                for (k = 0; k < n - 1; k++)
                {
                    max = MyMath.Abs(A[k * n + k]); /*find maxmum*/
                    r = k;
                    for (i = k + 1; i < n - 1; i++)
                    {
                        if (max < MyMath.Abs(A[i * n + i]))
                        {
                            max = MyMath.Abs(A[i * n + i]);
                            r = i;
                        }
                    }
                    if (r != k)
                    {
                        for (i = 0; i < n; i++)         /*change array:A[k]&A[r]  */
                        {
                            max = A[k * n + i];
                            A[k * n + i] = A[r * n + i];
                            A[r * n + i] = max;
                        }
                    }
                    max = b[k];                    /*change array:b[k]&b[r]     */
                    b[k] = b[r];
                    b[r] = max;
                    for (i = k + 1; i < n; i++)
                    {
                        for (j = k + 1; j < n; j++)
                        {
                            A[i * n + j] -= A[i * n + k] * A[k * n + j] / A[k * n + k];
                        }
                        b[i] -= A[i * n + k] * b[k] / A[k * n + k];
                    }
                }
                for (i = n - 1; i >= 0; x[i] /= A[i * n + i], i--)
                {
                    for (j = i + 1, x[i] = b[i]; j < n; j++)
                    {
                        x[i] -= A[i * n + j] * x[j];
                    }
                }
                return x;
            }
            /// <summary>
            /// 最小二乘法计算值
            /// </summary>
            /// <param name="pp"></param>
            /// <param name="x"></param>
            /// <returns></returns>
            static public double MyPolyVal(double[] pp, double x) //计算数值
            {
                double sum = 0;

                byte i = 0;


                for (i = 0; i < pp.Length; i++)
                {
                    sum += pp[i] * MyMath.pow(x, pp.Length - 1 - i);
                }
                return sum;
            }
            /// <summary>
            /// 有限记忆最小二乘法滤波
            /// </summary>
            /// <param name="data"></param>
            /// <param name="MemarySize"></param>
            /// <param name="poly"></param>
            public static void PolyFilter(ref double[] data, int MemarySize, int poly)
            {
                if (data.Length < MemarySize)
                {
                    return;
                }
                double[] buffer = new double[MemarySize];
                double[] y = new double[MemarySize];
                for (int i = 0; i < MemarySize; i++)
                {
                    y[i] = i;
                    FIFO_Buffer(ref buffer, data[i], false);
                }
                for (int i = 0; i < MemarySize; i++)
                {
                    double[] pfilt = polyfit(y, buffer, poly);
                    data[i] = MyPolyVal(pfilt, i);
                }
                for (int i = MemarySize; i < data.Length; i++)
                {
                    FIFO_Buffer(ref buffer, data[i], false);
                    double[] pfilt = polyfit(y, buffer, poly);
                    data[i] = MyPolyVal(pfilt, MemarySize - 1);
                }
            }
            /// <summary>
            /// 先入先出寄存器
            /// Forward:方向选择 true向上 false向下
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="data"></param>
            /// <param name="Forward"></param>
            public static void FIFO_Buffer(ref double[] buffer, double data, bool Forward)
            {

                if (Forward == true)
                {
                    for (int i = 1; i < buffer.Length; i++)
                    {
                        buffer[buffer.Length - i] = buffer[buffer.Length - 1 - i];
                    }
                    buffer[0] = data;
                }
                else
                {
                    for (int i = 0; i < buffer.Length - 1; i++)
                    {
                        buffer[i] = buffer[i + 1];
                    }
                    buffer[buffer.Length - 1] = data;
                }
            }
        }
        /// <summary>
        /// 提供矩阵运算
        /// </summary>
        public class Matrix
        {
            public static int MatrixErrorCnt;
            private List<double> MatrixData = new List<double>();
            public int nRows=0;
            public int nColumns=0;
            public Matrix() 
            {
                ;
            }
            public Matrix(List<double> data , int m,int n) 
            {
                this.nRows = m;
                this.nColumns = n;
                this.MatrixData = data;
            }
            public Matrix(int m, int n)
            {
                this.nRows = m;
                this.nColumns = n;
                for (int i = 0; i < m * n;i++ )
                {
                    this.MatrixData.Add(0d);
                }

            }
            public Matrix(int n)
            {
                this.nRows = n;
                this.nColumns = n;
                for (int i = 0; i < n * n; i++)
                {
                    this.MatrixData.Add(0d);
                }
            }
            public void Init(List<double> data, int m, int n)
            {
                this.nRows = m;
                this.nColumns = n;
                this.MatrixData = data;
            }
            public void Init(int n)
            {
                this.nRows = n;
                this.nColumns = n;
                for (int i = 0; i < n * n; i++)
                {
                    this.MatrixData.Add(0d);
                }
            }
            public void Init(int m, int n)
            {
                this.nRows = m;
                this.nColumns = n;
                for (int i = 0; i < m * n; i++)
                {
                    this.MatrixData.Add(0d);
                }
            }
            public void InitData(List<double> data) 
            {
                MatrixData = data;
            }
            public void InitData(double[] data)
            {
                MatrixData.Clear();
                MatrixData.AddRange(data);
            }
            public static Matrix operator+(Matrix MatInA,Matrix MatInB)
              {
                  if (MatInA.nRows != MatInB.nRows || MatInA.nColumns != MatInB.nColumns) 
                  {
                      MatrixErrorCnt += 1;
                      return default(Matrix);
                  }
                  Matrix TmpMat = new Matrix(MatInA.nRows, MatInA.nColumns);

                  matrix_addition(MatInA, MatInB, MatInA.nRows, MatInA.nColumns, ref TmpMat);

                  return TmpMat;
              }
            public static Matrix operator-(Matrix MatInA,Matrix MatInB)
              {
                  if (MatInA.nRows != MatInB.nRows || MatInA.nColumns != MatInB.nColumns)
                  {
                      MatrixErrorCnt += 1;
                      return default(Matrix);
                  }
                  Matrix TmpMat = new Matrix(MatInA.nRows, MatInA.nColumns);

                  matrix_subtraction(MatInA, MatInB, MatInA.nRows, MatInA.nColumns, ref TmpMat);

                  return TmpMat;
              }
            public static Matrix operator *(Matrix MatInA, Matrix MatInB)
              {
                  if (MatInA.nColumns != MatInB.nRows )
                  {
                      MatrixErrorCnt += 1;
                      return default(Matrix);
                  }
                  Matrix TmpMat = new Matrix(MatInA.nRows, MatInB.nColumns);

                  matrix_multiply(MatInA, MatInB, MatInA.nRows, MatInA.nColumns, MatInB.nColumns, ref TmpMat);

                  return TmpMat;
              }
            public static Matrix operator *(Matrix MatInA, double k)
            {

                Matrix TmpMat = new Matrix(MatInA.nRows, MatInA.nColumns);

                matrix_multiply_k(MatInA,k, MatInA.nRows, MatInA.nColumns,  ref TmpMat);

                return TmpMat;
            }
            public static Matrix Inversion(Matrix MatIn) 
            {
                if (MatIn.nColumns != MatIn.nRows)
                {
                    MatrixErrorCnt += 1;
                    return default(Matrix);
                }
                Matrix TmpMat = new Matrix(MatIn.nColumns);
                if (Matrix_Inversion(MatIn, MatIn.nColumns, ref TmpMat) == true)
                {
                    return TmpMat;
                }
                else 
                {
                    MatrixErrorCnt += 1;
                    return default(Matrix);
                }
            }
            public static Matrix trans(Matrix MatIn) 
            {
                Matrix TmpMat = new Matrix(MatIn.nColumns,MatIn.nRows);

                matrix_transpose(MatIn, MatIn.nRows, MatIn.nColumns,ref TmpMat);

                return TmpMat;
            }
            public double this[int m,int n]
              {
                  get
                  {
                      if (!(m > nRows || n > nColumns || MatrixData.Count == 0)) 
                      {
                          return default(double);
                      }
                        return MatrixData[(m-1)*nColumns+n-1];
                  }
                  set
                  {
                      if (!(m > nRows || n > nColumns || MatrixData.Count==0)) 
                      {
                          MatrixData[(m - 1) * nColumns + n - 1] = value;
                      }
                  }
              }
            public override string ToString() 
            {
                int i, j;
                string showprintf = "";
                showprintf += "\r\n";
                for (i = 0; i < nRows; i++)
                {
                    showprintf += "| ";
                    for (j = 0; j < nColumns; j++)
                    {
                        if (MatrixData[nColumns * i + j] >= 0)
                        {
                            showprintf += " ";
                        }
                        showprintf += MatrixData[nColumns * i + j].ToString("e") + " ";
                    }
                    showprintf += "|\r\n";
                }
                showprintf += "\r\n";
                return showprintf;
            }
            #region MatrixLib
            /// <summary>
            /// 矩阵求逆
            /// <para>A = input matrix (n x n)</para>
            /// <para>n = dimension of A</para>
            /// <para>AInverse = inverted matrix (n x n)  </para>
            /// <para>返回值</para>
            /// <para>true：求逆成功</para>
            /// <para>false：矩阵不可逆或输入有误</para>
            /// </summary>
            /// <param name="A"></param>
            /// <param name="n"></param>
            /// <param name="AInverse"></param>
            /// <returns></returns>
            public static bool Matrix_Inversion(Matrix A, int n, ref Matrix AInverse)
            {
                int i, j, k, m;
                double[] tmp_matrix = new double[2 * n * n];
                double temp_data, mik;
                m = 2 * n;
                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        if (i == j)
                        {
                            AInverse.MatrixData[i * n + j] = 1;
                        }
                        else
                        {
                            AInverse.MatrixData[i * n + j] = 0;
                        }
                    }
                }
                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        tmp_matrix[i * m + j] = A.MatrixData[i * n + j];  //复制A到a，避免改变A的值
                    }
                }
                for (i = 0; i < n; i++)
                {
                    for (j = n; j < m; j++)
                    {
                        tmp_matrix[i * m + j] = AInverse.MatrixData[i * n + j - n];  //复制AInverse到a，增广矩阵
                    }
                }
                for (k = 1; k <= n - 1; k++)
                {
                    for (i = k + 1; i <= n; i++)
                    {
                        if (tmp_matrix[(k - 1) * m + k - 1] == 0)
                        {
                            return false;
                        }
                        mik = tmp_matrix[(i - 1) * m + k - 1] / tmp_matrix[(k - 1) * m + k - 1];
                        for (j = k + 1; j <= m; j++)
                        {
                            tmp_matrix[(i - 1) * m + j - 1] -= mik * tmp_matrix[(k - 1) * m + j - 1];
                        }
                    }
                } //顺序高斯消去法化左下角为零
                for (i = 1; i <= n; i++)
                {
                    temp_data = tmp_matrix[(i - 1) * m + i - 1];
                    if (temp_data == 0)
                    {
                        return false;
                    }
                    for (j = 1; j <= m; j++)
                    {
                        tmp_matrix[(i - 1) * m + j - 1] = tmp_matrix[(i - 1) * m + j - 1] / temp_data;
                    }
                }        //归一化
                for (k = n - 1; k >= 1; k--)
                {
                    for (i = k; i >= 1; i--)
                    {
                        mik = tmp_matrix[(i - 1) * m + k];
                        for (j = k + 1; j <= m; j++)
                        {
                            tmp_matrix[(i - 1) * m + j - 1] -= mik * tmp_matrix[k * m + j - 1];
                        }
                    }
                }        //逆序高斯消去法化增广矩阵左边为单位矩阵

                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        AInverse.MatrixData[i * n + j] = tmp_matrix[i * m + j + n];  //取出求逆结果
                    }
                }

                Matrix C = new Matrix(n);
                matrix_multiply(A, AInverse, n, n, n, ref C);
                double tmp_Value = 0;
                for (i = 0; i < n * n; i++)//验证结果
                {
                    tmp_Value += C.MatrixData[i];
                }
                if (tmp_Value > n + 0.1f || tmp_Value < n - 0.1f)
                {
                    return false;
                }
                return true;
            }
            /// <summary>
            /// 矩阵相乘
            /// <para>A = input matrix (m x p)</para>
            /// <para>B = input matrix (p x n)</para>
            /// <para>m = number of rows in A</para>
            /// <para>p = number of columns in A = number of rows in B</para>
            /// <para>n = number of columns in B</para>
            /// <para>C = output matrix = A*B (m x n)</para>
            /// </summary>
            /// <param name="A"></param>
            /// <param name="B"></param>
            /// <param name="m"></param>
            /// <param name="p"></param>
            /// <param name="n"></param>
            /// <param name="C"></param>
            public static void matrix_multiply(Matrix A, Matrix B, int m, int p, int n, ref Matrix C)
            {
                int i, j, k;
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        C.MatrixData[n * i + j] = 0;
                        for (k = 0; k < p; k++)
                        {
                            C.MatrixData[n * i + j] = C.MatrixData[n * i + j] + A.MatrixData[p * i + k] * B.MatrixData[n * k + j];
                        }
                    }
                }
            }
            /// <summary>
            /// 矩阵转置
            /// <para>A = input matrix (m x n)</para>
            /// <para>m = number of rows in A</para>
            /// <para>n = number of columns in A</para>
            /// <para>C = output matrix = the transpose of A (n x m)</para>
            /// </summary>
            /// <param name="A"></param>
            /// <param name="m"></param>
            /// <param name="n"></param>
            /// <param name="C"></param>
            public static void matrix_transpose(Matrix A, int m, int n, ref Matrix C)
            {
                int i, j;
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        C.MatrixData[m * j + i] = A.MatrixData[n * i + j];
                    }
                }
            }
            /// <summary>
            /// 矩阵相加
            /// <para>A = input matrix (m x n)</para>
            /// <para>B = input matrix (m x n)</para>
            /// <para>m = number of rows in A = number of rows in B</para>
            /// <para>n = number of columns in A = number of columns in B</para>
            /// <para>C = output matrix = A+B (m x n)</para>
            /// </summary>
            /// <param name="A"></param>
            /// <param name="B"></param>
            /// <param name="m"></param>
            /// <param name="n"></param>
            /// <param name="C"></param>
            public static void matrix_addition(Matrix A, Matrix B, int m, int n, ref Matrix C)
            {
                int i, j;
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        C.MatrixData[n * i + j] = A.MatrixData[n * i + j] + B.MatrixData[n * i + j];
                    }
                }
            }
            /// <summary>
            /// 矩阵调比例
            /// <para>A = input matrix (m x n)</para>
            /// <para>m = number of rows in A</para>
            /// <para>n = number of columns in A</para>
            /// <para>C = output matrix = k*A (m x n) </para>
            /// </summary>
            /// <param name="A"></param>
            /// <param name="k"></param>
            /// <param name="m"></param>
            /// <param name="n"></param>
            /// <param name="C"></param>
            public static void matrix_multiply_k(Matrix A, double k, int m, int n, ref Matrix C)
            {
                int i, j;
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        C.MatrixData[n * i + j] = A.MatrixData[n * i + j] * k;
                    }
                }
            }
            /// <summary>
            /// 矩阵清零
            /// <para>A = input matrix (m x n)</para>
            /// <para>m = number of rows in A</para>
            /// <para>n = number of columns in A</para>
            /// </summary>
            /// <param name="A"></param>
            /// <param name="m"></param>
            /// <param name="n"></param>
            public static void matrix_init0(ref Matrix A, int m, int n)
            {
                int i, j;
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        A.MatrixData[n * i + j] = 0;
                    }
                }
            }
            /// <summary>
            /// 矩阵复制
            /// <para>A = input matrix (m x n)</para>
            /// <para>m = number of rows in A</para>
            /// <para>n = number of columns in A</para>
            ///  <para>C = output matrix = A (m x n)</para>
            /// </summary>
            /// <param name="A"></param>
            /// <param name="m"></param>
            /// <param name="n"></param>
            /// <param name="C"></param>
            public static void matrix_copy(Matrix A, int m, int n, ref Matrix C)
            {
                int i, j;
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        C.MatrixData[n * i + j] = A.MatrixData[n * i + j];
                    }
                }
            }
            /// <summary>
            /// 单位矩阵构造
            /// <para>A = input matrix (n x n)</para> 
            /// <para>n = number of columns and rows in A</para> 
            /// </summary>
            /// <param name="A"></param>
            /// <param name="n"></param>
            public static void matrix_eye(ref Matrix A, int n)
            {
                int i, j;
                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        if (i == j)
                        {
                            A.MatrixData[n * i + j] = 1;
                        }
                        else
                        {
                            A.MatrixData[n * i + j] = 0;
                        }
                    }
                }
            }
            /// <summary>
            /// 求负矩阵
            /// <para>A = input matrix (m x n)</para> 
            /// <para>m = number of rows in A</para> 
            /// <para>n = number of columns in A</para> 
            /// <para>C = output matrix = -A (m x n)</para> 
            /// </summary>
            /// <param name="A"></param>
            /// <param name="m"></param>
            /// <param name="n"></param>
            /// <param name="C"></param>
            public static void matrix_negate(Matrix A, int m, int n, ref Matrix C)
            {
                int i, j;
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        C.MatrixData[n * i + j] = -A.MatrixData[n * i + j];
                    }
                }
            }
            /// <summary>
            /// 矩阵相减
            /// <para>A = input matrix (m x n)</para> 
            /// <para>B = input matrix (m x n)</para> 
            /// <para>m = number of rows in A = number of rows in B</para> 
            /// <para>n = number of columns in A = number of columns in B</para> 
            /// <para>C = output matrix = A-B (m x n)</para> 
            /// </summary>
            /// <param name="A"></param>
            /// <param name="B"></param>
            /// <param name="m"></param>
            /// <param name="n"></param>
            /// <param name="C"></param>
            public static void matrix_subtraction(Matrix A, Matrix B, int m, int n, ref Matrix C)
            {

                int i, j;
                for (i = 0; i < m; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        C.MatrixData[n * i + j] = A.MatrixData[n * i + j] - B.MatrixData[n * i + j];
                    }
                }
            }
            /// <summary>
            /// 矩阵打印
            /// <para>A = input matrix (m x n)</para> 
            /// <para>m = number of rows in A</para> 
            /// <para>n = number of columns in A</para> 
            /// </summary>
            /// <param name="A"></param>
            /// <param name="m"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            public static string matrix_print(Matrix A)
            {
                return A.ToString();
            }
            #endregion
        }
        /// <summary>
        /// 提供算法方案代码
        /// </summary>
        public class FunC
        {
            /// <summary>
            /// 提供实现ln(x)方法代码
            /// </summary>
            /// <param name="Value"></param>
            /// <returns></returns>
            public static string Function_To_In()
            {
                string func = string.Empty;
                func += "\r\n";
                func += "/****************************************************/" + "\r\n";
                func += "/*函数功能：快速求ln(x)*/" + "\r\n";
                func += "/*函数特点：cnt为运算次数*/" + "\r\n";
                func += "/*函数特点：减少运算量，并提高运算速度*/" + "\r\n";
                func += "/*函数注意：为增加运算速度可以将double改为float*/" + "\r\n";
                func += "/****************************************************/" + "\r\n";
                func += "double Math_In(double Value,int cnt)" + "\r\n" + "{" + "\r\n";
                func += "   int i=0;" + "\r\n";
                func += "   double tmp = (Value - 1) / (Value + 1);" + "\r\n";
                func += "   double func=0;" + "\r\n";
                func += "   for(i=cnt;i>=0;i--)" + "\r\n" + "   {" + "\r\n";
                func += "       func=func*tmp+1/(2*i+1);" + "//秦九昭算法求多项式值" + "\r\n" + "   }" + "\r\n";
                func += "   func=func*tmp*2;" + "\r\n";
                func += "   return func;" + "\r\n" + "}" + "\r\n";
                return func;
            }
            /// <summary>
            /// 迭代法求sqrt(a)
            /// </summary>
            /// <param name="Value"></param>
            /// <returns></returns>
            public static string Function_To_Sqrt()
            {
                string func = string.Empty;
                func += "\r\n";
                func += "/****************************************************/" + "\r\n";
                func += "/*函数功能：迭代法求sqrt(a)*/" + "\r\n";
                func += "/*函数特点：cnt为运算次数*/" + "\r\n";
                func += "/*函数注意：为增加运算速度可以将double改为float,并且输入值可以适当归一化*/" + "\r\n";
                func += "/****************************************************/" + "\r\n";
                func += "double Math_Sqrt(double Value,int cnt)" + "\r\n" + "{" + "\r\n";
                func += "   int i=0;" + "\r\n";
                func += "   double tmp;" + "\r\n";
                func += "   double func=0;" + "\r\n";
                func += "   for(i=cnt;i>=0;i--)" + "\r\n" + "   {" + "\r\n";
                func += "       tmp=(tmp+Value/tmp)/2;" + "//迭代法求多项式值" + "\r\n" + "   }" + "\r\n";
                func += "   func=tmp;" + "\r\n";
                func += "   return func;" + "\r\n" + "}" + "\r\n";
                return func;
            }
            /// <summary>
            /// 泰勒展开级数求sin(x)
            /// </summary>
            /// <returns></returns>
            public static string Function_To_sin()
            {
                string func = string.Empty;
                func += "\r\n";
                func += "/****************************************************/" + "\r\n";
                func += "/*函数功能：泰勒展开级数求sin(x)*/" + "\r\n";
                func += "/*函数注意：要求绝对误差小Q=0.775,P=0.225，相对误差小 Q=0.782,P=0.218，输入为弧度不是角度*/" + "\r\n";
                func += "/*函数注意：为增加运算速度可以将double改为float*/" + "\r\n";
                func += "/****************************************************/" + "\r\n";
                func += "double Math_sin(double Value)" + "\r\n" + "{" + "\r\n";
                func += "   double P=0.225;//Q=0.775;// Q=0.782,P=0.218" + "\r\n";
                func += "   double div4PI=1.27324;//4/PI" + "\r\n";
                func += "   double div4PI2=0.40528;//-4/PI/PI" + "\r\n";
                func += "   double func;" + "\r\n";
                func += "   double PI=3.14159;//4/PI" + "\r\n";
                func += "   while(Math_Abs(Value)>PI)" + "\r\n" + "    {" + "\r\n";
                func += "       if(Value>0)" + "\r\n" + "        {" + "\r\n";
                func += "           Value=Value-PI;" + "\r\n" + "        }" + "\r\n";
                func += "       else" + "\r\n" + "        {" + "\r\n";
                func += "           Value=Value+PI;" + "\r\n" + "        }" + "\r\n" + "    }" + "\r\n";
                func += "   func=div4PI*Value-div4PI2*Value*Math_Abs(Value);" + "\r\n";
                func += "   func=P*(func*Math_Abs(func)-func)+func;" + "\r\n";
                func += "   return func;" + "\r\n" + "}" + "\r\n";
                return func;
            }
            /// <summary>
            /// 泰勒展开级数求cos(x)
            /// </summary>
            /// <returns></returns>
            public static string Function_To_cos()
            {
                string func = string.Empty;
                func += "\r\n";
                func += "/****************************************************/" + "\r\n";
                func += "/*函数功能：泰勒展开级数求cos(x)*/" + "\r\n";
                func += "/*函数注意：要求绝对误差小Q=0.775,P=0.225，相对误差小 Q=0.782,P=0.218，输入为弧度不是角度*/" + "\r\n";
                func += "/*函数注意：为增加运算速度可以将double改为float*/" + "\r\n";
                func += "/****************************************************/" + "\r\n";
                func += "double Math_cos(double Value)" + "\r\n" + "{" + "\r\n";
                func += "   double div2PI=1.57080;//PI/2" + "\r\n";
                func += "   return Math_sin(div2PI-Value);" + "\r\n" + "}" + "\r\n";
                return func;
            }

        }
    }
    /// <summary>
    /// 遗传算法
    /// </summary>
    public class AForgeGenetic
    {
        List<individual> oldpop = new List<individual>();
        List<individual> newpop = new List<individual>();
        public MyMathXYData Result = new MyMathXYData();
        List<bool> ColFlag = new List<bool>();
        public individual bestOne=new individual();
        /// <summary>
        /// 种群大小
        /// </summary>
        int popsize;
        /// <summary>
        /// 种群中个体适应度累计
        /// </summary>
        double sumfitness;
        /// <summary>
        /// 染色体长度
        /// </summary>
        int lchrom;
        /// <summary>
        /// 交叉概率
        /// </summary>
        double pcross;
        /// <summary>
        /// 变异概率
        /// </summary>
        double pmutation;
        /// <summary>
        /// 当前代变异发生次数
        /// </summary>
        int nmutation;
        /// <summary>
        /// 当前代交叉发生次数
        /// </summary>
        int ncross;
        /// <summary>
        /// 存储一染色体所需字节数
        /// </summary>
        int chromsize;

        int alivenum = 20;

        int Cnt = 0;

        public AForgeGenetic()
        {

        }
        /// <summary>
        /// 初始化算法
        /// </summary>
        /// <param name="randomseed"></param>
        public void initAForgeGenetic(double[] chrom,bool[] flag)
        {
            nmutation = 0;
            ncross = 0;
            chromsize = lchrom;
            for (int i = 0; i < flag.Length; i++) 
            {
                ColFlag.Add(flag[i]);
            }
            initpop(chrom);
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="popsize"></param>
        /// <param name="lchrom"></param>
        /// <param name="maxgen"></param>
        /// <param name="pcross"></param>
        /// <param name="pmutation"></param>
        public void initdata(int popsize, int lchrom, double pcross, double pmutation)
        {
            this.popsize = popsize;
            if ((popsize % 2) != 0)
            {
                popsize++;
            }
            this.lchrom = lchrom;
            this.pcross = pcross;
            this.pmutation = pmutation;

        }
        /// <summary>
        /// 随机初始化种群
        /// </summary>
        public void initpop(double[] chrom)
        {
            int j, j1, k, stop;
            individual tmpdividual = new individual(chrom);
            for (j = 0; j < popsize; j++)
            {
                if (j == 0)
                {
                    ;
                }
                else
                {
                    tmpdividual = new individual(new double[12] { 0, 0, 0, 0, 0, 0 ,0,0,0,0,0,0});
                    for (k = 0; k < chromsize; k++)
                    {
                        if (ColFlag[k] == true)
                        {
                            if (k == (chromsize - 1))
                                stop = lchrom - k;
                            else
                                stop = k;
                            for (j1 = 1; j1 <= stop; j1++)
                            {
                                tmpdividual.chrom[k] = oldpop[0].chrom[k] + (randomperc() - 0.5) * oldpop[0].chrom[k];
                            }
                        }
                        else 
                        {
                            tmpdividual.chrom[k] = oldpop[0].chrom[k];
                        }
                    }
                }
                oldpop.Add(tmpdividual);
                oldpop[j].parent[0] = 0;     /* 初始父个体信息 */
                oldpop[j].parent[1] = 0;
                oldpop[j].xsite = 0;

                objfunc(true, j);       /* 计算初始适应度*/
            }
        }
        /// <summary>
        /// 选择优等进行繁殖
        /// </summary>
        public void generation()
        {
            int mate1, mate2, jcross, j = 0;
            /*  每代运算前进行预选 */
            preselect();
            for (int i = 0; i < popsize; i++)
            {
                if ((double)(oldpop[i].fitness / sumfitness) < (double)(0.9 / popsize))
                {
                    oldpop[i].fitness = 0;
                }
            }
            preselect();
            /* 选择, 交叉, 变异 */
            for (int i = 0; i < alivenum; i++)
            {
                newpop.Add(bestOne);
                if (i > 10)
                {
                    //mutation(i);

                }
                    objfunc(false, i);
            }
          

            j += alivenum;
            do
            {
                /* 挑选交叉配对 */
                mate1 = select();
                mate2 = select();
                /* 交叉和变异 */
                jcross = crossover(mate1, mate2);
                mutation(j);
                mutation(j + 1);
                /* 解码, 计算适应度 */
                objfunc(false, j);
                /*记录亲子关系和交叉位置 */
                newpop[j].parent[0] = mate1 + 1;
                newpop[j].xsite = jcross;
                newpop[j].parent[1] = mate2 + 1;
                objfunc(false, j + 1);
                newpop[j + 1].parent[0] = mate1 + 1;
                newpop[j + 1].xsite = jcross;
                newpop[j + 1].parent[1] = mate2 + 1;
                j = j + 2;
            }
            while (j < (popsize - 1));


        }
        /// <summary>
        /// 变异操作
        /// </summary>
        /// <param name="num"></param>
        public void mutation(int num)
        {
            int k;
            individual child = newpop[num];
            double mask;

            for (k = 1; k < chromsize; k++)
            {
                if (ColFlag[k] == true)
                {
                    mask = 0;
                    if (flip(pmutation) == true)
                    {
                        nmutation++;
                        mask = (double)(randomperc() - 0.5) * 0.1;
                    }
                    child.chrom[k] = child.chrom[k] * (1 + mask);
                }
                else
                {
                    child.chrom[k] = oldpop[0].chrom[k];
                }
            }
            newpop[num] = child;
        }
        /// <summary>
        /// 由两个父个体交叉产生两个子个体
        /// </summary>
        /// <param name="old1"></param>
        /// <param name="old2"></param>
        /// <param name="new1"></param>
        /// <param name="new2"></param>
        /// <returns></returns>
        int crossover(int old1, int old2)
        {
            int jcross, k;
            individual parent1 = oldpop[old1];
            individual parent2 = oldpop[old2];
            individual child1 = new individual(lchrom);
            individual child2 = new individual(lchrom);
            if (flip(pcross))
            {
                jcross = rnd(0, lchrom);
                ncross++;
                for (k = 0; k < chromsize; k++)
                {
                    if (ColFlag[k] == true)
                    {
                        if (jcross >= k)
                        {
                            child1.chrom[k] = parent1.chrom[k];
                            child2.chrom[k] = parent2.chrom[k];
                        }
                        else if (jcross == k)
                        {
                            child1.chrom[k] = parent2.chrom[k];
                            child2.chrom[k] = parent1.chrom[k];
                        }
                        else
                        {
                            child1.chrom[k] = parent1.chrom[k];
                            child2.chrom[k] = parent2.chrom[k];
                        }
                    }
                }
            }
            else
            {
                for (k = 0; k < chromsize; k++)
                {
                    child1.chrom[k] = parent1.chrom[k];
                    child2.chrom[k] = parent2.chrom[k];
                }
                jcross = 0;
            }

            oldpop[old1] = parent1;
            oldpop[old2] = parent2;
            newpop.Add(child1);
            newpop.Add(child1);
            return (jcross);
        }
        /// <summary>
        /// 每代运算前进行预选
        /// </summary>
        public void preselect()
        {
            int j;
            sumfitness = 0;
            for (j = 0; j < popsize; j++) sumfitness += oldpop[j].fitness;
        }
        /// <summary>
        /// 计算适应度函数值
        /// </summary>
        /// <param name="?"></param>
        public void objfunc(bool Isold, int num)
        {

            individual critter;
            if (Isold == true)
            {
                critter = oldpop[num];
            }
            else
            {
                critter = newpop[num];
            }
            critter.varible = 0.0;
            //自己的算法

            UserFunc(ref critter);


            if (Isold == true)
            {
                oldpop[num] = critter;
            }
            else
            {
                newpop[num] = critter;
            }
        }
        /// <summary>
        /// 轮盘赌选择
        /// </summary>
        /// <returns></returns>
        public int select()
        {
            double sum, pick;
            int i;
            pick = randomperc();
            sum = 0;
            if (sumfitness != 0)
            {
                for (i = 0; (sum < pick) && (i < popsize); i++)
                {
                    sum += oldpop[i].fitness / sumfitness;
                }
            }
            else
            {
                i = rnd(1, popsize);
            }
            return (i - 1);
        }
        /// <summary>
        /// 计算最优个体
        /// </summary>
        public void bestindividual() 
        {
            if (bestOne.fitness == 0)
            {
                for (int i = 0; i < oldpop.Count; i++)
                {
                    if (oldpop[i].fitness != 0)
                    {
                        bestOne = oldpop[i];
                        break;
                    }
                }
            }
            for (int i = 0; i < oldpop.Count; i++) 
            {
                if (bestOne.varible > oldpop[i].varible && oldpop[i].varible!=0) 
                {
                    bestOne = oldpop[i];
                }
            }
        }

        public void updata()
        {
            for (int i = 0; i < newpop.Count; i++) 
            {
                  oldpop[i] = newpop[i];   
            }
            newpop = new List<individual>();
        }
        /// <summary>
        /// 结果清楚
        /// </summary>
        public void clear() 
        {
         //   Result.Clear();
        }
        #region USER_PART
        public MyMathXYData Data = new MyMathXYData();
  

        public List<double> Data1 = new List<double>();
        public List<double> Data2 = new List<double>();
        public List<double> Data3 = new List<double>();
        public List<double> Data4 = new List<double>();
        public void Add(string dataIn)
        {
            try
            {
                string[] tmpValue = dataIn.Split('=');
                Data1.Add(double.Parse(tmpValue[0]));
                Data2.Add(double.Parse(tmpValue[1]));
                Data3.Add(double.Parse(tmpValue[2]));
                Data4.Add(double.Parse(tmpValue[3]));
            }
            catch
            {
                return;
            }
        }
        public void UserFunc(ref individual critter) 
        {
            List<double> tmp = critter.Getchrom();
            MyMath.Matrix MatA = new MyMath.Matrix(3);
            MyMath.Matrix MatB = new MyMath.Matrix(3, 1);
            MyMath.Matrix Xt2 = new MyMath.Matrix(3, 1);
            MyMath.Matrix Xt1 = new MyMath.Matrix(3, 1);
            double dt = 0.02;
            double err=0;
            double adb=10000;
            MatA.InitData(tmp.Take(9).ToList());
            tmp.RemoveRange(0,9);
            MatB.InitData(tmp);
            Xt1[1, 2] = Data1[0];// /5d/20d;
            Xt1[1, 3] = Data2[0];// *1.23d / 10d;
            Xt1[1, 1] = Data3[0];// / 5d/5d/20d;
            critter.varible = 0;
            critter.fitness = 0;
            for (int i = 1; i < Data1.Count; i++) 
            {
                     Xt2 = (MatA * Xt1 + MatB * Data4[i]) * dt + Xt1;//*(26.0d/10000.0d)
                     Xt1[1, 2] = Data1[i];// /5d/20d;
                     Xt1[1, 3] = Data2[i];// * 1.23d/10d;
                     Xt1[1, 1] = Data3[i];// / 5d/5d/20d;
                     err += ((Xt2[1, 1] - Xt1[1, 1]) * (Xt2[1, 1] - Xt1[1, 1]) + (Xt2[1, 2] - Xt1[1, 2]) * (Xt2[1, 2] - Xt1[1, 2]) * dt * dt + (Xt2[1, 3] - Xt1[1, 3]) * (Xt2[1, 3] - Xt1[1, 3]));
                    
                     err = err / adb;
                     critter.varible += err;
                     critter.fitness +=1/err;
            }
            Result.DataPoint.Add(new InDataPoint(++Cnt - popsize / 2, critter.varible));
            if (Result.DataPoint.Count > popsize) 
            {
                Result.DataPoint.RemoveRange(0, Result.DataPoint.Count - popsize);
            }
            if (Cnt == popsize) 
            {
                Cnt = 0;
            }
            //double dt=0.02;
            //double towe = 0.05;
            //double[] ddv = new double[Data.DataPoint.Count+2];
            //double[] dv = new double[Data.DataPoint.Count+2];
            //double[] v = new double[Data.DataPoint.Count+2];  
            //double err=0;
            //double derr=0;
            //for (int i = 0; i < Data.DataPoint.Count+1; i++) 
            //{
            //    if (i < 2)
            //    {
            //        ddv[i] = 0;
            //        dv[i] = 0;
            //        v[i] = 0;
            //        err = 0;
            //        derr = 0;
            //    }
            //    else 
            //    {
            //        double ut = Data.DataPoint[i - 1].Y_Value * 8 / 10000;
            //        double vy = Data.DataPoint[i - 2].X_Value / 6675.0  ;
            //        ddv[i] = critter.chrom[3] * critter.chrom[4] * v[i - 1] + ut / (critter.chrom[4] * critter.chrom[1] * critter.chrom[5] - critter.chrom[3] * critter.chrom[2] * critter.chrom[5]) + towe;
            //        dv[i] = dv[i - 1] + ddv[i-1] * dt;
            //        v[i] = v[i - 1] + dv[i - 1] * dt;

            //        err = vy - v[i];
            //  //      derr += ((Data.DataPoint[i - 1].Y_Value - v[i] - vy) - (v[i] - v[i - 1])) / dt;

            //        critter.fitness += (100.0 / err * err) / (double)Data.DataPoint.Count;
            //        critter.varible += MyMath.Abs(err) / (double)Data.DataPoint.Count;

            //    }

            //}

            //Result.DataPoint.Add(new InDataPoint(Result.DataPoint.Count - popsize/2, critter.varible));
            //if (MyMath.Abs(critter.varible) > 10000)
            //{
            //    critter.fitness = 0;
            //    critter.varible = 0;
            //}
        }
        #endregion
        #region 随机数发生
        Random Random1 = new Random();
        private double randomperc()
        {
             return Random1.NextDouble();
        }
        private bool flip(double Value)
        {
             if(Value>Random1.NextDouble())
             {
                return true;
             }
             else
             {
                return false;
             }
        }
        private int rnd(int Value1,int Value2)
        {
            if(Value1<Value2)
            {
            return Random1.Next(Value1,Value2);
            }
            else if(Value1>Value2)
            {
            return Random1.Next(Value2,Value1);
            }
            else
            {
            return Value1;
            }
        }
        #endregion
        #region tostring
        public override string ToString()
        {
            string tmp=string.Empty;
            for (int i = 0; i < this.bestOne.chrom.Count; i++) 
            {
                tmp += this.bestOne.chrom[i].ToString("G7")+":";
            }
               
            return tmp;
        }
        #endregion
    }

}
