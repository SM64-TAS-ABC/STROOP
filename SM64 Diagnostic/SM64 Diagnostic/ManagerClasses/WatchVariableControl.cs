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

        public WatchVariableControl(WatchVariable watchVar)
        {
            _watchVar = watchVar;

            CreateControls();
        }

        private void CreateControls()
        {
            this._nameLabel = new Label();
            this._nameLabel.Width = 200;
            this._nameLabel.Text = _watchVar.Name;
            this._nameLabel.Margin = new Padding(3, 3, 3, 3);

            if (_watchVar.IsBool)
            {
                this._checkBoxBool = new CheckBox();
                this._checkBoxBool.CheckAlign = ContentAlignment.MiddleRight;
            }
            else
            {
                this._textBoxValue = new TextBox();
                this._textBoxValue.ReadOnly = true;
                this._textBoxValue.BorderStyle = BorderStyle.None;
                this._textBoxValue.TextAlign = HorizontalAlignment.Right;
                this._textBoxValue.Width = 200;
                this._textBoxValue.Margin = new Padding(6, 3, 6, 3);
            }

            this._tablePanel = new TableLayoutPanel();
            this._tablePanel.Size = new Size(220, _nameLabel.Height + 2);
            this._tablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this._tablePanel.RowCount = 1;
            this._tablePanel.ColumnCount = 2;
            this._tablePanel.RowStyles.Clear();
            this._tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, _nameLabel.Height + 3));
            this._tablePanel.ColumnStyles.Clear();
            this._tablePanel.Margin = new Padding(0);
            this._tablePanel.Padding = new Padding(0);
            this._tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            this._tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            this._tablePanel.Controls.Add(_nameLabel, 0, 0);
            this._tablePanel.Controls.Add(_watchVar.IsBool ? this._checkBoxBool as Control: this._textBoxValue, 1, 0);
        }

        public void Update(ProcessStream stream, uint offset = 0)
        {
            if (_watchVar.IsBool)
            {
                _checkBoxBool.Checked = _watchVar.GetBoolValue(stream, offset);
            }
            else
            {
                _textBoxValue.Text = _watchVar.GetStringValue(stream, offset);
            }
        }
    }
}
