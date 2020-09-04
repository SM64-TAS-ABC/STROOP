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
    public class MapHurtboxCylinderObject : MapCylinderObject
    {
        private readonly PositionAngle _posAngle;

        public MapHurtboxCylinderObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        protected override List<(float centerX, float centerZ, float radius, float minY, float maxY)> Get3DDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
            float hurtboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HurtboxRadiusOffset);
            float hurtboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HurtboxHeightOffset);
            float hitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffsetOffset);
            float hurtboxMinY = objY - hitboxDownOffset;
            float hurtboxMaxY = hurtboxMinY + hurtboxHeight;
            return new List<(float centerX, float centerZ, float radius, float minY, float maxY)>()
            {
                ((float)_posAngle.X, (float)_posAngle.Z, hurtboxRadius, hurtboxMinY, hurtboxMaxY)
            };
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Hurtbox Cylinder for " + _posAngle.GetMapName();
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }
    }
}
