using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;

namespace STROOP.Map3
{
    public abstract class Map3TriangleObject : Map3Object
    {
        public Map3TriangleObject()
            : base()
        {
        }

        public override void DrawOnControl()
        {
            if (ShowTriUnits)
            {
                DrawOnControlWithUnits();
            }
            else
            {
                DrawOnControlWithoutUnits();
            }
        }

        private void DrawOnControlWithoutUnits()
        {
            List<List<(float x, float z)>> vertexLists = GetVertexLists();
            List<List<(float x, float z)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.z)));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.Triangles);
            foreach (List<(float x, float z)> vertexList in vertexListsForControl)
            {
                foreach ((float x, float z) in vertexList)
                {
                    GL.Vertex2(x, z);
                }
            }
            GL.End();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                foreach (List<(float x, float z)> vertexList in vertexListsForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z) in vertexList)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        private void DrawOnControlWithUnits()
        {
            List<List<(float x, float z)>> triVertexLists = GetVertexLists();
            List<(int x, int z)> unitPoints = triVertexLists.ConvertAll(vertexList =>
            {
                if (vertexList.Count == 0) return new List<(int x, int z)>();

                int xMin = (int)Math.Max(vertexList.Min(vertex => vertex.x), Config.Map3Graphics.MapViewXMin - 1);
                int xMax = (int)Math.Min(vertexList.Max(vertex => vertex.x), Config.Map3Graphics.MapViewXMax + 1);
                int zMin = (int)Math.Max(vertexList.Min(vertex => vertex.z), Config.Map3Graphics.MapViewZMin - 1);
                int zMax = (int)Math.Min(vertexList.Max(vertex => vertex.z), Config.Map3Graphics.MapViewZMax + 1);

                List<(int x, int z)> points = new List<(int x, int z)>();
                for (int x = xMin; x <= xMax; x++)
                {
                    for (int z = zMin; z <= zMax; z++)
                    {
                        if (MoreMath.IsPointInsideTriangle(
                            x, z,
                            vertexList[0].x, vertexList[0].z,
                            vertexList[1].x, vertexList[1].z,
                            vertexList[2].x, vertexList[2].z))
                        {
                            points.Add((x, z));
                        }
                    }
                }
                return points;
            }).SelectMany(points => points).Distinct().ToList();

            List<List<(float x, float z)>> quadList = Map3Utilities.ConvertUnitPointsToQuads(unitPoints);
            List<List<(float x, float z)>> quadListForControl =
                quadList.ConvertAll(quad => quad.ConvertAll(
                    vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.z)));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.Quads);
            foreach (List<(float x, float z)> quad in quadListForControl)
            {
                foreach ((float x, float z) in quad)
                {
                    GL.Vertex2(x, z);
                }
            }
            GL.End();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                foreach (List<(float x, float z)> quad in quadListForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z) in quad)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        protected abstract List<List<(float x, float z)>> GetVertexLists();
    }
}
