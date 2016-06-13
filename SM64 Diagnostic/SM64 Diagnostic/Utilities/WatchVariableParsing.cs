using SM64_Diagnostic.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public static class WatchVariableParsingExtensions
    {
        public static string GetStringValue(this WatchVariable watchVar, ProcessStream stream, uint offset)
        {
            // Get dataBytes
            var byteCount = TypeSize[watchVar.Type];
            var dataBytes = stream.ReadRam(watchVar.OtherOffset ? offset + watchVar.Address 
                : watchVar.Address, byteCount, watchVar.AbsoluteAddressing);

            // Parse floating point
            if (!watchVar.UseHex && (watchVar.Type == typeof(float) || watchVar.Type == typeof(double)))
            {
                if (watchVar.Type == typeof(float))
                    return BitConverter.ToSingle(dataBytes, 0).ToString();

                if (watchVar.Type == typeof(double))
                    return BitConverter.ToDouble(dataBytes, 0).ToString();
            }

            // Get Uint64 value
            var intBytes = new byte[8];
            dataBytes.CopyTo(intBytes, 0);
            UInt64 dataValue = BitConverter.ToUInt64(intBytes, 0);

            // Apply mask
            if (watchVar.Mask.HasValue)
                dataValue &= watchVar.Mask.Value;

            // Boolean parsing
            if (watchVar.IsBool)
                return (dataValue != 0x00).ToString();

            // Print hex
            if (watchVar.UseHex)
                return "0x" + dataValue.ToString("X" + byteCount * 2);
            else
                return dataValue.ToString();
        }

        public static bool GetBoolValue(this WatchVariable watchVar, ProcessStream stream, uint offset)
        {
            // Get dataBytes
            var byteCount = TypeSize[watchVar.Type];
            var dataBytes = stream.ReadRam(watchVar.OtherOffset ? offset + watchVar.Address
                : watchVar.Address, byteCount, watchVar.AbsoluteAddressing);

            // Get Uint64 value
            var intBytes = new byte[8];
            dataBytes.CopyTo(intBytes, 0);
            UInt64 dataValue = BitConverter.ToUInt64(intBytes, 0);

            // Apply mask
            if (watchVar.Mask.HasValue)
                dataValue &= watchVar.Mask.Value;

            // Boolean parsing
            return (dataValue != 0x00);
        }

        public static int GetByteCount(this WatchVariable watchVar)
        {
            return TypeSize[watchVar.Type];
        }

        public static Type GetStringType(string str)
        {
            return StringToType[str];
        }

        public static Dictionary<Type, int> TypeSize = new Dictionary<Type, int>()
        {
            {typeof(byte), 1},
            {typeof(char), 1},
            {typeof(Int16), 2},
            {typeof(UInt16), 2},
            {typeof(Int32), 4},
            {typeof(UInt32), 4},
            {typeof(Int64), 8},
            {typeof(UInt64), 8},
            {typeof(float), 4},
            {typeof(double), 4}
        };

        public static Dictionary<String, Type> StringToType = new Dictionary<string, Type>()
        {
            { "byte", typeof(byte) },
            { "char", typeof(char) },
            { "int16", typeof(Int16) },
            { "uint16", typeof(UInt16) },
            { "int32", typeof(Int32) },
            { "uint32", typeof(UInt32) },
            { "int64", typeof(Int64) },
            { "uint64", typeof(UInt64) },
            { "float", typeof(float) },
            { "double", typeof(double) },
        };
    }
}
