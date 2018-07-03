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

        public TtcAmp(TtcRng rng, uint address) : this(rng, false)
        {
        }

        public TtcAmp(TtcRng rng) : this(rng, false)
        {
        }

        public TtcAmp(TtcRng rng, bool initializedYet) : base(rng)
        {
            _initializedYet = initializedYet;
        }

        public override void Update()
        {
            if (!_initializedYet)
            {
                PollRNG();
                _initializedYet = true;
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _initializedYet + CLOSER;
        }
    }
}
