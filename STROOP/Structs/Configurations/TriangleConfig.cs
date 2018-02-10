using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class TriangleConfig
    {
        public static readonly uint TriangleStructSize = 0x30;

        public static readonly uint TriangleListPointerAddress = 0x8038EE9C;

        public static uint LevelTriangleCountAddress { get { return Config.SwitchRomVersion(LevelTriangleCountAddressUS, LevelTriangleCountAddressJP); } }
        public static readonly uint LevelTriangleCountAddressUS = 0x80361178;
        public static readonly uint LevelTriangleCountAddressJP = 0x8035FE08;

        public static uint TotalTriangleCountAddress { get { return Config.SwitchRomVersion(TotalTriangleCountAddressUS, TotalTriangleCountAddressJP); } }
        public static readonly uint TotalTriangleCountAddressUS = 0x80361170;
        public static readonly uint TotalTriangleCountAddressJP = 0x8035FE00;

        public static readonly uint NodeListPointerAddress = 0x8038EE98;

        public static uint LevelNodeCountAddress { get { return Config.SwitchRomVersion(LevelNodeCountAddressUS, LevelNodeCountAddressJP); } }
        public static readonly uint LevelNodeCountAddressUS = 0x80361174;
        public static readonly uint LevelNodeCountAddressJP = 0x8035FE04;

        public static uint TotalNodeCountAddress { get { return Config.SwitchRomVersion(TotalNodeCountAddressUS, TotalNodeCountAddressJP); } }
        public static readonly uint TotalNodeCountAddressUS = 0x8036116C;
        public static readonly uint TotalNodeCountAddressJP = 0x8035FDFC;

        public static uint ExertionForceTableAddress { get { return Config.SwitchRomVersion(ExertionForceTableAddressUS, ExertionForceTableAddressJP); } }
        public static readonly uint ExertionForceTableAddressUS = 0x8032DD38;
        public static readonly uint ExertionForceTableAddressJP = 0x8032CDD8;
    }
}
