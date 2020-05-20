﻿using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class MiscConfig
    {
        public static uint LevelAddress { get => RomVersionConfig.Switch(LevelAddressUS, LevelAddressJP, LevelAddressSH, LevelAddressEU); }
        public static readonly uint LevelAddressUS = 0x8033B249;
        public static readonly uint LevelAddressJP = 0x80339ED9;
        public static readonly uint LevelAddressSH = 0x8031DAA1;
        public static readonly uint LevelAddressEU = 0x80309511;

        public static uint AreaAddress { get => RomVersionConfig.Switch(AreaAddressUS, AreaAddressJP, AreaAddressSH, AreaAddressEU); }
        public static readonly uint AreaAddressUS = 0x8033B24A;
        public static readonly uint AreaAddressJP = 0x80339EDA;
        public static readonly uint AreaAddressSH = 0x8031DAA2;
        public static readonly uint AreaAddressEU = 0x80309512;

        public static uint LoadingPointAddress { get => RomVersionConfig.Switch(LoadingPointAddressUS, LoadingPointAddressJP, LoadingPointAddressSH, LoadingPointAddressEU); }
        public static readonly uint LoadingPointAddressUS = 0x8033BACA;
        public static readonly uint LoadingPointAddressJP = 0x8033A75A;
        public static readonly uint LoadingPointAddressSH = 0x8031E31A;
        public static readonly uint LoadingPointAddressEU = 0x80309D8A;

        public static uint MissionAddress { get => RomVersionConfig.Switch(MissionAddressUS, MissionAddressJP, MissionAddressSH, MissionAddressEU); }
        public static readonly uint MissionAddressUS = 0x8033BAC8;
        public static readonly uint MissionAddressJP = 0x8033A758;
        public static readonly uint MissionAddressSH = 0x8031E318;
        public static readonly uint MissionAddressEU = 0x80309D88;

        public static uint LevelIndexAddress { get => RomVersionConfig.Switch(LevelIndexAddressUS, LevelIndexAddressJP, LevelIndexAddressSH, LevelIndexAddressEU); }
        public static readonly uint LevelIndexAddressUS = 0x8033BAC6;
        public static readonly uint LevelIndexAddressJP = 0x8033A756;
        public static readonly uint LevelIndexAddressSH = 0x8031E316;
        public static readonly uint LevelIndexAddressEU = 0x80309D86;

        public static uint WaterLevelMedianAddress { get => RomVersionConfig.Switch(WaterLevelMedianAddressUS, WaterLevelMedianAddressJP, WaterLevelMedianAddressSH, WaterLevelMedianAddressEU); }
        public static readonly uint WaterLevelMedianAddressUS = 0x8036118A;
        public static readonly uint WaterLevelMedianAddressJP = 0x8035FE1A;
        public static readonly uint WaterLevelMedianAddressSH = 0x8034334A;
        public static readonly uint WaterLevelMedianAddressEU = 0x8032EDBA;

        public static uint WaterPointerAddress { get => RomVersionConfig.Switch(WaterPointerAddressUS, WaterPointerAddressJP, null, WaterPointerAddressEU); }
        public static readonly uint WaterPointerAddressUS = 0x80361184;
        public static readonly uint WaterPointerAddressJP = 0x8035FE14;
        public static readonly uint WaterPointerAddressEU = 0x8032EDB4;

        public static uint CurrentFileAddress { get => RomVersionConfig.Switch(CurrentFileAddressUS, CurrentFileAddressJP, CurrentFileAddressSH, CurrentFileAddressEU); }
        public static readonly uint CurrentFileAddressUS = 0x8032DDF4;
        public static readonly uint CurrentFileAddressJP = 0x8032CE94;
        public static readonly uint CurrentFileAddressSH = 0x8030D524;
        public static readonly uint CurrentFileAddressEU = 0x802F9FC4;

        public static uint SpecialTripleJumpAddress { get => RomVersionConfig.Switch(SpecialTripleJumpAddressUS, SpecialTripleJumpAddressJP, SpecialTripleJumpAddressEU); }
        public static readonly uint SpecialTripleJumpAddressUS = 0x8032DD94;
        public static readonly uint SpecialTripleJumpAddressJP = 0x8032CE34;
        public static readonly uint SpecialTripleJumpAddressEU = 0x802F9F64;

        public static uint HackedAreaAddress { get => RomVersionConfig.Switch(HackedAreaAddressUS, HackedAreaAddressJP, HackedAreaAddressSH, HackedAreaAddressEU); }
        public static readonly uint HackedAreaAddressUS = 0x803E0000;
        public static readonly uint HackedAreaAddressJP = 0x803E0000;
        public static readonly uint HackedAreaAddressSH = 0x803E0000;
        public static readonly uint HackedAreaAddressEU = 0x803E0000;

        public static uint GlobalTimerAddress { get => RomVersionConfig.Switch(GlobalTimerAddressUS, GlobalTimerAddressJP, GlobalTimerAddressSH, GlobalTimerAddressEU); }
        public static readonly uint GlobalTimerAddressUS = 0x8032D5D4;
        public static readonly uint GlobalTimerAddressJP = 0x8032C694;
        public static readonly uint GlobalTimerAddressSH = 0x8030CD04;
        public static readonly uint GlobalTimerAddressEU = 0x802F9784;

        public static uint RngAddress { get => RomVersionConfig.Switch(RngAddressUS, RngAddressJP, RngAddressSH, RngAddressEU); }
        public static readonly uint RngAddressUS = 0x8038EEE0;
        public static readonly uint RngAddressJP = 0x8038EEE0;
        public static readonly uint RngAddressSH = 0x8038BBC0; 
        public static readonly uint RngAddressEU = 0x80389C60; 

        public static uint AnimationTimerAddress { get => RomVersionConfig.Switch(AnimationTimerAddressUS, AnimationTimerAddressJP, null, AnimationTimerAddressEU); }
        public static readonly uint AnimationTimerAddressUS = 0x8032DF08;
        public static readonly uint AnimationTimerAddressJP = 0x8032CFA8;
        public static readonly uint AnimationTimerAddressEU = 0x802FA0E8;

        public static uint MusicOnAddress { get => RomVersionConfig.Switch(MusicOnAddressUS, MusicOnAddressJP, null, MusicOnAddressEU); }
        public static readonly uint MusicOnAddressUS = 0x80222618;
        public static readonly uint MusicOnAddressJP = 0x80222A18;
        public static readonly uint MusicOnAddressEU = 0x80223D68;

        public static readonly byte MusicOnMask = 0x20;

        public static uint MusicVolumeAddress { get => RomVersionConfig.Switch(MusicVolumeAddressUS, MusicVolumeAddressJP, null, MusicVolumeAddressEU); }
        public static readonly uint MusicVolumeAddressUS = 0x80222630;
        public static readonly uint MusicVolumeAddressJP = 0x80222A30;
        public static readonly uint MusicVolumeAddressEU = 0x80223D80;

        public static uint DemoCounterAddress { get => RomVersionConfig.Switch(DemoCounterAddressUS, DemoCounterAddressJP, null, DemoCounterAddressEU); }
        public static readonly uint DemoCounterAddressUS = 0x8032D5F4;
        public static readonly uint DemoCounterAddressJP = 0x8032C6B4;
        public static readonly uint DemoCounterAddressEU = 0x802F97A4;

        public static uint TtcSpeedSettingAddress { get => RomVersionConfig.Switch(TtcSpeedSettingAddressUS, TtcSpeedSettingAddressJP, null, TtcSpeedSettingAddressEU); }
        public static readonly uint TtcSpeedSettingAddressUS = 0x80361258;
        public static readonly uint TtcSpeedSettingAddressJP = 0x8035FEE8;
        public static readonly uint TtcSpeedSettingAddressEU = 0x8032EE88;
    }
}
