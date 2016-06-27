using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SM64_Diagnostic.Utilities
{
    public static class ColorExtensions
    {
        public static Color Lighten(this Color color, double amount)
        {
            double red = (255 - color.R) * amount + color.R;
            double green = (255 - color.G) * amount + color.G;
            double blue = (255 - color.B) * amount + color.B;
            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        public static Color Darken(this Color color, double amount)
        {
            return Lighten(color, -amount);
        }
    }
}
