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

        private bool _editMode;
        private bool _highlighted;

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
                    _textBox.BackColor = _editMode ? Color.White : _currentColor;
                    if (_editMode)
                    {
                        _textBox.Focus();
                        _textBox.SelectAll();
                    }
                }
            }
        }

        private static readonly int FAILURE_DURATION_MS = 1000;
        private static readonly Color FAILURE_COLOR = Color.Red;
        private static readonly Color DEFAULT_COLOR = SystemColors.Control;

        private readonly Color _baseColor;
        private Color _currentColor;
        private bool _justFailed;
        private DateTime _lastFailureTime;



        public static VarX CreateVarX(
            string name, AddressHolder addressHolder, VarXSubclass varXSubclcass, Color? backgroundColor)
        {
            switch (varXSubclcass)
            {
                case VarXSubclass.String:
                case VarXSubclass.Boolean:
                    return new VarX(name, addressHolder, backgroundColor);

                case VarXSubclass.Number:
                    return new VarXNumber(name, addressHolder, backgroundColor);

                case VarXSubclass.UnsignedAngle:
                    return new VarXAngle(name, addressHolder, backgroundColor, false);
                case VarXSubclass.SignedAngle:
                    return new VarXAngle(name, addressHolder, backgroundColor, true);

                case VarXSubclass.Object:
                    return new VarXObject(name, addressHolder, backgroundColor);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public VarX(string name, AddressHolder addressHolder, Color? backgroundColor)
        {
            Name = name;
            AddressHolder = addressHolder;
            _baseColor = backgroundColor ?? DEFAULT_COLOR;
            _currentColor = _baseColor;

            _editMode = false;
            _highlighted = false;
            _justFailed = false;
            _lastFailureTime = DateTime.Now;

            CreateControls();
            AddContextMenuStrip();
        }





        private BorderedTableLayoutPanel _tablePanel;
        protected Label _nameLabel;
        protected TextBox _textBox;

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
        
        private void CreateControls()
        {
            this._nameLabel = new Label();
            this._nameLabel.Size = new Size(210, 20); //TODO check this
            this._nameLabel.Text = Name;
            this._nameLabel.Margin = new Padding(3, 3, 3, 3);
            this._nameLabel.Click += _nameLabel_Click;
            this._nameLabel.ImageAlign = ContentAlignment.MiddleRight;
            this._nameLabel.BackColor = Color.Transparent;

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
            this._tablePanel.BackColor = _currentColor;
        }

        private void AddContextMenuStrip()
        {
            ToolStripMenuItem itemHighlight = new ToolStripMenuItem("Highlight");
            itemHighlight.Click += (sender, e) =>
            {
                _highlighted = !_highlighted;
                _tablePanel.ShowBorder = _highlighted;
                itemHighlight.Checked = _highlighted;
            };
            itemHighlight.Checked = _highlighted;

            ToolStripMenuItem itemEdit = new ToolStripMenuItem("Edit");
            itemEdit.Click += (sender, e) => { EditMode = true; };

            ToolStripMenuItem itemCopy = new ToolStripMenuItem("Copy");
            itemCopy.Click += (sender, e) => { Clipboard.SetText(GetValueFinal(false)); };

            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");
            itemPaste.Click += (sender, e) => { SetValueFinal(Clipboard.GetText()); };

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(itemHighlight);
            contextMenuStrip.Items.Add(itemEdit);
            contextMenuStrip.Items.Add(itemCopy);
            contextMenuStrip.Items.Add(itemPaste);

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

        private void InvokeFailure()
        {
            _justFailed = true;
            _lastFailureTime = DateTime.Now;
        }

        public void Update()
        {
            if (!_editMode)
            {
                _textBox.Text = GetValueFinal();
            }

            UpdateColor();
        }

        private void UpdateColor()
        {
            if (_justFailed)
            {
                DateTime currentTime = DateTime.Now;
                double timeSinceLastFailure = currentTime.Subtract(_lastFailureTime).TotalMilliseconds;
                if (timeSinceLastFailure < FAILURE_DURATION_MS)
                {
                    _currentColor = ColorUtilities.InterpolateColor(
                        FAILURE_COLOR, _baseColor, timeSinceLastFailure / FAILURE_DURATION_MS);
                }
                else
                {
                    _currentColor = _baseColor;
                    _justFailed = false;
                }
            }

            _tablePanel.BackColor = _currentColor;
            if (!_editMode) _textBox.BackColor = _currentColor;
        }

        private void OnTextValueKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                EditMode = false;
                return;
            }

            if (e.KeyData == Keys.Enter)
            {
                bool success = SetValueFinal(_textBox.Text);
                EditMode = false;
                if (!success)
                {
                    InvokeFailure();
                }
                return;
            }
        }






        public string GetValueFinal(bool handleRounding = true)
        {
            List<string> values = AddressHolder.GetValues();
            (bool meaningfulValue, string value) = CombineValues(values);
            if (!meaningfulValue) return value;

            value = HandleAngleConverting(value);
            if (handleRounding) value = HandleRounding(value);
            value = HandleAngleRoundingOut(value);
            value = HandleNegating(value);
            value = HandleHexDisplaying(value);
            value = HandleObjectDisplaying(value);

            return value;
        }

        public bool SetValueFinal(string value)
        {
            value = HandleObjectUndisplaying(value);
            value = HandleHexUndisplaying(value);
            value = HandleUnnegating(value);
            value = HandleAngleUnconverting(value);

            return AddressHolder.SetValue(value);
        }





        public (bool meaningfulValue, string stringValue) CombineValues(List<string> values)
        {
            string combinedValue = "(none)";
            string firstValue = null;
            bool atLeastOneValueIncorporated = false;
            bool meaningfulValue = false;

            foreach (string value in values)
            {
                if (!atLeastOneValueIncorporated)
                {
                    combinedValue = value;
                    firstValue = value;
                    atLeastOneValueIncorporated = true;
                    meaningfulValue = true;
                }
                else
                {
                    if (value != firstValue)
                    {
                        combinedValue = "(multiple values)";
                        meaningfulValue = false;
                        break;
                    }
                }
            }

            return (meaningfulValue, combinedValue);
        }

        // Number methods

        public virtual string HandleRounding(string value)
        {
            return value;
        }

        public virtual string HandleNegating(string value)
        {
            return value;
        }

        public virtual string HandleUnnegating(string value)
        {
            return value;
        }

        public virtual string HandleHexDisplaying(string value)
        {
            return value;
        }

        public virtual string HandleHexUndisplaying(string value)
        {
            return value;
        }

        // Angle methods

        public virtual string HandleAngleConverting(string value)
        {
            return value;
        }

        public virtual string HandleAngleUnconverting(string value)
        {
            return value;
        }

        public virtual string HandleAngleRoundingOut(string value)
        {
            return value;
        }

        // Object methods

        public virtual string HandleObjectDisplaying(string value)
        {
            return value;
        }

        public virtual string HandleObjectUndisplaying(string value)
        {
            return value;
        }

    }
}
