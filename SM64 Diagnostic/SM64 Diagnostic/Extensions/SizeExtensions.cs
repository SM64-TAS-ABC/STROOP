using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SM64_Diagnostic.Utilities
{
    public static class SizeExtensions
    {
        public static Size Divide (this Size a, int b)
        {
            return new Size(a.Width / b, a.Height / b);
        }
    }
}
