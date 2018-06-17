using STROOP.Forms;
using STROOP.Managers;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;

namespace STROOP.Controls
{
    public partial class WatchVariableControl : UserControl
    {
        // Main objects
        public readonly WatchVariableControlPrecursor WatchVarPrecursor;
        private readonly WatchVariableWrapper _watchVarWrapper;
        public readonly List<VariableGroup> GroupList;

        // Sub controls
        private readonly ContextMenuStrip _valueTextboxContextMenuStrip;
        private readonly ContextMenuStrip _nameTextboxContextMenuStrip;
        private readonly ContextMenuStrip _variableContextMenuStrip;
        private readonly List<ToolStripItem> _variableContextMenuStripItems;

        private ContextMenuStrip _selectionContextMenuStrip;
        private readonly List<ToolStripItem> _selectionContextMenuStripItems;

        // Parent control
        private WatchVariableFlowLayoutPanel _watchVariablePanel;

        public string TextBoxValue
        {
            get { return _valueTextBox.Text; }
            set { _valueTextBox.Text = value; }
        }

        public CheckState CheckBoxValue
        {
            get { return _valueCheckBox.CheckState; }
            set { _valueCheckBox.CheckState = value; }
        }

        public static readonly Color DEFAULT_COLOR = SystemColors.Control;
        public static readonly Color FAILURE_COLOR = Color.Red;
        public static readonly Color ENABLE_CUSTOM_FUNCIONALITY_COLOR = Color.Yellow;
        public static readonly Color ADD_TO_CUSTOM_TAB_COLOR = Color.CornflowerBlue;
        public static readonly Color REORDER_START_COLOR = Color.DarkGreen;
        public static readonly Color REORDER_END_COLOR = Color.LightGreen;
        public static readonly Color REORDER_RESET_COLOR = Color.Black;
        public static readonly Color ADD_TO_VAR_HACK_TAB_COLOR = Color.SandyBrown;
        public static readonly Color SELECTED_COLOR = Color.FromArgb(51, 153, 255);
        private static readonly int FLASH_DURATION_MS = 1000;

        private Color _baseColor;
        public Color BaseColor
        {
            get { return _baseColor; }
            set { _baseColor = value; _currentColor = value; }
        }

        private Color _currentColor;
        private bool _isFlashing;
        private DateTime _flashStartTime;
        private Color _flashColor;

        private string _varName;
        public string VarName
        {
            get
            {
                return _varName;
            }
        }

        public bool Highlighted
        {
            get
            {
                return _tableLayoutPanel.ShowBorder;
            }
            set
            {
                if (!_tableLayoutPanel.ShowBorder && value)
                {
                    _tableLayoutPanel.BorderColor = Color.Red;
                }
                _tableLayoutPanel.ShowBorder = value;
            }
        }

