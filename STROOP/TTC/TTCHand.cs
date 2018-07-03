using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A hand is the long horizontal clock hand that rotates
      *  in a circle and that Mario is supposed to ride on to
      *  get to the other side of the course.
      *  
      *  A hand has a displacement (i.e. how much it should tick, which
      *  will be positive for CCW ticks and negative for CW ticks).
      *  When timer > max, the hand ticks and calculates a new max
      *  using RNG. Furthermore, if at this time the direction timer
      *  has decremented to zero, then the hand will calculate a new
      *  displacement (i.e. whether it should tick CW or CCW) as well as
      *  how long it should be until it has the chance to change
      *  direction again (i.e. what value its direction timer will be).
      */
    public class TtcHand : TtcObject
    {


        public readonly static int DISPLACEMENT_MAGNITUDE = 1092;
        public readonly static int INITIAL_MAX = 10;

        public int angle;
        public int max;
        public int targetAngle;
        public int displacement;
        public int directionTimer;
        public int timer;

        public TtcHand(int startingAngle)
        {
            angle = startingAngle;
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
                    {
                        displacement = DISPLACEMENT_MAGNITUDE;
                        directionTimer = (pollRNG() % 3) * 30 + 30; // = 30, 60, 90
                    }
                    else
                    {
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
