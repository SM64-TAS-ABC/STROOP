using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;

namespace STROOP.Map
{
    public class MapPuGridlinesObject : MapLineObject
    {
        public MapPuGridlinesObject()
            : base()
        {
            OutlineWidth = 1;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            int xMin = ((((int)Config.MapGraphics.MapViewXMin) / 65536) - 1) * 65536;
            int xMax = ((((int)Config.MapGraphics.MapViewXMax) / 65536) + 1) * 65536;
            int zMin = ((((int)Config.MapGraphics.MapViewZMin) / 65536) - 1) * 65536;
            int zMax = ((((int)Config.MapGraphics.MapViewZMax) / 65536) + 1) * 65536;

            List<(float x, float z)> vertices = new List<(float x, float z)>();
            for (int x = xMin; x <= xMax; x += 65536)
            {
                vertices.Add((x, zMin));
                vertices.Add((x, zMax));
            }
            for (int z = zMin; z <= zMax; z += 65536)
            {
                vertices.Add((xMin, z));
                vertices.Add((xMax, z));
            }
            return vertices;
        }

        public override string GetName()
        {
            return "PU Gridlines";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.UnitGridlinesImage;
        }
    }
}
