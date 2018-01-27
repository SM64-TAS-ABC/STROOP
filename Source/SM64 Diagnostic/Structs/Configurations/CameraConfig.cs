using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class CameraConfig
    {
        public static uint CameraStructAddress { get { return Config.SwitchRomVersion(CameraStructAddressUS, CameraStructAddressJP); } }
        public static readonly uint CameraStructAddressUS = 0x8033C618;
        public static readonly uint CameraStructAddressJP = 0x8033B2A8;

        public static readonly uint XOffset = 0x8C;
        public static readonly uint YOffset = 0x90;
        public static readonly uint ZOffset = 0x94;
        public static readonly uint FocusXOffset = 0x80;
        public static readonly uint FocusYOffset = 0x84;
        public static readonly uint FocusZOffset = 0x88;
        public static readonly uint YawFacingOffset = 0xCE;
        public static readonly uint MarioCamPossibleOffset = 0x6D;
        public static readonly byte MarioCamPossibleMask = 0x04;

        public static uint SecondaryObjectAddress { get { return Config.SwitchRomVersion(SecondaryObjectAddressUS, SecondaryObjectAddressJP); } }
        public static readonly uint SecondaryObjectAddressUS = 0x8032DF30;
        public static readonly uint SecondaryObjectAddressJP = 0x8032CFD0;
    }
}
