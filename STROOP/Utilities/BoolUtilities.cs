using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class BoolUtilities
    {
        public static bool Combine(params bool[] bools)
        {
            bool success = true;
            foreach (bool b in bools)
            {
                success &= b;
            }
            return success;
        }
    }
}
