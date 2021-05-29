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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectCurrentUnit : MapObjectQuad
    {
        private readonly PositionAngle _posAngle;

        public MapObjectCurrentUnit(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Opacity = 0.5;
            Color = Color.Purple;
        }

        protected override List<List<(float x, float y, float z)>> GetQuadList()
        {
            (float posAngleX, float posAngleY, float posAngleZ, float posAngleAngle) =
                ((float, float, float, float))_posAngle.GetValues();

            int xMin = (short)posAngleX;
            int xMax = xMin + (posAngleX >= 0 ? 1 : -1);
            int zMin = (short)posAngleZ;
            int zMax = zMin + (posAngleZ >= 0 ? 1 : -1);

            List<(float x, float y, float z)> quad =
                new List<(float x, float y, float z)>()
                {
                    (xMin, posAngleY, zMin),
                    (xMin, posAngleY, zMax),
                    (xMax, posAngleY, zMax),
                    (xMax, posAngleY, zMin),
                };
            return new List<List<(float x, float y, float z)>>() { quad };
        }

        public override string GetName()
        {
            return "Current Unit for " + _posAngle.GetMapName();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CurrentUnitImage;
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
