using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class PaintingConfig
    {
        public static uint CastlePaintingStartAddress { get => RomVersionConfig.Switch(CastlePaintingStartAddressUS, CastlePaintingStartAddressJP); }
        public static readonly uint CastlePaintingStartAddressUS = 0x07023620; //bad
        public static readonly uint CastlePaintingStartAddressJP = 0x07023620;

        public static uint TtmPaintingStartAddress { get => RomVersionConfig.Switch(TtmPaintingStartAddressUS, TtmPaintingStartAddressJP); }
        public static readonly uint TtmPaintingStartAddressUS = 0x07012F00; //bad
        public static readonly uint TtmPaintingStartAddressJP = 0x07012F00;

        public static uint HmcPaintingStartAddress { get => RomVersionConfig.Switch(HmcPaintingStartAddressUS, HmcPaintingStartAddressJP); }
        public static readonly uint HmcPaintingStartAddressUS = 0x07012F00; //bad
        public static readonly uint HmcPaintingStartAddressJP = 0x07012F00; //bad

        public static uint PaintingStructSize = 0x78;

        public static uint GetAddress(PaintingListTypeEnum paintingListType, int index)
        {
            uint startAddress;
            switch (paintingListType)
            {
                case PaintingListTypeEnum.Castle:
                    startAddress = CastlePaintingStartAddress;
                    break;
                case PaintingListTypeEnum.TTM:
                    startAddress = TtmPaintingStartAddress;
                    break;
                case PaintingListTypeEnum.HMC:
                    startAddress = HmcPaintingStartAddress;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            uint offset = (uint)index * PaintingStructSize;
            uint segmentedAddress = startAddress + offset;
            uint address = SegmentationUtilities.DecodeSegmentedAddress(segmentedAddress);
            return address;
        }
    }
}
