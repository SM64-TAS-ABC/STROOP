using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class SpawnHack
    {
        public string Name;
        public uint Behavior;
        public byte GfxId;
        public byte Extra;

        public override string ToString()
        {
            return Name;
        }
    }
}
