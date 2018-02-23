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
        public readonly string _specialType;
        public readonly string _varName;
        public readonly uint _address;
        public readonly Type _memoryType;
        public readonly bool _useHex;
        public readonly uint? _pointerOffset;
        public readonly int _xPos;
        public readonly int _yPos;

        public VarHackContainerDefaults(int creationIndex)
        {
            _xPos = VarHackConfig.DefaultXPos;
            _yPos = VarHackConfig.DefaultYPos - creationIndex * VarHackConfig.DefaultYDelta;
            _useHex = false;
            _pointerOffset = null;
            _specialType = null;

            switch (creationIndex)
            {
                case 0:
                    _varName = "HSPD ";
                    _address = MarioConfig.StructAddress + MarioConfig.HSpeedOffset;
                    _memoryType = typeof(float);
                    break;
                case 1:
                    _varName = "Angle ";
                    _address = MarioConfig.StructAddress + MarioConfig.YawFacingOffset;
                    _memoryType = typeof(ushort);
                    break;
                case 2:
                    _varName = "HP ";
                    _address = MarioConfig.StructAddress + HudConfig.HpCountOffset;
                    _memoryType = typeof(short);
                    _useHex = true;
                    break;
                case 3:
                    _varName = "Floor Room ";
                    _address = MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset;
                    _memoryType = typeof(byte);
                    _pointerOffset = 0x05;
                    break;
                case 4:
                    _varName = "X ";
                    _address = MarioConfig.StructAddress + MarioConfig.XOffset;
                    _memoryType = typeof(float);
                    break;
                case 5:
                    _varName = "Y ";
                    _address = MarioConfig.StructAddress + MarioConfig.YOffset;
                    _memoryType = typeof(float);
                    break;
                case 6:
                    _varName = "Z ";
                    _address = MarioConfig.StructAddress + MarioConfig.ZOffset;
                    _memoryType = typeof(float);
                    break;
                case 7:
                    _varName = "HOLP X ";
                    _address = MarioConfig.StructAddress + MarioConfig.HOLPXOffset;
                    _memoryType = typeof(float);
                    break;
                case 8:
                    _varName = "HOLP Y ";
                    _address = MarioConfig.StructAddress + MarioConfig.HOLPYOffset;
                    _memoryType = typeof(float);
                    break;
                case 9:
                default:
                    _varName = "HOLP Z ";
                    _address = MarioConfig.StructAddress + MarioConfig.HOLPZOffset;
                    _memoryType = typeof(float);
                    break;
            }
        }
    }
}
