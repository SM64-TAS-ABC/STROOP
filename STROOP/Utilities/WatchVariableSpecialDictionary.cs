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
        private readonly Dictionary<string, (Func<uint, object>, Func<object, bool, uint, bool>)> _dictionary;

        public WatchVariableSpecialDictionary()
        {
            _dictionary = new Dictionary<string, (Func<uint, object>, Func<object, bool, uint, bool>)>();
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public (Func<uint, object>, Func<object, bool, uint, bool>) Get(string key)
        {
            return _dictionary[key];
        }

        public void Add(string key, (Func<uint, object>, Func<object, bool, uint, bool>) value)
        {
            _dictionary[key] = value;
        }

        public void Add(string key, (Func<uint, object>, Func<double, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<double, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                double? doubleValue = ParsingUtilities.ParseDoubleNullable(objectValue);
                if (!doubleValue.HasValue) return false;
                return setter(doubleValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<float, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<float, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                float? floatValue = ParsingUtilities.ParseFloatNullable(objectValue);
                if (!floatValue.HasValue) return false;
                return setter(floatValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<int, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<int, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                int? intValue = ParsingUtilities.ParseIntNullable(objectValue);
                if (!intValue.HasValue) return false;
                return setter(intValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<uint, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<uint, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                uint? uintValue = ParsingUtilities.ParseUIntNullable(objectValue);
                if (!uintValue.HasValue) return false;
                return setter(uintValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<short, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<short, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                short? shortValue = ParsingUtilities.ParseShortNullable(objectValue);
                if (!shortValue.HasValue) return false;
                return setter(shortValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<ushort, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<ushort, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                ushort? ushortValue = ParsingUtilities.ParseUShortNullable(objectValue);
                if (!ushortValue.HasValue) return false;
                return setter(ushortValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<byte, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<byte, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                byte? byteValue = ParsingUtilities.ParseByteNullable(objectValue);
                if (!byteValue.HasValue) return false;
                return setter(byteValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<sbyte, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<sbyte, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                sbyte? sbyteValue = ParsingUtilities.ParseSByteNullable(objectValue);
                if (!sbyteValue.HasValue) return false;
                return setter(sbyteValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<bool, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<bool, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                bool? boolValue = ParsingUtilities.ParseBoolNullable(objectValue);
                if (!boolValue.HasValue) return false;
                return setter(boolValue.Value, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<string, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<string, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                if (objectValue == null) return false;
                return setter(objectValue.ToString(), setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

        public void Add(string key, (Func<uint, object>, Func<PositionAngle, bool, uint, bool>) value)
        {
            (Func<uint, object> getter, Func<PositionAngle, bool, uint, bool> setter) = value;
            Func<object, bool, uint, bool> newSetter = (object objectValue, bool setManually, uint address) =>
            {
                if (objectValue == null) return false;
                PositionAngle posAngle = PositionAngle.FromString(objectValue.ToString());
                if (posAngle == null) return false;
                if (posAngle.IsSelfOrPoint()) return false;
                return setter(posAngle, setManually, address);
            };
            _dictionary[key] = (getter, newSetter);
        }

    }
}