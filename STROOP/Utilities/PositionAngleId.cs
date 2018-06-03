using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public class PositionAngleId
    {
        public readonly PositionAngleTypeEnum PosAngleType;
        public readonly uint? Address;

        public PositionAngleId(PositionAngleTypeEnum posAngleType, uint? address = null)
        {
            PosAngleType = posAngleType;
            Address = address;

            bool shouldHaveAddress =
                posAngleType == PositionAngleTypeEnum.Object ||
                posAngleType == PositionAngleTypeEnum.ObjectHome;
            if (address.HasValue != shouldHaveAddress)
                throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            string suffix = Address.HasValue ? " " + HexUtilities.FormatByValue(Address.Value) : "";
            return PosAngleType + suffix;
        }
    }
}
