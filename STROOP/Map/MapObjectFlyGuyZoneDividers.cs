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
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectFlyGuyZoneDividers : MapObject
    {
        private readonly PositionAngle _posAngle;

        public MapObjectFlyGuyZoneDividers(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            LineWidth = 3;
            LineColor = Color.Blue;
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            if (LineWidth == 0) return;

            var top = MapUtilities.ConvertCoordsForControlOrthographicView((float)_posAngle.X, (float)_posAngle.Y - 200, (float)_posAngle.Z, UseRelativeCoordinates);
            var bottom = MapUtilities.ConvertCoordsForControlOrthographicView((float)_posAngle.X, (float)_posAngle.Y - 400, (float)_posAngle.Z, UseRelativeCoordinates);

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(LineColor.R, LineColor.G, LineColor.B, OpacityByte);
            GL.LineWidth(LineWidth);
            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(0, top.z);
            GL.Vertex2(Config.MapGui.CurrentControl.Width, top.z);
            GL.Vertex2(0, bottom.z);
            GL.Vertex2(Config.MapGui.CurrentControl.Width, bottom.z);

            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
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

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.FacingDividerImage;
        }

        public override string GetName()
        {
            return "Fly Guy Zone Divider for " + _posAngle.GetMapName();
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
