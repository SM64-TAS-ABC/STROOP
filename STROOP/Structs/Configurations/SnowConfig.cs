using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class SnowConfig
    {
        public static uint SnowArrayPointerAddress { get => RomVersionConfig.Switch(SnowArrayPointerAddressUS, SnowArrayPointerAddressJP); }
        public static readonly uint SnowArrayPointerAddressUS = 0x80361400;
        public static readonly uint SnowArrayPointerAddressJP = 0x80360090;

        public static uint ParticleStructSize = 0x38;

        public static uint XOffset = 0x04;
        public static uint YOffset = 0x08;
        public static uint ZOffset = 0x0C;
    }
}
