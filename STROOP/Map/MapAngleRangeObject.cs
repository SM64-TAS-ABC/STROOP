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
    public class MapAngleRangeObject : MapLineObject
    {
        private readonly PositionAngle _posAngle;

        public MapAngleRangeObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Size = 1000;
            OutlineWidth = 1;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (int angle = 0; angle < 65536; angle += 16)
            {
                (double x1, double y1, double z1, double a) = _posAngle.GetValues();
                (double x2, double z2) = MoreMath.AddVectorToPoint(Size, angle, x1, z1);
                vertices.Add(((float)x1, (float)y1, (float)z1));
                vertices.Add(((float)x2, (float)y1, (float)z2));
            }
            return vertices;
        }

        public override string GetName()
        {
            return "Angle Range";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CustomGridlinesImage;
        }
    }
}
