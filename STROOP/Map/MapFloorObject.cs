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
        public MapFloorObject()
            : base()
        {
            Size = 78;
            Opacity = 0.5;
            Color = Color.Blue;
        }

        protected List<ToolStripMenuItem> GetFloorToolStripMenuItems()
        {
            ToolStripMenuItem itemExcludeDeathBarriers = new ToolStripMenuItem("Exclude Death Barriers");
            itemExcludeDeathBarriers.Click += (sender, e) =>
            {
                _excludeDeathBarriers = !_excludeDeathBarriers;
                itemExcludeDeathBarriers.Checked = _excludeDeathBarriers;
            };

            ToolStripMenuItem itemEnableQuarterFrameLandings = new ToolStripMenuItem("Enable Quarter Frame Landings");
            itemEnableQuarterFrameLandings.Click += (sender, e) =>
            {
                _enableQuarterFrameLandings = !_enableQuarterFrameLandings;
                itemEnableQuarterFrameLandings.Checked = _enableQuarterFrameLandings;
            };

            return new List<ToolStripMenuItem>()
            {
                itemExcludeDeathBarriers,
                itemEnableQuarterFrameLandings,
            };
        }
    }
}
