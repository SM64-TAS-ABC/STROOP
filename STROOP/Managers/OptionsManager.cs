using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;

namespace STROOP.Managers
{
    public class OptionsManager : DataManager
    {
        private readonly List<Func<bool>> _savedSettingsGetterList;
        private readonly List<Action<bool>> _savedSettingsSetterList;
        private readonly List<string> _savedSettingsTextList;
        private readonly List<ToolStripMenuItem> _savedSettingsItemList;
        private readonly CheckedListBox _savedSettingsCheckedListBox;

        public OptionsManager(string varFilePath, TabPage tabControl, WatchVariableFlowLayoutPanel variableTable, Control cogControl)
            : base(varFilePath, variableTable)
        {
            _savedSettingsTextList = new List<string>()
            {
                "Use Night Mode",
                "Display Yaw Angles as Unsigned",
                "Variable Values Flush Right",
                "Start Slot Index From 1",
                "Offset Goto/Retrieve Functions",
                "PU Controller Moves Camera",
                "Scale Diagonal Position Controller Buttons",
                "Exclude Dust for Closest Object",
                "Use Misalignment Offset For Distance To Line",
                "Don't Round Values to 0",
                "Display as Hex Uses Memory",
                "Neutralize Triangles with 0x15",
                "Cloning Updates Holp Type",
                "Use In-Game Trig for Angle Logic",
                "Use Extended Level Boundaries",
                "Use Expanded Ram Size",
                "Do Quick Startup",
            };

            _savedSettingsGetterList = new List<Func<bool>>()
            {
                () => SavedSettingsConfig.UseNightMode,
                () => SavedSettingsConfig.DisplayYawAnglesAsUnsigned,
                () => SavedSettingsConfig.VariableValuesFlushRight,
                () => SavedSettingsConfig.StartSlotIndexsFromOne,
                () => SavedSettingsConfig.OffsetGotoRetrieveFunctions,
                () => SavedSettingsConfig.MoveCameraWithPu,
                () => SavedSettingsConfig.ScaleDiagonalPositionControllerButtons,
                () => SavedSettingsConfig.ExcludeDustForClosestObject,
                () => SavedSettingsConfig.UseMisalignmentOffsetForDistanceToLine,
                () => SavedSettingsConfig.DontRoundValuesToZero,
                () => SavedSettingsConfig.DisplayAsHexUsesMemory,
                () => SavedSettingsConfig.NeutralizeTrianglesWith0x15,
                () => SavedSettingsConfig.CloningUpdatesHolpType,
                () => SavedSettingsConfig.UseInGameTrigForAngleLogic,
                () => SavedSettingsConfig.UseExtendedLevelBoundaries,
                () => SavedSettingsConfig.UseExpandedRamSize,
                () => SavedSettingsConfig.DoQuickStartup,
            };

            _savedSettingsSetterList = new List<Action<bool>>()
            {
                (bool value) => SavedSettingsConfig.UseNightMode = value,
                (bool value) => SavedSettingsConfig.DisplayYawAnglesAsUnsigned = value,
                (bool value) => SavedSettingsConfig.VariableValuesFlushRight = value,
                (bool value) => SavedSettingsConfig.StartSlotIndexsFromOne = value,
                (bool value) => SavedSettingsConfig.OffsetGotoRetrieveFunctions = value,
                (bool value) => SavedSettingsConfig.MoveCameraWithPu = value,
                (bool value) => SavedSettingsConfig.ScaleDiagonalPositionControllerButtons = value,
                (bool value) => SavedSettingsConfig.ExcludeDustForClosestObject = value,
                (bool value) => SavedSettingsConfig.UseMisalignmentOffsetForDistanceToLine = value,
                (bool value) => SavedSettingsConfig.DontRoundValuesToZero = value,
                (bool value) => SavedSettingsConfig.DisplayAsHexUsesMemory = value,
                (bool value) => SavedSettingsConfig.NeutralizeTrianglesWith0x15 = value,
                (bool value) => SavedSettingsConfig.CloningUpdatesHolpType = value,
                (bool value) => SavedSettingsConfig.UseInGameTrigForAngleLogic = value,
                (bool value) => SavedSettingsConfig.UseExtendedLevelBoundaries = value,
                (bool value) => SavedSettingsConfig.UseExpandedRamSize = value,
                (bool value) => SavedSettingsConfig.DoQuickStartup = value,
            };

            SplitContainer splitContainerOptions = tabControl.Controls["splitContainerOptions"] as SplitContainer;

            _savedSettingsCheckedListBox = splitContainerOptions.Panel1.Controls["checkedListBoxSavedSettings"] as CheckedListBox;
            for (int i = 0; i < _savedSettingsTextList.Count; i++)
            {
                _savedSettingsCheckedListBox.Items.Add(_savedSettingsTextList[i], _savedSettingsGetterList[i]());
            }
            _savedSettingsCheckedListBox.ItemCheck += (sender, e) =>
            {
                _savedSettingsSetterList[e.Index](e.NewValue == CheckState.Checked);
            };

            Button buttonOptionsResetSavedSettings = splitContainerOptions.Panel1.Controls["buttonOptionsResetSavedSettings"] as Button;
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

            // object slot overlays
            List<string> objectSlotOverlayTextList = new List<string>()
            {
                "Held Object",
                "Stood On Object",
                "Ridden Object",
                "Interaction Object",
                "Used Object",
                "Closest Object",
                "Camera Object",
                "Camera Hack Object",
                "Floor Object",
                "Wall Object",
                "Ceiling Object",
                "Collision Object",
                "Hitbox Overlap Object",
                "Parent Object",
                "Child Object",
            };

            List<Func<bool>> objectSlotOverlayGetterList = new List<Func<bool>>()
            {
                () => OverlayConfig.ShowOverlayHeldObject,
                () => OverlayConfig.ShowOverlayStoodOnObject,
                () => OverlayConfig.ShowOverlayRiddenObject,
                () => OverlayConfig.ShowOverlayInteractionObject,
                () => OverlayConfig.ShowOverlayUsedObject,
                () => OverlayConfig.ShowOverlayClosestObject,
                () => OverlayConfig.ShowOverlayCameraObject,
                () => OverlayConfig.ShowOverlayCameraHackObject,
                () => OverlayConfig.ShowOverlayFloorObject,
                () => OverlayConfig.ShowOverlayWallObject,
                () => OverlayConfig.ShowOverlayCeilingObject,
                () => OverlayConfig.ShowOverlayCollisionObject,
                () => OverlayConfig.ShowOverlayHitboxOverlapObject,
                () => OverlayConfig.ShowOverlayParentObject,
                () => OverlayConfig.ShowOverlayChildObject,
            };

            List<Action<bool>> objectSlotOverlaySetterList = new List<Action<bool>>()
            {
                (bool value) => OverlayConfig.ShowOverlayHeldObject = value,
                (bool value) => OverlayConfig.ShowOverlayStoodOnObject = value,
                (bool value) => OverlayConfig.ShowOverlayRiddenObject = value,
                (bool value) => OverlayConfig.ShowOverlayInteractionObject = value,
                (bool value) => OverlayConfig.ShowOverlayUsedObject = value,
                (bool value) => OverlayConfig.ShowOverlayClosestObject = value,
                (bool value) => OverlayConfig.ShowOverlayCameraObject = value,
                (bool value) => OverlayConfig.ShowOverlayCameraHackObject = value,
                (bool value) => OverlayConfig.ShowOverlayFloorObject = value,
                (bool value) => OverlayConfig.ShowOverlayWallObject = value,
                (bool value) => OverlayConfig.ShowOverlayCeilingObject = value,
                (bool value) => OverlayConfig.ShowOverlayCollisionObject = value,
                (bool value) => OverlayConfig.ShowOverlayHitboxOverlapObject = value,
                (bool value) => OverlayConfig.ShowOverlayParentObject = value,
                (bool value) => OverlayConfig.ShowOverlayChildObject = value,
            };

            CheckedListBox checkedListBoxObjectSlotOverlaysToShow = splitContainerOptions.Panel1.Controls["checkedListBoxObjectSlotOverlaysToShow"] as CheckedListBox;
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
                int specialCount = 3;
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
        }

        public override void Update(bool updateView)
        {
            for (int i = 0; i < _savedSettingsCheckedListBox.Items.Count; i++)
            {
                bool value = _savedSettingsGetterList[i]();
                _savedSettingsCheckedListBox.SetItemChecked(i, value);
                _savedSettingsItemList[i].Checked = value;
            }

            if (!updateView) return;
            base.Update(updateView);
        }
    }
}
