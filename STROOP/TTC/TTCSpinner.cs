using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A spinner is the rectangle platform that spins about
      *  a horizontal axis (i.e. changes its pitch).
      *  They are mostly found near the red coins.
      *  
      *  When a spinner completes a spin, it calls
      *  RNG to determine which direction it should rotate next
      *  as well as how long the rotation should be.
      *  Then for 5 frames, the spinner remains still.
      *  Then for max-5 frames, the spinner spins in its
      *  intended direction.
      *  Then for 1 frame, the spinner spins counterclockwise.
      */
    public class TtcSpinner : TtcObject
    {

        public int angle;
        public int direction; //1 = CCW, -1 = CW
        public int max;
        public int counter;

        public TtcSpinner()
        {
            angle = 0;
            direction = 0;
            max = 0;
            counter = 0;
        }

        public override void update()
        {

            if (counter <= max)
            { //spin normal
                if (counter <= 5)
                { //don't spin
                    counter++;
                }
                else
                { //spin
                    angle += direction * 200;
                    angle = normalize(angle);
                    counter++;
                }
            }
            else
            { //start a new spin
              //do a CCW spin
                angle += 200;
                angle = normalize(angle);

                //calculate new spin
                direction = (pollRNG() <= 32766) ? -1 : 1; // = -1, 1
                max = (pollRNG() % 4) * 30 + 30; // = 30, 60, 90, 120
                counter = 0;
                counter++;
            }

        }

        public override string ToString()
        {
            return id + OPENER + angle + SEPARATOR +
                    direction + SEPARATOR +
                    max + SEPARATOR +
                    counter + CLOSER;
        }

    }


}
