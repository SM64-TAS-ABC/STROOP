using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
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
        public static readonly uint X1 = 0x0A;
        public static readonly uint Y1 = 0x0C;
        public static readonly uint Z1 = 0x0E;
        public static readonly uint X2 = 0x10;
        public static readonly uint Y2 = 0x12;
        public static readonly uint Z2 = 0x14;
        public static readonly uint X3 = 0x16;
        public static readonly uint Y3 = 0x18;
        public static readonly uint Z3 = 0x1A;
        public static readonly uint NormX = 0x1C;
        public static readonly uint NormY = 0x20;
        public static readonly uint NormZ = 0x24;
        public static readonly uint NormOffset = 0x28;
        public static readonly uint AssociatedObject = 0x2C;

        public static readonly byte BelongsToObjectMask = 0x01;
        public static readonly byte NoCamCollisionMask = 0x02;
        public static readonly byte XProjectionMask = 0x08;
    }
}
