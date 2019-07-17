using STROOP.Forms;
using STROOP.Managers;
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
        /*
        static void push_mario_out_of_object(struct MarioState *m, struct Object *o, f32 padding)
        {
            f32 minDistance = o->hitboxRadius + m->marioObj->hitboxRadius + padding;

            f32 offsetX = m->pos[0] - o->oPosX;
            f32 offsetZ = m->pos[2] - o->oPosZ;
            f32 distance = sqrtf(offsetX * offsetX + offsetZ * offsetZ);

            if (distance < minDistance)
            {
                struct Surface *floor;
                s16 pushAngle;
                f32 newMarioX;
                f32 newMarioZ;

                if (distance == 0.0f)
                    pushAngle = m->faceAngle[1];
                else
                    pushAngle = atan2s(offsetZ, offsetX);

                newMarioX = o->oPosX + minDistance * sins(pushAngle);
                newMarioZ = o->oPosZ + minDistance * coss(pushAngle);

                resolve_wall_collisions(&newMarioX, &m->pos[1], &newMarioZ, 60.0f, 50.0f);

                find_floor(newMarioX, m->pos[1], newMarioZ, &floor);
                if (floor != NULL)
                {
                    //! Doesn't update mario's referenced floor (allows oob death when
                    // an object pushes you into a steep slope while in a ground action)
                    m->pos[0] = newMarioX;
                    m->pos[2] = newMarioZ;
                }
            }
        }
        */

        public static (float newMarioX, float newMarioZ) PushMarioOutOfObject(
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
                /*
                resolve_wall_collisions(&newMarioX, &m->pos[1], &newMarioZ, 60.0f, 50.0f);

                find_floor(newMarioX, m->pos[1], newMarioZ, &floor);
                if (floor != NULL)
                {
                    //! Doesn't update mario's referenced floor (allows oob death when
                    // an object pushes you into a steep slope while in a ground action)
                    m->pos[0] = newMarioX;
                    m->pos[2] = newMarioZ;
                }
                */
                return (newMarioX, newMarioZ);
            }
            return (marioX, marioZ);
        }
        
        
    }
}
