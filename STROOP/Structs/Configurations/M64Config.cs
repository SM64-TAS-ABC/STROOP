using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class M64Config
    {
        public static readonly int HeaderSize = 0x400;
        public static readonly byte[] SignatureBytes = new byte[] { 0x4D, 0x36, 0x34, 0x1A };

    }
}
