using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Structs
{
    public class ObjectGroupsConfig
    {
        public List<byte> ProcessingGroups;
        public Dictionary<byte,Color> ProcessingGroupsColor;

        public uint VactantPointerAddress { get { return Config.SwitchRomVersion(VactantPointerAddressUS, VactantPointerAddressJP); } }
        public uint VactantPointerAddressUS;
        public uint VactantPointerAddressJP;

        public Color VacantSlotColor;
        public uint ProcessNextLinkOffset;
        public uint ProcessPreviousLinkOffset;
        public uint ParentObjectOffset;

        public uint FirstGroupingAddress { get { return Config.SwitchRomVersion(FirstGroupingAddressUS, FirstGroupingAddressJP); } }
        public uint FirstGroupingAddressUS;
        public uint FirstGroupingAddressJP;

        public uint ProcessGroupStructSize;
    }
}
