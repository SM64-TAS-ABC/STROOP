using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;

namespace STROOP.Map3
{
    public abstract class Map3ResizableCircleObject : Map3CircleObject
    {
        private readonly PositionAngle _posAngle;

        public Map3ResizableCircleObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        protected override (float centerX, float centerZ, float radius) GetDimensions()
        {
            return ((float)_posAngle.X, (float)_posAngle.Z, Size);
        }
    }
}
