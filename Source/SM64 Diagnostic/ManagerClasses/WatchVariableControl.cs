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

namespace SM64_Diagnostic.ManagerClasses
{
    public class WatchVariableControl
    {
        TableLayoutPanel _tablePanel;
        Label _nameLabel;
        WatchVariable _watchVar;
        CheckBox _checkBoxBool;
        TextBox _textBoxValue;
        ProcessStream _stream;

        public uint OtherOffset;
        bool _changedByUser = true;
        bool _editMode = false;
        bool _valueLocked = false;
        string _lockedStringValue = "0";

        static WatchVariableControl _lastSelected;

        public enum AngleViewModeType {Raw, Signed, Unsigned, Degrees, Radians};

        AngleViewModeType AngleViewMode = AngleViewModeType.Raw;

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

                    _angleMenu.Items.Add(AngleDropDownMenu);
                }
                return _angleMenu;
            }
        }
        private static ToolStripMenuItem _angleMenuDropDown;

        public static ToolStripMenuItem AngleDropDownMenu
        {
            get
            {
                if (_angleMenuDropDown == null)
                {
                    _angleMenuDropDown = new ToolStripMenuItem("View Angle As");
                    _angleMenuDropDown.DropDownItems.Add("Raw");
                    _angleMenuDropDown.DropDownItems.Add("Unsigned (short)");
                    _angleMenuDropDown.DropDownItems.Add("Signed (short)");
                    _angleMenuDropDown.DropDownItems.Add("Degrees");
                    _angleMenuDropDown.DropDownItems.Add("Radians");
                }
                return _angleMenuDropDown;
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

        public Control Control
        {
            get
            {
                return _tablePanel;
            }
        }

        public WatchVariableControl(ProcessStream stream, WatchVariable watchVar, uint otherOffset)
        {
            _watchVar = watchVar;
            _stream = stream;
            OtherOffset = otherOffset;

            CreateControls();
        }

        private void CreateControls()
        {
            this._nameLabel = new Label();
            this._nameLabel.Width = 210;
            this._nameLabel.Text = _watchVar.Name;
            this._nameLabel.Margin = new Padding(3, 3, 3, 3);
            this._nameLabel.MouseHover += (sender, e) =>
            {
                if (!_watchVar.OtherOffset)
                {
                    AddressToolTip.SetToolTip(this._nameLabel, String.Format("0x{0:X8} [{2} + 0x{1:X8}]",
                        _watchVar.GetRamAddress(_stream), _watchVar.GetProcessAddress(_stream, 0), _stream.ProcessName));
                }
                else
                {
                    AddressToolTip.SetToolTip(this._nameLabel, String.Format("0x{1:X8} + 0x{0:X8} = 0x{2:X8} [{4} + 0x{3:X8}]",
                        _watchVar.GetRamAddress(_stream, false), OtherOffset, _watchVar.GetRamOffsetAddress(_stream, OtherOffset), 
                        _watchVar.GetProcessAddress(_stream, OtherOffset), _stream.ProcessName));
                }
            };

            if (_watchVar.IsBool)
            {
                this._checkBoxBool = new CheckBox();
                this._checkBoxBool.CheckAlign = ContentAlignment.MiddleRight;
                this._checkBoxBool.CheckedChanged += OnModified;
            }
            else
            {
                this._textBoxValue = new TextBox();
                this._textBoxValue.ReadOnly = true;
                this._textBoxValue.BorderStyle = BorderStyle.None;
                this._textBoxValue.TextAlign = HorizontalAlignment.Right;
                this._textBoxValue.Width = 200;
                this._textBoxValue.Margin = new Padding(6, 3, 6, 3);
                this._textBoxValue.TextChanged += OnModified;
                this._textBoxValue.ContextMenuStrip = _watchVar.IsAngle ? WatchVariableControl.AngleMenu : WatchVariableControl.Menu;
                this._textBoxValue.KeyDown += OnTextValueKeyDown;
                this._textBoxValue.MouseEnter += _textBoxValue_MouseEnter;
                this._textBoxValue.DoubleClick += _textBoxValue_DoubleClick;
                this._textBoxValue.Leave += (sender, e) => { _editMode = false; this._textBoxValue.ReadOnly = true; };
                if (_watchVar.IsAngle)
                {
                    WatchVariableControl.AngleMenu.ItemClicked += OnMenuStripClick;
                    WatchVariableControl.AngleDropDownMenu.DropDownItemClicked += AngleDropDownMenu_DropDownItemClicked;
                }
                else
                    WatchVariableControl.Menu.ItemClicked += OnMenuStripClick;
            }

            this._tablePanel = new TableLayoutPanel();
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
            this._tablePanel.Controls.Add(_nameLabel, 0, 0);
            this._tablePanel.Controls.Add(_watchVar.IsBool ? this._checkBoxBool as Control: this._textBoxValue, 1, 0);
        }

        private void _textBoxValue_DoubleClick(object sender, EventArgs e)
        {
            _textBoxValue.ReadOnly = false;
            _textBoxValue.Focus();
            _editMode = true;
        }

        private void _textBoxValue_MouseEnter(object sender, EventArgs e)
        {
            _lastSelected = this;
            if (_watchVar.IsAngle)
            {
                (AngleMenu.Items["HexView"] as ToolStripMenuItem).Checked = _watchVar.UseHex;
                (AngleMenu.Items["LockValue"] as ToolStripMenuItem).Checked = _valueLocked;
                (AngleDropDownMenu.DropDownItems[0] as ToolStripMenuItem).Checked = (AngleViewMode == AngleViewModeType.Raw);
                (AngleDropDownMenu.DropDownItems[1] as ToolStripMenuItem).Checked = (AngleViewMode == AngleViewModeType.Unsigned);
                (AngleDropDownMenu.DropDownItems[2] as ToolStripMenuItem).Checked = (AngleViewMode == AngleViewModeType.Signed);
                (AngleDropDownMenu.DropDownItems[3] as ToolStripMenuItem).Checked = (AngleViewMode == AngleViewModeType.Degrees);
                (AngleDropDownMenu.DropDownItems[4] as ToolStripMenuItem).Checked = (AngleViewMode == AngleViewModeType.Radians);
            }
            else
            {
                (Menu.Items["HexView"] as ToolStripMenuItem).Checked = _watchVar.UseHex;
                (Menu.Items["LockValue"] as ToolStripMenuItem).Checked = _valueLocked;
            }
        }

        private void AngleDropDownMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this != _lastSelected)
                return;

            switch (e.ClickedItem.Text)
            {
                case "Raw":
                    AngleViewMode = AngleViewModeType.Raw;
                    break;
                case "Unsigned (short)":
                    AngleViewMode = AngleViewModeType.Unsigned;
                    break;
                case "Signed (short)":
                    AngleViewMode = AngleViewModeType.Signed;
                    break;
                case "Degrees":
                    AngleViewMode = AngleViewModeType.Degrees;
                    break;
                case "Radians":
                    AngleViewMode = AngleViewModeType.Radians;
                    break;
            }
        }

        public void Update()
        {
            if (_valueLocked)
                _watchVar.SetStringValue(_stream, OtherOffset, _lockedStringValue);

            if (_editMode)
                return;

            if (_watchVar.IsBool)
            {
                _changedByUser = false;
                _checkBoxBool.Checked = _watchVar.GetBoolValue(_stream, OtherOffset);
                _changedByUser = true;
            }
            else if (_watchVar.IsAngle)
            {
                _textBoxValue.Text = _watchVar.GetAngleStringValue(_stream, OtherOffset, AngleViewMode);
            }
            else
            {
                _textBoxValue.Text = _watchVar.GetStringValue(_stream, OtherOffset);
            }
        }

        private void OnModified(object sender, EventArgs e)
        {
            if (!_changedByUser)
                return;

            if (_watchVar.IsBool)
            {
                _watchVar.SetBoolValue(_stream, OtherOffset, _checkBoxBool.Checked);
            }
        }

        private void OnMenuStripClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this != _lastSelected)
                return;

            switch (e.ClickedItem.Text)
            {
                case "Edit":
                    _textBoxValue.ReadOnly = false;
                    _textBoxValue.Focus();
                    _editMode = true;
                    break;
                case "View As Hexadecimal":
                    _watchVar.UseHex = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    (e.ClickedItem as ToolStripMenuItem).Checked = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    break;
                case "Lock Value":
                    _valueLocked = !_valueLocked;
                    _textBoxValue.ReadOnly = true;
                    _editMode = false;
                    (e.ClickedItem as ToolStripMenuItem).Checked = !(e.ClickedItem as ToolStripMenuItem).Checked;
                    _lockedStringValue = _textBoxValue.Text;
                    break;
            }
        }

        private void OnTextValueKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
                return;

            _textBoxValue.ReadOnly = true;
            _editMode = false;
            if (_watchVar.IsAngle)
                _watchVar.SetAngleStringValue(_stream, OtherOffset, _textBoxValue.Text, AngleViewMode);
            else
                _watchVar.SetStringValue(_stream, OtherOffset, _textBoxValue.Text);
        }
    }
}
