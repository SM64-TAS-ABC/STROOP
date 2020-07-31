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
    public class MapLineSegmentObject : MapLineObject
    {
        private PositionAngle _posAngle1;
        private PositionAngle _posAngle2;
        private bool _useFixedSize;
        private float _backwardsSize;

        public MapLineSegmentObject(PositionAngle posAngle1, PositionAngle posAngle2)
            : base()
        {
            _posAngle1 = posAngle1;
            _posAngle2 = posAngle2;
            _useFixedSize = false;
            _backwardsSize = 0;

            Size = 0;
            OutlineWidth = 3;
            OutlineColor = Color.Red;
        }

        public static MapObject Create(string text1, string text2)
        {
            PositionAngle posAngle1 = PositionAngle.FromString(text1);
            PositionAngle posAngle2 = PositionAngle.FromString(text2);
            if (posAngle1 == null || posAngle2 == null) return null;
            return new MapLineSegmentObject(posAngle1, posAngle2);
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            (double x1, double y1, double z1, double angle1) = _posAngle1.GetValues();
            (double x2, double y2, double z2, double angle2) = _posAngle2.GetValues();
            double dist = PositionAngle.GetHDistance(_posAngle1, _posAngle2);
            (double startX, double startZ) = MoreMath.ExtrapolateLine2D(x2, z2, x1, z1, dist + _backwardsSize);
            (double endX, double endZ) = MoreMath.ExtrapolateLine2D(x1, z1, x2, z2, (_useFixedSize ? 0 : dist) + Size);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            vertices.Add(((float)startX, 0, (float)startZ));
            vertices.Add(((float)endX, 0, (float)endZ));
            return vertices;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemUseFixedSize = new ToolStripMenuItem("Use Fixed Size");
                itemUseFixedSize.Click += (sender, e) =>
                {
                    _useFixedSize = !_useFixedSize;
                    itemUseFixedSize.Checked = _useFixedSize;
                };

                ToolStripMenuItem itemSetBackwardsSize = new ToolStripMenuItem("Set Backwards Size...");
                itemSetBackwardsSize.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter backwards size.");
                    double? backwardsSizeNullable = ParsingUtilities.ParseDoubleNullable(text);
                    if (!backwardsSizeNullable.HasValue) return;
                    _backwardsSize = (float)backwardsSizeNullable.Value;
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemUseFixedSize);
                _contextMenuStrip.Items.Add(itemSetBackwardsSize);
            }

            return _contextMenuStrip;
        }

        public override string GetName()
        {
            return "Line Segment";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.ArrowImage;
        }
    }
}
