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
        private static readonly List<uint> _coinRingSpawnerAddresses =
            new List<uint>()
            {
                0x80349028,
                0x8034C6C8,
                0x80349748,
                0x803499A8,
                0x80349C08,
            };

        private static readonly List<(int row, int col)> _coinOffsets =
            new List<(int row, int col)>()
            {
                (2,4),
                (1,3),
                (0,2),
                (1,1),
                (2,0),
                (3,1),
                (4,2),
                (3,3),
            };

        private readonly Image _coinImage;

        public CoinRingDisplayPanel()
        {
            DoubleBuffered = true;

            _coinImage = Config.ObjectAssociations.GetObjectImage("Yellow Coin");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (int coinRingIndex = 0; coinRingIndex < 5; coinRingIndex++)
            {
                uint coinRingSpawnerAddress = _coinRingSpawnerAddresses[coinRingIndex];
                List<bool> coinPresents = new List<bool>();
                for (uint mask = 0x01; mask <= 0x80; mask <<= 1)
                {
                    coinPresents.Add(Config.Stream.GetByte(coinRingSpawnerAddress + 0xF7, mask: mask) == 0);
                }

                int coinRingCol = 6 * coinRingIndex;
                for (int coinIndex = 0; coinIndex < coinPresents.Count; coinIndex++)
                {
                    if (!coinPresents[coinIndex]) continue;
                    (int row, int relCol) = _coinOffsets[coinIndex];
                    int col = coinRingCol + relCol;
                    e.Graphics.DrawImage(_coinImage, GetRectangle(row, col));
                }
            }
        }

        private Rectangle GetRectangle(int row, int col)
        {
            double ratio = 29 / 5;
            int unitsWide = 29;

            bool tooWide = Size.Width > Size.Height * ratio;
            double totalWidth = tooWide ? Size.Height * ratio : Size.Width;
            int rectWidth = (int)(totalWidth / unitsWide);

            return new Rectangle(col * rectWidth, row * rectWidth, rectWidth, rectWidth);
        }
    }
}
