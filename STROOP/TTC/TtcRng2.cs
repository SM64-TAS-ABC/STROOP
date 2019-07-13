using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    public class TtcRng2 : TtcRng
    {
        private int _counter;

        public TtcRng2() : base(0)
        {
            _counter = 0;
        }

        public override ushort PollRNG()
        {
            return Function1();
        }

        private ushort Function1()
        {
            ushort returnValue;
            switch (_counter)
            {
                case 0:
                    returnValue = 1;
                    break;
                case 1:
                    returnValue = 1;
                    break;
                case 2:
                    returnValue = 0;
                    break;
                case 3:
                    returnValue = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _counter = (_counter + 1) % 4;
            return returnValue;
        }
    }
}
