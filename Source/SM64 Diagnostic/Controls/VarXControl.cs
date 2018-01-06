using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class VarXControl : TableLayoutPanel
    {
        public VarX _varX;
        public string _name;

        public Label _nameLabel;
        public TextBox _textBox;
        public CheckBox _checkBoxBool;
        public ContextMenuStrip _contextMenuStrip;
        public ContextMenuStrip _textboxOldContextMenuStrip;

        private Color _currentColor = SystemColors.Control;

        // TODO refactor this
        private static readonly int nameLabelHeight = 20;

        public VarXControl(VarX varX, string name, bool useCheckbox)
        {
            _varX = varX;
            _name = name;

            InitializeBase();
            InitializeControls(useCheckbox);
            InitializeContextMenuStrip();

            this.ShowBorder = false;

        }

        private void InitializeBase()
        {
            base.Size = new Size(230, nameLabelHeight + 2);
            base.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            base.RowCount = 1;
            base.ColumnCount = 2;
            base.RowStyles.Clear();
            base.RowStyles.Add(new RowStyle(SizeType.Absolute, nameLabelHeight + 3));
            base.ColumnStyles.Clear();
            base.Margin = new Padding(0);
            base.Padding = new Padding(0);
            base.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            base.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            base.BackColor = _currentColor;
        }

        private void InitializeControls(bool useCheckbox)
        {
            // Name Label
            _nameLabel = new Label();
            _nameLabel.Size = new Size(210, nameLabelHeight);
            _nameLabel.Text = _name;
            _nameLabel.Margin = new Padding(3, 3, 3, 3);
            _nameLabel.Click += (sender, e) => _varX._nameLabel_Click();
            _nameLabel.ImageAlign = ContentAlignment.MiddleRight;
            _nameLabel.BackColor = Color.Transparent;
            base.Controls.Add(_nameLabel, 0, 0);

            // Textbox
            _textBox = new TextBox();
            _textBox.ReadOnly = true;
            _textBox.BorderStyle = BorderStyle.None;
            _textBox.TextAlign = HorizontalAlignment.Right;
            _textBox.Width = 200;
            _textBox.Margin = new Padding(6, 3, 6, 3);
            _textBox.KeyDown += (sender, e) => _varX.OnTextValueKeyDown(e);
            _textBox.DoubleClick += (sender, e) => _varX._textBoxValue_DoubleClick();
            _textBox.Leave += (sender, e) => { _varX.EditMode = false; };
            base.Controls.Add(this._textBox, 1, 0);

            // Checkbox
            _checkBoxBool = new CheckBox();
            _checkBoxBool.CheckAlign = ContentAlignment.MiddleRight;
            _checkBoxBool.CheckState = CheckState.Unchecked;
            _checkBoxBool.Click += (sender, e) => _varX.SetValueFromCheckbox(_checkBoxBool.CheckState);
            _checkBoxBool.BackColor = Color.Transparent;
            base.Controls.Add(this._checkBoxBool, 1, 0);

            // Only let one of Textbox/Checkbox remain visible
            SetUseCheckbox(useCheckbox);
        }

        private void InitializeContextMenuStrip()
        {
            // Context Menu Strip
            _textboxOldContextMenuStrip = _textBox.ContextMenuStrip;
            _contextMenuStrip = new ContextMenuStrip();
            _nameLabel.ContextMenuStrip = _contextMenuStrip;
            _textBox.ContextMenuStrip = _contextMenuStrip;
            base.ContextMenuStrip = _contextMenuStrip;
        }

        public void SetUseCheckbox(bool useCheckbox)
        {
            if (useCheckbox)
            {
                _textBox.Visible = false;
                _checkBoxBool.Visible = true;
            }
            else
            {
                _textBox.Visible = true;
                _checkBoxBool.Visible = false;
            }
        }

        private Pen _borderPen = new Pen(Color.Red, 5);

        private bool _showBorder = true;
        public bool ShowBorder
        {
            get
            {
                return _showBorder;
            }
            set
            {
                if (_showBorder == value)
                    return;

                _showBorder = value;
                Invalidate();
            }
        }
        /*
        public Color BorderColor
        {
            get
            {
                return _borderPen.Color;
            }
            set
            {
                if (_borderPen.Color == value)
                    return;

                _borderPen.Color = value;
                Invalidate();
            }
        }

        public float BorderWidth
        {
            get
            {
                return _borderPen.Width;
            }
            set
            {
                if (_borderPen.Width == value)
                    return;

                _borderPen.Width = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var rec = DisplayRectangle;
            rec.Width -= 1;
            rec.Height -= 1;
            if (_showBorder)
                e.Graphics.DrawRectangle(_borderPen, rec);
        }
        */
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _borderPen?.Dispose();
        }
    }
}
