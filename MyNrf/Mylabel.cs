using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNrf
{
    public partial class Mylabel : UserControl
    {
        public Mylabel()
        {
            InitializeComponent();
            lbl.Top = 0;
            lbl.Left = 0;
            lbl.BorderStyle = BorderStyle.None;
            lbl.Click += new EventHandler(UcLabel_Click);
            lbl.MouseMove += new MouseEventHandler(UcLabel_MouseMove);
            lbl.MouseLeave += new EventHandler(UcLabel_MouseLeave);
            lbl.Leave += new EventHandler(UcLabel_Leave);
            this.Controls.Add(lbl);
        }
                Label lbl = new Label();

        private Color mycolor;

        public Color MyColor
        {
            get 
            { 
                return mycolor; 
            }
            set 
            {
                mycolor = value;
                lbl.BackColor = mycolor;
            }
        }


        public void UcLabel_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = lbl.BackColor;
            colorDialog.ShowDialog();
            lbl.BackColor = colorDialog.Color;
            mycolor = colorDialog.Color;
        }

        private void UcLabel_Resize(object sender, EventArgs e)
        {
            lbl.Width = this.Width;
            lbl.Height = this.Height;
        }


        private void UcLabel_MouseMove(object sender, MouseEventArgs e)
        {
            lbl.BorderStyle = BorderStyle.Fixed3D;
        }

        private void UcLabel_MouseLeave(object sender, EventArgs e)
        {
            lbl.BorderStyle = BorderStyle.None;
        }

        private void UcLabel_Leave(object sender, EventArgs e)
        {
            if (lbl.BorderStyle == BorderStyle.FixedSingle)
            {
                lbl.BorderStyle = BorderStyle.None;
            }
        }
    }
}
