using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public static class MoreMath
    {
        public static double DistanceTo(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float dx, dy, dz;
            dx = x1 - x1;
            dy = y1 - y2;
            dz = z1 - z2;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static double DistanceTo(float x1, float y1, float x2, float y2)
        {
            float dx, dy;
            dx = x1 - x1;
            dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
