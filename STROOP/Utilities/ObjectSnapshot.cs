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
                if (i == ObjectConfig.NextLinkOffset ||
                    i == ObjectConfig.PreviousLinkOffset ||
                    i == ObjectConfig.ProcessedNextLinkOffset ||
                    i == ObjectConfig.ProcessedPreviousLinkOffset) continue;

                uint uintValue = uintValues[i / 4];
                Config.Stream.SetValue(uintValue, address + (uint)i);
            }
        }
    }
}
