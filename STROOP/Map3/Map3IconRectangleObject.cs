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

        public override void DrawOn2DControl()
        {
            List<(PointF loc, SizeF size)> dimensions = GetDimensions();
            float angle = InternalRotates ? Map3Utilities.ConvertAngleForControl(0) : 0; 
            foreach ((PointF loc, SizeF size) in dimensions)
            {
                Map3Utilities.DrawTexture(TextureId, loc, size, angle, Opacity);
            }
        }

        protected abstract List<(PointF loc, SizeF size)> GetDimensions();
    }
}
