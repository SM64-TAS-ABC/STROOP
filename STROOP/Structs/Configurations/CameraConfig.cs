using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class CameraConfig
    {
        public static uint StructAddress { get => RomVersionConfig.Switch(StructAddressUS, StructAddressJP, StructAddressSH); }
        public static readonly uint StructAddressUS = 0x8033C520;
        public static readonly uint StructAddressJP = 0x8033B1B0;
        public static readonly uint StructAddressSH = 0x8031EC78;

        public static readonly uint XOffset = 0x184;
        public static readonly uint YOffset = 0x188;
        public static readonly uint ZOffset = 0x18C;
        public static readonly uint FocusXOffset = 0x178;
        public static readonly uint FocusYOffset = 0x17C;
        public static readonly uint FocusZOffset = 0x180;
        public static readonly uint FacingYawOffset = 0x1C6;
        public static readonly uint FacingPitchOffset = 0x1C4;
        public static readonly uint FacingRollOffset = 0x1C8;
        public static readonly uint CentripetalAngleOffset = 0x1F4;

        public static readonly uint MarioCamPossibleOffset = 0x165;
        public static readonly byte MarioCamPossibleMask = 0x04;

        public static uint FOVAddress { get => RomVersionConfig.Switch(FOVAddressUS, FOVAddressJP); }
        public static readonly uint FOVAddressUS = 0x8033C5A4;
        public static readonly uint FOVAddressJP = 0x8033B234;

        public static uint SecondaryObjectAddress { get => RomVersionConfig.Switch(SecondaryObjectAddressUS, SecondaryObjectAddressJP); }
        public static readonly uint SecondaryObjectAddressUS = 0x8032DF30;
        public static readonly uint SecondaryObjectAddressJP = 0x8032CFD0;


        public static uint FovFunctionAwakeAddress { get => RomVersionConfig.Switch(FovFunctionAwakeAddressUS, FovFunctionAwakeAddressJP); }
        public static readonly uint FovFunctionAwakeAddressUS = 0x8029A7C8;
        public static readonly uint FovFunctionAwakeAddressJP = 0x8029A0AC;

        public static uint FovFunctionSleepingAddress { get => RomVersionConfig.Switch(FovFunctionSleepingAddressUS, FovFunctionSleepingAddressJP); }
        public static readonly uint FovFunctionSleepingAddressUS = 0x8029A774;
        public static readonly uint FovFunctionSleepingAddressJP = 0x8029A058;

        public static uint FovFunctionUseDoorAddress { get => RomVersionConfig.Switch(FovFunctionUseDoorAddressUS, FovFunctionUseDoorAddressJP); }
        public static readonly uint FovFunctionUseDoorAddressUS = 0x8029AA20;
        public static readonly uint FovFunctionUseDoorAddressJP = 0x8029A304;

        public static uint FovFunctionCollectStarAddress { get => RomVersionConfig.Switch(FovFunctionCollectStarAddressUS, FovFunctionCollectStarAddressJP); }
        public static readonly uint FovFunctionCollectStarAddressUS = 0x8029A984;
        public static readonly uint FovFunctionCollectStarAddressJP = 0x8029A268;


        public static uint FovFunctionAwakeValue { get => RomVersionConfig.Switch(FovFunctionAwakeValueUS, FovFunctionAwakeValueJP); }
        public static readonly uint FovFunctionAwakeValueUS = 0x0C0A2673;
        public static readonly uint FovFunctionAwakeValueJP = 0x0C0A24F9;

        public static uint FovFunctionSleepingValue { get => RomVersionConfig.Switch(FovFunctionSleepingValueUS, FovFunctionSleepingValueJP); }
        public static readonly uint FovFunctionSleepingValueUS = 0x0C0A2673;
        public static readonly uint FovFunctionSleepingValueJP = 0x0C0A24F9;

        public static uint FovFunctionUseDoorValue { get => RomVersionConfig.Switch(FovFunctionUseDoorValueUS, FovFunctionUseDoorValueJP); }
        public static readonly uint FovFunctionUseDoorValueUS = 0xE420C5A4;
        public static readonly uint FovFunctionUseDoorValueJP = 0xE420B234;

        public static uint FovFunctionCollectStarValue { get => RomVersionConfig.Switch(FovFunctionCollectStarValueUS, FovFunctionCollectStarValueJP); }
        public static readonly uint FovFunctionCollectStarValueUS = 0x0C0A2673;
        public static readonly uint FovFunctionCollectStarValueJP = 0x0C0A24F9;

        public static List<uint> FovFunctionAddresses
        {
            get
            {
                return new List<uint>()
                {
                    FovFunctionAwakeAddress,
                    FovFunctionSleepingAddress,
                    FovFunctionUseDoorAddress,
                    FovFunctionCollectStarAddress,
                };
            }
        }

        public static List<uint> FovFunctionValues
        {
            get
            {
                return new List<uint>()
                {
                    FovFunctionAwakeValue,
                    FovFunctionSleepingValue,
                    FovFunctionUseDoorValue,
                    FovFunctionCollectStarValue,
                };
            }
        }
    }
}
