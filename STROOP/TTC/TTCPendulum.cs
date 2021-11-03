using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Ttc
{
    public class TtcPendulum : TtcObject
    {
        public float _accelerationDirection;
        public float _angle;
        public float _angularVelocity;
        public float _accelerationMagnitude;
        public int _waitingTimer;

        public TtcPendulum(TtcRng rng, uint address) :
            this(
                rng: rng,
                accelerationDirection: Config.Stream.GetFloat(address + 0xF4),
                angle: Config.Stream.GetFloat(address + 0xF8),
                angularVelocity: Config.Stream.GetFloat(address + 0xFC),
                accelerationMagnitude: Config.Stream.GetFloat(address + 0x100),
                waitingTimer: Config.Stream.GetInt(address + 0x104))
        {
        }

        public TtcPendulum(TtcRng rng) :
            this(rng, 1, 6500, 0, 0, 0)
        {
        }

        public TtcPendulum(TtcRng rng, float accelerationDirection, float angle,
            float angularVelocity, float accelerationMagnitude, int waitingTimer) : base(rng)
        {
            _accelerationDirection = accelerationDirection;
            _angle = angle;
            _angularVelocity = angularVelocity;
            _accelerationMagnitude = accelerationMagnitude;
            _waitingTimer = waitingTimer;
        }

        public override void Update()
        {
            if (_waitingTimer != 0)
            {
                _waitingTimer--;
            }
            else
            {
                if (_angle * _accelerationDirection > 0.0f)
                {
                    _accelerationDirection = -_accelerationDirection;
                }
                _angularVelocity += _accelerationMagnitude * _accelerationDirection;

                if (_angularVelocity == 0.0f)
                {
                    if (PollRNG() % 3 != 0)
                    {
                        _accelerationMagnitude = 13.0f;
                    }
                    else
                    {
                        _accelerationMagnitude = 42.0f;
                    }

                    if (PollRNG() % 2 == 0)
                    {
                        _waitingTimer = (int)(PollRNG() / 65536.0 * 30 + 5);
                    }
                }

                _angle += _angularVelocity;
            }
        }

        public void Update2(bool goFast)
        {
            _accelerationMagnitude = goFast ? 42.0f : 13.0f;

            if (_angle * _accelerationDirection > 0.0f)
            {
                _accelerationDirection = -_accelerationDirection;
            }
            _angularVelocity += _accelerationMagnitude * _accelerationDirection;

            _angle += _angularVelocity;
        }

        public bool PerformSwing(bool goFast)
        {
            bool swungThroughZero = false;
            while (true)
            {
                Update2(goFast);
                if (_angle == 0) swungThroughZero = true;
                if (_angularVelocity == 0.0f) break;
            }
            return swungThroughZero;
        }

        public override string ToString()
        {
            return _id + OPENER + _accelerationDirection + SEPARATOR +
                      _angle + SEPARATOR +
                      _angularVelocity + SEPARATOR +
                      _accelerationMagnitude + SEPARATOR +
                      _waitingTimer + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>()
            {
                _accelerationDirection, _angle, _angularVelocity, _accelerationMagnitude, _waitingTimer
            };
        }

        public override XElement ToXml()
        {
            XElement xElement = new XElement("TtcPendulum");
            xElement.Add(new XAttribute("_accelerationDirection", _accelerationDirection));
            xElement.Add(new XAttribute("_angle", _angle));
            xElement.Add(new XAttribute("_angularVelocity", _angularVelocity));
            xElement.Add(new XAttribute("_accelerationMagnitude", _accelerationMagnitude));
            xElement.Add(new XAttribute("_waitingTimer", _waitingTimer));
            return xElement;
        }

        public int GetAmplitude()
        {
            return (int)WatchVariableSpecialUtilities.GetPendulumAmplitude(
                _accelerationDirection, _accelerationMagnitude, _angularVelocity, _angle);
        }

        public int? GetSwingIndex()
        {
            return TableConfig.PendulumSwings.GetPendulumSwingIndex(GetAmplitude());
        }

        public string GetSwingIndexExtended()
        {
            return TableConfig.PendulumSwings.GetPendulumSwingIndexExtended(GetAmplitude());
        }

        public (int, int)? GetSwingIndexExtendedPair()
        {
            return TableConfig.PendulumSwings.GetPendulumSwingIndexExtendedPair(GetAmplitude());
        }

        public int GetCountdown()
        {
            return WatchVariableSpecialUtilities.GetPendulumCountdown(
                _accelerationDirection, _accelerationMagnitude, _angularVelocity, _angle, _waitingTimer);
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_accelerationDirection, address + 0xF4);
            Config.Stream.SetValue(_angle, address + 0xF8);
            Config.Stream.SetValue(_angularVelocity, address + 0xFC);
            Config.Stream.SetValue(_accelerationMagnitude, address + 0x100);
            Config.Stream.SetValue(_waitingTimer, address + 0x104);
        }

        public override TtcObject Clone(TtcRng rng)
        {
            return new TtcPendulum(rng, _accelerationDirection, _angle, _angularVelocity, _accelerationMagnitude, _waitingTimer);
        }

        public override bool Equals(object obj)
        {
            if (obj is TtcPendulum other)
            {
                return _accelerationDirection == other._accelerationDirection &&
                    _angle == other._angle &&
                    _angularVelocity == other._angularVelocity &&
                    _accelerationMagnitude == other._accelerationMagnitude &&
                    _waitingTimer == other._waitingTimer;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return
                (int)_accelerationDirection * 7 +
                (int)_angle * 11 +
                (int)_angularVelocity * 13 +
                (int)_accelerationMagnitude * 17 +
                _waitingTimer * 19;
        }
    }
}
