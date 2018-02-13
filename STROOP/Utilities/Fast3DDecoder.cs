using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs.Configurations;

namespace STROOP.Utilities
{
    public class Fast3DDecoder
    {
        private const int MaxDisplayListLength = 5000; 

        public enum F3DOpcode
        {
            // DMA |00xxxxxx|
            G_NOOP = 0x00,
            G_MTX = 0x01,
            G_RESERVED0 = 0x02,
            G_MOVEMEM = 0x03,
            G_VTX = 0x04,
            G_RESERVED1 = 0x05,
            G_DL = 0x06,
            G_RESERVED2 = 0x07,
            G_RESERVED3 = 0x08,
            G_SPRITE2D_BASE = 0x09,

            // Immediate Mode |10xxxxxx|
            G_RDPHALF_2 = 0xB3,
            G_RDPHALF_1 = 0xB4,
            G_LINE3D = 0xB5,
            G_CLEARGEOMETRYMODE = 0xB6,
            G_SETGEOMETRYMODE = 0xB7,
            G_ENDDL = 0xB8,
            G_SETOTHERMODE_L = 0xB9,
            G_SETOTHERMODE_H = 0xBA,
            G_TEXTURE = 0xBB,
            G_MOVEWORD = 0xBC,
            G_POPMTX = 0xBD,
            G_CULLDL = 0xBE,
            G_TRI1 = 0xBF,

            // RDP |11xxxxxx|
            G_TEXRECT = 0xE4,
            G_TEXRECTFLIP = 0xE5,
            G_RDPLOADSYNC = 0xE6,
            G_RDPPIPESYNC = 0xE7,
            G_RDPTILESYNC = 0xE8,
            G_RDPFULLSYNC = 0xE9,
            G_SETKEYGB = 0xEA,
            G_SETKEYR = 0xEB,
            G_SETCONVERT = 0xEC,
            G_SETSCISSOR = 0xED,
            G_SETPRIMDEPTH = 0xEE,
            G_RDPSETOTHERMODE = 0xEF,
            G_LOADTLUT = 0xF0,
            G_SETTILESIZE = 0xF2,
            G_LOADBLOCK = 0xF3,
            G_LOADTILE = 0xF4,
            G_SETTILE = 0xF5,
            G_FILLRECT = 0xF6,
            G_SETFILLCOLOR = 0xF7,
            G_SETFOGCOLOR = 0xF8,
            G_SETBLENDCOLOR = 0xF9,
            G_SETPRIMCOLOR = 0xFA,
            G_SETENVCOLOR = 0xFB,
            G_SETCOMBINE = 0xFC,
            G_SETTIMG = 0xFD,
            G_SETZIMG = 0xFE,
            G_SETCIMG = 0xFF,
        }

        public static string DecodeList(uint address)
        {
            var res = new StringBuilder();

            for(int i = 0; i < MaxDisplayListLength; i++)
            {
                
                var firstWord = Config.Stream.GetUInt32(address);
                var secondWord = Config.Stream.GetUInt32(address + 4);
                var opcode = (F3DOpcode)((firstWord >> 24) & 0xFF);
                var name = Enum.GetName(typeof(F3DOpcode), opcode);
                res.AppendLine($"{firstWord:X8} {secondWord:X8} "+name);

                if (opcode == F3DOpcode.G_ENDDL) break;
                address += 4;
            }

            return res.ToString();
        }

        // A segmented address is 4 bytes. The first byte contains the index of the segment in the segment table, the 
        // other 3 bytes are the offset from the segment. Segmented addresses are used for locating object behavior scripts, 
        // display lists, textures and other resources.
        // todo: not limited to Fast3D display lists, can be put in a more general utilitiies class
        public static uint DecodeSegmentedAddress(uint address)
        {
            var val = Config.Stream.GetUInt32(address);
            var offset = val & 0xFFFFFF;
            var segment = (val >> 24) & 0xFF;
            return offset + Config.Stream.GetUInt32(4 * segment + Config.SwitchRomVersion(0x33b400, 0x33A090));
        }
    }
}
