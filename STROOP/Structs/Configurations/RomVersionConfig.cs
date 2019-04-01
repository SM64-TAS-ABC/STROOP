using STROOP.Managers;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs.Configurations
{
    public static class RomVersionConfig
    {
        public static RomVersion Version = RomVersion.US;

        public static uint RomVersionTellAddress = 0x80322B24;
        public static uint RomVersionTellValueUS = 0x8FA6001C;
        public static uint RomVersionTellValueJP = 0x46006004;
        public static uint RomVersionTellValueSH = 0x00000000;

        public static void UpdateRomVersion(ComboBox comboBoxRomVersion)
        {
            RomVersionSelection romVersionSelection = (RomVersionSelection)comboBoxRomVersion.SelectedItem;
            switch (romVersionSelection)
            {
                case RomVersionSelection.AUTO:
                case RomVersionSelection.AUTO_US:
                case RomVersionSelection.AUTO_JP:
                case RomVersionSelection.AUTO_SH:
                    RomVersion? autoRomVersionNullable = GetRomVersionUsingTell();
                    if (!autoRomVersionNullable.HasValue) return;
                    RomVersion autoRomVersion = autoRomVersionNullable.Value;
                    Version = autoRomVersion;
                    if (!comboBoxRomVersion.DroppedDown)
                    {
                        switch (autoRomVersion)
                        {
                            case RomVersion.US:
                                comboBoxRomVersion.SelectedItem = RomVersionSelection.AUTO_US;
                                break;
                            case RomVersion.JP:
                                comboBoxRomVersion.SelectedItem = RomVersionSelection.AUTO_JP;
                                break;
                            case RomVersion.SH:
                                comboBoxRomVersion.SelectedItem = RomVersionSelection.AUTO_SH;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
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
            if (tell == RomVersionTellValueSH) return RomVersion.SH;
            return RomVersion.SH;
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
