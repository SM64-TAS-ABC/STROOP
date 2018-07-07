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
            return (int)TypeUtilities.ConvertBytes(
                typeof(int),
                GetBytes(TypeUtilities.TypeSize[typeof(int)]));
        }

        public ushort GetUShort()
        {
            return (ushort)TypeUtilities.ConvertBytes(
                typeof(ushort),
                GetBytes(TypeUtilities.TypeSize[typeof(ushort)]));
        }

        private byte[] GetBytes(int numBytes)
        {
            byte[] bytes = _bytes.Skip(index).Take(numBytes).ToArray();
            index += numBytes;
            return bytes;
        }

        public bool IsDone()
        {
            return index == _bytes.Count;
        }
    }
}
