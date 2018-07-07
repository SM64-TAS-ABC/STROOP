using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class HudConfig
    {
        public static readonly uint LifeCountOffset = 0xAD;
        public static readonly uint HpCountOffset = 0xAE;
        public static readonly uint CoinCountOffset = 0xA8;
        public static readonly uint StarCountOffset = 0xAA;

        public static readonly uint LifeDisplayOffset = 0xF0;
        public static readonly uint HpDisplayOffset = 0xF6;
        public static readonly uint CoinDisplayOffset = 0xF2;
        public static readonly uint StarDisplayOffset = 0xF4;

        public static readonly uint CheckedStarCountOffset = 0xB8;

        public static readonly uint TimeOffset = 0xFC;

        public static readonly uint VisibilityOffset = 0xFB;
        public static readonly byte VisibilityMask = 0x0F;

        public static readonly short FullHp = 2176;
        public static readonly short FullHpInt = 8;
        public static readonly short DeathHp = 255;
        public static readonly sbyte StandardLives = 4;
        public static readonly short StandardCoins = 0;
        public static readonly short StandardStars = 120;

        public static uint FunctionEnableCoinDisplayAddress { get => RomVersionConfig.Switch(FunctionEnableCoinDisplayAddressUS, FunctionEnableCoinDisplayAddressJP); }
        public static readonly uint FunctionEnableCoinDisplayAddressUS = 0x8024B19C;
        public static readonly uint FunctionEnableCoinDisplayAddressJP = 0x8024B01C;

        public static uint FunctionDisableCoinDisplayAddress { get => RomVersionConfig.Switch(FunctionDisableCoinDisplayAddressUS, FunctionDisableCoinDisplayAddressJP); }
        public static readonly uint FunctionDisableCoinDisplayAddressUS = 0x8024B1B4;
        public static readonly uint FunctionDisableCoinDisplayAddressJP = 0x8024B034;
    }
}
