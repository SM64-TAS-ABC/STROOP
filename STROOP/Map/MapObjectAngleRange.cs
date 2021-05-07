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

namespace STROOP.Map
{
    public class MapObjectAngleRange : MapObjectLine
    {
        private readonly PositionAngle _posAngle;

        private bool _useRelativeAngles;
        private int _angleDiff;
        private bool _useInGameAngles;

        private ToolStripMenuItem _itemUseRelativeAngles;
        private ToolStripMenuItem _itemSetAngleDiff;
        private ToolStripMenuItem _itemUseInGameAngles;

        private static readonly string SET_ANGLE_DIFF_TEXT = "Set Angle Diff";

        public MapObjectAngleRange(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            _useRelativeAngles = false;
            _angleDiff = 16;
            _useInGameAngles = false;

            Size = 1000;
            OutlineWidth = 1;
            OutlineColor = Color.Black;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            (double x1, double y1, double z1, double a) = _posAngle.GetValues();
            int startingAngle = _useRelativeAngles ? MoreMath.NormalizeAngleTruncated(a) : 0;
            void addPointUsingAngle(int angle)
            {
                (double x2, double z2) = MoreMath.AddVectorToPoint(Size, angle, x1, z1);
                vertices.Add(((float)x1, (float)y1, (float)z1));
                vertices.Add(((float)x2, (float)y1, (float)z2));
            }
            if (_useInGameAngles)
            {
                foreach (int angle in InGameTrigUtilities.GetInGameAngles())
                {
                    addPointUsingAngle(MoreMath.NormalizeAngleTruncated(angle));
                }
            }
            else
            {
                for (int angle = startingAngle; angle < startingAngle + 65536; angle += _angleDiff)
                {
                    addPointUsingAngle(angle);
                }
            }
            return vertices;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override string GetName()
        {
            return "Angle Range";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.AngleRangeImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _itemUseRelativeAngles = new ToolStripMenuItem("Use Relative Angles");
                _itemUseRelativeAngles.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeAngleRangeUseRelativeAngles: true,
                        newAngleRangeUseRelativeAngles: !_useRelativeAngles);
                    GetParentMapTracker().ApplySettings(settings);
                };

                string suffix = string.Format(" ({0})", _angleDiff);
                _itemSetAngleDiff = new ToolStripMenuItem(SET_ANGLE_DIFF_TEXT + suffix);
                _itemSetAngleDiff.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter angle diff.");
                    int? angleDiff = ParsingUtilities.ParseIntNullable(text);
                    if (!angleDiff.HasValue || angleDiff.Value <= 0) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeAngleRangeAngleDiff: true, newAngleRangeAngleDiff: angleDiff.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _itemUseInGameAngles = new ToolStripMenuItem("Use In-Game Angles");
                _itemUseInGameAngles.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeAngleRangeUseInGameAngles: true,
                        newAngleRangeUseInGameAngles: !_useInGameAngles);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemUseRelativeAngles);
                _contextMenuStrip.Items.Add(_itemSetAngleDiff);
                _contextMenuStrip.Items.Add(_itemUseInGameAngles);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeAngleRangeUseRelativeAngles)
            {
                _useRelativeAngles = settings.NewAngleRangeUseRelativeAngles;
                _itemUseRelativeAngles.Checked = settings.NewAngleRangeUseRelativeAngles;
            }

            if (settings.ChangeAngleRangeAngleDiff)
            {
                _angleDiff = settings.NewAngleRangeAngleDiff;
                string suffix = string.Format(" ({0})", _angleDiff);
                _itemSetAngleDiff.Text = SET_ANGLE_DIFF_TEXT + suffix;
            }

            if (settings.ChangeAngleRangeUseInGameAngles)
            {
                _useInGameAngles = settings.NewAngleRangeUseInGameAngles;
                _itemUseInGameAngles.Checked = settings.NewAngleRangeUseInGameAngles;
            }
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
            };
        }
    }
}
