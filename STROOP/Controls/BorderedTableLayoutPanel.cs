using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Controls
{
    class BorderedTableLayoutPanel : TableLayoutPanel
    {
        private readonly Pen _borderPen;

        public BorderedTableLayoutPanel()
        {
            _borderPen = new Pen(Color.Black, 1);
            _borderPen.Alignment = PenAlignment.Inset;
        }

        bool _showBorder = false;
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

                if (_showBorder)
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

                if (_showBorder)
                    Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_showBorder)
                e.Graphics.DrawRectangle(_borderPen, DisplayRectangle);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _borderPen?.Dispose();
        }
    }
}
