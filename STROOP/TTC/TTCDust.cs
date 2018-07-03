using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.TTC
{
    /** Dust is the dust created at Mario's feet when he accelerates
    *  or decelerates or does any other dust making technique.
    *  
    *  Dust calls RNG 4 times when it spawns. To simulate this
    *  with this class, simply use the addDustFrames method
    *  to specify which frames dust is present on.
    *  
    *  For example, if you hold forwards when the frame counter
    *  says 98 and then press frame advance, then on frame 99
    *  Mario will still be motionless. It's not until frame 100
    *  that Mario moves and the dust is present. So in that case,
    *  you'd add 100 as a dust frame.
    */
    public class TTCDust : TTCObject
    {

        public List<int> dustFrames;

        public TTCDust()
        {
            dustFrames = new List<int>();
        }

        public override void update()
        {
            if (dustFrames.Contains(currentFrame))
            {
                pollRNG();
                pollRNG();
                pollRNG();
                pollRNG();
            }
        }

        public override string ToString()
        {
            return id + OPENER + dustFrames.Contains(currentFrame) + CLOSER;
        }

        /** Add an arbitrary number of dust frames.
	     */
        public void addDustFrames(params int[] frames)
        {
            foreach (int frame in frames)
            {
                dustFrames.Add(frame);
            }
        }

    }


}
