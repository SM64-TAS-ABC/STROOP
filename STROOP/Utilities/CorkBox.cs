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
    public class CorkBox
    {
        private static int FLOOR_LOWER_LIMIT_MISC = -10_000;
        private static float Buoyancy = 1.4f;
        private static float Gravity = 2.5f;
        private static float Friction = 0.99f;

        private static short OBJ_COL_FLAG_GROUNDED = (1 << 0);
        private static short OBJ_COL_FLAG_HIT_WALL = (1 << 1);
        private static short OBJ_COL_FLAG_UNDERWATER = (1 << 2);
        private static short OBJ_COL_FLAG_NO_Y_VEL = (1 << 3);
        private static short OBJ_COL_FLAGS_LANDED = (short)(OBJ_COL_FLAG_GROUNDED | OBJ_COL_FLAG_NO_Y_VEL);

        public float X;
        public float Y;
        public float Z;
        public float XSpeed;
        public float YSpeed;
        public float ZSpeed;
        public float HSpeed;
        public ushort Yaw;
        public int InactivityTimer;
        public bool Broken;

        public TriangleDataModel StaticFloor;
        public List<TriangleDataModel> WallTris;

        public CorkBox(float x, float y, float z, List<TriangleDataModel> wallTris)
        {
            X = x;
            Y = y;
            Z = z;
            XSpeed = 0;
            YSpeed = 0;
            ZSpeed = 0;
            HSpeed = 0;
            Yaw = 0;
            InactivityTimer = 0;
            Broken = false;

            StaticFloor = null;
            WallTris = wallTris;
        }

        public void Update()
        {
            small_breakable_box_act_move();
            breakable_box_small_released_loop();
        }

        private void small_breakable_box_act_move()
        {
            short collisionFlags = object_step();

            //obj_attack_collided_from_other_object(o);

            //if (collisionFlags == OBJ_COL_FLAG_GROUNDED)
            //{
            //    cur_obj_play_sound_2(SOUND_GENERAL_BOX_LANDING_2);
            //}

            //if (collisionFlags & OBJ_COL_FLAG_GROUNDED)
            //{
            //    if (o->oForwardVel > 20.0f)
            //    {
            //        cur_obj_play_sound_2(SOUND_ENV_SLIDING);
            //        small_breakable_box_spawn_dust();
            //    }
            //}

            //if (collisionFlags & OBJ_COL_FLAG_HIT_WALL)
            //{
            //    spawn_mist_particles();
            //    spawn_triangle_break_particles(20, MODEL_DIRT_ANIMATION, 0.7f, 3);
            //    obj_spawn_yellow_coins(o, 3);
            //    create_sound_spawner(SOUND_GENERAL_BREAK_BOX);
            //    o->activeFlags = ACTIVE_FLAG_DEACTIVATED;
            //}

            //obj_check_floor_death(collisionFlags, sObjFloor);
        }

        private void breakable_box_small_released_loop()
        {
            InactivityTimer++;
        }

        private short object_step()
        {
            float objX = X;
            float objY = Y;
            float objZ = Z;

            float waterY = FLOOR_LOWER_LIMIT_MISC;

            float objVelX = HSpeed * InGameTrigUtilities.InGameSine(Yaw);
            float objVelZ = HSpeed * InGameTrigUtilities.InGameCosine(Yaw);

            short collisionFlags = 0;

            if (obj_find_wall(objX + objVelX, objY, objZ + objVelZ, objVelX, objVelZ) == 0)
            {
                Broken = true;
            }

            (TriangleDataModel staticFloor, float floorY) = TriangleUtilities.FindFloorAndY(objX + objVelX, objY, objZ + objVelZ);
            StaticFloor = staticFloor;

            if (turn_obj_away_from_steep_floor(StaticFloor, floorY, objVelX, objVelZ) == 1)
            {
                waterY = WaterUtilities.GetWaterAtPos(objX + objVelX, objZ + objVelZ);
                if (waterY > objY)
                {
                    calc_new_obj_vel_and_pos_y_underwater(StaticFloor, floorY, objVelX, objVelZ, waterY);
                    collisionFlags += OBJ_COL_FLAG_UNDERWATER;
                }
                else
                {
                    calc_new_obj_vel_and_pos_y(StaticFloor, floorY, objVelX, objVelZ);
                }
            }
            else
            {
                collisionFlags = (short)(collisionFlags + ((collisionFlags & OBJ_COL_FLAG_HIT_WALL) ^ OBJ_COL_FLAG_HIT_WALL));
            }

            obj_update_pos_vel_xz();
            if ((int)Y == (int)floorY)
            {
                collisionFlags += OBJ_COL_FLAG_GROUNDED;
            }

            if ((int)YSpeed == 0)
            {
                collisionFlags += OBJ_COL_FLAG_NO_Y_VEL;
            }

            return collisionFlags;
        }

        int obj_find_wall(float objNewX, float objY, float objNewZ, float objVelX, float objVelZ)
        {
            int numCollisions = WallDisplacementCalculator.GetNumWallCollisions(
                objNewX, objY, objNewZ, WallTris, 60, 50);
            return numCollisions > 0 ? 0 : 1;
        }

        int turn_obj_away_from_steep_floor(TriangleDataModel objFloor, float floorY, float objVelX, float objVelZ)
        {
            float floor_nX, floor_nY, floor_nZ, objVelXCopy, objVelZCopy, objYawX, objYawZ;

            if (objFloor == null) {
                Yaw = MoreMath.NormalizeAngleUshort(Yaw + 32767);
                return 0;
            }

            floor_nX = objFloor.NormX;
            floor_nY = objFloor.NormY;
            floor_nZ = objFloor.NormZ;

            if (floor_nY < 0.5 && floorY > Y)
            {
                objVelXCopy = objVelX;
                objVelZCopy = objVelZ;
                turn_obj_away_from_surface(
                    objVelXCopy, objVelZCopy, floor_nX, floor_nY, floor_nZ, out objYawX, out objYawZ);
                Yaw = InGameTrigUtilities.InGameATan(objYawZ, objYawX);
                return 0;
            }

            return 1;
        }

        void turn_obj_away_from_surface(
            float velX, float velZ, float nX, float nY, float nZ, out float objYawX, out float objYawZ)
        {
            objYawX = (nZ * nZ - nX * nX) * velX / (nX * nX + nZ * nZ)
                       - 2 * velZ * (nX * nZ) / (nX * nX + nZ * nZ);

            objYawZ = (nX * nX - nZ * nZ) * velZ / (nX * nX + nZ * nZ)
                       - 2 * velX * (nX * nZ) / (nX * nX + nZ * nZ);
        }

        void calc_new_obj_vel_and_pos_y_underwater(
            TriangleDataModel objFloor, float floorY, float objVelX, float objVelZ, float waterY)
        {
            float floor_nX = objFloor.NormX;
            float floor_nY = objFloor.NormY;
            float floor_nZ = objFloor.NormZ;

            float netYAccel = (1.0f - Buoyancy) * (-1.0f * Gravity);
            YSpeed -= netYAccel;

            if (YSpeed > 75.0)
            {
                YSpeed = 75.0f;
            }
            if (YSpeed < -75.0)
            {
                YSpeed = -75.0f;
            }

            Y += YSpeed;

            if (Y < floorY) {
                Y = floorY;

                if (YSpeed < -17.5)
                {
                    YSpeed = -(YSpeed / 2);
                }
                else
                {
                    YSpeed = 0;
                }
            }

            if (HSpeed > 12.5 && (waterY + 30.0f) > Y && (waterY - 30.0f) < Y) {
                YSpeed = -YSpeed;
            }

            if ((int) Y >= (int) floorY && (int) Y < (int) floorY + 37)
            {
                objVelX += floor_nX * (floor_nX * floor_nX + floor_nZ * floor_nZ)
                           / (floor_nX * floor_nX + floor_nY * floor_nY + floor_nZ * floor_nZ) * netYAccel * 2;
                objVelZ += floor_nZ * (floor_nX * floor_nX + floor_nZ * floor_nZ)
                           / (floor_nX * floor_nX + floor_nY * floor_nY + floor_nZ * floor_nZ) * netYAccel * 2;
            }

            if (objVelX < 0.000001 && objVelX > -0.000001)
            {
                objVelX = 0;
            }
            if (objVelZ < 0.000001 && objVelZ > -0.000001)
            {
                objVelZ = 0;
            }

            if (YSpeed < 0.000001 && YSpeed > -0.000001)
            {
                YSpeed = 0;
            }

            if (objVelX != 0 || objVelZ != 0)
            {
                Yaw = InGameTrigUtilities.InGameATan(objVelZ, objVelX);
            }

            HSpeed = (float)Math.Sqrt(objVelX * objVelX + objVelZ * objVelZ) * 0.8f;
            YSpeed = (float)(YSpeed * 0.8);
        }

        void calc_new_obj_vel_and_pos_y(TriangleDataModel objFloor, float objFloorY, float objVelX, float objVelZ)
        {
            float floor_nX = objFloor.NormX;
            float floor_nY = objFloor.NormY;
            float floor_nZ = objFloor.NormZ;
            float objFriction;

            YSpeed -= Gravity;
            if (YSpeed > 75.0)
            {
                YSpeed = 75.0f;
            }
            if (YSpeed < -75.0)
            {
                YSpeed = -75.0f;
            }

            Y += YSpeed;

            if (Y < objFloorY)
            {
                Y = objFloorY;

                if (YSpeed < -17.5)
                {
                    YSpeed = -(YSpeed / 2);
                }
                else
                {
                    YSpeed = 0;
                }
            }

            if ((int) Y >= (int) objFloorY && (int) Y < (int) objFloorY + 37)
            {
                objVelX += floor_nX * (floor_nX * floor_nX + floor_nZ * floor_nZ)
                           / (floor_nX * floor_nX + floor_nY * floor_nY + floor_nZ * floor_nZ) * Gravity
                           * 2;
                objVelZ += floor_nZ * (floor_nX * floor_nX + floor_nZ * floor_nZ)
                           / (floor_nX * floor_nX + floor_nY * floor_nY + floor_nZ * floor_nZ) * Gravity
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

                calc_obj_friction(out objFriction, floor_nY);
                HSpeed = (float)Math.Sqrt(objVelX * objVelX + objVelZ * objVelZ) * objFriction;
            }
        }

        void calc_obj_friction(out float objFriction, float floor_nY)
        {
            if (floor_nY < 0.2 && Friction < 0.9999)
            {
                objFriction = 0;
            }
            else
            {
                objFriction = Friction;
            }
        }

        void obj_update_pos_vel_xz()
        {
            float xVel = HSpeed * InGameTrigUtilities.InGameSine(Yaw);
            float zVel = HSpeed * InGameTrigUtilities.InGameCosine(Yaw);

            X += xVel;
            Z += zVel;
        }
    }
}
