using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SM64_Diagnostic.Structs.VarXUtilities;

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
        public bool IsScript;
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
