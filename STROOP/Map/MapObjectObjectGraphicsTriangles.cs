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
using STROOP.Models;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectObjectGraphicsTriangles : MapObject
    {
        private readonly PositionAngle _posAngle;

        public MapObjectObjectGraphicsTriangles(PositionAngle posAngle)
        {
            _posAngle = posAngle;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {

        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {

        }

        public override void DrawOn3DControl()
        {

        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleOtherImage;
        }

        public override string GetName()
        {
            return "Gfx Tris for " + _posAngle.GetMapName();
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
