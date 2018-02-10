using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SM64_Diagnostic.Utilities
{
    public struct ProcessSelection
    {
        public Process Process;
        public int Index;

        public ProcessSelection(Process process, int index)
        {
            Process = process;
            Index = index;
        }

        public override string ToString()
        {
            return Index + ". " + this.Process.ProcessName;
        }
    }
}
