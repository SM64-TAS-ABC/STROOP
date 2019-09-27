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

namespace STROOP.Map3
{
    public class Map3CellGridlinesObject : Map3LineObject
    {
        public Map3CellGridlinesObject()
            : base()
        {
            OutlineWidth = 3;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            List<(float x, float z)> vertices = new List<(float x, float z)>();
            for (int x = -8192; x <= 8192; x += 1024)
            {
                vertices.Add((x, -8192));
                vertices.Add((x, 8192));
            }
            for (int z = -8192; z <= 8192; z += 1024)
            {
                vertices.Add((-8192, z));
                vertices.Add((8192, z));
            }
            return vertices;
        }

        public override string GetName()
        {
            return "Cell Gridlines";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.EmptyImage;
        }
    }
}
