using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.TTC
{
    /** The pit block is the cube platform that moves up and down
     *  near the pit and the pendulums star. There is only 1 pit block.
     *  
     *  The piblock moves up, waits, moves down, waits, then repeats.
     *  It calls RNG when it reaches the top to determine how long
     *  it should wait for. It does not do this at the bottom,
     *  since the time it waits there is always 20 frames.
     */
    public class PitBlock : RNGObject
    {

        public readonly static int MIN_HEIGHT = -71;
        public readonly static int MAX_HEIGHT = 259;

        public int height;
        public int verticalSpeed;
        public int state; //0 = going up, 1 = going down
        public int max;
        public int counter;

        public PitBlock()
        {
            height = MIN_HEIGHT;
            verticalSpeed = 0;
            state = 0;
            max = 0;
            counter = 0;
        }

        public override void update()
        {
            if (counter <= max)
            { //don't move
                counter++;
            }
            else
            { //move
                if (state == 0)
                { //move up
                    height = Math.Min(MAX_HEIGHT, height + verticalSpeed);
                    if (height == MIN_HEIGHT || height == MAX_HEIGHT)
                    { //reached top
                        verticalSpeed = -9;
                        state = 1;
                        counter = 0;
                        max = (pollRNG() % 6) * 20 + 10; // = 10, 30, 50, 70, 90, 110
                    }
                    counter++;
                }
                else
                { //move down
                    height = Math.Max(MIN_HEIGHT, height + verticalSpeed);
                    if (height == MIN_HEIGHT || height == MAX_HEIGHT)
                    { //reached bottom
                        verticalSpeed = 11;
                        state = 0;
                        counter = 0;
                        max = 20;
                    }
                    counter++;
                }
            }
        }

        public override string ToString()
        {
            return id + OPENER + height + SEPARATOR +
                    verticalSpeed + SEPARATOR +
                    state + SEPARATOR +
                    max + SEPARATOR +
                    counter + CLOSER;
        }

    }

}
