using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class BehaviorDecoder
    {
        public enum BehaviorCommandType
        {
            Start = 0x00,
            Cmd_01 = 0x01,
            Cmd_02 = 0x02,
            Cmd_03 = 0x03,
            Jump = 0x04,
            ForLoopBegin = 0x05,
            ForLoopEnd = 0x06,
            JumpBack = 0x07,
            LoopStart = 0x08,
            LoopEnd = 0x09,
            Empty_0A = 0x0A,
            Empty_0B = 0x0B,
            Call = 0x0C,
            SetPositionOffset = 0x0D,
            SetSightDistance = 0x0E,
            SetAnimationRate = 0x0F,
            SetParameter = 0x10,
            LogicalOr = 0x11,
            LogicalAnd = 0x12,
            Cmd_13 = 0x13,
            Cmd_14 = 0x14,
            Cmd_15 = 0x15,
            Cmd_16 = 0x16,
            Cmd_17 = 0x17,
            Empty_18 = 0x18,
            Empty_19 = 0x19,
            Empty_1A = 0x1A,
            SetModelID = 0x1B,
            SpawnChildObject = 0x1C,
            Deactivate = 0x1D,
            PositionOnGround = 0x1E,
            SetWaves = 0x1F,
            Cmd_20 = 0x20,
            ConfigAsBillboard = 0x21,
            Cmd_22 = 0x22,
            SetHitboxSphere = 0x23,
            Empty_24 = 0x24,
            Cmd_25 = 0x25,
            Cmd_26 = 0x26,
            SetUInt32 = 0x27,
            Animate = 0x28,
            LoadChildObjectWParam = 0x29,
            SetHitbox = 0x2A,
            SetHitboxSphere2 = 0x2B,
            SpawnObject = 0x2C,
            SetHome = 0x2D,
            Cmd_2E = 0x2E,
            Cmd_2F = 0x2F,
            SetPhysics = 0x30,
            Cmd_31 = 0x31,
            SetScaleUniform = 0x32,
            SetChildObject = 0x33,
            Cmd_34 = 0x34,
            Cmd_35 = 0x35,
            Cmd_36 = 0x36,
            Cmd_37 = 0x37,
        };

        static readonly Dictionary<BehaviorCommandType, int> BehaviorCommandLength = new Dictionary<BehaviorCommandType, int>()
        {
            { BehaviorCommandType.Start, 4 },
            { BehaviorCommandType.Cmd_01, 4 },
            { BehaviorCommandType.Cmd_02, 8 },
            { BehaviorCommandType.Cmd_03, 4 },
            { BehaviorCommandType.Jump, 8 },
            { BehaviorCommandType.ForLoopBegin, 4 },
            { BehaviorCommandType.ForLoopEnd, 4 },
            { BehaviorCommandType.JumpBack, 4 },
            { BehaviorCommandType.LoopStart, 4 },
            { BehaviorCommandType.LoopEnd, 4 },
            { BehaviorCommandType.Empty_0A, 4 },
            { BehaviorCommandType.Empty_0B, 4 },
            { BehaviorCommandType.Call, 8 },
            { BehaviorCommandType.SetPositionOffset, 4 },
            { BehaviorCommandType.SetSightDistance, 4 },
            { BehaviorCommandType.SetAnimationRate, 4 },
            { BehaviorCommandType.SetParameter, 4 },
            { BehaviorCommandType.LogicalOr, 4 },
            { BehaviorCommandType.LogicalAnd, 4 },
            { BehaviorCommandType.Cmd_13, 8 },
            { BehaviorCommandType.Cmd_14, 8 },
            { BehaviorCommandType.Cmd_15, 4 },
            { BehaviorCommandType.Cmd_16, 8 },
            { BehaviorCommandType.Cmd_17, 8 },
            { BehaviorCommandType.Empty_18, 4 },
            { BehaviorCommandType.Empty_19, 4 },
            { BehaviorCommandType.Empty_1A, 4 },
            { BehaviorCommandType.SetModelID, 4 },
            { BehaviorCommandType.SpawnChildObject, 12 },
            { BehaviorCommandType.Deactivate, 4 },
            { BehaviorCommandType.PositionOnGround, 4 },
            { BehaviorCommandType.SetWaves, 4 },
            { BehaviorCommandType.Cmd_20, 4 },
            { BehaviorCommandType.ConfigAsBillboard, 4 },
            { BehaviorCommandType.Cmd_22, 4 },
            { BehaviorCommandType.SetHitboxSphere, 8 },
            { BehaviorCommandType.Empty_24, 4 },
            { BehaviorCommandType.Cmd_25, 4 },
            { BehaviorCommandType.Cmd_26, 4 },
            { BehaviorCommandType.SetUInt32, 8 },
            { BehaviorCommandType.Animate, 4 },
            { BehaviorCommandType.LoadChildObjectWParam, 12 },
            { BehaviorCommandType.SetHitbox, 8 },
            { BehaviorCommandType.SetHitboxSphere2, 12 },
            { BehaviorCommandType.SpawnObject, 12 },
            { BehaviorCommandType.SetHome, 4 },
            { BehaviorCommandType.Cmd_2E, 8 },
            { BehaviorCommandType.Cmd_2F, 8},
            { BehaviorCommandType.SetPhysics, 20 },
            { BehaviorCommandType.Cmd_31, 8 },
            { BehaviorCommandType.SetScaleUniform, 4 },
            { BehaviorCommandType.SetChildObject, 4 },
            { BehaviorCommandType.Cmd_34, 4 },
            { BehaviorCommandType.Cmd_35, 4 },
            { BehaviorCommandType.Cmd_36, 8 },
            { BehaviorCommandType.Cmd_37, 8 },
        };

        static Dictionary<short, string> OffsetNames = new Dictionary<short, string>()
        {
            {0x8C, "flags"},
            {0x9C, "collision_timer"},
            {0xA0, "x"},
            {0xA4, "y"},
            {0xA8, "z"},
            {0xAC, "x_speed"},
            {0xB0, "y_speed"},
            {0xB4, "z_speed"},
            {0xB8, "h_speed"},
            {0xE4, "gravity"},
        };

        private static string GetOffsetName(short offset)
        {
            if (OffsetNames.ContainsKey(offset))
                return $".{OffsetNames[offset]}";
            else
                return $"[0x{offset:X2}]";
        }

        public static string Decode(uint address)
        {
            int nextAddress = (int)(address & ~0x8000000);
            string decoded = "";
            int maxDecode;
            _indentationLevel = 0;
            for (maxDecode = 1000; maxDecode > 0 && nextAddress != -1; maxDecode--) // Don't loop forever, 
            {
                decoded += DecodeLine(ref nextAddress);
            }

            return maxDecode == 0 ? null : decoded; 
        }

        static int _indentationLevel = 0;
        static string DecodeLine(ref int lineAddress)
        {
            bool incrementIndentation = false;
            string decoded = "";
            uint address = (uint) lineAddress;
            var stream = Config.Stream;

            // Get command
            int cmdByte = stream.GetByte(address++);
            BehaviorCommandType? cmd = typeof(BehaviorCommandType).IsEnumDefined(cmdByte) ? 
                (BehaviorCommandType?) cmdByte : null;
            
            switch (cmd)
            {
                case BehaviorCommandType.Start:
                    {
                        byte processGroup = stream.GetByte(address++);
                        decoded = $"obj.process_group = 0x{processGroup:X2}";
                        break;
                    }
                case BehaviorCommandType.LoopStart:
                    {
                        decoded = "while(True):";
                        incrementIndentation = true;
                        break;
                    }
                case BehaviorCommandType.LoopEnd:
                    {
                        decoded = "";
                        _indentationLevel--;
                        break;
                    }
                case BehaviorCommandType.Call:
                    {
                        address += 3; // Ignored
                        uint function = stream.GetUInt32(address);
                        decoded = $"fn{function:X8}()";
                        break;
                    }
                case BehaviorCommandType.LogicalOr:
                    {
                        short offset = (short)(0x88 + stream.GetByte(address++) * 4);
                        ushort operand = BitConverter.ToUInt16(stream.ReadRam(address, sizeof(UInt16)), 0);
                        decoded = $"obj{GetOffsetName(offset)} |= 0x{operand:X4}";
                        break;
                    }
                case BehaviorCommandType.JumpBack:
                    {
                        decoded = $"ExecutePrevious()";
                        break;
                    }
                case BehaviorCommandType.SetHitbox:
                    {
                        address += 3; // Ignored
                        UInt32 hitboxPtr = BitConverter.ToUInt32(stream.ReadRam(address, sizeof(UInt32)), 0);
                        decoded = $"obj.hitbox_ptr = 0x{hitboxPtr:X8}";
                        break;
                    }
                case BehaviorCommandType.SetPositionOffset:
                case BehaviorCommandType.SetParameter:
                    {
                        short offset = (short)(0x88 + stream.GetByte(address++) * 4);
                        ushort operand = BitConverter.ToUInt16(stream.ReadRam(address, sizeof(UInt16)), 0);
                        decoded = $"obj{GetOffsetName(offset)} += 0x{operand:X4}";
                        break;
                    }
                case BehaviorCommandType.SetAnimationRate:
                case BehaviorCommandType.SetSightDistance:
                    {
                        short offset = (short)(0x88 + stream.GetByte(address++) * 4);
                        ushort operand = BitConverter.ToUInt16(stream.ReadRam(address, sizeof(UInt16)), 0);
                        decoded = $"obj{GetOffsetName(offset)} = 0x{operand:X4}";
                        break;
                    }
                case BehaviorCommandType.PositionOnGround:
                    {
                        decoded = $"obj.position_on_ground()\nobj[0xEC] = 2";
                        break;
                    }
                case BehaviorCommandType.SetHitboxSphere:
                    {
                        address += 3;
                        UInt16 xz = stream.GetUInt16(address);
                        address += 2;
                        UInt16 y = stream.GetUInt16(address);
                        decoded = $"obj.set_sphere_hitbox(radius_xz={xz}, radius_y={y})";
                        break;
                    }
                case BehaviorCommandType.SetHome:
                    {
                        decoded = $"obj.set_current_pos_as_home()";
                        break;
                    }
                case BehaviorCommandType.SetPhysics:
                    {
                        address += 3;
                        UInt16 minWallDistance = stream.GetUInt16(address);
                        address += 2;
                        float floorHeight = stream.GetUInt16(address) / 100.0f;
                        address += 2;
                        float bounce = stream.GetUInt16(address) / 100.0f;
                        address += 2;
                        float drag = stream.GetUInt16(address) / 100.0f;
                        address += 2;
                        float v_174 = stream.GetUInt16(address) / 100.0f;
                        address += 2;
                        float buoyancy = stream.GetUInt16(address) / 100.0f;
                        address += 2;
                        address += 4; // Ignored?
                        decoded = $"SetGravity(min_wall_distance={minWallDistance}, floor_height={floorHeight}, bounce={bounce}, drag={drag}, obj[0x174] = {v_174}), bouyancy={buoyancy})";
                        break;
                    }
                case BehaviorCommandType.Animate:
                    {
                        byte number = stream.GetByte(address++);
                        decoded = $"obj.animate(animation_number={number})";
                        break;
                    }
                case BehaviorCommandType.SpawnChildObject:
                    {
                        address += 3; // Ignored
                        UInt32 modelId = stream.GetUInt32(address);
                        address += 4;
                        UInt32 behavior = stream.GetUInt32(address);
                        decoded = $"obj.SpawnChildObject(model=0x{modelId:X8}, behavior=0x{behavior:X8})";
                        break;
                    }
                case BehaviorCommandType.SpawnObject:
                    {
                        address += 3; // Ignored
                        UInt32 modelId = stream.GetUInt32(address);
                        address += 4;
                        UInt32 behavior = stream.GetUInt32(address);
                        decoded = $"SpawnObject(model=0x{modelId:X8}, behavior=0x{behavior:X8})";
                        break;
                    }

                case BehaviorCommandType.Empty_0A:
                case BehaviorCommandType.Empty_0B:
                case BehaviorCommandType.Empty_18:
                case BehaviorCommandType.Empty_19:
                case BehaviorCommandType.Empty_1A:
                case BehaviorCommandType.Empty_24:
                    decoded = "nop()";
                    break;
                default:
                    decoded = $"Unknown {cmdByte:X2}({String.Join(" ", stream.ReadRam(address, BehaviorCommandLength[cmd.Value] - 1).Select(b => $"{b:X2}"))})";
                    break;
            }

            string indentation = new String('\t', _indentationLevel);
            decoded = $"{cmdByte:X2} {indentation}{decoded}\n";

            // Incremet address
            lineAddress += cmd.HasValue && BehaviorCommandLength.ContainsKey(cmd.Value) 
                ? BehaviorCommandLength[cmd.Value] : 4;

            // Check for end
            switch(cmd)
            {
                case BehaviorCommandType.LoopEnd:
                    lineAddress = -1; // End 
                    break;
            }

            if (incrementIndentation)
                _indentationLevel++;

            return decoded;
        }
    }
}
