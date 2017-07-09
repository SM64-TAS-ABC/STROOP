using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct FileConfig
    {
        public uint FileStructAddress;
        public uint FileStructSize;
        public uint FileAAddress;
        public uint FileBAddress;
        public uint FileCAddress;
        public uint FileDAddress;
        public uint FileASavedAddress;
        public uint FileBSavedAddress;
        public uint FileCSavedAddress;
        public uint FileDSavedAddress;

        public uint ChecksumConstantOffset;
        public ushort ChecksumConstantValue;
        public uint ChecksumOffset;

        public uint CourseStarsAddressStart;
        public uint TotWCStarAddress;
        public uint CotMCStarAddress;
        public uint VCutMStarAddress;
        public uint PSSStarsAddress;
        public uint SAStarAddress;
        public uint WMotRStarAddress;
        public uint BitDWStarAddress;
        public uint BitFSStarAddress;
        public uint BitSStarAddress;
        public uint ToadMIPSStarsAddress;

        public uint MainCourseCannonsAddressStart;
        public uint WMotRCannonAddress;
        public byte CannonMask;

        public uint WFDoorAddress;
        public uint JRBDoorAddress;
        public uint CCMDoorAddress;
        public uint PSSDoorAddress;
        public uint BitDWDoorAddress;
        public uint BitFSDoorAddress;
        public uint BitSDoorAddress;

        public byte WFDoorMask;
        public byte JRBDoorMask;
        public byte CCMDoorMask;
        public byte PSSDoorMask;
        public byte BitDWDoorMask;
        public byte BitFSDoorMask;
        public byte BitSDoorMask;

        public uint CoinScoreAddressStart;

        public uint FileStartedAddress;
        public byte FileStartedMask;
        public uint CapSwitchPressedAddress;
        public byte RedCapSwitchMask;
        public byte GreenCapSwitchMask;
        public byte BlueCapSwitchMask;
        public uint KeyDoorAddress;
        public byte KeyDoor1KeyMask;
        public byte KeyDoor1OpenedMask;
        public byte KeyDoor2KeyMask;
        public byte KeyDoor2OpenedMask;
        public uint MoatDrainedAddress;
        public byte MoatDrainedMask;
        public uint DDDMovedBackAddress;
        public byte DDDMovedBackMask;

        public uint HatLocationModeOffset;
        public byte HatLocationModeMask;
        public byte HatLocationMarioMask;
        public byte HatLocationGroundMask;
        public byte HatLocationKleptoMask;
        public byte HatLocationUkikiMask;
        public byte HatLocationSnowmanMask;
        public uint HatLocationCourseOffset;
        public ushort HatLocationCourseSSLValue;
        public ushort HatLocationCourseSLValue;
        public ushort HatLocationCourseTTMValue;

        public uint HatPositionXAddress;
        public uint HatPositionYAddress;
        public uint HatPositionZAddress;
    }
}
