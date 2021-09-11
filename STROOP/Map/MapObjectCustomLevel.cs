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
    public class MapObjectCustomLevel : MapObjectLevel
    {
        private readonly Dictionary<string, object> _dictionary;
        private object _mapLayoutChoice;

        public MapObjectCustomLevel()
            : base()
        {
            _dictionary = new Dictionary<string, object>();
            _mapLayoutChoice = "Recommended";
        }

        public override MapLayout GetMapLayout()
        {
            return MapUtilities.GetMapLayout(_mapLayoutChoice);
        }

        public override string GetName()
        {
            return "Custom Level";
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                List<MapLayout> mapLayouts = Config.MapAssociations.GetAllMaps();
                List<object> mapLayoutChoices = new List<object>() { "Recommended" };
                mapLayouts.ForEach(mapLayout => mapLayoutChoices.Add(mapLayout));
                mapLayoutChoices.ForEach(mapLayout => _dictionary[mapLayout.ToString()] = mapLayout);

                ToolStripMenuItem itemSelectMap = new ToolStripMenuItem("Select Map");
                itemSelectMap.Click += (sender, e) =>
                {
                    SelectionForm form = new SelectionForm();
                    form.Initialize(
                        "Select a Map",
                        "Set Map",
                        mapLayoutChoices,
                        mapLayoutChoice =>
                        {
                            MapObjectSettings settings = new MapObjectSettings(
                                changeMap: true, newMap: mapLayoutChoice.ToString());
                            GetParentMapTracker().ApplySettings(settings);
                        });
                    form.Show();
                };
                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSelectMap);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeMap)
            {
                _mapLayoutChoice = _dictionary[settings.NewMap];
            }
        }
    }
}
