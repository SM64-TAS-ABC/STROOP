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
    public class MapObjectArrowObject : MapLineObject
    {
        private readonly uint _objAddress;
        private readonly uint _yawOffset;
        private readonly int _numBytes;

        private float _arrowHeadSideLength;

        public MapObjectArrowObject(uint objAddress, uint yawOffset, int numBytes)
            : base()
        {
            _objAddress = objAddress;
            _yawOffset = yawOffset;
            _numBytes = numBytes;

            _arrowHeadSideLength = 100;

            Size = 300;
            OutlineWidth = 3;
            OutlineColor = Color.Yellow;
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            float x = Config.Stream.GetSingle(_objAddress + ObjectConfig.XOffset);
            float y = Config.Stream.GetSingle(_objAddress + ObjectConfig.YOffset);
            float z = Config.Stream.GetSingle(_objAddress + ObjectConfig.ZOffset);
            uint yaw = _numBytes == 2 ?
                Config.Stream.GetUInt16(_objAddress + _yawOffset) :
                Config.Stream.GetUInt32(_objAddress + _yawOffset);
            (float arrowHeadX, float arrowHeadZ) =
                ((float, float))MoreMath.AddVectorToPoint(Size, yaw, x, z);

            (float pointSide1X, float pointSide1Z) =
                ((float, float))MoreMath.AddVectorToPoint(_arrowHeadSideLength, yaw + 32768 + 8192, arrowHeadX, arrowHeadZ);
            (float pointSide2X, float pointSide2Z) =
                ((float, float))MoreMath.AddVectorToPoint(_arrowHeadSideLength, yaw + 32768 - 8192, arrowHeadX, arrowHeadZ);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();

            vertices.Add((x, y, z));
            vertices.Add((arrowHeadX, y, arrowHeadZ));

            vertices.Add((arrowHeadX, y, arrowHeadZ));
            vertices.Add((pointSide1X, y, pointSide1Z));

            vertices.Add((arrowHeadX, y, arrowHeadZ));
            vertices.Add((pointSide2X, y, pointSide2Z));

            return vertices;
        }

        public override string GetName()
        {
            return "Object Arrow";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.ArrowImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemSetArrowHeadSideLength = new ToolStripMenuItem("Set Arrow Head Side Length");
                itemSetArrowHeadSideLength.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the side length of the arrow head:");
                    float? arrowHeadSideLength = ParsingUtilities.ParseFloatNullable(text);
                    if (!arrowHeadSideLength.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        arrowChangeHeadSideLength: true, arrowNewHeadSideLength: arrowHeadSideLength.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSetArrowHeadSideLength);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ArrowChangeHeadSideLength)
            {
                _arrowHeadSideLength = settings.ArrowNewHeadSideLength;
            }
        }
    }
}
