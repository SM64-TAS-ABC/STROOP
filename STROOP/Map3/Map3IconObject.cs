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
        private Func<Image> ImageFunction;
        private Image Image;
        private int TextureId;

        public Map3IconObject(Map3Graphics graphics, Func<Image> imageFunction) : base(graphics)
        {
            ImageFunction = imageFunction;
            Image = null;
            TextureId = -1;
        }

        public override void DrawOnControl()
        {
            UpdateImage();

            // Update map object
            (double x, double y, double z, double angle) = GetPositionAngle();
            float relX = (float)PuUtilities.GetRelativeCoordinate(x);
            float relY = (float)PuUtilities.GetRelativeCoordinate(y);
            float relZ = (float)PuUtilities.GetRelativeCoordinate(z);
            float angleDegrees = (float)MoreMath.AngleUnitsToDegrees(angle);

            // Additional stats
            int iconSize = 50;
            float alpha = 1;

            // Calculate location on the OpenGl control
            PointF locationOnContol = Config.Map3Manager.CalculateLocationOnControl(new PointF(relX, relZ), Graphics.MapView);
            SizeF size = Graphics.ScaleImageSize(Image.Size, iconSize);

            // Place and rotate texture to correct location on control
            GL.LoadIdentity();
            GL.Translate(new Vector3(locationOnContol.X, locationOnContol.Y, 0));
            GL.Rotate(360 - angleDegrees, Vector3.UnitZ);
            GL.Color4(1.0, 1.0, 1.0, alpha);

            // Start drawing texture
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.Begin(PrimitiveType.Quads);

            // Set drawing coordinates
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-size.Width / 2, size.Height / 2);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(size.Width / 2, size.Height / 2);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(size.Width / 2, -size.Height / 2);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-size.Width / 2, -size.Height / 2);

            GL.End();
        }

        private void UpdateImage()
        {
            Image image = ImageFunction();
            if (image != Image)
            {
                Image = image;
                GL.DeleteTexture(TextureId);
                TextureId = Graphics.LoadTexture(image as Bitmap);
            }
        }

        protected abstract (double x, double y, double z, double angle) GetPositionAngle();

        public override void Dispose()
        {
            GL.DeleteTexture(TextureId);
        }
    }
}
