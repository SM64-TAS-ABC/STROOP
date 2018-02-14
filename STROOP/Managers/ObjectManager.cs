using STROOP.Structs;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Controls;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using STROOP.Forms;

namespace STROOP.Managers
{
    public class ObjectManager : DataManager
    {
        string _slotIndex;
        string _slotPos;
        string _behavior;

        BinaryButton _releaseButton;
        BinaryButton _interactButton;
        BinaryButton _cloneButton;
        BinaryButton _unloadButton;

        Label _objAddressLabelValue;
        Label _objAddressLabel;
        Label _objSlotIndexLabel;
        Label _objSlotPositionLabel;
        Label _objBehaviorLabel;
        TextBox _objectNameTextBox;
        Panel _objectBorderPanel;
        IntPictureBox _objectImagePictureBox;
        
        #region Fields
        public void SetBehaviorWatchVariables(List<WatchVariableControl> watchVarControls, Color color)
        {
            RemoveObjSpecificVariables();
            watchVarControls.ForEach(watchVarControl => watchVarControl.BaseColor = color);
            AddVariables(watchVarControls);
        }

        List<uint> _currentAddresses = new List<uint>();
        public List<uint> CurrentAddresses
        {
            get
            {
                return _currentAddresses;
            }
            set
            {
                if (_currentAddresses.SequenceEqual(value))
                    return;

                _currentAddresses = value.ToList();

                if (_currentAddresses.Count > 1)
                    _objAddressLabelValue.Text = "";
                else if (_currentAddresses.Count > 0)
                    _objAddressLabelValue.Text = "0x" + _currentAddresses[0].ToString("X8");
                else
                    _objAddressLabelValue.Text = "";

                AddressChanged();
            }
        }

        public string SlotIndex
        {
            get
            {
                return _slotIndex;
            }
            set
            {
                if (_slotIndex != value)
                {
                    _slotIndex = value;
                    _objSlotIndexLabel.Text = _slotIndex;
                }
            }
        }

        public string SlotPos
        {
            get
            {
                return _slotPos;
            }
            set
            {
                if (_slotPos != value)
                {
                    _slotPos = value;
                    _objSlotPositionLabel.Text = _slotPos;
                }
            }
        }

        public string Behavior
        {
            get
            {
                return _behavior;
            }
            set
            {
                if (_behavior != value)
                {
                    _behavior = value;
                    _objBehaviorLabel.Text = value;
                }
            }
        }

        public string Name
        {
            get
            {
                return _objectNameTextBox.Text;
            }
            set
            {
                if (_objectNameTextBox.Text != value)
                    _objectNameTextBox.Text = value;
            }
        }

        public Color BackColor
        {
            set
            {
                if (_objectBorderPanel.BackColor != value)
                {
                    _objectBorderPanel.BackColor = value;
                    _objectImagePictureBox.BackColor = value.Lighten(0.7);
                }
            }
            get
            {
                return _objectBorderPanel.BackColor;
            }
        }

        public Image Image
        {
            get
            {
                return _objectImagePictureBox.Image;
            }
            set
            {
                if (_objectImagePictureBox.Image != value)
                    _objectImagePictureBox.Image = value;
            }
        }

        #endregion

        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
                VariableGroup.ObjectSpecific,
                VariableGroup.Collision,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.ObjectSpecific,
            };

