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
using STROOP.Controls.Map.Objects;

namespace STROOP.Controls.Map.Trackers
{
    public partial class MapTracker : UserControl
    {
        private readonly MapTrackerFlowLayoutPanel _flowLayoutPanel;
        private readonly MapIconObject _iconObj;

        public MapTracker(
            MapTrackerFlowLayoutPanel flowLayoutPanel,
            MapIconObject iconObj)
        {
            _flowLayoutPanel = flowLayoutPanel;
            _iconObj = iconObj;

            InitializeComponent();
        }

        private void MapTracker_Load(object sender, EventArgs e)
        {
            tableLayoutPanel.BorderWidth = 2;
            tableLayoutPanel.ShowBorder = true;

            comboBoxVisibilityType.DataSource = Enum.GetValues(typeof(MapTrackerVisibilityType));
            comboBoxOrderType.DataSource = Enum.GetValues(typeof(MapTrackerOrderType));
        }

        private void trackBarSize_ValueChanged(object sender, EventArgs e)
        {
            const float minSize = 0.01f;
            const float maxSize = 0.20f;
            _iconObj.Size = minSize + (maxSize - minSize) *
                (trackBarSize.Value - trackBarSize.Minimum)
                / (trackBarSize.Maximum - trackBarSize.Minimum); 
        }

        private void trackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            _iconObj.Opacity = (float) (trackBarOpacity.Value - trackBarOpacity.Minimum)
                / (trackBarOpacity.Maximum - trackBarOpacity.Minimum);
        }

        private void checkBoxRotates_CheckedChanged(object sender, EventArgs e)
        {
            _iconObj.Rotates = checkBoxRotates.Checked;
        }

        private void pictureBoxRedX_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxUpArrow_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxDownArrow_Click(object sender, EventArgs e)
        {

        }
    }
}
