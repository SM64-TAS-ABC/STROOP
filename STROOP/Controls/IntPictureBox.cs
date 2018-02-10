using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Controls
{
    /// <summary>
    /// Inherits from PictureBox; adds Interpolation Mode Setting
    /// </summary>
    public class IntPictureBox : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.High;

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(paintEventArgs);
        }
    }
}
