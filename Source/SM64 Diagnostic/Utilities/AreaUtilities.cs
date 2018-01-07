using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public static class AreaUtilities
    {
        public static int? GetAreaIndex(uint areaAddress)
        {
            for (int i = 0; i < 8; i++)
            {
                uint address = (uint)(Config.Area.AreaStartAddress + i * Config.Area.AreaStructSize);
                if (address == areaAddress) return i;
            }
            return null;
        }

        public static string GetAreaIndexString(uint areaAddress)
        {
            return GetAreaIndex(areaAddress)?.ToString() ?? "Unrecognized";
        }

        public static uint GetAreaAddress(int index)
        {
            return (uint)(Config.Area.AreaStartAddress + index * Config.Area.AreaStructSize);
        }

        public static string GetAreaDescription(int terrainType)
        {
            switch (terrainType)
            {
                case 0:
                    return "Grassy";
                case 1:
                    return "Normal";
                case 2:
                    return "Cold";
                case 3:
                    return "Sandy";
                case 4:
                    return "Spooky";
                case 5:
                    return "Aquatic";
                case 6:
                    return "Slide";
                default:
                    return "Unrecognized";
            }
        }

    }
}
