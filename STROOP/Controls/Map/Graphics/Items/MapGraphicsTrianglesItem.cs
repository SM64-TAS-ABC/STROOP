using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map.Graphics.Items
{
    class MapGraphicsTrianglesItem : MapGraphicsItem
    {
        int _vertexBuffer = -1;

        Vertex[] _loadedVertices = new Vertex[] { new Vertex(), new Vertex(), new Vertex() };
        Vertex[] _newVertices = null;

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

        public MapGraphicsTrianglesItem() : base(false)
        {
        }

        public void SetTriangles(IEnumerable<Vertex> newVertices)
        {
            _newVertices = (Vertex[]) newVertices?.ToArray().Clone();
        }

        public override void Load(MapGraphics graphics)
        {
            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_loadedVertices.Length * Vertex.Size),
                _loadedVertices, BufferUsageHint.DynamicDraw);
        }

        public override Matrix4 GetModelMatrix(MapGraphics graphics)
        {
            return Matrix4.Identity;
        }

        public override void Draw(MapGraphics graphics)
        {
            CheckUpdateTriangle();

            GL.BindTexture(TextureTarget.Texture2D, graphics.Utilities.WhiteTexture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _loadedVertices.Length);
        }

        public override void Unload(MapGraphics graphics)
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
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_loadedVertices.Length * Vertex.Size),
                _loadedVertices, BufferUsageHint.DynamicDraw);
        }
    }
}
