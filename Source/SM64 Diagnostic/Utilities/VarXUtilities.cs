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
    public static class VarXUtilities
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

        public static VarXSubclass GetVarXSubclass(string stringValue)
        {
            if (stringValue == null) return VarXSubclass.Number;
            return (VarXSubclass)Enum.Parse(typeof(VarXSubclass), stringValue);
        }

        public static VarXCoordinate GetVarXCoordinate(string stringValue)
        {
            return (VarXCoordinate)Enum.Parse(typeof(VarXCoordinate), stringValue);
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

        // TODO remove this version once var X is the norm
        public static List<uint> GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum? baseAddressType, bool returnNonEmptyList)
        {
            List<uint> output;
            switch (baseAddressType)
            {
                case BaseAddressTypeEnum.Absolute:
                    output = BaseAddressListZero;
                    break;
                case BaseAddressTypeEnum.Relative:
                    output = BaseAddressListZero;
                    break;
                case BaseAddressTypeEnum.Mario:
                    output = new List<uint> { Config.Mario.StructAddress };
                    break;
                case BaseAddressTypeEnum.MarioObj:
                    output = new List<uint> { Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress) };
                    break;
                case BaseAddressTypeEnum.Camera:
                    output = new List<uint> { Config.Camera.CameraStructAddress };
                    break;
                case BaseAddressTypeEnum.File:
                    output = new List<uint> { FileManager.Instance.CurrentFileAddress };
                    break;
                case BaseAddressTypeEnum.Object:
                    output = ObjectManager.Instance.CurrentAddresses;
                    break;
                case BaseAddressTypeEnum.Triangle:
                    output = new List<uint> { TriangleManager.Instance.TriangleAddress };
                    break;
                case BaseAddressTypeEnum.TriangleExertionForceTable:
                    uint triangleAddress = TriangleManager.Instance.TriangleAddress;
                    if (triangleAddress == 0)
                    {
                        output = BaseAddressListZero;
                    }
                    else
                    {
                        uint exertionForceIndex = Config.Stream.GetByte(triangleAddress + Config.TriangleOffsets.ExertionForceIndex);
                        output = new List<uint> { Config.Triangle.ExertionForceTableAddress + 2 * exertionForceIndex };
                    }
                    break;
                case BaseAddressTypeEnum.InputCurrent:
                    output = new List<uint> { Config.Input.CurrentInputAddress };
                    break;
                case BaseAddressTypeEnum.InputJustPressed:
                    output = new List<uint> { Config.Input.JustPressedInputAddress };
                    break;
                case BaseAddressTypeEnum.InputBuffered:
                    output = new List<uint> { Config.Input.BufferedInputAddress };
                    break;
                case BaseAddressTypeEnum.Graphics:
                    output = GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.BehaviorGfxOffset));
                    break;
                case BaseAddressTypeEnum.Animation:
                    output = GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.AnimationOffset));
                    break;
                case BaseAddressTypeEnum.Waypoint:
                    output = GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.WaypointOffset));
                    break;
                case BaseAddressTypeEnum.Water:
                    output = new List<uint> { Config.Stream.GetUInt32(Config.WaterPointerAddress) };
                    break;
                case BaseAddressTypeEnum.Area:
                    output = new List<uint> { AreaManager.Instance.SelectedAreaAddress };
                    break;
                case BaseAddressTypeEnum.HackedArea:
                    output = new List<uint> { Config.HackedAreaAddress };
                    break;
                case BaseAddressTypeEnum.CamHack:
                    output = new List<uint> { Config.CameraHack.CameraHackStruct };
                    break;
                case BaseAddressTypeEnum.None:
                    throw new ArgumentOutOfRangeException("None in switch statement");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (returnNonEmptyList && output.Count == 0)
            {
                output = BaseAddressListZero;
            }
            return output;
        }

        public static List<uint> GetBaseAddressListFromBaseAddressTypeVarX(BaseAddressTypeEnum baseAddressType)
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
                    return new List<uint> { Config.Mario.StructAddress };

                case BaseAddressTypeEnum.MarioObj:
                    return new List<uint> { Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress) };

                case BaseAddressTypeEnum.Camera:
                    return new List<uint> { Config.Camera.CameraStructAddress };

                case BaseAddressTypeEnum.File:
                    return new List<uint> { FileManager.Instance.CurrentFileAddress };

                case BaseAddressTypeEnum.Object:
                    return ObjectManager.Instance.CurrentAddresses;

                case BaseAddressTypeEnum.Triangle:
                    {
                        uint triangleAddress = TriangleManager.Instance.TriangleAddress;
                        return triangleAddress != 0 ? new List<uint>() { triangleAddress } : BaseAddressListEmpy;
                    }

                case BaseAddressTypeEnum.TriangleExertionForceTable:
                    return GetBaseAddressListFromBaseAddressTypeVarX(BaseAddressTypeEnum.Triangle)
                        .ConvertAll(triangleAddress =>
                        {
                            uint exertionForceIndex = Config.Stream.GetByte(triangleAddress + Config.TriangleOffsets.ExertionForceIndex);
                            return Config.Triangle.ExertionForceTableAddress + 2 * exertionForceIndex;
                        });

                case BaseAddressTypeEnum.InputCurrent:
                    return new List<uint> { Config.Input.CurrentInputAddress };

                case BaseAddressTypeEnum.InputJustPressed:
                    return new List<uint> { Config.Input.JustPressedInputAddress };

                case BaseAddressTypeEnum.InputBuffered:
                    return new List<uint> { Config.Input.BufferedInputAddress };

                case BaseAddressTypeEnum.Graphics:
                    return GetBaseAddressListFromBaseAddressTypeVarX(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.BehaviorGfxOffset));

                case BaseAddressTypeEnum.Animation:
                    return GetBaseAddressListFromBaseAddressTypeVarX(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.AnimationOffset));

                case BaseAddressTypeEnum.Waypoint:
                    return GetBaseAddressListFromBaseAddressTypeVarX(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.WaypointOffset));

                case BaseAddressTypeEnum.Water:
                    return new List<uint> { Config.Stream.GetUInt32(Config.WaterPointerAddress) };

                case BaseAddressTypeEnum.Area:
                    return new List<uint> { AreaManager.Instance.SelectedAreaAddress };

                case BaseAddressTypeEnum.HackedArea:
                    return new List<uint> { Config.HackedAreaAddress };

                case BaseAddressTypeEnum.CamHack:
                    return new List<uint> { Config.CameraHack.CameraHackStruct };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
