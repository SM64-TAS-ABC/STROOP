using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public enum ExecuteModeType {Once, Always, UserCalledS };
    public struct GameScript
    {
        public uint[] Script;
        public uint InsertAddress;
        public ExecuteModeType ExecuteMode;
        public int MemoryPadding;
        public uint ExecutionSpace;
        public bool Allocated;
    }
}
