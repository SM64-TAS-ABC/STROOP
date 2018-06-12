using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STROOP.Structs
{
    public class WatchVariableSpecialDictionary
    {
        private readonly Dictionary<string, (Func<uint, object>, Func<object, uint, bool>)> _dictionary;

        public WatchVariableSpecialDictionary()
        {
            _dictionary = new Dictionary<string, (Func<uint, object>, Func<object, uint, bool>)>();

        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public object this[string key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                if (value.GetType() == typeof((Func<uint, object>, Func<object, uint, bool>)))
                {
                    _dictionary.Add(key, ((Func<uint, object>, Func<object, uint, bool>))value);
                }
            }
        }
    }
}