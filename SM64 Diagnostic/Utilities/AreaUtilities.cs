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
                uint address = (uint)(AreaConfig.AreaStartAddress + i * AreaConfig.AreaStructSize);
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
            return (uint)(AreaConfig.AreaStartAddress + index * AreaConfig.AreaStructSize);
        }

        public static string GetTerrainDescription(short terrainType)
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

        public static short? GetTerrainType(string terrainDescription)
        {
            terrainDescription = terrainDescription.ToLower().Trim();
            switch (terrainDescription)
            {
                case "grassy":
                    return 0;
                case "normal":
                    return 1;
                case "cold":
                    return 2;
                case "sandy":
                    return 3;
                case "spooky":
                    return 4;
                case "aquatic":
                    return 5;
                case "slide":
                    return 6;
                default:
                    return null;
            }
        }

    }
}
