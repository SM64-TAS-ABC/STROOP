using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Extensions;
using System.Reflection;
using STROOP.Managers;
using STROOP.Structs.Configurations;

namespace STROOP.Controls
{
    public class WatchVariable
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
        public readonly int? Shift;

        private readonly Func<uint, object> _getterFunction;
        private readonly Func<object, uint, bool> _setterFunction;

        public bool IsSpecial { get { return SpecialType != null; } }

        private bool UseAbsoluteAddressing { get { return BaseAddressType == BaseAddressTypeEnum.Absolute; } }

        public uint Offset
        {
            get
            {
                switch (RomVersionConfig.Version)
                {
                    case RomVersion.US:
                        if (OffsetUS.HasValue) return OffsetUS.Value;
                        break;
                    case RomVersion.JP:
                        if (OffsetJP.HasValue) return OffsetJP.Value;
                        break;
                    case RomVersion.PAL:
                        if (OffsetPAL.HasValue) return OffsetPAL.Value;
                        break;
                }
                if (OffsetDefault.HasValue) return OffsetDefault.Value;
                return 0;
            }
        }

        public List<uint> BaseAddressList
        {
            get
            {
                return WatchVariableUtilities.GetBaseAddressListFromBaseAddressType(BaseAddressType);
            }
        }

        public List<uint> AddressList
        {
            get
            {
                return BaseAddressList.ConvertAll(baseAddress => baseAddress + Offset);
            }
        }

        public WatchVariable(string memoryTypeName, string specialType, BaseAddressTypeEnum baseAddressType,
            uint? offsetUS, uint? offsetJP, uint? offsetPAL, uint? offsetDefault, uint? mask, int? shift)
        {
            if (offsetDefault.HasValue && (offsetUS.HasValue || offsetJP.HasValue || offsetPAL.HasValue))
            {
                throw new ArgumentOutOfRangeException("Can't have both a default offset value and a rom-specific offset value");
            }

            if (specialType != null)
            {
                if (baseAddressType != BaseAddressTypeEnum.None &&
                    baseAddressType != BaseAddressTypeEnum.Object &&
                    baseAddressType != BaseAddressTypeEnum.ProcessGroup &&
                    baseAddressType != BaseAddressTypeEnum.Ghost &&
                    baseAddressType != BaseAddressTypeEnum.LastCoin &&
                    baseAddressType != BaseAddressTypeEnum.File &&
                    baseAddressType != BaseAddressTypeEnum.MainSave &&
                    baseAddressType != BaseAddressTypeEnum.Triangle)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have base address type " + baseAddressType);
                }

                if (offsetDefault.HasValue || offsetUS.HasValue || offsetJP.HasValue || offsetPAL.HasValue)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have any type of offset");
                }

