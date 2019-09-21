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
    public class Map3BackgroundObject : Map3IconRectangleObject
    {
        public Map3BackgroundObject(Map3Graphics graphics)
            : base(graphics, () => Config.MapAssociations.GetBestMap().BackgroundImage)
        {
        }

        protected override (PointF loc, SizeF size) GetDimensions()
        {
            float xCenter = Graphics.MapView.X + Graphics.MapView.Width / 2;
            float yCenter = Graphics.MapView.Y + Graphics.MapView.Height / 2;
            float length = Math.Max(Graphics.Control.Width, Graphics.Control.Height);
            return (new PointF(xCenter, yCenter), new SizeF(length, length));
        }
    }
}
