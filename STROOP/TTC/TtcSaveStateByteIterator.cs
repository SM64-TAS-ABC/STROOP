using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    public class TtcSaveStateByteIterator
    {
        private readonly List<byte> _bytes;
        private int index;

        public TtcSaveStateByteIterator(List<byte> bytes)
        {
            _bytes = bytes;
            index = 0;
        }

        public int GetInt()
        {
            int value = (int)TypeUtilities.ConvertBytes(typeof(int), _bytes.ToArray(), index, false);
            index += TypeUtilities.TypeSize[typeof(int)];
            return value;
        }

        public ushort GetUShort()
        {
            ushort value = (ushort)TypeUtilities.ConvertBytes(typeof(ushort), _bytes.ToArray(), index, false);
            index += TypeUtilities.TypeSize[typeof(ushort)];
            return value;
        }

        public bool IsDone()
        {
            return index == _bytes.Count;
        }
    }
}
