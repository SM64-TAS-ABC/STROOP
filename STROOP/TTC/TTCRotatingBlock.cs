using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** Rotating block is the cube that rotates around a
     *  horizontal axis (i.e. changes its pitch).
     *  
     *  When it completes a rotation, it calls RNG to determine
     *  how long it should wait until the next rotation.
     *  Once it has waited this long, it begins rotating
     *  and the process repeats.
     */
    public class TtcRotatingBlock : TtcObject
    {

        //the turning time for rotating blocks
        public static readonly int TURNING_TIME = 40;

        public int max;
        public int timer;

        public TtcRotatingBlock()
        {
            max = 0;
            timer = 0;
        }

        public override void update()
        {
            if (timer < max + TURNING_TIME)
            { //waiting
                timer++;
            }
            else
            { //done waiting
                max = (pollRNG() % 7) * 20 + 5; // = 5, 25, 45, 65, 85, 105, 125
                timer = 0;
                timer++;
            }
        }

        public override string ToString()
        {
            return id + OPENER + max + SEPARATOR +
                    timer + CLOSER;
        }

    }


}
