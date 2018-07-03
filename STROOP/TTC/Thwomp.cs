using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.TTC
{
    /** A thwomp is the blue cube-like enemy that moves up and down
     *  in an attempt to squish Mario. There is only one Thwomp
     *  in TTC, near the very top of the clock.
     *  
     *  A thwomp moves up, waits, moves down, waits, then repeats.
     *  It calls RNG when it reaches the top to determine how long
     *  it should wait for, and it also does this at the bottom.
     */
    public class Thwomp : RNGObject
    {

        public readonly static int MIN_HEIGHT = 6192;
        public readonly static int MAX_HEIGHT = 6607;
        public readonly static int RISING_SPEED = 10;

        public int height;
        public int verticalSpeed;
        public int max;
        public int state; //0 = going up, 1 = at top, 2 = going down, 3/4 = at bottom
        public int counter;

        public Thwomp()
        {
            height = MIN_HEIGHT;
            verticalSpeed = 0;
            max = 0;
            state = 0;
            counter = 0;
        }

        public override void update()
        {
            if (state == 0)
            { //going up
                height = Math.Min(MAX_HEIGHT, height + RISING_SPEED);
                counter++;
                if (height == MAX_HEIGHT)
                { //reached top
                    state = 1;
                    counter = 0;
                }
            }
            else if (state == 1)
            { //at top
                if (counter == 0)
                { //just reached top
                    max = (int)(pollRNG() / 65536.0 * 30 + 10); // = [10,40)
                }
                if (counter <= max)
                { //waiting
                    counter++;
                }
                else
                { //done waiting
                    state = 2;
                    counter = 0;
                }
            }
            else if (state == 2)
            { //going down
                verticalSpeed -= 4;
                height = Math.Max(MIN_HEIGHT, height + verticalSpeed);
                counter++;
                if (height == MIN_HEIGHT)
                { //reached bottom
                    verticalSpeed = 0;
                    state = 3;
                    counter = 0;
                }
            }
            else if (state == 3)
            { //at bottom (1/2)
                if (counter < 10)
                { //waiting
                    counter++;
                }
                else
                { //done waiting
                    state = 4;
                    counter = 0;
                }
            }
            else
            { //at bottom (2/2)
                if (counter == 0)
                { //just reached bottom
                    max = (int)(pollRNG() / 65536.0 * 10 + 20); // = [20,30)
                }
                if (counter <= max)
                { //waiting
                    counter++;
                }
                else
                { //done waiting
                    state = 0;
                    counter = 0;
                }
            }
        }

        public override string ToString()
        {
            return id + OPENER + height + SEPARATOR +
                    verticalSpeed + SEPARATOR +
                    max + SEPARATOR +
                    state + SEPARATOR +
                    counter + CLOSER;
        }

    }



}
