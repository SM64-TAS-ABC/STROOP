using STROOP.Structs.Configurations;
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

        public TtcSpinningTriangle(TtcRng rng, uint address) :
            this(
                rng: rng,
                angle: Normalize(Config.Stream.GetInt32(address + 0xD4)),
                currentAngularVelocity: (int)Config.Stream.GetSingle(address + 0xF8),
                targetAngularVelocity: (int)Config.Stream.GetSingle(address + 0xFC))
        {
        }

        public TtcSpinningTriangle(TtcRng rng, int angle) : this(rng, angle, 0, 0)
        {
        }

        public TtcSpinningTriangle(TtcRng rng, int angle,
            int currentAngularVelocity, int targetAngularVelocity) : base(rng)
        {
            _angle = angle;
            _currentAngularVelocity = currentAngularVelocity;
            _targetAngularVelocity = targetAngularVelocity;
        }

        public override void Update()
        {
            if (_currentAngularVelocity > _targetAngularVelocity) _currentAngularVelocity -= 50;
            else if (_currentAngularVelocity < _targetAngularVelocity) _currentAngularVelocity += 50;

            _angle += _currentAngularVelocity;
            _angle = Normalize(_angle);

            if (_currentAngularVelocity == _targetAngularVelocity)
            {
                int magnitude = (PollRNG() % 7) * 200; // = 0, 200, 400, 600, 800, 1000, 1200
                int sign = (PollRNG() <= 32766) ? -1 : 1; // = -1, 1
                _targetAngularVelocity = magnitude * sign; // = -1200, -1000, ... , 1000, 1200
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _angle + SEPARATOR +
                    _currentAngularVelocity + SEPARATOR +
                    _targetAngularVelocity + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>() { _angle, _currentAngularVelocity, _targetAngularVelocity };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_angle, address + 0xD4);
            Config.Stream.SetValue((float)_currentAngularVelocity, address + 0xF8);
            Config.Stream.SetValue((float)_targetAngularVelocity, address + 0xFC);
        }
    }


}
