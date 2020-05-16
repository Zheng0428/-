using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace MyNrf
{
    public partial class MyForm : Form
    {
        #region 动画启动窗体
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        /*
        1. AW_SLIDE : 使用滑动类型, 默认为该类型. 当使用 AW_CENTER 效果时, 此效果被忽略
        2. AW_ACTIVE: 激活窗口, 在使用了 AW_HIDE 效果时不可使用此效果
        3. AW_BLEND: 使用淡入效果
        4. AW_HIDE: 隐藏窗口
        5. AW_CENTER: 与 AW_HIDE 效果配合使用则效果为窗口几内重叠,  单独使用窗口向外扩展.
        6. AW_HOR_POSITIVE : 自左向右显示窗口
        7. AW_HOR_NEGATIVE: 自右向左显示窗口
        8. AW_VER_POSITVE: 自顶向下显示窗口
        9. AW_VER_NEGATIVE : 自下向上显示窗口
        */
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;
        public const Int32 AW_ACTIVATE = 0x00020000;
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;
        #endregion
        public MyForm()
        {
            InitializeComponent();
            AnimateWindow(this.Handle, 100, AW_BLEND + AW_CENTER);
        }
        private void MyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //动态关闭窗体
            AnimateWindow(this.Handle, 100, AW_BLEND + AW_HIDE + AW_CENTER);
        }
        private void MyForm_Shown(object sender, EventArgs e)
        {
            
        }
    }
}
