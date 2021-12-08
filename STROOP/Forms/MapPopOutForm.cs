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
            MapGraphics mapGraphics = new MapGraphics(false, Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked);
            mapGraphics.Load(glControlMap2D);
        }
    }
}
