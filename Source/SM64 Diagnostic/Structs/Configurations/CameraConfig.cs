using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct CameraConfig
    {
        public uint CameraStructAddress { get { return Config.SwitchRomVersion(CameraStructAddressUS, CameraStructAddressJP); } }
        public uint CameraStructAddressUS;
        public uint CameraStructAddressJP;

        public uint XOffset;
        public uint YOffset;
        public uint ZOffset;
        public uint FocusXOffset;
        public uint FocusYOffset;
        public uint FocusZOffset;
        public uint YawFacingOffset;
        public uint MarioCamPossibleOffset;
        public byte MarioCamPossibleMask;

        public uint SecondaryObjectAddress { get { return Config.SwitchRomVersion(SecondaryObjectAddressUS, SecondaryObjectAddressJP); } }
        public uint SecondaryObjectAddressUS;
        public uint SecondaryObjectAddressJP;
    }
}
