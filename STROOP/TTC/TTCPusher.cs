using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
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
    public class TtcPusher : TtcObject
    {

        public int _max;
        public int _countdown;
        public int _state; //0 = flush with wall, 1 = retracted, 2 = extending, 3 = retracting
        public int _counter;

        public TtcPusher(TtcRng rng, int countdown) :
            this(rng, 55, countdown, 0, 0)
        {
        }

        public TtcPusher(TtcRng rng, int max, int countdown, int state, int counter) : base(rng)
        {
            _max = max;
            _countdown = countdown;
            _state = state;
            _counter = counter;
        }

        public override void Update()
        {
            if (_state == 0)
            { //flush with wall
                if (_counter <= _max)
                {
                    _counter++;
                }
                else if (_countdown > 0)
                {
                    _countdown--;
                    _counter++;
                }
                else
                {
                    int rand = PollRNG();
                    if (rand % 4 == 0) _max = 1;
                    if (rand % 4 == 1) _max = 12;
                    if (rand % 4 == 2) _max = 55;
                    if (rand % 4 == 3) _max = 100;

                    // countdown = 0 or [20,120)
                    if (PollRNG() % 2 == 0)
                    {
                        _countdown = (int)(PollRNG() / 65536.0 * 100 + 20); // = [20,120)
                    }

                    _state = 1;
                    _counter = 0;
                }
            }
            else if (_state == 1)
            { //retracted
                if (_counter < 10)
                { //waiting
                    _counter++;
                }
                else
                {
                    if (_countdown > 0)
                    { //moving back in
                        _countdown--;
                        _counter++;
                    }
                    else
                    { //moving back in
                        _state = 2;
                        _counter = 0;
                    }
                }
            }
            else if (_state == 2)
            { //extending
                if (_counter == 0)
                { //wait one frame
                    _counter++;
                }
                else if (_counter == 1)
                { //either extend out or fake it
                    if (PollRNG() % 4 == 0)
                    { //fake extend
                        _state = 0;
                        _counter = 0;
                    }
                    else
                    { //actually extend
                        _counter++;
                    }
                }
                else if (_counter < 36)
                { //continue extending out
                    _counter++;
                }
                else
                { //finished extending out
                    _state = 3;
                    _counter = 0;
                }
            }
            else
            { //retracting
                if (_counter < 82)
                { //still retracting
                    _counter++;
                }
                else
                { //finished retracting
                    _state = 0;
                    _counter = 0;
                }
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _max + SEPARATOR +
                          _countdown + SEPARATOR +
                          _state + SEPARATOR +
                          _counter + CLOSER;
        }

    }

}
