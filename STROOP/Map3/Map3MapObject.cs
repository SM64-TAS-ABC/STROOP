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

        protected override Image GetImage()
        {
            return Map3Utilities.GetMapLayout().MapImage;
        }

        protected override (PointF loc, SizeF size) GetDimensions()
        {
            RectangleF coordinates = Map3Utilities.GetMapLayout().Coordinates;
            float coordinatesCenterX = coordinates.X + coordinates.Width / 2;
            float coordinatesCenterZ = coordinates.Y + coordinates.Height / 2;
            float xOffset = coordinatesCenterX - Config.Map3Graphics.MapViewCenterXValue;
            float zOffset = coordinatesCenterZ - Config.Map3Graphics.MapViewCenterZValue;
            float xOffsetPixels = xOffset * Config.Map3Graphics.MapViewScaleValue;
            float zOffsetPixels = zOffset * Config.Map3Graphics.MapViewScaleValue;
            float centerX = Config.Map3Graphics.Control.Width / 2 + xOffsetPixels;
            float centerZ = Config.Map3Graphics.Control.Height / 2 + zOffsetPixels;
            float sizeX = coordinates.Width * Config.Map3Graphics.MapViewScaleValue;
            float sizeZ = coordinates.Height * Config.Map3Graphics.MapViewScaleValue;
            return (new PointF(centerX, centerZ), new SizeF(sizeX, sizeZ));

            /*
            RectangleF coordinates = Map3Utilities.GetMapLayout().Coordinates;

            float xScale = coordinates.Width / (Config.Map3Graphics.XMax - Config.Map3Graphics.XMin);
            float mapWidth = xScale * Config.Map3Graphics.MapView.Width;

            float yScale = coordinates.Height / (Config.Map3Graphics.ZMax - Config.Map3Graphics.ZMin);
            float mapHeight = yScale * Config.Map3Graphics.MapView.Height;

            float xOffsetInGameUnits = (coordinates.X - Config.Map3Graphics.XMin) + coordinates.Width / 2;
            float xOffsetPixels = xOffsetInGameUnits * Config.Map3Graphics.ConversionScale;
            float xCenter = Config.Map3Graphics.MapView.X + xOffsetPixels;

            float yOffsetInGameUnits = (coordinates.Y - Config.Map3Graphics.ZMin) + coordinates.Height / 2;
            float yOffsetPixels = yOffsetInGameUnits * Config.Map3Graphics.ConversionScale;
            float yCenter = Config.Map3Graphics.MapView.Y + yOffsetPixels;

            // Calculate where the map image should be drawn
            return (new PointF(xCenter, yCenter), new SizeF(mapWidth, mapHeight));
            */
        }
    }
}
