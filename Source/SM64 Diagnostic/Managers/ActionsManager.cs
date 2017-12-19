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

        public ActionsManager(List<WatchVariable> actionsData, NoTearFlowLayoutPanel variableTable, Control actionsControl)
            : base(actionsData, variableTable)
        {
            actionDescriptionLabel = actionsControl.Controls["labelActionDescription"] as Label;
            animationDescriptionLabel = actionsControl.Controls["labelAnimationDescription"] as Label;
        }

        protected override List<SpecialWatchVariable> _specialWatchVars { get; } = new List<SpecialWatchVariable>()
        {
             new SpecialWatchVariable("ActionDescription"),
             new SpecialWatchVariable("PrevActionDescription"),
             new SpecialWatchVariable("MarioAnimationDescription"),
        };

        public void ProcessSpecialVars()
        {
            var marioObjRef = Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress);
            short marioObjAnimation = Config.Stream.GetInt16(marioObjRef + Config.Mario.ObjectAnimationOffset);
            short marioObjAnimationTimer = Config.Stream.GetInt16(marioObjRef + Config.Mario.ObjectAnimationTimerOffset);

            foreach (var specialVar in _specialDataControls)
            {
                switch(specialVar.SpecialName)
                {                  
                    case "ActionDescription":
                        string actionDescription = Config.MarioActions.GetActionName(Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.ActionOffset));
                        (specialVar as DataContainer).Text = actionDescription;
                        actionDescriptionLabel.Text = actionDescription;
                        break;

                    case "PrevActionDescription":
                        (specialVar as DataContainer).Text = Config.MarioActions.GetActionName(Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.PrevActionOffset));
                        break;

                    case "MarioAnimationDescription":
                        string animationDescription = Config.MarioAnimations.GetAnimationName(marioObjAnimation);
                        (specialVar as DataContainer).Text = animationDescription;
                        animationDescriptionLabel.Text = animationDescription;
                        break;
                }
            }
        }

        public override void Update(bool updateView)
        {
            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();
            ProcessSpecialVars();
        }
    }
}
