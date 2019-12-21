using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK;

namespace STROOP.Map3.Map.Graphics.Items
{
    public class Map4GraphicsIconItem : Map4GraphicsItem
    {
        Bitmap _image;
        bool _imageUpdated;
        SizeF _imageNormalizedSize;

        public override Bitmap BitmapImage
        {
            get => _image;
        }

        int _imageTexID = -1;
        int _vertexBuffer = -1;

        static readonly Map4Vertex[] _vertices = new Map4Vertex[]
        {
            new Map4Vertex(new Vector3(-1, -1, 0), new Vector2(0, 1)),
            new Map4Vertex(new Vector3(1, -1, 0),  new Vector2(1, 1)),
            new Map4Vertex(new Vector3(-1, 1, 0),  new Vector2(0, 0)),
            new Map4Vertex(new Vector3(1, 1, 0),   new Vector2(1, 0)),
            new Map4Vertex(new Vector3(-1, 1, 0),  new Vector2(0, 0)),
            new Map4Vertex(new Vector3(1, -1, 0),  new Vector2(1, 1)),
        };

        public Vector3 Position { get; set; }

        public float Rotation { get; set; } = 0.0f;

        public float Size { get; set; } = 0.1f;

        public int DisplayLayer { get; set; }

        private float _loadedOpacity = 1.0f;
        public float Opacity { get; set; } = 1.0f;

        public override IEnumerable<Type> DrawOnCameraTypes => CameraTypeAny;

        public override float? Depth => Position.Y + 0x10000 * DisplayLayer;

        public override DrawType Type => DrawType.Overlay;

        public Map4GraphicsIconItem(Bitmap image) : base(false)
        {
            ChangeImage(image);
        }

        public void ChangeImage(Bitmap image)
        {
            _image?.Dispose();
            _image = image == null ? null : (Bitmap)image.Clone();
            _imageUpdated = false;
        }

        public override void Load(Map4Graphics graphics)
        {
            CheckUpdateImage(graphics);

            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_vertices.Length * Map4Vertex.Size),
                _vertices, BufferUsageHint.StaticDraw);
        }

        private void CheckUpdateOpacity()
        {
            if (_loadedOpacity == Opacity)
                return;

            _loadedOpacity = Opacity;

            // Get updated vertices
            Map4Vertex[] updatedVertices = _vertices.ToArray();
            for (int i = 0; i < updatedVertices.Length; i++)
                updatedVertices[i].Color.A = _loadedOpacity;

            // Update buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(updatedVertices.Length * Map4Vertex.Size),
                updatedVertices, BufferUsageHint.StaticDraw);
        }

        public override Matrix4 GetModelMatrix(Map4Graphics graphics)
        {
            return Matrix4.CreateScale(
                Size * _imageNormalizedSize.Width,
                Size * _imageNormalizedSize.Height,
                1)
                * Matrix4.CreateRotationZ(Rotation)
                * Matrix4.CreateScale(1.0f / graphics.NormalizedWidth, 1.0f, 1.0f / graphics.NormalizedHeight)
                * Matrix4.CreateTranslation(graphics.Utilities.GetPositionOnViewFromCoordinate(Position));
        }

        public override void Draw(Map4Graphics graphics)
        {
            CheckUpdateImage(graphics);
            CheckUpdateOpacity();

            if (_imageTexID == -1)
                return;

            GL.BindTexture(TextureTarget.Texture2D, _imageTexID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);
        }

        public override void Unload(Map4Graphics graphics)
        {
            ChangeImage(null);
            CheckUpdateImage(graphics);
            GL.DeleteBuffer(_vertexBuffer);
        }

        private void CheckUpdateImage(Map4Graphics graphics)
        {
            // Image already updated
            if (_imageUpdated)
                return;

            // Remove previous image
            GL.DeleteTexture(_imageTexID);
            _imageTexID = -1;
            _imageUpdated = true;
            _imageNormalizedSize = new SizeF(1.0f, 1.0f);

            // Don't add image if its null (we want it cleared)
            if (_image == null)
                return;

            // Update image
            _imageTexID = graphics.Utilities.LoadTexture(_image);
            _imageNormalizedSize = new SizeF(
                _image.Width >= _image.Height ? 1.0f : (float) _image.Width / _image.Height,
                _image.Width <= _image.Height ? 1.0f : (float) _image.Height / _image.Width);

            // Dispose of temp copy (its in graphics memory now)
            _image.Dispose();
            _image = null;
        }
    }
}
