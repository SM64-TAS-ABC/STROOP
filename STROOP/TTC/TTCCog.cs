using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A cog is the hexagon platform that spins about
        *  a vertical axis (i.e. changes its yaw).
        *  
        *  A cog has a target angular velocity and a current angular velocity.
        *  On every frame, the current angular velocity moves 50 towards
        *  the target angular velocity. Then the cog's angle changes
        *  by the current angular velocity. When the current angular velocity
        *  reaches the target angular velocity, a new target angular velocity
        *  is calculated.
        */
    public class TtcCog : TtcObject
    {

        public int _angle;
        public int _currentAngularVelocity;
        public int _targetAngularVelocity;

        public int _endingYaw
        {
            get => WatchVariableSpecialUtilities.GetCogEndingYaw(
                MoreMath.NormalizeAngleUshort(_angle), _currentAngularVelocity, _targetAngularVelocity);
        }

        public TtcCog(TtcRng rng, uint address) :
            this(
                rng: rng,
                angle: Normalize(Config.Stream.GetInt32(address + 0xD4)),
                currentAngularVelocity: (int)Config.Stream.GetSingle(address + 0xF8),
                targetAngularVelocity: (int)Config.Stream.GetSingle(address + 0xFC))
        {
        }

        public TtcCog(TtcRng rng) : this(rng, 0, 0, 0)
        {
        }

        public TtcCog(TtcRng rng, int angle, int currentAngularVelocity, int targetAngularVelocity) : base(rng)
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
