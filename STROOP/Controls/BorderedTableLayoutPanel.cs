using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Controls
{
    class BorderedTableLayoutPanel : TableLayoutPanel
    {
        static Random rng = new Random();
        Pen _borderPen = new Pen(Color.Red, 5);

        bool _showBorder = true;
        public bool ShowBorder
        {
            get
            {
                return _showBorder;
            }
            set
            {
                if (_showBorder == value)
                    return;

                _showBorder = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get
            {
                return _borderPen.Color;
            }
            set
            {
                if (_borderPen.Color == value)
                    return;

                _borderPen.Color = value;
                Invalidate();
            }
        }

        public float BorderWidth
        {
            get
            {
                return _borderPen.Width;
            }
            set
            {
                if (_borderPen.Width == value)
                    return;

                _borderPen.Width = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var rec = DisplayRectangle;
            rec.Width -= 1;
            rec.Height -= 1;
            if (_showBorder)
                e.Graphics.DrawRectangle(_borderPen, rec);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _borderPen?.Dispose();
        }
    }
}
