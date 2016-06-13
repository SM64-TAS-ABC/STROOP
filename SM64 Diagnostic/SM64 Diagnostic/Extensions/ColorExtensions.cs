using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SM64_Diagnostic.Utilities
{
    public enum LightenColorMethod { Add, Multiply };
    public static class ColorExtensions
    {
        public static Color Lighten(this Color color, double amount, LightenColorMethod method)
        {
            int r = color.R;
            int b = color.B;
            int g = color.G;

            switch(method)
            {
                case LightenColorMethod.Add:
                    int addValue = (int) (amount * byte.MaxValue);
                    r += addValue;
                    b += addValue;
                    g += addValue;
                    r = Math.Min(r, byte.MaxValue);
                    b = Math.Min(b, byte.MaxValue);
                    g = Math.Min(g, byte.MaxValue);
                    break;

                case LightenColorMethod.Multiply:
                    double multiplyValue = amount + 1;
                    r = (int)(r * multiplyValue);
                    b = (int)(b * multiplyValue);
                    g = (int)(g * multiplyValue);
                    r = Math.Min(r, byte.MaxValue);
                    b = Math.Min(b, byte.MaxValue);
                    g = Math.Min(g, byte.MaxValue);
                    break;
            }

            return Color.FromArgb(r, g, b);
        }
    }
}
