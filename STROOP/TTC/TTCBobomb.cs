using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A bob-omb is the black bomb enemy. There are
    *  two of them in TTC, near the start of the course.
    *  
    *  A bob-omb calls RNG every frame to determine whether
    *  it should blink its eyes. If it does blink, then it blinks
    *  for 16 frames, during which it won't call RNG.
    *  
    *  A bob-omb will only update when Mario is within 4000
    *  units of the bob-omb. Note that this is not synonymous
    *  with whether the bob-omb is visible or not, which is
    *  dictated by a radius smaller than 4000. In other words,
    *  for certain distances (like 3500), the bob-omb will
    *  not be visible, but will still update and call RNG
    *  just as normal.
    */
    public class TtcBobomb : TtcObject
    {

        //how deep into the blink the bob-omb is
        //this variable is 0 when the bob-omb is not blinking
        public int _blinkingTimer;

        //whether Mario is within 4000 units of the bob-omb
        public int _withinMarioRange;

        public TtcBobomb(TtcRng rng, uint address) :
            this(
                rng: rng,
                blinkingTimer: Config.Stream.GetInt32(address + 0xF4),
                withinMarioRange: PositionAngle.GetDistance(PositionAngle.Obj(address), PositionAngle.Mario) < 4000 ? 1 : 0)
        {
        }

        public TtcBobomb(TtcRng rng) : this(rng, 0, 1)
        {
        }

        public TtcBobomb(TtcRng rng, int blinkingTimer, int withinMarioRange) : base(rng)
        {
            _blinkingTimer = blinkingTimer;
            _withinMarioRange = withinMarioRange;
        }

        public override void Update()
        {
            //don't update at all if not within mario range
            if (_withinMarioRange == 0) return;

            if (_blinkingTimer > 0)
            { //currently blinking
                _blinkingTimer = (_blinkingTimer + 1) % 16;
            }
            else
            { //not currently blinking
                if (PollRNG() <= 655)
                {
                    _blinkingTimer++;
                }
            }
        }

        /** Change whether Mario is within the bob-omb's
	     *  4000 unit radius.
	     */
        public void SetWithinMarioRange(int withinMarioRange)
        {
            _withinMarioRange = withinMarioRange;
        }

        public override string ToString()
        {
            return _id + OPENER + _blinkingTimer + SEPARATOR + _withinMarioRange + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>() { _blinkingTimer, _withinMarioRange };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_blinkingTimer, address + 0xF4);
        }
    }

}
