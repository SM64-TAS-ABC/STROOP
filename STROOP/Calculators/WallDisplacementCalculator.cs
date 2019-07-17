using STROOP.Forms;
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
    public static class WallDisplacementCalculator
    {
        public static (float newMarioX, float newMarioZ) HandleWallDisplacement(
            float marioX, float marioY, float marioZ, TriangleDataModel surf, float radius, float offsetY)
        {
            return HandleWallDisplacement(marioX, marioY, marioZ, new List<TriangleDataModel>() { surf }, radius, offsetY);
        }

        public static (float newMarioX, float newMarioZ) HandleWallDisplacement(
            float marioX, float marioY, float marioZ, List<TriangleDataModel> surfs, float radius, float offsetY)
        {
            float offset;
            float x = marioX;
            float y = marioY + offsetY;
            float z = marioZ;
            float px, pz;
            float w1, w2, w3;
            float y1, y2, y3;

            // Max collision radius = 200
            if (radius > 200.0f) radius = 200.0f;

            foreach (TriangleDataModel surf in surfs)
            {
                if (y < surf.YMinMinus5 || y > surf.YMaxPlus5)
                    continue;

                offset = surf.NormX * x + surf.NormY * y + surf.NormZ * z + surf.NormOffset;

                if (offset < -radius || offset > radius)
                    continue;

                px = x;
                pz = z;

                if (surf.XProjection)
                {
                    w1 = -surf.Z1;
                    w2 = -surf.Z2;
                    w3 = -surf.Z3;
                    y1 = surf.Y1;
                    y2 = surf.Y2;
                    y3 = surf.Y3;

                    if (surf.NormX > 0.0f)
                    {
                        if ((y1 - y) * (w2 - w1) - (w1 - -pz) * (y2 - y1) > 0.0f) continue;
                        if ((y2 - y) * (w3 - w2) - (w2 - -pz) * (y3 - y2) > 0.0f) continue;
                        if ((y3 - y) * (w1 - w3) - (w3 - -pz) * (y1 - y3) > 0.0f) continue;
                    }
                    else
                    {
                        if ((y1 - y) * (w2 - w1) - (w1 - -pz) * (y2 - y1) < 0.0f) continue;
                        if ((y2 - y) * (w3 - w2) - (w2 - -pz) * (y3 - y2) < 0.0f) continue;
                        if ((y3 - y) * (w1 - w3) - (w3 - -pz) * (y1 - y3) < 0.0f) continue;
                    }
                }
                else
                {
                    w1 = surf.X1;
                    w2 = surf.X2;
                    w3 = surf.X3;
                    y1 = surf.Y1;
                    y2 = surf.Y2;
                    y3 = surf.Y3;

                    if (surf.NormZ > 0.0f)
                    {
                        if ((y1 - y) * (w2 - w1) - (w1 - px) * (y2 - y1) > 0.0f) continue;
                        if ((y2 - y) * (w3 - w2) - (w2 - px) * (y3 - y2) > 0.0f) continue;
                        if ((y3 - y) * (w1 - w3) - (w3 - px) * (y1 - y3) > 0.0f) continue;
                    }
                    else
                    {
                        if ((y1 - y) * (w2 - w1) - (w1 - px) * (y2 - y1) < 0.0f) continue;
                        if ((y2 - y) * (w3 - w2) - (w2 - px) * (y3 - y2) < 0.0f) continue;
                        if ((y3 - y) * (w1 - w3) - (w3 - px) * (y1 - y3) < 0.0f) continue;
                    }
                }

                marioX += surf.NormX * (radius - offset);
                marioZ += surf.NormZ * (radius - offset);
            }

            return (marioX, marioZ);
        }
    }
}
