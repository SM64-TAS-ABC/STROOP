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
    }
}
