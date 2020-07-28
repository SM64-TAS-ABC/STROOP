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
    public class MapDrawingObject : MapLineObject
    {
        private readonly List<(float x, float y, float z)> _vertices;

        public MapDrawingObject()
            : base()
        {
            OutlineWidth = 3;
            OutlineColor = Color.Red;

            _vertices = new List<(float x, float y, float z)>();
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            return _vertices;
        }

        public override string GetName()
        {
            return "Drawing";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.PathImage;
        }
    }
}
