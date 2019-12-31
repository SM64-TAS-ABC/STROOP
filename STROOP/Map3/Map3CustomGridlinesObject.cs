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

namespace STROOP.Map3
{
    public class Map3CustomGridlinesObject : Map3LineObject
    {
        public Map3CustomGridlinesObject()
            : base()
        {
            Size = 1;
            OutlineWidth = 3;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            int size = (int)Size;
            if (size < 0) size = 0;
            int increment = (int)(16384 / Math.Pow(2, size));
            List<(float x, float z)> vertices = new List<(float x, float z)>();
            for (int x = -8192; x <= 8192; x += increment)
            {
                vertices.Add((x, -8192));
                vertices.Add((x, 8192));
            }
            for (int z = -8192; z <= 8192; z += increment)
            {
                vertices.Add((-8192, z));
                vertices.Add((8192, z));
            }
            return vertices;
        }

        public override string GetName()
        {
            return "Custom Gridlines";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.CustomGridlinesImage;
        }
    }
}
