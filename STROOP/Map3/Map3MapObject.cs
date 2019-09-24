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
    public class Map3MapObject : Map3IconRectangleObject
    {
        public Map3MapObject()
            : base()
        {
        }

        public override Image GetImage()
        {
            return Map3Utilities.GetMapLayout().MapImage;
        }

        protected override (PointF loc, SizeF size) GetDimensions()
        {
            RectangleF rectangle = Map3Utilities.GetMapLayout().Coordinates;
            float rectangleCenterX = rectangle.X + rectangle.Width / 2;
            float rectangleCenterZ = rectangle.Y + rectangle.Height / 2;
            (float centerX, float centerZ) = Map3Utilities.ConvertCoordsForControl(rectangleCenterX, rectangleCenterZ);
            float sizeX = rectangle.Width * Config.Map3Graphics.MapViewScaleValue;
            float sizeZ = rectangle.Height * Config.Map3Graphics.MapViewScaleValue;
            return (new PointF(centerX, centerZ), new SizeF(sizeX, sizeZ));
        }

        public override string GetName()
        {
            return "Map";
        }
    }
}
