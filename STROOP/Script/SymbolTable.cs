using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using OpenTK.Graphics;

namespace STROOP.Script
{
    public static class SymbolTable
    {
        private static Dictionary<string, object> _dictionary;

        public static void Reset()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public static void Set(string key, object value)
        {
            _dictionary[key] = value;
        }

        public static object Get(string key)
        {
            return _dictionary[key];
        }

        public static bool Has(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public static string GetString()
        {
            return DictionaryUtilities.GetString(_dictionary);
        }
    }
}
