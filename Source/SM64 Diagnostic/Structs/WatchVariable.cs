using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct WatchVariable
    {
        public Type Type;
        public uint Address;
        public String Name;
        public String SpecialType;
        public bool Special;
        public Boolean AbsoluteAddressing;
        public UInt64? Mask;
        public bool IsBool;
        public bool UseHex;
        public bool OtherOffset;
        public bool InvertBool;
        public bool IsAngle;


        public static Dictionary<Type, int> TypeSize = new Dictionary<Type, int>()
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

        public static Dictionary<String, Type> StringToType = new Dictionary<string, Type>()
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
    }
}
