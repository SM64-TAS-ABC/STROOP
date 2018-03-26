using System;
using System.Collections.Generic;
using System.Drawing;

namespace STROOP.Utilities
{
    public static class HexUtilities
    {
        public static string Format(object number, int? numDigits = null, bool usePrefix = true)
        {
            string numDigitsString = numDigits.HasValue ? numDigits.Value.ToString() : "";
            string prefix = usePrefix ? "0x" : "";
            return prefix + String.Format("{0:X" + numDigitsString + "}", number);
        }
    }
}
