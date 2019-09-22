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

namespace STROOP.Map3
{
    public abstract class Map3IconRectangleObject : Map3IconObject
    {
        public Map3IconRectangleObject()
            : base()
        {
        }

        public override void DrawOnControl()
        {
            UpdateImage();

            (PointF loc, SizeF size) = GetDimensions();
            int angle = 0;

            DrawTexture(loc, size, angle);
        }

        protected abstract (PointF loc, SizeF size) GetDimensions();
    }
}
