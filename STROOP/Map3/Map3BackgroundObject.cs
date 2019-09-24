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
        }

        public override Image GetImage()
        {
            object backgroundChoice = Config.Map3Gui.comboBoxMap3OptionsBackground.SelectedItem;
            if (backgroundChoice is BackgroundImage background)
            {
                return background.Image;
            }
            else
            {
                return Config.MapAssociations.GetBestMap().BackgroundImage;
            }
        }

        protected override (PointF loc, SizeF size) GetDimensions()
        {
            float xCenter = Config.Map3Gui.GLControl.Width / 2;
            float yCenter = Config.Map3Gui.GLControl.Height / 2;
            float length = Math.Max(Config.Map3Gui.GLControl.Width, Config.Map3Gui.GLControl.Height);
            return (new PointF(xCenter, yCenter), new SizeF(length, length));
        }

        public override string GetName()
        {
            return "Background";
        }
    }
}
