using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A wheel is the little cog-like structure that is found
     *  slightly under the floor in various parts of TTC.
     *  They appear in pairs. They function exactly as the hands do,
     *  except that their ticks are greater in magnitude.
     */
    public class TtcWheel : TtcObject
    {

        public readonly static int DISPLACEMENT_MAGNITUDE = 3276;
        public readonly static int INITIAL_MAX = 5;

        public int angle;
        public int max;
        public int targetAngle;
        public int displacement;
        public int directionTimer;
        public int timer;

        public TtcWheel()
        {
            angle = 0;
            max = 0;
            targetAngle = 0;
            displacement = 0;
            directionTimer = 0;
            timer = 0;
        }

        public override void update()
        {

            if (max == 0)
            { //course just started
                max = INITIAL_MAX;
                displacement = -1 * DISPLACEMENT_MAGNITUDE;
            }

            angle = this.moveAngleTowards(angle, targetAngle, 200);

            directionTimer = Math.Max(0, directionTimer - 1);

            if (timer <= max)
            { //waiting
                timer++;
            }
            else if (angle == targetAngle)
            { //done waiting and reached target
                targetAngle = targetAngle + displacement;
                targetAngle = normalize(targetAngle);

                if (directionTimer == 0)
                { //time to maybe switch directions
                    if (pollRNG() % 4 == 0)
                    { //time to move CCW
                        displacement = DISPLACEMENT_MAGNITUDE;
                        directionTimer = (pollRNG() % 3) * 30 + 30; // = 30, 60, 90
                    }
                    else
                    { //time to move CW
                        displacement = -1 * DISPLACEMENT_MAGNITUDE;
                        directionTimer = (pollRNG() % 4) * 60 + 90; // = 90, 150, 210, 270
                    }
                }

                max = (pollRNG() % 3) * 20 + 10; // = 10, 30, 50
                timer = 0;
                timer++;
            }
            else
            { //timer high enough, but not at target angle (will only happen at level start)
                timer++;
            }
        }

        public override string ToString()
        {
            return id + OPENER + angle + SEPARATOR +
                          max + SEPARATOR +
                          targetAngle + SEPARATOR +
                          displacement + SEPARATOR +
                          directionTimer + SEPARATOR +
                          timer + CLOSER;
        }

    }

}
