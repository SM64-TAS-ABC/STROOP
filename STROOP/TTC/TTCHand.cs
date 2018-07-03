using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A hand is the long horizontal clock hand that rotates
      *  in a circle and that Mario is supposed to ride on to
      *  get to the other side of the course.
      *  
      *  A hand has a displacement (i.e. how much it should tick, which
      *  will be positive for CCW ticks and negative for CW ticks).
      *  When timer > max, the hand ticks and calculates a new max
      *  using RNG. Furthermore, if at this time the direction timer
      *  has decremented to zero, then the hand will calculate a new
      *  displacement (i.e. whether it should tick CW or CCW) as well as
      *  how long it should be until it has the chance to change
      *  direction again (i.e. what value its direction timer will be).
      */
    public class TtcHand : TtcObject
    {

        public readonly static int DISPLACEMENT_MAGNITUDE = 1092;
        public readonly static int INITIAL_MAX = 10;

        public int _angle;
        public int _max;
        public int _targetAngle;
        public int _displacement;
        public int _directionTimer;
        public int _timer;

        public TtcHand(TtcRng rng, int startingAngle) : base(rng)
        {
            _angle = startingAngle;
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
                    {
                        _displacement = DISPLACEMENT_MAGNITUDE;
                        _directionTimer = (PollRNG() % 3) * 30 + 30; // = 30, 60, 90
                    }
                    else
                    {
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
