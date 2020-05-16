using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNrf
{
    public partial class MyCopy : Form
    {
        private System.Drawing.Point mousePosition;
       public List<CopyDataInfo> data = new List<CopyDataInfo>();
        public MyCopy()
        {
            InitializeComponent();
        }

        private void myButton1_Click(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < this.myDataToCopy1.ListConData.Count; i++) 
            {
                if (this.myDataToCopy1.ListConData[i].cbxWaveOn.Checked == true) 
                {
                    this.data[i].ChooseOn = true;
                }

            }
            this.Close();
        }

        private void MyPar_Set_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mousePosition.X = e.X;
                this.mousePosition.Y = e.Y;
            }

        }
        private void MyPar_Set_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                this.Top = Control.MousePosition.Y - mousePosition.Y - SystemInformation.FrameBorderSize.Height;
                this.Left = Control.MousePosition.X - mousePosition.X - SystemInformation.FrameBorderSize.Width;
            }
        }
        private void FrmConData_Load(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < data.Count; i++)
            {
                this.myDataToCopy1.Add(data[i].Name, data[i].ChooseOn);
            }
            this.myDataToCopy1.Init();
        }

    }
}
