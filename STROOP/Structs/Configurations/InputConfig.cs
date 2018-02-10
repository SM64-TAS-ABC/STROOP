using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class InputConfig
    {
        public static uint CurrentInputAddress { get { return Config.SwitchRomVersion(CurrentInputAddressUS, CurrentInputAddressJP); } }
        public static readonly uint CurrentInputAddressUS = 0x8033AFF8;
        public static readonly uint CurrentInputAddressJP = 0x80339C88;

        public static uint JustPressedInputAddress { get { return Config.SwitchRomVersion(JustPressedInputAddressUS, JustPressedInputAddressJP); } }
        public static readonly uint JustPressedInputAddressUS = 0x8033AFA2;
        public static readonly uint JustPressedInputAddressJP = 0x80339C32;

        public static uint BufferedInputAddress { get { return Config.SwitchRomVersion(BufferedInputAddressUS, BufferedInputAddressJP); } }
        public static readonly uint BufferedInputAddressUS = 0x80367054;
        public static readonly uint BufferedInputAddressJP = 0x80365CE4;

        public static readonly uint ButtonAOffset = 0x00;
        public static readonly uint ButtonBOffset = 0x00;
        public static readonly uint ButtonZOffset = 0x00;
        public static readonly uint ButtonStartOffset = 0x00;
        public static readonly uint ButtonROffset = 0x01;
        public static readonly uint ButtonLOffset = 0x01;
        public static readonly uint ButtonCUpOffset = 0x01;
        public static readonly uint ButtonCDownOffset = 0x01;
        public static readonly uint ButtonCLeftOffset = 0x01;
        public static readonly uint ButtonCRightOffset = 0x01;
        public static readonly uint ButtonDUpOffset = 0x00;
        public static readonly uint ButtonDDownOffset = 0x00;
        public static readonly uint ButtonDLeftOffset = 0x00;
        public static readonly uint ButtonDRightOffset = 0x00;
        public static readonly uint ControlStickXOffset = 0x02;
        public static readonly uint ControlStickYOffset = 0x03;

        public static readonly uint ButtonAMask = 0x80;
        public static readonly uint ButtonBMask = 0x40;
        public static readonly uint ButtonZMask = 0x20;
        public static readonly uint ButtonStartMask = 0x10;
        public static readonly uint ButtonRMask = 0x10;
        public static readonly uint ButtonLMask = 0x20;
        public static readonly uint ButtonCUpMask = 0x08;
        public static readonly uint ButtonCDownMask = 0x04;
        public static readonly uint ButtonCLeftMask = 0x02;
        public static readonly uint ButtonCRightMask = 0x01;
        public static readonly uint ButtonDUpMask = 0x08;
        public static readonly uint ButtonDDownMask = 0x04;
        public static readonly uint ButtonDLeftMask = 0x02;
        public static readonly uint ButtonDRightMask = 0x01;
    }
}
