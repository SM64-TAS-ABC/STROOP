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
        private static int FrameCountAddress
        {
            get
            {
                switch (Config.Stream.ProcessName)
                {
                    case "mupen64-rerecording":
                        return 0x0047A7A4;
                    case "mupen64_lua":
                        return 0x004D614C;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        private static int VICountAddress
        {
            get
            {
                switch (Config.Stream.ProcessName)
                {
                    case "mupen64-rerecording":
                        return 0x0047A7A0;
                    case "mupen64_lua":
                        return 0x004D6150;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /*
        private static int FrameCountAddress2 = 0x0077DF50;
        private static int VICountAddress2 = 0x0077DF44;
        */

        public static int GetFrameCount()
        {
            if (!IsUsingMupen()) throw new ArgumentOutOfRangeException("Not using mupen");
            byte[] buffer = new byte[4];
            Config.Stream.ReadProcessMemory(FrameCountAddress, buffer, true);
            int frameCount = BitConverter.ToInt32(buffer, 0);
            return frameCount - 1;
        }

        public static int GetVICount()
        {
            if (!IsUsingMupen()) throw new ArgumentOutOfRangeException("Not using mupen");
            byte[] buffer = new byte[4];
            Config.Stream.ReadProcessMemory(VICountAddress, buffer, true);
            int viCount = BitConverter.ToInt32(buffer, 0);
            return viCount;
        }

        public static int GetLagCount()
        {
            return GetVICount() - 2 * GetFrameCount();
        }

        public static bool IsUsingMupen()
        {
            return Config.Stream.ProcessName == "mupen64-rerecording" ||
                Config.Stream.ProcessName == "mupen64_lua";
        }

    }
}
