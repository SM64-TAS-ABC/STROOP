using System;
using System.Text;
using STROOP.Structs.Configurations;

namespace STROOP.Utilities
{
    /**
     * Decodes Fast 3D Display lists.
     * A display list is an array of 8 byte instructions that ends in a 'G_ENDDL' instruction
     * The instruction is contained in the first byte, the other 7 bytes are parameters depending on the type.
     * A notable instructions is the G_VTX instruction which buffers up to 16 vertices. Subsequently, G_TRI1
     * commands draw triangles using these vertices.
     */
    public class Fast3DDecoder
    {
        private const int MaxDisplayListLength = 1000; // Prevents long / endless loops when a malformed display list is decoded
        private const int MaxRecursionDepth = 5;

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

        // There are multiple color / alpha formats for textures 
        static readonly string[] ColorModes = { "RGBA", "YUV", "CI", "IA", "I" };

        // For indentation of the decoded list
        public static string Indent(int level)
        {
            return "".PadLeft(level * 4);
        }

        // Gives a string decoding the display list starting at a given address
        public static string DecodeList(uint address, int recursionDepth = 0)
        {
            if (recursionDepth > MaxRecursionDepth) return "Recursion too deep";

            var res = new StringBuilder();
            res.AppendLine(Indent(recursionDepth) + $"Decoding list at 0x{address:X8}");
            for (int i = 0; i < MaxDisplayListLength; i++)
            {
                var firstWord = Config.Stream.GetUInt32(address);
                var secondWord = Config.Stream.GetUInt32(address + 4);
                var opcode = (F3DOpcode)((firstWord >> 24) & 0xFF);
                var name = Enum.GetName(typeof(F3DOpcode), opcode);
                res.Append(Indent(recursionDepth) + $"{firstWord:X8} {secondWord:X8} " + name + " ");

                // todo: interpret the other commands
                switch (opcode)
                {
                    case F3DOpcode.G_LOADBLOCK:

                        res.AppendLine();
                        break;
                    case F3DOpcode.G_MOVEMEM:
                    case F3DOpcode.G_SETTIMG:

                        res.AppendLine($"0x{DecodeSegmentedAddress(secondWord):X8}");
                        break;

                    case F3DOpcode.G_MTX:
                        res.Append($"0x{DecodeSegmentedAddress(secondWord):X8} ");
                        var p = (firstWord >> 16) & 0xFF;
                        res.Append(((p & 0x01) != 0) ? "projection: " : "model view: ");
                        res.Append(((p & 0x02) != 0) ? "load" : "multiply");
                        res.AppendLine(((p & 0x04) != 0) ? "and push" : "and don't push");
                        break;
                    case F3DOpcode.G_SETTILESIZE:
                        var h = ((secondWord & 0xFFF) + 4) / 4;
                        var w = (((secondWord >> 12) & 0xFFF) + 4) / 4;
                        res.AppendLine($"{w} * {h}");
                        break;
                    case F3DOpcode.G_SETTILE:
                        var colorFormat = (firstWord >> 21) & 0x7;
                        res.Append(colorFormat < ColorModes.Length ? ColorModes[colorFormat] : "Invalid color mode");
                        int pixelBits = ((int)firstWord >> 19) & 0x3;
                        res.AppendLine(" " + (4 << pixelBits) + "-bit");
                        break;
                    case F3DOpcode.G_CLEARGEOMETRYMODE:
                    case F3DOpcode.G_SETGEOMETRYMODE:
                        res.AppendLine(GetGeometryFlags(secondWord));
                        break;
                    case F3DOpcode.G_VTX:
                        var vertexAmount = ((firstWord >> 20) & 0xF) + 1;
                        var startIndex = ((firstWord >> 16) & 0xF);
                        var vertexAddress = DecodeSegmentedAddress(secondWord);
                        res.AppendLine($"{vertexAmount} vertices at 0x{vertexAddress:X8}, start index {startIndex}");
                        res.AppendLine(Indent(recursionDepth) + "(pos) flags (tex) (normal/color)");
                        for (byte j = 0; j < vertexAmount; j++)
                        {
                            uint add = (uint)(vertexAddress + (j * 0x10));
                            var x = Config.Stream.GetInt16(add + 0x00);
                            var y = Config.Stream.GetInt16(add + 0x02);
                            var z = Config.Stream.GetInt16(add + 0x04);
                            var flags = Config.Stream.GetUInt16(add + 0x06);
                            var texX = Config.Stream.GetInt16(add + 0x08);
                            var texY = Config.Stream.GetInt16(add + 0x0A);
                            var r = Config.Stream.GetByte(add + 0x0C);
                            var g = Config.Stream.GetByte(add + 0x0D);
                            var b = Config.Stream.GetByte(add + 0x0E);
                            var a = Config.Stream.GetByte(add + 0x0F);
                            res.AppendLine(Indent(recursionDepth) + $"({x}, {y}, {z}) 0x{flags:X4} ({texX}, {texY}) ({r}, {g}, {b}, {a})");
                        }
                        break;
                    case F3DOpcode.G_TRI1:
                        var v1 = ((secondWord >> 16) & 0xFF) / 0x0A;
                        var v2 = ((secondWord >> 8) & 0xFF) / 0x0A;
                        var v3 = ((secondWord >> 0) & 0xFF) / 0x0A;
                        res.AppendLine($"indices ({v1}, {v2}, {v3})");
                        break;
                    case F3DOpcode.G_DL:
                        res.AppendLine();
                        res.AppendLine(DecodeList(DecodeSegmentedAddress(secondWord), recursionDepth + 1));
                        break;
                    default:
                        res.AppendLine();
                        break;
                }

                if (opcode == F3DOpcode.G_ENDDL) break;
                address += 8;   //Fast 3D instructions are always 8 bytes long
            }

            return res.ToString();
        }

        enum GeometryFlags
        {
            G_ZBUFFER = 0x0000000,
            G_SHADE = 0x00000004,
            G_TEXTURE_ENABLE = 0x00000002,
            G_SMOOTH = 0x00000200,
            G_CULL_FRONT = 0x00001000,
            G_CULL_BACK = 0x00002000,
            G_FOG = 0x00010000,
            G_LIGHTING = 0x00020000,
            G_TEXTURE_GEN = 0x00040000,
            G_TEXTURE_GEN_LINEAR = 0x00080000,
        }

        public static string GetGeometryFlags(uint word)
        {
            string res = "";
            foreach (var flag in (int[])Enum.GetValues(typeof(GeometryFlags)))
            {
                if ((word & flag) != 0) res += Enum.GetName(typeof(GeometryFlags), word) + " ";
            }
            return res;
        }

        // A segmented address is 4 bytes. The first byte contains the index of the segment in the segment table, the 
        // other 3 bytes are the offset from the segment. Segmented addresses are used for locating object behavior scripts, 
        // display lists, textures and other resources.
        // todo: not limited to Fast3D display lists, can be put in a more general utilitiies class
        public static uint DecodeSegmentedAddress(uint segmentedAddress)
        {
            var offset = segmentedAddress & 0xFFFFFF;
            var segment = (segmentedAddress >> 24);
            return offset + Config.Stream.GetUInt32(4 * segment + RomVersionConfig.Switch(0x33B400, 0x33A090));
        }
    }
}
