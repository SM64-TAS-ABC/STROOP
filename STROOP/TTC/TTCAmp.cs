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

        public bool initializedYet;

        public TtcAmp(TtcRng rng) : base(rng)
        {
            initializedYet = false;
        }

        public override void update()
        {
            if (!initializedYet)
            {
                pollRNG();
                initializedYet = true;
            }
        }

        public override string ToString()
        {
            return id + OPENER + initializedYet + CLOSER;
        }
    }
}
