using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.TTC
{
    /** A pendulum is the pendulum that swings back and forth.
      *  
      *  A pendulum at rest will call RNG to determine how long
      *  it should wait for and how fast it should accelerates
      *  during the next swing. After it's waited the allotted time,
      *  it swings with that acceleration. Once it crosses strictly
      *  past the vertical (i.e. angle 0), the pendulum decelerates
      *  by that same acceleration until it comes to a stop.
      */
    public class TTCPendulum : TTCObject
    {


        public int accelerationDirection;
        public int angle;
        public int angularVelocity;
        public int accelerationMagnitude;
        public int waitingTimer;

        public TTCPendulum()
        {
            accelerationDirection = 0;
            angle = 6500;
            angularVelocity = 0;
            accelerationMagnitude = 0;
            waitingTimer = 0;
        }

        public override void update()
        {

            if (waitingTimer > 0)
            { //waiting
                waitingTimer--;
            }
            else
            { //swinging

                if (accelerationMagnitude == 0)
                { //give initial acceleration on start
                    accelerationMagnitude = 13;
                }

                if (angle > 0) accelerationDirection = -1;
                else if (angle < 0) accelerationDirection = 1;

                angularVelocity = angularVelocity + accelerationDirection * accelerationMagnitude;
                angle = angle + angularVelocity;

                if (angularVelocity == 0)
                { //reached peak of swing
                    accelerationMagnitude = (pollRNG() % 3 == 0) ? 42 : 13; // = 13, 42
                    if (pollRNG() % 2 == 0)
                    { //stop for some time
                        waitingTimer = (int)(pollRNG() / 65536.0 * 30 + 5); // = [5,35)
                    }
                }
            }

        }

        public override string ToString()
        {
            return id + OPENER + accelerationDirection + SEPARATOR +
                      angle + SEPARATOR +
                      angularVelocity + SEPARATOR +
                      accelerationMagnitude + SEPARATOR +
                      waitingTimer + CLOSER;
        }

    }

}
