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

namespace STROOP.Map3
{
    public class MapHitboxCylinderObject : MapCylinderObject
    {
        private readonly PositionAngle _posAngle;

        public MapHitboxCylinderObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        protected override (float centerX, float centerZ, float radius, float minY, float maxY) Get3DDimensions()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
            float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadiusOffset);
            float hitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeightOffset);
            float hitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffsetOffset);
            float hitboxMinY = objY - hitboxDownOffset;
            float hitboxMaxY = hitboxMinY + hitboxHeight;
            return ((float)_posAngle.X, (float)_posAngle.Z, hitboxRadius, hitboxMinY, hitboxMaxY);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.CylinderImage;
        }

        public override string GetName()
        {
            return "Hitbox Cylinder for " + _posAngle.GetMapName();
        }

        public override float GetY()
        {
            return (float)_posAngle.Y;
        }
    }
}
