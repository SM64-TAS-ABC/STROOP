using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Drawing;
using STROOP.Structs;

namespace STROOP.M64Editor
{
    public class M64InputFrame
    {
        public static FrameInputRelation frameInputRelation = FrameInputRelation.FrameAfterInput;
        public static int ClassIdIndex = 0;

        public int FrameIndex;
        public uint RawValue;
        public readonly int IdIndex;

        public M64InputFrame(int frameIndex, uint rawValue)
        {
            FrameIndex = frameIndex;
            RawValue = rawValue;
            IdIndex = ClassIdIndex;
            ClassIdIndex++;
        }

        public int Frame { get => FrameIndex + GetFrameInputRelationOffset(); }
        public int Id { get => IdIndex + GetFrameInputRelationOffset(); }
        public sbyte X { get => (sbyte)GetByte(2); set => SetByte(2, (byte)value); }
        public sbyte Y { get => (sbyte)GetByte(3); set => SetByte(3, (byte)value); }
        public bool A { get => GetBit(7); set => SetBit(7, value); }
        public bool B { get => GetBit(6); set => SetBit(6, value); }
        public bool Z { get => GetBit(5); set => SetBit(5, value); }
        public bool S { get => GetBit(4); set => SetBit(4, value); }
        public bool R { get => GetBit(12); set => SetBit(12, value); }
        public bool C_Up { get => GetBit(11); set => SetBit(11, value); }
        public bool C_Down { get => GetBit(10); set => SetBit(10, value); }
        public bool C_Left { get => GetBit(9); set => SetBit(9, value); }
        public bool C_Right { get => GetBit(8); set => SetBit(8, value); }
        public bool L { get => GetBit(13); set => SetBit(13, value); }
        public bool D_Up { get => GetBit(3); set => SetBit(3, value); }
        public bool D_Down { get => GetBit(2); set => SetBit(2, value); }
        public bool D_Left { get => GetBit(1); set => SetBit(1, value); }
        public bool D_Right { get => GetBit(0); set => SetBit(0, value); }

        private void SetByte(int num, byte value)
        {
            uint mask = ~(uint)(0xFF << (num * 8));
            RawValue = ((uint)(value << (num * 8)) | (RawValue & mask));
        }

        private byte GetByte(int num)
        {
            return (byte)(RawValue >> (num * 8));
        }

        private void SetBit(int bit, bool value)
        {
            uint mask = (uint)(1 << bit);
            if (value)
            {
                RawValue |= mask;
            }
            else
            {
                RawValue &= ~mask;
            }
        }

        private bool GetBit(int bit)
        {
            return ((RawValue >> bit) & 0x01) == 0x01;
        }

        private int GetFrameInputRelationOffset()
        {
            switch (frameInputRelation)
            {
                case FrameInputRelation.FrameOfInput:
                    return -1;
                case FrameInputRelation.FrameAfterInput:
                    return 0;
                case FrameInputRelation.FrameWhenObserved:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(RawValue).ToArray();
        }
    }
}
