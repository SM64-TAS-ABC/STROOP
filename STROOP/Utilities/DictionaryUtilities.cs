using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;

namespace STROOP.Utilities
{
    public static class DictionaryUtilities
    {
        public static Dictionary<V,K> ReverseDictionary<K,V>(Dictionary<K,V> dictionary)
        {
            Dictionary<V, K> reverseDictionary = new Dictionary<V, K>();
            dictionary.ToList().ForEach(keyValuePair =>
            {
                reverseDictionary.Add(keyValuePair.Value, keyValuePair.Key);
            });
            return reverseDictionary;
        }
    }
}
