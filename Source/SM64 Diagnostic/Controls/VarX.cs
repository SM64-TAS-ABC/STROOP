using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class VarX
    {
        Label _nameLabel;
        BorderedTableLayoutPanel _tablePanel;
        TextBox _textBoxValue;
        string _specialName;
        static VarX _lastSelected;

        private static ContextMenuStrip _menu;
        public static ContextMenuStrip Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new ContextMenuStrip();
                    var newItem = new ToolStripMenuItem("Highlight");
                    newItem.Name = "Highlight";
                    _menu.Items.Add(newItem);
                }
                return _menu;
            }
        }

        public VarX(string name)
        {
            _specialName = name;

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
            this._textBoxValue.ContextMenuStrip = DataContainer.Menu;
            this._textBoxValue.MouseEnter += (sender, e) => {
                _lastSelected = this;
                (Menu.Items["Highlight"] as ToolStripMenuItem).Checked = _tablePanel.ShowBorder;
            };

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

            Menu.ItemClicked += Menu_ItemClicked;
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_lastSelected != this)
                return;

            var item = e.ClickedItem as ToolStripMenuItem;
            item.Checked = !item.Checked;
            _tablePanel.ShowBorder = item.Checked;
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

        public string Text
        {
            get
            {
                return _textBoxValue.Text;
            }
            set
            {
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
                _textBoxValue.BackColor = Color;
            }
        }

        public void Update()
        {
        }
    }
}
