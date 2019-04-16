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

        public ActionsManager(string varFilePath, WatchVariableFlowLayoutPanel variableTable, Control actionsControl)
            : base(varFilePath, variableTable)
        {
            textBoxActionDescription = actionsControl.Controls["textBoxActionDescription"] as BetterTextbox;
            textBoxAnimationDescription = actionsControl.Controls["textBoxAnimationDescription"] as BetterTextbox;

            textBoxActionDescription.DoubleClick += (sender, e) => SelectionForm.ShowActionDescriptionSelectionForm();
            textBoxAnimationDescription.DoubleClick += (sender, e) => SelectionForm.ShowAnimationDescriptionSelectionForm();

            ControlUtilities.AddContextMenuStripFunctions(
                textBoxActionDescription,
                new List<string>() { "Select Action", "Free Movement Action", "Open Action Form" },
                new List<Action>()
                {
                    () => SelectionForm.ShowActionDescriptionSelectionForm(),
                    () => Config.Stream.SetValue(MarioConfig.FreeMovementAction, MarioConfig.StructAddress + MarioConfig.ActionOffset),
                    () => new ActionForm().Show(),
                });

            ControlUtilities.AddContextMenuStripFunctions(
                textBoxAnimationDescription,
                new List<string>() { "Select Animation", "Replace Animation" },
                new List<Action>()
                {
                    () => SelectionForm.ShowAnimationDescriptionSelectionForm(),
                    () =>
                    {
                        int? animationToBeReplaced = SelectionForm.GetAnimation("Choose Animation to Be Replaced", "Select Animation");
                        int? animationToReplaceIt = SelectionForm.GetAnimation("Choose Animation to Replace It", "Select Animation");
                        if (animationToBeReplaced == null || animationToReplaceIt == null) return;
                        AnimationUtilities.ReplaceAnimation(animationToBeReplaced.Value, animationToReplaceIt.Value);
                    },
                });
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
