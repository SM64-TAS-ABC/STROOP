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
using static SM64_Diagnostic.Structs.VarXUtilities;

namespace SM64_Diagnostic.Controls
{
    public class VarX
    {
        public readonly AddressHolder AddressHolder;
        public uint Address { get { return AddressHolder.Address; } }

        public readonly OffsetType Offset;
        public readonly string Name;
        public readonly string SpecialType;
        public readonly ulong? Mask;
        public readonly bool IsBool;
        public readonly bool IsObject;
        public readonly bool IsAngle;
        public readonly Color? BackroundColor;
        public readonly List<VariableGroup> GroupList;
        public readonly string TypeName;
        public readonly Type Type;
        public readonly int ByteCount;

        public bool UseHex;
        public bool InvertBool;

        public VarX(
            string name,
            OffsetType offset,
            List<VariableGroup> groupList,
            string specialType,
            Color? backgroundColor,
            AddressHolder addressHolder,
            bool useHex,
            ulong? mask,
            bool isBool,
            bool isObject,
            string typeName,
            bool invertBool,
            bool isAngle)
        {
            Name = name;
            Offset = offset;
            GroupList = groupList;
            SpecialType = specialType;
            BackroundColor = backgroundColor;

            if (IsSpecial) return;

            AddressHolder = addressHolder;
            UseHex = useHex;
            Mask = mask;
            IsBool = isBool;
            IsObject = isObject;
            InvertBool = invertBool;
            IsAngle = isAngle;

            TypeName = typeName;
            Type = StringToType[TypeName];
            ByteCount = TypeSize[Type];


            CreateControls();
            if (BackroundColor.HasValue)
                Color = BackroundColor.Value;
        }

        public bool IsSpecial
        {
            get
            {
                return Offset == OffsetType.Special;
            }
        }














        BorderedTableLayoutPanel _tablePanel;
        Label _nameLabel;
        CheckBox _checkBoxBool;
        TextBox _textBoxValue;

        bool _changedByUser = true;
        bool _editMode = false;

        static Image _lockedImage = new Bitmap(Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("SM64_Diagnostic.EmbeddedResources.lock.png")), new Size(16, 16));
        static Image _someLockedImage = _lockedImage.GetOpaqueImage(0.5f);

        static VarX _lastSelected;

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

