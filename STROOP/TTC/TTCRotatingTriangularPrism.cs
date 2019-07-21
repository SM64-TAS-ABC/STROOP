using STROOP.Structs.Configurations;
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

        public int _timerMax;
        public int _timer;

        public TtcRotatingTriangularPrism(TtcRng rng, uint address) :
            this(
                rng: rng,
                timerMax: Config.Stream.GetInt32(address + 0xFC),
                timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcRotatingTriangularPrism(TtcRng rng) : this(rng, 0, 0)
        {
        }

        public TtcRotatingTriangularPrism(TtcRng rng, int timerMax, int timer) : base(rng)
        {
            _timerMax = timerMax;
            _timer = timer;
        }

        public override void Update()
        {
            if (_timer < _timerMax + TURNING_TIME)
            { //waiting
                _timer++;
            }
            else
            { //done waiting
                _timerMax = (PollRNG() % 7) * 20 + 5; // = 5, 25, 45, 65, 85, 105, 125
                _timer = 0;
                _timer++;
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _timerMax + SEPARATOR +
                    _timer + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>() { _timerMax, _timer };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_timerMax, address + 0xFC);
            Config.Stream.SetValue(_timer, address + 0x154);
        }
    }

}
