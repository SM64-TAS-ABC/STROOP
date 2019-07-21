using STROOP.Structs.Configurations;
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
        public int _timerMax;
        public int _targetAngle;
        public int _displacement;
        public int _directionCountdown;
        public int _timer;

        public TtcHand(TtcRng rng, uint address) :
            this(
                rng: rng,
                angle: Normalize(Config.Stream.GetInt32(address + 0xD4)),
                timerMax: Config.Stream.GetInt32(address + 0xF4),
                targetAngle: Normalize(Config.Stream.GetInt32(address + 0xF8)),
                displacement: Config.Stream.GetInt32(address + 0xFC),
                directionCountdown: Config.Stream.GetInt32(address + 0x104),
                timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcHand(TtcRng rng, int angle) :
            this(rng, angle, 0, 0, 0, 0, 0)
        {
        }

        public TtcHand(TtcRng rng, int angle, int timerMax, int targetAngle,
            int displacement, int directionCountdown, int timer) : base(rng)
        {
            _angle = angle;
            _timerMax = timerMax;
            _targetAngle = targetAngle;
            _displacement = displacement;
            _directionCountdown = directionCountdown;
            _timer = timer;
        }

        public override void Update()
        {

            if (_timerMax == 0)
            { //course just started
                _timerMax = INITIAL_MAX;
                _displacement = -1 * DISPLACEMENT_MAGNITUDE;
            }

            _angle = MoveAngleTowards(_angle, _targetAngle, 200);

            _directionCountdown = Math.Max(0, _directionCountdown - 1);

            if (_timer <= _timerMax)
            { //waiting
                _timer++;
            }
            else if (_angle == _targetAngle)
            { //done waiting and reached target
                _targetAngle = _targetAngle + _displacement;
                _targetAngle = Normalize(_targetAngle);

                if (_directionCountdown == 0)
                { //time to maybe switch directions
                    if (PollRNG() % 4 == 0)
                    {
                        _displacement = DISPLACEMENT_MAGNITUDE;
                        _directionCountdown = (PollRNG() % 3) * 30 + 30; // = 30, 60, 90
                    }
                    else
                    {
                        _displacement = -1 * DISPLACEMENT_MAGNITUDE;
                        _directionCountdown = (PollRNG() % 4) * 60 + 90; // = 90, 150, 210, 270
                    }
                }

                _timerMax = (PollRNG() % 3) * 20 + 10; // = 10, 30, 50
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
                          _timerMax + SEPARATOR +
                          _targetAngle + SEPARATOR +
                          _displacement + SEPARATOR +
                          _directionCountdown + SEPARATOR +
                          _timer + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>()
            {
                _angle, _timerMax, _targetAngle, _displacement, _directionCountdown, _timer
            };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_angle, address + 0xD4);
            Config.Stream.SetValue(_timerMax, address + 0xF4);
            Config.Stream.SetValue(_targetAngle, address + 0xF8);
            Config.Stream.SetValue(_displacement, address + 0xFC);
            Config.Stream.SetValue(_directionCountdown, address + 0x104);
            Config.Stream.SetValue(_timer, address + 0x154);
        }
    }



}
