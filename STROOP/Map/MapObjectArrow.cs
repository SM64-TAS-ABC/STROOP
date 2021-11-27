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

namespace STROOP.Map
{
    public abstract class MapObjectArrow : MapObjectLine
    {
        protected bool _useRecommendedArrowLength;
        private bool _useTruncatedAngle;
        private float _arrowHeadSideLength;
        private float _angleOffset;
        private bool _usePitch;

        private ToolStripMenuItem _itemUseRecommendedArrowLength;
        private ToolStripMenuItem _itemUseTruncatedAngle;
        private ToolStripMenuItem _itemSetArrowHeadSideLength;
        private ToolStripMenuItem _itemSetAngleOffset;
        private ToolStripMenuItem _itemUsePitch;

        private static readonly string SET_ARROW_HEAD_SIDE_LENGTH_TEXT = "Set Arrow Head Side Length";
        private static readonly string SET_ANGLE_OFFSET_TEXT = "Set Angle Offset";

        public MapObjectArrow()
            : base()
        {
            _useRecommendedArrowLength = false;
            _useTruncatedAngle = true;
            _arrowHeadSideLength = 100;
            _angleOffset = 0;
            _usePitch = false;

            Size = 300;
            LineWidth = 3;
            LineColor = Color.Yellow;
            Scales = true;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            PositionAngle posAngle = GetPositionAngle();
            float x = (float)posAngle.X;
            float y = (float)posAngle.Y;
            float z = (float)posAngle.Z;
            float preYaw = (float)GetYaw() + _angleOffset;
            float yaw = _useTruncatedAngle ? MoreMath.NormalizeAngleTruncated(preYaw) : preYaw;
            float pitch = _usePitch ? (float)GetPitch() : 0;
            float size = _useRecommendedArrowLength ? (float)GetRecommendedSize() : Size;
            if (!Scales) size /= Config.CurrentMapGraphics.MapViewScaleValue;
            float sideLength = _arrowHeadSideLength;
            if (!Scales) sideLength /= Config.CurrentMapGraphics.MapViewScaleValue;

            float arrowHeadX;
            float arrowHeadY;
            float arrowHeadZ;
            if (_usePitch)
            {
                (arrowHeadX, arrowHeadY, arrowHeadZ) = ((float, float, float))
                    MoreMath.AddVectorToPointWithPitch(size, yaw, pitch, x, y, z, false);
            }
            else
            {
                (arrowHeadX, arrowHeadZ) = ((float, float))
                    MoreMath.AddVectorToPoint(size, yaw, x, z);
                arrowHeadY = y;
            }

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            vertices.Add((x, y, z));
            vertices.Add((arrowHeadX, arrowHeadY, arrowHeadZ));

            if (Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked)
            {
                (float pointSide1X, float pointSide1Y, float pointSide1Z) =
                    ((float, float, float))MoreMath.AddVectorToPointWithPitch(
                        sideLength, yaw + 32768, -pitch + 8192, arrowHeadX, arrowHeadY, arrowHeadZ, false);
                (float pointSide2X, float pointSide2Y, float pointSide2Z) =
                    ((float, float, float))MoreMath.AddVectorToPointWithPitch(
                        sideLength, yaw + 32768, -pitch - 8192, arrowHeadX, arrowHeadY, arrowHeadZ, false);

                vertices.Add((arrowHeadX, arrowHeadY, arrowHeadZ));
                vertices.Add((pointSide1X, pointSide1Y, pointSide1Z));

                vertices.Add((arrowHeadX, arrowHeadY, arrowHeadZ));
                vertices.Add((pointSide2X, pointSide2Y, pointSide2Z));
            }
            else
            {
                (float pointSide1X, float pointSide1Z) =
                    ((float, float))MoreMath.AddVectorToPoint(sideLength, yaw + 32768 + 8192, arrowHeadX, arrowHeadZ);
                (float pointSide2X, float pointSide2Z) =
                    ((float, float))MoreMath.AddVectorToPoint(sideLength, yaw + 32768 - 8192, arrowHeadX, arrowHeadZ);

                vertices.Add((arrowHeadX, arrowHeadY, arrowHeadZ));
                vertices.Add((pointSide1X, arrowHeadY, pointSide1Z));

                vertices.Add((arrowHeadX, arrowHeadY, arrowHeadZ));
                vertices.Add((pointSide2X, arrowHeadY, pointSide2Z));
            }

            return vertices;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            byte opacityByte = OpacityByte;
            if (this == hoverData?.MapObject)
            {
                opacityByte = MapUtilities.GetHoverOpacityByte();
            }
            MapUtilities.DrawLinesOn2DControlTopDownView(GetVerticesTopDownView(), LineWidth, LineColor, opacityByte);
        }

        private (float x, float y, float z) GetArrowHeadPosition()
        {
            PositionAngle posAngle = GetPositionAngle();
            float x = (float)posAngle.X;
            float y = (float)posAngle.Y;
            float z = (float)posAngle.Z;
            float preYaw = (float)GetYaw() + _angleOffset;
            float yaw = _useTruncatedAngle ? MoreMath.NormalizeAngleTruncated(preYaw) : preYaw;
            float pitch = _usePitch ? (float)GetPitch() : 0;
            float size = _useRecommendedArrowLength ? (float)GetRecommendedSize() : Size;
            if (!Scales) size /= Config.CurrentMapGraphics.MapViewScaleValue;

            float arrowHeadX;
            float arrowHeadY;
            float arrowHeadZ;
            if (_usePitch)
            {
                (arrowHeadX, arrowHeadY, arrowHeadZ) = ((float, float, float))
                    MoreMath.AddVectorToPointWithPitch(size, yaw, pitch, x, y, z, false);
            }
            else
            {
                (arrowHeadX, arrowHeadZ) = ((float, float))
                    MoreMath.AddVectorToPoint(size, yaw, x, z);
                arrowHeadY = y;
            }

            return (arrowHeadX, arrowHeadY, arrowHeadZ);
        }

