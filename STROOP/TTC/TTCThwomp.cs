using STROOP.Structs.Configurations;
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
        public int _timerMax;
        public int _state; //0 = going up, 1 = at top, 2 = going down, 3/4 = at bottom
        public int _timer;

        public TtcThwomp(TtcRng rng, uint address) :
            this(
                rng: rng,
                height: (int)Config.Stream.GetSingle(address + 0xA4),
                verticalSpeed: (int)Config.Stream.GetSingle(address + 0xB0),
                timerMax: Config.Stream.GetInt32(address + 0xF4),
                state: Config.Stream.GetInt32(address + 0x14C),
                timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcThwomp(TtcRng rng) : this(rng, MIN_HEIGHT, 0, 0, 0, 0)
        {
        }

        public TtcThwomp(TtcRng rng, int height, int verticalSpeed,
            int timerMax, int state, int timer) : base(rng)
        {
            _height = height;
            _verticalSpeed = verticalSpeed;
            _timerMax = timerMax;
            _state = state;
            _timer = timer;
        }

        public override void Update()
        {
            if (_state == 0)
            { //going up
                _height = Math.Min(MAX_HEIGHT, _height + RISING_SPEED);
                _timer++;
                if (_height == MAX_HEIGHT)
                { //reached top
                    _state = 1;
                    _timer = 0;
                }
            }
            else if (_state == 1)
            { //at top
                if (_timer == 0)
                { //just reached top
                    _timerMax = (int)(PollRNG() / 65536.0 * 30 + 10); // = [10,40)
                }
                if (_timer <= _timerMax)
                { //waiting
                    _timer++;
                }
                else
                { //done waiting
                    _state = 2;
                    _timer = 0;
                }
            }
            else if (_state == 2)
            { //going down
                _verticalSpeed -= 4;
                _height = Math.Max(MIN_HEIGHT, _height + _verticalSpeed);
                _timer++;
                if (_height == MIN_HEIGHT)
                { //reached bottom
                    _verticalSpeed = 0;
                    _state = 3;
                    _timer = 0;
                }
            }
            else if (_state == 3)
            { //at bottom (1/2)
                if (_timer < 10)
                { //waiting
                    _timer++;
                }
                else
                { //done waiting
                    _state = 4;
                    _timer = 0;
                }
            }
            else
            { //at bottom (2/2)
                if (_timer == 0)
                { //just reached bottom
                    _timerMax = (int)(PollRNG() / 65536.0 * 10 + 20); // = [20,30)
                }
                if (_timer <= _timerMax)
                { //waiting
                    _timer++;
                }
                else
                { //done waiting
                    _state = 0;
                    _timer = 0;
                }
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _height + SEPARATOR +
                    _verticalSpeed + SEPARATOR +
                    _timerMax + SEPARATOR +
                    _state + SEPARATOR +
                    _timer + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>()
            {
                _height, _verticalSpeed, _timerMax, _state, _timer
            };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue((float)_height, address + 0xA4);
            Config.Stream.SetValue((float)_verticalSpeed, address + 0xB0);
            Config.Stream.SetValue(_timerMax, address + 0xF4);
            Config.Stream.SetValue(_state, address + 0x14C);
            Config.Stream.SetValue(_timer, address + 0x154);
        }
    }



}
