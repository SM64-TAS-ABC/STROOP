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
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Xml.Linq;
using STROOP.Map.Map3D;
using STROOP.Models;

namespace STROOP.Map
{
    public class MapObjectCrouchSlidePositions : MapObject
    {
        private float storedMarioX = 0;
        private float storedMarioY = 0;
        private float storedMarioZ = 0;
        private float storedXSpeed = 0;
        private float storedYSpeed = 0;
        private float storedZSpeed = 0;
        private float storedHSpeed = 0;
        private float storedSlidingSpeedX = 0;
        private float storedSlidingSpeedZ = 0;
        private ushort storedSlidingAngle = 0;
        private ushort storedMarioAngle = 0;
        private ushort storedCameraAngle = 0;

        private List<CrouchSlidePoint> storedPoints = new List<CrouchSlidePoint>();

        private int _tex = -1;

        public MapObjectCrouchSlidePositions()
            : base()
        {
            LineWidth = 0;
        }

        private class CrouchSlidePoint
        {
            public readonly float X;
            public readonly float Y;
            public readonly float Z;
            public readonly ushort Angle;

            public CrouchSlidePoint(
                float x,
                float y,
                float z,
                ushort angle)
            {
                X = x;
                Y = y;
                Z = z;
                Angle = angle;
            }
        }

        private List<CrouchSlidePoint> GetPoints()
        {
            float testMarioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float testMarioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float testMarioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float testXSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
            float testYSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
            float testZSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
            float testHSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            float testSlidingSpeedX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
            float testSlidingSpeedZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
            ushort testSlidingAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.SlidingYawOffset);
            ushort testMarioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            ushort testCameraAngle = Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset);

            if (testMarioX == storedMarioX &&
                testMarioY == storedMarioY &&
                testMarioZ == storedMarioZ &&
                testXSpeed == storedXSpeed &&
                testYSpeed == storedYSpeed &&
                testZSpeed == storedZSpeed &&
                testHSpeed == storedHSpeed &&
                testSlidingSpeedX == storedSlidingSpeedX &&
                testSlidingSpeedZ == storedSlidingSpeedZ &&
                testSlidingAngle == storedSlidingAngle &&
                testMarioAngle == storedMarioAngle &&
                testCameraAngle == storedCameraAngle)
            {
                return storedPoints;
            }

            storedMarioX = testMarioX;
            storedMarioY = testMarioY;
            storedMarioZ = testMarioZ;
            storedXSpeed = testXSpeed;
            storedYSpeed = testYSpeed;
            storedZSpeed = testZSpeed;
            storedHSpeed = testHSpeed;
            storedSlidingSpeedX = testSlidingSpeedX;
            storedSlidingSpeedZ = testSlidingSpeedZ;
            storedSlidingAngle = testSlidingAngle;
            storedMarioAngle = testMarioAngle;
            storedCameraAngle = testCameraAngle;

