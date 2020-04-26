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
    public class BobombState
    {
        public float X;
        public float Y;
        public float Z;
        public float XSpeed;
        public float YSpeed;
        public float ZSpeed;
        public float HSpeed;
        public ushort Yaw;
        public float HomeX;
        public float HomeY;
        public float HomeZ;

        public int Timer;

        public BobombState(
            float x,
            float y,
            float z,
            float xSpeed,
            float ySpeed,
            float zSpeed,
            float hSpeed,
            ushort yaw,
            float homeX,
            float homeY,
            float homeZ)
        {
            X = x;
            Y = y;
            Z = z;
            XSpeed = xSpeed;
            YSpeed = ySpeed;
            ZSpeed = zSpeed;
            HSpeed = hSpeed;
            Yaw = yaw;
            HomeX = homeX;
            HomeY = homeY;
            HomeZ = homeZ;

            Timer = 0;
        }

        public void bobomb_act_patrol()
        {
            HSpeed = 5.0f;
            object_step();
            obj_return_home_if_safe();
            Timer++;
        }

        public void object_step()
        {
            float objVelX = HSpeed * InGameTrigUtilities.InGameSine(Yaw);
            float objVelZ = HSpeed * InGameTrigUtilities.InGameCosine(Yaw);
            calc_new_obj_vel_and_pos_y(objVelX, objVelZ);
            obj_update_pos_vel_xz();
        }

        void calc_new_obj_vel_and_pos_y(float objVelX, float objVelZ) {
            float floor_nX = 0;
            float floor_nY = 1;
            float floor_nZ = 0;

            // Caps vertical speed with a "terminal velocity".
            YSpeed -= 2.5f;
            if (YSpeed > 75.0)
            {
                YSpeed = 75.0f;
            }
            if (YSpeed < -75.0)
            {
                YSpeed = -75.0f;
            }

            float floorY = Y;
            Y += YSpeed;

            //Snap the object up to the floor.
            if (Y < floorY)
            {
                Y = floorY;

                // Bounces an object if the ground is hit fast enough.
                if (YSpeed < -17.5)
                {
                    YSpeed = -(YSpeed / 2);
                }
                else
                {
                    YSpeed = 0;
                }
            }

            //! (Obj Position Crash) If you got an object with height past 2^31, the game would crash.
            if ((int)Y >= (int)floorY && (int)Y < (int)floorY + 37)
            {
                // Adds horizontal component of gravity for horizontal speed.
                objVelX += floor_nX * (floor_nX * floor_nX + floor_nZ * floor_nZ)
                           / (floor_nX * floor_nX + floor_nY * floor_nY + floor_nZ * floor_nZ) * 2.5f
                           * 2;
                objVelZ += floor_nZ * (floor_nX * floor_nX + floor_nZ * floor_nZ)
                           / (floor_nX * floor_nX + floor_nY * floor_nY + floor_nZ * floor_nZ) * 2.5f
                           * 2;

                if (objVelX < 0.000001 && objVelX > -0.000001)
                {
                    objVelX = 0;
                }
                if (objVelZ < 0.000001 && objVelZ > -0.000001)
                {
                    objVelZ = 0;
                }

                if (objVelX != 0 || objVelZ != 0)
                {
                    Yaw = InGameTrigUtilities.InGameATan(objVelZ, objVelX);
                }

                HSpeed = (float)Math.Sqrt(objVelX * objVelX + objVelZ * objVelZ) * 0.8f;
            }
        }

        public void obj_update_pos_vel_xz()
        {
            float xVel = HSpeed * InGameTrigUtilities.InGameSine(Yaw);
            float zVel = HSpeed * InGameTrigUtilities.InGameCosine(Yaw);
            X += xVel;
            Z += zVel;
        }

        public void obj_return_home_if_safe() {
            float homeDistX = HomeX - X;
            float homeDistZ = HomeZ - Z;
            short angleTowardsHome = (short)InGameTrigUtilities.InGameATan(homeDistZ, homeDistX);
            Yaw = (ushort)approach_s16_symmetric((short)Yaw, angleTowardsHome, 320);
        }

        public short approach_s16_symmetric(short value, short target, short increment)
        {
            short dist = MoreMath.NormalizeAngleShort(target - value);
            if (dist >= 0)
            {
                if (dist > increment)
                {
                    value += increment;
                }
                else
                {
                    value = target;
                }
            }
            else
            {
                if (dist < -increment)
                {
                    value -= increment;
                }
                else
                {
                    value = target;
                }
            }
            return value;
        }

        public override string ToString()
        {
            return string.Format(
                "[{13}] pos=({0},{1},{2}) speed=({3},{4},{5}) hSpeed={6} yaw={7} home=({8},{9},{10}) distFromHome={11} angleFromHome={12}",
                (double)X, (double)Y, (double)Z,
                (double)XSpeed, (double)YSpeed, (double)ZSpeed,
                (double)HSpeed, Yaw,
                (double)HomeX, (double)HomeY, (double)HomeZ,
                MoreMath.GetDistanceBetween(HomeX, HomeZ, X, Z),
                MoreMath.AngleTo_AngleUnitsRounded(HomeX, HomeZ, X, Z), Timer);
        }
    }
}
