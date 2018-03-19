using System;
using System.Collections.Generic;
using System.Drawing;

namespace STROOP.Utilities
{
    public static class HexUtilities
    {
        public static string Format(object number, int? numDigits = null)
        {
            string numDigitsString = numDigits.HasValue ? numDigits.Value.ToString() : "";
            return "0x" + String.Format("{0:X" + numDigitsString + "}", number);
        }
    }
}
