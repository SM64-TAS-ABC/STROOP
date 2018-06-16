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
using STROOP.Models;
using System.Collections.ObjectModel;

namespace STROOP.Managers
{
    public class ObjectManager : DataManager
    {
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

        Image _multiImage = null;
        List<BehaviorCriteria> _lastBehaviors = new List<BehaviorCriteria>();
        BehaviorCriteria? _lastGeneralizedBehavior;

        #region UI Properties
        string _slotIndex;
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

        string _slotPos;
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

        string _behavior;
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

        private List<uint> _addresses
        {
            get => Config.ObjectSlotsManager.SelectedSlotsAddresses;
        }
        private List<ObjectDataModel> _objects
        {
            get => Config.ObjectSlotsManager.SelectedObjects;
        }

        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
                VariableGroup.ObjectSpecific,
                VariableGroup.Collision,
                VariableGroup.Movement,
                VariableGroup.Transformation,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.ObjectSpecific,
            };

        public ObjectManager(string varFilePath, Control objectControl, WatchVariableFlowLayoutPanel variableTable)
            : base(varFilePath, variableTable, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
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
            goToButton.Click += (sender, e) => ButtonUtilities.GotoObjects(_objects);
            ControlUtilities.AddContextMenuStripFunctions(
                goToButton,
                new List<string>() { "Goto", "Goto Laterally", "Goto X", "Goto Y", "Goto Z" },
                new List<Action>() {
                    () => ButtonUtilities.GotoObjects(_objects, (true, true, true)),
                    () => ButtonUtilities.GotoObjects(_objects, (true, false, true)),
                    () => ButtonUtilities.GotoObjects(_objects, (true, false, false)),
                    () => ButtonUtilities.GotoObjects(_objects, (false, true, false)),
                    () => ButtonUtilities.GotoObjects(_objects, (false, false, true)),
                });

            var retrieveButton = objPanel.Controls["buttonObjRetrieve"] as Button;
            retrieveButton.Click += (sender, e) => ButtonUtilities.RetrieveObjects(_objects);
            ControlUtilities.AddContextMenuStripFunctions(
                retrieveButton,
                new List<string>() { "Retrieve", "Retrieve Laterally", "Retrieve X", "Retrieve Y", "Retrieve Z" },
                new List<Action>() {
                    () => ButtonUtilities.RetrieveObjects(_objects, (true, true, true)),
                    () => ButtonUtilities.RetrieveObjects(_objects, (true, false, true)),
                    () => ButtonUtilities.RetrieveObjects(_objects, (true, false, false)),
                    () => ButtonUtilities.RetrieveObjects(_objects, (false, true, false)),
                    () => ButtonUtilities.RetrieveObjects(_objects, (false, false, true)),
                });

            var goToHomeButton = objPanel.Controls["buttonObjGotoHome"] as Button;
            goToHomeButton.Click += (sender, e) => ButtonUtilities.GotoObjectsHome(_objects);
            ControlUtilities.AddContextMenuStripFunctions(
                goToHomeButton,
                new List<string>() { "Goto Home", "Goto Home Laterally", "Goto Home X", "Goto Home Y", "Goto Home Z" },
                new List<Action>() {
                    () => ButtonUtilities.GotoObjectsHome(_objects, (true, true, true)),
                    () => ButtonUtilities.GotoObjectsHome(_objects, (true, false, true)),
                    () => ButtonUtilities.GotoObjectsHome(_objects, (true, false, false)),
                    () => ButtonUtilities.GotoObjectsHome(_objects, (false, true, false)),
                    () => ButtonUtilities.GotoObjectsHome(_objects, (false, false, true)),
                });

            var retrieveHomeButton = objPanel.Controls["buttonObjRetrieveHome"] as Button;
            retrieveHomeButton.Click += (sender, e) => ButtonUtilities.RetrieveObjectsHome(_objects);
            ControlUtilities.AddContextMenuStripFunctions(
                retrieveHomeButton,
                new List<string>() { "Retrieve Home", "Retrieve Home Laterally", "Retrieve Home X", "Retrieve Home Y", "Retrieve Home Z" },
                new List<Action>() {
                    () => ButtonUtilities.RetrieveObjectsHome(_objects, (true, true, true)),
                    () => ButtonUtilities.RetrieveObjectsHome(_objects, (true, false, true)),
                    () => ButtonUtilities.RetrieveObjectsHome(_objects, (true, false, false)),
                    () => ButtonUtilities.RetrieveObjectsHome(_objects, (false, true, false)),
                    () => ButtonUtilities.RetrieveObjectsHome(_objects, (false, false, true)),
                });

            _releaseButton = objPanel.Controls["buttonObjRelease"] as BinaryButton;
            _releaseButton.Initialize(
                "Release",
                "UnRelease",
                () => ButtonUtilities.ReleaseObject(_objects),
                () => ButtonUtilities.UnReleaseObject(_objects),
                () => _objects.Count > 0 && _objects.All(o => 
                    o.ReleaseStatus == ObjectConfig.ReleaseStatusThrownValue 
                    || o.ReleaseStatus == ObjectConfig.ReleaseStatusDroppedValue));
            ControlUtilities.AddContextMenuStripFunctions(
                _releaseButton,
                new List<string>() { "Release by Throwing", "Release by Dropping", "UnRelease" },
                new List<Action>() {
                    () => ButtonUtilities.ReleaseObject(_objects, true),
                    () => ButtonUtilities.ReleaseObject(_objects, false),
                    () => ButtonUtilities.UnReleaseObject(_objects),
                });

            _interactButton = objPanel.Controls["buttonObjInteract"] as BinaryButton;
            _interactButton.Initialize(
                "Interact",
                "UnInteract",
                () => ButtonUtilities.InteractObject(_objects),
                () => ButtonUtilities.UnInteractObject(_objects),
                () => _objects.Count > 0 && _objects.All(o => o.InteractionStatus != 0));
            ControlUtilities.AddContextMenuStripFunctions(
                _interactButton,
                new List<string>() { "Interact", "UnInteract" },
                new List<Action>() {
                    () => ButtonUtilities.InteractObject(_objects),
                    () => ButtonUtilities.UnInteractObject(_objects),
                });

            _cloneButton = objPanel.Controls["buttonObjClone"] as BinaryButton;
            _cloneButton.Initialize(
                "Clone",
                "UnClone",
                () => ButtonUtilities.CloneObject(_objects.First()),
                () => ButtonUtilities.UnCloneObject(),
                () => _objects.Count == 1 && _objects.Any(o => o.Address == DataModels.Mario.HeldObject));
            ControlUtilities.AddContextMenuStripFunctions(
                _cloneButton,
                new List<string>() {
                    "Clone with Action Update",
                    "Clone without Action Update",
                    "UnClone with Action Update",
                    "UnClone without Action Update",
                },
                new List<Action>() {
                    () => ButtonUtilities.CloneObject(_objects.First(), true),
                    () => ButtonUtilities.CloneObject(_objects.First(), false),
                    () => ButtonUtilities.UnCloneObject(true),
                    () => ButtonUtilities.UnCloneObject(false),
                });

            _unloadButton = objPanel.Controls["buttonObjUnload"] as BinaryButton;
            _unloadButton.Initialize(
                "Unload",
                "Revive",
                () => ButtonUtilities.UnloadObject(_objects),
                () => ButtonUtilities.ReviveObject(_objects),
                () => _objects.Count > 0 && _objects.All(o => !o.IsActive));
            ControlUtilities.AddContextMenuStripFunctions(
                _unloadButton,
                new List<string>() { "Unload", "Revive" },
                new List<Action>() {
                    () => ButtonUtilities.UnloadObject(_objects),
                    () => ButtonUtilities.ReviveObject(_objects),
                });

            var objPosGroupBox = objPanel.Controls["groupBoxObjPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
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
                        _objects,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative,
                        KeyboardUtilities.IsCtrlHeld());
                });

            var objAngleGroupBox = objPanel.Controls["groupBoxObjAngle"] as GroupBox;
            ControlUtilities.InitializeScalarController(
                objAngleGroupBox.Controls["buttonObjAngleYawN"] as Button,
                objAngleGroupBox.Controls["buttonObjAngleYawP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAngleYaw"] as TextBox,
                (float yawValue) =>
                {
                    ButtonUtilities.RotateObjects(_objects, (int)Math.Round(yawValue), 0, 0);
                });
            ControlUtilities.InitializeScalarController(
                objAngleGroupBox.Controls["buttonObjAnglePitchN"] as Button,
                objAngleGroupBox.Controls["buttonObjAnglePitchP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAnglePitch"] as TextBox,
                (float pitchValue) =>
                {
                    ButtonUtilities.RotateObjects(_objects, 0, (int)Math.Round(pitchValue), 0);
                });
            ControlUtilities.InitializeScalarController(
                objAngleGroupBox.Controls["buttonObjAngleRollN"] as Button,
                objAngleGroupBox.Controls["buttonObjAngleRollP"] as Button,
                objAngleGroupBox.Controls["textBoxObjAngleRoll"] as TextBox,
                (float rollValue) =>
                {
                    ButtonUtilities.RotateObjects(_objects, 0, 0, (int)Math.Round(rollValue));
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
                    ButtonUtilities.ScaleObjects(_objects, widthChange, heightChange, depthChange, multiply);
                });

            var objHomeGroupBox = objPanel.Controls["groupBoxObjHome"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
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
                        _objects,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });
        }

        private void _objBehaviorLabel_Click(object sender, EventArgs e)
        {
            if (_objects.Count == 0)
                return;

            var scriptAddress = Config.Stream.GetUInt32(_objects.First().Address + ObjectConfig.BehaviorScriptOffset);
            Config.ScriptManager.Go(scriptAddress);
            Config.StroopMainForm.SwitchTab("tabPageScripts");
        }

        public void SetBehaviorWatchVariables(List<WatchVariableControl> watchVarControls, Color color)
        {
            RemoveVariableGroup(VariableGroup.ObjectSpecific);
            watchVarControls.ForEach(watchVarControl => watchVarControl.BaseColor = color);
            AddVariables(watchVarControls);
        }

        private void ObjAddressLabel_Click(object sender, EventArgs e)
        {
            if (_objects.Count == 0)
                return;

            var variableTitle = "Object Address" + (_objects.Count > 1 ? " (First of Multiple)" : "");
            var variableInfo = new VariableViewerForm(
                variableTitle,
                "Object",
                "Relative + " + HexUtilities.FormatValue(_objects.First().Address, 8),
                HexUtilities.FormatValue(_objects.First().Address, 8),
                HexUtilities.FormatValue(Config.Stream.GetAbsoluteAddress(_objects.First().Address).ToUInt32(), 8));
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

            UpdateUI();

            base.Update(updateView);
        }

        void UpdateUI()
        {
            if (!_objects.Any())
            {
                Name = "No Object Selected";
                Image = null;
                BackColor = ObjectSlotsConfig.VacantSlotColor;
                Behavior = "";
                SlotIndex = "";
                SlotPos = "";
                _objAddressLabelValue.Text = "";
                _cloneButton.Enabled = false;
                _lastGeneralizedBehavior = null;
                SetBehaviorWatchVariables(new List<WatchVariableControl>(), Color.White);
            }
            else if (_objects.Count() == 1)
            {
                ObjectDataModel obj = _objects.First();
                var newBehavior = obj.BehaviorCriteria;
                if (_lastGeneralizedBehavior != newBehavior)
                {
                    Behavior = $"0x{obj.SegmentedBehavior & 0x00FFFFFF:X4}";
                    Name = Config.ObjectAssociations.GetObjectName(newBehavior);
                    SetBehaviorWatchVariables(
                        Config.ObjectAssociations.GetWatchVarControls(newBehavior),
                        ObjectSlotsConfig.GetProcessingGroupColor(obj.BehaviorProcessGroup)
                        .Lighten(0.8));
                    Image = Config.ObjectAssociations.GetObjectImage(newBehavior);
                    _lastGeneralizedBehavior = newBehavior;
                }
                BackColor = ObjectSlotsConfig.GetProcessingGroupColor(obj.CurrentProcessGroup);
                int slotPos = obj.VacantSlotIndex ?? obj.ProcessIndex;
                SlotIndex = (Config.ObjectSlotsManager.GetSlotIndexFromObj(obj) 
                    + (SavedSettingsConfig.StartSlotIndexsFromOne ? 1 : 0))?.ToString() ?? "";
                SlotPos = $"{(obj.VacantSlotIndex.HasValue ? "VS " : "")}{slotPos + (SavedSettingsConfig.StartSlotIndexsFromOne ? 1 : 0)}";
                _objAddressLabelValue.Text = $"0x{_objects.First().Address:X8}";
                _cloneButton.Enabled = true;
            }
            else
            {
                IEnumerable<BehaviorCriteria> newBehaviors = _objects.Select(o => o.BehaviorCriteria);

                // Find new generalized criteria
                BehaviorCriteria? multiBehavior = _objects.First().BehaviorCriteria;
                foreach (ObjectDataModel obj in _objects)
                    multiBehavior = multiBehavior?.Generalize(obj.BehaviorCriteria);

                // Find general process group
                byte? processGroup = _objects.First().CurrentProcessGroup;
                if (_objects.Any(o => o.CurrentProcessGroup != processGroup)) processGroup = null;

                // Update behavior and watach variables
                if (_lastGeneralizedBehavior != multiBehavior)
                {
                    if (multiBehavior.HasValue)
                    {
                        Behavior = $"0x{multiBehavior.Value.BehaviorAddress:X4}";
                        SetBehaviorWatchVariables(
                            Config.ObjectAssociations.GetWatchVarControls(multiBehavior.Value),
                            ObjectSlotsConfig.GetProcessingGroupColor(processGroup).Lighten(0.8));
                    }
                    else
                    {
                        Behavior = "";
                        SetBehaviorWatchVariables(new List<WatchVariableControl>(), Color.White);
                    }
                    _lastGeneralizedBehavior = multiBehavior;
                }
                if (!newBehaviors.SequenceEqual(_lastBehaviors))
                {
                    // Generate new image
                    _multiImage?.Dispose();
                    _multiImage = CreateMultiObjectImage(newBehaviors);

                    _lastBehaviors = newBehaviors.ToList();
                }

                Image = _multiImage;
                Name = _objects.Count + " Objects Selected";
                BackColor = ObjectSlotsConfig.GetProcessingGroupColor(processGroup);
                SlotIndex = "";
                SlotPos = "";
                _objAddressLabelValue.Text = "";
                _cloneButton.Enabled = false;
            }
        }

        private Image CreateMultiObjectImage(IEnumerable<BehaviorCriteria> criterias)
        {
            Image multiBitmap = new Bitmap(256, 256);
            using (Graphics gfx = Graphics.FromImage(multiBitmap))
            {
                int count = criterias.Count();
                int numCols = (int)Math.Ceiling(Math.Sqrt(count));
                int numRows = (int)Math.Ceiling(count / (double)numCols);
                int imageSize = 256 / numCols;
                foreach (int row in Enumerable.Range(0, numRows))
                {
                    foreach (int col in Enumerable.Range(0, numCols))
                    {
                        int index = row * numCols + col;
                        if (index >= count) break;
                        Image image = Config.ObjectAssociations.GetObjectImage(criterias.ElementAt(index), false);
                        Rectangle rect = new Rectangle(col * imageSize, row * imageSize, imageSize, imageSize);
                        Rectangle zoomedRect = rect.Zoom(image.Size);
                        gfx.DrawImage(image, zoomedRect);
                    }
                }
            }

            return multiBitmap;
        }
    }
}
