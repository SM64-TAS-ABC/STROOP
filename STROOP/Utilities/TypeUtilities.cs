using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static object ConvertBytes(Type type, byte[] bytes, int startIndex, bool littleEndian)
        {
            if (type == typeof(byte)) return bytes[startIndex];
            if (type == typeof(sbyte)) return (sbyte)bytes[startIndex];
            if (type == typeof(short)) return BitConverter.ToInt16(bytes, startIndex);
            if (type == typeof(ushort)) return BitConverter.ToUInt16(bytes, startIndex);
            if (type == typeof(int)) return BitConverter.ToInt32(bytes, startIndex);
            if (type == typeof(uint)) return BitConverter.ToUInt32(bytes, startIndex);
            if (type == typeof(float)) return BitConverter.ToSingle(bytes, startIndex);
            if (type == typeof(double)) return BitConverter.ToDouble(bytes, startIndex);
            throw new ArgumentOutOfRangeException();
        }
    }
}
