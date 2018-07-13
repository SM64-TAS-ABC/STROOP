using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class MainSaveConfig
    {
        public static uint MainSaveStructAddress { get => RomVersionConfig.Switch(MainSaveStructAddressUS, MainSaveStructAddressJP); }
        public static readonly uint MainSaveStructAddressUS = 0x802078C0;
        public static readonly uint MainSaveStructAddressJP = 0x80207CC0;

        public static readonly uint MainSaveStructSize = 0x20;

        public static uint MainSaveAddress { get { return MainSaveStructAddress + 0 * MainSaveStructSize; } }
        public static uint MainSaveSavedAddress { get { return MainSaveStructAddress + 1 * MainSaveStructSize; } }

        public static readonly uint SoundModeOffset = 0x10;
        public static readonly ushort SoundModeStereoValue = 0;
        public static readonly ushort SoundModeMonoValue = 1;
        public static readonly ushort SoundModeHeadsetValue = 2;

        public static readonly uint ChecksumConstantOffset = 0x1C;
        public static readonly ushort ChecksumConstantValue = 0x4849;
        public static readonly uint ChecksumOffset = 0x1E;
  }
}
