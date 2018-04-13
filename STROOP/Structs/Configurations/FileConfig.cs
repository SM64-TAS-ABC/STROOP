using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class FileConfig
    {
        public static uint FileStructAddress { get { return RomVersionConfig.Switch(FileStructAddressUS, FileStructAddressJP); } }
        public static readonly uint FileStructAddressUS = 0x80207700;
        public static readonly uint FileStructAddressJP = 0x80207B00;

        public static readonly uint FileStructSize = 0x38;

        public static uint FileAAddress { get { return FileStructAddress + 0 * FileStructSize; } }
        public static uint FileBAddress { get { return FileStructAddress + 2 * FileStructSize; } }
        public static uint FileCAddress { get { return FileStructAddress + 4 * FileStructSize; } }
        public static uint FileDAddress { get { return FileStructAddress + 6 * FileStructSize; } }
        public static uint FileASavedAddress { get { return FileStructAddress + 1 * FileStructSize; } }
        public static uint FileBSavedAddress { get { return FileStructAddress + 3 * FileStructSize; } }
        public static uint FileCSavedAddress { get { return FileStructAddress + 5 * FileStructSize; } }
        public static uint FileDSavedAddress { get { return FileStructAddress + 7 * FileStructSize; } }

        public static readonly uint ChecksumConstantOffset = 0x34;
        public static readonly ushort ChecksumConstantValue = 0x4441;
        public static readonly uint ChecksumOffset = 0x36;

        public static readonly uint CourseStarsOffsetStart = 0x0C;
        public static readonly uint TotWCStarOffset = 0x20;
        public static readonly uint CotMCStarOffset = 0x1F;
        public static readonly uint VCutMStarOffset = 0x21;
        public static readonly uint PSSStarsOffset = 0x1E;
        public static readonly uint SAStarOffset = 0x23;
        public static readonly uint WMotRStarOffset = 0x22;
        public static readonly uint BitDWStarOffset = 0x1B;
        public static readonly uint BitFSStarOffset = 0x1C;
        public static readonly uint BitSStarOffset = 0x1D;
        public static readonly uint ToadMIPSStarsOffset = 0x08;

        public static readonly uint MainCourseCannonsOffsetStart = 0x0D;
        public static readonly uint WMotRCannonOffset = 0x23;
        public static readonly byte CannonMask = 0x80;

        public static readonly uint WFDoorOffset = 0x0A;
        public static readonly uint JRBDoorOffset = 0x0A;
        public static readonly uint CCMDoorOffset = 0x0A;
        public static readonly uint PSSDoorOffset = 0x0A;
        public static readonly uint BitDWDoorOffset = 0x0A;
        public static readonly uint BitFSDoorOffset = 0x0A;
        public static readonly uint BitSDoorOffset = 0x09;

        public static readonly byte WFDoorMask = 0x08;
        public static readonly byte JRBDoorMask = 0x20;
        public static readonly byte CCMDoorMask = 0x10;
        public static readonly byte PSSDoorMask = 0x04;
        public static readonly byte BitDWDoorMask = 0x40;
        public static readonly byte BitFSDoorMask = 0x80;
        public static readonly byte BitSDoorMask = 0x10;

        public static readonly uint CoinScoreOffsetStart = 0x25;

        public static readonly uint FileStartedOffset = 0x0B;
        public static readonly byte FileStartedMask = 0x01;
        public static readonly uint CapSwitchPressedOffset = 0x0B;
        public static readonly byte RedCapSwitchMask = 0x02;
        public static readonly byte GreenCapSwitchMask = 0x04;
        public static readonly byte BlueCapSwitchMask = 0x08;
        public static readonly uint KeyDoorOffset = 0x0B;
        public static readonly byte KeyDoor1KeyMask = 0x10;
        public static readonly byte KeyDoor1OpenedMask = 0x40;
        public static readonly byte KeyDoor2KeyMask = 0x20;
        public static readonly byte KeyDoor2OpenedMask = 0x80;
        public static readonly uint MoatDrainedOffset = 0x0A;
        public static readonly byte MoatDrainedMask = 0x02;
        public static readonly uint DDDMovedBackOffset = 0x0A;
        public static readonly byte DDDMovedBackMask = 0x01;

        public static readonly uint HatLocationModeOffset = 0x09;
        public static readonly byte HatLocationModeMask = 0x0F;
        public static readonly byte HatLocationMarioMask = 0x00;
        public static readonly byte HatLocationGroundMask = 0x01;
        public static readonly byte HatLocationKleptoMask = 0x02;
        public static readonly byte HatLocationUkikiMask = 0x04;
        public static readonly byte HatLocationSnowmanMask = 0x08;

        public static readonly uint HatLocationCourseOffset = 0x00;
        public static readonly ushort HatLocationCourseSSLValue = 0x0801;
        public static readonly ushort HatLocationCourseSLValue = 0x0A01;
        public static readonly ushort HatLocationCourseTTMValue = 0x2401;

        public static readonly uint HatPositionXOffset = 0x02;
        public static readonly uint HatPositionYOffset = 0x04;
        public static readonly uint HatPositionZOffset = 0x06;
    }
}
