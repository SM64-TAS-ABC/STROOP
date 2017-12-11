using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct FileConfig
    {
        public uint FileStructAddress { get { return Config.SwitchRomVersion(FileStructAddressUS, FileStructAddressJP); } }
        public uint FileStructAddressUS;
        public uint FileStructAddressJP;

        public uint FileStructSize;
        public uint FileAAddress { get { return FileStructAddress + 0 * FileStructSize; } }
        public uint FileBAddress { get { return FileStructAddress + 2 * FileStructSize; } }
        public uint FileCAddress { get { return FileStructAddress + 4 * FileStructSize; } }
        public uint FileDAddress { get { return FileStructAddress + 6 * FileStructSize; } }
        public uint FileASavedAddress { get { return FileStructAddress + 1 * FileStructSize; } }
        public uint FileBSavedAddress { get { return FileStructAddress + 3 * FileStructSize; } }
        public uint FileCSavedAddress { get { return FileStructAddress + 5 * FileStructSize; } }
        public uint FileDSavedAddress { get { return FileStructAddress + 7 * FileStructSize; } }

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
