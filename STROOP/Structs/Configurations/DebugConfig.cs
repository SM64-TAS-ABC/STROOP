using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class DebugConfig
    {
        public static uint AdvancedModeAddress { get { return Config.SwitchRomVersion(AdvancedModeAddressUS, AdvancedModeAddressJP); } }
        public static readonly uint AdvancedModeAddressUS = 0x8033D263;
        public static readonly uint AdvancedModeAddressJP = 0x8033BEF3;

        public static uint AdvancedModeSettingAddress { get { return Config.SwitchRomVersion(AdvancedModeSettingAddressUS, AdvancedModeSettingAddressJP); } }
        public static readonly uint AdvancedModeSettingAddressUS = 0x80330E94;
        public static readonly uint AdvancedModeSettingAddressJP = 0x8032FF34;

        public static uint ResourceMeterAddress { get { return Config.SwitchRomVersion(ResourceMeterAddressUS, ResourceMeterAddressJP); } }
        public static readonly uint ResourceMeterAddressUS = 0x8032D594;
        public static readonly uint ResourceMeterAddressJP = 0x8032C654;

        public static uint ResourceMeterSettingAddress { get { return Config.SwitchRomVersion(ResourceMeterSettingAddressUS, ResourceMeterSettingAddressJP); } }
        public static readonly uint ResourceMeterSettingAddressUS = 0x8032DF10;
        public static readonly uint ResourceMeterSettingAddressJP = 0x8032CFB0;

        public static uint ClassicModeAddress { get { return Config.SwitchRomVersion(ClassicModeAddressUS, ClassicModeAddressJP); } }
        public static readonly uint ClassicModeAddressUS = 0x8032D598;
        public static readonly uint ClassicModeAddressJP = 0x8032C658;

        public static uint SpawnModeAddress { get { return Config.SwitchRomVersion(SpawnModeAddressUS, SpawnModeAddressJP); } }
        public static readonly uint SpawnModeAddressUS = 0x8033D2DF;
        public static readonly uint SpawnModeAddressJP = 0x8033BF6F;

        public static uint StageSelectAddress { get { return Config.SwitchRomVersion(StageSelectAddressUS, StageSelectAddressJP); } }
        public static readonly uint StageSelectAddressUS = 0x8032D58C;
        public static readonly uint StageSelectAddressJP = 0x8032C64C;

        public static uint FreeMovementAddress { get { return Config.SwitchRomVersion(FreeMovementAddressUS, FreeMovementAddressJP); } }
        public static readonly uint FreeMovementAddressUS = 0x80269BDA;
        public static readonly uint FreeMovementAddressJP = 0x8026976E;

        public static ushort FreeMovementOnValue { get { return Config.SwitchRomVersion(FreeMovementOnValueUS, FreeMovementOnValueJP); } }
        public static readonly ushort FreeMovementOnValueUS = 0x5FAB;
        public static readonly ushort FreeMovementOnValueJP = 0x5F0D;

        public static ushort FreeMovementOffValue { get { return Config.SwitchRomVersion(FreeMovementOffValueUS, FreeMovementOffValueJP); } }
        public static readonly ushort FreeMovementOffValueUS = 0x98D5;
        public static readonly ushort FreeMovementOffValueJP = 0x97D1;
    }
}
