using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{

    public class TtcSaveState
    {

        private readonly List<byte> _bytes;

        public TtcSaveState() : this(TtcUtilities.CreateRngObjectsFromGame(new TtcRng(0)))
        {
        }

        public TtcSaveState(List<TtcObject> objects)
        {
            List<object> fields = objects.ConvertAll(obj => obj.GetFields())
                .SelectMany(list => list).ToList();
        }

        public TtcSaveState(string stringValue)
        {
            _bytes = new List<byte>();
            for (int i = 0; i < stringValue.Length; i += 2)
            {
                string substring = stringValue.Substring(i, 2);
                byte b = Convert.ToByte(substring, 16);
                _bytes.Add(b);
            }
        }

        public override string ToString()
        {
            List<string> byteStrings = _bytes.ConvertAll(b => HexUtilities.FormatValue(b, 2, false));
            return String.Join("", byteStrings);
        }

    }


}
