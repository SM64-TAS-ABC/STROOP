using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public class MapPreviousPositionsObject : MapObject
    {
        private int _redMarioTex = -1;
        private int _greenMarioTex = -1;
        private int _orangeMarioTex = -1;

        public MapPreviousPositionsObject()
            : base()
        {
            InternalRotates = true;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.NextPositionsImage;
        }

        public override string GetName()
        {
            return "Previous Positions";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Mario.Y;
        }

        public override void DrawOn2DControl()
        {
            List<(float x, float y, float z, float angle, int tex)> data = GetData();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z, float angle, int tex) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControl(x, z);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                MapUtilities.DrawTexture(tex, point, size, angleDegrees, Opacity);
            }

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, OpacityByte);
            GL.LineWidth(OutlineWidth);
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < data.Count - 1; i++)
            {
                (float x1, float y1, float z1, float angle1, int tex1) = data[i];
                (float x2, float y2, float z2, float angle2, int tex2) = data[i + 1];
                (float x, float z) vertex1ForControl = MapUtilities.ConvertCoordsForControl(x1, z1);
                (float x, float z) vertex2ForControl = MapUtilities.ConvertCoordsForControl(x2, z2);
                GL.Vertex2(vertex1ForControl.x, vertex1ForControl.z);
                GL.Vertex2(vertex2ForControl.x, vertex2ForControl.z);
            }
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn3DControl()
        {
            List<(float x, float y, float z, float angle, int tex)> data = GetData();
            data.Reverse();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z, float angle, int tex) = dataPoint;

                Matrix4 viewMatrix = GetModelMatrix(x, y, z, angle);
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                Map3DVertex[] vertices = GetVertices();
                int vertexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map3DVertex.Size),
                    vertices, BufferUsageHint.StaticDraw);
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
                GL.DeleteBuffer(vertexBuffer);
            }
        }
        
        public Matrix4 GetModelMatrix(float x, float y, float z, float ang)
        {
            Image image = Config.ObjectAssociations.MarioImage;
            SizeF _imageNormalizedSize = new SizeF(
                image.Width >= image.Height ? 1.0f : (float)image.Width / image.Height,
                image.Width <= image.Height ? 1.0f : (float)image.Height / image.Width);

            float angle = Rotates ? (float)MoreMath.AngleUnitsToRadians(ang - SpecialConfig.Map3DCameraYaw + 32768) : 0;
            Vector3 pos = new Vector3(x, y, z);

            float size = Size / 200;
            return Matrix4.CreateScale(size * _imageNormalizedSize.Width, size * _imageNormalizedSize.Height, 1)
                * Matrix4.CreateRotationZ(angle)
                * Matrix4.CreateScale(1.0f / Config.Map3DGraphics.NormalizedWidth, 1.0f / Config.Map3DGraphics.NormalizedHeight, 1)
                * Matrix4.CreateTranslation(MapUtilities.GetPositionOnViewFromCoordinate(pos));
        }
        
        private Map3DVertex[] GetVertices()
        {
            return new Map3DVertex[]
            {
                new Map3DVertex(new Vector3(-1, -1, 0), Color4, new Vector2(0, 1)),
                new Map3DVertex(new Vector3(1, -1, 0), Color4, new Vector2(1, 1)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color4, new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, 1, 0), Color4, new Vector2(1, 0)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color4,  new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, -1, 0), Color4, new Vector2(1, 1)),
            };
        }

        public List<(float x, float y, float z, float angle, int tex)> GetData()
        {
            float pos1X = Config.Stream.GetSingle(0x80372F00);
            float pos1Y = Config.Stream.GetSingle(0x80372F04);
            float pos1Z = Config.Stream.GetSingle(0x80372F08);
            float pos2X = Config.Stream.GetSingle(0x80372F10);
            float pos2Y = Config.Stream.GetSingle(0x80372F14);
            float pos2Z = Config.Stream.GetSingle(0x80372F18);
            float pos3X = Config.Stream.GetSingle(0x80372F20);
            float pos3Y = Config.Stream.GetSingle(0x80372F24);
            float pos3Z = Config.Stream.GetSingle(0x80372F28);
            float pos4X = Config.Stream.GetSingle(0x80372F30);
            float pos4Y = Config.Stream.GetSingle(0x80372F34);
            float pos4Z = Config.Stream.GetSingle(0x80372F38);
            float pos5X = Config.Stream.GetSingle(0x80372F40);
            float pos5Y = Config.Stream.GetSingle(0x80372F44);
            float pos5Z = Config.Stream.GetSingle(0x80372F48);
            float pos6X = Config.Stream.GetSingle(0x80372F50);
            float pos6Y = Config.Stream.GetSingle(0x80372F54);
            float pos6Z = Config.Stream.GetSingle(0x80372F58);
            float pos7X = Config.Stream.GetSingle(0x80372F60);
            float pos7Y = Config.Stream.GetSingle(0x80372F64);
            float pos7Z = Config.Stream.GetSingle(0x80372F68);
            float pos8X = Config.Stream.GetSingle(0x80372F70);
            float pos8Y = Config.Stream.GetSingle(0x80372F74);
            float pos8Z = Config.Stream.GetSingle(0x80372F78);
            float pos9X = Config.Stream.GetSingle(0x80372F80);
            float pos9Y = Config.Stream.GetSingle(0x80372F84);
            float pos9Z = Config.Stream.GetSingle(0x80372F88);

            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);

            return new List<(float x, float y, float z, float angle, int tex)>()
            {
                (pos1X, pos1Y, pos1Z, marioAngle, _greenMarioTex),
                (pos2X, pos2Y, pos2Z, marioAngle, _orangeMarioTex),
                (pos3X, pos3Y, pos3Z, marioAngle, _greenMarioTex),
                (pos4X, pos4Y, pos4Z, marioAngle, _orangeMarioTex),
                (pos5X, pos5Y, pos5Z, marioAngle, _greenMarioTex),
                (pos6X, pos6Y, pos6Z, marioAngle, _orangeMarioTex),
                (pos7X, pos7Y, pos7Z, marioAngle, _greenMarioTex),
                (pos8X, pos8Y, pos8Z, marioAngle, _orangeMarioTex),
                (pos9X, pos9Y, pos9Z, marioAngle, _redMarioTex),
            };
        }

        public override void Update()
        {
            if (_redMarioTex == -1)
            {
                _redMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.MarioMapImage as Bitmap);
            }
            if (_greenMarioTex == -1)
            {
                _greenMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.GreenMarioMapImage as Bitmap);
            }
            if (_orangeMarioTex == -1)
            {
                _orangeMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.OrangeMarioMapImage as Bitmap);
            }
        }

        public override bool ParticipatesInGlobalIconSize()
        {
            return true;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }
    }
}
