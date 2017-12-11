using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct InputConfig
    {
        public uint CurrentInputAddress { get { return Config.SwitchRomVersion(CurrentInputAddressUS, CurrentInputAddressJP); } }
        public uint CurrentInputAddressUS;
        public uint CurrentInputAddressJP;

        public uint JustPressedInputAddress { get { return Config.SwitchRomVersion(JustPressedInputAddressUS, JustPressedInputAddressJP); } }
        public uint JustPressedInputAddressUS;
        public uint JustPressedInputAddressJP;

        public uint BufferedInputAddress { get { return Config.SwitchRomVersion(BufferedInputAddressUS, BufferedInputAddressJP); } }
        public uint BufferedInputAddressUS;
        public uint BufferedInputAddressJP;

        public uint ButtonAOffset;
        public uint ButtonBOffset;
        public uint ButtonZOffset;
        public uint ButtonStartOffset;
        public uint ButtonROffset;
        public uint ButtonLOffset;
        public uint ButtonCUpOffset;
        public uint ButtonCDownOffset;
        public uint ButtonCLeftOffset;
        public uint ButtonCRightOffset;
        public uint ButtonDUpOffset;
        public uint ButtonDDownOffset;
        public uint ButtonDLeftOffset;
        public uint ButtonDRightOffset;
        public uint ControlStickXOffset;
        public uint ControlStickYOffset;
        public uint ButtonAMask;
        public uint ButtonBMask;
        public uint ButtonZMask;
        public uint ButtonStartMask;
        public uint ButtonRMask;
        public uint ButtonLMask;
        public uint ButtonCUpMask;
        public uint ButtonCDownMask;
        public uint ButtonCLeftMask;
        public uint ButtonCRightMask;
        public uint ButtonDUpMask;
        public uint ButtonDDownMask;
        public uint ButtonDLeftMask;
        public uint ButtonDRightMask;
    }
}
