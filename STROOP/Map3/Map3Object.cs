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
            // Get Mario position and rotation
            float x = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float y = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float z = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort marioFacing = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            float rot = (float)MoreMath.AngleUnitsToDegrees(marioFacing);

            // Update Mario map object
            X = x;
            Y = y;
            Z = z;
            Rotation = rot;

            // Calculate mario's location on the OpenGl control
            var marioCoord = new PointF(RelX, RelZ);
            var mapView = graphics.MapView;
            LocationOnContol = Config.Map3Manager.CalculateLocationOnControl(marioCoord, mapView);

            SizeF size = graphics.ScaleImageSize(Image.Size, 50);
            float alpha = 1;
            float angle = Rotation;

            // Place and rotate texture to correct location on control
            GL.LoadIdentity();
            GL.Translate(new Vector3(LocationOnContol.X, LocationOnContol.Y, 0));
            GL.Rotate(360 - angle, Vector3.UnitZ);
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
