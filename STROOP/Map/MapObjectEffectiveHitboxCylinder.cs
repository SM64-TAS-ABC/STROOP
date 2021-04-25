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

namespace STROOP.Map
{
    public class MapObjectEffectiveHitboxCylinder : MapObjectCylinder
    {
        private readonly PositionAngle _posAngle;

        public MapObjectEffectiveHitboxCylinder(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Color = Color.Purple;
        }

        protected override List<(float centerX, float centerZ, float radius, float minY, float maxY)> Get3DDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
            float hitboxRadius = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxRadiusOffset);
            float hitboxHeight = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxHeightOffset);
            float hitboxDownOffset = Config.Stream.GetFloat(objAddress + ObjectConfig.HitboxDownOffsetOffset);
            float hitboxMinY = objY - hitboxDownOffset;
            float hitboxMaxY = hitboxMinY + hitboxHeight;

            uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
            float marioHitboxRadius = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxRadiusOffset);
            float marioHitboxHeight = Config.Stream.GetFloat(marioObjRef + ObjectConfig.HitboxHeightOffset);

            float effectiveRadius = hitboxRadius + marioHitboxRadius;
            float effectiveMinY = hitboxMinY - marioHitboxHeight;
            float effectiveMaxY = hitboxMaxY;

            return new List<(float centerX, float centerZ, float radius, float minY, float maxY)>()
            {
                ((float)_posAngle.X, (float)_posAngle.Z, effectiveRadius, effectiveMinY, effectiveMaxY)
            };
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Effective Hitbox Cylinder for " + _posAngle.GetMapName();
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }
    }
}
