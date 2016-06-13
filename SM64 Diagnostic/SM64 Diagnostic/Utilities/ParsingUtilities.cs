using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;

namespace SM64_Diagnostic.Utilities
{
    public static class ParsingUtilities
    {

        public static uint ParseHex(string str)
        {
            return uint.Parse(str.Substring(str.IndexOf("0x") + 2), NumberStyles.HexNumber);
        }

        public static UInt64 ParseExtHex(string str)
        {
            return UInt64.Parse(str.Substring(str.IndexOf("0x") + 2), NumberStyles.HexNumber);
        }

        public static bool TryParseHex(string str, out uint hex)
        {
            // This is what you call lazy programming
            try
            {
                hex = ParseHex(str);
            }
            catch (FormatException)
            {
                hex = new uint();
                return false;
            }

            return true;
        }
    }
}
