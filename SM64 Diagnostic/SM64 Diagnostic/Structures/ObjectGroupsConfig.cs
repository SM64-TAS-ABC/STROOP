using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SM64_Diagnostic.Structs
{
    public class ObjectGroupsConfig
    {
        public List<byte> ProcessingGroups;
        public Dictionary<byte,Color> ProcessingGroupsColor;
        public uint VactantPointerAddress;
        public Color VacantSlotColor;
        public uint ProcessNextLinkOffset;
        public uint ProcessPreviousLinkOffset;
        public uint ParentObjectOffset;
        public uint FirstGroupingAddress;
        public uint ProcessGroupStructSize;
    }
}
