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
    public static class ObjectUtilities
    {
        public static uint? GetObjectRelativeAddress(uint absoluteAddress)
        {
            uint objRangeMinAddress = ObjectSlotsConfig.LinkStartAddress;
            uint objRangeMaxAddress =
                objRangeMinAddress + (uint)ObjectSlotsConfig.MaxSlots * ObjectConfig.StructSize;

            if (absoluteAddress < objRangeMinAddress ||
                absoluteAddress >= objRangeMaxAddress) return null;

            uint relativeAddress = (absoluteAddress - objRangeMinAddress) % ObjectConfig.StructSize;
            return relativeAddress;
        }
    }
}
