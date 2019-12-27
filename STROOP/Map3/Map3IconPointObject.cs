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
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3
{
    public abstract class Map3IconPointObject : Map3IconObject
    {
        public Map3IconPointObject()
            : base()
        {
        }

        public override void DrawOn2DControl()
        {
            // Update map object
            (double x, double y, double z, double angle) = GetPositionAngle().GetValues();
            (float xPosPixels, float zPosPixels) = Map3Utilities.ConvertCoordsForControl((float)x, (float)z);
            float angleDegrees = Rotates ? Map3Utilities.ConvertAngleForControl(angle) : 0;
            SizeF size = Map3Utilities.ScaleImageSizeForControl(Image.Size, Size);
            Map3Utilities.DrawTexture(TextureId, new PointF(xPosPixels, zPosPixels), size, angleDegrees, Opacity);
        }

        public override bool ParticipatesInGlobalIconSize()
        {
            return true;
        }







        public override Matrix4 GetModelMatrix()
        {

            SizeF _imageNormalizedSize = new SizeF(
                Image.Width >= Image.Height ? 1.0f : (float) Image.Width / Image.Height,
                Image.Width <= Image.Height ? 1.0f : (float) Image.Height / Image.Width);


            PositionAngle posAngle = GetPositionAngle();
            float angle = (float)MoreMath.AngleUnitsToRadians(posAngle.Angle);
            Vector3 pos = new Vector3((float)posAngle.X, (float)posAngle.Y, (float)posAngle.Z);

            float size = Size / 100;
            return Matrix4.CreateScale(
                size * _imageNormalizedSize.Width,
                size * _imageNormalizedSize.Height,
                1)
                * Matrix4.CreateRotationZ(angle)
                * Matrix4.CreateScale(1.0f / Config.Map4Graphics.NormalizedWidth, 1.0f, 1.0f / Config.Map4Graphics.NormalizedHeight)
                * Matrix4.CreateTranslation(Config.Map4Graphics.Utilities.GetPositionOnViewFromCoordinate(pos));
        }




        static readonly Map4Vertex[] _vertices = new Map4Vertex[]
        {
            new Map4Vertex(new Vector3(-1, -1, 0), new Vector2(0, 1)),
            new Map4Vertex(new Vector3(1, -1, 0),  new Vector2(1, 1)),
            new Map4Vertex(new Vector3(-1, 1, 0),  new Vector2(0, 0)),
            new Map4Vertex(new Vector3(1, 1, 0),   new Vector2(1, 0)),
            new Map4Vertex(new Vector3(-1, 1, 0),  new Vector2(0, 0)),
            new Map4Vertex(new Vector3(1, -1, 0),  new Vector2(1, 1)),
        };





        public override void DrawOn3DControl()
        {

            int _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_vertices.Length * Map4Vertex.Size),
                _vertices, BufferUsageHint.StaticDraw);



            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            Config.Map4Graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);







            GL.DeleteBuffer(_vertexBuffer);


        }
    }
}
