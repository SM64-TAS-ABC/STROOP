using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class AreaConfig
    {
        public static uint AreaStartAddress { get { return RomVersionConfig.Switch(AreaStartAddressUS, AreaStartAddressJP); } }
        public static readonly uint AreaStartAddressUS = 0x8033B8D0;
        public static readonly uint AreaStartAddressJP = 0x8033A560;

        public static readonly uint AreaStructSize = 0x3C;

        public static uint CurrentAreaPointerAddress { get { return RomVersionConfig.Switch(CurrentAreaPointerAddressUS, CurrentAreaPointerAddressJP); } }
        public static readonly uint CurrentAreaPointerAddressUS = 0x8032DDCC;
        public static readonly uint CurrentAreaPointerAddressJP = 0x8032CE6C;

        public static readonly uint TerrainTypeOffset = 0x02;
    }
}
