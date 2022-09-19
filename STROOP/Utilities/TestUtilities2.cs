using STROOP.Controls;
using STROOP.Forms;
using STROOP.M64;
using STROOP.Managers;
using STROOP.Map;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Utilities
{
    public static class TestUtilities2
    {
        public static void Test()
        {
            for (int i = -100_000; i <= 100_000; i += 4)
            {
                int address = 0x004E8A64 + i;
                byte[] buffer = new byte[4];
                Config.Stream.ReadProcessMemory((UIntPtr)address, buffer, EndiannessType.Little);
                int frameCount = BitConverter.ToInt32(buffer, 0);
                if (frameCount > 42090 - 5 && frameCount < 42090 + 5)
                {
                    Config.Print(HexUtilities.FormatValue(address) + " => " + frameCount);
                }
            }
        }
    }
}
