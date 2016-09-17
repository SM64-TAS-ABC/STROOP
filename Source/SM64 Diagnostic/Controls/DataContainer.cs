using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.ManagerClasses
{
    public class DataContainer
    {
        Label _nameLabel;
        TableLayoutPanel _tablePanel;
        TextBox TextBoxValue;

        public DataContainer(string name)
        {
            this._nameLabel = new Label();
            this._nameLabel.Width = 210;
            this._nameLabel.Text = name;
            this._nameLabel.Margin = new Padding(3, 3, 3, 3);

            this.TextBoxValue = new TextBox();
            this.TextBoxValue.ReadOnly = true;
            this.TextBoxValue.BorderStyle = BorderStyle.None;
            this.TextBoxValue.TextAlign = HorizontalAlignment.Right;
            this.TextBoxValue.Width = 200;
            this.TextBoxValue.Margin = new Padding(6, 3, 6, 3);

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
            this._tablePanel.Controls.Add(this.TextBoxValue, 1, 0);
        }

        public Control Control
        {
            get
            {
                return _tablePanel;
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

        public string Text
        {
            get
            {
                return TextBoxValue.Text;
            }
            set
            {
                TextBoxValue.Text = value;
            }
        }
    }
}
