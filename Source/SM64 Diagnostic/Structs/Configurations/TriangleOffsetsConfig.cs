using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct TriangleOffsetsConfig
    {
        public uint SurfaceType;
        public uint ExertionForceIndex;
        public uint ExertionAngle;
        public uint Flags;
        public uint Room;
        public uint YMin;
        public uint YMax;
        public uint X1;
        public uint Y1;
        public uint Z1;
        public uint X2;
        public uint Y2;
        public uint Z2;
        public uint X3;
        public uint Y3;
        public uint Z3;
        public uint NormX;
        public uint NormY;
        public uint NormZ;
        public uint NormOffset;
        public uint AssociatedObject;

        public byte BelongsToObjectMask;
        public byte NoCamCollisionMask;
        public byte ProjectionMask;
    }
}
