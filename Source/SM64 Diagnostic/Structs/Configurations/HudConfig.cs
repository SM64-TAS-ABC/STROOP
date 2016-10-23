using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct HudConfig
    {
        public uint HpAddress;
        public uint LiveCountAddress;
        public uint CoinCountAddress;
        public uint StarCountAddress;
        public short FullHp;
        public short StandardLives;
        public short StandardCoins;
        public short StandardStars;
    }
}
