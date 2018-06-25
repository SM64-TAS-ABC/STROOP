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
            List<string> objectSlotOverlayTextList = new List<string>()
            {
                "Held Object",
                "Stood On Object",
                "Interaction Object",
                "Used Object",
                "Closest Object",
                "Camera Object",
                "Camera Hack Object",
                "Floor Object",
                "Wall Object",
                "Ceiling Object",
                "Collision Object",
                "Parent Object",
                "Child Object",
            };

            List<Func<bool>> objectSlotOverlayGetterList = new List<Func<bool>>()
            {
                () => OverlayConfig.ShowOverlayHeldObject,
                () => OverlayConfig.ShowOverlayStoodOnObject,
                () => OverlayConfig.ShowOverlayInteractionObject,
                () => OverlayConfig.ShowOverlayUsedObject,
                () => OverlayConfig.ShowOverlayClosestObject,
                () => OverlayConfig.ShowOverlayCameraObject,
                () => OverlayConfig.ShowOverlayCameraHackObject,
                () => OverlayConfig.ShowOverlayFloorObject,
                () => OverlayConfig.ShowOverlayWallObject,
                () => OverlayConfig.ShowOverlayCeilingObject,
                () => OverlayConfig.ShowOverlayCollisionObject,
                () => OverlayConfig.ShowOverlayParentObject,
                () => OverlayConfig.ShowOverlayChildObject,
            };

            List<Action<bool>> objectSlotOverlaySetterList = new List<Action<bool>>()
            {
                (bool value) => OverlayConfig.ShowOverlayHeldObject = value,
                (bool value) => OverlayConfig.ShowOverlayStoodOnObject = value,
                (bool value) => OverlayConfig.ShowOverlayInteractionObject = value,
                (bool value) => OverlayConfig.ShowOverlayUsedObject = value,
                (bool value) => OverlayConfig.ShowOverlayClosestObject = value,
                (bool value) => OverlayConfig.ShowOverlayCameraObject = value,
                (bool value) => OverlayConfig.ShowOverlayCameraHackObject = value,
                (bool value) => OverlayConfig.ShowOverlayFloorObject = value,
                (bool value) => OverlayConfig.ShowOverlayWallObject = value,
                (bool value) => OverlayConfig.ShowOverlayCeilingObject = value,
                (bool value) => OverlayConfig.ShowOverlayCollisionObject = value,
                (bool value) => OverlayConfig.ShowOverlayParentObject = value,
                (bool value) => OverlayConfig.ShowOverlayChildObject = value,
            };

            CheckedListBox checkedListBoxObjectSlotOverlaysToShow = tabControl.Controls["checkedListBoxObjectSlotOverlaysToShow"] as CheckedListBox;
            for (int i = 0; i < objectSlotOverlayTextList.Count; i++)
            {
                checkedListBoxObjectSlotOverlaysToShow.Items.Add(objectSlotOverlayTextList[i], objectSlotOverlayGetterList[i]());
            }
            checkedListBoxObjectSlotOverlaysToShow.ItemCheck += (sender, e) =>
            {
                objectSlotOverlaySetterList[e.Index](e.NewValue == CheckState.Checked);
            };

            Action<bool> setAllObjectSlotOverlays = (bool value) =>
            {
                int specialCount = 2;
                int totalCount = checkedListBoxObjectSlotOverlaysToShow.Items.Count;
                for (int i = 0; i < totalCount - specialCount; i++)
                {
                    checkedListBoxObjectSlotOverlaysToShow.SetItemChecked(i, value);
                }
            };
            ControlUtilities.AddContextMenuStripFunctions(
                checkedListBoxObjectSlotOverlaysToShow,
                new List<string>() { "Set All On", "Set All Off" },
                new List<Action>()
                {
                    () => setAllObjectSlotOverlays(true),
                    () => setAllObjectSlotOverlays(false),
                });

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
