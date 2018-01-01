using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class VarX
    {
        public readonly string Name;
        public readonly AddressHolder AddressHolder;
        protected readonly Func<List<object>> _getterFunction;
        protected readonly Action<string> _setterFunction;

        public static VarX CreateVarX(
            string name, AddressHolder addressHolder, VarXSubclass varXSubclcass)
        {
            if (varXSubclcass == VarXSubclass.Number)
            {
                return new VarXNumber(name, addressHolder);
            }
            else if (varXSubclcass == VarXSubclass.Angle)
            {
                return new VarXAngle(name, addressHolder);
            }
            else
            {
                return new VarX(name, addressHolder);
            }
        }

        public VarX(string name, AddressHolder addressHolder)
        {
            Name = name;
            AddressHolder = addressHolder;

            // Created getter/setter functions
            if (AddressHolder.IsSpecial)
            {
                (_getterFunction, _setterFunction) = VarXSpecialUtilities.CreateGetterSetterFunctions(AddressHolder.SpecialType);
            }
            else
            {
                _getterFunction = () =>
                {
                    return AddressHolder.EffectiveAddressList.ConvertAll(
                        address => Config.Stream.GetValue(AddressHolder.MemoryType, address, AddressHolder.UseAbsoluteAddressing));
                };
                _setterFunction = (string stringValue) =>
                {
                    AddressHolder.EffectiveAddressList.ForEach(
                        address => Config.Stream.SetValue(AddressHolder.MemoryType, stringValue, address, AddressHolder.UseAbsoluteAddressing));
                };
            }

            CreateControls();
            AddContextMenuStrip();
        }





        private BorderedTableLayoutPanel _tablePanel;
        private Label _nameLabel;
        private TextBox _textBox;
        private bool _editMode = false;

        public Control Control
        {
            get
            {
                return _tablePanel;
            }
        }

        public List<Control> Controls
        {
            get
            {
                return new List<Control>() { _tablePanel, _nameLabel, _textBox };
            }
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
                if (_textBox != null)
                {
                    _textBox.ReadOnly = !_editMode;
                    _textBox.BackColor = _editMode ? Color.White : SystemColors.Control;
                    if (_editMode)
                    {
                        _textBox.Focus();
                        _textBox.SelectAll();
                    }
                }
            }
        }
        
        private void CreateControls()
        {
            this._nameLabel = new Label();
            this._nameLabel.Size = new Size(210, 20); //TODO check this
            this._nameLabel.Text = Name;
            this._nameLabel.Margin = new Padding(3, 3, 3, 3);
            this._nameLabel.Click += _nameLabel_Click;
            this._nameLabel.ImageAlign = ContentAlignment.MiddleRight;

            this._textBox = new TextBox();
            this._textBox.ReadOnly = true;
            this._textBox.BorderStyle = BorderStyle.None;
            this._textBox.TextAlign = HorizontalAlignment.Right;
            this._textBox.Width = 200;
            this._textBox.Margin = new Padding(6, 3, 6, 3);
            this._textBox.KeyDown += OnTextValueKeyDown;
            this._textBox.DoubleClick += _textBoxValue_DoubleClick;
            this._textBox.Leave += (sender, e) => { EditMode = false; };

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
            this._tablePanel.Controls.Add(this._textBox, 1, 0);
        }

        private void AddContextMenuStrip()
        {
            ToolStripMenuItem itemEdit = new ToolStripMenuItem("Edit");
            ToolStripMenuItem itemHighlight = new ToolStripMenuItem("Highlight");

            itemEdit.Click += (sender, e) => { EditMode = true; };
            itemHighlight.Click += (sender, e) =>
            {
                bool currentlyHighlighted = _tablePanel.ShowBorder;
                _tablePanel.ShowBorder = !currentlyHighlighted;
                itemHighlight.Checked = !currentlyHighlighted;
            };

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(itemEdit);
            contextMenuStrip.Items.Add(itemHighlight);

            foreach (Control control in Controls)
            {
                control.ContextMenuStrip = contextMenuStrip;
            }
        }

        private void _nameLabel_Click(object sender, EventArgs e)
        {
            VariableViewerForm varInfo;
            string typeDescr = AddressHolder.MemoryTypeName;

            varInfo = new VariableViewerForm(Name, typeDescr,
                String.Format("0x{0:X8}", AddressHolder.GetRamAddress()),
                String.Format("0x{0:X8}", AddressHolder.GetProcessAddress().ToUInt64()));

            varInfo.ShowDialog();
        }

        private void _textBoxValue_DoubleClick(object sender, EventArgs e)
        {
            EditMode = true;
        }

        public void Update()
        {
            if (_editMode)
                return;

            _textBox.Text = GetValueFinal();
        }

        public string GetValueFinal()
        {
            string combinedVarString = "(none)";
            string firstVarString = null;
            bool atLeastOneVarIncorporated = false;

            foreach (object value in GetValue())
            {
                string varString = value.ToString();

                if (!atLeastOneVarIncorporated)
                {
                    combinedVarString = varString;
                    firstVarString = varString;
                    atLeastOneVarIncorporated = true;
                }
                else
                {
                    if (varString != firstVarString)
                    {
                        combinedVarString = "(multiple values)";
                        break;
                    }
                }
            }

            return combinedVarString;
        }

        public virtual List<object> GetValue()
        {
            return _getterFunction();
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
            SetValueFinal(_textBox.Text);
            EditMode = false;
        }

        public void SetValueFinal(string stringValue)
        {
            Config.Stream.Suspend();

            SetValue(stringValue);

            Config.Stream.Resume();
        }

        public virtual void SetValue(string stringValue)
        {
            _setterFunction(stringValue);
        }

    }
}
