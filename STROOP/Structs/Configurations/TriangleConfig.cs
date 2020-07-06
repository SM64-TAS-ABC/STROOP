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

        public static uint TriangleListPointerAddress { get => RomVersionConfig.SwitchMap(TriangleListPointerAddressUS, TriangleListPointerAddressJP); }
        public static readonly uint TriangleListPointerAddressUS = 0x8038EE9C;
        public static readonly uint TriangleListPointerAddressJP = 0x8038EE9C;

        public static uint LevelTriangleCountAddress { get => RomVersionConfig.SwitchMap(LevelTriangleCountAddressUS, LevelTriangleCountAddressJP, LevelTriangleCountAddressSH); }
        public static readonly uint LevelTriangleCountAddressUS = 0x80361178;
        public static readonly uint LevelTriangleCountAddressJP = 0x8035FE08;
        public static readonly uint LevelTriangleCountAddressSH = 0x80343338;

        public static uint TotalTriangleCountAddress { get => RomVersionConfig.SwitchMap(TotalTriangleCountAddressUS, TotalTriangleCountAddressJP, TotalTriangleCountAddressSH); }
        public static readonly uint TotalTriangleCountAddressUS = 0x80361170;
        public static readonly uint TotalTriangleCountAddressJP = 0x8035FE00;
        public static readonly uint TotalTriangleCountAddressSH = 0x80343330;

        public static uint NodeListPointerAddress { get => RomVersionConfig.SwitchMap(NodeListPointerAddressUS, NodeListPointerAddressJP); }
        public static readonly uint NodeListPointerAddressUS = 0x8038EE98;
        public static readonly uint NodeListPointerAddressJP = 0x8038EE98;

        public static uint LevelNodeCountAddress { get => RomVersionConfig.SwitchMap(LevelNodeCountAddressUS, LevelNodeCountAddressJP); }
        public static readonly uint LevelNodeCountAddressUS = 0x80361174;
        public static readonly uint LevelNodeCountAddressJP = 0x8035FE04;

        public static uint TotalNodeCountAddress { get => RomVersionConfig.SwitchMap(TotalNodeCountAddressUS, TotalNodeCountAddressJP); }
        public static readonly uint TotalNodeCountAddressUS = 0x8036116C;
        public static readonly uint TotalNodeCountAddressJP = 0x8035FDFC;

        public static uint ExertionForceTableAddress { get => RomVersionConfig.SwitchMap(ExertionForceTableAddressUS, ExertionForceTableAddressJP); }
        public static readonly uint ExertionForceTableAddressUS = 0x8032DD38;
        public static readonly uint ExertionForceTableAddressJP = 0x8032CDD8;

        public static uint StaticTrianglePartitionAddress { get => RomVersionConfig.SwitchMap(StaticTrianglePartitionAddressUS, StaticTrianglePartitionAddressJP); }
        public static readonly uint StaticTrianglePartitionAddressUS = 0x8038BE98;
        public static readonly uint StaticTrianglePartitionAddressJP = 0x8038BE98;

        public static uint DynamicTrianglePartitionAddress { get => RomVersionConfig.SwitchMap(DynamicTrianglePartitionAddressUS, DynamicTrianglePartitionAddressJP); }
        public static readonly uint DynamicTrianglePartitionAddressUS = 0x8038D698;
        public static readonly uint DynamicTrianglePartitionAddressJP = 0x8038D698;
    }
}
