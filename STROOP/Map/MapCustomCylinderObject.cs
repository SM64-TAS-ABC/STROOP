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
    public class MapCustomCylinderObject : MapCylinderObject
    {
        private readonly PositionAngle _posAngle;

        private float _relativeMinY = 0;
        private float _relativeMaxY = 100;

        public MapCustomCylinderObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Size = 1000;
        }

        protected override (float centerX, float centerZ, float radius, float minY, float maxY) Get3DDimensions()
        {
            float y = GetY();
            return ((float)_posAngle.X, (float)_posAngle.Z, Size, y + _relativeMinY, y + _relativeMaxY);
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Cylinder for " + _posAngle.GetMapName();
        }

        public override float GetY()
        {
            return (float)_posAngle.Y;
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
