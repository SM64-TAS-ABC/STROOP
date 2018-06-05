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
                posAngleType == PositionAngleTypeEnum.ObjectHome ||
                posAngleType == PositionAngleTypeEnum.Tri;
            if (address.HasValue != shouldHaveAddress)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveTriVertex =
                posAngleType == PositionAngleTypeEnum.Tri;
            if (triVertex.HasValue != shouldHaveTriVertex)
                throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            string addressString = Address.HasValue ? " " + HexUtilities.FormatValue(Address.Value, 8) : "";
            string triVertexString = TriVertex.HasValue ? " V" + TriVertex.Value : "";
            return PosAngleType + addressString + triVertexString;
        }

        public static PositionAngleId FromString(string stringValue)
        {
            stringValue = stringValue.ToLower();
            List<string> parts = ParsingUtilities.ParseStringList(stringValue);

            if (parts.Count == 1 && parts[0] == "custom")
            {
                return new PositionAngleId(PositionAngleTypeEnum.Custom);
            }
            else if (parts.Count == 1 && parts[0] == "mario")
            {
                return new PositionAngleId(PositionAngleTypeEnum.Mario);
            }
            else if (parts.Count == 1 && parts[0] == "holp")
            {
                return new PositionAngleId(PositionAngleTypeEnum.Holp);
            }
            else if (parts.Count == 1 && (parts[0] == "cam" || parts[0] == "camera"))
            {
                return new PositionAngleId(PositionAngleTypeEnum.Camera);
            }
            else if (parts.Count == 2 && (parts[0] == "obj" || parts[0] == "object"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return new PositionAngleId(PositionAngleTypeEnum.Object, address.Value);
            }
            else if (parts.Count == 2 && (parts[0] == "objhome" || parts[0] == "objecthome"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return new PositionAngleId(PositionAngleTypeEnum.ObjectHome, address.Value);
            }
            else if (parts.Count == 3 && (parts[0] == "tri" || parts[0] == "triangle"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                if (parts[2].Length >= 1 && parts[2].Substring(0, 1) == "v") parts[2] = parts[2].Substring(1);
                int? triVertex = ParsingUtilities.ParseIntNullable(parts[2]);
                if (!triVertex.HasValue || triVertex.Value < 1 || triVertex.Value > 3) return null;
                return new PositionAngleId(PositionAngleTypeEnum.Tri, address.Value, triVertex.Value);
            }

            return null;
        }
    }
}
