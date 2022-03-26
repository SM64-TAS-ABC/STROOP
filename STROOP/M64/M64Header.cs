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
using STROOP.Structs.Gui;
using STROOP.Forms;

namespace STROOP.M64
{
    public class M64Header
    {
        public enum MovieStartTypeEnum { FromStart, FromSnapshot, FromEEPROM }

        private readonly M64File _m64File;
        private readonly M64Gui _gui;

        // 018 4-byte little-endian unsigned int: number of input samples for any controllers
        private int _numInputs;
        [Category("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("\u200B\u200B\u200B\u200BNum Inputs")]
        public int NumInputs { get => _numInputs; set { _numInputs = value; NotifyChange(); } }

        // 00C 4-byte little-endian unsigned int: number of frames(vertical interrupts)
        private int _numVis;
        [Category("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("\u200B\u200B\u200BNum VIs")]
        public int NumVis { get => _numVis; set { _numVis = value; NotifyChange(); } }

        // 010 4-byte little-endian unsigned int: rerecord count
        private int _numRerecords;
        [CategoryAttribute("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("\u200B\u200BNum Rerecords")]
        public int NumRerecords { get => _numRerecords; set { _numRerecords = value; NotifyChange(); } }

        // 01C 2-byte unsigned int: movie start type
        // value 1: movie begins from snapshot(the snapshot will be loaded from an externalfile
        //     with the movie filename and a .st extension)
        // value 2: movie begins from power-on
        // other values: invalid movie
        private MovieStartTypeEnum _movieStartType;
        [CategoryAttribute("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("\u200BMovie Start Type")]
        public MovieStartTypeEnum MovieStartType { get => _movieStartType; set { _movieStartType = value; NotifyChange(); } }

        // 014 1-byte unsigned int: frames(vertical interrupts) per second
        private byte _fps;
        [CategoryAttribute("\u200B\u200B\u200B\u200B\u200BMain"), DisplayName("FPS")]
        public byte Fps { get => _fps; set { _fps = value; NotifyChange(); } }

        // 0C4 32-byte ASCII string: internal name of ROM used when recording, directly from ROM
        private string _romName;
        [CategoryAttribute("\u200B\u200B\u200B\u200BRom"), DisplayName("\u200B\u200BRom Name")]
        public string RomName { get => _romName; set { _romName = StringUtilities.Cap(value, 32); NotifyChange(); } }

        // 0E8 2-byte unsigned int: country code of ROM used when recording, directly from ROM
        private ushort _countryCode;
        [CategoryAttribute("\u200B\u200B\u200B\u200BRom"), DisplayName("\u200BCountry Code")]
        public ushort CountryCode { get => _countryCode; set { _countryCode = value; NotifyChange(); } }

        // 0E4 4-byte unsigned int: CRC32 of ROM used when recording, directly from ROM
        private uint _crc32;
        [CategoryAttribute("\u200B\u200B\u200B\u200BRom"), DisplayName("CRC32")]
        public uint Crc32 { get => _crc32; set { _crc32 = value; NotifyChange(); } }

        // 222 222-byte UTF-8 string: author name info
        private string _author;
        [CategoryAttribute("\u200B\u200B\u200BDescription"), DisplayName("\u200BAuthor")]
        public string Author { get => _author; set { _author = StringUtilities.Cap(value, 222); NotifyChange(); } }

        // 300 256-byte UTF-8 string: author movie description info
        private string _description;
        [CategoryAttribute("\u200B\u200B\u200BDescription"), DisplayName("Description")]
        public string Description { get => _description; set { _description = StringUtilities.Cap(value, 256); NotifyChange(); } }

        // 015 1-byte unsigned int: number of controllers
        private byte _numControllers;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200B\u200BNum Controllers")]
        public byte NumControllers { get => _numControllers; set { _numControllers = value; NotifyChange(); } }

        // 020 4-byte unsigned int: controller flags
        // bit 0: controller 1 present
        // bit 4: controller 1 has mempak
        // bit 8: controller 1 has rumblepak
        // +1..3 for controllers 2..4.
        private bool _controller1Present;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200BController 1 Present")]
        public bool Controller1Present { get => _controller1Present; set { _controller1Present = value; NotifyChange(); } }

        private bool _controller2Present;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200BController 2 Present")]
        public bool Controller2Present { get => _controller2Present; set { _controller2Present = value; NotifyChange(); } }

        private bool _controller3Present;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200BController 3 Present")]
        public bool Controller3Present { get => _controller3Present; set { _controller3Present = value; NotifyChange(); } }

        private bool _controller4Present;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200B\u200BController 4 Present")]
        public bool Controller4Present { get => _controller4Present; set { _controller4Present = value; NotifyChange(); } }

        private bool _controller1MemPak;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200BController 1 MemPak")]
        public bool Controller1MemPak { get => _controller1MemPak; set { _controller1MemPak = value; NotifyChange(); } }

        private bool _controller2MemPak;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200BController 2 MemPak")]
        public bool Controller2MemPak { get => _controller2MemPak; set { _controller2MemPak = value; NotifyChange(); } }

        private bool _controller3MemPak;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200BController 3 MemPak")]
        public bool Controller3MemPak { get => _controller3MemPak; set { _controller3MemPak = value; NotifyChange(); } }

        private bool _controller4MemPak;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("\u200BController 4 MemPak")]
        public bool Controller4MemPak { get => _controller4MemPak; set { _controller4MemPak = value; NotifyChange(); } }

        private bool _controller1RumblePak;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("Controller 1 RumblePak")]
        public bool Controller1RumblePak { get => _controller1RumblePak; set { _controller1RumblePak = value; NotifyChange(); } }

        private bool _controller2RumblePak;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("Controller 2 RumblePak")]
        public bool Controller2RumblePak { get => _controller2RumblePak; set { _controller2RumblePak = value; NotifyChange(); } }

        private bool _controller3RumblePak;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("Controller 3 RumblePak")]
        public bool Controller3RumblePak { get => _controller3RumblePak; set { _controller3RumblePak = value; NotifyChange(); } }

        private bool _controller4RumblePak;
        [CategoryAttribute("\u200B\u200BController"), DisplayName("Controller 4 RumblePak")]
        public bool Controller4RumblePak { get => _controller4RumblePak; set { _controller4RumblePak = value; NotifyChange(); } }

        // 122 64-byte ASCII string: name of video plugin used when recording, directly from plugin
        private string _videoPlugin;
        [CategoryAttribute("\u200BPlugin"), DisplayName("\u200B\u200B\u200BVideo Plugin")]
        public string VideoPlugin { get => _videoPlugin; set { _videoPlugin = StringUtilities.Cap(value, 64); NotifyChange(); } }

        // 162 64-byte ASCII string: name of sound plugin used when recording, directly from plugin
        private string _soundPlugin;
        [CategoryAttribute("\u200BPlugin"), DisplayName("\u200B\u200BSound Plugin")]
        public string SoundPlugin { get => _soundPlugin; set { _soundPlugin = StringUtilities.Cap(value, 64); NotifyChange(); } }

        // 1A2 64-byte ASCII string: name of input plugin used when recording, directly from plugin
        private string _inputPlugin;
        [CategoryAttribute("\u200BPlugin"), DisplayName("\u200BInput Plugin")]
        public string InputPlugin { get => _inputPlugin; set { _inputPlugin = StringUtilities.Cap(value, 64); NotifyChange(); } }

        // 1E2 64-byte ASCII string: name of rsp plugin used when recording, directly from plugin
        private string _rspPlugin;
        [CategoryAttribute("\u200BPlugin"), DisplayName("RSP Plugin")]
        public string RspPlugin { get => _rspPlugin; set { _rspPlugin = StringUtilities.Cap(value, 64); NotifyChange(); } }

        // 000 4-byte signature: 4D 36 34 1A "M64\x1A"
        private uint _signature;
        [CategoryAttribute("Mupen"), DisplayName("\u200B\u200BSignature")]
        public uint Signature { get => _signature; set { _signature = value; NotifyChange(); } }

        // 004 4-byte little-endian unsigned int: version number, should be 3
        private uint _versionNumber;
        [CategoryAttribute("Mupen"), DisplayName("\u200BVersion Number")]
        public uint VersionNumber { get => _versionNumber; set { _versionNumber = value; NotifyChange(); } }

        // 008 4-byte little-endian integer: movie "uid" - identifies the movie-savestate relationship,
        // also used as the recording time in Unix epoch format
        private int _uid;
        [CategoryAttribute("Mupen"), DisplayName("UID")]
        public int Uid { get => _uid; set { _uid = value; NotifyChange(); } }

        public M64Header(M64File m64File, M64Gui gui)
        {
            _m64File = m64File;
            _gui = gui;
        }

        private void NotifyChange()
        {
            _m64File.IsModified = true;
            _gui.PropertyGridHeader.Refresh();
        }

        public void LoadBytes(byte[] bytes)
        {
            if (bytes.Length != M64Config.HeaderSize) throw new ArgumentOutOfRangeException();

            Signature = BitConverter.ToUInt32(bytes, 0x000);
            VersionNumber = BitConverter.ToUInt32(bytes, 0x004);
            Uid = BitConverter.ToInt32(bytes, 0x008);
            NumVis = BitConverter.ToInt32(bytes, 0x00C);
            NumRerecords = BitConverter.ToInt32(bytes, 0x010);
            Fps = bytes[0x014];
            NumControllers = bytes[0x015];
            NumInputs = BitConverter.ToInt32(bytes, 0x018);

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
            Crc32 = BitConverter.ToUInt32(bytes, 0x0E4);
            CountryCode = BitConverter.ToUInt16(bytes, 0x0E8);
            VideoPlugin = Encoding.ASCII.GetString(bytes, 0x122, 64).Replace("\0", "");
            SoundPlugin = Encoding.ASCII.GetString(bytes, 0x162, 64).Replace("\0", "");
            InputPlugin = Encoding.ASCII.GetString(bytes, 0x1A2, 64).Replace("\0", "");
            RspPlugin = Encoding.ASCII.GetString(bytes, 0x1E2, 64).Replace("\0", "");
            Author = Encoding.UTF8.GetString(bytes, 0x222, 222).Replace("\0", "");
            Description = Encoding.UTF8.GetString(bytes, 0x300, 256).Replace("\0", "");

            // Verify that serialization works correctly
            if (!Enumerable.SequenceEqual(bytes, ToBytes()))
            {
                DialogUtilities.DisplayMessage(
                    "The m64 header cannot be serialized exactly as it is. " +
                        "In other words, saving this file will change the m64 header. " +
                        "Proceed at your own risk.",
                    "Warning");
            }
        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(TypeUtilities.GetBytes(Signature));
            bytes.AddRange(TypeUtilities.GetBytes(VersionNumber));
            bytes.AddRange(TypeUtilities.GetBytes(Uid));
            bytes.AddRange(TypeUtilities.GetBytes(NumVis));
            bytes.AddRange(TypeUtilities.GetBytes(NumRerecords));
            bytes.AddRange(TypeUtilities.GetBytes(Fps));
            bytes.AddRange(TypeUtilities.GetBytes(NumControllers));
            bytes.AddRange(new byte[2]);
            bytes.AddRange(TypeUtilities.GetBytes(NumInputs));
            bytes.AddRange(TypeUtilities.GetBytes(ConvertMovieStartTypeEnumToShort(MovieStartType)));
            bytes.AddRange(new byte[2]);
            bytes.AddRange(TypeUtilities.GetBytes(GetControllerFlagsValue()));
            bytes.AddRange(new byte[160]);
            bytes.AddRange(TypeUtilities.GetBytes(RomName, 32, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(Crc32));
            bytes.AddRange(TypeUtilities.GetBytes(CountryCode));
            bytes.AddRange(new byte[56]);
            bytes.AddRange(TypeUtilities.GetBytes(VideoPlugin, 64, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(SoundPlugin, 64, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(InputPlugin, 64, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(RspPlugin, 64, Encoding.ASCII));
            bytes.AddRange(TypeUtilities.GetBytes(Author, 222, Encoding.UTF8));
            bytes.AddRange(TypeUtilities.GetBytes(Description, 256, Encoding.UTF8));
            if (bytes.Count != M64Config.HeaderSize) throw new ArgumentOutOfRangeException();
            return bytes.ToArray();
        }

        public void LoadBytesDebug(byte[] bytes)
        {
            Signature = BitConverter.ToUInt32(bytes, 0x000);
            VersionNumber = BitConverter.ToUInt32(bytes, 0x004);
            Uid = BitConverter.ToInt32(bytes, 0x008);
            NumVis = BitConverter.ToInt32(bytes, 0x00C);
            NumRerecords = BitConverter.ToInt32(bytes, 0x010);
            Fps = bytes[0x014];
            NumControllers = bytes[0x015];
            NumInputs = BitConverter.ToInt32(bytes, 0x018);

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
            Crc32 = BitConverter.ToUInt32(bytes, 0x0E4);
            CountryCode = BitConverter.ToUInt16(bytes, 0x0E8);
            VideoPlugin = Encoding.ASCII.GetString(bytes, 0x122, 64).Replace("\0", "");
            SoundPlugin = Encoding.ASCII.GetString(bytes, 0x162, 64).Replace("\0", "");
            InputPlugin = Encoding.ASCII.GetString(bytes, 0x1A2, 64).Replace("\0", "");
            RspPlugin = Encoding.ASCII.GetString(bytes, 0x1E2, 64).Replace("\0", "");
            Author = Encoding.UTF8.GetString(bytes, 0x222, 222).Replace("\0", "");
            Description = Encoding.UTF8.GetString(bytes, 0x300, 256).Replace("\0", "");

            byte[] SignatureBytes = TypeUtilities.GetBytes(Signature);
            byte[] VersionNumberBytes = TypeUtilities.GetBytes(VersionNumber);
            byte[] UidBytes = TypeUtilities.GetBytes(Uid);
            byte[] NumVisBytes = TypeUtilities.GetBytes(NumVis);
            byte[] NumRerecordsBytes = TypeUtilities.GetBytes(NumRerecords);
            byte[] FpsBytes = TypeUtilities.GetBytes(Fps);
            byte[] NumControllersBytes = TypeUtilities.GetBytes(NumControllers);
            byte[] Empty1Bytes = new byte[2];
            byte[] NumInputsBytes = TypeUtilities.GetBytes(NumInputs);
            byte[] MovieStartTypeBytes = TypeUtilities.GetBytes(ConvertMovieStartTypeEnumToShort(MovieStartType));
            byte[] Empty2Bytes = new byte[2];
            byte[] ControllerFlagsBytes = TypeUtilities.GetBytes(GetControllerFlagsValue());
            byte[] Empty3Bytes = new byte[160];
            byte[] RomNameBytes = TypeUtilities.GetBytes(RomName, 32, Encoding.ASCII);
            byte[] Crc32Bytes = TypeUtilities.GetBytes(Crc32);
            byte[] CountryCodeBytes = TypeUtilities.GetBytes(CountryCode);
            byte[] Empty4Bytes = new byte[56];
            byte[] VideoPluginBytes = TypeUtilities.GetBytes(VideoPlugin, 64, Encoding.ASCII);
            byte[] SoundPluginBytes = TypeUtilities.GetBytes(SoundPlugin, 64, Encoding.ASCII);
            byte[] InputPluginBytes = TypeUtilities.GetBytes(InputPlugin, 64, Encoding.ASCII);
            byte[] RspPluginBytes = TypeUtilities.GetBytes(RspPlugin, 64, Encoding.ASCII);
            byte[] AuthorBytes = TypeUtilities.GetBytes(Author, 222, Encoding.UTF8);
            byte[] DescriptionBytes = TypeUtilities.GetBytes(Description, 256, Encoding.UTF8);

            byte[] GetRemoveFirstBytesFromList(List<byte> byteList, int size)
            {
                byte[] output = byteList.Take(size).ToArray();
                List<byte> newByteList = byteList.Skip(size).ToList();
                byteList.Clear();
                byteList.AddRange(newByteList);
                return output;
            }

            List<byte> bytesToUse = bytes.ToList();
            byte[] SignatureBytes2 = GetRemoveFirstBytesFromList(bytesToUse, SignatureBytes.Length);
            byte[] VersionNumberBytes2 = GetRemoveFirstBytesFromList(bytesToUse, VersionNumberBytes.Length);
            byte[] UidBytes2 = GetRemoveFirstBytesFromList(bytesToUse, UidBytes.Length);
            byte[] NumVisBytes2 = GetRemoveFirstBytesFromList(bytesToUse, NumVisBytes.Length);
            byte[] NumRerecordsBytes2 = GetRemoveFirstBytesFromList(bytesToUse, NumRerecordsBytes.Length);
            byte[] FpsBytes2 = GetRemoveFirstBytesFromList(bytesToUse, FpsBytes.Length);
            byte[] NumControllersBytes2 = GetRemoveFirstBytesFromList(bytesToUse, NumControllersBytes.Length);
            byte[] Empty1Bytes2 = GetRemoveFirstBytesFromList(bytesToUse, Empty1Bytes.Length);
            byte[] NumInputsBytes2 = GetRemoveFirstBytesFromList(bytesToUse, NumInputsBytes.Length);
            byte[] MovieStartTypeBytes2 = GetRemoveFirstBytesFromList(bytesToUse, MovieStartTypeBytes.Length);
            byte[] Empty2Bytes2 = GetRemoveFirstBytesFromList(bytesToUse, Empty2Bytes.Length);
            byte[] ControllerFlagsBytes2 = GetRemoveFirstBytesFromList(bytesToUse, ControllerFlagsBytes.Length);
            byte[] Empty3Bytes2 = GetRemoveFirstBytesFromList(bytesToUse, Empty3Bytes.Length);
            byte[] RomNameBytes2 = GetRemoveFirstBytesFromList(bytesToUse, RomNameBytes.Length);
            byte[] Crc32Bytes2 = GetRemoveFirstBytesFromList(bytesToUse, Crc32Bytes.Length);
            byte[] CountryCodeBytes2 = GetRemoveFirstBytesFromList(bytesToUse, CountryCodeBytes.Length);
            byte[] Empty4Bytes2 = GetRemoveFirstBytesFromList(bytesToUse, Empty4Bytes.Length);
            byte[] VideoPluginBytes2 = GetRemoveFirstBytesFromList(bytesToUse, VideoPluginBytes.Length);
            byte[] SoundPluginBytes2 = GetRemoveFirstBytesFromList(bytesToUse, SoundPluginBytes.Length);
            byte[] InputPluginBytes2 = GetRemoveFirstBytesFromList(bytesToUse, InputPluginBytes.Length);
            byte[] RspPluginBytes2 = GetRemoveFirstBytesFromList(bytesToUse, RspPluginBytes.Length);
            byte[] AuthorBytes2 = GetRemoveFirstBytesFromList(bytesToUse, AuthorBytes.Length);
            byte[] DescriptionBytes2 = GetRemoveFirstBytesFromList(bytesToUse, DescriptionBytes.Length);

            List<string> diffs = new List<string>();
            if (!Enumerable.SequenceEqual(SignatureBytes, SignatureBytes2)) diffs.Add("SignatureBytes");
            if (!Enumerable.SequenceEqual(VersionNumberBytes, VersionNumberBytes2)) diffs.Add("VersionNumberBytes");
            if (!Enumerable.SequenceEqual(UidBytes, UidBytes2)) diffs.Add("UidBytes");
            if (!Enumerable.SequenceEqual(NumVisBytes, NumVisBytes2)) diffs.Add("NumVisBytes");
            if (!Enumerable.SequenceEqual(NumRerecordsBytes, NumRerecordsBytes2)) diffs.Add("NumRerecordsBytes");
            if (!Enumerable.SequenceEqual(FpsBytes, FpsBytes2)) diffs.Add("FpsBytes");
            if (!Enumerable.SequenceEqual(NumControllersBytes, NumControllersBytes2)) diffs.Add("NumControllersBytes");
            if (!Enumerable.SequenceEqual(Empty1Bytes, Empty1Bytes2)) diffs.Add("Empty1Bytes");
            if (!Enumerable.SequenceEqual(NumInputsBytes, NumInputsBytes2)) diffs.Add("NumInputsBytes");
            if (!Enumerable.SequenceEqual(MovieStartTypeBytes, MovieStartTypeBytes2)) diffs.Add("MovieStartTypeBytes");
            if (!Enumerable.SequenceEqual(Empty2Bytes, Empty2Bytes2)) diffs.Add("Empty2Bytes");
            if (!Enumerable.SequenceEqual(ControllerFlagsBytes, ControllerFlagsBytes2)) diffs.Add("ControllerFlagsBytes");
            if (!Enumerable.SequenceEqual(Empty3Bytes, Empty3Bytes2)) diffs.Add("Empty3Bytes");
            if (!Enumerable.SequenceEqual(RomNameBytes, RomNameBytes2)) diffs.Add("RomNameBytes");
            if (!Enumerable.SequenceEqual(Crc32Bytes, Crc32Bytes2)) diffs.Add("Crc32Bytes");
            if (!Enumerable.SequenceEqual(CountryCodeBytes, CountryCodeBytes2)) diffs.Add("CountryCodeBytes");
            if (!Enumerable.SequenceEqual(Empty4Bytes, Empty4Bytes2)) diffs.Add("Empty4Bytes");
            if (!Enumerable.SequenceEqual(VideoPluginBytes, VideoPluginBytes2)) diffs.Add("VideoPluginBytes");
            if (!Enumerable.SequenceEqual(SoundPluginBytes, SoundPluginBytes2)) diffs.Add("SoundPluginBytes");
            if (!Enumerable.SequenceEqual(InputPluginBytes, InputPluginBytes2)) diffs.Add("InputPluginBytes");
            if (!Enumerable.SequenceEqual(RspPluginBytes, RspPluginBytes2)) diffs.Add("RspPluginBytes");
            if (!Enumerable.SequenceEqual(AuthorBytes, AuthorBytes2)) diffs.Add("AuthorBytes");
            if (!Enumerable.SequenceEqual(DescriptionBytes, DescriptionBytes2)) diffs.Add("DescriptionBytes");
            InfoForm.ShowValue(string.Join("\r\n", diffs));
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
                case MovieStartTypeEnum.FromSnapshot:
                    return 1;
                case MovieStartTypeEnum.FromStart:
                    return 2;
                case MovieStartTypeEnum.FromEEPROM:
                    return 4;
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
                case 4:
                    return MovieStartTypeEnum.FromEEPROM;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Clear()
        {
            NumInputs = 0;
            NumVis = 0;
            NumRerecords = 0;
            MovieStartType = MovieStartTypeEnum.FromStart;
            Fps = 0;
            RomName = null;
            CountryCode = 0;
            Crc32 = 0;
            Author = null;
            Description = null;
            NumControllers = 0;
            Controller1Present = false;
            Controller2Present = false;
            Controller3Present = false;
            Controller4Present = false;
            Controller1MemPak = false;
            Controller2MemPak = false;
            Controller3MemPak = false;
            Controller4MemPak = false;
            Controller1RumblePak = false;
            Controller2RumblePak = false;
            Controller3RumblePak = false;
            Controller4RumblePak = false;
            VideoPlugin = null;
            SoundPlugin = null;
            InputPlugin = null;
            RspPlugin = null;
            Signature = 0;
            VersionNumber = 0;
            Uid = 0;
        }
    }
}
