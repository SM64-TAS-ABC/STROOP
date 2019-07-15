using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
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
    public class TtcDust : TtcObject
    {

        public List<int> _dustFrames;

        public TtcDust(TtcRng rng) : base(rng)
        {
            _dustFrames = new List<int>();
        }

        public override void Update()
        {
            if (_dustFrames.Contains(_currentFrame))
            {
                PollRNG();
                PollRNG();
                PollRNG();
                PollRNG();
            }
        }

        public int GetMaxDustFrame()
        {
            if (_dustFrames.Count == 0) return 0;
            return _dustFrames.Max();
        }

        public override string ToString()
        {
            return _id + OPENER + _dustFrames.Contains(_currentFrame) + CLOSER;
        }

        /** Add an arbitrary number of dust frames.
	     */
        public void AddDustFrames(List<int> dustFrames)
        {
            _dustFrames.AddRange(dustFrames);
        }

        public override List<object> GetFields()
        {
            return new List<object>() { };
        }

    }


}
