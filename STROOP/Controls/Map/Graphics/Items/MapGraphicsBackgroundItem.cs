using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK;

namespace STROOP.Controls.Map.Graphics.Items
{
    class MapGraphicsBackgroundItem : MapGraphicsItem
    {
        Bitmap _image;
        bool _imageUpdated;
        
        int _imageTexID = -1;
        int _vertexBuffer = -1;

        static readonly Vertex[] _vertices = new Vertex[]
        {
            new Vertex(new Vector3(-1, -1, 0), new Vector2(0, 1)),
            new Vertex(new Vector3(1, -1, 0),  new Vector2(1, 1)),
            new Vertex(new Vector3(-1, 1, 0),  new Vector2(0, 0)),
            new Vertex(new Vector3(1, 1, 0),   new Vector2(1, 0)),
            new Vertex(new Vector3(-1, 1, 0),  new Vector2(0, 0)),
            new Vertex(new Vector3(1, -1, 0),  new Vector2(1, 1)),
        };

        public override IEnumerable<Type> DrawOnCameraTypes => CameraTypeAny;

        public override float? Depth => null;

        public override DrawType Type => DrawType.Background;

        public MapGraphicsBackgroundItem(Bitmap image)
        {
            ChangeImage(image);
        }

        public void ChangeImage(Bitmap image)
        {
            _image?.Dispose();
            _image = image == null ? null : (Bitmap) image.Clone();
            _imageUpdated = false;
        }

        public override void Load(MapGraphics graphics)
        {
            CheckUpdateImage(graphics);

            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (_vertices.Length * Vertex.Size), 
                _vertices, BufferUsageHint.StaticDraw);
        }

        public override void Draw(MapGraphics graphics)
        {

            CheckUpdateImage(graphics);

            if (_imageTexID == -1)
                return;

            GL.BindTexture(TextureTarget.Texture2D, _imageTexID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);
        }

        public override void Unload(MapGraphics graphics)
        {
            ChangeImage(null);
            CheckUpdateImage(graphics);
            GL.DeleteBuffer(_vertexBuffer);
        }

        private void CheckUpdateImage(MapGraphics graphics)
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

            _imageTexID = graphics.Utilities.LoadTexture(_image);

            // Dispose of temp copy (its in graphics memory now)
            _image?.Dispose();
            _image = null;
        }
    }
}
