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
    class Map4GraphicsImageItem : Map4GraphicsItem
    {
        Bitmap _image;
        bool _imageUpdated;

        int _imageTexID = -1;
        int _vertexBuffer = -1;

        static readonly Map4Vertex[] _vertices = new Map4Vertex[]
        {
            new Map4Vertex(new Vector3(0, 0, 0), new Vector2(0, 0)),
            new Map4Vertex(new Vector3(0, 0, 1), new Vector2(0, 1)),
            new Map4Vertex(new Vector3(1, 0, 0), new Vector2(1, 0)),
            new Map4Vertex(new Vector3(1, 0, 1), new Vector2(1, 1)),
            new Map4Vertex(new Vector3(1, 0, 0), new Vector2(1, 0)),
            new Map4Vertex(new Vector3(0, 0, 1), new Vector2(0, 1)),
        };

        public RectangleF Region { get; set; }

        public float Y { get; set; } = 0.0f;

        public bool DrawOnTopDown { get; set; }

        public bool DrawOnPerspective { get; set; }

        public override IEnumerable<Type> DrawOnCameraTypes => CameraTypeAny;

        public override float? Depth => float.MaxValue;

        public override Map3DrawType Type => Map3DrawType.Perspective;

        public Map4GraphicsImageItem(Bitmap image) : base(true)
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

        public override Matrix4 GetModelMatrix(Map4Graphics graphics)
        {
            float _yDepth = graphics.Camera is Map4CameraTopView ? -0x2000 : Y + 20f;
            return Matrix4.CreateScale(Region.Width, 1.0f, Region.Height) 
                * Matrix4.CreateTranslation(Region.Location.X, _yDepth, Region.Location.Y);
        }

        public override void Draw(Map4Graphics graphics)
        {
            CheckUpdateImage(graphics);

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

            // Don't add image if its null (we want it cleared)
            if (_image == null)
                return;

            // Update image
            _imageTexID = graphics.Utilities.LoadTexture(_image);

            // Dispose of temp copy (its in graphics memory now)
            _image.Dispose();
            _image = null;
        }
    }
}
