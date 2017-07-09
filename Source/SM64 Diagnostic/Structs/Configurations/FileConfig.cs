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

        public uint CourseStarsOffsetStart;
        public uint TotWCStarOffset;
        public uint CotMCStarOffset;
        public uint VCutMStarOffset;
        public uint PSSStarsOffset;
        public uint SAStarOffset;
        public uint WMotRStarOffset;
        public uint BitDWStarOffset;
        public uint BitFSStarOffset;
        public uint BitSStarOffset;
        public uint ToadMIPSStarsOffset;

        public uint MainCourseCannonsOffsetStart;
        public uint WMotRCannonOffset;
        public byte CannonMask;

        public uint WFDoorOffset;
        public uint JRBDoorOffset;
        public uint CCMDoorOffset;
        public uint PSSDoorOffset;
        public uint BitDWDoorOffset;
        public uint BitFSDoorOffset;
        public uint BitSDoorOffset;

        public byte WFDoorMask;
        public byte JRBDoorMask;
        public byte CCMDoorMask;
        public byte PSSDoorMask;
        public byte BitDWDoorMask;
        public byte BitFSDoorMask;
        public byte BitSDoorMask;

        public uint CoinScoreOffsetStart;

        public uint FileStartedOffset;
        public byte FileStartedMask;
        public uint CapSwitchPressedOffset;
        public byte RedCapSwitchMask;
        public byte GreenCapSwitchMask;
        public byte BlueCapSwitchMask;
        public uint KeyDoorOffset;
        public byte KeyDoor1KeyMask;
        public byte KeyDoor1OpenedMask;
        public byte KeyDoor2KeyMask;
        public byte KeyDoor2OpenedMask;
        public uint MoatDrainedOffset;
        public byte MoatDrainedMask;
        public uint DDDMovedBackOffset;
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

        public uint HatPositionXOffset;
        public uint HatPositionYOffset;
        public uint HatPositionZOffset;
    }
}
