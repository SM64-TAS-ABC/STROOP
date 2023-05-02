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
using STROOP.Map.Map3D;
using System.Windows.Forms;
using STROOP.Controls;
using STROOP.Forms;
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectRenderTable : MapObject
    {
        private PositionAngle _posAngle;

        public MapObjectRenderTable(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<List<(float x, float y, Color color)>> squares = GetSquares();

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw square
            foreach (var square in squares)
            {
                GL.Color3(square[0].color);
                GL.Begin(PrimitiveType.Polygon);
                foreach (var point in square)
                {
                    GL.Vertex2(point.x, point.y);
                }
                GL.End();
            }

            // Draw outline
            foreach (var square in squares)
            {
                GL.Color3(Color.Black);
                GL.LineWidth(LineWidth);
                GL.Begin(PrimitiveType.LineLoop);
                foreach (var point in square)
                {
                    GL.Vertex2(point.x, point.y);
                }
                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        private List<List<(float x, float y, Color color)>> GetSquares()
        {
            float centerX = Config.MapGui.CurrentControl.Width / 2;
            float centerY = Config.MapGui.CurrentControl.Height / 2;
            float squareDiameter = 150;

            List<List<(float x, float y, Color color)>> squares =
                new List<List<(float x, float y, Color color)>>();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (x == 0 && y == 0) continue;

                    Color color = Color.White;

                    float squareCenterX = centerX + (x - 1) * squareDiameter;
                    float squareCenterY = centerY + (y - 1) * squareDiameter;
                    List<(float x, float y, Color color)> square =
                        new List<(float x, float y, Color color)>()
                        {
                            (squareCenterX - squareDiameter / 2, squareCenterY - squareDiameter / 2, color),
                            (squareCenterX - squareDiameter / 2, squareCenterY + squareDiameter / 2, color),
                            (squareCenterX + squareDiameter / 2, squareCenterY + squareDiameter / 2, color),
                            (squareCenterX + squareDiameter / 2, squareCenterY - squareDiameter / 2, color),
                        };
                    squares.Add(square);
                }
            }
            return squares;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CompassImage;
        }

        public override string GetName()
        {
            return "Render Table";
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
