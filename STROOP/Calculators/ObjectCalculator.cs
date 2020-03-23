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
    public static class ObjectCalculator
    {
        // push_mario_out_of_object
        public static (float newMarioX, float newMarioZ) GetHardObjectDisplacement(
            float marioX, float marioZ, float marioRadius, ushort marioAngle,
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
                    pushAngle = (short)marioAngle;
                else
                    pushAngle = (short)InGameTrigUtilities.InGameATan(offsetZ, offsetX);

                newMarioX = objectX + minDistance * InGameTrigUtilities.InGameSine(pushAngle);
                newMarioZ = objectZ + minDistance * InGameTrigUtilities.InGameCosine(pushAngle);

                return (newMarioX, newMarioZ);
            }
            return (marioX, marioZ);
        }

        // cur_obj_push_mario_away
        public static (float newMarioX, float newMarioZ) GetSoftObjectDisplacement(
            float marioX, float marioZ, float objX, float objZ, float radius)
        {
            float marioRelX = marioX - objX;
            float marioRelZ = marioZ - objZ;
            float marioDist = (float)Math.Sqrt(marioRelX * marioRelX + marioRelZ * marioRelZ);

            if (marioDist < radius)
            {
                marioX += (radius - marioDist) / radius * marioRelX;
                marioZ += (radius - marioDist) / radius * marioRelZ;
            }

            return (marioX, marioZ);
        }

        // cur_obj_set_pos_relative
        public static (float objectX, float objectY, float objectZ) GetRelativePosition(
            float marioX, float marioY, float marioZ, ushort marioAngle,
            float dleft, float dy, float dforward)
        {
            float facingZ = InGameTrigUtilities.InGameCosine(marioAngle);
            float facingX = InGameTrigUtilities.InGameSine(marioAngle);

            float dz = dforward * facingZ - dleft * facingX;
            float dx = dforward * facingX + dleft * facingZ;

            return (marioX + dx, marioY + dy, marioZ + dz);
        }
    }
}
