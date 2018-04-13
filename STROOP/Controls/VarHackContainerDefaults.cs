using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class VarHackContainerDefaults
    {
        public static readonly string StaticVarName = "";
        public static readonly uint StaticAddres = 0x8033B1AC;
        public static readonly Type StaticMemoryType = typeof(float);
        public static readonly bool StaticUseHex = false;
        public static readonly uint StaticPointerOffset = 0x10;
        public static readonly bool StaticNoNum = false;

        public readonly string SpecialType;
        public readonly string VarName;
        public readonly uint Address;
        public readonly Type MemoryType;
        public readonly bool UseHex;
        public readonly uint? PointerOffset;
        public readonly bool NoNum;
        public readonly int XPos;
        public readonly int YPos;

        public VarHackContainerDefaults(int creationIndex)
        {
            XPos = VarHackConfig.DefaultXPos;
            YPos = VarHackConfig.DefaultYPos - creationIndex * VarHackConfig.DefaultYDelta;
            UseHex = false;
            PointerOffset = null;
            SpecialType = null;
            NoNum = false;

            switch (creationIndex)
            {
                case 0:
                    VarName = "HSPD ";
                    Address = MarioConfig.StructAddress + MarioConfig.HSpeedOffset;
                    MemoryType = typeof(float);
                    break;
                case 1:
                    VarName = "Angle ";
                    Address = MarioConfig.StructAddress + MarioConfig.FacingYawOffset;
                    MemoryType = typeof(ushort);
                    break;
                case 2:
                    VarName = "HP ";
                    Address = MarioConfig.StructAddress + HudConfig.HpCountOffset;
                    MemoryType = typeof(short);
                    UseHex = true;
                    break;
                case 3:
                    VarName = "Floor Room ";
                    Address = MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset;
                    MemoryType = typeof(byte);
                    PointerOffset = 0x05;
                    break;
                case 4:
                    VarName = "X ";
                    Address = MarioConfig.StructAddress + MarioConfig.XOffset;
                    MemoryType = typeof(float);
                    break;
                case 5:
                    VarName = "Y ";
                    Address = MarioConfig.StructAddress + MarioConfig.YOffset;
                    MemoryType = typeof(float);
                    break;
                case 6:
                    VarName = "Z ";
                    Address = MarioConfig.StructAddress + MarioConfig.ZOffset;
                    MemoryType = typeof(float);
                    break;
                case 7:
                    VarName = "HOLP X ";
                    Address = MarioConfig.StructAddress + MarioConfig.HolpXOffset;
                    MemoryType = typeof(float);
                    break;
                case 8:
                    VarName = "HOLP Y ";
                    Address = MarioConfig.StructAddress + MarioConfig.HolpYOffset;
                    MemoryType = typeof(float);
                    break;
                case 9:
                default:
                    VarName = "HOLP Z ";
                    Address = MarioConfig.StructAddress + MarioConfig.HolpZOffset;
                    MemoryType = typeof(float);
                    break;
            }
        }
    }
}
