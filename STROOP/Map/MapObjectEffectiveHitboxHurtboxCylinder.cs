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
    public class MapObjectEffectiveHitboxHurtboxCylinder : MapObjectCylinder
    {
        private readonly PositionAngle _posAngle;

        private bool _useInteractionStatusAsColor;
        ToolStripMenuItem _useInteractionStatusAsColorItem;

        public MapObjectEffectiveHitboxHurtboxCylinder(PositionAngle posAngle)
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

            float hitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);
            float hitboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxHeightOffset);
            float hitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
            float hitboxMinY = objY - hitboxDownOffset;
            float hitboxMaxY = hitboxMinY + hitboxHeight;

            float hurtboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxRadiusOffset);
            float hurtboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HurtboxHeightOffset);
            float hurtboxMinY = objY - hitboxDownOffset;
            float hurtboxMaxY = hurtboxMinY + hurtboxHeight;

            uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
            float marioHitboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxRadiusOffset);
            float marioHurtboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HurtboxRadiusOffset);
            float marioHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);

            float effectiveHitboxRadius = hitboxRadius + marioHitboxRadius;
            float effectiveHitboxMinY = hitboxMinY - marioHitboxHeight;
            float effectiveHitboxMaxY = hitboxMaxY;

            float effectiveHurtboxRadius = hurtboxRadius + marioHurtboxRadius;
            float effectiveHurtboxMinY = hurtboxMinY - marioHitboxHeight;
            float effectiveHurtboxMaxY = hurtboxMaxY;

            Color hitboxColor = Color;
            Color hurtboxColor = Color;
            if (_useInteractionStatusAsColor)
            {
                hitboxColor = Color.Cyan;
                uint interactionStatus = Config.Stream.GetUInt(_posAngle.GetObjAddress() + ObjectConfig.InteractionStatusOffset);
                hurtboxColor = interactionStatus == 0 ? Color.Red : Color.Cyan;
            }

            return new List<(float centerX, float centerZ, float radius, float minY, float maxY, Color color)>()
            {
                ((float)_posAngle.X, (float)_posAngle.Z, effectiveHitboxRadius, effectiveHitboxMinY, effectiveHitboxMaxY, hitboxColor),
                ((float)_posAngle.X, (float)_posAngle.Z, effectiveHurtboxRadius, effectiveHurtboxMinY, effectiveHurtboxMaxY, hurtboxColor),
            };
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Effective Hitbox/Hurtbox Cylinder for " + _posAngle.GetMapName();
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