        private static List<ToolStripMenuItem> _scriptDropDownMenu;
        public static List<ToolStripMenuItem> ScriptDropDownMenu
        {
            get
            {
                if (_scriptDropDownMenu == null)
                {
                    _scriptDropDownMenu = new List<ToolStripMenuItem>();
                    _scriptDropDownMenu.Add(new ToolStripMenuItem("View Script"));
                }
                return _scriptDropDownMenu;
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

        private CheckState _checkBoxCheckState = CheckState.Unchecked;
        public CheckState CheckBoxCheckState
        {
            get
            {
                return _checkBoxCheckState;
            }
            set
            {
                if (_checkBoxCheckState == value)
                    return;
                _checkBoxCheckState = value;
                _checkBoxBool.CheckState = value;
            }
        }

        public List<uint> OffsetList
        {
            get
            {
                return GetOffsetListFromOffsetType(Offset);
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
                if (!IsBool)
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

        /*
        public WatchVariableLock GetVariableLock(uint offset)
        {
            var lockCriteria = new WatchVariableLock(_watchVar.GetRamAddress(offset, false), new byte[_watchVar.ByteCount]);

            if (!Config.Stream.LockedVariables.ContainsKey(lockCriteria))
                return null;

            return Config.Stream.LockedVariables[lockCriteria];
        }
        */

        private void CreateControls()
        {
            this._nameLabel = new Label();
            this._nameLabel.Size = new Size(210, 20); //TODO check this
            this._nameLabel.Text = Name;
            this._nameLabel.Margin = new Padding(3, 3, 3, 3);
            this._nameLabel.Click += _nameLabel_Click;
            this._nameLabel.ImageAlign = ContentAlignment.MiddleRight;
            this._nameLabel.MouseHover += (sender, e) =>
            {
                if (!AddressHolder.HasAdditiveOffset)
                {
                    AddressToolTip.SetToolTip(this._nameLabel, "TODO 1" /*String.Format("0x{0:X8} [{2} + 0x{1:X8}]",
                        _watchVar.GetRamAddress(), _watchVar.GetProcessAddress(), Config.Stream.ProcessName)*/);
                }
                else
                {
                    AddressToolTip.SetToolTip(this._nameLabel, "TODO 2" /*String.Format("0x{1:X8} + 0x{0:X8} = 0x{2:X8} [{4} + 0x{3:X8}]",
                        _watchVar.GetRamAddress(0, false), OffsetList[0], _watchVar.GetRamAddress(OffsetList[0]),
                        _watchVar.GetProcessAddress(OffsetList[0]), Config.Stream.ProcessName)*/);
                }
            };

            if (IsBool)
            {
                this._checkBoxBool = new CheckBox();
                this._checkBoxBool.CheckAlign = ContentAlignment.MiddleRight;
                this._checkBoxBool.CheckState = CheckState.Unchecked;
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
                this._textBoxValue.ContextMenuStrip = IsAngle ? WatchVariableControl.AngleMenu : WatchVariableControl.Menu;
                this._textBoxValue.KeyDown += OnTextValueKeyDown;
                this._textBoxValue.MouseEnter += _textBoxValue_MouseEnter;
                this._textBoxValue.DoubleClick += _textBoxValue_DoubleClick;
                this._textBoxValue.Leave += (sender, e) => { EditMode = false; };
                if (IsAngle)
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
            this._tablePanel.Controls.Add(IsBool ? this._checkBoxBool as Control : this._textBoxValue, 1, 0);
        }

        private void _nameLabel_Click(object sender, EventArgs e)
        {
            VariableViewerForm varInfo;
            var typeDescr = TypeName;
            if (Mask.HasValue)
            {
                typeDescr += String.Format(" w/ mask: 0x{0:X" + ByteCount * 2 + "}", Mask);
            }

            if (!AddressHolder.HasAdditiveOffset)
            {
                varInfo = new VariableViewerForm(Name, typeDescr,
                    String.Format("0x{0:X8}", AddressHolder.GetRamAddress()),
                    String.Format("0x{0:X8}", AddressHolder.GetProcessAddress().ToUInt64()));
            }
            else
            {
                varInfo = new VariableViewerForm(Name, typeDescr,
                    String.Format("0x{0:X8}", AddressHolder.GetRamAddress()),
                    String.Format("0x{0:X8}", AddressHolder.GetProcessAddress().ToUInt64()));
            }
            varInfo.ShowDialog();
        }

        private void _textBoxValue_DoubleClick(object sender, EventArgs e)
        {
            EditMode = true;
        }

        private void _textBoxValue_MouseEnter(object sender, EventArgs e)
        {
            /*
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
            */

            _lastSelected = this;
            if (IsAngle)
            {
                (AngleMenu.Items["HexView"] as ToolStripMenuItem).Checked = UseHex;
                /*(AngleMenu.Items["LockValue"] as ToolStripMenuItem).CheckState = lockedStatus;*/
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
                (Menu.Items["HexView"] as ToolStripMenuItem).Checked = UseHex;
                /*(Menu.Items["LockValue"] as ToolStripMenuItem).CheckState = lockedStatus;*/
                (Menu.Items["Highlight"] as ToolStripMenuItem).Checked = _tablePanel.ShowBorder;
                ObjectDropDownMenu.ForEach(d => Menu.Items.Remove(d));
                if (IsObject)
                {
                    ObjectDropDownMenu.ForEach(d => Menu.Items.Add(d));
                }
                ScriptDropDownMenu.ForEach(d => Menu.Items.Remove(d));
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
            if (IsSpecial)
                return;

            /*
            ShowLockedImage(OffsetList.Any(o => GetIsLocked(o)), !OffsetList.All(o => GetIsLocked(o)));
            */

            if (_editMode)
                return;

            _changedByUser = false;

            if (IsBool)
            {
                if (OffsetList.Any(o => GetBoolValue(o)))
                {
                    if (OffsetList.All(o => GetBoolValue(o)))
                    {
                        CheckBoxCheckState = CheckState.Checked;
                    }
                    else
                    {
                        CheckBoxCheckState = CheckState.Indeterminate;
                    }
                }
                else
                {
                    CheckBoxCheckState = CheckState.Unchecked;
                }
            }
            else
            {
                bool firstOffset = true;
                foreach (var offset in OffsetList)
                {
                    string newText = "";
                    if (IsAngle)
                    {
                        newText = GetAngleStringValue(offset, _angleViewMode, _angleTruncated);
                    }
                    else
                    {
                        newText = GetStringValue(offset);
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

            if (IsBool)
            {
                foreach (var offset in OffsetList)
                {
                    SetBoolValue(offset, _checkBoxBool.Checked);
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
                    UseHex = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    (e.ClickedItem as ToolStripMenuItem).Checked = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    break;
                    /*
                case "Lock Value":
                    EditMode = false;
                    (e.ClickedItem as ToolStripMenuItem).Checked = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    if (OffsetList.Any(o => GetIsLocked(o)))
                        OffsetList.ForEach(o => RemoveLock(o));
                    else
                        OffsetList.ForEach(o => LockUpdate(o));
                    break;
                    */
                    /*
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
                    */
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
                if (IsAngle)
                {
                    writeBytes = GetBytesFromAngleString(_textBoxValue.Text, _angleViewMode);
                }
                else
                {
                    writeBytes = GetBytesFromString(offset, _textBoxValue.Text);
                }
                SetBytes(offset, writeBytes);

                // Update locked value
                /*
                if (GetIsLocked(offset))
                    LockUpdate(offset, writeBytes);
                */
            }

            Config.Stream.Resume();
        }

        /*
        public bool GetIsLocked(uint offset)
        {
            return GetVariableLock(offset) != null;
        }
        */

        /*
        private bool RemoveLock(uint offset)
        {
            WatchVariableLock removed;
            var lockedVar = GetVariableLock(offset);
            if (lockedVar != null)
                return Config.Stream.LockedVariables.TryRemove(lockedVar, out removed);

            return true;
        }
        */

        /*
        private void LockUpdate(uint offset, byte[] lockedBytes = null)
        {
            if (lockedBytes == null)
                lockedBytes = _watchVar.GetByteData(offset);

            var lockedVar = new WatchVariableLock(_watchVar.GetRamAddress(offset, false), lockedBytes);
            Config.Stream.LockedVariables[lockedVar] = lockedVar;
        }
        */



















        public byte[] GetByteData(uint offset)
        {
            // Get dataBytes
            var dataBytes = Config.Stream.ReadRamLittleEndian(AddressHolder.HasAdditiveOffset ? new UIntPtr(offset + Address)
                : new UIntPtr(Address), ByteCount, AddressHolder.UseAbsoluteAddressing);

            // Make sure offset is a valid pointer
            if (AddressHolder.HasAdditiveOffset && offset == 0)
                return null;

            return dataBytes;
        }

        public string GetStringValue(uint offset)
        {
            // Get dataBytes
            var dataBytes = GetByteData(offset);

            // Make sure offset is a valid pointer
            if (dataBytes == null)
                return "(none)";

            // Parse object type
            if (IsObject)
            {
                var objAddress = BitConverter.ToUInt32(dataBytes, 0);
                if (objAddress == 0)
                    return "(none)";

                var slotName = ManagerContext.Current.ObjectSlotManager.GetSlotNameFromAddress(objAddress);
                if (slotName != null)
                    return "Slot: " + slotName;
            }

            // Parse floating point
            if (!UseHex && (Type == typeof(float) || Type == typeof(double)))
            {
                if (Type == typeof(float))
                    return BitConverter.ToSingle(dataBytes, 0).ToString();

                if (Type == typeof(double))
                    return BitConverter.ToDouble(dataBytes, 0).ToString();
            }

            // Get Uint64 value
            var intBytes = new byte[8];
            dataBytes.CopyTo(intBytes, 0);
            UInt64 dataValue = BitConverter.ToUInt64(intBytes, 0);

            // Apply mask
            if (Mask.HasValue)
                dataValue &= Mask.Value;

            // Boolean parsing
            if (IsBool)
                return (dataValue != 0x00).ToString();

            // Print hex
            if (UseHex)
                return "0x" + dataValue.ToString("X" + ByteCount * 2);

            // Print signed
            if (Type == typeof(Int64))
                return ((Int64)dataValue).ToString();
            else if (Type == typeof(Int32))
                return ((Int32)dataValue).ToString();
            else if (Type == typeof(Int16))
                return ((Int16)dataValue).ToString();
            else if (Type == typeof(sbyte))
                return ((sbyte)dataValue).ToString();
            else
                return dataValue.ToString();
        }

        public byte[] GetBytesFromAngleString(string value, AngleViewModeType viewMode)
        {
            if (Type != typeof(UInt32) && Type != typeof(UInt16)
                && Type != typeof(Int32) && Type != typeof(Int16))
                return null;

            UInt32 writeValue = 0;

            // Print hex
            if (ParsingUtilities.IsHex(value))
            {
                ParsingUtilities.TryParseHex(value, out writeValue);
            }
            else
            {
                switch (viewMode)
                {
                    case AngleViewModeType.Signed:
                    case AngleViewModeType.Unsigned:
                    case AngleViewModeType.Recommended:
                        int tempValue;
                        if (int.TryParse(value, out tempValue))
                            writeValue = (uint)tempValue;
                        else if (!uint.TryParse(value, out writeValue))
                            return null;
                        break;


                    case AngleViewModeType.Degrees:
                        double degValue;
                        if (!double.TryParse(value, out degValue))
                            return null;
                        writeValue = (UInt16)(degValue / (360d / 65536));
                        break;

                    case AngleViewModeType.Radians:
                        double radValue;
                        if (!double.TryParse(value, out radValue))
                            return null;
                        writeValue = (UInt16)(radValue / (2 * Math.PI / 65536));
                        break;
                }
            }

            return BitConverter.GetBytes(writeValue).Take(ByteCount).ToArray();
        }

        public bool SetAngleStringValue(uint offset, string value, AngleViewModeType viewMode)
        {
            var dataBytes = GetBytesFromAngleString(value, viewMode);
            return SetBytes(offset, dataBytes);
        }

        public string GetAngleStringValue(uint offset, AngleViewModeType viewMode, bool truncated = false)
        {
            // Get dataBytes
            var dataBytes = GetByteData(offset);

            // Make sure offset is a valid pointer
            if (dataBytes == null)
                return "(none)";

            // Make sure dataType is a valid angle type
            if (Type != typeof(UInt32) && Type != typeof(UInt16)
                && Type != typeof(Int32) && Type != typeof(Int16))
                return "Error: datatype";

            // Get Uint32 value
            UInt32 dataValue = (Type == typeof(UInt32)) ? BitConverter.ToUInt32(dataBytes, 0)
                : BitConverter.ToUInt16(dataBytes, 0);

            // Apply mask
            if (Mask.HasValue)
                dataValue = (UInt32)(dataValue & Mask.Value);

            // Truncate by 16
            if (truncated)
                dataValue &= ~0x000FU;

            // Print hex
            if (UseHex)
            {
                if (viewMode == AngleViewModeType.Recommended && ByteCount == 4)
                    return "0x" + dataValue.ToString("X8");
                else
                    return "0x" + ((UInt16)dataValue).ToString("X4");
            }

            switch (viewMode)
            {
                case AngleViewModeType.Recommended:
                    if (Type == typeof(Int16))
                        return ((Int16)dataValue).ToString();
                    else if (Type == typeof(UInt16))
                        return ((UInt16)dataValue).ToString();
                    else if (Type == typeof(Int32))
                        return ((Int32)dataValue).ToString();
                    else
                        return dataValue.ToString();

                case AngleViewModeType.Unsigned:
                    return ((UInt16)dataValue).ToString();

                case AngleViewModeType.Signed:
                    return ((Int16)(dataValue)).ToString();

                case AngleViewModeType.Degrees:
                    return (((UInt16)dataValue) * (360d / 65536)).ToString();

                case AngleViewModeType.Radians:
                    return (((UInt16)dataValue) * (2 * Math.PI / 65536)).ToString();
            }

            return "Error: ang. parse";
        }

        public bool GetBoolValue(uint offset)
        {
            // Get dataBytes
            var dataBytes = GetByteData(offset);

            // Make sure offset is a valid pointer
            if (dataBytes == null)
                return false;

            // Get Uint64 value
            var intBytes = new byte[8];
            dataBytes.CopyTo(intBytes, 0);
            UInt64 dataValue = BitConverter.ToUInt64(intBytes, 0);

            // Apply mask
            if (Mask.HasValue)
                dataValue &= Mask.Value;

            // Boolean parsing
            bool value = (dataValue != 0x00);
            value = InvertBool ? !value : value;
            return value;
        }

        public void SetBoolValue(uint offset, bool value)
        {
            // Get dataBytes
            var address = AddressHolder.HasAdditiveOffset ? offset + Address : Address;
            var dataBytes = GetByteData(offset);

            // Make sure offset is a valid pointer
            if (dataBytes == null)
                return;

            if (InvertBool)
                value = !value;

            // Get Uint64 value
            var intBytes = new byte[8];
            dataBytes.CopyTo(intBytes, 0);
            UInt64 dataValue = BitConverter.ToUInt64(intBytes, 0);

            // Apply mask
            if (Mask.HasValue)
            {
                if (value)
                    dataValue |= Mask.Value;
                else
                    dataValue &= ~Mask.Value;
            }
            else
            {
                dataValue = value ? 1U : 0U;
            }

            var writeBytes = new byte[ByteCount];
            var valueBytes = BitConverter.GetBytes(dataValue);
            Array.Copy(valueBytes, 0, writeBytes, 0, ByteCount);

            Config.Stream.WriteRamLittleEndian(writeBytes, address, AddressHolder.UseAbsoluteAddressing);
        }

        public byte[] GetBytesFromString(uint offset, string value)
        {
            // Get dataBytes
            var address = AddressHolder.HasAdditiveOffset ? offset + Address : Address;
            var dataBytes = new byte[8];
            Config.Stream.ReadRamLittleEndian(new UIntPtr(address), ByteCount, AddressHolder.UseAbsoluteAddressing).CopyTo(dataBytes, 0);
            UInt64 oldValue = BitConverter.ToUInt64(dataBytes, 0);
            UInt64 newValue;

            // Handle object values
            uint? objectAddress;
            if (IsObject && (objectAddress = ManagerContext.Current.ObjectSlotManager.GetSlotAddressFromName(value)).HasValue)
            {
                newValue = objectAddress.Value;
            }
            else
            // Handle hex variable
            if (ParsingUtilities.IsHex(value))
            {
                if (!ParsingUtilities.TryParseExtHex(value, out newValue))
                    return null;
            }
            // Handle floats
            else if (Type == typeof(float))
            {
                float newFloatValue;
                if (!float.TryParse(value, out newFloatValue))
                    return null;

                // Get bytes
                newValue = BitConverter.ToUInt32(BitConverter.GetBytes(newFloatValue), 0);
            }
            else if (Type == typeof(double))
            {
                double newFloatValue;
                if (double.TryParse(value, out newFloatValue))
                    return null;

                // Get bytes
                newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newFloatValue), 0);
            }
            else if (Type == typeof(UInt64))
            {
                if (!UInt64.TryParse(value, out newValue))
                {
                    Int64 newValueInt;
                    if (!Int64.TryParse(value, out newValueInt))
                        return null;

                    newValue = (UInt64)newValueInt;
                }
            }
            else
            {
                Int64 tempInt;
                if (!Int64.TryParse(value, out tempInt))
                    return null;
                newValue = (UInt64)tempInt;
            }

            // Apply mask
            if (Mask.HasValue)
                newValue = (newValue & Mask.Value) | ((~Mask.Value) & oldValue);

            var writeBytes = new byte[ByteCount];
            var valueBytes = BitConverter.GetBytes(newValue);
            Array.Copy(valueBytes, 0, writeBytes, 0, ByteCount);

            return writeBytes;
        }

        public bool SetBytes(uint offset, byte[] dataBytes)
        {
            if (dataBytes == null)
                return false;

            return Config.Stream.WriteRamLittleEndian(dataBytes, AddressHolder.HasAdditiveOffset ? offset + Address
                : Address, AddressHolder.UseAbsoluteAddressing);
        }
    }
}
