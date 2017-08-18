using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class AngleDataContainer : IDataContainer
    {
        Label _nameLabel;
        BorderedTableLayoutPanel _tablePanel;
        TextBox _textBoxValue;
        string _specialName;

        static AngleDataContainer _lastSelected;

        public enum AngleViewModeType { Signed, Unsigned, Degrees, Radians };

        AngleViewModeType _angleViewMode;
        bool _angleTruncated = false;

        public AngleDataContainer(string name, AngleViewModeType angleViewMode = AngleViewModeType.Unsigned)
        {
            _specialName = name;
            _angleViewMode = angleViewMode;

            this._nameLabel = new Label();
            this._nameLabel.Size = new Size(210, 20);
            this._nameLabel.Text = name;
            this._nameLabel.Margin = new Padding(3, 3, 3, 3);

            this._textBoxValue = new TextBox();
            this._textBoxValue.ReadOnly = true;
            this._textBoxValue.BorderStyle = BorderStyle.None;
            this._textBoxValue.TextAlign = HorizontalAlignment.Right;
            this._textBoxValue.Width = 200;
            this._textBoxValue.Margin = new Padding(6, 3, 6, 3);
            this._textBoxValue.ContextMenuStrip = AngleMenu;
            this._textBoxValue.MouseEnter += _textBoxValue_MouseEnter;

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
            this._tablePanel.ShowBorder = false;
            this._tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            this._tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            this._tablePanel.Controls.Add(_nameLabel, 0, 0);
            this._tablePanel.Controls.Add(this._textBoxValue, 1, 0);

            AngleMenu.ItemClicked += OnMenuStripClick;
            AngleDropDownMenu[0].DropDownItemClicked += AngleDropDownMenu_DropDownItemClicked;
            AngleDropDownMenu[1].Click += TruncateAngleMenu_ItemClicked;
            AngleDropDownMenu[2].Click += AngleDataContainer_Click;
        }

        private void AngleDataContainer_Click(object sender, EventArgs e)
        {
            if (this != _lastSelected)
                return;

            AngleDropDownMenu[2].Checked = !AngleDropDownMenu[2].Checked;
            _tablePanel.ShowBorder = AngleDropDownMenu[2].Checked;
        }

        private void TruncateAngleMenu_ItemClicked(object sender, EventArgs e)
        {
            if (this != _lastSelected)
                return;

            _angleTruncated = !_angleTruncated;

            UpdateAngleValue();
        }

        private void AngleDropDownMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this != _lastSelected)
                return;

            switch (e.ClickedItem.Text)
            {
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

            UpdateAngleValue();
        }

        private void OnMenuStripClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this != _lastSelected)
                return;
        }

        private void _textBoxValue_MouseEnter(object sender, EventArgs e)
        {
            _lastSelected = this;
            (AngleDropDownMenu[0].DropDownItems[0] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Unsigned);
            (AngleDropDownMenu[0].DropDownItems[1] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Signed);
            (AngleDropDownMenu[0].DropDownItems[2] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Degrees);
            (AngleDropDownMenu[0].DropDownItems[3] as ToolStripMenuItem).Checked = (_angleViewMode == AngleViewModeType.Radians);
            (AngleDropDownMenu[1] as ToolStripMenuItem).Checked = _angleTruncated;
            (AngleDropDownMenu[2] as ToolStripMenuItem).Checked = _tablePanel.ShowBorder;
        }

        public Control Control
        {
            get
            {
                return _tablePanel;
            }
        }

        private bool _valueExists = false;
        public bool ValueExists
        {
            get
            {
                return _valueExists;
            }
            set
            {
                _valueExists = value;
                UpdateAngleValue();
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

        private double _angleValue;
        public double AngleValue
        {
            get
            {
                return _angleValue;
            }
            set
            {
                _angleValue = MoreMath.FormatAngleDouble(value);
                UpdateAngleValue();
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
                _textBoxValue.BackColor = Color;
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
                    //_angleMenu.Items.Add("Edit");
                    //var newItem = new ToolStripMenuItem("View As Hexadecimal");
                    //newItem.Name = "HexView";
                    //_angleMenu.Items.Add(newItem);
                    //newItem = new ToolStripMenuItem("Lock Value");
                    //newItem.Name = "LockValue";
                    //_angleMenu.Items.Add(newItem);

                    _angleMenu.Items.Add(AngleDropDownMenu[0]);
                    _angleMenu.Items.Add(AngleDropDownMenu[1]);
                    _angleMenu.Items.Add(AngleDropDownMenu[2]);
                }
                return _angleMenu;
            }
        }

        private static ToolStripMenuItem[] _angleMenuDropDown;
        public static ToolStripMenuItem[] AngleDropDownMenu
        {
            get
            {
                if (_angleMenuDropDown == null)
                {
                    _angleMenuDropDown = new ToolStripMenuItem[3];
                    _angleMenuDropDown[0] = new ToolStripMenuItem("View Angle As");
                    _angleMenuDropDown[0].DropDownItems.Add("Unsigned (short)");
                    _angleMenuDropDown[0].DropDownItems.Add("Signed (short)");
                    _angleMenuDropDown[0].DropDownItems.Add("Degrees");
                    _angleMenuDropDown[0].DropDownItems.Add("Radians");
                    _angleMenuDropDown[1] = new ToolStripMenuItem("Truncate Angle (by 16)");
                    _angleMenuDropDown[2] = new ToolStripMenuItem("Highlight");
                }
                return _angleMenuDropDown;
            }
        }

        public void UpdateAngleValue()
        {
            if (!ValueExists)
            {
                _textBoxValue.Text = "(none)";
                return;
            }

            var angleValue = _angleValue;

            if (_angleTruncated)
            {
                angleValue = MoreMath.FormatAngleTruncated(angleValue);
            }

            switch (_angleViewMode)
            {
                case AngleViewModeType.Degrees:
                    _textBoxValue.Text = MoreMath.AngleUnitsToDegrees(angleValue).ToString();
                    break;

                case AngleViewModeType.Radians:
                    _textBoxValue.Text = MoreMath.AngleUnitsToRadians(angleValue).ToString();
                    break;

                case AngleViewModeType.Signed:
                    _textBoxValue.Text = MoreMath.FormatAngleShort(angleValue).ToString();
                    break;

                case AngleViewModeType.Unsigned:
                    _textBoxValue.Text = MoreMath.FormatAngleUshort(angleValue).ToString();
                    break;
            }       
        }

        public void Update()
        {
        }
    }
}
