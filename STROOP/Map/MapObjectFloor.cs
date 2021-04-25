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
    public abstract class MapObjectFloor : MapObjectHorizontalTriangle
    {
        private ToolStripMenuItem _itemExcludeDeathBarriers;
        private ToolStripMenuItem _itemEnableQuarterFrameLandings;

        public MapObjectFloor()
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
                    changeFloorExcludeDeathBarriers: true, newFloorExcludeDeathBarriers: !_excludeDeathBarriers);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemEnableQuarterFrameLandings = new ToolStripMenuItem("Enable Quarter Frame Landings");
            _itemEnableQuarterFrameLandings.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeFloorEnableQuarterFrameLandings: true, newFloorEnableQuarterFrameLandings: !_enableQuarterFrameLandings);
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

            if (settings.ChangeFloorExcludeDeathBarriers)
            {
                _excludeDeathBarriers = settings.NewFloorExcludeDeathBarriers;
                _itemExcludeDeathBarriers.Checked = settings.NewFloorExcludeDeathBarriers;
            }

            if (settings.ChangeFloorEnableQuarterFrameLandings)
            {
                _enableQuarterFrameLandings = settings.NewFloorEnableQuarterFrameLandings;
                _itemEnableQuarterFrameLandings.Checked = settings.NewFloorEnableQuarterFrameLandings;
            }
        }
    }
}
