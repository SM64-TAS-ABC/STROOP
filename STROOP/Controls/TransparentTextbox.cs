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

namespace STROOP
{
    public class TransparentTextbox : TextBox
    {
        public TransparentTextbox()
        {
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }
    }
}
