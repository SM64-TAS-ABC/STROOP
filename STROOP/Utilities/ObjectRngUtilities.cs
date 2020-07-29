using STROOP.Extensions;
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
    public static class ObjectRngUtilities
    {
        public static int? GetNumRngUsages(ObjectDataModel obj)
        {
            if (obj == null) return null;
            int? objIndex = ObjectUtilities.GetObjectIndex(obj.Address);
            if (!objIndex.HasValue) return null;
            uint memoryOffset = (uint)objIndex.Value * 4;
            return Config.Stream.GetInt32(0x803678A0 + memoryOffset);
        }

        public static string GetNumRngUsagesAsString(ObjectDataModel obj)
        {
            int? numUsages = GetNumRngUsages(obj);
            return numUsages?.ToString() ?? "";
        }

        public static Color GetColor(ObjectDataModel obj)
        {
            int? numRngUsages = GetNumRngUsages(obj);
            if (!numRngUsages.HasValue) return ObjectSlotsConfig.VacantSlotColor;
            int index = MoreMath.Clamp(numRngUsages.Value, 0, ObjectSlotsConfig.RngUsageColors.Count - 1);
            return ObjectSlotsConfig.RngUsageColors[index];
        }
    }
}
