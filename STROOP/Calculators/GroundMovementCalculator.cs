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
    public static class GroundMovementCalculator
    {
        public static MarioState ApplyInput(MarioState marioState, Input input, int numQSteps = 4)
        {
            /*
            if (m->input & INPUT_UNKNOWN_5)
                return set_mario_action(m, ACT_HOLD_DECELERATING, 0);

            if (m->input & INPUT_Z_PRESSED)
                return drop_and_set_mario_action(m, ACT_CROUCH_SLIDE, 0);

                m->intendedMag *= 0.4f;

            update_walking_speed(m);

            switch (perform_ground_step(m))
            {
            case GROUND_STEP_LEFT_GROUND:
                set_mario_action(m, ACT_HOLD_FREEFALL, 0);
                break;

            case GROUND_STEP_HIT_WALL:
                if (m->forwardVel > 16.0f)
                    mario_set_forward_vel(m, 16.0f);
                break;
            }

            func_8026570C(m);

            if (0.4f * m->intendedMag - m->forwardVel > 10.0f)
                m->particleFlags |= PARTICLE_DUST;

            return FALSE;
            */
            return null;
        }
    }
}
