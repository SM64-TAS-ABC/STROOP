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
using STROOP.Models;
using System.Windows.Forms;
using STROOP.Forms;
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3
{
    public class Map3LevelTriangleObject : Map3TriangleObject
    {
        private readonly List<uint> _triAddressList;

        public Map3LevelTriangleObject()
            : base()
        {
            _triAddressList = TriangleUtilities.GetLevelTriangles()
                .ConvertAll(tri => tri.Address);

            OutlineWidth = 2;
        }

        public override void DrawOn2DControl()
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            List<List<(float x, float y, float z, Color color)>> triData = GetTriangles()
                .ConvertAll(tri => new List<(float x, float y, float z, Color color)>()
                {
                    (tri.X1, tri.Y1, tri.Z1, ColorUtilities.AddAlpha(GetColorForTri(tri), OpacityByte)),
                    (tri.X2, tri.Y2, tri.Z2, ColorUtilities.AddAlpha(GetColorForTri(tri), OpacityByte)),
                    (tri.X3, tri.Y3, tri.Z3, ColorUtilities.AddAlpha(GetColorForTri(tri), OpacityByte)),
                });
            Map4Vertex[] vertexArray = triData.SelectMany(vertexList => vertexList).ToList()
                .ConvertAll(vertex => new Map4Vertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), vertex.color)).ToArray();
            List<Map4Vertex[]> vertexArray2 = triData.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map4Vertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), OutlineColor)).ToArray());

            int buffer1 = GL.GenBuffer();
            GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer1);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArray.Length * Map4Vertex.Size), vertexArray, BufferUsageHint.DynamicDraw);
            Config.Map4Graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexArray.Length);
            GL.DeleteBuffer(buffer1);

            if (OutlineWidth != 0)
            {
                vertexArray2.ForEach(vertexes =>
                {
                    int buffer2 = GL.GenBuffer();
                    GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer2);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map4Vertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                    GL.LineWidth(OutlineWidth);
                    Config.Map4Graphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexes.Length);
                    GL.DeleteBuffer(buffer2);
                });
            }
        }

        private static Color GetColorForTri(TriangleDataModel tri)
        {
            double clampedNormY = MoreMath.Clamp(tri.NormY, -1, 1);
            switch (tri.Classification)
            {
                case TriangleClassification.Wall:
                    return tri.XProjection ? Color.FromArgb(58, 116, 58) : Color.FromArgb(116, 203, 116);
                case TriangleClassification.Floor:
                    return Color.FromArgb(130, 130, 231).Darken(0.6 * (1 - clampedNormY));
                case TriangleClassification.Ceiling:
                    return Color.FromArgb(231, 130, 130).Darken(0.6 * (clampedNormY + 1));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override List<TriangleDataModel> GetTriangles()
        {
            return Map3Utilities.GetTriangles(_triAddressList);
        }

        public override string GetName()
        {
            return "Level Tris";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.HolpImage;
        }

        public override void Update()
        {
            int numLevelTriangles = Config.Stream.GetInt32(0x80361178);
            if (_triAddressList.Count != numLevelTriangles)
            {
                _triAddressList.Clear();
                _triAddressList.AddRange(TriangleUtilities.GetLevelTriangles()
                    .ConvertAll(tri => tri.Address));
            }
        }
    }
}
