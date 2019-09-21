using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;

namespace STROOP.Map3
{
    public class Map3Object : IDisposable
    {
        public Image Image;
        public PointF LocationOnContol;
        public float X;
        public float Y;
        public float Z;

        public float RelX { get => (float)PuUtilities.GetRelativeCoordinate(X); }
        public float RelY { get => (float)PuUtilities.GetRelativeCoordinate(Y); }
        public float RelZ { get => (float)PuUtilities.GetRelativeCoordinate(Z); }

        public float Rotation;
        public bool UsesRotation;
        public bool Transparent = false;

        public int TextureId;

        public Map3Object(Image image, PointF location = new PointF())
        {
            Image = image;
            X = location.X;
            Y = location.Y;
        }

        public void DrawOnControl(Map3Graphics graphics)
        {
            graphics.DrawTexture(TextureId, LocationOnContol, graphics.ScaleImageSize(Image.Size, 50),
                UsesRotation ? Rotation : 0, Transparent ? 0.5f : 1.0f);
        }

        public void Load(Map3Graphics graphics)
        {
            TextureId = graphics.LoadTexture(Image as Bitmap);
        }

        public void Dispose()
        {
            GL.DeleteTexture(TextureId);
        }
    }
}
