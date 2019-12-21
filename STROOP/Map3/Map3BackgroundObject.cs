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
        public Map3BackgroundObject()
            : base()
        {
            InternalRotates = false;
        }

        public override Image GetImage()
        {
            return Map3Utilities.GetBackgroundImage();
        }

        protected override List<(PointF loc, SizeF size)> GetDimensions()
        {
            float xCenter = Config.Map3Gui.GLControl2D.Width / 2;
            float yCenter = Config.Map3Gui.GLControl2D.Height / 2;
            float length = Math.Max(Config.Map3Gui.GLControl2D.Width, Config.Map3Gui.GLControl2D.Height);
            (PointF loc, SizeF size) dimension = (new PointF(xCenter, yCenter), new SizeF(length, length));
            return new List<(PointF loc, SizeF size)>() { dimension };
        }

        public override string GetName()
        {
            return "Background";
        }
    }
}
