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
using STROOP.Interfaces;

namespace STROOP.Controls.Map.Trackers
{
    public partial class MapTracker : UserControl
    {     
        public MapTracker()
        {
            InitializeComponent();
            tableLayoutPanelVarHack.BorderWidth = 2;
            tableLayoutPanelVarHack.ShowBorder = true;

            comboBoxVisibilityType.DataSource = Enum.GetValues(typeof(MapTrackerVisibilityType));
            comboBoxOrderType.DataSource = Enum.GetValues(typeof(MapTrackerOrderType));
        }

        public void UpdateControl()
        {
            
        }
    }
}
