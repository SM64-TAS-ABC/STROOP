using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** An elevator is the yellow rectangle platform that moves up and down
     *  and will periodically stops and switches directions.
     *  
     *  An elevator moves up or down and will switch directions
     *  when it reaches its min height or max height. In addition,
     *  when the counter variable exceeds the max variable,
     *  the elevator will call RNG to determine whether its new
     *  direction (up or down) and how long until the next
     *  possible direction switch.
     */
    public class TtcElevator : TtcObject
    {

        public readonly int MIN_HEIGHT;
        public readonly int MAX_HEIGHT;

        public int height;
        public int verticalSpeed;
        public int direction;
        public int max;
        public int counter;

        public TtcElevator(int minHeightIn, int maxHeightIn)
        {
            MIN_HEIGHT = minHeightIn;
            MAX_HEIGHT = maxHeightIn;
            height = MIN_HEIGHT;
            verticalSpeed = 0;
            direction = 1;
            max = 0;
            counter = 0;
        }

        public override void update()
        {
            if (counter <= 4)
            {
                verticalSpeed = 0;
            }
            else
            {
                verticalSpeed = direction * 6;
            }

            height = height + verticalSpeed;

            if (counter > max)
            {
                direction = (pollRNG() <= 32766) ? -1 : 1; // = -1, 1
                max = (pollRNG() % 6) * 30 + 30; // = 30, 60, 90, 120, 150, 180
                counter = 0;
            }

            height = Math.Max(height, MIN_HEIGHT);
            height = Math.Min(height, MAX_HEIGHT);
            if (height == MIN_HEIGHT || height == MAX_HEIGHT)
            {
                direction *= -1;
            }
            counter++;
        }

        public override string ToString()
        {
            return id + OPENER + height + SEPARATOR +
                      verticalSpeed + SEPARATOR +
                      direction + SEPARATOR +
                      max + SEPARATOR +
                      counter + CLOSER;
        }

    }


}
