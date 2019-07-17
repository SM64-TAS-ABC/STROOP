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
    public static class ObjectDisplacementCalculator
    {
        public static (float newMarioX, float newMarioZ) HandleObjectDisplacement(
            float marioX, float marioZ, float marioRadius, short marioAngle,
            float objectX, float objectZ, float objectRadius, float padding)
        {
            float minDistance = objectRadius + marioRadius + padding;

            float offsetX = marioX - objectX;
            float offsetZ = marioZ - objectZ;
            float distance = (float)Math.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

            if (distance < minDistance)
            {
                short pushAngle;
                float newMarioX;
                float newMarioZ;

                if (distance == 0.0f)
                    pushAngle = marioAngle;
                else
                    pushAngle = (short)InGameTrigUtilities.InGameATan(offsetZ, offsetX);

                newMarioX = objectX + minDistance * InGameTrigUtilities.InGameSine(pushAngle);
                newMarioZ = objectZ + minDistance * InGameTrigUtilities.InGameCosine(pushAngle);

                return (newMarioX, newMarioZ);
            }
            return (marioX, marioZ);
        }
    }
}
