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

        private static short SURFACE_BURNING = 0x0001;
        private static short SURFACE_DEATH_PLANE = 0x000A;

        public float X;
        public float Y;
        public float Z;
        public float XSpeed;
        public float YSpeed;
        public float ZSpeed;
        public float HSpeed;
        public ushort Yaw;
        public int InactivityTimer;
        public bool Dead;

        public TriangleDataModel StaticFloor;
        public CellSnapshot CellSnapshot;

        public CorkBox(float x, float y, float z, CellSnapshot cellSnapshot)
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
            Dead = false;

            StaticFloor = null;
            CellSnapshot = cellSnapshot;
        }

        public override string ToString()
        {
            return string.Format(
                "pos=({0},{1},{2}) spd=({3},{4},{5},{6}) yaw={7} itimer={8} dead={9}",
                (double)X, (double)Y, (double)Z,
                (double)XSpeed, (double)YSpeed, (double)ZSpeed, (double)HSpeed,
                Yaw, InactivityTimer, Dead);
        }

        public void Update()
        {
            small_breakable_box_act_move();
            breakable_box_small_released_loop();
        }

        private void small_breakable_box_act_move()
        {
            short collisionFlags = object_step();

            if ((collisionFlags & OBJ_COL_FLAG_HIT_WALL) != 0)
            {
                Dead = true;
            }

            obj_check_floor_death(collisionFlags, StaticFloor);
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
                Dead = true;
            }

            (TriangleDataModel staticFloor, float floorY) = CellSnapshot.FindFloorAndY(objX + objVelX, objY, objZ + objVelZ);
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
            List<TriangleDataModel> wallTris = CellSnapshot.GetTrianglesAtPosition(objNewX, objNewZ, true, TriangleClassification.Wall);
            int numCollisions = WallDisplacementCalculator.GetNumWallCollisions(
                objNewX, objY, objNewZ, wallTris, 60, 50);
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

        void obj_check_floor_death(short collisionFlags, TriangleDataModel floor)
        {
            if (floor == null)
            {
                return;
            }

            if ((collisionFlags & OBJ_COL_FLAG_GROUNDED) == OBJ_COL_FLAG_GROUNDED)
            {
                switch (floor.SurfaceType)
                {
                    case 0x0001:
                        Dead = true;
                        break;
                    case 0x000A:
                        Dead = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
