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
using STROOP.Models;

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

        private static readonly List<uint> _middleCoinAddresses =
            new List<uint>()
            {
                0x8034AF08,
                0x8034C928,
                0x8034B168,
                0x8034B3C8,
                0x8034B628,
            };

        private static readonly List<(int x, int y, int z)> _middleCoinPositions =
            new List<(int x, int y, int z)>()
            {
                (-1506,5517,1250),
                (-300,4200,1250),
                (1000,3600,1250),
                (2000,3600,1250),
                (3000,3600,1250),
            };

        private static readonly List<uint> _secretAddresses =
            new List<uint>()
            {
                0x80349288,
                0x8034B888,
                0x80349E68,
                0x8034A0C8,
                0x8034A328,
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
        private readonly Image _secretImage;

        public CoinRingDisplayPanel()
        {
            DoubleBuffered = true;

            _coinImage = Config.ObjectAssociations.GetObjectImage("Yellow Coin");
            _secretImage = Config.ObjectAssociations.GetObjectImage("Secret");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            List<ObjectDataModel> secrets = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Secret");
            List<ObjectDataModel> yellowCoins = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Yellow Coin");

            for (int coinRingIndex = 0; coinRingIndex < 5; coinRingIndex++)
            {
                // Get whether each coin is present
                uint coinRingSpawnerAddress = _coinRingSpawnerAddresses[coinRingIndex];
                List<bool> coinPresents = new List<bool>();
                for (uint mask = 0x01; mask <= 0x80; mask <<= 1)
                {
                    coinPresents.Add(Config.Stream.GetByte(coinRingSpawnerAddress + 0xF7, mask: mask) == 0);
                }

                // Draw the ring coins
                int coinRingCol = 6 * coinRingIndex;
                for (int coinIndex = 0; coinIndex < coinPresents.Count; coinIndex++)
                {
                    if (!coinPresents[coinIndex]) continue;
                    (int row, int relCol) = _coinOffsets[coinIndex];
                    int col = coinRingCol + relCol;
                    e.Graphics.DrawImage(_coinImage, GetRectangle(row, col));
                }

                // Draw the middle coins
                uint middleCoinAddress = _middleCoinAddresses[coinRingIndex];
                (int x, int y, int z) = _middleCoinPositions[coinRingIndex];
                bool middleCoinIsPresent = yellowCoins.Any(coin =>
                {
                    if (coin.Address != middleCoinAddress) return false;
                    if (coin.X != x) return false;
                    if (coin.Y != y) return false;
                    if (coin.Z != z) return false;
                    return true;
                });
                if (middleCoinIsPresent)
                {
                    double row = 1.5;
                    int col = coinRingCol + 2;
                    e.Graphics.DrawImage(_coinImage, GetRectangle(row, col));
                }

                // Draw the secrets
                uint secretAddress = _secretAddresses[coinRingIndex];
                if (secrets.Any(secret => secret.Address == secretAddress))
                {
                    double row = 2.5;
                    int col = coinRingCol + 2;
                    e.Graphics.DrawImage(_secretImage, GetRectangle(row, col));
                }
            }
        }

        private Rectangle GetRectangle(double row, double col)
        {
            double ratio = 29 / 5;
            int unitsWide = 29;

            bool tooWide = Size.Width > Size.Height * ratio;
            double totalWidth = tooWide ? Size.Height * ratio : Size.Width;
            int rectWidth = (int)(totalWidth / unitsWide);

            return new Rectangle((int)(col * rectWidth), (int)(row * rectWidth), rectWidth, rectWidth);
        }
    }
}
