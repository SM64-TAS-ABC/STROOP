﻿using System;
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
        MapIconObject _obj;

        public MapTracker(MapIconObject obj)
        {
            _obj = obj;

            InitializeComponent();
        }

        private void MapTracker_Load(object sender, EventArgs e)
        {
            tableLayoutPanel.BorderWidth = 2;
            tableLayoutPanel.ShowBorder = true;

            comboBoxVisibilityType.DataSource = Enum.GetValues(typeof(MapTrackerVisibilityType));
            comboBoxOrderType.DataSource = Enum.GetValues(typeof(MapTrackerOrderType));
        }

        private void TrackBarSize_ValueChanged(object sender, EventArgs e)
        {
            const float minSize = 0.01f;
            const float maxSize = 0.20f;
            _obj.Size = minSize + (maxSize - minSize) *
                (trackBarSize.Value - trackBarSize.Minimum)
                / (trackBarSize.Maximum - trackBarSize.Minimum); 
        }

        private void TrackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            _obj.Opacity = (float) (trackBarOpacity.Value - trackBarOpacity.Minimum)
                / (trackBarOpacity.Maximum - trackBarOpacity.Minimum);
        }

        private void CheckBoxRotates_CheckedChanged(object sender, EventArgs e)
        {
            _obj.Rotates = checkBoxRotates.Checked;
        }
    }
}
