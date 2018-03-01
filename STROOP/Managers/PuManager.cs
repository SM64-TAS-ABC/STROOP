using STROOP.Controls;
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

        public PuManager(List<WatchVariableControlPrecursor> variables, TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(variables, watchVariablePanel)
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

            _puController.Controls["labelPuConPuValue"].Text = PuUtilities.GetPuPosString(false, false);
            _puController.Controls["labelPuConQpuValue"].Text = PuUtilities.GetPuPosString(true, false);

            base.Update(updateView);
        }
    }
}
