using System.Drawing;


namespace MyNrf.OpenGL的一些操作
{
    public class OpenGLPublicFunction
    {
        #region 从.Net的Color类型转换成OpenGL可用的浮点型

        /// <summary>
        /// 从.Net的Color类型转换成OpenGL可用的浮点型 
        /// 返回一个float数组，数组长度为3，r,g,b分别对应着数组索引的0,1,2
        /// </summary>
        /// <param name="dotNetColor">.Net类型的颜色</param>
        /// <returns>返回一个float数组，数组长度为3，r,g,b分别对应着数组索引的0,1,2</returns>
        public static float[] GetGLColorFromDotNetColor(Color dotNetColor)
        {
            float[] floatColor = new float[3];

            floatColor[0] = ((float)dotNetColor.R) / 255;

            floatColor[1] = ((float)dotNetColor.G) / 255;

            floatColor[2] = ((float)dotNetColor.B) / 255;

            return floatColor;
        }

        public static float[] GetGLColorFromDotNetColor(Color dotNetColor,float alpha)
        {
            float[] floatColor = new float[4];

            floatColor[0] = ((float)dotNetColor.R) / 255;

            floatColor[1] = ((float)dotNetColor.G) / 255;

            floatColor[2] = ((float)dotNetColor.B) / 255;

            floatColor[3] = ((float)alpha) / 100;

            return floatColor;
        }

        #endregion
    }
}
