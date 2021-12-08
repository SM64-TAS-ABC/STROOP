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
            _mapGraphics.Load(glControlMap2D);
        }

        public bool IsOrthographicViewEnabled()
        {
            return _mapGraphics.IsOrthographicViewEnabled;
        }
    }
}
