using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** A pusher is rectangular prism that extends from and
     *  retracts into the wall. They are referred to as
     *  "moving bars" in the star Timed Jumps on Moving Bars.
     *  
     *  A pusher beings flush with the wall (i.e. its outer surface
     *  is flush with the wall). It waits in this state for some
     *  amount of time, dictated by RNG. Then it retracts deeper
     *  into the wall, poised to eject, and waits in this state
     *  for some amount of time, dictated by RNG. Then it extends
     *  out, and calls RNG to determine whether it should extend out
     *  all the way out or stop flush with the wall. If it extends all
     *  the way out, then it eventually comes to a stop and then
     *  retracts until it's flush with the wall.
     */
    public class TtcPusher : TtcObject
    {

        public int _timerMax;
        public int _countdown;
        public int _state; //0 = flush with wall, 1 = retracted, 2 = extending, 3 = retracting
        public int _timer;

        public TtcPusher(TtcRng rng, uint address) :
            this(
                rng: rng,
                timerMax: Config.Stream.GetInt32(address + 0xF4),
                countdown: Config.Stream.GetInt32(address + 0xF8),
                state: Config.Stream.GetInt32(address + 0x14C),
                timer: Config.Stream.GetInt32(address + 0x154))
        {
        }

        public TtcPusher(TtcRng rng, int countdown) :
            this(rng, 55, countdown, 0, 0)
        {
        }

        public TtcPusher(TtcRng rng, int timerMax, int countdown, int state, int timer) : base(rng)
        {
            _timerMax = timerMax;
            _countdown = countdown;
            _state = state;
            _timer = timer;
        }

        public override void Update()
        {
            if (_state == 0)
            { //flush with wall
                if (_timer <= _timerMax)
                {
                    _timer++;
                }
                else if (_countdown > 0)
                {
                    _countdown--;
                    _timer++;
                }
                else
                {
                    int rand = PollRNG();
                    if (rand % 4 == 0) _timerMax = 1;
                    if (rand % 4 == 1) _timerMax = 12;
                    if (rand % 4 == 2) _timerMax = 55;
                    if (rand % 4 == 3) _timerMax = 100;

                    // countdown = 0 or [20,120)
                    if (PollRNG() % 2 == 0)
                    {
                        _countdown = (int)(PollRNG() / 65536.0 * 100 + 20); // = [20,120)
                    }

                    _state = 1;
                    _timer = 0;
                }
            }
            else if (_state == 1)
            { //retracted
                if (_timer < 10)
                { //waiting
                    _timer++;
                }
                else
                {
                    if (_countdown > 0)
                    { //moving back in
                        _countdown--;
                        _timer++;
                    }
                    else
                    { //moving back in
                        _state = 2;
                        _timer = 0;
                    }
                }
            }
            else if (_state == 2)
            { //extending
                if (_timer == 0)
                { //wait one frame
                    _timer++;
                }
                else if (_timer == 1)
                { //either extend out or fake it
                    if (PollRNG() % 4 == 0)
                    { //fake extend
                        _state = 0;
                        _timer = 0;
                    }
                    else
                    { //actually extend
                        _timer++;
                    }
                }
                else if (_timer < 36)
                { //continue extending out
                    _timer++;
                }
                else
                { //finished extending out
                    _state = 3;
                    _timer = 0;
                }
            }
            else
            { //retracting
                if (_timer < 82)
                { //still retracting
                    _timer++;
                }
                else
                { //finished retracting
                    _state = 0;
                    _timer = 0;
                }
            }
        }

        public override string ToString()
        {
            return _id + OPENER + _timerMax + SEPARATOR +
                          _countdown + SEPARATOR +
                          _state + SEPARATOR +
                          _timer + CLOSER;
        }

        public override List<object> GetFields()
        {
            return new List<object>() { _timerMax, _countdown, _state, _timer };
        }

        public override void ApplyToAddress(uint address)
        {
            Config.Stream.SetValue(_timerMax, address + 0xF4);
            Config.Stream.SetValue(_countdown, address + 0xF8);
            Config.Stream.SetValue(_state, address + 0x14C);
            Config.Stream.SetValue(_timer, address + 0x154);
        }

        public bool IsExtended()
        {
            return _state == 2 || (_state == 3 && _timer < 33);
        }

        public bool IsRetracting()
        {
            return _state == 3 && _timer >= 33;
        }
    }

}
