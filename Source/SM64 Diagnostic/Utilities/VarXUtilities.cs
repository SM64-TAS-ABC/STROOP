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

        // TODO add new offset types
        public enum OffsetType
        {
            Absolute,
            Relative,
            Mario,
            MarioObj,
            Camera,
            File,
            Object,
            Triangle,
            TriangleExertionForceTable,
            InputCurrent,
            InputJustPressed,
            InputBuffered,
            Graphics,
            Animation,
            Waypoint,
            Water,
            Area,
            HackedArea,
            CamHack,
            Special,
        };

        public static OffsetType GetOffsetType(string offsetTypeString)
        {
            return (OffsetType)Enum.Parse(typeof(OffsetType), offsetTypeString);
        }

        public enum VariableGroup
        {
            Simple,
            Expanded,
            ObjectSpecific,
            Collision,
        };

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

        private static readonly List<uint> OffsetListZero = new List<uint> { 0 };

        public static List<uint> GetOffsetListFromOffsetType(OffsetType? offsetType, bool returnNonEmptyList = true)
        {
            List<uint> output;
            switch (offsetType)
            {
                case OffsetType.Absolute:
                    output = OffsetListZero;
                    break;
                case OffsetType.Relative:
                    output = OffsetListZero;
                    break;
                case OffsetType.Mario:
                    output = new List<uint> { Config.Mario.StructAddress };
                    break;
                case OffsetType.MarioObj:
                    output = new List<uint> { Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress) };
                    break;
                case OffsetType.Camera:
                    output = new List<uint> { Config.Camera.CameraStructAddress };
                    break;
                case OffsetType.File:
                    output = new List<uint> { FileManager.Instance.CurrentFileAddress };
                    break;
                case OffsetType.Object:
                    output = ObjectManager.Instance.CurrentAddresses;
                    break;
                case OffsetType.Triangle:
                    output = new List<uint> { TriangleManager.Instance.TriangleAddress };
                    break;
                case OffsetType.TriangleExertionForceTable:
                    uint triangleAddress = TriangleManager.Instance.TriangleAddress;
                    if (triangleAddress == 0)
                    {
                        output = OffsetListZero;
                    }
                    else
                    {
                        uint exertionForceIndex = Config.Stream.GetByte(triangleAddress + Config.TriangleOffsets.ExertionForceIndex);
                        output = new List<uint> { Config.Triangle.ExertionForceTableAddress + 2 * exertionForceIndex };
                    }
                    break;
                case OffsetType.InputCurrent:
                    output = new List<uint> { Config.Input.CurrentInputAddress };
                    break;
                case OffsetType.InputJustPressed:
                    output = new List<uint> { Config.Input.JustPressedInputAddress };
                    break;
                case OffsetType.InputBuffered:
                    output = new List<uint> { Config.Input.BufferedInputAddress };
                    break;
                case OffsetType.Graphics:
                    output = GetOffsetListFromOffsetType(OffsetType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.BehaviorGfxOffset));
                    break;
                case OffsetType.Animation:
                    output = GetOffsetListFromOffsetType(OffsetType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.AnimationOffset));
                    break;
                case OffsetType.Waypoint:
                    output = GetOffsetListFromOffsetType(OffsetType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.WaypointOffset));
                    break;
                case OffsetType.Water:
                    output = new List<uint> { Config.Stream.GetUInt32(Config.WaterPointerAddress) };
                    break;
                case OffsetType.Area:
                    output = new List<uint> { AreaManager.Instance.SelectedAreaAddress };
                    break;
                case OffsetType.HackedArea:
                    output = new List<uint> { Config.HackedAreaAddress };
                    break;
                case OffsetType.CamHack:
                    output = new List<uint> { Config.CameraHack.CameraHackStruct };
                    break;
                case OffsetType.Special:
                    throw new ArgumentOutOfRangeException("Should not get offset list for Special var");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (returnNonEmptyList && output.Count == 0)
            {
                output = OffsetListZero;
            }
            return output;
        }
    }
}
