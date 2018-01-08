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
        private readonly AddressHolder AddressHolder;
        public uint Address { get { return AddressHolder.Offset; } }

        public readonly BaseAddressTypeEnum Offset;
        public readonly string Name;
        public readonly string SpecialType;
        public readonly ulong? Mask;
        public readonly bool IsBool;
        public readonly bool IsObject;
        public readonly bool IsAngle;
        public readonly Color? BackroundColor;
        public readonly List<VariableGroup> GroupList;
        public readonly string TypeName;
        public readonly Type Type;
        public readonly int ByteCount;

        public bool UseHex;
        public bool InvertBool;

        public WatchVariable(
            string name,
            BaseAddressTypeEnum offset,
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
                return Offset != BaseAddressTypeEnum.Relative && Offset != BaseAddressTypeEnum.Absolute && !IsSpecial;
            }
        }

        public bool IsSpecial
        {
            get
            {
                return SpecialType != null;
            }
        }

        public bool UseAbsoluteAddressing
        {
            get
            {
                return Offset == BaseAddressTypeEnum.Absolute;
            }
        }

    }
}
