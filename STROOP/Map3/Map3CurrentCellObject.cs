using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;

namespace STROOP.Map3
{
    public class Map3CurrentCellObject : Map3QuadObject
    {
        public Map3CurrentCellObject()
            : base()
        {
            Color = Color.Yellow;
            Opacity = 0.5;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            (int cellX, int cellZ) = WatchVariableSpecialUtilities.GetMarioCell();
            int xMin = (cellX - 8) * 1024;
            int xMax = xMin + 1024;
            int zMin = (cellZ - 8) * 1024;
            int zMax = zMin + 1024;
            return new List<(float x, float z)>()
            {
                (xMin, zMin),
                (xMin, zMax),
                (xMax, zMax),
                (xMax, zMin),
            };
        }
    }
}
