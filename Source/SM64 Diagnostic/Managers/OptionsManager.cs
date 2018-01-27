using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using System.Windows.Forms;
using static SM64_Diagnostic.Structs.Configurations.Config;
using static SM64_Diagnostic.Structs.Configurations.PositionControllerRelativeAngleConfig;

namespace SM64_Diagnostic.Managers
{
    public class OptionsManager
    {
        public OptionsManager(TabPage tabControl)
        {
            // rom version
            GroupBox groupBoxRomVersion = tabControl.Controls["groupBoxRomVersion"] as GroupBox;
            RadioButton radioButtonRomVersionUS = groupBoxRomVersion.Controls["radioButtonRomVersionUS"] as RadioButton;
            radioButtonRomVersionUS.Checked = Config.Version == Config.RomVersion.US;
            radioButtonRomVersionUS.Click += (sender, e) => { Config.Version = RomVersion.US; };
            RadioButton radioButtonRomVersionJP = groupBoxRomVersion.Controls["radioButtonRomVersionJP"] as RadioButton;
            radioButtonRomVersionJP.Checked = Config.Version == Config.RomVersion.JP;
            radioButtonRomVersionJP.Click += (sender, e) => { Config.Version = RomVersion.JP; };
            RadioButton radioButtonRomVersionPAL = groupBoxRomVersion.Controls["radioButtonRomVersionPAL"] as RadioButton;
            radioButtonRomVersionPAL.Checked = Config.Version == Config.RomVersion.PAL;
            radioButtonRomVersionPAL.Click += (sender, e) => { Config.Version = RomVersion.PAL; };

            // goto/retrieve offsets
            GroupBox groupBoxGotoRetrieveOffsets = tabControl.Controls["groupBoxGotoRetrieveOffsets"] as GroupBox;
            BetterTextbox textBoxGotoAbove = groupBoxGotoRetrieveOffsets.Controls["textBoxGotoAbove"] as BetterTextbox;
            textBoxGotoAbove.LostFocus += (sender, e) => textBoxGotoRetrieve_LostFocus(
                sender, ref GotoRetrieveConfig.GotoAboveOffset, GotoRetrieveConfig.GotoAboveDefault);
            BetterTextbox textBoxGotoInfront = groupBoxGotoRetrieveOffsets.Controls["textBoxGotoInfront"] as BetterTextbox;
            textBoxGotoInfront.LostFocus += (sender, e) => textBoxGotoRetrieve_LostFocus(
                sender, ref GotoRetrieveConfig.GotoInfrontOffset, GotoRetrieveConfig.GotoInfrontDefault);
            BetterTextbox textBoxRetrieveAbove = groupBoxGotoRetrieveOffsets.Controls["textBoxRetrieveAbove"] as BetterTextbox;
            textBoxRetrieveAbove.LostFocus += (sender, e) => textBoxGotoRetrieve_LostFocus(
                sender, ref GotoRetrieveConfig.RetrieveAboveOffset, GotoRetrieveConfig.RetrieveAboveDefault);
            BetterTextbox textBoxRetrieveInfront = groupBoxGotoRetrieveOffsets.Controls["textBoxRetrieveInfront"] as BetterTextbox;
            textBoxRetrieveInfront.LostFocus += (sender, e) => textBoxGotoRetrieve_LostFocus(
                sender, ref GotoRetrieveConfig.RetrieveInfrontOffset, GotoRetrieveConfig.RetrieveInfrontDefault);

            // position controller relative angle
            GroupBox groupBoxPositionControllerRelativeAngle = tabControl.Controls["groupBoxPositionControllerRelativeAngle"] as GroupBox;
            RadioButton radioButtonPositionControllerRelativeAngleRecommended =
                groupBoxPositionControllerRelativeAngle.Controls["radioButtonPositionControllerRelativeAngleRecommended"] as RadioButton;
            radioButtonPositionControllerRelativeAngleRecommended.Click += (sender, e) =>
                Config.PositionControllerRelativeAngle.Relativity = RelativityType.Recommended;
            RadioButton radioButtonPositionControllerRelativeAngleMario =
                groupBoxPositionControllerRelativeAngle.Controls["radioButtonPositionControllerRelativeAngleMario"] as RadioButton;
            radioButtonPositionControllerRelativeAngleMario.Click += (sender, e) =>
                Config.PositionControllerRelativeAngle.Relativity = RelativityType.Mario;
            RadioButton radioButtonPositionControllerRelativeAngleCustom =
                groupBoxPositionControllerRelativeAngle.Controls["radioButtonPositionControllerRelativeAngleCustom"] as RadioButton;
            radioButtonPositionControllerRelativeAngleCustom.Click += (sender, e) =>
                Config.PositionControllerRelativeAngle.Relativity = RelativityType.Custom;
            BetterTextbox textBoxPositionControllerRelativeAngleCustom =
                groupBoxPositionControllerRelativeAngle.Controls["textBoxPositionControllerRelativeAngleCustom"] as BetterTextbox;
            textBoxPositionControllerRelativeAngleCustom.LostFocus += (sender, e) =>
            {
                double value;
                if (double.TryParse((sender as TextBox).Text, out value))
                {
                    Config.PositionControllerRelativeAngle.CustomAngle = value;
                }
                else
                {
                    (sender as TextBox).Text = Config.PositionControllerRelativeAngle.CustomAngle.ToString();
                }
            };

            // object slot overlays
            GroupBox groupBoxShowOverlay = tabControl.Controls["groupBoxShowOverlay"] as GroupBox;
            List<CheckBox> overlaysCheckboxes = new List<CheckBox>();
            CheckBox checkBoxShowOverlayHeldObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayHeldObject"] as CheckBox;
            checkBoxShowOverlayHeldObject.CheckedChanged += (sender, e) => Config.ShowOverlayHeldObject = checkBoxShowOverlayHeldObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayHeldObject);
            CheckBox checkBoxShowOverlayStoodOnObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayStoodOnObject"] as CheckBox;
            checkBoxShowOverlayStoodOnObject.CheckedChanged += (sender, e) => Config.ShowOverlayStoodOnObject = checkBoxShowOverlayStoodOnObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayStoodOnObject);
            CheckBox checkBoxShowOverlayInteractionObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayInteractionObject"] as CheckBox;
            checkBoxShowOverlayInteractionObject.CheckedChanged += (sender, e) => Config.ShowOverlayInteractionObject = checkBoxShowOverlayInteractionObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayInteractionObject);
            CheckBox checkBoxShowOverlayUsedObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayUsedObject"] as CheckBox;
            checkBoxShowOverlayUsedObject.CheckedChanged += (sender, e) => Config.ShowOverlayUsedObject = checkBoxShowOverlayUsedObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayUsedObject);
            CheckBox checkBoxShowOverlayClosestObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayClosestObject"] as CheckBox;
            checkBoxShowOverlayClosestObject.CheckedChanged += (sender, e) => Config.ShowOverlayClosestObject = checkBoxShowOverlayClosestObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayClosestObject);
            CheckBox checkBoxShowOverlayCameraObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayCameraObject"] as CheckBox;
            checkBoxShowOverlayCameraObject.CheckedChanged += (sender, e) => Config.ShowOverlayCameraObject = checkBoxShowOverlayCameraObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayCameraObject);
            CheckBox checkBoxShowOverlayCameraHackObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayCameraHackObject"] as CheckBox;
            checkBoxShowOverlayCameraHackObject.CheckedChanged += (sender, e) => Config.ShowOverlayCameraHackObject = checkBoxShowOverlayCameraHackObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayCameraHackObject);
            CheckBox checkBoxShowOverlayFloorObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayFloorObject"] as CheckBox;
            checkBoxShowOverlayFloorObject.CheckedChanged += (sender, e) => Config.ShowOverlayFloorObject = checkBoxShowOverlayFloorObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayFloorObject);
            CheckBox checkBoxShowOverlayWallObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayWallObject"] as CheckBox;
            checkBoxShowOverlayWallObject.CheckedChanged += (sender, e) => Config.ShowOverlayWallObject = checkBoxShowOverlayWallObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayWallObject);
            CheckBox checkBoxShowOverlayCeilingObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayCeilingObject"] as CheckBox;
            checkBoxShowOverlayCeilingObject.CheckedChanged += (sender, e) => Config.ShowOverlayCeilingObject = checkBoxShowOverlayCeilingObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayCeilingObject);

            CheckBox checkBoxShowOverlayParentObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayParentObject"] as CheckBox;
            checkBoxShowOverlayParentObject.CheckedChanged += (sender, e) => Config.ShowOverlayParentObject = checkBoxShowOverlayParentObject.Checked;

            groupBoxShowOverlay.Click += (sender, e) =>
            {
                bool newChecked = !overlaysCheckboxes.All(checkbox => checkbox.Checked);
                overlaysCheckboxes.ForEach(checkbox => checkbox.Checked = newChecked);
            };

            // FPS
            GroupBox groupBoxFPS = tabControl.Controls["groupBoxFPS"] as GroupBox;
            BetterTextbox betterTextboxFPS = groupBoxFPS.Controls["betterTextboxFPS"] as BetterTextbox;
            betterTextboxFPS.AddLostFocusAction(
                () =>
                {
                    uint value;
                    if (uint.TryParse(betterTextboxFPS.Text, out value))
                    {
                        Config.RefreshRateFreq = value;
                    }
                    else
                    {
                        betterTextboxFPS.Text = Config.RefreshRateFreq.ToString();
                    }
                });

            // miscellaneous
            CheckBox checkBoxStartSlotIndexOne = tabControl.Controls["checkBoxStartSlotIndexOne"] as CheckBox;
            checkBoxStartSlotIndexOne.Click += (sender, e) => Config.SlotIndexsFromOne = checkBoxStartSlotIndexOne.Checked;
            CheckBox checkBoxMoveCamWithPu = tabControl.Controls["checkBoxMoveCamWithPu"] as CheckBox;
            checkBoxMoveCamWithPu.Click += (sender, e) => Config.MoveCameraWithPu = checkBoxMoveCamWithPu.Checked;
            CheckBox checkBoxScaleDiagonalPositionControllerButtons = tabControl.Controls["checkBoxScaleDiagonalPositionControllerButtons"] as CheckBox;
            checkBoxScaleDiagonalPositionControllerButtons.Click += (sender, e) => Config.ScaleDiagonalPositionControllerButtons = checkBoxScaleDiagonalPositionControllerButtons.Checked;
            CheckBox checkBoxExcludeDustForClosestObject = tabControl.Controls["checkBoxExcludeDustForClosestObject"] as CheckBox;
            checkBoxExcludeDustForClosestObject.Click += (sender, e) => Config.ExcludeDustForClosestObject = checkBoxExcludeDustForClosestObject.Checked;
            CheckBox checkBoxNeutralizeTrianglesWith21 = tabControl.Controls["checkBoxNeutralizeTrianglesWith21"] as CheckBox;
            checkBoxNeutralizeTrianglesWith21.Click += (sender, e) => Config.NeutralizeTrianglesWith21 = checkBoxNeutralizeTrianglesWith21.Checked;
            CheckBox checkBoxUseMisalignmentOffsetForDistanceToLine = tabControl.Controls["checkBoxUseMisalignmentOffsetForDistanceToLine"] as CheckBox;
            checkBoxUseMisalignmentOffsetForDistanceToLine.Click += (sender, e) => Config.UseMisalignmentOffsetForDistanceToLine = checkBoxUseMisalignmentOffsetForDistanceToLine.Checked;
        }

        private void textBoxGotoRetrieve_LostFocus(object sender, ref float offset, float defaultOffset)
        {
            float value;
            if (float.TryParse((sender as TextBox).Text, out value))
            {
                offset = value;
            }
            else
            {
                offset = defaultOffset;
                (sender as TextBox).Text = defaultOffset.ToString();
            }
        }
    }
}
