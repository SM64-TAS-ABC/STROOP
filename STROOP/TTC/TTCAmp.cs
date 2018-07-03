using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.TTC
{
    /** Amp is the electric metal ball enemy that goes in a circle.
     *  He only calls RNG once when he is first initialized.
     *  After that, he no longer calls RNG.
     */
    public class TTCAmp : TTCObject
    {

        public bool initializedYet;

        public TTCAmp()
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
