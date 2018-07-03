using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A wheel is the little cog-like structure that is found
     *  slightly under the floor in various parts of TTC.
     *  They appear in pairs. They function exactly as the hands do,
     *  except that their ticks are greater in magnitude.
     */
    public class TtcWheel : TtcObject
    {

        public readonly static int DISPLACEMENT_MAGNITUDE = 3276;
        public readonly static int INITIAL_MAX = 5;

        public int _angle;
        public int _max;
        public int _targetAngle;
        public int _displacement;
        public int _directionTimer;
        public int _timer;

        public TtcWheel(TtcRng rng) : base(rng)
        {
            _angle = 0;
            _max = 0;
            _targetAngle = 0;
            _displacement = 0;
            _directionTimer = 0;
            _timer = 0;
        }

        public override void Update()
        {

            if (_max == 0)
            { //course just started
                _max = INITIAL_MAX;
                _displacement = -1 * DISPLACEMENT_MAGNITUDE;
            }

            _angle = this.MoveAngleTowards(_angle, _targetAngle, 200);

            _directionTimer = Math.Max(0, _directionTimer - 1);

            if (_timer <= _max)
            { //waiting
                _timer++;
            }
            else if (_angle == _targetAngle)
            { //done waiting and reached target
                _targetAngle = _targetAngle + _displacement;
                _targetAngle = Normalize(_targetAngle);

                if (_directionTimer == 0)
                { //time to maybe switch directions
                    if (PollRNG() % 4 == 0)
                    { //time to move CCW
                        _displacement = DISPLACEMENT_MAGNITUDE;
                        _directionTimer = (PollRNG() % 3) * 30 + 30; // = 30, 60, 90
                    }
                    else
                    { //time to move CW
                        _displacement = -1 * DISPLACEMENT_MAGNITUDE;
                        _directionTimer = (PollRNG() % 4) * 60 + 90; // = 90, 150, 210, 270
                    }
                }

                _max = (PollRNG() % 3) * 20 + 10; // = 10, 30, 50
                _timer = 0;
                _timer++;
            }
            else
            { //timer high enough, but not at target angle (will only happen at level start)
                _timer++;
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _angle + SEPARATOR +
                          _max + SEPARATOR +
                          _targetAngle + SEPARATOR +
                          _displacement + SEPARATOR +
                          _directionTimer + SEPARATOR +
                          _timer + CLOSER;
        }

    }

}
