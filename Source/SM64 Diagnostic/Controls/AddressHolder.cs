using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;
using System.Reflection;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Controls
{
    public class AddressHolder
    {
        public readonly string MemoryTypeName;
        public readonly Type MemoryType;
        public readonly int ByteCount;

        public readonly string SpecialType;

        public readonly BaseAddressTypeEnum BaseAddressType;

        public readonly uint? OffsetUS;
        public readonly uint? OffsetJP;
        public readonly uint? OffsetPAL;
        public readonly uint? OffsetDefault;

        private readonly Func<List<string>> _getterFunction;
        private readonly Action<string> _setterFunction;

        // TODO remove this
        private readonly bool _returnNonEmptyList;

        public bool UseAbsoluteAddressing
        {
            get
            {
                return BaseAddressType == BaseAddressTypeEnum.Absolute;
            }
        }

        public bool IsAdditive
        {
            get
            {
                return BaseAddressType != BaseAddressTypeEnum.Relative && BaseAddressType != BaseAddressTypeEnum.Absolute && BaseAddressType != BaseAddressTypeEnum.Special;
            }
        }

        public bool IsSpecial
        {
            get
            {
                return BaseAddressType == BaseAddressTypeEnum.Special;
            }
        }

        public uint Offset
        {
            get
            {
                switch (Config.Version)
                {
                    case Config.RomVersion.US:
                        if (OffsetUS != null) return OffsetUS.Value;
                        break;
                    case Config.RomVersion.JP:
                        if (OffsetJP != null) return OffsetJP.Value;
                        break;
                    case Config.RomVersion.PAL:
                        if (OffsetPAL != null) return OffsetPAL.Value;
                        break;
                }
                if (OffsetDefault != null) return OffsetDefault.Value;
                return 0;
            }
        }

        public List<uint> BaseAddressList
        {
            get
            {
                return VarXUtilities.GetBaseAddressListFromBaseAddressType(BaseAddressType, _returnNonEmptyList);
            }
        }

        public uint BaseAddressUnsafe
        {
            get
            {
                return BaseAddressList[0];
            }
        }

        public uint? BaseAddress
        {
            get
            {
                List<uint> baseAddressList = BaseAddressList;
                return baseAddressList.Count == 0 ? (uint?)null : baseAddressList[0];
            }
        }

        public List<uint> EffectiveAddressList
        {
            get
            {
                return BaseAddressList.ConvertAll(baseAddress => baseAddress + Offset);
            }
        }

        public uint EffectiveAddressUnsafe
        {
            get
            {
                return EffectiveAddressList[0];
            }
        }

        public uint? EffectiveAddress
        {
            get
            {
                List<uint> effectiveAddressList = EffectiveAddressList;
                return effectiveAddressList.Count == 0 ? (uint?)null : effectiveAddressList[0];
            }
        }

        public AddressHolder(string memoryTypeName, string specialType, BaseAddressTypeEnum baseAddress,
            uint? offsetUS, uint? offsetJP, uint? offsetPAL, uint? offsetDefault, bool returnNonEmptyList)
        {
            if (offsetUS == null && offsetJP == null && offsetPAL == null && offsetDefault == null)
            {
                //TODO add this back in after var refactor
                //throw new ArgumentOutOfRangeException("Cannot instantiate Address with all null values");
            }

            BaseAddressType = baseAddress;

            OffsetUS = offsetUS;
            OffsetJP = offsetJP;
            OffsetPAL = offsetPAL;
            OffsetDefault = offsetDefault;

            MemoryTypeName = IsSpecial ? "special" : memoryTypeName;
            MemoryType = IsSpecial ? null : VarXUtilities.StringToType[MemoryTypeName];
            ByteCount = IsSpecial ? 0 : VarXUtilities.TypeSize[MemoryType];

            SpecialType = specialType;

            _returnNonEmptyList = returnNonEmptyList;

            // Created getter/setter functions
            if (IsSpecial)
            {
                (_getterFunction, _setterFunction) = VarXSpecialUtilities.CreateGetterSetterFunctions(SpecialType);
            }
            else
            {
                _getterFunction = () =>
                {
                    return EffectiveAddressList.ConvertAll(
                        address => Config.Stream.GetValue(MemoryType, address, UseAbsoluteAddressing).ToString());
                };
                _setterFunction = (string value) =>
                {
                    EffectiveAddressList.ForEach(
                        address => Config.Stream.SetValueRoundingWrapping(MemoryType, value, address, UseAbsoluteAddressing));
                };
            }
        }

        public List<string> GetValues()
        {
            return _getterFunction();
        }

        public void SetValue(string stringValue)
        {
            Config.Stream.Suspend();
            _setterFunction(stringValue);
            Config.Stream.Resume();
        }

        public uint GetRamAddress(bool addressArea = true)
        {
            // TODO fix this
            if (IsSpecial) return 0;

            UIntPtr effectiveAddress = new UIntPtr(EffectiveAddressUnsafe);
            uint address;

            if (UseAbsoluteAddressing)
                address = (uint)Config.Stream.ConvertAddressEndianess(
                    new UIntPtr(effectiveAddress.ToUInt64() - (ulong)Config.Stream.ProcessMemoryOffset.ToInt64()),
                    ByteCount);
            else
                address = effectiveAddress.ToUInt32();

            return addressArea ? address | 0x80000000 : address & 0x0FFFFFFF;
        }

        public UIntPtr GetProcessAddress()
        {
            uint address = GetRamAddress(false);
            return Config.Stream.ConvertAddressEndianess(
                new UIntPtr(address + (ulong)Config.Stream.ProcessMemoryOffset.ToInt64()), ByteCount);
        }
    }
}
