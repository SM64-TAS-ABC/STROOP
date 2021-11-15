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
        public readonly string Name;

        public readonly string MemoryTypeName;
        public readonly Type MemoryType;
        public readonly int? ByteCount;
        public readonly int? NibbleCount;
        public readonly bool? SignedType;

        public readonly string SpecialType;

        public readonly BaseAddressTypeEnum BaseAddressType;

        public readonly uint? OffsetUS;
        public readonly uint? OffsetJP;
        public readonly uint? OffsetSH;
        public readonly uint? OffsetEU;
        public readonly uint? OffsetDefault;

        public readonly uint? Mask;
        public readonly int? Shift;
        public readonly bool HandleMapping;

        private readonly Func<uint, object> _getterFunction;
        private readonly Func<object, bool, uint, bool> _setterFunction;

        public bool IsSpecial { get => SpecialType != null; }
        public bool UseAbsoluteAddressing { get => BaseAddressType == BaseAddressTypeEnum.Absolute; }

        public uint Offset
        {
            get
            {
                if (OffsetUS.HasValue || OffsetJP.HasValue || OffsetSH.HasValue || OffsetEU.HasValue)
                {
                    if (HandleMapping)
                        return RomVersionConfig.SwitchMap(
                            OffsetUS ?? 0,
                            OffsetJP ?? 0,
                            OffsetSH ?? 0,
                            OffsetEU ?? 0);
                    else
                        return RomVersionConfig.SwitchOnly(
                            OffsetUS ?? 0,
                            OffsetJP ?? 0,
                            OffsetSH ?? 0,
                            OffsetEU ?? 0);
                }
                if (OffsetDefault.HasValue) return OffsetDefault.Value;
                return 0;
            }
        }

        public List<uint> GetBaseAddressList()
        {
            return WatchVariableUtilities.GetBaseAddressListFromBaseAddressType(BaseAddressType);
        }

        public List<uint> GetAddressList(List<uint> addresses)
        {
            List<uint> baseAddresses = addresses ?? GetBaseAddressList();
            uint offset = Offset;
            return baseAddresses.ConvertAll(baseAddress => baseAddress + offset);
        }

        public WatchVariable(string name, string memoryTypeName, string specialType, BaseAddressTypeEnum baseAddressType,
            uint? offsetUS, uint? offsetJP, uint? offsetSH, uint? offsetEU, uint? offsetDefault, uint? mask, int? shift, bool handleMapping)
        {
            if (offsetDefault.HasValue && (offsetUS.HasValue || offsetJP.HasValue || offsetSH.HasValue || offsetEU.HasValue))
            {
                throw new ArgumentOutOfRangeException("Can't have both a default offset value and a rom-specific offset value");
            }

            if (specialType != null)
            {
                if (baseAddressType == BaseAddressTypeEnum.Relative ||
                    baseAddressType == BaseAddressTypeEnum.Absolute)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have base address type " + baseAddressType);
                }

                if (offsetDefault.HasValue || offsetUS.HasValue || offsetJP.HasValue || offsetSH.HasValue || offsetEU.HasValue)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have any type of offset");
                }

                if (mask != null)
                {
                    throw new ArgumentOutOfRangeException("Special var cannot have mask");
                }
            }

            Name = name;

            BaseAddressType = baseAddressType;

            OffsetUS = offsetUS;
            OffsetJP = offsetJP;
            OffsetSH = offsetSH;
            OffsetEU = offsetEU;
            OffsetDefault = offsetDefault;

            SpecialType = specialType;

            MemoryTypeName = memoryTypeName;
            MemoryType = memoryTypeName == null ? null : TypeUtilities.StringToType[MemoryTypeName];
            ByteCount = memoryTypeName == null ? (int?)null : TypeUtilities.TypeSize[MemoryType];
            NibbleCount = memoryTypeName == null ? (int?)null : TypeUtilities.TypeSize[MemoryType] * 2;
            SignedType = memoryTypeName == null ? (bool?)null : TypeUtilities.TypeSign[MemoryType];

            Mask = mask;
            Shift = shift;
            HandleMapping = handleMapping;
            
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
                _setterFunction = (object value, bool setManually, uint address) =>
                {
                    return Config.Stream.SetValueRoundingWrapping(
                        MemoryType, value, address, UseAbsoluteAddressing, Mask, Shift);
                };

            }
        }

        public List<object> GetValues(List<uint> addresses = null)
        {
            List<uint> addressList = GetAddressList(addresses);
            List<object> realValues = addressList.ConvertAll(
                address => _getterFunction(address));
            List<object> lockValues = Config.LockManager.GetExistingLockValues(this, addresses);
            if (lockValues == null) return realValues; // short circuit if locking is disabled
            if (lockValues.Count != realValues.Count) throw new ArgumentOutOfRangeException();
            List<object> returnValues = new List<object>();
            for (int i = 0; i < lockValues.Count; i++)
            {
                returnValues.Add(lockValues[i] ?? realValues[i]);
            }
            return returnValues;
        }

        public bool SetValue(object value, bool setManually, List<uint> addresses = null)
        {
            List<uint> addressList = GetAddressList(addresses);
            if (addressList.Count == 0) return false;

            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();
            bool success = addressList.ConvertAll(
                address => _setterFunction(value, setManually, address))
                    .Aggregate(true, (b1, b2) => b1 && b2);
            if (!streamAlreadySuspended) Config.Stream.Resume();

            if (success)
            {
                Config.LockManager.UpdateLockValues(this, value, addresses);
            }

            return success;
        }

        public bool SetValues(List<object> values, bool setManually, List<uint> addresses = null)
        {
            List<uint> addressList = GetAddressList(addresses);
            if (addressList.Count == 0) return false;
            int minCount = Math.Min(addressList.Count, values.Count);

            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();
            bool success = true;
            for (int i = 0; i < minCount; i++)
            {
                if (values[i] == null) continue;
                success &= _setterFunction(values[i], setManually, addressList[i]);
            }
            if (!streamAlreadySuspended) Config.Stream.Resume();

            if (success)
            {
                Config.LockManager.UpdateLockValues(this, values, addresses);
            }

            return success;
        }

        public List<WatchVariableLock> GetLocks(List<uint> addresses = null)
        {
            List<uint> baseAddressList = addresses ?? GetBaseAddressList();
            List<uint> addressList = GetAddressList(addresses);
            List<object> values = GetValues(addresses);
            if (values.Count != addressList.Count || values.Count != baseAddressList.Count)
                return new List<WatchVariableLock>();

            List<WatchVariableLock> locks = new List<WatchVariableLock>();
            for (int i = 0; i < values.Count; i++)
            {
                locks.Add(new WatchVariableLock(
                    Name, BaseAddressType, IsSpecial, MemoryType, ByteCount, Mask, Shift, addressList[i], baseAddressList[i], SpecialType, _setterFunction, values[i]));
            }
            return locks;
        }

        public List<WatchVariableLock> GetLocksWithoutValues(List<uint> addresses = null)
        {
            List<uint> baseAddressList = addresses ?? GetBaseAddressList();
            List<uint> addressList = GetAddressList(addresses);
            if (baseAddressList.Count != addressList.Count)
                return new List<WatchVariableLock>();

            List<WatchVariableLock> locks = new List<WatchVariableLock>();
            for (int i = 0; i < addressList.Count; i++)
            {
                locks.Add(new WatchVariableLock(
                    Name, BaseAddressType, IsSpecial, MemoryType, ByteCount, Mask, Shift, addressList[i], baseAddressList[i], SpecialType, _setterFunction, null));
            }
            return locks;
        }

        public List<Func<object, bool, bool>> GetSetters(List<uint> addresses = null)
        {
            List<uint> addressList = GetAddressList(addresses);
            return addressList.ConvertAll(
                address => (Func<object, bool, bool>)((object value, bool setManually) => _setterFunction(value, setManually, address)));
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
                string byteCountString = "";
                if (ByteCount.HasValue)
                {
                    string pluralSuffix = ByteCount.Value == 1 ? "" : "s";
                    byteCountString = string.Format(" ({0} byte{1})", ByteCount.Value, pluralSuffix);
                }
                return MemoryTypeName + maskString + shiftString + byteCountString;
            }
        }

        public string GetBaseTypeOffsetDescription()
        {
            string offsetString = IsSpecial ? SpecialType : HexUtilities.FormatValue(Offset);
            return BaseAddressType + " + " + offsetString;
        }

        public string GetProcessAddressListString(List<uint> addresses = null)
        {
            if (IsSpecial) return "(none)";
            List<uint> addressList = GetAddressList(addresses);
            if (addressList.Count == 0) return "(none)";
            List<ulong> processAddressList = GetProcessAddressList(addressList, ByteCount.Value).ConvertAll(address => address.ToUInt64());
            List<string> stringList = processAddressList.ConvertAll(address => HexUtilities.FormatValue(address, address > 0xFFFFFFFFU ? 16 : 8));
            return string.Join(", ", stringList);
        }

        public string GetProcessBaseAddressListString(List<uint> addresses = null)
        {
            List<uint> addressList = addresses ?? GetBaseAddressList();
            if (addressList.Count == 0) return "(none)";
            List<ulong> processAddressList = GetProcessAddressList(addressList, null).ConvertAll(address => address.ToUInt64());
            List<string> stringList = processAddressList.ConvertAll(address => HexUtilities.FormatValue(address, address > 0xFFFFFFFFU ? 16 : 8));
            return string.Join(", ", stringList);
        }

        private List<UIntPtr> GetProcessAddressList(List<uint> addresses, int? byteCount)
        {
            List<uint> ramAddressList = GetRamAddressList(false, addresses);
            return ramAddressList.ConvertAll(
                address => Config.Stream.GetAbsoluteAddress(address, byteCount ?? (address % 4 == 2 ? 2 : 4)));
        }

        public string GetRamAddressListString(bool addressArea = true, List<uint> addresses = null)
        {
            if (IsSpecial) return "(none)";
            List<uint> addressList = GetAddressList(addresses);
            if (addressList.Count == 0) return "(none)";
            List<uint> ramAddressList = GetRamAddressList(addressArea, addressList);
            List<string> stringList = ramAddressList.ConvertAll(address => HexUtilities.FormatValue(address, 8));
            return string.Join(", ", stringList);
        }

        private List<uint> GetRamAddressList(bool addressArea, List<uint> addressList)
        {
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

        public string GetBaseAddressListString(List<uint> addresses = null)
        {
            List<uint> baseAddresses = addresses ?? GetBaseAddressList();
            if (baseAddresses.Count == 0) return "(none)";
            List<string> baseAddressesString = baseAddresses.ConvertAll(address => HexUtilities.FormatValue(address, 8));
            return string.Join(",", baseAddressesString);
        }
    }
}
