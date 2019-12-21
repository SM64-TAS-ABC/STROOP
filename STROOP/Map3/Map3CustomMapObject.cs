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
    public class Map3CustomMapObject : Map3IconRectangleObject
    {
        private object _mapLayoutChoice; 

        public Map3CustomMapObject()
            : base()
        {
            InternalRotates = true;
            _mapLayoutChoice = "Recommended";
        }

        public override Image GetImage()
        {
            return Map3Utilities.GetMapLayout(_mapLayoutChoice).MapImage;
        }

        protected override List<(PointF loc, SizeF size)> GetDimensions()
        {
            RectangleF rectangle = Map3Utilities.GetMapLayout(_mapLayoutChoice).Coordinates;
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

        public override string GetName()
        {
            return "Custom Map";
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                List<MapLayout> mapLayouts = Config.MapAssociations.GetAllMaps();
                List<object> mapLayoutChoices = new List<object>() { "Recommended" };
                mapLayouts.ForEach(mapLayout => mapLayoutChoices.Add(mapLayout));

                ToolStripMenuItem itemSelectMap = new ToolStripMenuItem("Select Map");
                itemSelectMap.Click += (sender, e) =>
                {
                    SelectionForm form = new SelectionForm();
                    form.Initialize(
                        "Select a Map",
                        "Set Map",
                        mapLayoutChoices,
                        mapLayoutChoice => _mapLayoutChoice = mapLayoutChoice);
                    form.Show();
                };
                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSelectMap);
            }

            return _contextMenuStrip;
        }
    }
}
