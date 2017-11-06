using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct TriangleConfig
    {
        public uint TriangleListPointerAddress;

        public uint LevelTriangleCountAddress { get { return Config.SwitchRomVersion(LevelTriangleCountAddressUS, LevelTriangleCountAddressJP); } }
        public uint LevelTriangleCountAddressUS;
        public uint LevelTriangleCountAddressJP;

        public uint TotalTriangleCountAddress { get { return Config.SwitchRomVersion(TotalTriangleCountAddressUS, TotalTriangleCountAddressJP); } }
        public uint TotalTriangleCountAddressUS;
        public uint TotalTriangleCountAddressJP;
    }
}
