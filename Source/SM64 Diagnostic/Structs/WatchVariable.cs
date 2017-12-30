using SM64_Diagnostic.Controls;
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
        public AddressHolder AddressHolder;
        public uint Address { get { return AddressHolder.Address; } }

        public OffsetType Offset;
        public string Name;
        public string SpecialType;
        public ulong? Mask;
        public bool IsBool;
        public bool IsObject;
        public bool UseHex;
        public bool InvertBool;
        public bool IsAngle;
        public Color? BackroundColor;
        public List<VariableGroup> GroupList;

        public string TypeName;
        public Type Type;
        public int ByteCount;

        public WatchVariable(
            string name,
            OffsetType offset,
            List<VariableGroup> groupList,
            string specialType,
            Color? backgroundColor,
            AddressHolder addressHolder,
            bool useHex,
            ulong? mask,
            bool isBool,
            bool isObject,
            string typeName,
            bool invertBool,
            bool isAngle)
        {
            Name = name;
            Offset = offset;
            GroupList = groupList;
            SpecialType = specialType;
            BackroundColor = backgroundColor;

            if (IsSpecial) return;

            AddressHolder = addressHolder;
            UseHex = useHex;
            Mask = mask;
            IsBool = isBool;
            IsObject = isObject;
            InvertBool = invertBool;
            IsAngle = isAngle;

            TypeName = typeName;
            Type = StringToType[TypeName];
            ByteCount = TypeSize[Type];
        }

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

        public bool UseAbsoluteAddressing
        {
            get
            {
                return Offset == OffsetType.Absolute;
            }
        }

    }
}
