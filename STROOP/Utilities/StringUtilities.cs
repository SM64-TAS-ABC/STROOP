using STROOP.Forms;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class StringUtilities
    {
        public static string Cap(string stringValue, int length)
        {
            if (stringValue == null) return stringValue;
            if (stringValue.Length <= length) return stringValue;
            return stringValue.Substring(0, length);
        }

        public static string ExactLength(string stringValue, int length, bool leftAppend, char appendChar)
        {
            if (stringValue == null) return stringValue;
            if (stringValue.Length < length)
            {
                return leftAppend
                    ? stringValue.PadLeft(length, appendChar)
                    : stringValue.PadRight(length, appendChar);
            }
            if (stringValue.Length > length)
            {
                return leftAppend
                  ? stringValue.Substring(stringValue.Length - length)
                  : stringValue.Substring(0, length);
            }
            return stringValue;
        }

        public static string FormatIntegerWithSign(int num)
        {
            return (num > 0 ? "+" : "") + num;
        }

        public static void WriteLine(string format, params object[] args)
        {
            string formatted = String.Format(format, args);
            System.Diagnostics.Trace.WriteLine(formatted);
        }

    }
} 
