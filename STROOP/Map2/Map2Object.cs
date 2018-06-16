using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;

namespace STROOP.Map2
{
    public class Map2Object : Map2BaseObject
    {
        public Image Image;
        public PointF LocationOnContol;
        public float X;
        public float Y;
        public float Z;
        public bool IsActive;
        public float Rotation;
        public bool UsesRotation;
        public bool Transparent = false;
        public bool Show = false;

        public int TextureId;

        public Map2Object(Image image, int depth = 0, PointF location = new PointF())
        {
            Image = image;
            X = location.X;
            Y = location.Y;
            Depth = depth;
        }

        public override void DrawOnControl(Map2Graphics graphics)
        {
            graphics.DrawTexture(TextureId, LocationOnContol, graphics.ScaleImageSize(Image.Size, graphics.IconSize),
                UsesRotation ? Rotation : 0, Transparent ? 0.5f : 1.0f);
        }

        public override void Load(Map2Graphics graphics)
        {
            TextureId = graphics.LoadTexture(Image as Bitmap);
        }

        public override void Dispose()
        {
            GL.DeleteTexture(TextureId);
        }

        public override double GetDepthScore()
        {
            return Y + Depth * 65536d;
        }
    }
}