                if (mask != null)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have mask");
                }
            }

            BaseAddressType = baseAddressType;

            OffsetUS = offsetUS;
            OffsetJP = offsetJP;
            OffsetPAL = offsetPAL;
            OffsetDefault = offsetDefault;

            SpecialType = specialType;

            MemoryTypeName = memoryTypeName;
            MemoryType = memoryTypeName == null ? null : TypeUtilities.StringToType[MemoryTypeName];
            ByteCount = memoryTypeName == null ? (int?)null : TypeUtilities.TypeSize[MemoryType];
            NibbleCount = memoryTypeName == null ? (int?)null : TypeUtilities.TypeSize[MemoryType] * 2;
            SignedType = memoryTypeName == null ? (bool?)null : TypeUtilities.TypeSign[MemoryType];

            Mask = mask;
            Shift = shift;
            
            // Created getter/setter functions
            if (IsSpecial)
            {
                (_getterFunction, _setterFunction) = WatchVariableSpecialUtilities.CreateGetterSetterFunctions(SpecialType);
            }
            else
            {
                _getterFunction = (uint address) =>
                {
                    return Config.Stream.GetValue(
                        MemoryType, address, UseAbsoluteAddressing, Mask, Shift);
                };
                _setterFunction = (object value, uint address) =>
                {
                    return Config.Stream.SetValueRoundingWrapping(
                        MemoryType, value, address, UseAbsoluteAddressing, Mask, Shift);
                };

            }
        }

        public List<object> GetValues(List<uint> addresses = null)
        {
            List<uint> addressList = addresses ?? AddressList;
            return addressList.ConvertAll(
                address => _getterFunction(address));
        }

        public bool SetValue(object value, List<uint> addresses = null)
        {
            List<uint> addressList = addresses ?? AddressList;
            if (addressList.Count == 0) return false;

            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();
            bool success = addressList.ConvertAll(
                address => _setterFunction(value, address))
                    .Aggregate(true, (b1, b2) => b1 && b2);
            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public bool SetValues(List<object> values, List<uint> addresses = null)
        {
            List<uint> addressList = addresses ?? AddressList;
            if (addressList.Count == 0) return false;
            if (addressList.Count != values.Count) return false;

            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();
            bool success = true;
            for (int i = 0; i < addressList.Count; i++)
            {
                if (values[i] == null) continue;
                success &= _setterFunction(values[i], addressList[i]);
            }
            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public List<WatchVariableLock> GetLocks(List<uint> addresses = null)
        {
            List<uint> addressList = addresses ?? AddressList;
            List<object> values = GetValues(addressList);
            if (values.Count != addressList.Count) return new List<WatchVariableLock>();

            List<WatchVariableLock> locks = new List<WatchVariableLock>();
            for (int i = 0; i < values.Count; i++)
            {
                locks.Add(new WatchVariableLock(
                    IsSpecial, MemoryType, ByteCount, Mask, addressList[i], SpecialType, _setterFunction, values[i]));
            }
            return locks;
        }

        public string GetTypeDescription()
        {
            if (IsSpecial)
            {
                return "special";
            }
            else
            {
                string maskString = "";
                if (Mask != null)
                {
                    maskString = " with mask " + HexUtilities.FormatValue(Mask.Value, NibbleCount.Value);
                }
                string shiftString = "";
                if (Shift != null)
                {
                    shiftString = " right shifted by " + Shift.Value;
                }
                return MemoryTypeName + maskString + shiftString;
            }
        }

        public string GetBaseOffsetDescription()
        {
            string offsetString = IsSpecial ? SpecialType : HexUtilities.FormatValue(Offset);
            return BaseAddressType + " + " + offsetString;
        }

        public string GetProcessAddressListString(List<uint> addresses = null)
        {
            if (IsSpecial) return "(none)";
            List<uint> addressList = addresses ?? AddressList;
            if (addressList.Count == 0) return "(none)";
            List<ulong> processAddressList = GetProcessAddressList(addresses).ConvertAll(address => address.ToUInt64());
            List<string> stringList = processAddressList.ConvertAll(address => HexUtilities.FormatValue(address, address > 0xFFFFFFFFU ? 16 : 8));
            return string.Join(", ", stringList);
        }

        private List<UIntPtr> GetProcessAddressList(List<uint> addresses = null)
        {
            List<uint> addressList = addresses ?? AddressList;
            List<uint> ramAddressList = GetRamAddressList(false, addressList);
            return ramAddressList.ConvertAll(address => Config.Stream.GetAbsoluteAddress(address, ByteCount.Value));
        }

        public string GetRamAddressListString(bool addressArea = true, List<uint> addresses = null)
        {
            if (IsSpecial) return "(none)";
            List<uint> addressList = addresses ?? AddressList;
            if (addressList.Count == 0) return "(none)";
            List<uint> ramAddressList = GetRamAddressList(addressArea, addressList);
            List<string> stringList = ramAddressList.ConvertAll(address => HexUtilities.FormatValue(address, 8));
            return string.Join(", ", stringList);
        }

        private List<uint> GetRamAddressList(bool addressArea = true, List<uint> addresses = null)
        {
            List<uint> addressList = addresses ?? AddressList;
            return addressList.ConvertAll(address => GetRamAddress(address, addressArea));
        }

        private uint GetRamAddress(uint addr, bool addressArea = true)
        {
            UIntPtr addressPtr = new UIntPtr(addr);
            uint address;

            if (UseAbsoluteAddressing)
                address = EndiannessUtilities.SwapAddressEndianness(
                    Config.Stream.GetRelativeAddress(addressPtr, ByteCount.Value), ByteCount.Value);
            else
                address = addressPtr.ToUInt32();

            return addressArea ? address | 0x80000000 : address & 0x0FFFFFFF;
        }
    }
}
