using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public class CoinTrajectory
    {
        public readonly float _hSpeed;
        public readonly float _vSpeed;
        public readonly float _angle;

        public CoinTrajectory(
            float hSpeed,
            float vSpeed,
            ushort angle)
        {
            _hSpeed = hSpeed;
            _vSpeed = vSpeed;
            _angle = angle;
        }

        public override string ToString()
        {
            return String.Format(
                "HSpeed:{0}, VSpeed:{1}, Angle:{2}",
                _hSpeed, _vSpeed, _angle);
        }

    }
}
