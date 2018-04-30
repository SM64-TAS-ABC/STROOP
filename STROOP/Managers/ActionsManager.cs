using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using STROOP.Forms;

namespace STROOP.Managers
{
    public class ActionsManager : DataManager
    {
        BetterTextbox textBoxActionDescription;
        BetterTextbox textBoxAnimationDescription;

        public ActionsManager(List<WatchVariableControlPrecursor> variables, WatchVariableFlowLayoutPanel variableTable, Control actionsControl)
            : base(variables, variableTable)
        {
            textBoxActionDescription = actionsControl.Controls["textBoxActionDescription"] as BetterTextbox;
            textBoxAnimationDescription = actionsControl.Controls["textBoxAnimationDescription"] as BetterTextbox;

            textBoxActionDescription.DoubleClick += (sender, e) => SelectionForm.ShowActionSelectionForm();
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);

            textBoxActionDescription.Text = TableConfig.MarioActions.GetActionName();
            textBoxAnimationDescription.Text = TableConfig.MarioAnimations.GetAnimationName();
        }
    }
}
