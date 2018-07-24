using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Managers;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Controls;
using STROOP.Extensions;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace STROOP
{
    public class CarretlessTextBox : TextBox
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);

        public CarretlessTextBox()
        {
        }

        public void HideTheCaret()
        {
            HideCaret(Handle);
        }

        public void ShowTheCaret()
        {
            ShowCaret(Handle);
        }
    }
}
