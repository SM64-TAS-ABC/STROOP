using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A thwomp is the blue cube-like enemy that moves up and down
     *  in an attempt to squish Mario. There is only one Thwomp
     *  in TTC, near the very top of the clock.
     *  
     *  A thwomp moves up, waits, moves down, waits, then repeats.
     *  It calls RNG when it reaches the top to determine how long
     *  it should wait for, and it also does this at the bottom.
     */
    public class TtcThwomp : TtcObject
    {

        public readonly static int MIN_HEIGHT = 6192;
        public readonly static int MAX_HEIGHT = 6607;
        public readonly static int RISING_SPEED = 10;

        public int _height;
        public int _verticalSpeed;
        public int _max;
        public int _state; //0 = going up, 1 = at top, 2 = going down, 3/4 = at bottom
        public int _counter;

        public TtcThwomp(TtcRng rng) : this(rng, MIN_HEIGHT, 0, 0, 0, 0)
        {
        }

        public TtcThwomp(TtcRng rng, int height, int verticalSpeed,
            int max, int state, int counter) : base(rng)
        {
            _height = height;
            _verticalSpeed = verticalSpeed;
            _max = max;
            _state = state;
            _counter = counter;
        }

        public override void Update()
        {
            if (_state == 0)
            { //going up
                _height = Math.Min(MAX_HEIGHT, _height + RISING_SPEED);
                _counter++;
                if (_height == MAX_HEIGHT)
                { //reached top
                    _state = 1;
                    _counter = 0;
                }
            }
            else if (_state == 1)
            { //at top
                if (_counter == 0)
                { //just reached top
                    _max = (int)(PollRNG() / 65536.0 * 30 + 10); // = [10,40)
                }
                if (_counter <= _max)
                { //waiting
                    _counter++;
                }
                else
                { //done waiting
                    _state = 2;
                    _counter = 0;
                }
            }
            else if (_state == 2)
            { //going down
                _verticalSpeed -= 4;
                _height = Math.Max(MIN_HEIGHT, _height + _verticalSpeed);
                _counter++;
                if (_height == MIN_HEIGHT)
                { //reached bottom
                    _verticalSpeed = 0;
                    _state = 3;
                    _counter = 0;
                }
            }
            else if (_state == 3)
            { //at bottom (1/2)
                if (_counter < 10)
                { //waiting
                    _counter++;
                }
                else
                { //done waiting
                    _state = 4;
                    _counter = 0;
                }
            }
            else
            { //at bottom (2/2)
                if (_counter == 0)
                { //just reached bottom
                    _max = (int)(PollRNG() / 65536.0 * 10 + 20); // = [20,30)
                }
                if (_counter <= _max)
                { //waiting
                    _counter++;
                }
                else
                { //done waiting
                    _state = 0;
                    _counter = 0;
                }
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _height + SEPARATOR +
                    _verticalSpeed + SEPARATOR +
                    _max + SEPARATOR +
                    _state + SEPARATOR +
                    _counter + CLOSER;
        }

    }



}
