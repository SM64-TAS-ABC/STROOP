﻿using System;
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

        protected abstract List<(float x, float y, float z)> GetGridlineIntersectionPositionsTopDownView();

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            base.DrawOn2DControlTopDownView(hoverData);

            if (_customImage != null)
            {
                List<(float x, float y, float z)> positions = GetGridlineIntersectionPositionsTopDownView();
                for (int i = 0; i < positions.Count; i++)
                {
                    (float x, float y, float z) = positions[i];
                    (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(x, z, UseRelativeCoordinates);
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

        protected abstract List<(float x, float y, float z)> GetGridlineIntersectionPositionsOrthographicView();

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            base.DrawOn2DControlOrthographicView(hoverData);

            if (_customImage != null)
            {
                List<(float x, float y, float z)> positions = GetGridlineIntersectionPositionsOrthographicView();
                for (int i = 0; i < positions.Count; i++)
                {
                    (float x, float y, float z) = positions[i];
                    (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(x, y, z, UseRelativeCoordinates);
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

        protected List<ToolStripMenuItem> GetGridlinesToolStripMenuItems()
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

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            if (_customImage == null) return null;

            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            var positions = GetGridlineIntersectionPositionsTopDownView();
            for (int i = positions.Count - 1; i >= 0; i--)
            {
                var position = positions[i];
                double dist = MoreMath.GetDistanceBetween(position.x, position.z, inGameX, inGameZ);
                double radius = Scales ? _imageSize : _imageSize / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, position.x, 0, position.z, index: i);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            if (_customImage == null) return null;

            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            var positions = GetGridlineIntersectionPositionsOrthographicView();
            for (int i = positions.Count - 1; i >= 0; i--)
            {
                var position = positions[i];
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(position.x, position.y, position.z, UseRelativeCoordinates);
                double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                double radius = Scales ? _imageSize * Config.CurrentMapGraphics.MapViewScaleValue : _imageSize;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, position.x, position.y, position.z, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var positions = Config.CurrentMapGraphics.IsOrthographicViewEnabled
                ? GetGridlineIntersectionPositionsOrthographicView()
                : GetGridlineIntersectionPositionsTopDownView();
            var position = positions[hoverData.Index.Value];
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(position.x, position.y, position.z, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
