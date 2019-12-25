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
    public abstract class Map3SphereObject : Map3CircleObject
    {
        public Map3SphereObject()
            : base()
        {
        }

        protected override (float centerX, float centerZ, float radius) Get2DDimensions()
        {
            (float centerX, float centerY, float centerZ, float radius3D) = Get3DDimensions();
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float yDiff = marioY - centerY;
            float radiusSquared = radius3D * radius3D - yDiff * yDiff;
            float radius2D = radiusSquared >= 0 ? (float)Math.Sqrt(radiusSquared) : 0;
            return (centerX, centerZ, radius2D);
        }

        protected abstract (float centerX, float centerY, float centerZ, float radius3D) Get3DDimensions();
    }
}
