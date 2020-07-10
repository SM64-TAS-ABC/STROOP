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

namespace STROOP.Map
{
    public class MapCustomGridlinesObject : MapLineObject
    {
        public MapCustomGridlinesObject()
            : base()
        {
            Size = 1;
            OutlineWidth = 3;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);

            int size = (int)Size;
            if (size < 0) size = 0;
            int increment = 16384 / (size + 1);
            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (int x = -8192; x <= 8192; x += increment)
            {
                vertices.Add((x, marioY, - 8192));
                vertices.Add((x, marioY, 8192));
            }
            for (int z = -8192; z <= 8192; z += increment)
            {
                vertices.Add((-8192, marioY, z));
                vertices.Add((8192, marioY, z));
            }
            return vertices;
        }

        public override string GetName()
        {
            return "Custom Gridlines";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CustomGridlinesImage;
        }
    }
}
