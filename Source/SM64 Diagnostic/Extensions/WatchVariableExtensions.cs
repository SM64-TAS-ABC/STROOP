using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.ManagerClasses;

namespace SM64_Diagnostic.Extensions
{
    public static class WatchVariableExtensions
    {

        public static uint GetRamAddress(this WatchVariable watchVar, ProcessStream stream, uint offset)
        {
            if (watchVar.AbsoluteAddressing)
                return LittleEndianessAddressing.AddressFix((uint)(watchVar.Address - stream.ProcessMemoryOffset),
                    watchVar.GetByteCount());

            uint address = watchVar.OtherOffset ? offset + watchVar.Address : watchVar.Address;
            return address & 0x0FFFFFFF;
        }

        public static uint GetProcessAddress(this WatchVariable watchVar, ProcessStream stream, uint offset)
        {
            uint address = GetRamAddress(watchVar, stream, offset);
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
  
        public static void SetAngleStringValue(this WatchVariable watchVar, ProcessStream stream, uint offset, string value, WatchVariableControl.AngleViewModeType viewMode)
        {
            if (watchVar.Type != typeof(UInt32))
                return;

            UInt32 writeValue = 0;

            // Print hex
            if (ParsingUtilities.IsHex(value))
            {
                ParsingUtilities.TryParseHex(value, out writeValue);
                if (viewMode != WatchVariableControl.AngleViewModeType.Raw)
                    writeValue <<= 16;
            }
            else
            {
                switch (viewMode)
                {
                    case WatchVariableControl.AngleViewModeType.Raw:
                        uint.TryParse(value, out writeValue);
                        break;

                    case WatchVariableControl.AngleViewModeType.Signed:
                    case WatchVariableControl.AngleViewModeType.Unsigned:
                        uint.TryParse(value, out writeValue);
                        writeValue <<= 16;
                        break;
                        

                    case WatchVariableControl.AngleViewModeType.Degrees:
                        var degValue = double.Parse(value);
                        writeValue = (UInt16)(degValue / (360d / 65536));
                        writeValue <<= 16;
                        break;

                    case WatchVariableControl.AngleViewModeType.Radians:
                        var radValue = double.Parse(value);
                        writeValue = (UInt16)(radValue / (2 * Math.PI / 65536));
                        writeValue <<= 16;
                        break;
                }
            }
            var byteCount = TypeSize[watchVar.Type];
            var dataBytes = stream.WriteRam(BitConverter.GetBytes(writeValue), watchVar.OtherOffset ? offset + watchVar.Address
                : watchVar.Address, byteCount, watchVar.AbsoluteAddressing);
        }

        public static string GetAngleStringValue(this WatchVariable watchVar, ProcessStream stream, uint offset, WatchVariableControl.AngleViewModeType viewMode)
        {
            // Get dataBytes
            var byteCount = TypeSize[watchVar.Type];
            var dataBytes = stream.ReadRam(watchVar.OtherOffset ? offset + watchVar.Address
                : watchVar.Address, byteCount, watchVar.AbsoluteAddressing);

            if (watchVar.Type != typeof(UInt32))
                return "Error: angle not UInt32";

            // Get Uint64 value
            UInt32 dataValue = BitConverter.ToUInt32(dataBytes, 0);

            // Apply mask
            if (watchVar.Mask.HasValue)
                dataValue = (UInt32)(dataValue & watchVar.Mask.Value);

            // Print hex
            if (watchVar.UseHex)
            {
                if (viewMode == WatchVariableControl.AngleViewModeType.Raw)
                    return "0x" + dataValue.ToString("X8"); 
                else
                    return "0x" + ((UInt16)dataValue).ToString("X4");
            }

            switch(viewMode)
            {
                case WatchVariableControl.AngleViewModeType.Raw:
                    return dataValue.ToString();

                case WatchVariableControl.AngleViewModeType.Unsigned:
                    return ((UInt16)dataValue).ToString();

                case WatchVariableControl.AngleViewModeType.Signed:
                    return ((Int16)(dataValue)).ToString();

                case WatchVariableControl.AngleViewModeType.Degrees:
                    return (((UInt16)dataValue) * (360d / 65536)).ToString();

                case WatchVariableControl.AngleViewModeType.Radians:
                    return (((UInt16)dataValue) * (2 * Math.PI / 65536)).ToString();
            }

            return "Error: ang. parse";
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
