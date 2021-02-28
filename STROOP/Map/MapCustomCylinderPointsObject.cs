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

namespace STROOP.Map
{
    public class MapCustomCylinderPointsObject : MapCylinderObject
    {
        private readonly List<(float x, float y, float z)> _points;

        private float _relativeMinY = 0;
        private float _relativeMaxY = 100;

        private ToolStripMenuItem _itemSetRelativeMinY;
        private ToolStripMenuItem _itemSetRelativeMaxY;

        private static readonly string SET_RELATIVE_MIN_Y_TEXT = "Set Relative Min Y";
        private static readonly string SET_RELATIVE_MAX_Y_TEXT = "Set Relative Max Y";

        public MapCustomCylinderPointsObject(List<(float x, float y, float z)> points)
            : base()
        {
            _points = points;

            Size = 100;
        }

        public static MapCustomCylinderPointsObject Create(string text, bool useTriplets)
        {
            List<(double x, double y, double z)> points = MapUtilities.ParsePoints(text, useTriplets);
            if (points == null) return null;
            List<(float x, float y, float z)> floatPoints = points.ConvertAll(
                point => ((float)point.x, (float)point.y, (float)point.z));
            return new MapCustomCylinderPointsObject(floatPoints);
        }

        protected override List<(float centerX, float centerZ, float radius, float minY, float maxY)> Get3DDimensions()
        {
            return _points.ConvertAll(point => (point.x, point.z, Size, point.y + _relativeMinY, point.y + _relativeMaxY));
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Custom Cylinder Points";
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                string suffixMin = string.Format(" ({0})", _relativeMinY);
                _itemSetRelativeMinY = new ToolStripMenuItem(SET_RELATIVE_MIN_Y_TEXT + suffixMin);
                _itemSetRelativeMinY.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter a number.");
                    float? relativeMinY = ParsingUtilities.ParseFloatNullable(text);
                    if (!relativeMinY.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        customCylinderChangeRelativeMinY: true, customCylinderNewRelativeMinY: relativeMinY.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                string suffixMax = string.Format(" ({0})", _relativeMaxY);
                _itemSetRelativeMaxY = new ToolStripMenuItem(SET_RELATIVE_MAX_Y_TEXT + suffixMax);
                _itemSetRelativeMaxY.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter a number.");
                    float? relativeMaxY = ParsingUtilities.ParseFloatNullable(text);
                    if (!relativeMaxY.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        customCylinderChangeRelativeMaxY: true, customCylinderNewRelativeMaxY: relativeMaxY.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemSetRelativeMinY);
                _contextMenuStrip.Items.Add(_itemSetRelativeMaxY);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.CustomCylinderChangeRelativeMinY)
            {
                _relativeMinY = settings.CustomCylinderNewRelativeMinY;
                string suffix = string.Format(" ({0})", _relativeMinY);
                _itemSetRelativeMinY.Text = SET_RELATIVE_MIN_Y_TEXT + suffix;
            }

            if (settings.CustomCylinderChangeRelativeMaxY)
            {
                _relativeMaxY = settings.CustomCylinderNewRelativeMaxY;
                string suffix = string.Format(" ({0})", _relativeMaxY);
                _itemSetRelativeMaxY.Text = SET_RELATIVE_MAX_Y_TEXT + suffix;
            }
        }
    }
}
