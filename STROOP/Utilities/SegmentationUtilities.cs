using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class SegmentationUtilities
    {
        public static uint SegmentationTableAddress { get => RomVersionConfig.SwitchMap(SegmentationTableAddressUS, SegmentationTableAddressJP); }
        public static uint SegmentationTableAddressUS = 0x8033B400;
        public static uint SegmentationTableAddressJP = 0x8033A090;

        // A segmented address is 4 bytes. The first byte contains the index of the segment in the segment table, the 
        // other 3 bytes are the offset from the segment. Segmented addresses are used for locating object behavior scripts, 
        // display lists, textures and other resources.
        public static uint DecodeSegmentedAddress(uint segmentedAddress)
        {
            var segment = (segmentedAddress >> 24);
            var offset = segmentedAddress & 0xFFFFFF;
            return offset + Config.Stream.GetUInt32(4 * segment + (SegmentationTableAddress & 0xFFFFFF));
        }

        public static uint SegmentedToVirtual(uint addr)
        {
            uint segment = addr >> 24;
            uint offset = addr & 0x00FFFFFF;
            uint segmentTableValue = Config.Stream.GetUInt32(SegmentationTableAddress + 4 * segment);
            return (segmentTableValue + offset) | 0x80000000;
        }

        public static uint VirtualToSegmented(uint segment, uint addr)
        {
            uint segmentTableValue = Config.Stream.GetUInt32(SegmentationTableAddress + 4 * segment);
            uint offset = (addr & 0x1FFFFFFF) - segmentTableValue;
            return (segment << 24) + offset;
        }
    }
}
