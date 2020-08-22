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
    public class MapCustomCirclePointsObject : MapCylinderObject
    {
        private readonly List<(float x, float y, float z)> _points;

        private float _relativeMinY = 0;
        private float _relativeMaxY = 100;

        public MapCustomCirclePointsObject(List<(float x, float y, float z)> points)
            : base()
        {
            _points = points;

            Size = 100;
        }

        public static MapCustomCirclePointsObject Create2D(string text)
        {
            if (text == null) return null;
            List<float?> nullableFloatList = ParsingUtilities.ParseStringList(text)
                .ConvertAll(word => ParsingUtilities.ParseFloatNullable(word));
            if (nullableFloatList.Any(nullableFloat => !nullableFloat.HasValue))
            {
                return null;
            }
            List<float> floatList = nullableFloatList.ConvertAll(nullableFloat => nullableFloat.Value);
            if (floatList.Count % 2 != 0)
            {
                return null;
            }
            List<(float x, float y, float z)> circlePoints = new List<(float x, float y, float z)>();
            for (int i = 0; i < floatList.Count; i += 2)
            {
                circlePoints.Add((floatList[i], 0, floatList[i + 1]));
            }
            return new MapCustomCirclePointsObject(circlePoints);
        }

        public static MapCustomCirclePointsObject Create3D(string text)
        {
            if (text == null) return null;
            List<float?> nullableFloatList = ParsingUtilities.ParseStringList(text)
                .ConvertAll(word => ParsingUtilities.ParseFloatNullable(word));
            if (nullableFloatList.Any(nullableFloat => !nullableFloat.HasValue))
            {
                return null;
            }
            List<float> floatList = nullableFloatList.ConvertAll(nullableFloat => nullableFloat.Value);
            if (floatList.Count % 3 != 0)
            {
                return null;
            }
            List<(float x, float y, float z)> circlePoints = new List<(float x, float y, float z)>();
            for (int i = 0; i < floatList.Count; i += 3)
            {
                circlePoints.Add((floatList[i], floatList[i + 1], floatList[i + 2]));
            }
            return new MapCustomCirclePointsObject(circlePoints);
        }

        protected override List<(float centerX, float centerZ, float radius, float minY, float maxY)> Get3DDimensions()
        {
            return _points.ConvertAll(point => (point.x, point.z, Size, _relativeMinY, _relativeMaxY));
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Custom Circle Points";
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemSetRelativeMinY = new ToolStripMenuItem("Set Relative Min Y...");
                itemSetRelativeMinY.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter a number.");
                    float? relativeMinY = ParsingUtilities.ParseFloatNullable(text);
                    if (!relativeMinY.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        customCylinderChangeRelativeMinY: true, customCylinderNewRelativeMinY: relativeMinY.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                ToolStripMenuItem itemSetRelativeMaxY = new ToolStripMenuItem("Set Relative Max Y...");
                itemSetRelativeMaxY.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter a number.");
                    float? relativeMaxY = ParsingUtilities.ParseFloatNullable(text);
                    if (!relativeMaxY.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        customCylinderChangeRelativeMaxY: true, customCylinderNewRelativeMaxY: relativeMaxY.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSetRelativeMinY);
                _contextMenuStrip.Items.Add(itemSetRelativeMaxY);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.CustomCylinderChangeRelativeMinY)
            {
                _relativeMinY = settings.CustomCylinderNewRelativeMinY;
            }

            if (settings.CustomCylinderChangeRelativeMaxY)
            {
                _relativeMaxY = settings.CustomCylinderNewRelativeMaxY;
            }
        }
    }
}
