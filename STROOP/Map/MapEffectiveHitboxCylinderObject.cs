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
    public class MapEffectiveHitboxCylinderObject : MapCylinderObject
    {
        private readonly PositionAngle _posAngle;

        public MapEffectiveHitboxCylinderObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Color = Color.Green;
        }

        protected override List<(float centerX, float centerZ, float radius, float minY, float maxY)> Get3DDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
            float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadiusOffset);
            float hitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeightOffset);
            float hitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffsetOffset);
            float hitboxMinY = objY - hitboxDownOffset;
            float hitboxMaxY = hitboxMinY + hitboxHeight;

            uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
            float marioHitboxRadius = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxRadiusOffset);
            float effectiveRadius = hitboxRadius + marioHitboxRadius;

            return new List<(float centerX, float centerZ, float radius, float minY, float maxY)>()
            {
                ((float)_posAngle.X, (float)_posAngle.Z, effectiveRadius, hitboxMinY, hitboxMaxY)
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
