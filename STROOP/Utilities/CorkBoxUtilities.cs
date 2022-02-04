using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class CorkBoxUtilities
    {
        public static (float y, int numFrames) GetNumFrames(double x, double z, List<TriangleDataModel> wallTris)
        {
            (TriangleDataModel tri, float y) = TriangleUtilities.FindFloorAndY((float)x, 20_000, (float)z);
            CorkBox corkBox = new CorkBox((float)x, y, (float)z, wallTris);
            while (true)
            {
                corkBox.Update();
                if (corkBox.Dead || corkBox.InactivityTimer >= 902)
                {
                    return (y, corkBox.InactivityTimer);
                }
            }
        }
    }
}
