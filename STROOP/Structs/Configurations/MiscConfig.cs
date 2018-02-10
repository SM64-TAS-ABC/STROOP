using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class MiscConfig
    {
        public static uint LevelAddress { get { return Config.SwitchRomVersion(LevelAddressUS, LevelAddressJP); } }
        public static readonly uint LevelAddressUS = 0x8033B249;
        public static readonly uint LevelAddressJP = 0x80339ED9;

        public static uint AreaAddress { get { return Config.SwitchRomVersion(AreaAddressUS, AreaAddressJP); } }
        public static readonly uint AreaAddressUS = 0x8033B24A;
        public static readonly uint AreaAddressJP = 0x80339EDA;

        public static uint LoadingPointAddress { get { return Config.SwitchRomVersion(LoadingPointAddressUS, LoadingPointAddressJP); } }
        public static readonly uint LoadingPointAddressUS = 0x8033BACA;
        public static readonly uint LoadingPointAddressJP = 0x8033A75A;

        public static uint MissionAddress { get { return Config.SwitchRomVersion(MissionAddressUS, MissionAddressJP); } }
        public static readonly uint MissionAddressUS = 0x8033BAC8;
        public static readonly uint MissionAddressJP = 0x8033A758;

        public static uint LevelIndexAddress { get { return Config.SwitchRomVersion(LevelIndexAddressUS, LevelIndexAddressJP); } }
        public static readonly uint LevelIndexAddressUS = 0x8033BAC6;
        public static readonly uint LevelIndexAddressJP = 0x8033A756;

        public static uint WaterLevelMedianAddress { get { return Config.SwitchRomVersion(WaterLevelMedianAddressUS, WaterLevelMedianAddressJP); } }
        public static readonly uint WaterLevelMedianAddressUS = 0x8036118A;
        public static readonly uint WaterLevelMedianAddressJP = 0x8035FE1A;

        public static uint WaterPointerAddress { get { return Config.SwitchRomVersion(WaterPointerAddressUS, WaterPointerAddressJP); } }
        public static readonly uint WaterPointerAddressUS = 0x80361184;
        public static readonly uint WaterPointerAddressJP = 0x8035FE14;

        public static uint CurrentFileAddress { get { return Config.SwitchRomVersion(CurrentFileAddressUS, CurrentFileAddressJP); } }
        public static readonly uint CurrentFileAddressUS = 0x8032DDF4;
        public static readonly uint CurrentFileAddressJP = 0x8032CE94;

        public static uint SpecialTripleJumpAddress { get { return Config.SwitchRomVersion(SpecialTripleJumpAddressUS, SpecialTripleJumpAddressJP); } }
        public static readonly uint SpecialTripleJumpAddressUS = 0x8032DD94;
        public static readonly uint SpecialTripleJumpAddressJP = 0x8032CE34;

        public static uint HackedAreaAddress { get { return Config.SwitchRomVersion(HackedAreaAddressUS, HackedAreaAddressJP); } }
        public static readonly uint HackedAreaAddressUS = 0x803E0000;
        public static readonly uint HackedAreaAddressJP = 0x803E0000;

        public static uint GlobalTimerAddress { get { return Config.SwitchRomVersion(GlobalTimerAddressUS, GlobalTimerAddressJP); } }
        public static readonly uint GlobalTimerAddressUS = 0x8032D5D4;
        public static readonly uint GlobalTimerAddressJP = 0x8032C694;

        public static uint RngAddress { get { return Config.SwitchRomVersion(RngAddressUS, RngAddressJP); } }
        public static readonly uint RngAddressUS = 0x8038EEE0;
        public static readonly uint RngAddressJP = 0x8038EEE0;

        public static uint AnimationTimerAddress { get { return Config.SwitchRomVersion(AnimationTimerAddressUS, AnimationTimerAddressJP); } }
        public static readonly uint AnimationTimerAddressUS = 0x8032DF08;
        public static readonly uint AnimationTimerAddressJP = 0x8032CFA8;

        public static uint MusicOnAddress { get { return Config.SwitchRomVersion(MusicOnAddressUS, MusicOnAddressJP); } }
        public static readonly uint MusicOnAddressUS = 0x80222618;
        public static readonly uint MusicOnAddressJP = 0x80222A18;

        public static readonly byte MusicOnMask = 0x20;

        public static uint MusicVolumeAddress { get { return Config.SwitchRomVersion(MusicVolumeAddressUS, MusicVolumeAddressJP); } }
        public static readonly uint MusicVolumeAddressUS = 0x80222630;
        public static readonly uint MusicVolumeAddressJP = 0x80222A30;
    }
}
