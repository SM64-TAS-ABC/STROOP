using STROOP.Managers;
using STROOP.Structs.Configurations;
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
        public float X;
        public float Y;
        public float Z;
        public float XSpeed;
        public float YSpeed;
        public float ZSpeed;
        public float HSpeed;
        public float Yaw;
        public int InactivityTimer;
        public bool Broken;

        public CorkBox(float x, float y, float z)
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
        }

        public void Update()
        {
            small_breakable_box_act_move();
            breakable_box_small_released_loop();
        }

        private void small_breakable_box_act_move()
        {
            //short collisionFlags = object_step();

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
    }
}
