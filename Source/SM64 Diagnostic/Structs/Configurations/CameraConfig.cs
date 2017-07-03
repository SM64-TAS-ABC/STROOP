using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct CameraConfig
    {
        public uint CameraStructAddress;
        public uint XOffset;
        public uint YOffset;
        public uint ZOffset;
        public uint FocusXOffset;
        public uint FocusYOffset;
        public uint FocusZOffset;
        public uint YawFacingOffset;
        public uint SecondObject;
    }
}
