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

        //whether this treadmill is the first treadmill
        public readonly bool _isFirstTreadmill;
	
	    public int _currentSpeed;
        public int _targetSpeed;
        public int _timerMax;
        public int _timer;

        public TtcTreadmill(TtcRng rng, uint address) :
             this(
                 rng: rng,
                 isFirstTreadmill: Config.Stream.GetInt32(address + 0x144) == 0,
                 currentSpeed: (int)Config.Stream.GetSingle(address + 0xFC),
                 targetSpeed: (int)Config.Stream.GetSingle(address + 0x100),
                 timerMax: Config.Stream.GetInt32(address + 0x104),
                 timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcTreadmill(TtcRng rng, bool isFirstTreadmill) :
            this(rng, isFirstTreadmill, 0, 0, 0, 0)
        {
        }

        public TtcTreadmill(TtcRng rng, bool isFirstTreadmill,
            int currentSpeed, int targetSpeed, int timerMax, int timer) : base(rng)
        {
            _isFirstTreadmill = isFirstTreadmill;
            _currentSpeed = currentSpeed;
            _targetSpeed = targetSpeed;
            _timerMax = timerMax;
            _timer = timer;
        }

        public override void Update()
        {
            if (!_isFirstTreadmill)
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
            return new List<object>() { _currentSpeed, _targetSpeed, _timerMax, _timer };
        }

    }

}
