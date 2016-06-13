using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SM64_Diagnostic.Structs
{
    public struct ObjectSlotData
    {
        public uint Address;
        public byte ObjectProcessGroup;
        public int Index;
        public int? VacantSlotIndex;
    }
}
