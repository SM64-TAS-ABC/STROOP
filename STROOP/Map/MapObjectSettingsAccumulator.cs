using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace STROOP.Map
{
    public class MapObjectSettingsAccumulator
    {
        private readonly Dictionary<string, object> _dictionary;

        public MapObjectSettingsAccumulator()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public void ApplySettings(MapObjectSettings settings)
        {
            FieldInfo[] fieldInfos = typeof(MapObjectSettings).GetFields();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                dictionary[fieldInfo.Name] = fieldInfo.GetValue(settings);
            }
            foreach (string key1 in dictionary.Keys)
            {
                object value1 = dictionary[key1];
                if (key1.StartsWith("Change") && value1.Equals(true))
                {
                    string key2 = "New" + key1.Substring(6);
                    object value2 = dictionary[key2];
                    _dictionary[key1] = value1;
                    _dictionary[key2] = value2;
                }
            }
        }

        public override string ToString()
        {
            return DictionaryUtilities.GetString(_dictionary);
        }
    }
}
