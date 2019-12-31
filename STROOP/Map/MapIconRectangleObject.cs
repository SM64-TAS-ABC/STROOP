using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;

namespace STROOP.Map
{
    public abstract class MapIconRectangleObject : MapIconObject
    {
        public MapIconRectangleObject()
            : base()
        {
        }

        public override void DrawOn2DControl()
        {
            List<(PointF loc, SizeF size)> dimensions = GetDimensions();
            float angle = InternalRotates ? MapUtilities.ConvertAngleForControl(0) : 0; 
            foreach ((PointF loc, SizeF size) in dimensions)
            {
                MapUtilities.DrawTexture(TextureId, loc, size, angle, Opacity);
            }
        }

        protected abstract List<(PointF loc, SizeF size)> GetDimensions();
    }
}
