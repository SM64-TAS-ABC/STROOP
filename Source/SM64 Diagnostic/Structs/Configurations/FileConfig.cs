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

        public uint HatLocationGroundOffset;
        public byte HatLocationGroundMask;
        public uint HatLocationKleptoOffset;
        public byte HatLocationKleptoMask;
        public uint HatLocationUkikiOffset;
        public byte HatLocationUkikiMask;
        public uint HatLocationSnowmanOffset;
        public byte HatLocationSnowmanMask;
        public uint HatLocationCourseOffset;
        public ushort HatLocationCourseSSLValue;
        public ushort HatLocationCourseSLValue;
        public ushort HatLocationCourseTTMValue;
    }
}
