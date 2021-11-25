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
using STROOP.Map.Map3D;
using System.Windows.Forms;

namespace STROOP.Map
{
    public abstract class MapObjectIconPoint : MapObjectIcon
    {
        public MapObjectIconPoint()
            : base()
        {
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            (double x, double y, double z, double angle) = GetPositionAngle().GetValues();
            (float xPosPixels, float zPosPixels) = MapUtilities.ConvertCoordsForControlTopDownView((float)x, (float)z);
            float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
            SizeF size = MapUtilities.ScaleImageSizeForControl(Image.Size, Size, Scales);
            double opacity = Opacity;
            if (this == hoverData?.MapObject)
            {
                opacity = MapUtilities.GetHoverOpacity();
            }
            MapUtilities.DrawTexture(TextureId, new PointF(xPosPixels, zPosPixels), size, angleDegrees, opacity);
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            (double x, double y, double z, double angle) = GetPositionAngle().GetValues();
            (float xPosPixels, float yPosPixels) = MapUtilities.ConvertCoordsForControlOrthographicView((float)x, (float)y, (float)z);
            float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
            SizeF size = MapUtilities.ScaleImageSizeForControl(Image.Size, Size, Scales);
            double opacity = Opacity;
            if (this == hoverData?.MapObject)
            {
                opacity = MapUtilities.GetHoverOpacity();
            }
            MapUtilities.DrawTexture(TextureId, new PointF(xPosPixels, yPosPixels), size, angleDegrees, opacity);
        }

        public override (double x, double y, double z)? GetDragPosition()
        {
            PositionAngle posAngle = GetPositionAngle();
            return (posAngle.X, posAngle.Y, posAngle.Z);
        }

        public override void SetDragPosition(double? x = null, double? y = null, double? z = null)
        {
            GetPositionAngle().SetValues(x, y, z);
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
            float angle = Rotates ? (float)MoreMath.AngleUnitsToRadians(posAngle.Angle - MapConfig.Map3DCameraYaw + 32768) : 0;
            Vector3 pos = new Vector3((float)posAngle.X, (float)posAngle.Y, (float)posAngle.Z);

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

        public override void DrawOn3DControl()
        {
            Map3DVertex[] vertices = GetVertices();

            Matrix4 viewMatrix = GetModelMatrix();
            GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

            int vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map3DVertex.Size),
                vertices, BufferUsageHint.StaticDraw);
            GL.BindTexture(TextureTarget.Texture2D, TextureId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            Config.Map3DGraphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            GL.DeleteBuffer(vertexBuffer);
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);
            (double x, double y, double z, double angle) = GetPositionAngle().GetValues();
            double dist = MoreMath.GetDistanceBetween(x, z, inGameX, inGameZ);
            double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
            if (dist <= radius)
            {
                return new MapObjectHoverData(this, x, y, z);
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (double x, double y, double z, double angle) = GetPositionAngle().GetValues();
            (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView((float)x, (float)y, (float)z);
            double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
            double radius = Scales ? Size * Config.CurrentMapGraphics.MapViewScaleValue : Size;
            if (dist <= radius)
            {
                return new MapObjectHoverData(this, x, y, z);
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            (double x, double y, double z, double angle) = GetPositionAngle().GetValues();
            List<double> posValues = new List<double>() { x, y, z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
