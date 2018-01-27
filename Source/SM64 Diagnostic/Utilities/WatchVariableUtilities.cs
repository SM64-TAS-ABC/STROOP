using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public static class WatchVariableUtilities
    {
        public readonly static Dictionary<String, Type> StringToType = new Dictionary<string, Type>()
        {
            { "byte", typeof(byte) },
            { "sbyte", typeof(sbyte) },
            { "short", typeof(Int16) },
            { "ushort", typeof(UInt16) },
            { "int", typeof(Int32) },
            { "uint", typeof(UInt32) },
            { "long", typeof(Int64) },
            { "ulong", typeof(UInt64) },
            { "float", typeof(float) },
            { "double", typeof(double) },
        };

        public readonly static Dictionary<Type, int> TypeSize = new Dictionary<Type, int>()
        {
            {typeof(byte), 1},
            {typeof(sbyte), 1},
            {typeof(Int16), 2},
            {typeof(UInt16), 2},
            {typeof(Int32), 4},
            {typeof(UInt32), 4},
            {typeof(Int64), 8},
            {typeof(UInt64), 8},
            {typeof(float), 4},
            {typeof(double), 4},
        };

        public readonly static Dictionary<Type, bool> TypeSign = new Dictionary<Type, bool>()
        {
            {typeof(byte), false},
            {typeof(sbyte), true},
            {typeof(Int16), true},
            {typeof(UInt16), false},
            {typeof(Int32), true},
            {typeof(UInt32), false},
            {typeof(Int64), true},
            {typeof(UInt64), false},
            {typeof(float), true},
            {typeof(double), true},
        };

        public static BaseAddressTypeEnum GetBaseAddressType(string stringValue)
        {
            return (BaseAddressTypeEnum)Enum.Parse(typeof(BaseAddressTypeEnum), stringValue);
        }

        public static WatchVariableSubclass GetSubclass(string stringValue)
        {
            if (stringValue == null) return WatchVariableSubclass.Number;
            return (WatchVariableSubclass)Enum.Parse(typeof(WatchVariableSubclass), stringValue);
        }

        public static WatchVariableCoordinate GetCoordinate(string stringValue)
        {
            return (WatchVariableCoordinate)Enum.Parse(typeof(WatchVariableCoordinate), stringValue);
        }

        public static VariableGroup GetVariableGroup(string stringValue)
        {
            return (VariableGroup)Enum.Parse(typeof(VariableGroup), stringValue);
        }

        public static List<VariableGroup> ParseVariableGroupList(string stringValue)
        {
            List<VariableGroup> variableGroupList = new List<VariableGroup>();
            if (stringValue != null)
            {
                string[] groupNames = stringValue.Split(',');
                foreach (string groupName in groupNames)
                {
                    variableGroupList.Add(GetVariableGroup(groupName));
                }
            }
            return variableGroupList;
        }

        private static readonly List<uint> BaseAddressListZero = new List<uint> { 0 };
        private static readonly List<uint> BaseAddressListEmpy = new List<uint> { };
        
        public static List<uint> GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum baseAddressType)
        {
            switch (baseAddressType)
            {
                case BaseAddressTypeEnum.None:
                    return BaseAddressListZero;

                case BaseAddressTypeEnum.Absolute:
                    return BaseAddressListZero;

                case BaseAddressTypeEnum.Relative:
                    return BaseAddressListZero;

                case BaseAddressTypeEnum.Mario:
                    return new List<uint> { MarioConfig.StructAddress };

                case BaseAddressTypeEnum.MarioObj:
                    return new List<uint> { Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress) };

                case BaseAddressTypeEnum.Camera:
                    return new List<uint> { CameraConfig.CameraStructAddress };

                case BaseAddressTypeEnum.File:
                    return new List<uint> { Config.FileManager.CurrentFileAddress };

                case BaseAddressTypeEnum.Object:
                    return Config.ObjectManager.CurrentAddresses;

                case BaseAddressTypeEnum.Triangle:
                    {
                        uint triangleAddress = Config.TriangleManager.TriangleAddress;
                        return triangleAddress != 0 ? new List<uint>() { triangleAddress } : BaseAddressListEmpy;
                    }

                case BaseAddressTypeEnum.TriangleExertionForceTable:
                    return GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Triangle)
                        .ConvertAll(triangleAddress =>
                        {
                            uint exertionForceIndex = Config.Stream.GetByte(triangleAddress + TriangleOffsetsConfig.ExertionForceIndex);
                            return TriangleConfig.ExertionForceTableAddress + 2 * exertionForceIndex;
                        });

                case BaseAddressTypeEnum.InputCurrent:
                    return new List<uint> { InputConfig.CurrentInputAddress };

                case BaseAddressTypeEnum.InputJustPressed:
                    return new List<uint> { InputConfig.JustPressedInputAddress };

                case BaseAddressTypeEnum.InputBuffered:
                    return new List<uint> { InputConfig.BufferedInputAddress };

                case BaseAddressTypeEnum.Graphics:
                    return GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + ObjectSlotsConfig.BehaviorGfxOffset));

                case BaseAddressTypeEnum.Animation:
                    return GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + ObjectSlotsConfig.AnimationOffset));

                case BaseAddressTypeEnum.Waypoint:
                    return GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + ObjectSlotsConfig.WaypointOffset));

                case BaseAddressTypeEnum.Water:
                    return new List<uint> { Config.Stream.GetUInt32(MiscConfig.WaterPointerAddress) };

                case BaseAddressTypeEnum.Area:
                    return new List<uint> { Config.AreaManager.SelectedAreaAddress };

                case BaseAddressTypeEnum.HackedArea:
                    return new List<uint> { MiscConfig.HackedAreaAddress };

                case BaseAddressTypeEnum.CamHack:
                    return new List<uint> { CameraHackConfig.CameraHackStruct };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
