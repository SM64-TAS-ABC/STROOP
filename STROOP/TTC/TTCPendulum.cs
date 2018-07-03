using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A pendulum is the pendulum that swings back and forth.
      *  
      *  A pendulum at rest will call RNG to determine how long
      *  it should wait for and how fast it should accelerates
      *  during the next swing. After it's waited the allotted time,
      *  it swings with that acceleration. Once it crosses strictly
      *  past the vertical (i.e. angle 0), the pendulum decelerates
      *  by that same acceleration until it comes to a stop.
      */
    public class TtcPendulum : TtcObject
    {

        public int _accelerationDirection;
        public int _angle;
        public int _angularVelocity;
        public int _accelerationMagnitude;
        public int _waitingTimer;

        public TtcPendulum(TtcRng rng) : base(rng)
        {
            _accelerationDirection = 0;
            _angle = 6500;
            _angularVelocity = 0;
            _accelerationMagnitude = 0;
            _waitingTimer = 0;
        }

        public override void Update()
        {

            if (_waitingTimer > 0)
            { //waiting
                _waitingTimer--;
            }
            else
            { //swinging

                if (_accelerationMagnitude == 0)
                { //give initial acceleration on start
                    _accelerationMagnitude = 13;
                }

                if (_angle > 0) _accelerationDirection = -1;
                else if (_angle < 0) _accelerationDirection = 1;

                _angularVelocity = _angularVelocity + _accelerationDirection * _accelerationMagnitude;
                _angle = _angle + _angularVelocity;

                if (_angularVelocity == 0)
                { //reached peak of swing
                    _accelerationMagnitude = (PollRNG() % 3 == 0) ? 42 : 13; // = 13, 42
                    if (PollRNG() % 2 == 0)
                    { //stop for some time
                        _waitingTimer = (int)(PollRNG() / 65536.0 * 30 + 5); // = [5,35)
                    }
                }
            }

        }

        public override string ToString()
        {
            return _id + OPENER + _accelerationDirection + SEPARATOR +
                      _angle + SEPARATOR +
                      _angularVelocity + SEPARATOR +
                      _accelerationMagnitude + SEPARATOR +
                      _waitingTimer + CLOSER;
        }

    }

}
