using STROOP.Structs.Configurations;
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

        public int GetRadius(uint objAddress)
        {
            return 0;
        }
    }
}
