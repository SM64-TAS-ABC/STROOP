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
    public class Map3Object : IDisposable
    {
        public Image Image;

        public float X;
        public float Y;
        public float Z;
        public float Rotation;
        public int TextureId;

        public Map3Object(Image image, PointF location = new PointF())
        {
            Image = image;
        }

        public void DrawOnControl(Map3Graphics graphics)
        {
            float x = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float y = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float z = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort angle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);

            // Update Mario map object
            X = (float)PuUtilities.GetRelativeCoordinate(x);
            Y = (float)PuUtilities.GetRelativeCoordinate(y);
            Z = (float)PuUtilities.GetRelativeCoordinate(z);
            Rotation = (float)MoreMath.AngleUnitsToDegrees(angle);

            // Calculate mario's location on the OpenGl control
            var marioCoord = new PointF(X, Z);
            var mapView = graphics.MapView;
            PointF locationOnContol = Config.Map3Manager.CalculateLocationOnControl(marioCoord, mapView);

            SizeF size = graphics.ScaleImageSize(Image.Size, 50);
            float alpha = 1;
            float rotation = Rotation;

            // Place and rotate texture to correct location on control
            GL.LoadIdentity();
            GL.Translate(new Vector3(locationOnContol.X, locationOnContol.Y, 0));
            GL.Rotate(360 - rotation, Vector3.UnitZ);
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
