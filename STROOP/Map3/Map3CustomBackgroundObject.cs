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
    public class Map3CustomBackgroundObject : Map3IconRectangleObject
    {
        private object _backgroundChoice;

        public Map3CustomBackgroundObject()
            : base()
        {
            _backgroundChoice = "Recommended";
        }

        public override Image GetImage()
        {
            return Map3Utilities.GetBackgroundImage(_backgroundChoice);
        }

        protected override List<(PointF loc, SizeF size)> GetDimensions()
        {
            float xCenter = Config.Map3Gui.GLControl.Width / 2;
            float yCenter = Config.Map3Gui.GLControl.Height / 2;
            float length = Math.Max(Config.Map3Gui.GLControl.Width, Config.Map3Gui.GLControl.Height);
            (PointF loc, SizeF size) dimension = (new PointF(xCenter, yCenter), new SizeF(length, length));
            return new List<(PointF loc, SizeF size)>() { dimension };
        }

        public override string GetName()
        {
            return "Custom Background";
        }
    }
}
