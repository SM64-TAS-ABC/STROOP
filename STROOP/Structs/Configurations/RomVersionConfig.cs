using STROOP.Managers;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class RomVersionConfig
    {
        public static RomVersion Version = RomVersion.US;

        public static uint RomVersionTellAddress = 0x80322B24;
        public static uint RomVersionTellValueUS = 0x8FA6001C;
        public static uint RomVersionTellValueJP = 0x46006004;

        public static void UpdateRomVersion(RomVersionSelection romVersionSelection)
        {
            switch (romVersionSelection)
            {
                case RomVersionSelection.AUTO:
                    RomVersion? autoRomVersion = GetRomVersionUsingTell();
                    if (autoRomVersion.HasValue)
                        Version = autoRomVersion.Value;
                    break;
                case RomVersionSelection.US:
                    Version = RomVersion.US;
                    break;
                case RomVersionSelection.JP:
                    Version = RomVersion.JP;
                    break;
                case RomVersionSelection.SH:
                    Version = RomVersion.SH;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static RomVersion? GetRomVersionUsingTell()
        {
            uint tell = Config.Stream.GetUInt32(RomVersionTellAddress);
            if (tell == RomVersionTellValueUS) return RomVersion.US;
            if (tell == RomVersionTellValueJP) return RomVersion.JP;
            return null;
        }

        public static uint Switch(uint? valUS = null, uint? valJP = null, uint? valSH = null)
        {
            switch (Version)
            {
                case RomVersion.US:
                    if (valUS.HasValue) return valUS.Value;
                    break;
                case RomVersion.JP:
                    if (valJP.HasValue) return valJP.Value;
                    break;
                case RomVersion.SH:
                    if (valSH.HasValue) return valSH.Value;
                    break;
            }
            return 0;
        }
        public static ushort Switch(ushort? valUS = null, ushort? valJP = null, ushort? valSH = null)
        {
            switch (Version)
            {
                case RomVersion.US:
                    if (valUS.HasValue) return valUS.Value;
                    break;
                case RomVersion.JP:
                    if (valJP.HasValue) return valJP.Value;
                    break;
                case RomVersion.SH:
                    if (valSH.HasValue) return valSH.Value;
                    break;
            }
            return 0;
        }
    }
}
