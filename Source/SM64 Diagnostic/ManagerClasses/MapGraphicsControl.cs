using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic.ManagerClasses
{
    class MapGraphicsControl
    {
        public RectangleF MapView;
        int _mapText = -1;
        Size _mapImageSize;
        List<MapObject> _mapObjects = new List<MapObject>();

        public GLControl Control;

        public MapGraphicsControl(GLControl control)
        {
            Control = control;
        }

        public void Load()
        {
            //Control.VSync = true;
            Control.Paint += OnPaint;
            Control.Resize += OnResize;

            GL.ClearColor(Color.FromKnownColor(KnownColor.Control));
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            SetupViewport();
        }

        public void OnPaint(object sender, EventArgs e)
        {
            if (Control.Disposing)
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (_mapText == -1)
            {
                Control.SwapBuffers();
                return;
            }

            GL.MatrixMode(MatrixMode.Modelview);

            GL.LoadIdentity();
            DrawTexture(_mapText, new PointF(MapView.X + MapView.Width / 2, MapView.Y + MapView.Height / 2), MapView.Size);

            foreach (var mapObj in _mapObjects)
            {
                DrawTexture(mapObj.TextureId, mapObj.LocationOnContol, new SizeF(30.0f, 30.0f), mapObj.Rotation);
            }

            Control.SwapBuffers();
        }

        public void OnResize(object sender, EventArgs e)
        {
            SetupViewport();
            SetMapView();
        }

        private void SetupViewport()
        {
            int w = Control.Width;
            int h = Control.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void SetMapView()
        {
            float hScale = ((float) Control.Width) / _mapImageSize.Width;
            float vScale = ((float) Control.Height) / _mapImageSize.Height;
            float scale = Math.Min(hScale, vScale);

            float marginV = 0;
            float marginH = 0;
            if (hScale > vScale)
                marginH = (Control.Width - scale * _mapImageSize.Width);
            else
                marginV = (Control.Height - scale * _mapImageSize.Height);

            MapView = new RectangleF(marginH / 2, marginV / 2, Control.Width - marginH, Control.Height - marginV);
        }

        public void SetMap(Image map)
        {
            int oldText = _mapText;

            _mapText = LoadTexture(map as Bitmap);
            _mapImageSize = map.Size;
            SetMapView();

            if (oldText != -1)
                GL.DeleteTexture(oldText);
        }

        static void DrawTexture(int texId, PointF loc, SizeF size, float angle = 0)
        {
            GL.LoadIdentity();
            GL.Translate(new Vector3(loc.X, loc.Y, 0));
            GL.Rotate(360-angle, Vector3.UnitZ);
            GL.BindTexture(TextureTarget.Texture2D, texId);
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-size.Width / 2, size.Height / 2);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(size.Width / 2, size.Height / 2);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(size.Width / 2, -size.Height / 2);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-size.Width / 2, -size.Height / 2);

            GL.End();
        }

        static int LoadTexture(Bitmap bmp)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
   
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                //OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            GL.TexStorage2D(TextureTarget2d.Texture2D, 8, SizedInternalFormat.Rgba8, bmp.Width, bmp.Height);
            GL.TexSubImage2D(
                TextureTarget.Texture2D, 0, 0, 0, bmp.Width, bmp.Height​, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            bmp.UnlockBits(bmp_data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return id;
        }

        public void AddMapObject(MapObject mapObj)
        {
            mapObj.TextureId = LoadTexture(mapObj.Image as Bitmap);
            _mapObjects.Add(mapObj);
        }

        public void RemoveMapObject(MapObject mapObj)
        {
            _mapObjects.Remove(mapObj);
            GL.DeleteTexture(mapObj.TextureId);
        }
    }
}
