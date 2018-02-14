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

namespace STROOP.Managers
{
    public class ActionsManager : DataManager
    {
        Label actionDescriptionLabel;
        Label animationDescriptionLabel;

        public ActionsManager(List<WatchVariableControlPrecursor> variables, WatchVariableFlowLayoutPanel variableTable, Control actionsControl)
            : base(variables, variableTable)
        {
            actionDescriptionLabel = actionsControl.Controls["labelActionDescription"] as Label;
            animationDescriptionLabel = actionsControl.Controls["labelAnimationDescription"] as Label;
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);

            actionDescriptionLabel.Text = TableConfig.MarioActions.GetActionName();
            animationDescriptionLabel.Text = TableConfig.MarioAnimations.GetAnimationName();
        }
    }
}
