using System;
using System.Collections.Generic;
using System.Drawing;

namespace STROOP.Utilities
{
    public static class HexUtilities
    {
        public static string Format(object number, int? numDigits = null, bool usePrefix = true)
        {
            if (number is float floatNumber)
            {
                byte[] bytes = BitConverter.GetBytes(floatNumber);
                number = BitConverter.ToUInt32(bytes, 0);
            }
            string numDigitsString = numDigits.HasValue ? numDigits.Value.ToString() : "";
            string prefix = usePrefix ? "0x" : "";
            return prefix + String.Format("{0:X" + numDigitsString + "}", number);
        }
    }
}
