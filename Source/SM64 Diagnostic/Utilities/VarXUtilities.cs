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
            {typeof(double), 4}
        };

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

        public static BaseAddressType GetBaseAddressType(string offsetTypeString)
        {
            return (BaseAddressType)Enum.Parse(typeof(BaseAddressType), offsetTypeString);
        }

        public static VariableGroup GetVariableGroup(string variableGroupString)
        {
            return (VariableGroup)Enum.Parse(typeof(VariableGroup), variableGroupString);
        }

        public static List<VariableGroup> ParseVariableGroupList(string variableGroupListString)
        {
            List<VariableGroup> variableGroupList = new List<VariableGroup>();
            if (variableGroupListString != null)
            {
                string[] groupNames = variableGroupListString.Split(',');
                foreach (string groupName in groupNames)
                {
                    variableGroupList.Add(GetVariableGroup(groupName));
                }
            }
            return variableGroupList;
        }

        private static readonly List<uint> BaseAddressListZero = new List<uint> { 0 };

        public static List<uint> GetBaseAddressListFromBaseAddressType(BaseAddressType? baseAddressType, bool returnNonEmptyList = true)
        {
            List<uint> output;
            switch (baseAddressType)
            {
                case BaseAddressType.Absolute:
                    output = BaseAddressListZero;
                    break;
                case BaseAddressType.Relative:
                    output = BaseAddressListZero;
                    break;
                case BaseAddressType.Mario:
                    output = new List<uint> { Config.Mario.StructAddress };
                    break;
                case BaseAddressType.MarioObj:
                    output = new List<uint> { Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress) };
                    break;
                case BaseAddressType.Camera:
                    output = new List<uint> { Config.Camera.CameraStructAddress };
                    break;
                case BaseAddressType.File:
                    output = new List<uint> { FileManager.Instance.CurrentFileAddress };
                    break;
                case BaseAddressType.Object:
                    output = ObjectManager.Instance.CurrentAddresses;
                    break;
                case BaseAddressType.Triangle:
                    output = new List<uint> { TriangleManager.Instance.TriangleAddress };
                    break;
                case BaseAddressType.TriangleExertionForceTable:
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
                case BaseAddressType.InputCurrent:
                    output = new List<uint> { Config.Input.CurrentInputAddress };
                    break;
                case BaseAddressType.InputJustPressed:
                    output = new List<uint> { Config.Input.JustPressedInputAddress };
                    break;
                case BaseAddressType.InputBuffered:
                    output = new List<uint> { Config.Input.BufferedInputAddress };
                    break;
                case BaseAddressType.Graphics:
                    output = GetBaseAddressListFromBaseAddressType(BaseAddressType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.BehaviorGfxOffset));
                    break;
                case BaseAddressType.Animation:
                    output = GetBaseAddressListFromBaseAddressType(BaseAddressType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.AnimationOffset));
                    break;
                case BaseAddressType.Waypoint:
                    output = GetBaseAddressListFromBaseAddressType(BaseAddressType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.WaypointOffset));
                    break;
                case BaseAddressType.Water:
                    output = new List<uint> { Config.Stream.GetUInt32(Config.WaterPointerAddress) };
                    break;
                case BaseAddressType.Area:
                    output = new List<uint> { AreaManager.Instance.SelectedAreaAddress };
                    break;
                case BaseAddressType.HackedArea:
                    output = new List<uint> { Config.HackedAreaAddress };
                    break;
                case BaseAddressType.CamHack:
                    output = new List<uint> { Config.CameraHack.CameraHackStruct };
                    break;
                case BaseAddressType.Special:
                    throw new ArgumentOutOfRangeException("Should not get offset list for Special var");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (returnNonEmptyList && output.Count == 0)
            {
                output = BaseAddressListZero;
            }
            return output;
        }
    }
}
