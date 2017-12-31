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
        public readonly uint? AddressUS;
        public readonly uint? AddressJP;
        public readonly uint? AddressPAL;
        public readonly uint? AddressOffset;

        public readonly BaseAddressType BaseAddress;
        public readonly int ByteCount;

        public bool UseAbsoluteAddressing
        {
            get
            {
                return BaseAddress == BaseAddressType.Absolute;
            }
        }

        public bool HasAdditiveBaseAddress
        {
            get
            {
                return BaseAddress != BaseAddressType.Relative && BaseAddress != BaseAddressType.Absolute && BaseAddress != BaseAddressType.Special;
            }
        }

        public uint Address
        {
            get
            {
                switch (Config.Version)
                {
                    case Config.RomVersion.US:
                        if (AddressUS != null) return AddressUS.Value;
                        break;
                    case Config.RomVersion.JP:
                        if (AddressJP != null) return AddressJP.Value;
                        break;
                    case Config.RomVersion.PAL:
                        if (AddressPAL != null) return AddressPAL.Value;
                        break;
                }
                if (AddressOffset != null) return AddressOffset.Value;
                return 0;
            }
        }

        public List<uint> BaseAddressList
        {
            get
            {
                return VarXUtilities.GetBaseAddressListFromBaseAddressType(BaseAddress);
            }
        }

        public AddressHolder(int byteCount, BaseAddressType offset,
            uint? offsetUS, uint? offsetJP, uint? offsetPAL, uint? offsetDefault)
        {
            if (offsetUS == null && offsetJP == null && offsetPAL == null && offsetDefault == null)
            {
                //TODO add this back in after var refactor
                //throw new ArgumentOutOfRangeException("Cannot instantiate Address with all null values");
            }

            ByteCount = byteCount;
            BaseAddress = offset;

            AddressUS = offsetUS;
            AddressJP = offsetJP;
            AddressPAL = offsetPAL;
            AddressOffset = offsetDefault;
        }

        public uint GetRamAddress(bool addressArea = true)
        {
            uint offset = BaseAddressList[0];
            var offsetedAddress = new UIntPtr(offset + Address);
            uint address;

            if (UseAbsoluteAddressing)
                address = (uint)Config.Stream.ConvertAddressEndianess(
                    new UIntPtr(offsetedAddress.ToUInt64() - (ulong)Config.Stream.ProcessMemoryOffset.ToInt64()),
                    ByteCount);
            else
                address = offsetedAddress.ToUInt32();

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
