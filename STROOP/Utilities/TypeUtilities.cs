using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class TypeUtilities
    {
        public readonly static Dictionary<string, Type> StringToType = new Dictionary<string, Type>()
        {
            { "byte", typeof(byte) },
            { "sbyte", typeof(sbyte) },
            { "short", typeof(short) },
            { "ushort", typeof(ushort) },
            { "int", typeof(int) },
            { "uint", typeof(uint) },
            { "long", typeof(long) },
            { "ulong", typeof(ulong) },
            { "float", typeof(float) },
            { "double", typeof(double) },
        };

        public readonly static Dictionary<Type, string> TypeToString = new Dictionary<Type, string>()
        {
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
        };

        public readonly static Dictionary<Type, int> TypeSize = new Dictionary<Type, int>()
        {
            {typeof(byte), 1},
            {typeof(sbyte), 1},
            {typeof(short), 2},
            {typeof(ushort), 2},
            {typeof(int), 4},
            {typeof(uint), 4},
            {typeof(long), 8},
            {typeof(ulong), 8},
            {typeof(float), 4},
            {typeof(double), 8},
        };

        public readonly static Dictionary<Type, bool> TypeSign = new Dictionary<Type, bool>()
        {
            {typeof(byte), false},
            {typeof(sbyte), true},
            {typeof(short), true},
            {typeof(ushort), false},
            {typeof(int), true},
            {typeof(uint), false},
            {typeof(long), true},
            {typeof(ulong), false},
            {typeof(float), true},
            {typeof(double), true},
        };

        public readonly static Dictionary<int, Type> UnsignedByteType = new Dictionary<int, Type>()
        {
            {1, typeof(byte)},
            {2, typeof(ushort)},
            {4, typeof(uint)},
            {8, typeof(ulong)},
        };

        public readonly static List<string> SimpleTypeList =
            new List<string>()
            {
                "byte",
                "sbyte",
                "short",
                "ushort",
                "int",
                "uint",
                "float",
            };

        public readonly static List<string> InGameTypeList =
            new List<string>()
            {
                "byte",
                "sbyte",
                "short",
                "ushort",
                "int",
                "uint",
                "float",
                "double",
            };

        public static object ConvertBytes(Type type, string hexString, bool littleEndian)
        {
            if (hexString == null) return null;
            if (hexString.Length >= 2 && hexString.Substring(0, 2) == "0x") hexString = hexString.Substring(2);
            hexString = StringUtilities.ExactLength(hexString, 2 * TypeSize[type], true, '0');

            try
            {
                byte[] bytes = Enumerable.Range(0, hexString.Length)
                                            .ToList()
                                            .FindAll(i => i % 2 == 0)
                                            .ConvertAll(i => Convert.ToByte(hexString.Substring(i, 2), 16))
                                            .ToArray();
                return ConvertBytes(type, bytes, 0, littleEndian);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static object ConvertBytes(Type type, byte[] allBytes, int startIndex, bool littleEndian)
        {
            int typeSize = TypeSize[type];
            int modValue = startIndex % 4;
            int baseValue = startIndex - modValue;
            int newModValue = modValue;
            if (littleEndian)
            {
                if (typeSize == 2) newModValue = 2 - modValue;
                if (typeSize == 1) newModValue = 3 - modValue;
            }
            int newStartAddress = baseValue + newModValue;

            byte[] bytes = new byte[typeSize];
            for (int i = 0; i < typeSize; i++)
            {
                byte byteValue = allBytes[newStartAddress + i];
                int index = typeSize - 1 - i;
                bytes[index] = byteValue;
            }

            return ConvertBytes(type, bytes);
        }

        public static object ConvertBytes(Type type, byte[] bytes)
        {
            if (type == typeof(byte)) return bytes[0];
            if (type == typeof(sbyte)) return (sbyte)bytes[0];
            if (type == typeof(short)) return BitConverter.ToInt16(bytes, 0);
            if (type == typeof(ushort)) return BitConverter.ToUInt16(bytes, 0);
            if (type == typeof(int)) return BitConverter.ToInt32(bytes, 0);
            if (type == typeof(uint)) return BitConverter.ToUInt32(bytes, 0);
            if (type == typeof(float)) return BitConverter.ToSingle(bytes, 0);
            if (type == typeof(double)) return BitConverter.ToDouble(bytes, 0);
            throw new ArgumentOutOfRangeException();
        }

        public static byte[] GetBytes(object obj, int? fixedLength = null, Encoding encoding = null)
        {
            byte[] bytes;

            if (obj is byte byteValue) bytes = new byte[] { byteValue };
            else if (obj is sbyte sbyteValue) bytes = new byte[] { (byte)sbyteValue };
            else if (obj is short shortValue) bytes = BitConverter.GetBytes(shortValue);
            else if (obj is ushort ushortValue) bytes = BitConverter.GetBytes(ushortValue);
            else if (obj is int intValue) bytes = BitConverter.GetBytes(intValue);
            else if (obj is uint uintValue) bytes = BitConverter.GetBytes(uintValue);
            else if (obj is float floatValue) bytes = BitConverter.GetBytes(floatValue);
            else if (obj is double doubleValue) bytes = BitConverter.GetBytes(doubleValue);
            else if (obj is string stringValue)
            {
                if (encoding == null) throw new ArgumentOutOfRangeException();
                bytes = encoding.GetBytes(stringValue);
            }
            else throw new ArgumentOutOfRangeException();

            if (fixedLength.HasValue)
            {
                if (bytes.Length > fixedLength.Value)
                {
                    bytes = bytes.Take(fixedLength.Value).ToArray();
                }
                else if (bytes.Length < fixedLength.Value)
                {
                    bytes = bytes.Concat(new byte[fixedLength.Value - bytes.Length]).ToArray();
                }
            }

            return bytes;
        }

        public static bool IsNumber(object obj)
        {
            return obj is byte ||
                   obj is sbyte ||
                   obj is short ||
                   obj is ushort ||
                   obj is int ||
                   obj is uint ||
                   obj is long ||
                   obj is ulong ||
                   obj is float ||
                   obj is double;
        }

        public static bool IsIntegerNumber(object obj)
        {
            return obj is byte ||
                   obj is sbyte ||
                   obj is short ||
                   obj is ushort ||
                   obj is int ||
                   obj is uint ||
                   obj is long ||
                   obj is ulong;
        }

        public static byte[] ConvertHexStringToByteArray(string stringValue, bool swapEndianness)
        {
            if (stringValue == null || stringValue.Length % 2 == 1)
            {
                throw new ArgumentOutOfRangeException("stringValue must have even length");
            }

            byte[] bytes = new byte[stringValue.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string byteString = stringValue.Substring(i * 2, 2);
                byte byteValue = byte.Parse(byteString, NumberStyles.HexNumber);
                int index = swapEndianness ? bytes.Length - 1 - i : i;
                bytes[index] = byteValue;
            }
            return bytes;
        }

        public static bool IsSubtype(Type type1, Type type2)
        {
            return type1 == type2 || type1.IsSubclassOf(type2);
        }

        public static uint GetRelativeAddressFromAbsoluteAddress(uint addr, int byteCount)
        {
            UIntPtr addressPtr = new UIntPtr(addr);
            uint address = EndiannessUtilities.SwapAddressEndianness(
                Config.Stream.GetRelativeAddress(addressPtr, byteCount), byteCount);
            return address | 0x80000000;
        }
    }
}
