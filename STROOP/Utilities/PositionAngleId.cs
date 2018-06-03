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
        public readonly PositionAngleTypeEnum PositionAngleType;
        public readonly uint? Address;

        public PositionAngleId(PositionAngleTypeEnum positionAngleType, uint? address = null)
        {
            PositionAngleType = positionAngleType;
            Address = address;

            bool shouldHaveAddress =
                positionAngleType == PositionAngleTypeEnum.Object ||
                positionAngleType == PositionAngleTypeEnum.ObjectHome;
            if (address.HasValue != shouldHaveAddress)
                throw new ArgumentOutOfRangeException();
        }
    }
}
