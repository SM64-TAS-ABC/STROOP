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
    public class MapObjectCurrentCell : MapObjectQuad
    {
        private readonly PositionAngle _posAngle;

        public MapObjectCurrentCell(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Opacity = 0.5;
            Color = Color.Yellow;
        }

        protected override List<List<(float x, float y, float z, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData)
        {
            (float posAngleX, float posAngleY, float posAngleZ, float posAngleAngle) =
                ((float, float, float, float))_posAngle.GetValues();

            (int cellX, int cellZ) = WatchVariableSpecialUtilities.GetCell(posAngleX, posAngleZ);
            int xMin = (cellX - 8) * 1024;
            int xMax = xMin + 1024;
            int zMin = (cellZ - 8) * 1024;
            int zMax = zMin + 1024;
            List<(float x, float y, float z, bool isHovered)> quad =
                new List<(float x, float y, float z, bool isHovered)>()
                {
                    (xMin, posAngleY, zMin, false),
                    (xMin, posAngleY, zMax, false),
                    (xMax, posAngleY, zMax, false),
                    (xMax, posAngleY, zMin, false),
                };
            return new List<List<(float x, float y, float z, bool isHovered)>>() { quad };
        }

        public override string GetName()
        {
            return "Current Cell for " + _posAngle.GetMapName();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CurrentCellImage;
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
