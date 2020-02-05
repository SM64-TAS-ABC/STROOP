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

        protected override List<(float x, float y, float z)> GetVertices()
        {
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);

            int xMin = ((((int)Config.MapGraphics.MapViewXMin) / 65536) - 1) * 65536;
            int xMax = ((((int)Config.MapGraphics.MapViewXMax) / 65536) + 1) * 65536;
            int zMin = ((((int)Config.MapGraphics.MapViewZMin) / 65536) - 1) * 65536;
            int zMax = ((((int)Config.MapGraphics.MapViewZMax) / 65536) + 1) * 65536;

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (int x = xMin; x <= xMax; x += 65536)
            {
                vertices.Add((x, marioY, zMin));
                vertices.Add((x, marioY, zMax));
            }
            for (int z = zMin; z <= zMax; z += 65536)
            {
                vertices.Add((xMin, marioY, z));
                vertices.Add((xMax, marioY, z));
            }
            return vertices;
        }

        public override string GetName()
        {
            return "PU Gridlines";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.UnitGridlinesImage;
        }
    }
}
