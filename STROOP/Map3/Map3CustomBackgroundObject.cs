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
using System.Windows.Forms;
using STROOP.Forms;

namespace STROOP.Map3
{
    public class Map3CustomBackgroundObject : Map3IconRectangleObject
    {
        private object _backgroundChoice;

        public Map3CustomBackgroundObject()
            : base()
        {
            InternalRotates = false;
            _backgroundChoice = "Recommended";
        }

        public override Image GetImage()
        {
            return Map3Utilities.GetBackgroundImage(_backgroundChoice);
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
            return "Custom Background";
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                List<BackgroundImage> backgroundImages = Config.MapAssociations.GetAllBackgroundImages();
                List<object> backgroundImageChoices = new List<object>() { "Recommended" };
                backgroundImages.ForEach(backgroundImage => backgroundImageChoices.Add(backgroundImage));

                ToolStripMenuItem itemSelectMap = new ToolStripMenuItem("Select Background");
                itemSelectMap.Click += (sender, e) =>
                {
                    SelectionForm form = new SelectionForm();
                    form.Initialize(
                        "Select a Background",
                        "Set Background",
                        backgroundImageChoices,
                        backgroundChoice => _backgroundChoice = backgroundChoice);
                    form.Show();
                };
                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSelectMap);
            }

            return _contextMenuStrip;
        }
    }
}
