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

namespace STROOP.Map3
{
    public class Map3UnitGridlinesObject : Map3LineObject
    {
        public Map3UnitGridlinesObject()
            : base()
        {
            OutlineWidth = 1;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            int xMin = (int)Config.Map3Graphics.MapViewXMin - 10;
            int xMax = (int)Config.Map3Graphics.MapViewXMax + 10;
            int zMin = (int)Config.Map3Graphics.MapViewZMin - 10;
            int zMax = (int)Config.Map3Graphics.MapViewZMax + 10;

            List<(float x, float z)> vertices = new List<(float x, float z)>();
            for (int x = xMin; x <= xMax; x += 1)
            {
                vertices.Add((x, zMin));
                vertices.Add((x, zMax));
            }
            for (int z = zMin; z <= zMax; z += 1)
            {
                vertices.Add((xMin, z));
                vertices.Add((xMax, z));
            }
            return vertices;
        }

        public override string GetName()
        {
            return "Unit Gridlines";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.UnitGridlinesImage;
        }
    }
}