        private bool _editMode;
        public bool EditMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                if (_editMode == value) return;
                _editMode = value;
                _watchVariablePanel.UnselectAllVariables();
                _valueTextBox.ReadOnly = !_editMode;
                _valueTextBox.BackColor = _editMode ? Color.White : _currentColor;
                _valueTextBox.ContextMenuStrip = _editMode ? _valueTextboxContextMenuStrip : ContextMenuStrip;
                if (_editMode)
                {
                    _valueTextBox.Focus();
                    _valueTextBox.SelectAll();
                }
            }
        }

        private bool _renameMode;
        public bool RenameMode
        {
            get
            {
                return _renameMode;
            }
            set
            {
                if (_renameMode == value) return;
                _renameMode = value;
                _watchVariablePanel.UnselectAllVariables();
                _nameTextBox.ReadOnly = !_renameMode;
                _nameTextBox.BackColor = _renameMode ? Color.White : _currentColor;
                _nameTextBox.ContextMenuStrip = _renameMode ? _nameTextboxContextMenuStrip : ContextMenuStrip;
                if (_renameMode)
                {
                    _nameTextBox.Focus();
                    _nameTextBox.SelectAll();
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
            }
        }

        public List<uint> FixedAddressList;

        private int _settingsLevel = 0;

        private static readonly Image _lockedImage = Properties.Resources.img_lock;
        private static readonly Image _someLockedImage = Properties.Resources.img_lock_grey;
        private static readonly Image _disabledLockImage = Properties.Resources.lock_blue;
        private static readonly Image _pinnedImage = Properties.Resources.img_pin;

        private bool _rightFlush;

        private static readonly int PIN_OUTER_PADDING = 11;
        private static readonly int PIN_INNER_PADDING = 24;

        private static readonly int VALUE_TEXTBOX_SIZE_DIFF = 6;
        private static readonly int VALUE_TEXTBOX_MARGIN = 3;

        public static readonly int DEFAULT_VARIABLE_NAME_WIDTH = 120;
        public static readonly int DEFAULT_VARIABLE_VALUE_WIDTH = 85;
        public static readonly int DEFAULT_VARIABLE_HEIGHT = 20;

        public static int VariableNameWidth = DEFAULT_VARIABLE_NAME_WIDTH;
        public static int VariableValueWidth = DEFAULT_VARIABLE_VALUE_WIDTH;
        public static int VariableHeight = DEFAULT_VARIABLE_HEIGHT;

        private int _variableNameWidth;
        private int _variableValueWidth;
        private int _variableHeight;

        public WatchVariableControl(
            WatchVariableControlPrecursor watchVarPrecursor,
            string name,
            WatchVariable watchVar,
            WatchVariableSubclass subclass,
            Color? backgroundColor,
            Type displayType,
            int? roundingLimit,
            bool? useHex,
            bool? invertBool,
            bool? isYaw,
            WatchVariableCoordinate? coordinate,
            List<VariableGroup> groupList,
            List<uint> fixedAddresses)
        {
            // Initialize controls
            InitializeComponent();
            _tableLayoutPanel.BorderColor = Color.Red;
            _tableLayoutPanel.BorderWidth = 3;
            _nameTextBox.Text = name;

            // Store the precursor
            WatchVarPrecursor = watchVarPrecursor;

            // Initialize main fields
            _varName = name;
            GroupList = groupList;
            _editMode = false;
            _renameMode = false;
            _isSelected = false;
            FixedAddressList = fixedAddresses;

            // Initialize color fields
            _baseColor = backgroundColor ?? DEFAULT_COLOR;
            _currentColor = _baseColor;
            _isFlashing = false;
            _flashStartTime = DateTime.Now;

            // Initialize flush/size fields
            _rightFlush = true;
            _variableNameWidth = 0;
            _variableValueWidth = 0;
            _variableHeight = 0;

            // Create watch var wrapper
            _watchVarWrapper = WatchVariableWrapper.CreateWatchVariableWrapper(
                watchVar, this, subclass, displayType, roundingLimit, useHex, invertBool, isYaw, coordinate);

            // Initialize context menu strip
            _valueTextboxContextMenuStrip = _valueTextBox.ContextMenuStrip;
            _nameTextboxContextMenuStrip = _nameTextBox.ContextMenuStrip;
            _variableContextMenuStrip = _watchVarWrapper.GetContextMenuStrip();
            _variableContextMenuStripItems = new List<ToolStripItem>();
            foreach (ToolStripItem item in _variableContextMenuStrip.Items)
            {
                _variableContextMenuStripItems.Add(item);
            }
            _selectionContextMenuStripItems = new List<ToolStripItem>();

            ContextMenuStrip = _variableContextMenuStrip;
            _nameTextBox.ContextMenuStrip = _variableContextMenuStrip;
            _valueTextBox.ContextMenuStrip = _variableContextMenuStrip;

            // Set whether to start as a checkbox
            SetUseCheckbox(_watchVarWrapper.StartsAsCheckbox());

            // Add functions
            _namePanel.Click += (sender, e) => OnVariableClick();
            _namePanel.DoubleClick += (sender, e) => OnNameTextBoxDoubleClick();

            _nameTextBox.Click += (sender, e) => OnVariableClick();
            _nameTextBox.DoubleClick += (sender, e) => OnNameTextBoxDoubleClick();
            _nameTextBox.Leave += (sender, e) => { RenameMode = false; };
            _nameTextBox.KeyDown += (sender, e) => OnNameTextValueKeyDown(e);

            _valuePanel.Click += (sender, e) => OnVariableClick();

            _valueTextBox.Click += (sender, e) => _watchVariablePanel.UnselectAllVariables();
            _valueTextBox.DoubleClick += (sender, e) => { EditMode = true; };
            _valueTextBox.KeyDown += (sender, e) => OnValueTextValueKeyDown(e);
            _valueTextBox.Leave += (sender, e) => { EditMode = false; };

            _valueCheckBox.Click += (sender, e) => OnCheckboxClick();

            ContextMenuStrip.Opening += (sender, e) => OnContextMenuStripOpening();
        }
        
        public void SetUseCheckbox(bool useCheckbox)
        {
            if (useCheckbox)
            {
                _valueTextBox.Visible = false;
                _valueCheckBox.Visible = true;
            }
            else
            {
                _valueTextBox.Visible = true;
                _valueCheckBox.Visible = false;
            }
        }

        private void OnValueTextValueKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (_editMode)
            {
                if (e.KeyData == Keys.Escape)
                {
                    EditMode = false;
                    this.Focus();
                    return;
                }

                if (e.KeyData == Keys.Enter ||
                    e.KeyData == (Keys.Enter | Keys.Control))
                {
                    EditMode = false;
                    SetValue(_valueTextBox.Text);
                    this.Focus();
                    return;
                }
            }
        }

        private void OnContextMenuStripOpening()
        {
            if (IsSelected && _watchVariablePanel.GetNumSelectedVariables() >= 2)
            {
                ContextMenuStrip.Items.Clear();
                _selectionContextMenuStripItems.ForEach(item => ContextMenuStrip.Items.Add(item));
            }
            else
            {
                ContextMenuStrip.Items.Clear();
                _variableContextMenuStripItems.ForEach(item => ContextMenuStrip.Items.Add(item));
            }
        }

        private void OnVariableClick()
        {
            this.Focus();

            bool isCtrlKeyHeld = KeyboardUtilities.IsCtrlHeld();
            bool isShiftKeyHeld = KeyboardUtilities.IsShiftHeld();
            bool isAltKeyHeld = KeyboardUtilities.IsAltHeld();
            bool isFKeyHeld = Keyboard.IsKeyDown(Key.F);
            bool isHKeyHeld = Keyboard.IsKeyDown(Key.H);
            bool isLKeyHeld = Keyboard.IsKeyDown(Key.L);
            bool isDKeyHeld = Keyboard.IsKeyDown(Key.D);
            bool isRKeyHeld = Keyboard.IsKeyDown(Key.R);
            bool isCKeyHeld = Keyboard.IsKeyDown(Key.C);
            bool isBKeyHeld = Keyboard.IsKeyDown(Key.B);
            bool isQKeyHeld = Keyboard.IsKeyDown(Key.Q);
            bool isOKeyHeld = Keyboard.IsKeyDown(Key.O);
            bool isTKeyHeld = Keyboard.IsKeyDown(Key.T);
            bool isMKeyHeld = Keyboard.IsKeyDown(Key.M);
            bool isPKeyHeld = Keyboard.IsKeyDown(Key.P);
            bool isXKeyHeld = Keyboard.IsKeyDown(Key.X);
            bool isSKeyHeld = Keyboard.IsKeyDown(Key.S);
            bool isDeletishKeyHeld = KeyboardUtilities.IsDeletishKeyHeld();
            bool isBacktickHeld = Keyboard.IsKeyDown(Key.OemTilde);
            bool isZHeld = Keyboard.IsKeyDown(Key.Z);
            bool isMinusHeld = Keyboard.IsKeyDown(Key.OemMinus);
            bool isPlusHeld = Keyboard.IsKeyDown(Key.OemPlus);
            bool isNumberHeld = KeyboardUtilities.IsCurrentlyInputtedNumber();

            if (isShiftKeyHeld && isNumberHeld)
            {
                BaseColor = ColorUtilities.GetColorForVariable();
                return;
            }

            if (isFKeyHeld)
            {
                ToggleFixedAddress();
                return;
            }

            if (isHKeyHeld)
            {
                ToggleHighlighted();
                return;
            }

            if (isNumberHeld)
            {
                Color? color = ColorUtilities.GetColorForHighlight();
                ToggleHighlighted(color);
                return;
            }

            if (isLKeyHeld)
            {
                _watchVarWrapper.ToggleLocked(FixedAddressList);
                return;
            }

            if (isDKeyHeld)
            {
                _watchVarWrapper.ToggleDisplayAsHex();
                return;
            }

            if (isRKeyHeld)
            {
                RenameMode = true;
                return;
            }

            if (isCKeyHeld)
            {
                _watchVarWrapper.ShowControllerForm();
                return;
            }

            if (isBKeyHeld)
            {
                _watchVarWrapper.ShowBitForm();
                return;
            }

            if (isDeletishKeyHeld)
            {
                DeleteFromPanel();
                return;
            }

            if (isSKeyHeld)
            {
                AddToTab(Config.CustomManager);
                return;
            }

            if (isXKeyHeld)
            {
                NotifyPanelOfReodering();
                return;
            }

            if (isAltKeyHeld)
            {
                EnableCustomFunctionality();
                return;
            }

            if (isBacktickHeld)
            {
                AddToVarHackTab();
                return;
            }

            if (isTKeyHeld)
            {
                AddToTab(Config.TasManager);
                return;
            }

            if (isMKeyHeld)
            {
                AddToTab(Config.MemoryManager);
                return;
            }

            if (isPKeyHeld)
            {
                SelectionForm.ShowDataManagerSelectionForm(this);
                return;
            }

            if (isZHeld)
            {
                SetValue("0");
                return;
            }

            if (isMinusHeld)
            {
                AddValue("1", false);
                return;
            }

            if (isPlusHeld)
            {
                AddValue("1", true);
                return;
            }

            if (isQKeyHeld)
            {
                Color? newColor = ColorUtilities.GetColorFromDialog(BaseColor);
                if (newColor.HasValue)
                {
                    BaseColor = newColor.Value;
                    ColorUtilities.LastSelectedColor = newColor.Value;
                }
                return;
            }

            if (isOKeyHeld)
            {
                BaseColor = DEFAULT_COLOR;
                return;
            }

            // default
            {
                _watchVariablePanel.NotifySelectClick(this, isCtrlKeyHeld, isShiftKeyHeld);
                return;
            }
        }

        private void OnNameTextBoxDoubleClick()
        {
            this.Focus();
            _nameTextBox.Select(0, 0);
            _watchVarWrapper.ShowVarInfo();
        }

        private void OnNameTextValueKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (_renameMode)
            {
                if (e.KeyData == Keys.Escape)
                {
                    RenameMode = false;
                    _nameTextBox.Text = VarName;
                    this.Focus();
                    return;
                }

                if (e.KeyData == Keys.Enter)
                {
                    _varName = _nameTextBox.Text;
                    RenameMode = false;
                    this.Focus();
                    return;
                }
            }
        }

        private void OnCheckboxClick()
        {
            bool success = _watchVarWrapper.SetCheckStateValue(_valueCheckBox.CheckState, FixedAddressList);
            if (!success) FlashColor(FAILURE_COLOR);
        }

        public void UpdateControl()
        {
            _watchVarWrapper.UpdateItemCheckStates();

            UpdateSettings();
            UpdateFlush();
            UpdateSize();
            UpdateColor();
            UpdatePictureBoxes();

            if (!EditMode)
            {
                if (_valueTextBox.Visible) _valueTextBox.Text = _watchVarWrapper.GetValue(true, true, FixedAddressList).ToString();
                if (_valueCheckBox.Visible) _valueCheckBox.CheckState = _watchVarWrapper.GetCheckStateValue(FixedAddressList);
            }
        }

        private void UpdateSettings()
        {
            if (_settingsLevel < WatchVariableControlSettingsManager.GetSettingsLevel())
            {
                WatchVariableControlSettingsManager.GetSettingsToApply(_settingsLevel)
                    .ForEach(settings => ApplySettings(settings));
                _settingsLevel = WatchVariableControlSettingsManager.GetSettingsLevel();
            }
        }

        private void UpdatePictureBoxes()
        {
            Image currentLockImage = GetImageForCheckState(_watchVarWrapper.GetLockedCheckState(FixedAddressList));
            bool isLocked = currentLockImage != null;
            bool isFixedAddress = FixedAddressList != null;

            if (_lockPictureBox.Image == currentLockImage &&
                _lockPictureBox.Visible == isLocked &&
                _pinPictureBox.Visible == isFixedAddress) return;

            _lockPictureBox.Image = currentLockImage;
            _lockPictureBox.Visible = isLocked;
            _pinPictureBox.Visible = isFixedAddress;

            int pinPadding = isLocked ? PIN_INNER_PADDING : PIN_OUTER_PADDING;
            _pinPictureBox.Location =
                new Point(
                    _variableNameWidth - pinPadding,
                    _pinPictureBox.Location.Y);
        }

        private static Image GetImageForCheckState(CheckState checkState)
        {
            Image image;
            switch (checkState)
            {
                case CheckState.Unchecked:
                    image = null;
                    break;
                case CheckState.Checked:
                    image = _lockedImage;
                    break;
                case CheckState.Indeterminate:
                    image = _someLockedImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (image != null && LockConfig.LockingDisabled)
                image = _disabledLockImage;
            return image;
        }

        private void UpdateFlush()
        {
            if (_rightFlush == SavedSettingsConfig.VariableValuesFlushRight) return;

            _rightFlush = SavedSettingsConfig.VariableValuesFlushRight;

            _valueTextBox.TextAlign = _rightFlush ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            _valueTextBox.Left = _rightFlush ? 0 : VALUE_TEXTBOX_MARGIN;
            _valueCheckBox.CheckAlign = _rightFlush ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
        }

        private void UpdateSize()
        {
            if (_variableNameWidth == VariableNameWidth &&
                _variableValueWidth == VariableValueWidth &&
                _variableHeight == VariableHeight)
                return;

            _variableNameWidth = VariableNameWidth;
            _variableValueWidth = VariableValueWidth;
            _variableHeight = VariableHeight;

            Size = new Size(_variableNameWidth + _variableValueWidth, _variableHeight + 2);
            _tableLayoutPanel.RowStyles[0].Height = _variableHeight;
            _tableLayoutPanel.ColumnStyles[0].Width = _variableNameWidth;
            _tableLayoutPanel.ColumnStyles[1].Width = _variableValueWidth;
            _valueTextBox.Width = _variableValueWidth - VALUE_TEXTBOX_SIZE_DIFF;
        }

        private void UpdateColor()
        {
            Color selectedOrBaseColor = IsSelected ? SELECTED_COLOR : _baseColor;
            if (_isFlashing)
            {
                DateTime currentTime = DateTime.Now;
                double timeSinceFlashStart = currentTime.Subtract(_flashStartTime).TotalMilliseconds;
                if (timeSinceFlashStart < FLASH_DURATION_MS)
                {
                    _currentColor = ColorUtilities.InterpolateColor(
                        _flashColor, selectedOrBaseColor, timeSinceFlashStart / FLASH_DURATION_MS);
                }
                else
                {
                    _currentColor = selectedOrBaseColor;
                    _isFlashing = false;
                }
            }
            else
            {
                _currentColor = selectedOrBaseColor;
            }
            _tableLayoutPanel.BackColor = _currentColor;
            if (!_editMode) _valueTextBox.BackColor = _currentColor;
            if (!_renameMode) _nameTextBox.BackColor = _currentColor;

            Color textColor = IsSelected ? Color.White : Color.Black;
            _valueTextBox.ForeColor = textColor;
            _nameTextBox.ForeColor = textColor;
        }

        public void FlashColor(Color color)
        {
            _flashStartTime = DateTime.Now;
            _flashColor = color;
            _isFlashing = true;
        }





        public bool BelongsToGroup(VariableGroup variableGroup)
        {
            if (variableGroup == VariableGroup.NoGroup)
                return GroupList.Count == 0;
            return GroupList.Contains(variableGroup);
        }

        public bool BelongsToAnyGroup(List<VariableGroup> variableGroups)
        {
            return variableGroups.Any(varGroup => BelongsToGroup(varGroup));
        }

        public bool BelongsToAnyGroupOrHasNoGroup(List<VariableGroup> variableGroups)
        {
            return GroupList.Count == 0 || BelongsToAnyGroup(variableGroups);
        }





        public void ApplySettings(WatchVariableControlSettings settings)
        {
            _watchVarWrapper.ApplySettings(settings);
        }

        public void SetPanel(WatchVariableFlowLayoutPanel panel)
        {
            _watchVariablePanel = panel;
            _selectionContextMenuStrip = panel?.GetSelectionContextMenuStrip();
            if (_selectionContextMenuStrip != null)
            {
                _selectionContextMenuStripItems.Clear();
                foreach (ToolStripItem item in _selectionContextMenuStrip.Items)
                {
                    _selectionContextMenuStripItems.Add(item);
                }
            }
        }

        public void DeleteFromPanel()
        {
            if (_watchVariablePanel == null) return;
            _watchVariablePanel.RemoveVariable(this);
        }

        public void OpenPanelOptions(Point point)
        {
            if (_watchVariablePanel == null) return;
            _watchVariablePanel.ContextMenuStrip.Show(point);
        }

        private static AddToTabTypeEnum GetAddToTabType()
        {
            if (Keyboard.IsKeyDown(Key.A)) return AddToTabTypeEnum.IndividualSpliced;
            if (Keyboard.IsKeyDown(Key.G)) return AddToTabTypeEnum.IndividualGrouped;
            if (Keyboard.IsKeyDown(Key.F)) return AddToTabTypeEnum.Fixed;
            return AddToTabTypeEnum.Regular;
        }

        public void AddToTab(DataManager dataManager, AddToTabTypeEnum? addToTabTypeNullable = null)
        {
            AddVarsToTab(new List<WatchVariableControl>() { this }, dataManager, addToTabTypeNullable);
        }

        public static void AddVarsToTab(
            List<WatchVariableControl> watchVars, DataManager dataManager, AddToTabTypeEnum? addToTabTypeNullable = null)
        {
            List<List<WatchVariableControl>> newVarListList = new List<List<WatchVariableControl>>();
            AddToTabTypeEnum addToTabType = addToTabTypeNullable ?? GetAddToTabType();
            
            foreach (WatchVariableControl watchVar in watchVars)
            {
                List<WatchVariableControl> newVarList = new List<WatchVariableControl>();
                List<uint> addressList = watchVar.FixedAddressList ?? watchVar._watchVarWrapper.GetCurrentAddresses();
                List<List<uint>> addressesLists =
                    addToTabType == AddToTabTypeEnum.IndividualSpliced
                            || addToTabType == AddToTabTypeEnum.IndividualGrouped
                        ? addressList.ConvertAll(address => new List<uint>() { address })
                        : new List<List<uint>>() { addressList };
                for (int i = 0; i < addressesLists.Count; i++)
                {
                    string name = watchVar.VarName;
                    if (addressesLists.Count > 1) name += " " + (i + 1);
                    bool useFixed =
                        addToTabType == AddToTabTypeEnum.Fixed ||
                        addToTabType == AddToTabTypeEnum.IndividualSpliced ||
                        addToTabType == AddToTabTypeEnum.IndividualGrouped;
                    List<uint> constructorAddressList = useFixed ? addressesLists[i] : null;
                    WatchVariableControl newControl =
                        watchVar.WatchVarPrecursor.CreateWatchVariableControl(
                            name,
                            watchVar._baseColor,
                            new List<VariableGroup>() { VariableGroup.Custom },
                            constructorAddressList);
                    newVarList.Add(newControl);
                }
                watchVar.FlashColor(ADD_TO_CUSTOM_TAB_COLOR);
                newVarListList.Add(newVarList);
            }

            if (addToTabType == AddToTabTypeEnum.IndividualGrouped)
            {
                int maxListLength = newVarListList.Max(list => list.Count);
                for (int i = 0; i < maxListLength; i++)
                {
                    for (int j = 0; j < newVarListList.Count; j++)
                    {
                        List<WatchVariableControl> newVarList = newVarListList[j];
                        if (i >= newVarList.Count) continue;
                        WatchVariableControl newVar = newVarList[i];
                        dataManager.AddVariable(newVar);
                    }
                }
            }
            else
            {
                foreach (List<WatchVariableControl> newVarList in newVarListList)
                {
                    foreach (WatchVariableControl newVar in newVarList)
                    {
                        dataManager.AddVariable(newVar);
                    }
                }
            }
        }

        public void AddToVarHackTab()
        {
            _watchVarWrapper.AddToVarHackTab(FixedAddressList);
            FlashColor(ADD_TO_VAR_HACK_TAB_COLOR);
        }

        public void EnableCustomFunctionality()
        {
            _watchVarWrapper.EnableCustomFunctionality();
            FlashColor(ENABLE_CUSTOM_FUNCIONALITY_COLOR);
        }

        public void NotifyPanelOfReodering()
        {
            _watchVariablePanel.NotifyOfReordering(this);
        }

        public void ToggleFixedAddress()
        {
            if (FixedAddressList == null)
            {
                FixedAddressList = _watchVarWrapper.GetCurrentAddresses();
            }
            else
            {
                FixedAddressList = null;
            }
        }

        public void ToggleHighlighted(Color? color = null)
        {
            if (color.HasValue)
            {
                if (_tableLayoutPanel.ShowBorder)
                {
                    if (_tableLayoutPanel.BorderColor == color.Value)
                    {
                        _tableLayoutPanel.ShowBorder = false;
                    }
                    else
                    {
                        _tableLayoutPanel.BorderColor = color.Value;
                    }
                }
                else
                {
                    _tableLayoutPanel.BorderColor = color.Value;
                    _tableLayoutPanel.ShowBorder = true;
                }
            }
            else
            {
                if (_tableLayoutPanel.ShowBorder)
                {
                    _tableLayoutPanel.ShowBorder = false;
                }
                else
                {
                    _tableLayoutPanel.BorderColor = Color.Red;
                    _tableLayoutPanel.ShowBorder = true;
                }
            }
        }

        public object GetValue(bool useRounding)
        {
            return _watchVarWrapper.GetValue(useRounding);
        }

        public void SetValue(string value)
        {
            bool success = _watchVarWrapper.SetValue(value, FixedAddressList);
            if (!success) FlashColor(FAILURE_COLOR);
        }

        public void AddValue(string value, bool add)
        {
            bool success = _watchVarWrapper.AddValue(value, add, FixedAddressList);
            if (!success) FlashColor(FAILURE_COLOR);
        }

        public XElement ToXml(bool useCurrentState = true)
        {
            Color? color = _baseColor == DEFAULT_COLOR ? (Color?)null : _baseColor;
            if (useCurrentState)
                return WatchVarPrecursor.ToXML(
                    VarName, color, GroupList, FixedAddressList);
            else
                return WatchVarPrecursor.ToXML();
        }

        public List<string> GetVarInfo()
        {
            return _watchVarWrapper.GetVarInfo();
        }

        public override string ToString()
        {
            return WatchVarPrecursor.ToString();
        }
    }
}
