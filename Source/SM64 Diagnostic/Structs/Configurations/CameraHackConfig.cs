using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class CameraHackConfig
    {
        public static uint CameraHackStruct { get { return Config.SwitchRomVersion(CameraHackStructUS, CameraHackStructJP); } }
        public static readonly uint CameraHackStructUS = 0x803E0000;
        public static readonly uint CameraHackStructJP = 0x803E0000;

        public static readonly uint CameraModeOffset = 0x00;
        public static readonly uint CameraXOffset = 0x04;
        public static readonly uint CameraYOffset = 0x08;
        public static readonly uint CameraZOffset = 0x0C;
        public static readonly uint FocusXOffset = 0x10;
        public static readonly uint FocusYOffset = 0x14;
        public static readonly uint FocusZOffset = 0x18;
        public static readonly uint AbsoluteAngleOffset = 0x1C;
        public static readonly uint ThetaOffset = 0x1E;
        public static readonly uint RadiusOffset = 0x20;
        public static readonly uint RelativeHeightOffset = 0x24;
        public static readonly uint ObjectOffset = 0x28;

    }
}