using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class OptionsManager
    {
        public OptionsManager(TabPage tabControl)
        {
            // saved settings
            GroupBox groupBoxOptionsSavedSettings = tabControl.Controls["groupBoxOptionsSavedSettings"] as GroupBox;

            CheckBox checkBoxStartSlotIndexOne = groupBoxOptionsSavedSettings.Controls["checkBoxStartSlotIndexOne"] as CheckBox;
            checkBoxStartSlotIndexOne.Click += (sender, e) => SavedSettingsConfig.StartSlotIndexsFromOne = checkBoxStartSlotIndexOne.Checked;
            checkBoxStartSlotIndexOne.Checked = SavedSettingsConfig.StartSlotIndexsFromOne;

            CheckBox checkBoxMoveCamWithPu = groupBoxOptionsSavedSettings.Controls["checkBoxMoveCamWithPu"] as CheckBox;
            checkBoxMoveCamWithPu.Click += (sender, e) => SavedSettingsConfig.MoveCameraWithPu = checkBoxMoveCamWithPu.Checked;
            checkBoxMoveCamWithPu.Checked = SavedSettingsConfig.MoveCameraWithPu;

            CheckBox checkBoxScaleDiagonalPositionControllerButtons = groupBoxOptionsSavedSettings.Controls["checkBoxScaleDiagonalPositionControllerButtons"] as CheckBox;
            checkBoxScaleDiagonalPositionControllerButtons.Click += (sender, e) => SavedSettingsConfig.ScaleDiagonalPositionControllerButtons = checkBoxScaleDiagonalPositionControllerButtons.Checked;
            checkBoxScaleDiagonalPositionControllerButtons.Checked = SavedSettingsConfig.ScaleDiagonalPositionControllerButtons;

            CheckBox checkBoxExcludeDustForClosestObject = groupBoxOptionsSavedSettings.Controls["checkBoxExcludeDustForClosestObject"] as CheckBox;
            checkBoxExcludeDustForClosestObject.Click += (sender, e) => SavedSettingsConfig.ExcludeDustForClosestObject = checkBoxExcludeDustForClosestObject.Checked;
            checkBoxExcludeDustForClosestObject.Checked = SavedSettingsConfig.ExcludeDustForClosestObject;

            CheckBox checkBoxNeutralizeTrianglesWith21 = groupBoxOptionsSavedSettings.Controls["checkBoxNeutralizeTrianglesWith21"] as CheckBox;
            checkBoxNeutralizeTrianglesWith21.Click += (sender, e) => SavedSettingsConfig.NeutralizeTrianglesWith21 = checkBoxNeutralizeTrianglesWith21.Checked;
            checkBoxNeutralizeTrianglesWith21.Checked = SavedSettingsConfig.NeutralizeTrianglesWith21;

            CheckBox checkBoxUseMisalignmentOffsetForDistanceToLine = groupBoxOptionsSavedSettings.Controls["checkBoxUseMisalignmentOffsetForDistanceToLine"] as CheckBox;
            checkBoxUseMisalignmentOffsetForDistanceToLine.Click += (sender, e) => SavedSettingsConfig.UseMisalignmentOffsetForDistanceToLine = checkBoxUseMisalignmentOffsetForDistanceToLine.Checked;
            checkBoxUseMisalignmentOffsetForDistanceToLine.Checked = SavedSettingsConfig.UseMisalignmentOffsetForDistanceToLine;

            CheckBox checkBoxDontRoundValuesToZero = groupBoxOptionsSavedSettings.Controls["checkBoxDontRoundValuesToZero"] as CheckBox;
            checkBoxDontRoundValuesToZero.Click += (sender, e) => SavedSettingsConfig.DontRoundValuesToZero = checkBoxDontRoundValuesToZero.Checked;
            checkBoxDontRoundValuesToZero.Checked = SavedSettingsConfig.DontRoundValuesToZero;

            CheckBox checkBoxOptionsUseInGameTrigForAngleLogic = groupBoxOptionsSavedSettings.Controls["checkBoxOptionsUseInGameTrigForAngleLogic"] as CheckBox;
            checkBoxOptionsUseInGameTrigForAngleLogic.Click += (sender, e) => SavedSettingsConfig.UseInGameTrigForAngleLogic = checkBoxOptionsUseInGameTrigForAngleLogic.Checked;
            checkBoxOptionsUseInGameTrigForAngleLogic.Checked = SavedSettingsConfig.UseInGameTrigForAngleLogic;


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
                PositionControllerRelativityConfig.Relativity = PositionControllerRelativity.Recommended;
            RadioButton radioButtonPositionControllerRelativeAngleMario =
                groupBoxPositionControllerRelativeAngle.Controls["radioButtonPositionControllerRelativeAngleMario"] as RadioButton;
            radioButtonPositionControllerRelativeAngleMario.Click += (sender, e) =>
                PositionControllerRelativityConfig.Relativity = PositionControllerRelativity.Mario;
            RadioButton radioButtonPositionControllerRelativeAngleCustom =
                groupBoxPositionControllerRelativeAngle.Controls["radioButtonPositionControllerRelativeAngleCustom"] as RadioButton;
            radioButtonPositionControllerRelativeAngleCustom.Click += (sender, e) =>
                PositionControllerRelativityConfig.Relativity = PositionControllerRelativity.Custom;
            BetterTextbox textBoxPositionControllerRelativeAngleCustom =
                groupBoxPositionControllerRelativeAngle.Controls["textBoxPositionControllerRelativeAngleCustom"] as BetterTextbox;
            textBoxPositionControllerRelativeAngleCustom.LostFocus += (sender, e) =>
            {
                double value;
                if (double.TryParse((sender as TextBox).Text, out value))
                {
                    PositionControllerRelativityConfig.CustomAngle = value;
                }
                else
                {
                    (sender as TextBox).Text = PositionControllerRelativityConfig.CustomAngle.ToString();
                }
            };

            // object slot overlays
            GroupBox groupBoxShowOverlay = tabControl.Controls["groupBoxShowOverlay"] as GroupBox;
            List<CheckBox> overlaysCheckboxes = new List<CheckBox>();
            CheckBox checkBoxShowOverlayHeldObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayHeldObject"] as CheckBox;
            checkBoxShowOverlayHeldObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayHeldObject = checkBoxShowOverlayHeldObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayHeldObject);
            CheckBox checkBoxShowOverlayStoodOnObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayStoodOnObject"] as CheckBox;
            checkBoxShowOverlayStoodOnObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayStoodOnObject = checkBoxShowOverlayStoodOnObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayStoodOnObject);
            CheckBox checkBoxShowOverlayInteractionObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayInteractionObject"] as CheckBox;
            checkBoxShowOverlayInteractionObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayInteractionObject = checkBoxShowOverlayInteractionObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayInteractionObject);
            CheckBox checkBoxShowOverlayUsedObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayUsedObject"] as CheckBox;
            checkBoxShowOverlayUsedObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayUsedObject = checkBoxShowOverlayUsedObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayUsedObject);
            CheckBox checkBoxShowOverlayClosestObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayClosestObject"] as CheckBox;
            checkBoxShowOverlayClosestObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayClosestObject = checkBoxShowOverlayClosestObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayClosestObject);
            CheckBox checkBoxShowOverlayCameraObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayCameraObject"] as CheckBox;
            checkBoxShowOverlayCameraObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayCameraObject = checkBoxShowOverlayCameraObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayCameraObject);
            CheckBox checkBoxShowOverlayCameraHackObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayCameraHackObject"] as CheckBox;
            checkBoxShowOverlayCameraHackObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayCameraHackObject = checkBoxShowOverlayCameraHackObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayCameraHackObject);
            CheckBox checkBoxShowOverlayFloorObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayFloorObject"] as CheckBox;
            checkBoxShowOverlayFloorObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayFloorObject = checkBoxShowOverlayFloorObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayFloorObject);
            CheckBox checkBoxShowOverlayWallObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayWallObject"] as CheckBox;
            checkBoxShowOverlayWallObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayWallObject = checkBoxShowOverlayWallObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayWallObject);
            CheckBox checkBoxShowOverlayCeilingObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayCeilingObject"] as CheckBox;
            checkBoxShowOverlayCeilingObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayCeilingObject = checkBoxShowOverlayCeilingObject.Checked;
            overlaysCheckboxes.Add(checkBoxShowOverlayCeilingObject);

            CheckBox checkBoxShowOverlayParentObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayParentObject"] as CheckBox;
            checkBoxShowOverlayParentObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayParentObject = checkBoxShowOverlayParentObject.Checked;
            CheckBox checkBoxShowOverlayChildObject = groupBoxShowOverlay.Controls["checkBoxShowOverlayChildObject"] as CheckBox;
            checkBoxShowOverlayChildObject.CheckedChanged += (sender, e) => OverlayConfig.ShowOverlayChildObject = checkBoxShowOverlayChildObject.Checked;

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
                        RefreshRateConfig.RefreshRateFreq = value;
                    }
                    else
                    {
                        betterTextboxFPS.Text = RefreshRateConfig.RefreshRateFreq.ToString();
                    }
                });
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
