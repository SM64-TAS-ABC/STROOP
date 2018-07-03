using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A spinning triangle is the triangle platform that spins about
     *  a vertical axis (i.e. changes its yaw).
     *  It functions exactly as the cog does.
     */
    public class TtcSpinningTriangle : TtcObject
    {

        public int _angle;
        public int _currentAngularVelocity;
        public int _targetAngularVelocity;

        public TtcSpinningTriangle(TtcRng rng, int startingAngle) : base(rng)
        {
            _angle = startingAngle;
            _currentAngularVelocity = 0;
            _targetAngularVelocity = 0;
        }

        public override void update()
        {
            if (_currentAngularVelocity > _targetAngularVelocity) _currentAngularVelocity -= 50;
            else if (_currentAngularVelocity < _targetAngularVelocity) _currentAngularVelocity += 50;

            _angle += _currentAngularVelocity;
            _angle = normalize(_angle);

            if (_currentAngularVelocity == _targetAngularVelocity)
            {
                int magnitude = (pollRNG() % 7) * 200; // = 0, 200, 400, 600, 800, 1000, 1200
                int sign = (pollRNG() <= 32766) ? -1 : 1; // = -1, 1
                _targetAngularVelocity = magnitude * sign; // = -1200, -1000, ... , 1000, 1200
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _angle + SEPARATOR +
                    _currentAngularVelocity + SEPARATOR +
                    _targetAngularVelocity + CLOSER;
        }

    }


}
