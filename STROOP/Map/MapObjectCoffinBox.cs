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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectCoffinBox : MapObjectLine
    {
        private readonly PositionAngle _posAngle;

        public MapObjectCoffinBox(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            OutlineWidth = 3;
            OutlineColor = Color.Magenta;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            (double x, double y, double z, double angle) = _posAngle.GetValues();
            (double frontCenterX, double frontCenterZ) = MoreMath.AddVectorToPoint(150, angle, x, z);
            (double backCenterX, double backCenterZ) = MoreMath.AddVectorToPoint(-450, angle, x, z);
            (double frontLeftX, double frontLeftZ) = MoreMath.AddVectorToPoint(140, angle + 16384, frontCenterX, frontCenterZ);
            (double frontRightX, double frontRightZ) = MoreMath.AddVectorToPoint(140, angle - 16384, frontCenterX, frontCenterZ);
            (double backLeftX, double backLeftZ) = MoreMath.AddVectorToPoint(140, angle + 16384, backCenterX, backCenterZ);
            (double backRightX, double backRightZ) = MoreMath.AddVectorToPoint(140, angle - 16384, backCenterX, backCenterZ);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();

            vertices.Add(((float)frontLeftX, (float)y, (float)frontLeftZ));
            vertices.Add(((float)frontRightX, (float)y, (float)frontRightZ));

            vertices.Add(((float)frontRightX, (float)y, (float)frontRightZ));
            vertices.Add(((float)backRightX, (float)y, (float)backRightZ));

            vertices.Add(((float)backRightX, (float)y, (float)backRightZ));
            vertices.Add(((float)backLeftX, (float)y, (float)backLeftZ));

            vertices.Add(((float)backLeftX, (float)y, (float)backLeftZ));
            vertices.Add(((float)frontLeftX, (float)y, (float)frontLeftZ));

            return vertices;
        }

        public override string GetName()
        {
            return "Coffin Box";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CoffinBoxImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
            };
        }
    }
}
