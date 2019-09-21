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
        public Map3MapObject(Map3Graphics graphics)
            : base(graphics, () => Config.MapAssociations.GetBestMap().MapImage)
        {
        }

        protected override (PointF loc, SizeF size) GetDimensions()
        {
            RectangleF coordinates = Config.MapAssociations.GetBestMap().Coordinates;

            float xScale = coordinates.Width / (Graphics.XMax - Graphics.XMin);
            float mapWidth = xScale * Graphics.MapView.Width;

            float yScale = coordinates.Height / (Graphics.ZMax - Graphics.ZMin);
            float mapHeight = yScale * Graphics.MapView.Height;

            float xOffsetInGameUnits = (coordinates.X - Graphics.XMin) + coordinates.Width / 2;
            float xOffsetPixels = xOffsetInGameUnits * Graphics.ConversionScale;
            float xCenter = Graphics.MapView.X + xOffsetPixels;

            float yOffsetInGameUnits = (coordinates.Y - Graphics.ZMin) + coordinates.Height / 2;
            float yOffsetPixels = yOffsetInGameUnits * Graphics.ConversionScale;
            float yCenter = Graphics.MapView.Y + yOffsetPixels;

            // Calculate where the map image should be drawn
            return (new PointF(xCenter, yCenter), new SizeF(mapWidth, mapHeight));
        }
    }
}
