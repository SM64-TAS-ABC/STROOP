using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.TTC
{
    /** A pusher is rectangular prism that extends from and
     *  retracts into the wall. They are referred to as
     *  "moving bars" in the star Timed Jumps on Moving Bars.
     *  
     *  A pusher beings flush with the wall (i.e. its outer surface
     *  is flush with the wall). It waits in this state for some
     *  amount of time, dictated by RNG. Then it retracts deeper
     *  into the wall, poised to eject, and waits in this state
     *  for some amount of time, dictated by RNG. Then it extends
     *  out, and calls RNG to determine whether it should extend out
     *  all the way out or stop flush with the wall. If it extends all
     *  the way out, then it eventually comes to a stop and then
     *  retracts until it's flush with the wall.
     */
    public class TTCPusher : TTCObject
    {

        public int max;
        public int countdown;
        public int state; //0 = flush with wall, 1 = retracted, 2 = extending, 3 = retracting
        public int counter;

        public TTCPusher(int countdownIn)
        {
            max = 55;
            countdown = countdownIn;
            state = 0;
            counter = 0;
        }

        public override void update()
        {
            if (state == 0)
            { //flush with wall
                if (counter <= max)
                {
                    counter++;
                }
                else if (countdown > 0)
                {
                    countdown--;
                    counter++;
                }
                else
                {
                    int rand = pollRNG();
                    if (rand % 4 == 0) max = 1;
                    if (rand % 4 == 1) max = 12;
                    if (rand % 4 == 2) max = 55;
                    if (rand % 4 == 3) max = 100;

                    // countdown = 0 or [20,120)
                    if (pollRNG() % 2 == 0)
                    {
                        countdown = (int)(pollRNG() / 65536.0 * 100 + 20); // = [20,120)
                    }

                    state = 1;
                    counter = 0;
                }
            }
            else if (state == 1)
            { //retracted
                if (counter < 10)
                { //waiting
                    counter++;
                }
                else
                {
                    if (countdown > 0)
                    { //moving back in
                        countdown--;
                        counter++;
                    }
                    else
                    { //moving back in
                        state = 2;
                        counter = 0;
                    }
                }
            }
            else if (state == 2)
            { //extending
                if (counter == 0)
                { //wait one frame
                    counter++;
                }
                else if (counter == 1)
                { //either extend out or fake it
                    if (pollRNG() % 4 == 0)
                    { //fake extend
                        state = 0;
                        counter = 0;
                    }
                    else
                    { //actually extend
                        counter++;
                    }
                }
                else if (counter < 36)
                { //continue extending out
                    counter++;
                }
                else
                { //finished extending out
                    state = 3;
                    counter = 0;
                }
            }
            else
            { //retracting
                if (counter < 82)
                { //still retracting
                    counter++;
                }
                else
                { //finished retracting
                    state = 0;
                    counter = 0;
                }
            }
        }

        public override string ToString()
        {
            return id + OPENER + max + SEPARATOR +
                          countdown + SEPARATOR +
                          state + SEPARATOR +
                          counter + CLOSER;
        }

    }

}