        public ObjectManager(List<WatchVariableControlPrecursor> variables, Control objectControl, WatchVariableFlowLayoutPanel variableTable)
            : base(variables, variableTable, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
            SplitContainer splitContainerObject = objectControl.Controls["splitContainerObject"] as SplitContainer;

            _objAddressLabelValue = splitContainerObject.Panel1.Controls["labelObjAddValue"] as Label;
            _objAddressLabel = splitContainerObject.Panel1.Controls["labelObjAdd"] as Label;
            _objSlotIndexLabel = splitContainerObject.Panel1.Controls["labelObjSlotIndValue"] as Label;
            _objSlotPositionLabel = splitContainerObject.Panel1.Controls["labelObjSlotPosValue"] as Label;
            _objBehaviorLabel = splitContainerObject.Panel1.Controls["labelObjBhvValue"] as Label;
            _objBehaviorLabel.Click += _objBehaviorLabel_Click;
            _objectNameTextBox = splitContainerObject.Panel1.Controls["textBoxObjName"] as TextBox;
            _objectBorderPanel = splitContainerObject.Panel1.Controls["panelObjectBorder"] as Panel;
            _objectImagePictureBox = _objectBorderPanel.Controls["pictureBoxObject"] as IntPictureBox;

            _objAddressLabelValue.Click += ObjAddressLabel_Click;
            _objAddressLabel.Click += ObjAddressLabel_Click;

            Panel objPanel = splitContainerObject.Panel1.Controls["panelObj"] as Panel;

            var goToButton = objPanel.Controls["buttonObjGoto"] as Button;
            goToButton.Click += (sender, e) => ButtonUtilities.GotoObjects(_currentAddresses);
            ControlUtilities.AddContextMenuStripFunctions(
                goToButton,
                new List<string>() { "Goto", "Goto Laterally", "Goto X", "Goto Y", "Goto Z" },
                new List<Action>() {
                    () => ButtonUtilities.GotoObjects(_currentAddresses, (true, true, true)),
                    () => ButtonUtilities.GotoObjects(_currentAddresses, (true, false, true)),
                    () => ButtonUtilities.GotoObjects(_currentAddresses, (true, false, false)),
                    () => ButtonUtilities.GotoObjects(_currentAddresses, (false, true, false)),
                    () => ButtonUtilities.GotoObjects(_currentAddresses, (false, false, true)),
                });

            var retrieveButton = objPanel.Controls["buttonObjRetrieve"] as Button;
            retrieveButton.Click += (sender, e) => ButtonUtilities.RetrieveObjects(_currentAddresses);
            ControlUtilities.AddContextMenuStripFunctions(
                retrieveButton,
                new List<string>() { "Retrieve", "Retrieve Laterally", "Retrieve X", "Retrieve Y", "Retrieve Z" },
                new List<Action>() {
                    () => ButtonUtilities.RetrieveObjects(_currentAddresses, (true, true, true)),
                    () => ButtonUtilities.RetrieveObjects(_currentAddresses, (true, false, true)),
                    () => ButtonUtilities.RetrieveObjects(_currentAddresses, (true, false, false)),
                    () => ButtonUtilities.RetrieveObjects(_currentAddresses, (false, true, false)),
                    () => ButtonUtilities.RetrieveObjects(_currentAddresses, (false, false, true)),
                });

            var goToHomeButton = objPanel.Controls["buttonObjGotoHome"] as Button;
            goToHomeButton.Click += (sender, e) => ButtonUtilities.GotoObjectsHome(_currentAddresses);
            ControlUtilities.AddContextMenuStripFunctions(
                goToHomeButton,
                new List<string>() { "Goto Home", "Goto Home Laterally", "Goto Home X", "Goto Home Y", "Goto Home Z" },
                new List<Action>() {
                    () => ButtonUtilities.GotoObjectsHome(_currentAddresses, (true, true, true)),
                    () => ButtonUtilities.GotoObjectsHome(_currentAddresses, (true, false, true)),
                    () => ButtonUtilities.GotoObjectsHome(_currentAddresses, (true, false, false)),
                    () => ButtonUtilities.GotoObjectsHome(_currentAddresses, (false, true, false)),
                    () => ButtonUtilities.GotoObjectsHome(_currentAddresses, (false, false, true)),
                });

            var retrieveHomeButton = objPanel.Controls["buttonObjRetrieveHome"] as Button;
            retrieveHomeButton.Click += (sender, e) => ButtonUtilities.RetrieveObjectsHome(_currentAddresses);
            ControlUtilities.AddContextMenuStripFunctions(
                retrieveHomeButton,
                new List<string>() { "Retrieve Home", "Retrieve Home Laterally", "Retrieve Home X", "Retrieve Home Y", "Retrieve Home Z" },
                new List<Action>() {
                    () => ButtonUtilities.RetrieveObjectsHome(_currentAddresses, (true, true, true)),
                    () => ButtonUtilities.RetrieveObjectsHome(_currentAddresses, (true, false, true)),
                    () => ButtonUtilities.RetrieveObjectsHome(_currentAddresses, (true, false, false)),
                    () => ButtonUtilities.RetrieveObjectsHome(_currentAddresses, (false, true, false)),
                    () => ButtonUtilities.RetrieveObjectsHome(_currentAddresses, (false, false, true)),
                });

            _releaseButton = objPanel.Controls["buttonObjRelease"] as BinaryButton;
            _releaseButton.Initialize(
                "Release",
                "UnRelease",
                () => ButtonUtilities.ReleaseObject(_currentAddresses),
                () => ButtonUtilities.UnReleaseObject(_currentAddresses),
                () => _currentAddresses.Count > 0 && _currentAddresses.All(
                    address =>
                    {
                        uint releasedValue = Config.Stream.GetUInt32(address + ObjectConfig.ReleaseStatusOffset);
                        return releasedValue == ObjectConfig.ReleaseStatusThrownValue || releasedValue == ObjectConfig.ReleaseStatusDroppedValue;
                    }));
            ControlUtilities.AddContextMenuStripFunctions(
                _releaseButton,
                new List<string>() { "Release by Throwing", "Release by Dropping", "UnRelease" },
                new List<Action>() {
                    () => ButtonUtilities.ReleaseObject(_currentAddresses, true),
                    () => ButtonUtilities.ReleaseObject(_currentAddresses, false),
                    () => ButtonUtilities.UnReleaseObject(_currentAddresses),
                });

            _interactButton = objPanel.Controls["buttonObjInteract"] as BinaryButton;
            _interactButton.Initialize(
                "Interact",
                "UnInteract",
                () => ButtonUtilities.InteractObject(_currentAddresses),
                () => ButtonUtilities.UnInteractObject(_currentAddresses),
                () => _currentAddresses.Count > 0 && _currentAddresses.All(
                    address => Config.Stream.GetUInt32(address + ObjectConfig.InteractionStatusOffset) != 0));
            ControlUtilities.AddContextMenuStripFunctions(
                _interactButton,
                new List<string>() { "Interact", "UnInteract" },
                new List<Action>() {
                    () => ButtonUtilities.InteractObject(_currentAddresses),
                    () => ButtonUtilities.UnInteractObject(_currentAddresses),
                });

            _cloneButton = objPanel.Controls["buttonObjClone"] as BinaryButton;
            _cloneButton.Initialize(
                "Clone",
                "UnClone",
                () => ButtonUtilities.CloneObject(_currentAddresses[0]),
                () => ButtonUtilities.UnCloneObject(),
                () => _currentAddresses.Count == 1 && _currentAddresses.Contains(
                    Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.HeldObjectPointerOffset)));
            ControlUtilities.AddContextMenuStripFunctions(
                _cloneButton,
                new List<string>() {
                    "Clone with Action Update",
                    "Clone without Action Update",
                    "UnClone with Action Update",
                    "UnClone without Action Update",
                },
                new List<Action>() {
                    () => ButtonUtilities.CloneObject(_currentAddresses[0], true),
                    () => ButtonUtilities.CloneObject(_currentAddresses[0], false),
                    () => ButtonUtilities.UnCloneObject(true),
                    () => ButtonUtilities.UnCloneObject(false),
                });

