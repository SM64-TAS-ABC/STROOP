using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct HudConfig
    {
        public uint StarCountOffset;
        public uint CoinCountOffset;
        public uint LifeCountOffset;
        public uint HpCountOffset;
        public uint StarDisplayOffset;
        public uint CoinDisplayOffset;
        public uint LifeDisplayOffset;
        public uint HpDisplayOffset;

        public uint HpAddress;
        public uint LiveCountAddress;
        public uint CoinCountAddress;
        public uint StarCountAddress;
        public uint DisplayHpAddress;
        public uint DisplayLiveCountAddress;
        public uint DisplayCoinCountAddress;
        public uint DisplayStarCountAddress;

        public short FullHp;
        public short FullHpInt;
        public sbyte StandardLives;
        public short StandardCoins;
        public short StandardStars;
    }
}
