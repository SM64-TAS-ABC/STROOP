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

        private ToolStripMenuItem _itemSetRelativeMinY;
        private ToolStripMenuItem _itemSetRelativeMaxY;

        private static readonly string SET_RELATIVE_MIN_Y_TEXT = "Set Relative Min Y";
        private static readonly string SET_RELATIVE_MAX_Y_TEXT = "Set Relative Max Y";

        public MapCustomCylinderObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Size = 1000;
        }

        protected override List<(float centerX, float centerZ, float radius, float minY, float maxY)> Get3DDimensions()
        {
            float y = GetY();
            return new List<(float centerX, float centerZ, float radius, float minY, float maxY)>()
            {
                ((float)_posAngle.X, (float)_posAngle.Z, Size, y + _relativeMinY, y + _relativeMaxY)
            };
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Cylinder for " + _posAngle.GetMapName();
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                string suffixMin = string.Format(" ({0})", _relativeMinY);
                _itemSetRelativeMinY = new ToolStripMenuItem(SET_RELATIVE_MIN_Y_TEXT + suffixMin);
                _itemSetRelativeMinY.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the relative min y.");
                    float? relativeMinY = ParsingUtilities.ParseFloatNullable(text);
                    if (!relativeMinY.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeCustomCylinderRelativeMinY: true, newCustomCylinderRelativeMinY: relativeMinY.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                string suffixMax = string.Format(" ({0})", _relativeMaxY);
                _itemSetRelativeMaxY = new ToolStripMenuItem(SET_RELATIVE_MAX_Y_TEXT + suffixMax);
                _itemSetRelativeMaxY.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the relative max y.");
                    float? relativeMaxY = ParsingUtilities.ParseFloatNullable(text);
                    if (!relativeMaxY.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeCustomCylinderRelativeMaxY: true, newCustomCylinderRelativeMaxY: relativeMaxY.Value);
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

            GetContextMenuStrip(); // avoids NPE

            if (settings.ChangeCustomCylinderRelativeMinY)
            {
                _relativeMinY = settings.NewCustomCylinderRelativeMinY;
                string suffix = string.Format(" ({0})", _relativeMinY);
                _itemSetRelativeMinY.Text = SET_RELATIVE_MIN_Y_TEXT + suffix;
            }

            if (settings.ChangeCustomCylinderRelativeMaxY)
            {
                _relativeMaxY = settings.NewCustomCylinderRelativeMaxY;
                string suffix = string.Format(" ({0})", _relativeMaxY);
                _itemSetRelativeMaxY.Text = SET_RELATIVE_MAX_Y_TEXT + suffix;
            }
        }
    }
}
