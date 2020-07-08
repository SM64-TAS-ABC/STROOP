using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class TriangleOffsetsConfig
    {
        public static readonly uint SurfaceType = 0x00;
        public static readonly uint ExertionForceIndex = 0x02;
        public static readonly uint ExertionAngle = 0x03;
        public static readonly uint Flags = 0x04;
        public static readonly uint Room = 0x05;
        public static readonly uint YMin = 0x06;
        public static readonly uint YMax = 0x08;

        private static readonly uint X1 = 0x0A;
        private static readonly uint Y1 = 0x0C;
        private static readonly uint Z1 = 0x0E;
        private static readonly uint X2 = 0x10;
        private static readonly uint Y2 = 0x12;
        private static readonly uint Z2 = 0x14;
        private static readonly uint X3 = 0x16;
        private static readonly uint Y3 = 0x18;
        private static readonly uint Z3 = 0x1A;

        public static readonly uint NormX = 0x1C;
        public static readonly uint NormY = 0x20;
        public static readonly uint NormZ = 0x24;
        public static readonly uint NormOffset = 0x28;
        public static readonly uint AssociatedObject = 0x2C;

        public static readonly byte BelongsToObjectMask = 0x01;
        public static readonly byte NoCamCollisionMask = 0x02;
        public static readonly byte XProjectionMask = 0x08;

        public static short GetX1(uint triAddress) => Config.Stream.GetInt16(triAddress + X1);
        public static short GetY1(uint triAddress) => Config.Stream.GetInt16(triAddress + Y1);
        public static short GetZ1(uint triAddress) => Config.Stream.GetInt16(triAddress + Z1);
        public static short GetX2(uint triAddress) => Config.Stream.GetInt16(triAddress + X2);
        public static short GetY2(uint triAddress) => Config.Stream.GetInt16(triAddress + Y2);
        public static short GetZ2(uint triAddress) => Config.Stream.GetInt16(triAddress + Z2);
        public static short GetX3(uint triAddress) => Config.Stream.GetInt16(triAddress + X3);
        public static short GetY3(uint triAddress) => Config.Stream.GetInt16(triAddress + Y3);
        public static short GetZ3(uint triAddress) => Config.Stream.GetInt16(triAddress + Z3);

        public static bool SetX1(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + X1);
        public static bool SetY1(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + Y1);
        public static bool SetZ1(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + Z1);
        public static bool SetX2(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + X2);
        public static bool SetY2(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + Y2);
        public static bool SetZ2(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + Z2);
        public static bool SetX3(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + X3);
        public static bool SetY3(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + Y3);
        public static bool SetZ3(short value, uint triAddress) => Config.Stream.SetValue(value, triAddress + Z3);
    }
}
