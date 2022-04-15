using STROOP.Controls;
using STROOP.Managers;
using STROOP.Map;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Forms
{
    public partial class MapPopOutForm : Form, IUpdatableForm
    {
        private MapGraphics _mapGraphics;

        public MapPopOutForm()
        {
            InitializeComponent();
            FormManager.AddForm(this);
            FormClosing += (sender, e) => FormManager.RemoveForm(this);
        }

        public void UpdateForm()
        {
            glControlMap2D.Invalidate();
        }

        public void ShowForm()
        {
            Show();
        }

        private async void Load2D(object sender, EventArgs e)
        {
            _mapGraphics = new MapGraphics(false, Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked);

            _mapGraphics.MapViewScaleValue = Config.MapGraphics.MapViewScaleValue;
            _mapGraphics.MapViewCenterXValue = Config.MapGraphics.MapViewCenterXValue;
            _mapGraphics.MapViewCenterYValue = Config.MapGraphics.MapViewCenterYValue;
            _mapGraphics.MapViewCenterZValue = Config.MapGraphics.MapViewCenterZValue;
            _mapGraphics.MapViewYawValue = Config.MapGraphics.MapViewYawValue;
            _mapGraphics.MapViewPitchValue = Config.MapGraphics.MapViewPitchValue;

            _mapGraphics.Load(glControlMap2D);
        }

        public bool IsOrthographicViewEnabled()
        {
            return _mapGraphics.IsOrthographicViewEnabled;
        }
    }
}
