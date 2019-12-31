using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using STROOP.Forms;

namespace STROOP.Map
{
    public class MapCustomMapObject : MapMapObject
    {
        private object _mapLayoutChoice; 

        public MapCustomMapObject()
            : base()
        {
            _mapLayoutChoice = "Recommended";
        }

        public override MapLayout GetMapLayout()
        {
            return MapUtilities.GetMapLayout(_mapLayoutChoice);
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
