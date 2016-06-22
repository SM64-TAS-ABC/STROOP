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

        public ProcessSelection(Process process)
        {
            Process = process;
        }

        public override string ToString()
        {
            return this.Process.ProcessName;
        }
    }
}
