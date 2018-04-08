using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using STROOP.Structs;
using System.ComponentModel;
using STROOP.Utilities;

namespace STROOP.M64Editor
{
    public class M64Header
    {
        public static readonly int HeaderSize = 0x400;

        public enum MovieStartTypeEnum { FromStart, FromSnapshot }

        // 018 4-byte little-endian unsigned int: number of input samples for any controllers
        [Category("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("\u200B\u200B\u200B\u200BInputs")]
        public int Inputs { get; set; }

        // 00C 4-byte little-endian unsigned int: number of frames(vertical interrupts)
        [Category("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("\u200B\u200B\u200BVIs")]
        public int Vis { get; set; }

        // 010 4-byte little-endian unsigned int: rerecord count
        [CategoryAttribute("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("\u200B\u200BRerecords")]
        public int Rerecords { get; set; }

        // 01C 2-byte unsigned int: movie start type
        // value 1: movie begins from snapshot(the snapshot will be loaded from an externalfile
        //     with the movie filename and a .st extension)
        // value 2: movie begins from power-on
        // other values: invalid movie
        [CategoryAttribute("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("\u200BMovie Start Type")]
        public MovieStartTypeEnum MovieStartType { get; set; }

        // 014 1-byte unsigned int: frames(vertical interrupts) per second
        [CategoryAttribute("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("FPS")]
        public byte Fps { get; set; }

        // 0C4 32-byte ASCII string: internal name of ROM used when recording, directly from ROM
        [CategoryAttribute("\u200B\u200B\u200B\u200BRom"), DisplayName("\u200B\u200BRom Name")]
        public string RomName { get; set; }

        // 0E8 2-byte unsigned int: country code of ROM used when recording, directly from ROM
        [CategoryAttribute("\u200B\u200B\u200B\u200BRom"), DisplayName("\u200BCountry Code")]
        public ushort CountryCode { get; set; }

        // 0E4 4-byte unsigned int: CRC32 of ROM used when recording, directly from ROM
        [CategoryAttribute("\u200B\u200B\u200B\u200BRom"), DisplayName("CR32")]
        public uint Cr32 { get; set; }

        // 222 222-byte UTF-8 string: author name info
        [CategoryAttribute("\u200B\u200B\u200BDescription"), DisplayName("\u200BAuthor")]
        public string Author { get; set; }

        // 300 256-byte UTF-8 string: author movie description info
        [CategoryAttribute("\u200B\u200B\u200BDescription"), DisplayName("Description")]
        public string Description { get; set; }

        // 015 1-byte unsigned int: number of controllers
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200B\u200BNum Controllers")]
        public byte NumControllers { get; set; }

        // 020 4-byte unsigned int: controller flags
        // bit 0: controller 1 present
        // bit 4: controller 1 has mempak
        // bit 8: controller 1 has rumblepak
        // +1..3 for controllers 2..4.
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200BController 1 Present")]
        public bool Controller1Present { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200BController 2 Present")]
        public bool Controller2Present { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200BController 3 Present")]
        public bool Controller3Present { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200BController 4 Present")]
        public bool Controller4Present { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200BController 1 MemPak")]
        public bool Controller1MemPak { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200BController 2 MemPak")]
        public bool Controller2MemPak { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200BController 3 MemPak")]
        public bool Controller3MemPak { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200BController 4 MemPak")]
        public bool Controller4MemPak { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("Controller 1 RumblePak")]
        public bool Controller1RumblePak { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("Controller 2 RumblePak")]
        public bool Controller2RumblePak { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("Controller 3 RumblePak")]
        public bool Controller3RumblePak { get; set; }
        [CategoryAttribute("\u200B\u200BController"), DisplayName("Controller 4 RumblePak")]
        public bool Controller4RumblePak { get; set; }

        // 122 64-byte ASCII string: name of video plugin used when recording, directly from plugin
        [CategoryAttribute("\u200BPlugin"), DisplayName("\u200B\u200B\u200BVideo Plugin")]
        public string VideoPlugin { get; set; }

        // 162 64-byte ASCII string: name of sound plugin used when recording, directly from plugin
        [CategoryAttribute("\u200BPlugin"), DisplayName("\u200B\u200BSound Plugin")]
        public string SoundPlugin { get; set; }

        // 1A2 64-byte ASCII string: name of input plugin used when recording, directly from plugin
        [CategoryAttribute("\u200BPlugin"), DisplayName("\u200BInput Plugin")]
        public string InputPlugin { get; set; }

        // 1E2 64-byte ASCII string: name of rsp plugin used when recording, directly from plugin
        [CategoryAttribute("\u200BPlugin"), DisplayName("RSP Plugin")]
        public string RspPlugin { get; set; }

        // 000 4-byte signature: 4D 36 34 1A "M64\x1A"
        [CategoryAttribute("Mupen"), DisplayName("\u200B\u200BSignature")]
        public uint Signature { get; set; }

        // 004 4-byte little-endian unsigned int: version number, should be 3
        [CategoryAttribute("Mupen"), DisplayName("\u200BVersion Number")]
        public uint VersionNumber { get; set; }

        // 008 4-byte little-endian integer: movie "uid" - identifies the movie-savestate relationship,
        // also used as the recording time in Unix epoch format
        [CategoryAttribute("Mupen"), DisplayName("UID")]
        public int Uid { get; set; }

        public M64Header()
        {

        }

        public void LoadBytes(byte[] bytes)
        {
            if (bytes.Length != HeaderSize) throw new ArgumentOutOfRangeException();

            Signature = BitConverter.ToUInt32(bytes, 0x000);
            VersionNumber = BitConverter.ToUInt32(bytes, 0x004);
            Uid = BitConverter.ToInt32(bytes, 0x008);
            Vis = BitConverter.ToInt32(bytes, 0x00C);
            Rerecords = BitConverter.ToInt32(bytes, 0x010);
            Fps = bytes[0x014];
            NumControllers = bytes[0x015];
            Inputs = BitConverter.ToInt32(bytes, 0x018);

            short movieStartTypeShort = BitConverter.ToInt16(bytes, 0x01C);
            MovieStartType = ConvertShortToMovieStartTypeEnum(movieStartTypeShort);

            uint controllerFlagsValue = BitConverter.ToUInt16(bytes, 0x020);
            Controller1Present = (controllerFlagsValue & (1 << 0)) != 0;
            Controller2Present = (controllerFlagsValue & (1 << 1)) != 0;
            Controller3Present = (controllerFlagsValue & (1 << 2)) != 0;
            Controller4Present = (controllerFlagsValue & (1 << 3)) != 0;
            Controller1MemPak = (controllerFlagsValue & (1 << 4)) != 0;
            Controller2MemPak = (controllerFlagsValue & (1 << 5)) != 0;
            Controller3MemPak = (controllerFlagsValue & (1 << 6)) != 0;
            Controller4MemPak = (controllerFlagsValue & (1 << 7)) != 0;
            Controller1RumblePak = (controllerFlagsValue & (1 << 8)) != 0;
            Controller2RumblePak = (controllerFlagsValue & (1 << 9)) != 0;
            Controller3RumblePak = (controllerFlagsValue & (1 << 10)) != 0;
            Controller4RumblePak = (controllerFlagsValue & (1 << 11)) != 0;

            RomName = Encoding.ASCII.GetString(bytes, 0x0C4, 32).Replace("\0", "");
            Cr32 = BitConverter.ToUInt32(bytes, 0x0E4);
            CountryCode = BitConverter.ToUInt16(bytes, 0x0E8);
            VideoPlugin = Encoding.ASCII.GetString(bytes, 0x122, 64).Replace("\0", "");
            SoundPlugin = Encoding.ASCII.GetString(bytes, 0x162, 64).Replace("\0", "");
            InputPlugin = Encoding.ASCII.GetString(bytes, 0x1A2, 64).Replace("\0", "");
            RspPlugin = Encoding.ASCII.GetString(bytes, 0x1E2, 64).Replace("\0", "");
            Author = Encoding.UTF8.GetString(bytes, 0x222, 222).Replace("\0", "");
            Description = Encoding.UTF8.GetString(bytes, 0x300, 256).Replace("\0", "");

            // Verify that serialization works correctly
            if (!Enumerable.SequenceEqual(bytes, ToBytes())) throw new ArgumentOutOfRangeException();
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
            bytes.AddRange(new byte[2]);
            bytes.AddRange(TypeUtilities.GetBytes(Inputs));
            bytes.AddRange(TypeUtilities.GetBytes(ConvertMovieStartTypeEnumToShort(MovieStartType)));
            bytes.AddRange(new byte[2]);
            bytes.AddRange(TypeUtilities.GetBytes(GetControllerFlagsValue()));
            bytes.AddRange(new byte[160]);
            bytes.AddRange(TypeUtilities.GetBytes(RomName, 32, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(Cr32));
            bytes.AddRange(TypeUtilities.GetBytes(CountryCode));
            bytes.AddRange(new byte[56]);
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

        private short ConvertMovieStartTypeEnumToShort(MovieStartTypeEnum movieStartType)
        {
            switch (movieStartType)
            {
                case MovieStartTypeEnum.FromStart:
                    return 2;
                case MovieStartTypeEnum.FromSnapshot:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private MovieStartTypeEnum ConvertShortToMovieStartTypeEnum(short shortValue)
        {
            switch (shortValue)
            {
                case 1:
                    return MovieStartTypeEnum.FromSnapshot;
                case 2:
                    return MovieStartTypeEnum.FromStart;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
