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
        public readonly List<MapIconObject> MapObjectList;

        private bool _visible;
        private static readonly Image ImageEyeOpen = Properties.Resources.image_eye_open2;
        private static readonly Image ImageEyeClosed = Properties.Resources.image_eye_closed2;

        public MapTracker(
            MapTrackerFlowLayoutPanel flowLayoutPanel,
            List<MapIconObject> mapObjectList)
        {
            _flowLayoutPanel = flowLayoutPanel;
            MapObjectList = new List<MapIconObject>(mapObjectList);

            _visible = true;

            InitializeComponent();
        }

        private void MapTracker_Load(object sender, EventArgs e)
        {
            tableLayoutPanel.BorderWidth = 2;
            tableLayoutPanel.ShowBorder = true;

            comboBoxVisibilityType.DataSource = Enum.GetValues(typeof(MapTrackerVisibilityType));
            comboBoxOrderType.DataSource = Enum.GetValues(typeof(MapTrackerOrderType));
        }

        public MapTrackerOrderType GetOrderType()
        {
            return (MapTrackerOrderType) comboBoxOrderType.SelectedItem;
        }

        private void trackBarSize_ValueChanged(object sender, EventArgs e)
        {
            const float minSize = 0.01f;
            const float maxSize = 0.20f;
            MapObjectList.ForEach(icon =>
            {
                icon.Size = minSize + (maxSize - minSize) *
                    (trackBarSize.Value - trackBarSize.Minimum)
                    / (trackBarSize.Maximum - trackBarSize.Minimum);
            });
        }

        private void trackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            MapObjectList.ForEach(icon =>
            {
                icon.Opacity = (float)(trackBarOpacity.Value - trackBarOpacity.Minimum)
                    / (trackBarOpacity.Maximum - trackBarOpacity.Minimum);
            });
        }

        private void checkBoxRotates_CheckedChanged(object sender, EventArgs e)
        {
            MapObjectList.ForEach(icon =>
            {
                icon.Rotates = checkBoxRotates.Checked;
            });
        }

        private void pictureBoxRedX_Click(object sender, EventArgs e)
        {
            _flowLayoutPanel.RemoveControl(this);
        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {
            _visible = !_visible;
            pictureBoxEye.BackgroundImage = _visible ? ImageEyeOpen : ImageEyeClosed;
        }

        private void pictureBoxUpArrow_Click(object sender, EventArgs e)
        {
            _flowLayoutPanel.MoveUpControl(this);
        }

        private void pictureBoxDownArrow_Click(object sender, EventArgs e)
        {
            _flowLayoutPanel.MoveDownControl(this);
        }
    }
}
