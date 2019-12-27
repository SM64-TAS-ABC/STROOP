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
    public abstract class Map3MapObject : Map3IconRectangleObject
    {
        public Map3MapObject()
            : base()
        {
            InternalRotates = true;
        }

        public abstract MapLayout GetMapLayout();

        public override Image GetImage()
        {
            return GetMapLayout().MapImage;
        }

        protected override List<(PointF loc, SizeF size)> GetDimensions()
        {
            RectangleF rectangle = GetMapLayout().Coordinates;
            float rectangleCenterX = rectangle.X + rectangle.Width / 2;
            float rectangleCenterZ = rectangle.Y + rectangle.Height / 2;
            List<(float x, float z)> rectangleCenters = Config.Map3Graphics.MapViewEnablePuView ?
                Map3Utilities.GetPuCoordinates(rectangleCenterX, rectangleCenterZ) :
                new List<(float x, float z)>() { (rectangleCenterX, rectangleCenterZ) };
            List<(float x, float z)> controlCenters = rectangleCenters.ConvertAll(
                rectangleCenter => Map3Utilities.ConvertCoordsForControl(rectangleCenter.x, rectangleCenter.z));
            float sizeX = rectangle.Width * Config.Map3Graphics.MapViewScaleValue;
            float sizeZ = rectangle.Height * Config.Map3Graphics.MapViewScaleValue;
            List<(PointF loc, SizeF size)> dimensions = controlCenters.ConvertAll(
                controlCenter => (new PointF(controlCenter.x, controlCenter.z), new SizeF(sizeX, sizeZ)));
            return dimensions;
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }
    }
}
