using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct WatchVariable
    {
        public Type Type;
        public uint Address;
        public String Name;
        public Boolean AbsoluteAddressing;
        public UInt64? Mask;
        public bool IsBool;
        public bool UseHex;
        public bool OtherOffset;
        public bool InvertBool;
        public bool IsAngle;
    }
}
