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
    public abstract class Map3CylinderObject : Map3CircleObject
    {
        public Map3CylinderObject()
            : base()
        {
        }

        protected override (float centerX, float centerZ, float radius) Get2DDimensions()
        {
            (float centerX, float centerZ, float radius, float minY, float maxY) = Get3DDimensions();
            return (centerX, centerZ, radius);
        }

        protected abstract (float centerX, float centerZ, float radius, float minY, float maxY) Get3DDimensions();
    }
}
