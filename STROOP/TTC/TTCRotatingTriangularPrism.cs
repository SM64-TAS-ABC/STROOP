using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** Rotating block is the triangular prism that rotates around a
     *  horizontal axis (i.e. changes its pitch). They look like
     *  and function like the rotating blocks.
     *  
     *  When it completes a rotation, it calls RNG to determine
     *  how long it should wait until the next rotation.
     *  Once it has waited this long, it begins rotating
     *  and the process repeats.
     */
    public class TtcRotatingTriangularPrism : TtcObject
    {

        //the turning time for rotating triangular prisms
        public static readonly int TURNING_TIME = 45;

        public int _max;
        public int _timer;

        public TtcRotatingTriangularPrism(TtcRng rng) : base(rng)
        {
            _max = 0;
            _timer = 0;
        }

        public override void Update()
        {
            if (_timer < _max + TURNING_TIME)
            { //waiting
                _timer++;
            }
            else
            { //done waiting
                _max = (PollRNG() % 7) * 20 + 5; // = 5, 25, 45, 65, 85, 105, 125
                _timer = 0;
                _timer++;
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _max + SEPARATOR +
                    _timer + CLOSER;
        }

    }

}
