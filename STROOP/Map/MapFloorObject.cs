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
using System.Drawing.Imaging;
using STROOP.Models;
using System.Windows.Forms;

namespace STROOP.Map
{
    public abstract class MapFloorObject : MapHorizontalTriangleObject
    {
        private ToolStripMenuItem _itemExcludeDeathBarriers;
        private ToolStripMenuItem _itemEnableQuarterFrameLandings;

        public MapFloorObject()
            : base()
        {
            Size = 78;
            Opacity = 0.5;
            Color = Color.Blue;
        }

        protected List<ToolStripMenuItem> GetFloorToolStripMenuItems()
        {
            _itemExcludeDeathBarriers = new ToolStripMenuItem("Exclude Death Barriers");
            _itemExcludeDeathBarriers.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    floorChangeExcludeDeathBarriers: true, floorNewExcludeDeathBarriers: !_excludeDeathBarriers);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemEnableQuarterFrameLandings = new ToolStripMenuItem("Enable Quarter Frame Landings");
            _itemEnableQuarterFrameLandings.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    floorChangeEnableQuarterFrameLandings: true, floorNewEnableQuarterFrameLandings: !_enableQuarterFrameLandings);
                GetParentMapTracker().ApplySettings(settings);
            };

            return new List<ToolStripMenuItem>()
            {
                _itemExcludeDeathBarriers,
                _itemEnableQuarterFrameLandings,
            };
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.FloorChangeExcludeDeathBarriers)
            {
                _excludeDeathBarriers = settings.FloorNewExcludeDeathBarriers;
                _itemExcludeDeathBarriers.Checked = settings.FloorNewExcludeDeathBarriers;
            }

            if (settings.FloorChangeEnableQuarterFrameLandings)
            {
                _enableQuarterFrameLandings = settings.FloorNewEnableQuarterFrameLandings;
                _itemEnableQuarterFrameLandings.Checked = settings.FloorNewEnableQuarterFrameLandings;
            }
        }
    }
}
