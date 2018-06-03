using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class Emulator
    {
        public enum EndianessType
        {
            Big,
            Little
        }
        public string Name;
        public string ProcessName;
        public uint RamStart;
        public string Dll;
        public EndianessType Endianess;
    }
}
