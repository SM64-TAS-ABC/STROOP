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
        public int _max;
        public int _counter;

        public TtcTreadmill(TtcRng rng, bool isFirstTreadmill) :
            this(rng, isFirstTreadmill, 0, 0, 0, 0)
        {
        }

        public TtcTreadmill(TtcRng rng, bool isFirstTreadmill,
            int currentSpeed, int targetSpeed, int max, int counter) : base(rng)
        {
            _isFirstTreadmill = isFirstTreadmill;
            _currentSpeed = currentSpeed;
            _targetSpeed = targetSpeed;
            _max = max;
            _counter = counter;
        }

        public override void Update()
        {
            if (!_isFirstTreadmill)
            { //if not first treadmill, do nothing
                _counter++;
                return;
            }

            if (_counter <= _max)
            { //still/accelerate/move
                if (_counter <= 5)
                { //be still
                    _counter++;
                }
                else
                { //accelerate/move
                    _currentSpeed = MoveNumberTowards(_currentSpeed, _targetSpeed, 10);
                    _counter++;
                }
            }
            else
            { //slow down
                _currentSpeed = MoveNumberTowards(_currentSpeed, 0, 10);
                if (_currentSpeed == 0)
                { //came to a stop
                    _max = (PollRNG() % 7) * 20 + 10; // = 10, 30, 50, 70, 90, 110, 130
                    _targetSpeed = (PollRNG() <= 32766) ? -50 : 50; // = -50, 50
                    _counter = 0;
                }
                _counter++;
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _currentSpeed + SEPARATOR +
                      _targetSpeed + SEPARATOR +
                      _max + SEPARATOR +
                      _counter + CLOSER;
        }

    }

}
