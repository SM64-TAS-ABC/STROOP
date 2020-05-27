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
    public class MapLineSegmentObject : MapLineObject
    {
        private PositionAngle _posAngle1;
        private PositionAngle _posAngle2;

        public MapLineSegmentObject(PositionAngle posAngle1, PositionAngle posAngle2)
            : base()
        {
            _posAngle1 = posAngle1;
            _posAngle2 = posAngle2;

            Size = 1;
            OutlineWidth = 3;
            OutlineColor = Color.Red;
        }

        public static MapObject Create(string text1, string text2)
        {
            PositionAngle posAngle1 = PositionAngle.FromString(text1);
            PositionAngle posAngle2 = PositionAngle.FromString(text2);
            if (posAngle1 == null || posAngle2 == null) return null;
            return new MapLineSegmentObject(posAngle1, posAngle2);
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            vertices.Add(((float)_posAngle1.X, (float)_posAngle1.Y, (float)_posAngle1.Z));
            vertices.Add(((float)_posAngle2.X, (float)_posAngle2.Y, (float)_posAngle2.Z));
            return vertices;
        }

        public override string GetName()
        {
            return "Line Segment";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.ArrowImage;
        }
    }
}
