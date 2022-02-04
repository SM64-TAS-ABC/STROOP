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

            float floorY;
            float waterY = FLOOR_LOWER_LIMIT_MISC;

            float objVelX = HSpeed * InGameTrigUtilities.InGameSine(Yaw);
            float objVelZ = HSpeed * InGameTrigUtilities.InGameCosine(Yaw);

            short collisionFlags = 0;

            // Find any wall collisions, receive the push, and set the flag.
            if (obj_find_wall(objX + objVelX, objY, objZ + objVelZ, objVelX, objVelZ) == 0)
            {
                Broken = true;
            }

            //floorY = find_floor(objX + objVelX, objY, objZ + objVelZ, &sObjFloor);
            //if (turn_obj_away_from_steep_floor(sObjFloor, floorY, objVelX, objVelZ) == 1)
            //{
            //    waterY = find_water_level(objX + objVelX, objZ + objVelZ);
            //    if (waterY > objY)
            //    {
            //        calc_new_obj_vel_and_pos_y_underwater(sObjFloor, floorY, objVelX, objVelZ, waterY);
            //        collisionFlags += OBJ_COL_FLAG_UNDERWATER;
            //    }
            //    else
            //    {
            //        calc_new_obj_vel_and_pos_y(sObjFloor, floorY, objVelX, objVelZ);
            //    }
            //}
            //else
            //{
            //    // Treat any awkward floors similar to a wall.
            //    collisionFlags +=
            //        ((collisionFlags & OBJ_COL_FLAG_HIT_WALL) ^ OBJ_COL_FLAG_HIT_WALL);
            //}

            //obj_update_pos_vel_xz();
            //if ((s32)o->oPosY == (s32)floorY)
            //{
            //    collisionFlags += OBJ_COL_FLAG_GROUNDED;
            //}

            //if ((s32)o->oVelY == 0)
            //{
            //    collisionFlags += OBJ_COL_FLAG_NO_Y_VEL;
            //}

            //// Generate a splash if in water.
            //obj_splash((s32)waterY, (s32)o->oPosY);
            
            return collisionFlags;
        }

        int obj_find_wall(float objNewX, float objY, float objNewZ, float objVelX, float objVelZ)
        {
            int numCollisions = WallDisplacementCalculator.GetNumWallCollisions(
                objNewX, objY, objNewZ, WallTris, 60, 50);
            return numCollisions > 0 ? 0 : 1;
        }
    }
}
