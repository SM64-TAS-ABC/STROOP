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
        public readonly int? ByteCount;
        public readonly int? NibbleCount;
        public readonly bool? SignedType;

        public readonly string SpecialType;

        public readonly BaseAddressTypeEnum BaseAddressType;

        public readonly uint? OffsetUS;
        public readonly uint? OffsetJP;
        public readonly uint? OffsetPAL;
        public readonly uint? OffsetDefault;

        public readonly uint? Mask;

        private readonly Func<uint, string> _getterFunction;
        private readonly Func<string, uint, bool> _setterFunction;

        // TODO remove this
        private readonly bool _returnNonEmptyList;

        public bool IsSpecial { get { return SpecialType != null; } }

        private bool UseAbsoluteAddressing { get { return BaseAddressType == BaseAddressTypeEnum.Absolute; } }

        // TODO make this private once var x is the norm
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

        private List<uint> EffectiveAddressList
        {
            get
            {
                if (_returnNonEmptyList)
                {
                    return VarXUtilities.GetBaseAddressListFromBaseAddressType(BaseAddressType, true)
                        .ConvertAll(baseAddress => baseAddress + Offset);
                }

                return VarXUtilities.GetBaseAddressListFromBaseAddressTypeVarX(BaseAddressType)
                    .ConvertAll(baseAddress => baseAddress + Offset);
            }
        }

        public AddressHolder(string memoryTypeName, string specialType, BaseAddressTypeEnum baseAddress,
            uint? offsetUS, uint? offsetJP, uint? offsetPAL, uint? offsetDefault, uint? mask, bool returnNonEmptyList)
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

            SpecialType = specialType;

            MemoryTypeName = memoryTypeName;
            MemoryType = memoryTypeName == null ? null : VarXUtilities.StringToType[MemoryTypeName];
            ByteCount = memoryTypeName == null ? (int?)null : VarXUtilities.TypeSize[MemoryType];
            NibbleCount = memoryTypeName == null ? (int?)null : VarXUtilities.TypeSize[MemoryType] * 2;
            SignedType = memoryTypeName == null ? (bool?)null : VarXUtilities.TypeSign[MemoryType];

            Mask = mask;

            // TODO remove this after var x is the norm
            _returnNonEmptyList = returnNonEmptyList;

            // Created getter/setter functions
            if (IsSpecial)
            {
                (_getterFunction, _setterFunction) = VarXSpecialUtilities.CreateGetterSetterFunctions(SpecialType);
            }
            else
            {
                _getterFunction = (uint address) =>
                {
                    return Config.Stream.GetValue(MemoryType, address, UseAbsoluteAddressing, Mask).ToString();
                };
                _setterFunction = (string value, uint address) =>
                {
                    return Config.Stream.SetValueRoundingWrapping(
                        MemoryType, value, address, UseAbsoluteAddressing, Mask);
                };

            }
        }

        public List<string> GetValues()
        {
            return EffectiveAddressList.ConvertAll(
                address => _getterFunction(address));
        }

        public bool SetValue(string value)
        {
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();
            bool success = EffectiveAddressList.ConvertAll(
                address => _setterFunction(value, address))
                    .Aggregate(true, (b1, b2) => b1 && b2);
            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public List<AddressHolderLock> GetLocks()
        {
            List<string> values = GetValues();
            List<uint> effectiveAddresses = EffectiveAddressList;
            if (values.Count != effectiveAddresses.Count) return new List<AddressHolderLock>();

            List<AddressHolderLock> locks = new List<AddressHolderLock>();
            for (int i = 0; i < values.Count; i++)
            {
                locks.Add(new AddressHolderLock(
                    IsSpecial, MemoryType, ByteCount, Mask, effectiveAddresses[i], SpecialType, _setterFunction, values[i]));
            }
            return locks;
        }

        public string GetTypeDescription()
        {
            if (IsSpecial)
            {
                return "special (" + SpecialType + ")";
            }
            else
            {
                string maskString = "";
                if (Mask != null)
                {
                    maskString = " with mask " + String.Format("0x{0:X" + NibbleCount.Value + "}", Mask.Value);
                }
                return MemoryTypeName + maskString;
            }
        }

        public string GetRamAddressString(bool addressArea = true)
        {
            if (IsSpecial) return "(none)";
            if (EffectiveAddressList.Count == 0) return "(none)";
            return String.Format("0x{0:X8}", GetRamAddress(addressArea));
        }

        public uint GetRamAddress(bool addressArea = true)
        {
            List<uint> effectiveAddresses = EffectiveAddressList;
            if (effectiveAddresses.Count == 0) return 0;

            UIntPtr effectiveAddress = new UIntPtr(effectiveAddresses[0]);
            uint address;

            if (UseAbsoluteAddressing)
                address = (uint)Config.Stream.ConvertAddressEndianess(
                    new UIntPtr(effectiveAddress.ToUInt64() - (ulong)Config.Stream.ProcessMemoryOffset.ToInt64()),
                    ByteCount.Value);
            else
                address = effectiveAddress.ToUInt32();

            return addressArea ? address | 0x80000000 : address & 0x0FFFFFFF;
        }

        public string GetProcessAddressString()
        {
            if (IsSpecial) return "(none)";
            if (EffectiveAddressList.Count == 0) return "(none)";
            return String.Format("0x{0:X8}", GetProcessAddress().ToUInt64());
        }

        public UIntPtr GetProcessAddress()
        {
            uint address = GetRamAddress(false);
            return Config.Stream.ConvertAddressEndianess(
                new UIntPtr(address + (ulong)Config.Stream.ProcessMemoryOffset.ToInt64()), ByteCount.Value);
        }
    }
}
