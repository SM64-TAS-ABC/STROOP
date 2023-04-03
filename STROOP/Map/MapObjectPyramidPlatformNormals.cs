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
    public class MapObjectPyramidPlatformNormals : MapObject
    {
        private readonly PositionAngle _posAngle;

        public MapObjectPyramidPlatformNormals(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            // TODO: implement
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override string GetName()
        {
            return "Pyramid Platform Normals for " + _posAngle.GetMapName();
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
