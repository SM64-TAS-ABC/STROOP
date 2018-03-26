using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class MupenUtilities
    {
        private static int FrameCountAddress = 0x0047A7A4;
        private static int VICountAddress = 0x0047A7A0;

        private static int FrameCountAddress2 = 0x0077DF50;
        private static int VICountAddress2 = 0x0077DF44;

        public static int GetFrameCount()
        {
            byte[] buffer = new byte[4];
            Config.Stream.ReadProcessMemory(FrameCountAddress, buffer, true);
            int framecount = BitConverter.ToInt32(buffer, 0);
            return framecount - 1;
        }

        public static int GetVICount()
        {
            byte[] buffer = new byte[4];
            Config.Stream.ReadProcessMemory(VICountAddress, buffer, true);
            int vicount = BitConverter.ToInt32(buffer, 0);
            return vicount;
        }

        public static int GetLagCount()
        {
            return GetVICount() - 2 * GetFrameCount();
        }

    }
}
