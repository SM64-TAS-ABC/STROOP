using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class ImageUtilities
    {
        public static Image ChangeTransparency(Image image, byte alpha)
        {
            Bitmap originalBitmap = new Bitmap(image);
            Bitmap transparentBitmap = new Bitmap(image.Width, image.Height);

            Color originalColor = Color.Black;
            Color transparentColor = Color.Black;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Width; y++)
                {
                    originalColor = originalBitmap.GetPixel(x, y);
                    transparentColor = Color.FromArgb(alpha, originalColor.R, originalColor.G, originalColor.B);
                    transparentBitmap.SetPixel(x, y, transparentColor);
                }
            }

            return transparentBitmap;
        }
    }
}
