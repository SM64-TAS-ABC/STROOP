using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class EnumUtilities
    {
        public static List<T> GetEnumValues<T>(Type type)
        {
            Array array = Enum.GetValues(type);
            List<T> list = new List<T>();
            foreach (object obj in array)
            {
                list.Add((T)obj);
            }
            return list;
        }

        public static List<string> GetEnumStrings<T>(Type type)
        {
            return GetEnumValues<T>(type).ConvertAll(e => e.ToString());
        }
    }
}
