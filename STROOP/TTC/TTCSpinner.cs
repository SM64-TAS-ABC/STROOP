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
        public int _max;
        public int _counter;

        public TtcSpinner(TtcRng rng) : base(rng)
        {
            _angle = 0;
            _direction = 0;
            _max = 0;
            _counter = 0;
        }

        public override void update()
        {

            if (_counter <= _max)
            { //spin normal
                if (_counter <= 5)
                { //don't spin
                    _counter++;
                }
                else
                { //spin
                    _angle += _direction * 200;
                    _angle = normalize(_angle);
                    _counter++;
                }
            }
            else
            { //start a new spin
              //do a CCW spin
                _angle += 200;
                _angle = normalize(_angle);

                //calculate new spin
                _direction = (pollRNG() <= 32766) ? -1 : 1; // = -1, 1
                _max = (pollRNG() % 4) * 30 + 30; // = 30, 60, 90, 120
                _counter = 0;
                _counter++;
            }

        }

        public override string ToString()
        {
            return _id + OPENER + _angle + SEPARATOR +
                    _direction + SEPARATOR +
                    _max + SEPARATOR +
                    _counter + CLOSER;
        }

    }


}
