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
using System.Xml.Linq;
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapObjectOffsetPositionAngle : MapObjectIconPoint
    {
        private readonly PositionAngle _posAngle;

        private static readonly string SET_ICON_SIZE_TEXT = "Set Icon Size";
        private ToolStripMenuItem _itemSetIconSize;
        private float _iconSize;

        public MapObjectOffsetPositionAngle(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
            _iconSize = 25;

            Size = 100;
            LineWidth = 0;
        }

        public override void Update()
        {
            base.Update();

            _posAngle.SetOffsetDist(Size);
            _posAngle.SetOffsetAngle(LineWidth);
            _posAngle.SetOffsetAngleRelative(Rotates);
        }

        public override void SetDragPositionTopDownView(double? x = null, double? y = null, double? z = null)
        {
            if (!x.HasValue || !z.HasValue) return;

            PositionAngle posAngle = _posAngle.GetBasePositionAngle();
            double dist = MoreMath.GetDistanceBetween(posAngle.X, posAngle.Z, x.Value, z.Value);
            double angle = MoreMath.AngleTo_AngleUnits(posAngle.X, posAngle.Z, x.Value, z.Value);
            if (Rotates)
            {
                angle -= posAngle.Angle;
            }

            if (!KeyboardUtilities.IsCtrlHeld())
            {
                _posAngle.SetOffsetDist(dist);
            }
            if (!KeyboardUtilities.IsShiftHeld())
            {
                _posAngle.SetOffsetAngle(angle);
            }

            MapTracker mapTracker = GetParentMapTracker();
            mapTracker.SetSize((float)_posAngle.GetOffsetDist());
            mapTracker.SetLineWidth((float)_posAngle.GetOffsetAngle());
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.GreenMarioMapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override string GetName()
        {
            return _posAngle.GetMapName();
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                string suffix = string.Format(" ({0})", _iconSize);
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

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemSetIconSize);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeIconSize)
            {
                _iconSize = settings.NewIconSize;
                string suffix = string.Format(" ({0})", _iconSize);
                _itemSetIconSize.Text = SET_ICON_SIZE_TEXT + suffix;
            }
        }

        public override float GetSize()
        {
            return _iconSize;
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
