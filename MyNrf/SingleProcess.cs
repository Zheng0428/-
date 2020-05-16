using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNrf
{
    class SingleProcess
    {



    }

    public class DataListforBalanceCar
    {
        public float ACC_Value;//加速度计值  1   
        public float Gyro_Value;//陀螺仪值    2
        public float SpeedL_Value;//左轮转速 3
        public float SpeedR_Value;//右轮转速 4
        public float OutL_Value;//左轮输出    5
        public float OutR_Value;//右轮输出    6

        public float Angle;
        public float DDAngle;
        public float Uout_Value;
        public float DUout_Value;
        public DataListforBalanceCar()
        {
            ;
        }
        public DataListforBalanceCar(float acc, float gyro, float speedl, float speedr, float outl, float outr)
        {
            this.ACC_Value = acc;
            this.Gyro_Value = gyro;
            this.SpeedL_Value = speedl;
            this.SpeedR_Value = speedr;
            this.OutL_Value = outl;
            this.OutR_Value = outr;
        }
    }

}
