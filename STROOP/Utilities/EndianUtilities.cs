using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class EndianUtilities
    {
        public static int SwapEndianness(int index)
        {
            int baseValue = (index / 4) * 4;
            int modValue = index % 4;
            return baseValue + (3 - modValue);
        }
    }
}
