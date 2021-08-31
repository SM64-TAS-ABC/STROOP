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
    public abstract class MapObjectGridlines : MapObjectLine
    {
        private float _imageSize;
        private ToolStripMenuItem _itemSetIconSize;
        private static readonly string SET_ICON_SIZE_TEXT = "Set Icon Size";

        public MapObjectGridlines()
            : base()
        {
            _imageSize = 8;
        }

        protected virtual List<(float x, float z)> GetGridlineIntersectionPositions()
        {
            return new List<(float x, float z)>();
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            base.DrawOn2DControlTopDownView(hoverData);

            if (_customImage != null)
            {
                List<(float x, float z)> positions = GetGridlineIntersectionPositions();
                for (int i = 0; i < positions.Count; i++)
                {
                    (float x, float z) = positions[i];
                    (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                    SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, _imageSize, Scales);
                    double opacity = Opacity;
                    if (this == hoverData?.MapObject && i == hoverData?.Index)
                    {
                        opacity = MapUtilities.GetHoverOpacity();
                    }
                    MapUtilities.DrawTexture(_customImageTex.Value, new PointF(controlX, controlZ), size, 0, opacity);
                }
            }
        }

        protected List<ToolStripMenuItem> GetLineToolStripMenuItems()
        {
            string suffix = string.Format(" ({0})", _imageSize);
            _itemSetIconSize = new ToolStripMenuItem(SET_ICON_SIZE_TEXT + suffix);
            _itemSetIconSize.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter icon size.");
                float? sizeNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!sizeNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    changeIconSize: true, newIconSize: sizeNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            return new List<ToolStripMenuItem>()
            {
                _itemSetIconSize,
            };
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeIconSize)
            {
                _imageSize = settings.NewIconSize;
                string suffix = string.Format(" ({0})", _imageSize);
                _itemSetIconSize.Text = SET_ICON_SIZE_TEXT + suffix;
            }
        }

        public override MapObjectHoverData GetHoverData()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            var positions = GetGridlineIntersectionPositions();
            for (int i = positions.Count - 1; i >= 0; i--)
            {
                var position = positions[i];
                double dist = MoreMath.GetDistanceBetween(position.x, position.z, inGameX, inGameZ);
                double radius = Scales ? _imageSize : _imageSize / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius)
                {
                    return new MapObjectHoverData(this, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var positions = GetGridlineIntersectionPositions();
            var position = positions[hoverData.Index.Value];
            List<double> posValues = new List<double>() { position.x, position.z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
