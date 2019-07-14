using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class TtcObjectConfig
    {
        public static uint TtcRotatingBlockAddress { get => RomVersionConfig.Switch(TtcRotatingBlockAddressUS, TtcRotatingBlockAddressJP); }
        public static readonly uint TtcRotatingBlockAddressUS = 0x8033D488;
        public static readonly uint TtcRotatingBlockAddressJP = 0x8033C118;

        public static uint TtcRotatingTriangularPrismAddress { get => RomVersionConfig.Switch(TtcRotatingTriangularPrismAddressUS, TtcRotatingTriangularPrismAddressJP); }
        public static readonly uint TtcRotatingTriangularPrismAddressUS = 0x8033E2C8;
        public static readonly uint TtcRotatingTriangularPrismAddressJP = 0x8033CF58;

        public static uint TtcPendulumAddress { get => RomVersionConfig.Switch(TtcPendulumAddressUS, TtcPendulumAddressJP); }
        public static readonly uint TtcPendulumAddressUS = 0x8033E788;
        public static readonly uint TtcPendulumAddressJP = 0x8033D418;

        public static uint TtcTreadmill1Address { get => RomVersionConfig.Switch(TtcTreadmill1AddressUS, TtcTreadmill1AddressJP); }
        public static readonly uint TtcTreadmill1AddressUS = 0x8033F108;
        public static readonly uint TtcTreadmill1AddressJP = 0x8033DD98;

        public static uint TtcPusherAddress { get => RomVersionConfig.Switch(TtcPusherAddressUS, TtcPusherAddressJP); }
        public static readonly uint TtcPusherAddressUS = 0x8033FCE8;
        public static readonly uint TtcPusherAddressJP = 0x8033E978;

        public static uint TtcCog1Address { get => RomVersionConfig.Switch(TtcCog1AddressUS, TtcCog1AddressJP); }
        public static readonly uint TtcCog1AddressUS = 0x80341968;
        public static readonly uint TtcCog1AddressJP = 0x803405F8;

        public static uint TtcSpinningTriangleAddress { get => RomVersionConfig.Switch(TtcSpinningTriangleAddressUS, TtcSpinningTriangleAddressJP); }
        public static readonly uint TtcSpinningTriangleAddressUS = 0x80342548;
        public static readonly uint TtcSpinningTriangleAddressJP = 0x803411D8;

        public static uint TtcPitBlockAddress { get => RomVersionConfig.Switch(TtcPitBlockAddressUS, TtcPitBlockAddressJP); }
        public static readonly uint TtcPitBlockAddressUS = 0x80342A08;
        public static readonly uint TtcPitBlockAddressJP = 0x80341698;

        public static uint TtcHandAddress { get => RomVersionConfig.Switch(TtcHandAddressUS, TtcHandAddressJP); }
        public static readonly uint TtcHandAddressUS = 0x80342C68;
        public static readonly uint TtcHandAddressJP = 0x803418F8;

        public static uint TtcSpinnerAddress { get => RomVersionConfig.Switch(TtcSpinnerAddressUS, TtcSpinnerAddressJP); }
        public static readonly uint TtcSpinnerAddressUS = 0x80343128;
        public static readonly uint TtcSpinnerAddressJP = 0x80341DB8;

        public static uint TtcWheelAddress { get => RomVersionConfig.Switch(TtcWheelAddressUS, TtcWheelAddressJP); }
        public static readonly uint TtcWheelAddressUS = 0x80345268;
        public static readonly uint TtcWheelAddressJP = 0x80343EF8;

        public static uint TtcElevatorAddress { get => RomVersionConfig.Switch(TtcElevatorAddressUS, TtcElevatorAddressJP); }
        public static readonly uint TtcElevatorAddressUS = 0x80347608;
        public static readonly uint TtcElevatorAddressJP = 0x80346298;

        public static uint TtcCog2Address { get => RomVersionConfig.Switch(TtcCog2AddressUS, TtcCog2AddressJP); }
        public static readonly uint TtcCog2AddressUS = 0x8034B3C8;
        public static readonly uint TtcCog2AddressJP = 0x8034A058;

        public static uint TtcTreadmill2Address { get => RomVersionConfig.Switch(TtcTreadmill2AddressUS, TtcTreadmill2AddressJP); }
        public static readonly uint TtcTreadmill2AddressUS = 0x8034D508;
        public static readonly uint TtcTreadmill2AddressJP = 0x8034C198;

        public static uint TtcThwompAddress { get => RomVersionConfig.Switch(TtcThwompAddressUS, TtcThwompAddressJP); }
        public static readonly uint TtcThwompAddressUS = 0x8034E808;
        public static readonly uint TtcThwompAddressJP = 0x8034D498;

        public static uint TtcAmp1Address { get => RomVersionConfig.Switch(TtcAmp1AddressUS, TtcAmp1AddressJP); }
        public static readonly uint TtcAmp1AddressUS = 0x80347AC8;
        public static readonly uint TtcAmp1AddressJP = 0x80346758;

        public static uint TtcAmp2Address { get => RomVersionConfig.Switch(TtcAmp2AddressUS, TtcAmp2AddressJP); }
        public static readonly uint TtcAmp2AddressUS = 0x8034A328;
        public static readonly uint TtcAmp2AddressJP = 0x80348FB8;
    }
}
