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
using System.Xml.Linq;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public class MapObjectLineSegment : MapObjectLine
    {
        private PositionAngle _posAngle1;
        private PositionAngle _posAngle2;
        private bool _useFixedSize;
        private float _backwardsSize;
        private bool _showMidline;
        private float _iconSize;

        private ToolStripMenuItem _itemUseFixedSize;
        private ToolStripMenuItem _itemSetBackwardsSize;
        private ToolStripMenuItem _itemShowMidline;
        private ToolStripMenuItem _itemSetIconSize;

        private static readonly string SET_BACKWARDS_SIZE_TEXT = "Set Backwards Size";
        private static readonly string SET_ICON_SIZE_TEXT = "Set Icon Size";

        public MapObjectLineSegment(PositionAngle posAngle1, PositionAngle posAngle2)
            : base()
        {
            _posAngle1 = posAngle1;
            _posAngle2 = posAngle2;
            _useFixedSize = false;
            _backwardsSize = 0;
            _showMidline = false;
            _iconSize = 10;

            Size = 0;
            LineWidth = 3;
            LineColor = Color.Red;
        }

        public static MapObject Create(string text1, string text2)
        {
            PositionAngle posAngle1 = PositionAngle.FromString(text1);
            PositionAngle posAngle2 = PositionAngle.FromString(text2);
            if (posAngle1 == null || posAngle2 == null) return null;
            return new MapObjectLineSegment(posAngle1, posAngle2);
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            (double x1, double y1, double z1, double angle1) = _posAngle1.GetValues();
            (double x2, double y2, double z2, double angle2) = _posAngle2.GetValues();
            double dist = PositionAngle.GetHDistance(_posAngle1, _posAngle2);
            (double startX, double startZ) = MoreMath.ExtrapolateLine2D(x2, z2, x1, z1, dist + _backwardsSize);
            (double endX, double endZ) = MoreMath.ExtrapolateLine2D(x1, z1, x2, z2, (_useFixedSize ? 0 : dist) + Size);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            vertices.Add(((float)startX, 0, (float)startZ));
            vertices.Add(((float)endX, 0, (float)endZ));

            if (_showMidline)
            {
                double midX = (startX + endX) / 2;
                double midZ = (startZ + endZ) / 2;
                double angle = PositionAngle.GetAngleTo(_posAngle1, _posAngle2, false, false);

                (double sideX1, double sideZ1) = MoreMath.AddVectorToPoint(Config.CurrentMapGraphics.MapViewRadius, angle + 16384, midX, midZ);
                (double sideX2, double sideZ2) = MoreMath.AddVectorToPoint(Config.CurrentMapGraphics.MapViewRadius, angle - 16384, midX, midZ);

                vertices.Add(((float)sideX1, 0, (float)sideZ1));
                vertices.Add(((float)sideX2, 0, (float)sideZ2));
            }

            return vertices;
        }

        protected override List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            (double x1, double y1, double z1, double angle1) = _posAngle1.GetValues();
            (double x2, double y2, double z2, double angle2) = _posAngle2.GetValues();
            double dist = PositionAngle.GetDistance(_posAngle1, _posAngle2);
            (double startX, double startY, double startZ) = MoreMath.ExtrapolateLine3D(x2, y2, z2, x1, y1, z1, dist + _backwardsSize);
            (double endX, double endY, double endZ) = MoreMath.ExtrapolateLine3D(x1, y1, z1, x2, y2, z2, (_useFixedSize ? 0 : dist) + Size);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            vertices.Add(((float)startX, (float)startY, (float)startZ));
            vertices.Add(((float)endX, (float)endY, (float)endZ));
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetVertices3D()
        {
            return GetVerticesOrthographicView();
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            base.DrawOn2DControlTopDownView(hoverData);

            if (_customImage != null)
            {
                (float x, float y, float z) = ((float, float, float))PositionAngle.GetMidPoint(_posAngle1, _posAngle2);
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(x, z, UseRelativeCoordinates);
                PointF point = new PointF(controlX, controlZ);
                SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, _iconSize, Scales);
                double opacity = Opacity;
                if (this == hoverData?.MapObject)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(_customImageTex.Value, point, size, 0, opacity);
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            base.DrawOn2DControlOrthographicView(hoverData);

            if (_customImage != null)
            {
                (float x, float y, float z) = ((float, float, float))PositionAngle.GetMidPoint(_posAngle1, _posAngle2);
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(x, y, z, UseRelativeCoordinates);
                PointF point = new PointF(controlX, controlZ);
                SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, _iconSize, Scales);
                double opacity = Opacity;
                if (this == hoverData?.MapObject)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(_customImageTex.Value, point, size, 0, opacity);
            }
        }

        public override void DrawOn3DControl()
        {
            base.DrawOn3DControl();

            if (_customImage != null)
            {
                (float x, float y, float z) = ((float, float, float))PositionAngle.GetMidPoint(_posAngle1, _posAngle2);
                Matrix4 viewMatrix = GetModelMatrix(x, y, z, 0);
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                Map3DVertex[] vertices2 = GetVertices();
                int vertexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices2.Length * Map3DVertex.Size),
                    vertices2, BufferUsageHint.StaticDraw);
                GL.BindTexture(TextureTarget.Texture2D, _customImageTex.Value);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices2.Length);
                GL.DeleteBuffer(vertexBuffer);
            }
        }

        public Matrix4 GetModelMatrix(float x, float y, float z, float ang)
        {
            SizeF _imageNormalizedSize = new SizeF(
                _customImage.Width >= _customImage.Height ? 1.0f : (float)_customImage.Width / _customImage.Height,
                _customImage.Width <= _customImage.Height ? 1.0f : (float)_customImage.Height / _customImage.Width);

            Vector3 pos = new Vector3(x, y, z);

            float size = _iconSize / 200;
            return Matrix4.CreateScale(size * _imageNormalizedSize.Width, size * _imageNormalizedSize.Height, 1)
                * Matrix4.CreateRotationZ(0)
                * Matrix4.CreateScale(1.0f / Config.Map3DGraphics.NormalizedWidth, 1.0f / Config.Map3DGraphics.NormalizedHeight, 1)
                * Matrix4.CreateTranslation(MapUtilities.GetPositionOnViewFromCoordinate(pos));
        }

        private Map3DVertex[] GetVertices()
        {
            return new Map3DVertex[]
            {
                new Map3DVertex(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1)),
                new Map3DVertex(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color.White,  new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1)),
            };
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _itemUseFixedSize = new ToolStripMenuItem("Use Fixed Size");
                _itemUseFixedSize.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeLineSegmentUseFixedSize: true, newLineSegmentUseFixedSize: !_useFixedSize);
                    GetParentMapTracker().ApplySettings(settings);
                };

                string suffix1 = string.Format(" ({0})", _backwardsSize);
                _itemSetBackwardsSize = new ToolStripMenuItem(SET_BACKWARDS_SIZE_TEXT + suffix1);
                _itemSetBackwardsSize.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter backwards size.");
                    float? backwardsSizeNullable = ParsingUtilities.ParseFloatNullable(text);
                    if (!backwardsSizeNullable.HasValue) return;
                    float backwardsSize = backwardsSizeNullable.Value;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeLineSegmentBackwardsSize: true, newLineSegmentBackwardsSize: backwardsSize);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _itemShowMidline = new ToolStripMenuItem("Show Midline");
                _itemShowMidline.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeLineSegmentShowMidline: true, newLineSegmentShowMidline: !_showMidline);
                    GetParentMapTracker().ApplySettings(settings);
                };

                string suffix2 = string.Format(" ({0})", _iconSize);
                _itemSetIconSize = new ToolStripMenuItem(SET_ICON_SIZE_TEXT + suffix2);
                _itemSetIconSize.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter icon size.");
                    float? iconSizeNullable = ParsingUtilities.ParseFloatNullable(text);
                    if (!iconSizeNullable.HasValue) return;
                    float iconSize = iconSizeNullable.Value;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeIconSize: true, newIconSize: iconSize);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemUseFixedSize);
                _contextMenuStrip.Items.Add(_itemSetBackwardsSize);
                _contextMenuStrip.Items.Add(_itemShowMidline);
                _contextMenuStrip.Items.Add(_itemSetIconSize);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeLineSegmentUseFixedSize)
            {
                _useFixedSize = settings.NewLineSegmentUseFixedSize;
                _itemUseFixedSize.Checked = settings.NewLineSegmentUseFixedSize;
            }

            if (settings.ChangeLineSegmentBackwardsSize)
            {
                _backwardsSize = settings.NewLineSegmentBackwardsSize;
                string suffix = string.Format(" ({0})", settings.NewLineSegmentBackwardsSize);
                _itemSetBackwardsSize.Text = SET_BACKWARDS_SIZE_TEXT + suffix;
            }

            if (settings.ChangeLineSegmentShowMidline)
            {
                _showMidline = settings.NewLineSegmentShowMidline;
                _itemShowMidline.Checked = settings.NewLineSegmentShowMidline;
            }

            if (settings.ChangeIconSize)
            {
                _iconSize = settings.NewIconSize;
                string suffix = string.Format(" ({0})", settings.NewIconSize);
                _itemSetIconSize.Text = SET_ICON_SIZE_TEXT + suffix;
            }
        }

        public override string GetName()
        {
            return "Line Segment from " + _posAngle1 + " to " + _posAngle2;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.LineSegmentImage;
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle1", _posAngle1),
                new XAttribute("positionAngle2", _posAngle2),
            };
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            if (_customImage == null) return null;

            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            (double x, double y, double z) = PositionAngle.GetMidPoint(_posAngle1, _posAngle2);
            double dist = MoreMath.GetDistanceBetween(x, z, inGameX, inGameZ);
            double radius = Scales ? _iconSize : _iconSize / Config.CurrentMapGraphics.MapViewScaleValue;
            if (dist <= radius || forceCursorPosition)
            {
                return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, x, y, z);
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            if (_customImage == null) return null;

            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            (double x, double y, double z) = PositionAngle.GetMidPoint(_posAngle1, _posAngle2);
            (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView((float)x, (float)y, (float)z, UseRelativeCoordinates);
            double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
            double radius = Scales ? _iconSize * Config.CurrentMapGraphics.MapViewScaleValue : _iconSize;
            if (dist <= radius || forceCursorPosition)
            {
                return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, x, y, z);
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            (double x, double y, double z) = PositionAngle.GetMidPoint(_posAngle1, _posAngle2);
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(x, y, z, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
