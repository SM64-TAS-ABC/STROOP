using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Structs;
using STROOP.Utilities;
using System.Xml.Linq;
using STROOP.Structs.Configurations;
using System.Drawing.Drawing2D;

namespace STROOP.Controls
{
    public partial class MapTracker : UserControl
    {
        private readonly MapTrackerFlowLayoutPanel _flowLayoutPanel;
        
        public MapTracker(MapTrackerFlowLayoutPanel flowLayoutPanel)
        {
            _flowLayoutPanel = flowLayoutPanel;
        }

        public void UpdateControl()
        {
            
        }
    }
}