            uint action = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.ActionOffset);
            TriangleDataModel floor = TriangleDataModel.CreateLazy(Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset));
            float floorHeight = Config.Stream.GetFloat(MarioConfig.StructAddress + 0x70);
            TriangleDataModel wall = TriangleDataModel.CreateLazy(Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset));
            short terrainType = Config.Stream.GetShort(Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.AreaPointerOffset) + 0x2);

            storedPoints.Clear();
            for (int x = -128; x <= 127; x++)
            {
                for (int y = -128; y <= 127; y++)
                {
                    SlidingMarioState marioState =
                        new SlidingMarioState(
                            x: testMarioX,
                            y: testMarioY,
                            z: testMarioZ,
                            xSpeed: testXSpeed,
                            ySpeed: testYSpeed,
                            zSpeed: testZSpeed,
                            hSpeed: testHSpeed,
                            slidingSpeedX: testSlidingSpeedX,
                            slidingSpeedZ: testSlidingSpeedZ,
                            slidingAngle: testSlidingAngle,
                            marioAngle: testMarioAngle,
                            cameraAngle: testCameraAngle,
                            action: action,
                            floor: floor,
                            floorHeight: floorHeight,
                            wall: wall,
                            terrainType: terrainType,
                            new Input(x, y));
                    CrouchSlideCalculator.act_crouch_slide(marioState);
                    CrouchSlidePoint point = new CrouchSlidePoint(marioState.X, marioState.Y, marioState.Z, marioState.MarioAngle);
                    storedPoints.Add(point);
                }
            }
            return storedPoints;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<CrouchSlidePoint> points = GetPoints();

            for (int i = 0; i <points.Count; i++)
            {
                var p = points[i];
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView(p.X, p.Z, UseRelativeCoordinates);
                Image image = _customImage ?? Config.ObjectAssociations.GreenMarioMapImage;
                SizeF size = MapUtilities.ScaleImageSizeForControl(image.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(p.Angle) : 0;
                double opacity = Opacity;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(_customImageTex ?? _tex, point, size, angleDegrees, opacity);
            }

            if (LineWidth != 0)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var p1 = points[i];
                    var p2 = points[i + 1];
                    //MapUtilities.DrawLinesOn2DControlTopDownView(new List<(float x, float y, float z)>() { p1, p2 }, LineWidth, LineColor, 255, UseRelativeCoordinates);
                }
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<CrouchSlidePoint> points = GetPoints();

            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlOrthographicView(p.X, p.Y, p.Z, UseRelativeCoordinates);
                Image image = _customImage ?? Config.ObjectAssociations.GreenMarioMapImage;
                SizeF size = MapUtilities.ScaleImageSizeForControl(image.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(p.Angle) : 0;
                double opacity = Opacity;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(_customImageTex ?? _tex, point, size, angleDegrees, opacity);
            }

            if (LineWidth != 0)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var p1 = points[i];
                    var p2 = points[i + 1];
                    //MapUtilities.DrawLinesOn2DControlOrthographicView(new List<(float x, float y, float z)>() { p1, p2 }, LineWidth, LineColor, 255, UseRelativeCoordinates);
                }
            }
        }

        public override void DrawOn3DControl()
        {
            List<CrouchSlidePoint> points = GetPoints();

            foreach (var p in points)
            {
                Matrix4 viewMatrix = GetModelMatrix(p.X, p.Y, p.Z, p.Angle);
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                Map3DVertex[] vertices = GetVertices();
                int vertexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map3DVertex.Size),
                    vertices, BufferUsageHint.StaticDraw);
                GL.BindTexture(TextureTarget.Texture2D, _customImageTex ?? _tex);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
                GL.DeleteBuffer(vertexBuffer);
            }

            if (LineWidth != 0)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var p1 = points[i];
                    var p2 = points[i + 1];
                    //MapUtilities.DrawLinesOn3DControl(new List<(float x, float y, float z)>() { p1, p2 }, LineWidth, LineColor, 255, GetModelMatrix());
                }
            }
        }

        public Matrix4 GetModelMatrix(float x, float y, float z, float ang)
        {
            Image image = _customImage ?? Config.ObjectAssociations.GreenMarioMapImage;
            SizeF _imageNormalizedSize = new SizeF(
                image.Width >= image.Height ? 1.0f : (float)image.Width / image.Height,
                image.Width <= image.Height ? 1.0f : (float)image.Height / image.Width);

            float angle = Rotates ? (float)MoreMath.AngleUnitsToRadians(ang - MapConfig.Map3DCameraYaw + 32768) : 0;
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

        public override void Update()
        {
            if (_tex == -1)
            {
                _tex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.GreenMarioMapImage as Bitmap);
            }
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.GreenMarioMapImage;
        }

        public override string GetName()
        {
            return "Crouch Slide Positions";
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            List<CrouchSlidePoint> points = GetPoints();

            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            for (int i = points.Count - 1; i >= 0; i--)
            {
                var point = points[i];
                double dist = MoreMath.GetDistanceBetween(point.X, point.Z, inGameX, inGameZ);
                double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, point.X, point.Y, point.Z, index: i);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            List<CrouchSlidePoint> points = GetPoints();

            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            for (int i = points.Count - 1; i >= 0; i--)
            {
                var point = points[i];
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(point.X, point.Y, point.Z, UseRelativeCoordinates);
                double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                double radius = Scales ? Size * Config.CurrentMapGraphics.MapViewScaleValue : Size;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, point.X, point.Y, point.Z, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<CrouchSlidePoint> points = GetPoints();

            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var point = points[hoverData.Index.Value];
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(point.X, point.Y, point.Z, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
