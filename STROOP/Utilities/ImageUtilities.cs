using STROOP.Extensions;
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
        public static Image CreateMultiImage(List<Image> images, int width, int height)
        {
            Image multiBitmap = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(multiBitmap))
            {
                int count = images.Count();
                int numCols = (int)Math.Ceiling(Math.Sqrt(count));
                int numRows = (int)Math.Ceiling(count / (double)numCols);
                int imageSize = Math.Min(width, height) / numCols;
                foreach (int row in Enumerable.Range(0, numRows))
                {
                    foreach (int col in Enumerable.Range(0, numCols))
                    {
                        int index = row * numCols + col;
                        if (index >= count) break;
                        Image image = images[index];
                        Rectangle rect = new Rectangle(col * imageSize, row * imageSize, imageSize, imageSize);
                        Rectangle zoomedRect = rect.Zoom(image.Size);
                        gfx.DrawImage(image, zoomedRect);
                    }
                }
            }
            return multiBitmap;
        }

        public static Image ChangeTransparency(Image image, byte alpha)
        {
            Bitmap originalBitmap = new Bitmap(image);
            Bitmap transparentBitmap = new Bitmap(image.Width, image.Height);

            Color originalColor = Color.Black;
            Color transparentColor = Color.Black;

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    originalColor = originalBitmap.GetPixel(x, y);
                    transparentColor = Color.FromArgb(alpha, originalColor.R, originalColor.G, originalColor.B);
                    transparentBitmap.SetPixel(x, y, transparentColor);
                }
            }

            return transparentBitmap;
        }

        public static bool IsOnlyColor(Image image, Color onlyColor)
        {
            if (image == null) return false;
            Bitmap bitmap = new Bitmap(image);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    if (color != onlyColor && color.A != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Image ChangeColor(Image image, Color changeColor)
        {
            if (image == null) return null;
            Bitmap originalBitmap = new Bitmap(image);
            Bitmap transparentBitmap = new Bitmap(image.Width, image.Height);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color oldColor = originalBitmap.GetPixel(x, y);
                    Color newColor = oldColor.A == 0 ? oldColor : changeColor;
                    transparentBitmap.SetPixel(x, y, newColor);
                }
            }
            return transparentBitmap;
        }
    }
}
