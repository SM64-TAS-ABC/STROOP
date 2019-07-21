using STROOP.Structs;
using STROOP.Structs.Configurations;
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

        public int _state;
        public int _angle;

        public TtcAmp(TtcRng rng, uint address) :
            this(
                rng: rng,
                state: Config.Stream.GetInt32(address + 0x14C),
                angle: Normalize(Config.Stream.GetInt32(address + 0xC8)))
        {
        }

        public TtcAmp(TtcRng rng) : this(rng, 0, 0)
        {
        }

        public TtcAmp(TtcRng rng, int state, int angle) : base(rng)
        {
            _state = state;
            _angle = angle;
        }

        public override void Update()
        {
            if (_state == 0)
            {
                _angle = PollRNG();
                _state = 2;
            }
            _angle = Normalize(_angle + 1024);
        }

        public override string ToString()
        {
            return _id + OPENER + _state + SEPARATOR + _angle + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>() { _state, _angle };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_state, address + 0x14C);
            Config.Stream.SetValue(_angle, address + 0xC8);
        }
    }
}
