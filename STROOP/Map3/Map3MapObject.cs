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
            int xMin = -8191;
            int xMax = 8192;
            int zMin = -8191;
            int zMax = 8192;

            float scale = Graphics.MapView.Width / (xMax - xMin);

            RectangleF coordinates = Config.MapAssociations.GetBestMap().Coordinates;
            float xScale = coordinates.Width / (xMax - xMin);
            float mapWidth = xScale * Graphics.MapView.Width;
            float yScale = coordinates.Height / (zMax - zMin);
            float mapHeight = yScale * Graphics.MapView.Height;

            float xOffsetInGameUnits = (coordinates.X - xMin) + coordinates.Width / 2;
            float xOffsetPixels = xOffsetInGameUnits * scale;
            float xCenter = Graphics.MapView.X + xOffsetPixels;

            float yOffsetInGameUnits = (coordinates.Y - zMin) + coordinates.Height / 2;
            float yOffsetPixels = yOffsetInGameUnits * scale;
            float yCenter = Graphics.MapView.Y + yOffsetPixels;

            //mapWidth = 50;
            //mapHeight = 50;

            // Calculate where the map image should be drawn
            return (
                new PointF(
                    xCenter,
                    yCenter),
                new SizeF(
                    mapWidth,
                    mapHeight));
        }
    }
}
