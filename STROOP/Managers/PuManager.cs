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
        Control _puController;

        enum PuControl { Home, PuUp, PuDown, PuLeft, PuRight, QpuUp, QpuDown, QpuLeft, QpuRight };

        public PuManager(List<WatchVariableControlPrecursor> variables, TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel, Control puController)
            : base(variables, watchVariablePanel)
        {
            _puController = puController;

            // Pu Controller initialize and register click events
            _puController.Controls["buttonPuConHome"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.Home);
            _puController.Controls["buttonPuConZnQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuUp);
            _puController.Controls["buttonPuConZpQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuDown);
            _puController.Controls["buttonPuConXnQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuLeft);
            _puController.Controls["buttonPuConXpQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuRight);
            _puController.Controls["buttonPuConZnPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuUp);
            _puController.Controls["buttonPuConZpPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuDown);
            _puController.Controls["buttonPuConXnPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuLeft);
            _puController.Controls["buttonPuConXpPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuRight);
        }

        private void PuControl_Click(object sender, EventArgs e, PuControl controlType)
        {
            switch (controlType)
            {
                case PuControl.Home:
                    PuUtilities.MoveToPu(0, 0, 0);
                    break;
                case PuControl.PuUp:
                    PuUtilities.MoveToRelativePu(0, 0, -1);
                    break;
                case PuControl.PuDown:
                    PuUtilities.MoveToRelativePu(0, 0, 1);
                    break;
                case PuControl.PuLeft:
                    PuUtilities.MoveToRelativePu(-1, 0, 0);
                    break;
                case PuControl.PuRight:
                    PuUtilities.MoveToRelativePu(1, 0, 0);
                    break;
                case PuControl.QpuUp:
                    PuUtilities.MoveToRelativePu(0, 0, -4);
                    break;
                case PuControl.QpuDown:
                    PuUtilities.MoveToRelativePu(0, 0, 4);
                    break;
                case PuControl.QpuLeft:
                    PuUtilities.MoveToRelativePu(-4, 0, 0);
                    break;
                case PuControl.QpuRight:
                    PuUtilities.MoveToRelativePu(4, 0, 0);
                    break;
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            _puController.Controls["labelPuConPuValue"].Text = PuUtilities.GetPuPosString();
            _puController.Controls["labelPuConQpuValue"].Text = PuUtilities.GetQpuPosString();

            base.Update(updateView);
        }
    }
}
