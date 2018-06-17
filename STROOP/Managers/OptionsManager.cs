using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System.Windows.Forms;
using STROOP.Utilities;

namespace STROOP.Managers
{
    public class OptionsManager
    {
        private readonly List<Func<bool>> _savedSettingsGetterList;
        private readonly List<Action<bool>> _savedSettingsSetterList;
        private readonly List<string> _savedSettingsTextList;
        private readonly List<ToolStripMenuItem> _savedSettingsItemList;
        private readonly CheckedListBox _savedSettingsCheckedListBox;

        public OptionsManager(TabPage tabControl, Control cogControl)
        {
            _savedSettingsTextList = new List<string>()
            {
                "Display Yaw Angles as Unsigned",
                "Variable Values Flush Right",
                "Start Slot Index From 1",
                "Offset Goto/Retrieve Functions",
                "PU Controller Moves Camera",
                "Scale Diagonal Position Controller Buttons",
                "Exclude Dust for Closest Object",
                "Neutralize Triangles with 21",
                "Use Misalignment Offset For Distance To Line",
                "Don't Round Values to 0",
                "Use In-Game Trig for Angle Logic",
            };

            _savedSettingsGetterList = new List<Func<bool>>()
            {
                () => SavedSettingsConfig.DisplayYawAnglesAsUnsigned,
                () => SavedSettingsConfig.VariableValuesFlushRight,
                () => SavedSettingsConfig.StartSlotIndexsFromOne,
                () => SavedSettingsConfig.OffsetGotoRetrieveFunctions,
                () => SavedSettingsConfig.MoveCameraWithPu,
                () => SavedSettingsConfig.ScaleDiagonalPositionControllerButtons,
                () => SavedSettingsConfig.ExcludeDustForClosestObject,
                () => SavedSettingsConfig.NeutralizeTrianglesWith21,
                () => SavedSettingsConfig.UseMisalignmentOffsetForDistanceToLine,
                () => SavedSettingsConfig.DontRoundValuesToZero,
                () => SavedSettingsConfig.UseInGameTrigForAngleLogic,
            };

            _savedSettingsSetterList = new List<Action<bool>>()
            {
                (bool value) => SavedSettingsConfig.DisplayYawAnglesAsUnsigned = value,
                (bool value) => SavedSettingsConfig.VariableValuesFlushRight = value,
                (bool value) => SavedSettingsConfig.StartSlotIndexsFromOne = value,
                (bool value) => SavedSettingsConfig.OffsetGotoRetrieveFunctions = value,
                (bool value) => SavedSettingsConfig.MoveCameraWithPu = value,
                (bool value) => SavedSettingsConfig.ScaleDiagonalPositionControllerButtons = value,
                (bool value) => SavedSettingsConfig.ExcludeDustForClosestObject = value,
                (bool value) => SavedSettingsConfig.NeutralizeTrianglesWith21 = value,
                (bool value) => SavedSettingsConfig.UseMisalignmentOffsetForDistanceToLine = value,
                (bool value) => SavedSettingsConfig.DontRoundValuesToZero = value,
                (bool value) => SavedSettingsConfig.UseInGameTrigForAngleLogic = value,
            };

            _savedSettingsCheckedListBox = tabControl.Controls["checkedListBoxSavedSettings"] as CheckedListBox;
            for (int i = 0; i < _savedSettingsTextList.Count; i++)
            {
                _savedSettingsCheckedListBox.Items.Add(_savedSettingsTextList[i], _savedSettingsGetterList[i]());
            }
            _savedSettingsCheckedListBox.ItemCheck += (sender, e) =>
            {
                _savedSettingsSetterList[e.Index](e.NewValue == CheckState.Checked);
            };

            Button buttonOptionsResetSavedSettings = tabControl.Controls["buttonOptionsResetSavedSettings"] as Button;
            buttonOptionsResetSavedSettings.Click += (sender, e) => SavedSettingsConfig.ResetSavedSettings();

            _savedSettingsItemList = _savedSettingsTextList.ConvertAll(text => new ToolStripMenuItem(text));
            for (int i = 0; i < _savedSettingsItemList.Count; i++)
            {
                ToolStripMenuItem item = _savedSettingsItemList[i];
                Action<bool> setter = _savedSettingsSetterList[i];
                Func<bool> getter = _savedSettingsGetterList[i];
                item.Click += (sender, e) =>
                {
                    bool newValue = !getter();
                    setter(newValue);
                    item.Checked = newValue;
                };
                item.Checked = getter();
            }

            ToolStripMenuItem resetSavedSettingsItem = new ToolStripMenuItem(buttonOptionsResetSavedSettings.Text);
            resetSavedSettingsItem.Click += (sender, e) => SavedSettingsConfig.ResetSavedSettings();

            ToolStripMenuItem goToOptionsTabItem = new ToolStripMenuItem("Go to Options Tab");
            goToOptionsTabItem.Click += (sender, e) =>
                Config.TabControlMain.SelectedTab = Config.TabControlMain.TabPages["tabPageOptions"];

            cogControl.ContextMenuStrip = new ContextMenuStrip();
            cogControl.Click += (sender, e) => cogControl.ContextMenuStrip.Show(Cursor.Position);

            _savedSettingsItemList.ForEach(item => cogControl.ContextMenuStrip.Items.Add(item));
            cogControl.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            cogControl.ContextMenuStrip.Items.Add(resetSavedSettingsItem);
            cogControl.ContextMenuStrip.Items.Add(goToOptionsTabItem);

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

        public void Update(bool updateView)
        {
            for (int i = 0; i < _savedSettingsCheckedListBox.Items.Count; i++)
            {
                bool value = _savedSettingsGetterList[i]();
                _savedSettingsCheckedListBox.SetItemChecked(i, value);
                _savedSettingsItemList[i].Checked = value;
            }
        }
    }
}
