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
using STROOP.Structs.Configurations;
using Accord.Video.FFMPEG;

namespace STROOP
{
    public class CoinRingDisplayPanel : Panel
    {
        private readonly Image _coinImage;

        public CoinRingDisplayPanel()
        {
            DoubleBuffered = true;

            _coinImage = Config.ObjectAssociations.GetObjectImage("Yellow Coin");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 24; col++)
                {
                    Rectangle rect = GetRectangle(row, col);
                    e.Graphics.DrawImage(_coinImage, rect);
                }
            }
        }

        private Rectangle GetRectangle(int row, int col)
        {
            bool tooWide = Size.Width > Size.Height * 6;
            int totalWidth = tooWide ? Size.Height * 6 : Size.Width;
            int totalHeight = tooWide ? Size.Height : Size.Width / 6;

            int rectWidth = totalWidth / 24;
            int rectHeight = totalHeight / 4;

            return new Rectangle(col * rectWidth, row * rectHeight, rectWidth, rectHeight);
        }
    }
}
