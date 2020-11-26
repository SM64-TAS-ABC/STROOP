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

        public static string GetString<K,V>(Dictionary<K,V> dictionary)
        {
            List<string> entries = new List<string>();
            foreach (var entry in dictionary)
            {
                entries.Add("(" + entry.Key + "," + entry.Value + ")");
            }
            return "[" + string.Join(",", entries) + "]";
        }

        public static void TransferDictionary<K,V>(Dictionary<K, V> sender, Dictionary<K, V> receiver)
        {
            receiver.Clear();
            foreach (K key in sender.Keys)
            {
                receiver[key] = sender[key];
            }
        }
    }
}