        public override (double x, double y, double z)? GetDragPosition()
        {
            return GetArrowHeadPosition();
        }

        public override void SetDragPositionTopDownView(double? x = null, double? y = null, double? z = null)
        {
            if (!x.HasValue || !z.HasValue) return;

            PositionAngle posAngle = GetPositionAngle();
            double dist = MoreMath.GetDistanceBetween(posAngle.X, posAngle.Z, x.Value, z.Value);
            double angle = MoreMath.AngleTo_AngleUnits(posAngle.X, posAngle.Z, x.Value, z.Value);

            if (!KeyboardUtilities.IsCtrlHeld())
            {
                if (_useRecommendedArrowLength)
                {
                    SetRecommendedSize(dist);
                }
                else
                {
                    GetParentMapTracker().SetSize((float)(Scales ? dist : dist * Config.CurrentMapGraphics.MapViewScaleValue));
                }
            }
            if (!KeyboardUtilities.IsShiftHeld())
            {
                SetYaw(angle);
            }
        }

        protected abstract double GetYaw();

        protected abstract double GetPitch();

        protected abstract double GetRecommendedSize();

        protected abstract void SetRecommendedSize(double size);

        protected abstract void SetYaw(double yaw);

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.ArrowImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _itemUseRecommendedArrowLength = new ToolStripMenuItem("Use Recommended Arrow Size");
                _itemUseRecommendedArrowLength.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeArrowUseRecommendedLength: true,
                        newArrowUseRecommendedLength: !_useRecommendedArrowLength);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemUseRecommendedArrowLength.Checked = _useRecommendedArrowLength;

                _itemUseTruncatedAngle = new ToolStripMenuItem("Use Truncated Angle");
                _itemUseTruncatedAngle.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeArrowUseTruncatedAngle: true,
                        newArrowUseTruncatedAngle: !_useTruncatedAngle);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemUseTruncatedAngle.Checked = _useTruncatedAngle;

                string suffix1 = string.Format(" ({0})", _arrowHeadSideLength);
                _itemSetArrowHeadSideLength = new ToolStripMenuItem(SET_ARROW_HEAD_SIDE_LENGTH_TEXT + suffix1);
                _itemSetArrowHeadSideLength.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the side length of the arrow head:");
                    float? arrowHeadSideLength = ParsingUtilities.ParseFloatNullable(text);
                    if (!arrowHeadSideLength.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeArrowHeadSideLength: true, newArrowHeadSideLength: arrowHeadSideLength.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                string suffix2 = string.Format(" ({0})", _angleOffset);
                _itemSetAngleOffset = new ToolStripMenuItem(SET_ANGLE_OFFSET_TEXT + suffix2);
                _itemSetAngleOffset.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the angle offset:");
                    float? angleOffsetNullable = ParsingUtilities.ParseFloatNullable(text);
                    if (!angleOffsetNullable.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeArrowAngleOffset: true, newArrowAngleOffset: angleOffsetNullable.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _itemUsePitch = new ToolStripMenuItem("Use Pitch");
                _itemUsePitch.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeUsePitch: true, newUsePitch: !_usePitch);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemUsePitch.Checked = _usePitch;

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemUseRecommendedArrowLength);
                _contextMenuStrip.Items.Add(_itemUseTruncatedAngle);
                _contextMenuStrip.Items.Add(_itemSetArrowHeadSideLength);
                _contextMenuStrip.Items.Add(_itemSetAngleOffset);
                _contextMenuStrip.Items.Add(_itemUsePitch);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeArrowUseRecommendedLength)
            {
                _useRecommendedArrowLength = settings.NewArrowUseRecommendedLength;
                _itemUseRecommendedArrowLength.Checked = _useRecommendedArrowLength;
            }

            if (settings.ChangeArrowUseTruncatedAngle)
            {
                _useTruncatedAngle = settings.NewArrowUseTruncatedAngle;
                _itemUseTruncatedAngle.Checked = _useTruncatedAngle;
            }

            if (settings.ChangeArrowHeadSideLength)
            {
                _arrowHeadSideLength = settings.NewArrowHeadSideLength;
                string suffix = string.Format(" ({0})", _arrowHeadSideLength);
                _itemSetArrowHeadSideLength.Text = SET_ARROW_HEAD_SIDE_LENGTH_TEXT + suffix;
            }

            if (settings.ChangeArrowAngleOffset)
            {
                _angleOffset = settings.NewArrowAngleOffset;
                string suffix = string.Format(" ({0})", _angleOffset);
                _itemSetAngleOffset.Text = SET_ANGLE_OFFSET_TEXT + suffix;
            }

            if (settings.ChangeUsePitch)
            {
                _usePitch = settings.NewUsePitch;
                _itemUsePitch.Checked = _usePitch;
            }
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            (float inGameX, float inGameY, float inGameZ) = GetArrowHeadPosition();
            (double controlX, double controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(inGameX, inGameZ);
            double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
            if (dist <= 20 || forceCursorPosition)
            {
                return new MapObjectHoverData(this, inGameX, inGameY, inGameZ);
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            (double x, double y, double z) = GetArrowHeadPosition();
            List<double> posValues = new List<double>() { x, y, z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Arrow Head Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
