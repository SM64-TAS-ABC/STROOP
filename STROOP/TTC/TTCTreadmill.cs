using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A treadmill is the yellow treadmill that periodically
     *  reverses directions. Although there are 7 treadmills
     *  in TTC, only the first one actually updates meaningfully
     *  using RNG, and the others simply copy what this
     *  first one is doing.
     *  
     *  When a treadmill comes to a stop, it calls RNG to determine
     *  whether it should move forwards or backwards and also
     *  how long it should move in that direction. Then the treadmill
     *  accelerates to 50 speed in that direction. Once it's moved
     *  in that direction for the allotted time, it decelerates to
     *  a stop, and the process repeats.
     */
    public class TtcTreadmill : TtcObject
    {

        public readonly int _subType;
		public int _currentSpeed;
        public int _targetSpeed;
        public int _timerMax;
        public int _timer;

        public TtcTreadmill(TtcRng rng, uint address) :
             this(
                 rng: rng,
                 subType: Config.Stream.GetInt32(address + 0x144),
                 currentSpeed: (int)Config.Stream.GetSingle(address + 0xFC),
                 targetSpeed: (int)Config.Stream.GetSingle(address + 0x100),
                 timerMax: Config.Stream.GetInt32(address + 0x104),
                 timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcTreadmill(TtcRng rng, int subType) :
            this(rng, subType, 0, 0, 0, 0)
        {
        }

        public TtcTreadmill(TtcRng rng, int subType,
            int currentSpeed, int targetSpeed, int timerMax, int timer) : base(rng)
        {
            _subType = subType;
            _currentSpeed = currentSpeed;
            _targetSpeed = targetSpeed;
            _timerMax = timerMax;
            _timer = timer;
        }

        public override void Update()
        {
            if (_subType != 0)
            { //if not first treadmill, do nothing
                _timer++;
                return;
            }

            if (_timer <= _timerMax)
            { //still/accelerate/move
                if (_timer <= 5)
                { //be still
                    _timer++;
                }
                else
                { //accelerate/move
                    _currentSpeed = MoveNumberTowards(_currentSpeed, _targetSpeed, 10);
                    _timer++;
                }
            }
            else
            { //slow down
                _currentSpeed = MoveNumberTowards(_currentSpeed, 0, 10);
                if (_currentSpeed == 0)
                { //came to a stop
                    _timerMax = (PollRNG() % 7) * 20 + 10; // = 10, 30, 50, 70, 90, 110, 130
                    _targetSpeed = (PollRNG() <= 32766) ? -50 : 50; // = -50, 50
                    _timer = 0;
                }
                _timer++;
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _currentSpeed + SEPARATOR +
                      _targetSpeed + SEPARATOR +
                      _timerMax + SEPARATOR +
                      _timer + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>() { _subType, _currentSpeed, _targetSpeed, _timerMax, _timer };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_subType, address + 0x144);
            Config.Stream.SetValue((float)_currentSpeed, address + 0xFC);
            Config.Stream.SetValue((float)_targetSpeed, address + 0x100);
            Config.Stream.SetValue(_timerMax, address + 0x104);
            Config.Stream.SetValue(_timer, address + 0x154);
        }
    }

}
