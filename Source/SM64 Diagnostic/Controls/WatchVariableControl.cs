using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;
using System.Reflection;
using SM64_Diagnostic.Managers;
using static SM64_Diagnostic.Structs.WatchVariable;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Controls
{
    public class WatchVariableControl : IDataContainer
    {
        BorderedTableLayoutPanel _tablePanel;
        Label _nameLabel;
        WatchVariable _watchVar;
        CheckBox _checkBoxBool;
        TextBox _textBoxValue;
        string _specialName;

        bool _changedByUser = true;
        bool _editMode = false;

        static Image _lockedImage = new Bitmap(Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("SM64_Diagnostic.EmbeddedResources.lock.png")), new Size(16, 16));
        static Image _someLockedImage = _lockedImage.GetOpaqueImage(0.5f);

        static WatchVariableControl _lastSelected;

        public enum AngleViewModeType { Recommended, Signed, Unsigned, Degrees, Radians };

        AngleViewModeType _angleViewMode = AngleViewModeType.Recommended;
        Boolean _angleTruncated = false;

        private static ContextMenuStrip _menu;
        public static ContextMenuStrip Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new ContextMenuStrip();
                    _menu.Items.Add("Edit");
                    var newItem = new ToolStripMenuItem("View As Hexadecimal");
                    newItem.Name = "HexView";
                    _menu.Items.Add(newItem);
                    newItem = new ToolStripMenuItem("Lock Value");
                    newItem.Name = "LockValue";
                    _menu.Items.Add(newItem);
                    newItem = new ToolStripMenuItem("Highlight");
                    newItem.Name = "Highlight";
                    _menu.Items.Add(newItem);
                }
                return _menu;
            }
        }

        private static ContextMenuStrip _angleMenu;
        public static ContextMenuStrip AngleMenu
        {
            get
            {
                if (_angleMenu == null)
                {
                    _angleMenu = new ContextMenuStrip();
                    _angleMenu.Items.Add("Edit");
                    var newItem = new ToolStripMenuItem("View As Hexadecimal");
                    newItem.Name = "HexView";
                    _angleMenu.Items.Add(newItem);
                    newItem = new ToolStripMenuItem("Lock Value");
                    newItem.Name = "LockValue";
                    _angleMenu.Items.Add(newItem);
                    newItem = new ToolStripMenuItem("Highlight");
                    newItem.Name = "Highlight";
                    _angleMenu.Items.Add(newItem);

                    _angleMenu.Items.Add(AngleDropDownMenu[0]);
                    _angleMenu.Items.Add(AngleDropDownMenu[1]);
                }
                return _angleMenu;
            }
        }

        private static ContextMenuStrip _checkMenu;
        public static ContextMenuStrip CheckMenu
        {
            get
            {
                if (_checkMenu == null)
                {
                    _checkMenu = new ContextMenuStrip();
                    var newItem = new ToolStripMenuItem("Highlight");
                    newItem.Name = "Highlight";
                    _checkMenu.Items.Add(newItem);
                }
                return _checkMenu;
            }
        }

        private static ToolStripMenuItem[] _angleMenuDropDown;
        public static ToolStripMenuItem[] AngleDropDownMenu
        {
            get
            {
                if (_angleMenuDropDown == null)
                {
                    _angleMenuDropDown = new ToolStripMenuItem[2];
                    _angleMenuDropDown[0] = new ToolStripMenuItem("View Angle As");
                    _angleMenuDropDown[0].DropDownItems.Add("Recommended");
                    _angleMenuDropDown[0].DropDownItems.Add("Unsigned (short)");
                    _angleMenuDropDown[0].DropDownItems.Add("Signed (short)");
                    _angleMenuDropDown[0].DropDownItems.Add("Degrees");
                    _angleMenuDropDown[0].DropDownItems.Add("Radians");
                    _angleMenuDropDown[1] = new ToolStripMenuItem("Truncate Angle (by 16)");
                }
                return _angleMenuDropDown;
            }
        }

        private static List<ToolStripMenuItem> _objectDropDownMenu;
        public static List<ToolStripMenuItem> ObjectDropDownMenu
        {
            get
            {
                if (_objectDropDownMenu == null)
                {
                    _objectDropDownMenu = new List<ToolStripMenuItem>();
                    _objectDropDownMenu.Add(new ToolStripMenuItem("Select Object"));
                }
                return _objectDropDownMenu;
            }
        }

        static ToolTip _toolTip;
        public static ToolTip AddressToolTip
        {
            get
            {
                if (_toolTip == null)
                {
                    _toolTip = new ToolTip();
                    _toolTip.IsBalloon = true;
                    _toolTip.ShowAlways = true;
                }
                return _toolTip;
            }
            set
            {
                _toolTip = value;
            }
        }

        public static readonly List<uint> OffsetListZero = new List<uint> { 0 };

        private List<uint> GetOffsetListFromOffsetType(OffsetType? offsetType, bool nonEmptyList = true) //TODO make static once stream is config var
        {
            List<uint> output;
            switch (offsetType)
            {
                case OffsetType.Absolute:
                    output = OffsetListZero;
                    break;
                case OffsetType.Relative:
                    output = OffsetListZero;
                    break;
                case OffsetType.Mario:
                    output = new List<uint> { Config.Mario.StructAddress };
                    break;
                case OffsetType.MarioObj:
                    output = new List<uint> { Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress) };
                    break;
                case OffsetType.Camera:
                    output = new List<uint> { Config.Camera.CameraStructAddress };
                    break;
                case OffsetType.File:
                    output = new List<uint> { FileManager.Instance.CurrentFileAddress };
                    break;
                case OffsetType.Object:
                    output = ObjectManager.Instance.CurrentAddresses;
                    break;
                case OffsetType.Triangle:
                    output = new List<uint> { TriangleManager.Instance.TriangleAddress };
                    break;
                case OffsetType.InputCurrent:
                    output = new List<uint> { Config.Input.CurrentInputAddress };
                    break;
                case OffsetType.InputBuffered:
                    output = new List<uint> { Config.Input.BufferedInputAddress };
                    break;
                case OffsetType.Graphics:
                    output = GetOffsetListFromOffsetType(OffsetType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.BehaviorGfxOffset));
                    break;
                case OffsetType.Animation:
                    output = GetOffsetListFromOffsetType(OffsetType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.AnimationOffset));
                    break;
                case OffsetType.Waypoint:
                    output = GetOffsetListFromOffsetType(OffsetType.Object, false)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.WaypointOffset));
                    break;
                case OffsetType.Water:
                    output = new List<uint> { Config.Stream.GetUInt32(Config.WaterPointerAddress) };
                    break;
                case OffsetType.HackedArea:
                    output = new List<uint> { Config.HackedAreaAddress };
                    break;
                case OffsetType.CamHack:
                    output = new List<uint> { Config.CameraHack.CameraHackStruct };
                    break;
                case OffsetType.Special:
                    throw new ArgumentOutOfRangeException("Should not get offset list for Special var");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (nonEmptyList && output.Count == 0)
            {
                output = OffsetListZero;
            }
            return output;
        }

        public List<uint> OffsetList
        {
            get
            {
                return GetOffsetListFromOffsetType(_watchVar.Offset);
            }
        }

        public string Name
        {
            get
            {
                return _nameLabel.Text;
            }
            set
            {
                _nameLabel.Text = value;
            }
        }

        public string SpecialName
        {
            get
            {
                return _specialName;
            }
            set
            {
                _specialName = value;
            }
        }

        public string Value
        {
            get
            {
                if (_textBoxValue == null)
                    return "";

                return _textBoxValue.Text;
            }
            set
            {
                if (_textBoxValue == null)
                    return;

                _textBoxValue.Text = value;
            }
        }

        public Color Color
        {
            get
            {
                return Control.BackColor;
            }
            set
            {
                Control.BackColor = value;
                if (!_watchVar.IsBool)
                    _textBoxValue.BackColor = Color;
                else
                    _checkBoxBool.BackColor = Color;
            }
        }

        public Control Control
        {
            get
            {
                return _tablePanel;
            }
        }

        public WatchVariable WatchVariable
        {
            get
            {
                return _watchVar;
            }
        }

        Image _lastLockedImage = null;
        private void ShowLockedImage(bool show, bool transparent = false)
        {
            Image nextImage = null;
            if (show)
                nextImage = transparent ? _someLockedImage : _lockedImage;

            if (_lastLockedImage == nextImage)
                return;

            _lastLockedImage = nextImage;
            _nameLabel.Image = nextImage;
        }

        public bool EditMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                _editMode = value;
                if (_textBoxValue != null)
                {
                    _textBoxValue.ReadOnly = !_editMode;
                    _textBoxValue.BackColor = _editMode ? Color.White : Color == Color.Transparent ? SystemColors.Control : Color;
                    if (_editMode)
                    {
                        _textBoxValue.Focus();
                        _textBoxValue.SelectAll();
                    }
                }
            }
        }

        public WatchVariableControl(WatchVariable watchVar)
        {
            _specialName = watchVar.Name;
            _watchVar = watchVar;

            CreateControls();

            if (watchVar.BackroundColor.HasValue)
                Color = watchVar.BackroundColor.Value;
        }

        public WatchVariableLock GetVariableLock(uint offset)
        {
            var lockCriteria = new WatchVariableLock(_watchVar.GetRamAddress(offset, false), new byte[_watchVar.ByteCount]);

            if (!Config.Stream.LockedVariables.ContainsKey(lockCriteria))
                return null;

            return Config.Stream.LockedVariables[lockCriteria];
        }

        private void CreateControls()
        {
            this._nameLabel = new Label();
            this._nameLabel.Size = new Size(210, 20); //TODO check this
            this._nameLabel.Text = _watchVar.Name;
            this._nameLabel.Margin = new Padding(3, 3, 3, 3);
            this._nameLabel.Click += _nameLabel_Click;
            this._nameLabel.ImageAlign = ContentAlignment.MiddleRight;
            this._nameLabel.MouseHover += (sender, e) =>
            {
                if (!_watchVar.HasAdditiveOffset)
                {
                    AddressToolTip.SetToolTip(this._nameLabel, String.Format("0x{0:X8} [{2} + 0x{1:X8}]",
                        _watchVar.GetRamAddress(), _watchVar.GetProcessAddress(), Config.Stream.ProcessName));
                }
                else
                {
                    AddressToolTip.SetToolTip(this._nameLabel, String.Format("0x{1:X8} + 0x{0:X8} = 0x{2:X8} [{4} + 0x{3:X8}]",
                        _watchVar.GetRamAddress(0, false), OffsetList[0], _watchVar.GetRamAddress(OffsetList[0]),
                        _watchVar.GetProcessAddress(OffsetList[0]), Config.Stream.ProcessName));
                }
            };

            if (_watchVar.IsBool)
            {
                this._checkBoxBool = new CheckBox();
                this._checkBoxBool.CheckAlign = ContentAlignment.MiddleRight;
                this._checkBoxBool.CheckedChanged += OnEdited;
                this._checkBoxBool.MouseEnter += (sender, e) =>
                {
                    _lastSelected = this;
                    (CheckMenu.Items["Highlight"] as ToolStripMenuItem).Checked = _tablePanel.ShowBorder;
                };
                this._checkBoxBool.ContextMenuStrip = WatchVariableControl.CheckMenu;
            }
            else
            {
                this._textBoxValue = new TextBox();
                this._textBoxValue.ReadOnly = true;
                this._textBoxValue.BorderStyle = BorderStyle.None;
                this._textBoxValue.TextAlign = HorizontalAlignment.Right;
                this._textBoxValue.Width = 200;
                this._textBoxValue.Margin = new Padding(6, 3, 6, 3);
                this._textBoxValue.TextChanged += OnEdited;
                this._textBoxValue.ContextMenuStrip = _watchVar.IsAngle ? WatchVariableControl.AngleMenu : WatchVariableControl.Menu;
                this._textBoxValue.KeyDown += OnTextValueKeyDown;
                this._textBoxValue.MouseEnter += _textBoxValue_MouseEnter;
                this._textBoxValue.DoubleClick += _textBoxValue_DoubleClick;
                this._textBoxValue.Leave += (sender, e) => { EditMode = false; };
                if (_watchVar.IsAngle)
                {
                    WatchVariableControl.AngleMenu.ItemClicked += OnMenuStripClick;
                    WatchVariableControl.AngleDropDownMenu[0].DropDownItemClicked += AngleDropDownMenu_DropDownItemClicked;
                    WatchVariableControl.AngleDropDownMenu[1].Click += TruncateAngleMenu_ItemClicked;
                }
                else
                    WatchVariableControl.Menu.ItemClicked += OnMenuStripClick;
            }

            this._tablePanel = new BorderedTableLayoutPanel();
            this._tablePanel.Size = new Size(230, _nameLabel.Height + 2);
            this._tablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this._tablePanel.RowCount = 1;
            this._tablePanel.ColumnCount = 2;
            this._tablePanel.RowStyles.Clear();
            this._tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, _nameLabel.Height + 3));
            this._tablePanel.ColumnStyles.Clear();
            this._tablePanel.Margin = new Padding(0);
            this._tablePanel.Padding = new Padding(0);
            this._tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            this._tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            this._tablePanel.ShowBorder = false;
            this._tablePanel.Controls.Add(_nameLabel, 0, 0);
            this._tablePanel.Controls.Add(_watchVar.IsBool ? this._checkBoxBool as Control : this._textBoxValue, 1, 0);
        }

        private void _nameLabel_Click(object sender, EventArgs e)
        {
            VariableViewerForm varInfo;
            var typeDescr = _watchVar.TypeName;
            if (_watchVar.Mask.HasValue)
            {
                typeDescr += String.Format(" w/ mask: 0x{0:X" + _watchVar.ByteCount * 2 + "}", _watchVar.Mask);
            }

            if (!_watchVar.HasAdditiveOffset)
            {
                varInfo = new VariableViewerForm(_watchVar.Name, typeDescr,
                    String.Format("0x{0:X8}", _watchVar.GetRamAddress()),
                    String.Format("0x{0:X8}", _watchVar.GetProcessAddress().ToUInt64()));
            }
            else
            {
                varInfo = new VariableViewerForm(_watchVar.Name, typeDescr,
                    String.Format("0x{0:X8}", _watchVar.GetRamAddress(OffsetList[0])),
                    String.Format("0x{0:X8}", _watchVar.GetProcessAddress(OffsetList[0]).ToUInt64()));
            }
            varInfo.ShowDialog();
        }

        private void _textBoxValue_DoubleClick(object sender, EventArgs e)
        {
            EditMode = true;
        }

        private void _textBoxValue_MouseEnter(object sender, EventArgs e)
        {
            var lockedStatus = CheckState.Unchecked;
            if (OffsetList.Any(o => GetIsLocked(o)))
            {
                if (OffsetList.All(o => GetIsLocked(o)))
                {
                    lockedStatus = CheckState.Checked;
                }
                else
                {
                    lockedStatus = CheckState.Indeterminate;
                }
            }

            _lastSelected = this;
            if (_watchVar.IsAngle)
            {
                (AngleMenu.Items["HexView"] as ToolStripMenuItem).Checked = _watchVar.UseHex;
                (AngleMenu.Items["LockValue"] as ToolStripMenuItem).CheckState = lockedStatus;
                (AngleMenu.Items["Highlight"] as ToolStripMenuItem).Checked = _tablePanel.ShowBorder;
                (AngleDropDownMenu[0].DropDownItems[0] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Recommended);
                (AngleDropDownMenu[0].DropDownItems[1] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Unsigned);
                (AngleDropDownMenu[0].DropDownItems[2] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Signed);
                (AngleDropDownMenu[0].DropDownItems[3] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Degrees);
                (AngleDropDownMenu[0].DropDownItems[4] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Radians);
                (AngleDropDownMenu[1] as ToolStripMenuItem).Checked = _angleTruncated;
            }
            else
            {
                (Menu.Items["HexView"] as ToolStripMenuItem).Checked = _watchVar.UseHex;
                (Menu.Items["LockValue"] as ToolStripMenuItem).CheckState = lockedStatus;
                (Menu.Items["Highlight"] as ToolStripMenuItem).Checked = _tablePanel.ShowBorder;
                ObjectDropDownMenu.ForEach(d => Menu.Items.Remove(d));
                if (_watchVar.IsObject)
                {
                    ObjectDropDownMenu.ForEach(d => Menu.Items.Add(d));
                }
            }
        }

        private void AngleDropDownMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this != _lastSelected)
                return;

            switch (e.ClickedItem.Text)
            {
                case "Recommended":
                    _angleViewMode = AngleViewModeType.Recommended;
                    break;
                case "Unsigned (short)":
                    _angleViewMode = AngleViewModeType.Unsigned;
                    break;
                case "Signed (short)":
                    _angleViewMode = AngleViewModeType.Signed;
                    break;
                case "Degrees":
                    _angleViewMode = AngleViewModeType.Degrees;
                    break;
                case "Radians":
                    _angleViewMode = AngleViewModeType.Radians;
                    break;
            }
        }

        private void TruncateAngleMenu_ItemClicked(object sender, EventArgs e)
        {
            if (this != _lastSelected)
                return;

            _angleTruncated = !_angleTruncated;
        }

        public void Update()
        {
            if (_watchVar.IsSpecial)
                return;

            ShowLockedImage(OffsetList.Any(o => GetIsLocked(o)), !OffsetList.All(o => GetIsLocked(o)));

            if (_editMode)
                return;

            _changedByUser = false;

            if (_watchVar.IsBool)
            {
                if (OffsetList.Any(o => _watchVar.GetBoolValue(o)))
                {
                    if (OffsetList.All(o => _watchVar.GetBoolValue(o)))
                    {
                        _checkBoxBool.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        _checkBoxBool.CheckState = CheckState.Indeterminate;
                    }
                }
                else
                {
                    _checkBoxBool.CheckState = CheckState.Unchecked;
                }
            }
            else
            {
                bool firstOffset = true;
                foreach (var offset in OffsetList)
                {
                    string newText = "";
                    if (_watchVar.IsAngle)
                    {
                        newText = _watchVar.GetAngleStringValue(offset, _angleViewMode, _angleTruncated);
                    }
                    else
                    {
                        newText = _watchVar.GetStringValue(offset);
                    }

                    if (firstOffset)
                    {
                        _textBoxValue.Text = newText;
                    }
                    else if (_textBoxValue.Text != newText)
                    {
                        _textBoxValue.Text = "";
                        continue;
                    }

                    firstOffset = false;
                }
            }

            _changedByUser = true;
        }

        private void OnEdited(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            if (_watchVar.IsBool)
            {
                foreach (var offset in OffsetList)
                {
                    _watchVar.SetBoolValue(offset, _checkBoxBool.Checked);
                }
            }
        }

        private void OnMenuStripClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this != _lastSelected)
                return;

            switch (e.ClickedItem.Text)
            {
                case "Edit":
                    EditMode = true;
                    break;
                case "View As Hexadecimal":
                    _watchVar.UseHex = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    (e.ClickedItem as ToolStripMenuItem).Checked = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    break;
                case "Lock Value":
                    EditMode = false;
                    (e.ClickedItem as ToolStripMenuItem).Checked = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    if (OffsetList.Any(o => GetIsLocked(o)))
                        OffsetList.ForEach(o => RemoveLock(o));
                    else
                        OffsetList.ForEach(o => LockUpdate(o));
                    break;
                case "Select Object":
                    if (_watchVar.ByteCount != 4)
                        return;

                    var slotManager = ManagerContext.Current.ObjectSlotManager;
                    slotManager.SelectedSlotsAddresses.Clear();
                    foreach (var otherOffset in OffsetList)
                    {
                        var objAddress = BitConverter.ToUInt32(_watchVar.GetByteData(otherOffset), 0);
                        if (ManagerContext.Current.ObjectSlotManager.ObjectSlots.Count(s => s.Address == objAddress) > 0)
                            slotManager.SelectedSlotsAddresses.Add(objAddress);
                    }
                    break;
                case "Highlight":
                    var toolItem = (e.ClickedItem as ToolStripMenuItem);
                    toolItem.Checked = !toolItem.Checked;
                    _tablePanel.ShowBorder = toolItem.Checked;
                    break;
            }
        }

        private void OnTextValueKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                // Exit edit mode
                EditMode = false;
                return;
            }

            // On "Enter" key press
            if (e.KeyData != Keys.Enter)
                return;

            // Exit edit mode
            EditMode = false;

            Config.Stream.Suspend();

            // Write new value to RAM
            byte[] writeBytes;
            foreach (var offset in OffsetList)
            {
                if (_watchVar.IsAngle)
                {
                    writeBytes = _watchVar.GetBytesFromAngleString(_textBoxValue.Text, _angleViewMode);
                }
                else
                {
                    writeBytes = _watchVar.GetBytesFromString(offset, _textBoxValue.Text);
                }
                _watchVar.SetBytes(offset, writeBytes);

                // Update locked value
                if (GetIsLocked(offset))
                    LockUpdate(offset, writeBytes);
            }

            Config.Stream.Resume();
        }

        public bool GetIsLocked(uint offset)
        {
            return GetVariableLock(offset) != null;
        }

        private bool RemoveLock(uint offset)
        {
            WatchVariableLock removed;
            var lockedVar = GetVariableLock(offset);
            if (lockedVar != null)
                return Config.Stream.LockedVariables.TryRemove(lockedVar, out removed);

            return true;
        }

        private void LockUpdate(uint offset, byte[] lockedBytes = null)
        {
            if (lockedBytes == null)
                lockedBytes = _watchVar.GetByteData(offset);

            var lockedVar = new WatchVariableLock(_watchVar.GetRamAddress(offset, false), lockedBytes);
            Config.Stream.LockedVariables[lockedVar] = lockedVar;
        }
    }
}
