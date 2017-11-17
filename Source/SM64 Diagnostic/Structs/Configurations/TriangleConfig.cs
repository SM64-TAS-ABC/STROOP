using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct TriangleConfig
    {
        public uint TriangleStructSize;

        public uint TriangleListPointerAddress;

        public uint LevelTriangleCountAddress { get { return Config.SwitchRomVersion(LevelTriangleCountAddressUS, LevelTriangleCountAddressJP); } }
        public uint LevelTriangleCountAddressUS;
        public uint LevelTriangleCountAddressJP;

        public uint TotalTriangleCountAddress { get { return Config.SwitchRomVersion(TotalTriangleCountAddressUS, TotalTriangleCountAddressJP); } }
        public uint TotalTriangleCountAddressUS;
        public uint TotalTriangleCountAddressJP;

        public uint NodeListPointerAddress;

        public uint LevelNodeCountAddress { get { return Config.SwitchRomVersion(LevelNodeCountAddressUS, LevelNodeCountAddressJP); } }
        public uint LevelNodeCountAddressUS;
        public uint LevelNodeCountAddressJP;

        public uint TotalNodeCountAddress { get { return Config.SwitchRomVersion(TotalNodeCountAddressUS, TotalNodeCountAddressJP); } }
        public uint TotalNodeCountAddressUS;
        public uint TotalNodeCountAddressJP;

        public uint ExertionForceTableAddress { get { return Config.SwitchRomVersion(ExertionForceTableAddressUS, ExertionForceTableAddressJP); } }
        public uint ExertionForceTableAddressUS;
        public uint ExertionForceTableAddressJP;
    }
}
