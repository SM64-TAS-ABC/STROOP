using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct HudConfig
    {
        public uint LifeCountOffset;
        public uint HpCountOffset;
        public uint CoinCountOffset;
        public uint StarCountOffset;

        public uint LifeDisplayOffset;
        public uint HpDisplayOffset;
        public uint CoinDisplayOffset;
        public uint StarDisplayOffset;

        public uint TimeOffset;

        public uint VisibilityOffset;
        public byte VisibilityMask;

        public short FullHp;
        public short FullHpInt;
        public sbyte StandardLives;
        public short StandardCoins;
        public short StandardStars;
    }
}
