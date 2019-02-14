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
    public class ObjectSnapshot
    {
        private readonly List<uint> uintValues;

        private static readonly List<uint> _primaryVariables = new List<uint>()
        {
            ObjectConfig.NextLinkOffset,
            ObjectConfig.PreviousLinkOffset,
            ObjectConfig.ProcessedNextLinkOffset,
            ObjectConfig.ProcessedPreviousLinkOffset,
        };

        private static readonly List<uint> _secondaryVariables = new List<uint>()
        {
            ObjectConfig.NextLinkOffset,
            ObjectConfig.PreviousLinkOffset,
            ObjectConfig.ProcessedNextLinkOffset,
            ObjectConfig.ProcessedPreviousLinkOffset,
            ObjectConfig.XOffset,
            ObjectConfig.YOffset,
            ObjectConfig.ZOffset,
            ObjectConfig.HomeXOffset,
            ObjectConfig.HomeYOffset,
            ObjectConfig.HomeZOffset,
            ObjectConfig.YawFacingOffsetUInt,
            ObjectConfig.PitchFacingOffsetUInt,
            ObjectConfig.RollFacingOffsetUInt,
            ObjectConfig.YawMovingOffsetUInt,
            ObjectConfig.PitchMovingOffsetUInt,
            ObjectConfig.RollMovingOffsetUInt,
        };

        public ObjectSnapshot(uint address)
        {
            uintValues = new List<uint>();
            for (int i = 0; i < ObjectConfig.StructSize; i += 4)
            {
                uint uintValue = Config.Stream.GetUInt32(address + (uint)i);
                uintValues.Add(uintValue);
            }
        }

        public void Apply(uint address, bool spareSecondary)
        {
            List<uint> toBeSpared = spareSecondary ? _secondaryVariables : _primaryVariables;

            for (int i = 0; i < ObjectConfig.StructSize; i += 4)
            {
                if (toBeSpared.Any(offset => offset == i)) continue;
                uint uintValue = uintValues[i / 4];
                Config.Stream.SetValue(uintValue, address + (uint)i);
            }
        }

        public void Apply(List<uint> addresses, bool spareSecondary)
        {
            foreach (uint address in addresses)
            {
                Apply(address, spareSecondary);
            }
        }
    }
}
