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
        public Map3IconRectangleObject(Map3Graphics graphics, Func<Image> imageFunction)
            : base(graphics, imageFunction)
        {
        }

        public override void DrawOnControl()
        {
            UpdateImage();

            (PointF loc, SizeF size) = GetDimensions();
            int angle = 0;
            int alpha = 1;

            DrawTexture(loc, size, angle, alpha);
        }

        protected abstract (PointF loc, SizeF size) GetDimensions();
    }
}
