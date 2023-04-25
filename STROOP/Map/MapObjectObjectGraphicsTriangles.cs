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
            List<List<(float x, float y, float z)>> triangles =
                new List<List<(float x, float y, float z)>>();
            List<(float x, float y, float z)> vertices =
                new List<(float x, float y, float z)>();

            uint address = 0x80400800;
            while (true)
            {
                uint command = Config.Stream.GetUInt(address);
                uint commandID = command >> 24;

                if (commandID == 0x06) // tag command
                {
                    address += 8;
                }
                else if (commandID == 0x04) // vertex command
                {
                    uint numVertices = (command & 0xFFFF) / 16;
                    address += 4;

                    vertices = new List<(float x, float y, float z)>();
                    for (int i = 0; i < numVertices; i++)
                    {
                        float x = Config.Stream.GetFloat(address);
                        address += 4;
                        float y = Config.Stream.GetFloat(address);
                        address += 4;
                        float z = Config.Stream.GetFloat(address);
                        address += 4;

                        vertices.Add((x, y, z));
                    }
                }
                else if (commandID == 0xBF) // triangle command
                {
                    uint xIndex10 = (command >> 16) & 0xFF;
                    uint yIndex10 = (command >> 8) & 0xFF;
                    uint zIndex10 = command & 0xFF;

                    int xIndex = (int)xIndex10 / 10;
                    int yIndex = (int)yIndex10 / 10;
                    int zIndex = (int)zIndex10 / 10;

                    triangles.Add(new List<(float x, float y, float z)>()
                    {
                        vertices[xIndex],
                        vertices[yIndex],
                        vertices[zIndex],
                    });

                    address += 4;
                }
                else if (commandID == 0xB8) // end command
                {
                    return triangles;
                }
                else
                {
                    return triangles;
                }
            }
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

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem useThisForGfxTrisItem = new ToolStripMenuItem("Use This for Gfx Tris");
                useThisForGfxTrisItem.Click += (sender, e) =>
                {
                    Config.Stream.SetValue(_posAngle.GetObjAddress(), 0x804007F0);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(useThisForGfxTrisItem);
            }

            return _contextMenuStrip;
        }
    }
}
