using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic.ManagerClasses
{
    class WatchVariableControl
    {
        TableLayoutPanel _tablePanel;
        Label _nameLabel;
        WatchVariable _watchVar;
        CheckBox _checkBoxBool;
        TextBox _textBoxValue;
        ProcessStream _stream;
        public uint OtherOffset;
        bool _changedByUser = true;

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

        public void Update()
        {
            if (_watchVar.IsBool)
            {
                _changedByUser = false;
                _checkBoxBool.Checked = _watchVar.GetBoolValue(_stream, OtherOffset);
                _changedByUser = true;
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
    }
}
