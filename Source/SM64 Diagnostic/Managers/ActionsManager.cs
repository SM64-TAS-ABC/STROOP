using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class ActionsManager : DataManager
    {
        Label actionDescriptionLabel;
        Label animationDescriptionLabel;

        public ActionsManager(List<WatchVariableControlPrecursor> variables, WatchVariablePanel variableTable, Control actionsControl)
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
