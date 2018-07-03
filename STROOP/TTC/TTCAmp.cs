using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** Amp is the electric metal ball enemy that goes in a circle.
     *  He only calls RNG once when he is first initialized.
     *  After that, he no longer calls RNG.
     */
    public class TtcAmp : TtcObject
    {

        public bool _initializedYet;

        public TtcAmp(TtcRng rng) : base(rng)
        {
            _initializedYet = false;
        }

        public override void update()
        {
            if (!_initializedYet)
            {
                pollRNG();
                _initializedYet = true;
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _initializedYet + CLOSER;
        }
    }
}
