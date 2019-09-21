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

namespace STROOP.Map3
{
    public abstract class Map3IconObject : Map3Object
    {
        private readonly Func<Image> ImageFunction;
        protected Image Image;
        protected int TextureId;

        public Map3IconObject(Map3Graphics graphics, Func<Image> imageFunction)
            : base(graphics)
        {
            ImageFunction = imageFunction;
            Image = null;
            TextureId = -1;
        }

        protected void DrawTexture(int texId, PointF loc, SizeF size, float angle, float alpha)
        {
            // Place and rotate texture to correct location on control
            GL.LoadIdentity();
            GL.Translate(new Vector3(loc.X, loc.Y, 0));
            GL.Rotate(360 - angle, Vector3.UnitZ);
            GL.Color4(1.0, 1.0, 1.0, alpha);

            // Start drawing texture
            GL.BindTexture(TextureTarget.Texture2D, texId);
            GL.Begin(PrimitiveType.Quads);

            // Set drawing coordinates
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-size.Width / 2, size.Height / 2);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(size.Width / 2, size.Height / 2);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(size.Width / 2, -size.Height / 2);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-size.Width / 2, -size.Height / 2);

            GL.End();
        }

        protected void UpdateImage()
        {
            Image image = ImageFunction();
            if (image != Image)
            {
                Image = image;
                GL.DeleteTexture(TextureId);
                TextureId = Graphics.LoadTexture(image as Bitmap);
            }
        }

        public override void Dispose()
        {
            GL.DeleteTexture(TextureId);
        }
    }
}
