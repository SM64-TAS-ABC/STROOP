using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class PuManager : DataManager
    {
        GroupBox _puController;

        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
            };

        public PuManager(List<WatchVariableControlPrecursor> variables, TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(variables, watchVariablePanel, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
            SplitContainer splitContainerFile = tabControl.Controls["splitContainerPu"] as SplitContainer;

            _puController = splitContainerFile.Panel1.Controls["groupBoxPuController"] as GroupBox;

            // Pu Controller initialize and register click events
            _puController.Controls["buttonPuConHome"].Click += (sender, e) => PuUtilities.SetMarioPu(0, 0, 0);
            _puController.Controls["buttonPuConZnQpu"].Click += (sender, e) => PuUtilities.TranslateMarioPu(0, 0, -4);
            _puController.Controls["buttonPuConZpQpu"].Click += (sender, e) => PuUtilities.TranslateMarioPu(0, 0, 4);
            _puController.Controls["buttonPuConXnQpu"].Click += (sender, e) => PuUtilities.TranslateMarioPu(-4, 0, 0);
            _puController.Controls["buttonPuConXpQpu"].Click += (sender, e) => PuUtilities.TranslateMarioPu(4, 0, 0);
            _puController.Controls["buttonPuConZnPu"].Click += (sender, e) => PuUtilities.TranslateMarioPu(0, 0, -1);
            _puController.Controls["buttonPuConZpPu"].Click += (sender, e) => PuUtilities.TranslateMarioPu(0, 0, 1);
            _puController.Controls["buttonPuConXnPu"].Click += (sender, e) => PuUtilities.TranslateMarioPu(-1, 0, 0);
            _puController.Controls["buttonPuConXpPu"].Click += (sender, e) => PuUtilities.TranslateMarioPu(1, 0, 0);
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            _puController.Controls["labelPuConPuValue"].Text = PuUtilities.GetPuIndexString(false, false);
            _puController.Controls["labelPuConQpuValue"].Text = PuUtilities.GetPuIndexString(true, false);

            base.Update(updateView);
        }
    }
}
