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
    public class MapAngleRangeObject : MapLineObject
    {
        private readonly PositionAngle _posAngle;

        private bool _useRelativeAngles;
        private int _angleDiff;
        private bool _useInGameAngles;

        private ToolStripMenuItem _itemUseRelativeAngles;
        private ToolStripMenuItem _itemUseInGameAngles;

        public MapAngleRangeObject(PositionAngle posAngle)
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
            return Config.ObjectAssociations.CustomGridlinesImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _itemUseRelativeAngles = new ToolStripMenuItem("Use Relative Angles");
                _itemUseRelativeAngles.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        angleRangeChangeUseRelativeAngles: true,
                        angleRangeNewUseRelativeAngles: !_useRelativeAngles);
                    GetParentMapTracker().ApplySettings(settings);
                };

                ToolStripMenuItem itemSetAngleDiff = new ToolStripMenuItem("Set Angle Diff");
                itemSetAngleDiff.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter angle diff.");
                    int? angleDiff = ParsingUtilities.ParseIntNullable(text);
                    if (!angleDiff.HasValue || angleDiff.Value <= 0) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        angleRangeChangeAngleDiff: true, angleRangeNewAngleDiff: angleDiff.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _itemUseInGameAngles = new ToolStripMenuItem("Use In-Game Angles");
                _itemUseInGameAngles.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        angleRangeChangeUseInGameAngles: true,
                        angleRangeNewUseInGameAngles: !_useInGameAngles);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemUseRelativeAngles);
                _contextMenuStrip.Items.Add(itemSetAngleDiff);
                _contextMenuStrip.Items.Add(_itemUseInGameAngles);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.AngleRangeChangeUseRelativeAngles)
            {
                _useRelativeAngles = settings.AngleRangeNewUseRelativeAngles;
                _itemUseRelativeAngles.Checked = settings.AngleRangeNewUseRelativeAngles;
            }

            if (settings.AngleRangeChangeAngleDiff)
            {
                _angleDiff = settings.AngleRangeNewAngleDiff;
            }

            if (settings.AngleRangeChangeUseInGameAngles)
            {
                _useInGameAngles = settings.AngleRangeNewUseInGameAngles;
                _itemUseInGameAngles.Checked = settings.AngleRangeNewUseInGameAngles;
            }
        }
    }
}
