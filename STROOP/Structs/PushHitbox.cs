﻿using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class PushHitbox
    {
        private readonly int? _padding;
        private readonly int? _radius;
        private readonly int? _extentY;
        private readonly bool _isKoopaTheQuick;
        private readonly bool _isRacingPenguin;

        public PushHitbox(
            int? padding,
            int? radius,
            int? extentY,
            bool isKoopaTheQuick,
            bool isRacingPenguin)
        {
            _padding = padding;
            _radius = radius;
            _extentY = extentY;
            _isKoopaTheQuick = isKoopaTheQuick;
            _isRacingPenguin = isRacingPenguin;
        }

        public (float radius, float minY, float maxY) GetDetails(uint objAddress)
        {
            int? padding = _padding;
            int? radius = _radius;
            int? extentY = _extentY;

            if (_isKoopaTheQuick)
            {
                int action = Config.Stream.GetInt(objAddress + ObjectConfig.ActionOffset);
                radius = action == 3 ? 180 : 140;
                extentY = 300;
            }
            
            if (_isRacingPenguin)
            {
                int subType = Config.Stream.GetInt(objAddress + ObjectConfig.BehaviorSubtypeOffset);
                radius = subType == 0 ? 200 : 350;
                extentY = subType == 0 ? 200 : 250;
            }

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

            float finalRadius = radius ?? effectiveRadius + (padding ?? 0);
            float finalMinY;
            float finalMaxY;
            if (radius.HasValue && !extentY.HasValue) // pole
            {
                finalMinY = objY - 10;
                finalMaxY = objY + hitboxHeight + 30;
            }
            else if (extentY.HasValue)
            {
                finalMinY = objY - extentY.Value;
                finalMaxY = objY + extentY.Value;
            }
            else
            {
                finalMinY = effectiveMinY;
                finalMaxY = effectiveMaxY;
            }
            return (finalRadius, finalMinY, finalMaxY);
        }
    }
}
