using STROOP.Structs;
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

        public TtcSaveState() : this(
            Config.Stream.GetUInt16(MiscConfig.RngAddress),
            TtcUtilities.CreateRngObjectsFromGame(new TtcRng(Config.Stream.GetUInt16(MiscConfig.RngAddress))))
        {
        }

        public TtcSaveState(ushort rng, List<TtcObject> objects)
        {
            List<byte> rngBytes = TypeUtilities.GetBytes(rng).ToList();
            List<object> fields = objects.SelectMany(obj => obj.GetFields()).ToList();
            List<byte> fieldBytes = fields.SelectMany(field => TypeUtilities.GetBytes(field)).ToList();
            _bytes = rngBytes.Concat(fieldBytes).ToList();
        }

        public TtcSaveState(string saveStateString)
        {
            saveStateString = saveStateString.Trim();
            _bytes = new List<byte>();
            for (int i = 0; i < saveStateString.Length; i += 2)
            {
                string substring = saveStateString.Substring(i, 2);
                byte b = Convert.ToByte(substring, 16);
                _bytes.Add(b);
            }
        }

        public override string ToString()
        {
            List<string> byteStrings = _bytes.ConvertAll(b => HexUtilities.FormatValue(b, 2, false));
            return String.Join("", byteStrings);
        }

        public ushort GetRng()
        {
            return GetIterator().GetUShort();
        }

        public TtcSaveStateByteIterator GetIterator()
        {
            return new TtcSaveStateByteIterator(_bytes);
        }

    }


}
