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
using STROOP.Map3.Map.Graphics;
using STROOP.Models;

namespace STROOP.Map3
{
    public abstract class Map3TriangleObject : Map3Object
    {
        public Map3TriangleObject()
            : base()
        {
        }

        public override void DrawOn3DControl()
        {
            List<List<(float x, float y, float z)>> vertexLists = GetVertexLists();
            Map4Vertex[] vertexArray = vertexLists.SelectMany(vertexList => vertexList).ToList()
                .ConvertAll(vertex => new Map4Vertex(new Vector3(vertex.x, vertex.y, vertex.z), Color4)).ToArray();

            int buffer = GL.GenBuffer();
            GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArray.Length * Map4Vertex.Size), vertexArray, BufferUsageHint.DynamicDraw);
            Config.Map4Graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexArray.Length);
            GL.DeleteBuffer(buffer);
        }

        protected List<List<(float x, float y, float z)>> GetVertexLists()
        {
            return GetTriangles().ConvertAll(tri => tri.Get3DVertices());
        }

        protected abstract List<TriangleDataModel> GetTriangles();
    }
}
