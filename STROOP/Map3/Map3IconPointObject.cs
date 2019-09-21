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
    public abstract class Map3IconPointObject : Map3IconObject
    {
        public Map3IconPointObject(Map3Graphics graphics, Func<Image> imageFunction)
            : base(graphics, imageFunction)
        {
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

            float xOffsetInGameUnits = relX - Graphics.XMin;
            float xOffsetPixels = xOffsetInGameUnits * Graphics.ConversionScale;
            float xPixels = Graphics.MapView.X + xOffsetPixels;

            float zOffsetInGameUnits = relZ - Graphics.ZMin;
            float zOffsetPixels = zOffsetInGameUnits * Graphics.ConversionScale;
            float zPixels = Graphics.MapView.Y + zOffsetPixels;

            SizeF size = Graphics.ScaleImageSize(Image.Size, iconSize);

            // Place and rotate texture to correct location on control
            GL.LoadIdentity();
            GL.Translate(new Vector3(xPixels, zPixels, 0));
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

        protected abstract (double x, double y, double z, double angle) GetPositionAngle();
    }
}
