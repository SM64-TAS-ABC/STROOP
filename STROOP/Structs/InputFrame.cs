using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class InputFrame
    {
        public sbyte ControlStickH { get; set; }
        public sbyte ControlStickV { get; set; }

        public bool IsButtonPressed_A { get; set; }
        public bool IsButtonPressed_B { get; set; }
        public bool IsButtonPressed_Z { get; set; }
        public bool IsButtonPressed_Start { get; set; }
        public bool IsButtonPressed_R { get; set; }
        public bool IsButtonPressed_L { get; set; }
        public bool IsButtonPressed_CUp { get; set; }
        public bool IsButtonPressed_CDown { get; set; }
        public bool IsButtonPressed_CLeft { get; set; }
        public bool IsButtonPressed_CRight { get; set; }
        public bool IsButtonPressed_DUp { get; set; }
        public bool IsButtonPressed_DDown { get; set; }
        public bool IsButtonPressed_DLeft { get; set; }
        public bool IsButtonPressed_DRight { get; set; }
        public bool IsButtonPressed_U1 { get; set; }
        public bool IsButtonPressed_U2 { get; set; }

        public static InputFrame GetCurrent()
        {
            uint inputAddress = InputConfig.CurrentInputAddress;

            return new InputFrame()
            {

                IsButtonPressed_A = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonAOffset) & InputConfig.ButtonAMask) != 0,
                IsButtonPressed_B = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonBOffset) & InputConfig.ButtonBMask) != 0,
                IsButtonPressed_Z = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonZOffset) & InputConfig.ButtonZMask) != 0,
                IsButtonPressed_Start = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonStartOffset) & InputConfig.ButtonStartMask) != 0,
                IsButtonPressed_R = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonROffset) & InputConfig.ButtonRMask) != 0,
                IsButtonPressed_L = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonLOffset) & InputConfig.ButtonLMask) != 0,
                IsButtonPressed_CUp = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCUpOffset) & InputConfig.ButtonCUpMask) != 0,
                IsButtonPressed_CDown = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCDownOffset) & InputConfig.ButtonCDownMask) != 0,
                IsButtonPressed_CLeft = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCLeftOffset) & InputConfig.ButtonCLeftMask) != 0,
                IsButtonPressed_CRight = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCRightOffset) & InputConfig.ButtonCRightMask) != 0,
                IsButtonPressed_DUp = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDUpOffset) & InputConfig.ButtonDUpMask) != 0,
                IsButtonPressed_DDown = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDDownOffset) & InputConfig.ButtonDDownMask) != 0,
                IsButtonPressed_DLeft = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDLeftOffset) & InputConfig.ButtonDLeftMask) != 0,
                IsButtonPressed_DRight = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDRightOffset) & InputConfig.ButtonDRightMask) != 0,
                IsButtonPressed_U1 = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonU1Offset) & InputConfig.ButtonU1Mask) != 0,
                IsButtonPressed_U2 = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonU2Offset) & InputConfig.ButtonU2Mask) != 0,

                ControlStickH = (sbyte)Config.Stream.GetByte(inputAddress + InputConfig.ControlStickXOffset),
                ControlStickV = (sbyte)Config.Stream.GetByte(inputAddress + InputConfig.ControlStickYOffset)
            };
        }

        public override bool Equals(object obj)
        {
            InputFrame other = obj as InputFrame;
            if (other == null)
                return false;

            return (ControlStickH == other.ControlStickH && ControlStickV == other.ControlStickV
                && IsButtonPressed_A == other.IsButtonPressed_A
                && IsButtonPressed_B == other.IsButtonPressed_B
                && IsButtonPressed_Z == other.IsButtonPressed_Z
                && IsButtonPressed_Start == other.IsButtonPressed_Start
                && IsButtonPressed_L == other.IsButtonPressed_L
                && IsButtonPressed_R == other.IsButtonPressed_R
                && IsButtonPressed_CUp == other.IsButtonPressed_CUp
                && IsButtonPressed_CDown == other.IsButtonPressed_CDown
                && IsButtonPressed_CRight == other.IsButtonPressed_CRight
                && IsButtonPressed_CLeft == other.IsButtonPressed_CLeft
                && IsButtonPressed_DUp == other.IsButtonPressed_DUp
                && IsButtonPressed_DDown == other.IsButtonPressed_DDown
                && IsButtonPressed_DRight == other.IsButtonPressed_DRight
                && IsButtonPressed_DLeft == other.IsButtonPressed_DLeft
                && IsButtonPressed_U1 == other.IsButtonPressed_U1
                && IsButtonPressed_U2 == other.IsButtonPressed_U2
                );
        }
    }
}
