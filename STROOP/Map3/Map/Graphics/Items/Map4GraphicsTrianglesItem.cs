using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map3.Map.Graphics.Items
{
    class Map4GraphicsTrianglesItem : Map4GraphicsItem
    {
        int _vertexBuffer = -1;

        Map4Vertex[] _loadedVertices = new Map4Vertex[] { new Map4Vertex(), new Map4Vertex(), new Map4Vertex() };
        Map4Vertex[] _newVertices = null;

        public int DisplayLayer { get; set; }

        public override IEnumerable<Type> DrawOnCameraTypes => CameraTypeAny;

        public override float? Depth => 0x10000 * DisplayLayer + Y;

        public float Y
        {
            get
            {
                return (_loadedVertices[0].Position.Y +
                        _loadedVertices[1].Position.Y +
                        _loadedVertices[2].Position.Y) / 3;
            }
        }

        public override DrawType Type => DrawType.Perspective;

        public Map4GraphicsTrianglesItem() : base(false)
        {
        }

        public void SetTriangles(IEnumerable<Map4Vertex> newVertices)
        {
            _newVertices = (Map4Vertex[]) newVertices?.ToArray().Clone();
        }

        public override void Load(Map4Graphics graphics)
        {
            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_loadedVertices.Length * Map4Vertex.Size),
                _loadedVertices, BufferUsageHint.DynamicDraw);
        }

        public override Matrix4 GetModelMatrix(Map4Graphics graphics)
        {
            return Matrix4.Identity;
        }

        public override void Draw(Map4Graphics graphics)
        {
            CheckUpdateTriangle();

            GL.BindTexture(TextureTarget.Texture2D, graphics.Utilities.WhiteTexture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _loadedVertices.Length);
        }

        public override void Unload(Map4Graphics graphics)
        {
            GL.DeleteBuffer(_vertexBuffer);
        }

        void CheckUpdateTriangle()
        {
            if (_newVertices == null)
                return;

            _loadedVertices = _newVertices;
            _newVertices = null;

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_loadedVertices.Length * Map4Vertex.Size),
                _loadedVertices, BufferUsageHint.DynamicDraw);
        }
    }
}
