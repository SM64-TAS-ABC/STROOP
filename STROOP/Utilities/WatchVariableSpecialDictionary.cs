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

        public (Func<uint, object>, Func<object, uint, bool>) Get(string key)
        {
            return _dictionary[key];
        }

        public void Add(string key, (Func<uint, object>, Func<object, uint, bool>) value)
        {
            _dictionary[key] = value;
        }

        public void Add(string key, (Func<uint, object>, Func<double, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<double, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                double? doubleValue = ParsingUtilities.ParseDoubleNullable(objectValue);
                if (!doubleValue.HasValue) return false;
                return setter(doubleValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<float, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<float, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                float? floatValue = ParsingUtilities.ParseFloatNullable(objectValue);
                if (!floatValue.HasValue) return false;
                return setter(floatValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<int, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<int, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                int? intValue = ParsingUtilities.ParseIntNullable(objectValue);
                if (!intValue.HasValue) return false;
                return setter(intValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<uint, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<uint, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                uint? uintValue = ParsingUtilities.ParseUIntNullable(objectValue);
                if (!uintValue.HasValue) return false;
                return setter(uintValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<short, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<short, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                short? shortValue = ParsingUtilities.ParseShortNullable(objectValue);
                if (!shortValue.HasValue) return false;
                return setter(shortValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<ushort, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<ushort, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                ushort? ushortValue = ParsingUtilities.ParseUShortNullable(objectValue);
                if (!ushortValue.HasValue) return false;
                return setter(ushortValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<byte, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<byte, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                byte? byteValue = ParsingUtilities.ParseByteNullable(objectValue);
                if (!byteValue.HasValue) return false;
                return setter(byteValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<sbyte, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<sbyte, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                sbyte? sbyteValue = ParsingUtilities.ParseSByteNullable(objectValue);
                if (!sbyteValue.HasValue) return false;
                return setter(sbyteValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<bool, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                bool? boolValue = ParsingUtilities.ParseBoolNullable(objectValue);
                if (!boolValue.HasValue) return false;
                return setter(boolValue.Value, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<string, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<string, uint, bool> setter) = value;
            Func<object, uint, bool> newSetter = (object objectValue, uint address) =>
            {
                return setter(objectValue.ToString(), address);
            };
            _dictionary[key] = (getter, newSetter);
        }

    }
}