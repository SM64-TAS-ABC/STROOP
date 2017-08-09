using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct DebugConfig
    {
        public uint AdvancedModeAddress { get { return Config.SwitchRomVersion(AdvancedModeAddressUS, AdvancedModeAddressJP); } }
        public uint AdvancedModeAddressUS;
        public uint AdvancedModeAddressJP;

        public uint SettingAddress { get { return Config.SwitchRomVersion(SettingAddressUS, SettingAddressJP); } }
        public uint SettingAddressUS;
        public uint SettingAddressJP;

        public uint SpawnModeAddress { get { return Config.SwitchRomVersion(SpawnModeAddressUS, SpawnModeAddressJP); } }
        public uint SpawnModeAddressUS;
        public uint SpawnModeAddressJP;

        public uint ClassicModeAddress { get { return Config.SwitchRomVersion(ClassicModeAddressUS, ClassicModeAddressJP); } }
        public uint ClassicModeAddressUS;
        public uint ClassicModeAddressJP;

        public uint ResourceModeAddress { get { return Config.SwitchRomVersion(ResourceModeAddressUS, ResourceModeAddressJP); } }
        public uint ResourceModeAddressUS;
        public uint ResourceModeAddressJP;

        public uint StageSelectAddress { get { return Config.SwitchRomVersion(StageSelectAddressUS, StageSelectAddressJP); } }
        public uint StageSelectAddressUS;
        public uint StageSelectAddressJP;

        public uint FreeMovementAddress { get { return Config.SwitchRomVersion(FreeMovementAddressUS, FreeMovementAddressJP); } }
        public uint FreeMovementAddressUS;
        public uint FreeMovementAddressJP;

        public ushort FreeMovementOnValue;
        public ushort FreeMovementOffValue;
    }
}
