using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Extensions
{
    public static class ProcessStreamExtensions
    {
        public static byte GetByte(this ProcessStream stream, uint address, bool absoluteAddress = false)
        {
            return stream.ReadRam(address, 1, absoluteAddress)[0];
        }

        public static sbyte GetSByte(this ProcessStream stream, uint address, bool absoluteAddress = false)
        {
            return (sbyte)stream.ReadRam(address, 1, absoluteAddress)[0];
        }

        public static short GetInt16(this ProcessStream stream, uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToInt16(stream.ReadRam(address, 2, absoluteAddress), 0);
        }

        public static ushort GetUInt16(this ProcessStream stream, uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToUInt16(stream.ReadRam(address, 2, absoluteAddress), 0);
        }

        public static int GetInt32(this ProcessStream stream, uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToInt32(stream.ReadRam(address, 4, absoluteAddress), 0);
        }

        public static uint GetUInt32(this ProcessStream stream, uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToUInt32(stream.ReadRam(address, 4, absoluteAddress), 0);
        }

        public static float GetSingle(this ProcessStream stream, uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToSingle(stream.ReadRam(address, 4, absoluteAddress), 0);
        }
    }
}
