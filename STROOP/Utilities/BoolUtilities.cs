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

        public static CheckState GetCheckState(List<bool> bools)
        {
            if (bools.Count == 0) return CheckState.Indeterminate;
            if (bools.All(b => b)) return CheckState.Checked;
            if (bools.All(b => !b)) return CheckState.Unchecked;
            return CheckState.Indeterminate;
        }
    }
}
