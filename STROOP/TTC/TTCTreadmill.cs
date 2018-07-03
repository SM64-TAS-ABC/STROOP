using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A treadmill is the yellow treadmill that periodically
     *  reverses directions. Although there are 7 treadmills
     *  in TTC, only the first one actually updates meaningfully
     *  using RNG, and the others simply copy what this
     *  first one is doing.
     *  
     *  When a treadmill comes to a stop, it calls RNG to determine
     *  whether it should move forwards or backwards and also
     *  how long it should move in that direction. Then the treadmill
     *  accelerates to 50 speed in that direction. Once it's moved
     *  in that direction for the allotted time, it decelerates to
     *  a stop, and the process repeats.
     */
    public class TtcTreadmill : TtcObject
    {

        //whether this treadmill is the first treadmill
        public readonly bool isFirstTreadmill;
	
	    public int currentSpeed;
        public int targetSpeed;
        public int max;
        public int counter;

        public TtcTreadmill(TtcRng rng, bool isFirstTreadmillIn) : base(rng)
        {
            isFirstTreadmill = isFirstTreadmillIn;
            currentSpeed = 0;
            targetSpeed = 0;
            max = 0;
            counter = 0;
        }

        public override void update()
        {
            if (!isFirstTreadmill)
            { //if not first treadmill, do nothing
                counter++;
                return;
            }

            if (counter <= max)
            { //still/accelerate/move
                if (counter <= 5)
                { //be still
                    counter++;
                }
                else
                { //accelerate/move
                    currentSpeed = moveNumberTowards(currentSpeed, targetSpeed, 10);
                    counter++;
                }
            }
            else
            { //slow down
                currentSpeed = moveNumberTowards(currentSpeed, 0, 10);
                if (currentSpeed == 0)
                { //came to a stop
                    max = (pollRNG() % 7) * 20 + 10; // = 10, 30, 50, 70, 90, 110, 130
                    targetSpeed = (pollRNG() <= 32766) ? -50 : 50; // = -50, 50
                    counter = 0;
                }
                counter++;
            }
        }

        public override string ToString()
        {
            return id + OPENER + currentSpeed + SEPARATOR +
                      targetSpeed + SEPARATOR +
                      max + SEPARATOR +
                      counter + CLOSER;
        }

    }

}
