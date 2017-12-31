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
using static SM64_Diagnostic.Structs.WatchVariable;
using SM64_Diagnostic.Structs.Configurations;
using static SM64_Diagnostic.Structs.VarXUtilities;

namespace SM64_Diagnostic.Controls
{
    public class AddressHolder
    {
        public readonly uint? AddressUS;
        public readonly uint? AddressJP;
        public readonly uint? AddressPAL;
        public readonly uint? AddressOffset;

        public readonly OffsetType Offset;
        public readonly int ByteCount;

        public bool UseAbsoluteAddressing
        {
            get
            {
                return Offset == OffsetType.Absolute;
            }
        }

        public bool HasAdditiveOffset
        {
            get
            {
                return Offset != OffsetType.Relative && Offset != OffsetType.Absolute && Offset != OffsetType.Special;
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

        public List<uint> OffsetList
        {
            get
            {
                return GetOffsetListFromOffsetType(Offset);
            }
        }

        public AddressHolder(int byteCount, OffsetType offset,
            uint? addressUS, uint? addressJP, uint? addressPAL, uint? addressOffset)
        {
            if (addressUS == null && addressJP == null && addressPAL == null && addressOffset == null)
            {
                //TODO add this back in after var refactor
                //throw new ArgumentOutOfRangeException("Cannot instantiate Address with all null values");
            }

            ByteCount = byteCount;
            Offset = offset;

            AddressUS = addressUS;
            AddressJP = addressJP;
            AddressPAL = addressPAL;
            AddressOffset = addressOffset;
        }

        public uint GetRamAddress(bool addressArea = true)
        {
            uint offset = OffsetList[0];
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
