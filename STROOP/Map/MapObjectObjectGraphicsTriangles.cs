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

            Color = Color.Yellow;
            Opacity = 0.5;
        }

        private List<List<(float x, float y, float z)>> GetVertexLists()
        {
            return new List<List<(float x, float y, float z)>>()
            {
                new List<(float x, float y, float z)>()
                {
                    (100, 100, 100),
                    (-100, 200, 100),
                    (-100, 300, -100),
                },
            };
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            List<List<(float x, float y, float z)>> vertexLists = GetVertexLists();
            List<List<(float x, float z)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList =>
                {
                    return vertexList.ConvertAll(
                        vertex => MapUtilities.ConvertCoordsForControlTopDownView(
                            vertex.x, vertex.z, UseRelativeCoordinates));
                });

            // Draw triangle
            for (int i = 0; i < vertexListsForControl.Count; i++)
            {
                var vertexList = vertexListsForControl[i];
                GL.Begin(PrimitiveType.Polygon);
                foreach (var vertex in vertexList)
                {
                    GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
                    GL.Vertex2(vertex.x, vertex.z);
                }
                GL.End();
            }

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                foreach (var vertexList in vertexListsForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach (var vertex in vertexList)
                    {
                        GL.Vertex2(vertex.x, vertex.z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
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
