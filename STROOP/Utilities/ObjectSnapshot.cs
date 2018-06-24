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

        private static readonly List<uint> _doNotEditList = new List<uint>()
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

        public void Apply(uint address)
        {
            for (int i = 0; i < ObjectConfig.StructSize; i += 4)
            {
                if (_doNotEditList.Any(offset => offset == i)) continue;
                uint uintValue = uintValues[i / 4];
                Config.Stream.SetValue(uintValue, address + (uint)i);
            }
        }
    }
}
