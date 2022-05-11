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
using System.Xml.Linq;
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapObjectEffectiveHurtboxCylinder : MapObjectCylinder
    {
        private readonly PositionAngle _posAngle;

        private bool _useInteractionStatusAsColor;
        ToolStripMenuItem _useInteractionStatusAsColorItem;

        public MapObjectEffectiveHurtboxCylinder(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
            _useInteractionStatusAsColor = false;

            Color = Color.Purple;
        }

        protected override List<(float centerX, float centerZ, float radius, float minY, float maxY, Color color)> Get3DDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
            float hurtboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxRadiusOffset);
            float hurtboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxHeightOffset);
            float hitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
            float hurtboxMinY = objY - hitboxDownOffset;
            float hurtboxMaxY = hurtboxMinY + hurtboxHeight;

            uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
            float marioHurtboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HurtboxRadiusOffset);
            float marioHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);

            float effectiveRadius = hurtboxRadius + marioHurtboxRadius;
            float effectiveMinY = hurtboxMinY - marioHitboxHeight;
            float effectiveMaxY = hurtboxMaxY;

            Color color = Color;
            if (_useInteractionStatusAsColor)
            {
                uint interactionStatus = Config.Stream.GetUInt(_posAngle.GetObjAddress() + ObjectConfig.InteractionStatusOffset);
                color = interactionStatus == 0 ? Color.Red : Color.Cyan;
            }

            return new List<(float centerX, float centerZ, float radius, float minY, float maxY, Color color)>()
            {
                ((float)_posAngle.X, (float)_posAngle.Z, effectiveRadius, effectiveMinY, effectiveMaxY, color)
            };
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Effective Hurtbox Cylinder for " + _posAngle.GetMapName();
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _useInteractionStatusAsColorItem = new ToolStripMenuItem("Use Interaction Status as Color");
                _useInteractionStatusAsColorItem.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeUseInteractionStatusAsColor: true, newUseInteractionStatusAsColor: !_useInteractionStatusAsColor);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_useInteractionStatusAsColorItem);
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetCircleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeUseInteractionStatusAsColor)
            {
                _useInteractionStatusAsColor = settings.NewUseInteractionStatusAsColor;
                _useInteractionStatusAsColorItem.Checked = settings.NewUseInteractionStatusAsColor;
            }
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
