using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** The pit block is the cube platform that moves up and down
     *  near the pit and the pendulums star. There is only 1 pit block.
     *  
     *  The piblock moves up, waits, moves down, waits, then repeats.
     *  It calls RNG when it reaches the top to determine how long
     *  it should wait for. It does not do this at the bottom,
     *  since the time it waits there is always 20 frames.
     */
    public class TtcPitBlock : TtcObject
    {

        public readonly static int MIN_HEIGHT = -71;
        public readonly static int MAX_HEIGHT = 259;

        public int _height;
        public int _verticalSpeed;
        public int _state; //0 = going up, 1 = going down
        public int _max;
        public int _counter;

        public TtcPitBlock(TtcRng rng) : base(rng)
        {
            _height = MIN_HEIGHT;
            _verticalSpeed = 0;
            _state = 0;
            _max = 0;
            _counter = 0;
        }

        public override void Update()
        {
            if (_counter <= _max)
            { //don't move
                _counter++;
            }
            else
            { //move
                if (_state == 0)
                { //move up
                    _height = Math.Min(MAX_HEIGHT, _height + _verticalSpeed);
                    if (_height == MIN_HEIGHT || _height == MAX_HEIGHT)
                    { //reached top
                        _verticalSpeed = -9;
                        _state = 1;
                        _counter = 0;
                        _max = (PollRNG() % 6) * 20 + 10; // = 10, 30, 50, 70, 90, 110
                    }
                    _counter++;
                }
                else
                { //move down
                    _height = Math.Max(MIN_HEIGHT, _height + _verticalSpeed);
                    if (_height == MIN_HEIGHT || _height == MAX_HEIGHT)
                    { //reached bottom
                        _verticalSpeed = 11;
                        _state = 0;
                        _counter = 0;
                        _max = 20;
                    }
                    _counter++;
                }
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _height + SEPARATOR +
                    _verticalSpeed + SEPARATOR +
                    _state + SEPARATOR +
                    _max + SEPARATOR +
                    _counter + CLOSER;
        }

    }

}