            _unloadButton = objPanel.Controls["buttonObjUnload"] as BinaryButton;
            _unloadButton.Initialize(
                "Unload",
                "Revive",
                () => ButtonUtilities.UnloadObject(_currentAddresses),
                () => ButtonUtilities.ReviveObject(_currentAddresses),
                () => _currentAddresses.Count > 0 && _currentAddresses.All(
                    address => Config.Stream.GetUInt16(address + ObjectConfig.ActiveOffset) == 0x0000));
            ControlUtilities.AddContextMenuStripFunctions(
                _unloadButton,
                new List<string>() { "Unload", "Revive" },
                new List<Action>() {
                    () => ButtonUtilities.UnloadObject(_currentAddresses),
                    () => ButtonUtilities.ReviveObject(_currentAddresses),
                });

            var objPosGroupBox = objPanel.Controls["groupBoxObjPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                objPosGroupBox,
                objPosGroupBox.Controls["buttonObjPosXn"] as Button,
                objPosGroupBox.Controls["buttonObjPosXp"] as Button,
                objPosGroupBox.Controls["buttonObjPosZn"] as Button,
                objPosGroupBox.Controls["buttonObjPosZp"] as Button,
                objPosGroupBox.Controls["buttonObjPosXnZn"] as Button,
                objPosGroupBox.Controls["buttonObjPosXnZp"] as Button,
                objPosGroupBox.Controls["buttonObjPosXpZn"] as Button,
                objPosGroupBox.Controls["buttonObjPosXpZp"] as Button,
                objPosGroupBox.Controls["buttonObjPosYp"] as Button,
                objPosGroupBox.Controls["buttonObjPosYn"] as Button,
                objPosGroupBox.Controls["textBoxObjPosXZ"] as TextBox,
                objPosGroupBox.Controls["textBoxObjPosY"] as TextBox,
                objPosGroupBox.Controls["checkBoxObjPosRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateObjects(
                        _currentAddresses,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var objAngleGroupBox = objPanel.Controls["groupBoxObjAngle"] as GroupBox;
            ControlUtilities.InitializeScalarController(
                objAngleGroupBox.Controls["buttonObjAngleYawN"] as Button,
                objAngleGroupBox.Controls["buttonObjAngleYawP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAngleYaw"] as TextBox,
                (float yawValue) =>
                {
                    ButtonUtilities.RotateObjects(_currentAddresses, (int)Math.Round(yawValue), 0, 0);
                });
            ControlUtilities.InitializeScalarController(
                objAngleGroupBox.Controls["buttonObjAnglePitchN"] as Button,
                objAngleGroupBox.Controls["buttonObjAnglePitchP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAnglePitch"] as TextBox,
                (float pitchValue) =>
                {
                    ButtonUtilities.RotateObjects(_currentAddresses, 0, (int)Math.Round(pitchValue), 0);
                });
            ControlUtilities.InitializeScalarController(
                objAngleGroupBox.Controls["buttonObjAngleRollN"] as Button,
                objAngleGroupBox.Controls["buttonObjAngleRollP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAngleRoll"] as TextBox,
                (float rollValue) =>
                {
                    ButtonUtilities.RotateObjects(_currentAddresses, 0, 0, (int)Math.Round(rollValue));
                });

            var objScaleGroupBox = objPanel.Controls["groupBoxObjScale"] as GroupBox;
            ControlUtilities.InitializeScaleController(
                objScaleGroupBox.Controls["buttonObjScaleWidthN"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleWidthP"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleHeightN"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleHeightP"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleDepthN"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleDepthP"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleAggregateN"] as Button,
                objScaleGroupBox.Controls["buttonObjScaleAggregateP"] as Button,
                objScaleGroupBox.Controls["textBoxObjScaleWidth"] as TextBox,
                objScaleGroupBox.Controls["textBoxObjScaleHeight"] as TextBox,
                objScaleGroupBox.Controls["textBoxObjScaleDepth"] as TextBox,
                objScaleGroupBox.Controls["textBoxObjScaleAggregate"] as TextBox,
                objScaleGroupBox.Controls["checkBoxObjScaleAggregate"] as CheckBox,
                objScaleGroupBox.Controls["checkBoxObjScaleMultiply"] as CheckBox,
                (float widthChange, float heightChange, float depthChange, bool multiply) =>
                {
                    ButtonUtilities.ScaleObjects(_currentAddresses, widthChange, heightChange, depthChange, multiply);
                });

            var objHomeGroupBox = objPanel.Controls["groupBoxObjHome"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                objHomeGroupBox,
                objHomeGroupBox.Controls["buttonObjHomeXn"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeZn"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeZp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXnZn"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXnZp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXpZn"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeXpZp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeYp"] as Button,
                objHomeGroupBox.Controls["buttonObjHomeYn"] as Button,
                objHomeGroupBox.Controls["textBoxObjHomeXZ"] as TextBox,
                objHomeGroupBox.Controls["textBoxObjHomeY"] as TextBox,
                objHomeGroupBox.Controls["checkBoxObjHomeRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateObjectHomes(
                        _currentAddresses,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });
        }

        private void _objBehaviorLabel_Click(object sender, EventArgs e)
        {
            if (CurrentAddresses.Count == 0)
                return;

            var scriptAddress = Config.Stream.GetUInt32(CurrentAddresses[0] + ObjectConfig.BehaviorScriptOffset);
            Config.ScriptManager.Go(scriptAddress);
            Config.StroopMainForm.SwitchTab("tabPageScripts");
        }

        private void AddressChanged()
        {
            if (CurrentAddresses.Count <= 1)
            {
                _cloneButton.Enabled = true;
            }
            else
            {
                _cloneButton.Enabled = false;
            }
        }

        private void ObjAddressLabel_Click(object sender, EventArgs e)
        {
            if (_currentAddresses.Count == 0)
                return;

            var variableTitle = "Object Address" + (_currentAddresses.Count > 1 ? " (First of Multiple)" : ""); 
            var variableInfo = new VariableViewerForm(variableTitle, "Object", "Relative + " + String.Format("0x{0:X8}", _currentAddresses[0]),
                String.Format("0x{0:X8}", _currentAddresses[0]), String.Format("0x{0:X8}", (_currentAddresses[0] & ~0x80000000) + Config.Stream.ProcessMemoryOffset.ToInt64()));
            variableInfo.Show();
        }
      
        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            _releaseButton.UpdateButton();
            _interactButton.UpdateButton();
            _cloneButton.UpdateButton();
            _unloadButton.UpdateButton();

            base.Update(updateView);
        }

    }
}
