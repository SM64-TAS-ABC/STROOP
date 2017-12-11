using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class WatchVariable
    {
        Type _type;
        public Type Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type == value)
                    return;

                _type = value;
                _byteCount = TypeSize[_type];
                _typeName = StringToType.First(s => s.Value == _type).Key;
            }
        }

        private uint? _addressUS;
        private uint? _addressJP;
        private uint? _addressPAL;
        private uint? _address;
        public uint Address
        {
            get
            {
                switch (Config.Version)
                {
                    case Config.RomVersion.US:
                        if (_addressUS != null) return (uint)_addressUS;
                        break;
                    case Config.RomVersion.JP:
                        if (_addressJP != null) return (uint)_addressJP;
                        break;
                    case Config.RomVersion.PAL:
                        if (_addressPAL != null) return (uint)_addressPAL;
                        break;
                }
                if (_address != null) return (uint)_address;
                return 0;
            }
        }

        public OffsetType Offset;
        public String Name;
        public String SpecialType;
        public UInt64? Mask;
        public bool IsBool;
        public bool IsObject;
        public bool UseHex;
        public bool InvertBool;
        public bool IsAngle;
        public Color? BackroundColor;
        public List<VariableGroup> GroupList;

        public bool HasAdditiveOffset
        {
            get
            {
                return Offset != OffsetType.Relative && Offset != OffsetType.Absolute && Offset != OffsetType.Special;
            }
        }

        public bool IsSpecial
        {
            get
            {
                return Offset == OffsetType.Special;
            }
        }

        public Boolean UseAbsoluteAddressing
        {
            get
            {
                return Offset == OffsetType.Absolute;
            }
        }


        int _byteCount;
        public int ByteCount
        {
            get
            {
                return _byteCount;
            }
        }

        string _typeName;
        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                if (_typeName == value || !StringToType.ContainsKey(value))
                    return;

                _typeName = value;
                _type = StringToType[_typeName];
                _byteCount = TypeSize[_type];
            }
        }

        readonly static Dictionary<Type, int> TypeSize = new Dictionary<Type, int>()
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

        readonly static Dictionary<String, Type> StringToType = new Dictionary<string, Type>()
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

        public void SetAddress(uint? addressUS, uint? addressJP, uint? addressPAL, uint? address)
        {
            if (addressUS == null && addressJP == null && addressPAL == null && address == null)
                throw new ArgumentOutOfRangeException();

            _addressUS = addressUS;
            _addressJP = addressJP;
            _addressPAL = addressPAL;
            _address = address;
        }
    }
}
