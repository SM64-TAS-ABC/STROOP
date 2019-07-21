using STROOP.Structs.Configurations;
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
        public int _direction; //0 = going up, 1 = going down
        public int _timerMax;
        public int _timer;

        public TtcPitBlock(TtcRng rng, uint address) :
            this(
                rng: rng,
                height: (int)Config.Stream.GetSingle(address + 0xA4),
                verticalSpeed: (int)Config.Stream.GetSingle(address + 0xB0),
                direction: Config.Stream.GetInt32(address + 0xF8),
                timerMax: Config.Stream.GetInt32(address + 0xFC),
                timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcPitBlock(TtcRng rng) :
            this(rng, MIN_HEIGHT, 0, 0, 0, 0)
        {
        }

        public TtcPitBlock(TtcRng rng, int height, int verticalSpeed,
            int direction, int timerMax, int timer) : base(rng)
        {
            _height = height;
            _verticalSpeed = verticalSpeed;
            _direction = direction;
            _timerMax = timerMax;
            _timer = timer;
        }

        public override void Update()
        {
            if (_timer <= _timerMax)
            { //don't move
                _timer++;
            }
            else
            { //move
                if (_direction == 0)
                { //move up
                    _height = Math.Min(MAX_HEIGHT, _height + _verticalSpeed);
                    if (_height == MIN_HEIGHT || _height == MAX_HEIGHT)
                    { //reached top
                        _verticalSpeed = -9;
                        _direction = 1;
                        _timer = 0;
                        _timerMax = (PollRNG() % 6) * 20 + 10; // = 10, 30, 50, 70, 90, 110
                    }
                    _timer++;
                }
                else
                { //move down
                    _height = Math.Max(MIN_HEIGHT, _height + _verticalSpeed);
                    if (_height == MIN_HEIGHT || _height == MAX_HEIGHT)
                    { //reached bottom
                        _verticalSpeed = 11;
                        _direction = 0;
                        _timer = 0;
                        _timerMax = 20;
                    }
                    _timer++;
                }
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _height + SEPARATOR +
                    _verticalSpeed + SEPARATOR +
                    _direction + SEPARATOR +
                    _timerMax + SEPARATOR +
                    _timer + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>()
            {
                _height, _verticalSpeed, _direction, _timerMax, _timer
            };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue((float)_height, address + 0xA4);
            Config.Stream.SetValue((float)_verticalSpeed, address + 0xB0);
            Config.Stream.SetValue(_direction, address + 0xF8);
            Config.Stream.SetValue(_timerMax, address + 0xFC);
            Config.Stream.SetValue(_timer, address + 0x154);
        }
    }

}
