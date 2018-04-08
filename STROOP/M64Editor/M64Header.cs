using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using STROOP.Structs;

namespace STROOP.M64Editor
{
    [Serializable]
    public class M64Header
    {
        public static readonly int HeaderSize = 0x400;

        // 000 4-byte signature: 4D 36 34 1A "M64\x1A"
        public uint Signature;

        // 004 4-byte little-endian unsigned int: version number, should be 3
        public uint VersionNumber;

        // 008 4-byte little-endian integer: movie "uid" - identifies the movie-savestate relationship,
        // also used as the recording time in Unix epoch format
        public int Uid;

        // 00C 4-byte little-endian unsigned int: number of frames(vertical interrupts)
        public int Vis;

        // 010 4-byte little-endian unsigned int: rerecord count
        public int Rerecords;

        // 014 1-byte unsigned int: frames(vertical interrupts) per second
        public byte Fps;

        // 015 1-byte unsigned int: number of controllers
        public byte NumControllers;

        // 016 2-byte unsigned int: reserved, should be 0

        // 018 4-byte little-endian unsigned int: number of input samples for any controllers
        public int Inputs;

        // 01C 2-byte unsigned int: movie start type
        // value 1: movie begins from snapshot(the snapshot will be loaded from an externalfile
        //     with the movie filename and a .st extension)
        // value 2: movie begins from power-on
        // other values: invalid movie
        public short MovieStartType;

        // 01E 2-byte unsigned int: reserved, should be 0

        // 020 4-byte unsigned int: controller flags
        // bit 0: controller 1 present
        // bit 4: controller 1 has mempak
        // bit 8: controller 1 has rumblepak
        // +1..3 for controllers 2..4.
        public bool Controller1Present;
        public bool Controller1MemPak;
        public bool Controller1RumblePak;
        public bool Controller2Present;
        public bool Controller2MemPak;
        public bool Controller2RumblePak;
        public bool Controller3Present;
        public bool Controller3MemPak;
        public bool Controller3RumblePak;
        public bool Controller4Present;
        public bool Controller4MemPak;
        public bool Controller4RumblePak;

        // 024 160 bytes: reserved, should be 0

        // 0C4 32-byte ASCII string: internal name of ROM used when recording, directly from ROM
        public string RomName;

        // 0E4 4-byte unsigned int: CRC32 of ROM used when recording, directly from ROM
        public uint Cr32;

        // 0E8 2-byte unsigned int: country code of ROM used when recording, directly from ROM
        public ushort CountryCode;

        // 0EA 56 bytes: reserved, should be 0

        // 122 64-byte ASCII string: name of video plugin used when recording, directly from plugin
        public string VideoPlugin;

        // 162 64-byte ASCII string: name of sound plugin used when recording, directly from plugin
        public string SoundPlugin;

        // 1A2 64-byte ASCII string: name of input plugin used when recording, directly from plugin
        public string InputPlugin;

        // 1E2 64-byte ASCII string: name of rsp plugin used when recording, directly from plugin
        public string RspPlugin;

        // 222 222-byte UTF-8 string: author name info
        public string Author;

        // 300 256-byte UTF-8 string: author movie description info
        public string Description;

        public M64Header(byte[] bytes)
        {
            if (bytes.Length != HeaderSize) throw new ArgumentOutOfRangeException();


        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(TypeUtilities.GetBytes(Signature));
            bytes.AddRange(TypeUtilities.GetBytes(VersionNumber));
            bytes.AddRange(TypeUtilities.GetBytes(Uid));
            bytes.AddRange(TypeUtilities.GetBytes(Vis));
            bytes.AddRange(TypeUtilities.GetBytes(Rerecords));
            bytes.AddRange(TypeUtilities.GetBytes(Fps));
            bytes.AddRange(TypeUtilities.GetBytes(NumControllers));
            bytes.AddRange(TypeUtilities.GetBytes(new byte[2]));
            bytes.AddRange(TypeUtilities.GetBytes(Inputs));
            bytes.AddRange(TypeUtilities.GetBytes(MovieStartType));
            bytes.AddRange(TypeUtilities.GetBytes(new byte[2]));
            bytes.AddRange(TypeUtilities.GetBytes(GetControllerFlagsValue()));
            bytes.AddRange(TypeUtilities.GetBytes(new byte[160]));
            bytes.AddRange(TypeUtilities.GetBytes(RomName, 32, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(Cr32));
            bytes.AddRange(TypeUtilities.GetBytes(CountryCode));
            bytes.AddRange(TypeUtilities.GetBytes(new byte[160]));
            bytes.AddRange(TypeUtilities.GetBytes(VideoPlugin, 64, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(SoundPlugin, 64, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(InputPlugin, 64, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(RspPlugin, 64, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(Author, 222, Encoding.UTF8));
            bytes.AddRange(TypeUtilities.GetBytes(Description, 256, Encoding.UTF8));
            if (bytes.Count != HeaderSize) throw new ArgumentOutOfRangeException();
            return bytes.ToArray();
        }

        private uint GetControllerFlagsValue()
        {
            uint flags = 0;
            uint currentBit = 1;
            foreach (bool boolValue in GetControllerBoolList())
            {
                if (boolValue) flags |= currentBit;
                currentBit <<= 1;
            }
            return flags;
        }

        private List<bool> GetControllerBoolList()
        {
            return new List<bool>()
            {
                Controller1Present,
                Controller2Present,
                Controller3Present,
                Controller4Present,
                Controller1MemPak,
                Controller2MemPak,
                Controller3MemPak,
                Controller4MemPak,
                Controller1RumblePak,
                Controller2RumblePak,
                Controller3RumblePak,
                Controller4RumblePak,
            };
        }

        /*
        public int FrameIndex { get => Index; }

        public bool A { get => GetBit(7); set => SetBit(7, value); }
        public bool B { get => GetBit(6); set => SetBit(6, value); }
        public bool Z { get => GetBit(5); set => SetBit(5, value); }
        public bool Start { get => GetBit(4); set => SetBit(4, value); }
        public bool L { get => GetBit(13); set => SetBit(13, value); }
        public bool R { get => GetBit(12); set => SetBit(12, value); }
        public sbyte AnalogX { get => (sbyte)GetByte(2); set => SetByte(2, (byte)value); }
        public sbyte AnalogY { get => (sbyte)GetByte(3); set => SetByte(3, (byte)value); }
        public bool C_Up { get => GetBit(11); set => SetBit(11, value); }
        public bool C_Down { get => GetBit(10); set => SetBit(10, value); }
        public bool C_Left { get => GetBit(9); set => SetBit(9, value); }
        public bool C_Right { get => GetBit(8); set => SetBit(8, value); }
        public bool D_Up { get => GetBit(3); set => SetBit(3, value); }
        public bool D_Down { get => GetBit(2); set => SetBit(2, value); }
        public bool D_Left { get => GetBit(1); set => SetBit(1, value); }
        public bool D_Right { get => GetBit(0); set => SetBit(0, value); }

        private void SetByte(int num, byte value)
        {
            uint mask = ~(uint)(0xFF << (num * 8));
            RawValue = ((uint)(value << (num * 8)) | (RawValue & mask));
        }

        private byte GetByte(int num)
        {
            return (byte)(RawValue >> (num * 8));
        }

        private void SetBit(int bit, bool value)
        {
            uint mask = (uint)(1 << bit);
            if (value)
            {
                RawValue |= mask;
            }
            else
            {
                RawValue &= ~mask;
            }
        }

        private bool GetBit(int bit)
        {
            return ((RawValue >> bit) & 0x01) == 0x01;
        }
        */
    }
}
