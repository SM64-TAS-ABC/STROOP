using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A spinner is the rectangle platform that spins about
      *  a horizontal axis (i.e. changes its pitch).
      *  They are mostly found near the red coins.
      *  
      *  When a spinner completes a spin, it calls
      *  RNG to determine which direction it should rotate next
      *  as well as how long the rotation should be.
      *  Then for 5 frames, the spinner remains still.
      *  Then for max-5 frames, the spinner spins in its
      *  intended direction.
      *  Then for 1 frame, the spinner spins counterclockwise.
      */
    public class TtcSpinner : TtcObject
    {

        public int _angle;
        public int _direction; //1 = CCW, -1 = CW
        public int _timerMax;
        public int _timer;

        public TtcSpinner(TtcRng rng, uint address) :
            this(
                rng: rng,
                angle: Normalize(Config.Stream.GetInt32(address + 0xD0)),
                direction: Config.Stream.GetInt32(address + 0xF4),
                timerMax: Config.Stream.GetInt32(address + 0xF8),
                timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcSpinner(TtcRng rng) : this(rng, 0, 0, 0, 0)
        {
        }

        public TtcSpinner(TtcRng rng, int angle, int direction, int timerMax, int timer) : base(rng)
        {
            _angle = angle;
            _direction = direction;
            _timerMax = timerMax;
            _timer = timer;
        }

        public override void Update()
        {

            if (_timer <= _timerMax)
            { //spin normal
                if (_timer <= 5)
                { //don't spin
                    _timer++;
                }
                else
                { //spin
                    _angle += _direction * 200;
                    _angle = Normalize(_angle);
                    _timer++;
                }
            }
            else
            { //start a new spin
              //do a CCW spin
                _angle += 200;
                _angle = Normalize(_angle);

                //calculate new spin
                _direction = (PollRNG() <= 32766) ? -1 : 1; // = -1, 1
                _timerMax = (PollRNG() % 4) * 30 + 30; // = 30, 60, 90, 120
                _timer = 0;
                _timer++;
            }

        }

        public override string ToString()
        {
            return _id + OPENER + _angle + SEPARATOR +
                    _direction + SEPARATOR +
                    _timerMax + SEPARATOR +
                    _timer + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>() { _angle, _direction, _timerMax, _timer };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_angle, address + 0xD0);
            Config.Stream.SetValue(_direction, address + 0xF4);
            Config.Stream.SetValue(_timerMax, address + 0xF8);
            Config.Stream.SetValue(_timer, address + 0x154);
        }
    }


}
