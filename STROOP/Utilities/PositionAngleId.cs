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
        public readonly int? TriVertex;

        public PositionAngleId(
            PositionAngleTypeEnum posAngleType,
            uint? address = null,
            int? triVertex = null)
        {
            PosAngleType = posAngleType;
            Address = address;
            TriVertex = triVertex;

            bool shouldHaveAddress =
                posAngleType == PositionAngleTypeEnum.Object ||
                posAngleType == PositionAngleTypeEnum.ObjectHome;
            if (address.HasValue != shouldHaveAddress)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveTriVertex =
                posAngleType == PositionAngleTypeEnum.Tri;
            if (triVertex.HasValue != shouldHaveTriVertex)
                throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            string suffix = Address.HasValue ? " " + HexUtilities.FormatByValue(Address.Value) : "";
            return PosAngleType + suffix;
        }

        public static PositionAngleId FromString(string stringValue)
        {
            return null;
        }
    }
}
