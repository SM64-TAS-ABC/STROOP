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
        public static uint CastlePaintingsStartAddress = 0x07023620;
        public static uint TtmPaintingsStartAddress = 0x07012F00;
        public static uint HmcPaintingsStartAddress = 0x0702551C;
        public static uint PaintingStructSize = 0x78;

        public static uint GetAddress(PaintingListTypeEnum paintingListType, int index)
        {
            uint startAddress;
            switch (paintingListType)
            {
                case PaintingListTypeEnum.Castle:
                    startAddress = CastlePaintingsStartAddress;
                    break;
                case PaintingListTypeEnum.TTM:
                    startAddress = TtmPaintingsStartAddress;
                    break;
                case PaintingListTypeEnum.HMC:
                    startAddress = HmcPaintingsStartAddress;
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
