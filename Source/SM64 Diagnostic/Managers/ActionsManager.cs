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
            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();

            // Update action label
            uint action = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.ActionOffset);
            string actionDescription = Config.MarioActions.GetActionName(action);
            actionDescriptionLabel.Text = actionDescription;

            // Update animation label
            uint marioObjRef = Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress);
            short animation = Config.Stream.GetInt16(marioObjRef + Config.Mario.ObjectAnimationOffset);
            string animationDescription = Config.MarioAnimations.GetAnimationName(animation);
            animationDescriptionLabel.Text = animationDescription;
        }
    }
}
