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
        public readonly double _hSpeed;
        public readonly double _vSpeed;
        public readonly ushort _angle;

        public CoinTrajectory(
            double hSpeed,
            double vSpeed,
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
