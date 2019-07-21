using STROOP.Structs.Configurations;
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

        public int _height;
        public int _verticalSpeed;
        public int _direction;
        public int _timerMax;
        public int _timer;

        public TtcElevator(TtcRng rng, uint address) :
            this(
                rng: rng,
                minHeight: (int)Config.Stream.GetSingle(address + 0x168),
                maxHeight: (int)Config.Stream.GetSingle(address + 0xF8),
                height: (int)Config.Stream.GetSingle(address + 0xA4),
                verticalSpeed: (int)Config.Stream.GetSingle(address + 0xB0),
                direction: (int)Config.Stream.GetSingle(address + 0xF4),
                timerMax: Config.Stream.GetInt32(address + 0xFC),
                timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcElevator(TtcRng rng, int minHeight, int maxHeight) :
            this(rng, minHeight, maxHeight, minHeight, 0, 1, 0, 0)
        {
        }

        public TtcElevator(
            TtcRng rng, int minHeight, int maxHeight, int height,
            int verticalSpeed, int direction, int timerMax, int timer) : base(rng)
        {
            MIN_HEIGHT = minHeight;
            MAX_HEIGHT = maxHeight;
            _height = height;
            _verticalSpeed = verticalSpeed;
            _direction = direction;
            _timerMax = timerMax;
            _timer = timer;
        }

        public override void Update()
        {
            if (_timer <= 4)
            {
                _verticalSpeed = 0;
            }
            else
            {
                _verticalSpeed = _direction * 6;
            }

            _height = _height + _verticalSpeed;

            if (_timer > _timerMax)
            {
                _direction = (PollRNG() <= 32766) ? -1 : 1; // = -1, 1
                _timerMax = (PollRNG() % 6) * 30 + 30; // = 30, 60, 90, 120, 150, 180
                _timer = 0;
            }

            _height = Math.Max(_height, MIN_HEIGHT);
            _height = Math.Min(_height, MAX_HEIGHT);
            if (_height == MIN_HEIGHT || _height == MAX_HEIGHT)
            {
                _direction *= -1;
            }
            _timer++;
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
            return new List<object>() { _height, _verticalSpeed, _direction, _timerMax, _timer };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue((float)MIN_HEIGHT, address + 0x168);
            Config.Stream.SetValue((float)MAX_HEIGHT, address + 0xF8);
            Config.Stream.SetValue((float)_height, address + 0xA4);
            Config.Stream.SetValue((float)_verticalSpeed, address + 0xB0);
            Config.Stream.SetValue((float)_direction, address + 0xF4);
            Config.Stream.SetValue(_timerMax, address + 0xFC);
            Config.Stream.SetValue(_timer, address + 0x154);
        }
    }


}
