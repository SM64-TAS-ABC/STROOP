using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SM64_Diagnostic.Controls.Map;
using SM64_Diagnostic.ManagerClasses;
using OpenTK.Graphics.OpenGL;

namespace SM64_Diagnostic.Structs
{
    public class MapObject : MapBaseObject
    {
        public Image Image;
        public PointF LocationOnContol;
        public float X;
        public float Y;
        public float Z;
        public bool IsActive;
        public float Rotation;
        public bool UsesRotation;
        public bool Show = true;

        public int TextureId;

        public MapObject(Image image, int depth = 0, PointF location = new PointF())
        {
            Image = image;
            X = location.X;
            Y = location.Y;
            Depth = depth;
        }

        public override void DrawOnControl(MapGraphics graphics)
        {
            graphics.DrawTexture(TextureId, LocationOnContol, graphics.ScaleImageSize(Image.Size, graphics.IconSize), UsesRotation ? Rotation : 0);
        }

        public override void Load(MapGraphics graphics)
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
