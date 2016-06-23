using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Extensions
{
    public static class WatchVariableExtensions
    {

        public static uint GetRamAddress(this WatchVariable watchVar, uint offset)
        {
            uint address = watchVar.OtherOffset ? offset + watchVar.Address : watchVar.Address;
            return address & 0x0FFFFFFF;
        }

        public static uint GetProcessAddress(this WatchVariable watchVar, ProcessStream stream, uint offset)
        {
            uint address = GetRamAddress(watchVar, offset);
            return (uint) LittleEndianessAddressing.AddressFix(
                (int)(address + stream.ProcessMemoryOffset), watchVar.GetByteCount())
                & 0x0FFFFFFF;
        }

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
            bool value = (dataValue != 0x00);
            value = watchVar.InvertBool ? !value : value;
            return value;
        }


        public static void SetBoolValue(this WatchVariable watchVar, ProcessStream stream, uint offset, bool value)
        {
            // Get dataBytes
            var byteCount = TypeSize[watchVar.Type];
            var address = watchVar.OtherOffset ? offset + watchVar.Address : watchVar.Address;
            var dataBytes = stream.ReadRam(address, byteCount, watchVar.AbsoluteAddressing);

            if (watchVar.InvertBool)
                value = !value;

            // Get Uint64 value
            var intBytes = new byte[8];
            dataBytes.CopyTo(intBytes, 0);
            UInt64 dataValue = BitConverter.ToUInt64(intBytes, 0);

            // Apply mask
            if (watchVar.Mask.HasValue)
            {
                if (value)
                    dataValue |= watchVar.Mask.Value;
                else
                    dataValue &= ~watchVar.Mask.Value;
            }
            else
            {
                dataValue = value ? 1U : 0U;
            }

            var writeBytes = new byte[byteCount];
            var valueBytes = BitConverter.GetBytes(dataValue);
            Array.Copy(valueBytes, 0, writeBytes, 0, byteCount);

            stream.WriteRam(writeBytes, address, watchVar.AbsoluteAddressing);

        }

        public static bool SetStringValue(this WatchVariable watchVar, ProcessStream stream, uint offset, string value)
        {
            // Get dataBytes
            var byteCount = TypeSize[watchVar.Type];
            var address = watchVar.OtherOffset ? offset + watchVar.Address : watchVar.Address;
            var dataBytes = new byte[8];
            stream.ReadRam(address, byteCount, watchVar.AbsoluteAddressing).CopyTo(dataBytes,0);
            UInt64 oldValue = BitConverter.ToUInt64(dataBytes, 0);
            UInt64 newValue;

            // Handle hex variable
            if (ParsingUtilities.IsHex(value))
            {
                if (!ParsingUtilities.TryParseExtHex(value, out newValue))
                    return false;
            }
            // Handle floats
            else if (watchVar.Type == typeof(float))
            {
                float newFloatValue;
                if (!float.TryParse(value, out newFloatValue))
                    return false;

                // Get bytes
                newValue = BitConverter.ToUInt32(BitConverter.GetBytes(newFloatValue), 0);
            }
            else if (watchVar.Type == typeof(double))
            {
                double newFloatValue;
                if (double.TryParse(value, out newFloatValue))
                    return false;

                // Get bytes
                newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newFloatValue), 0);
            }
            else if (watchVar.Type == typeof(UInt64))
            {
                if (!UInt64.TryParse(value, out newValue))
                    return false;
            }
            else
            {
                Int64 tempInt;
                if (!Int64.TryParse(value, out tempInt))
                    return false;
                newValue = (UInt64)tempInt;
            }

            // Apply mask
            if (watchVar.Mask.HasValue)
                newValue = (newValue & watchVar.Mask.Value) | ((~watchVar.Mask.Value) & oldValue);


            var writeBytes = new byte[byteCount];
            var valueBytes = BitConverter.GetBytes(newValue);
            Array.Copy(valueBytes, 0, writeBytes, 0, byteCount);

            stream.WriteRam(writeBytes, address, watchVar.AbsoluteAddressing);
            return true;
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
