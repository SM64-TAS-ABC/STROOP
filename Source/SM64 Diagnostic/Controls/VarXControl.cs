using SM64_Diagnostic.Structs;
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
    public class VarXControl : TableLayoutPanel
    {
        public readonly VarX _varX;
        public readonly string VarName;

        public readonly Label _nameLabel;
        public readonly TextBox _valueTextBox;
        public readonly CheckBox _valueCheckBox;
        public readonly ContextMenuStrip _contextMenuStrip;
        public readonly ContextMenuStrip _textboxOldContextMenuStrip;

        private Pen _borderPen = new Pen(Color.Red, 5);

        public static readonly int FAILURE_DURATION_MS = 1000;
        public static readonly Color FAILURE_COLOR = Color.Red;
        public static readonly Color DEFAULT_COLOR = SystemColors.Control;

        public readonly Color _baseColor;
        public Color _currentColor;
        public bool _justFailed;
        public DateTime _lastFailureTime;

        private bool _showBorder;
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

        private bool _editMode;
        public bool EditMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                _editMode = value;
                if (_valueTextBox != null)
                {
                    _valueTextBox.ReadOnly = !_editMode;
                    _valueTextBox.BackColor = _editMode ? Color.White : _currentColor;
                    _valueTextBox.ContextMenuStrip = _editMode ? _textboxOldContextMenuStrip : _contextMenuStrip;
                    if (_editMode)
                    {
                        _valueTextBox.Focus();
                        _valueTextBox.SelectAll();
                    }
                }
            }
        }

        // TODO refactor this
        private static readonly int nameLabelHeight = 20;

        public VarXControl(
            string name,
            AddressHolder addressHolder,
            VarXSubclass varXSubclcass,
            Color? backgroundColor,
            bool invertBool = false)
        {
            // Initialize main fields
            VarName = name;
            _showBorder = false;
            _editMode = false;

            // initialize color fields
            _baseColor = backgroundColor ?? DEFAULT_COLOR;
            _currentColor = _baseColor;
            _justFailed = false;
            _lastFailureTime = DateTime.Now;

            // Initialize control fields
            InitializeBase();
            _nameLabel = CreateNameLabel();
            _valueTextBox = CreateValueTextBox();
            _valueCheckBox = CreateValueCheckBox();
            base.Controls.Add(_nameLabel, 0, 0);
            base.Controls.Add(_valueTextBox, 1, 0);
            base.Controls.Add(_valueCheckBox, 1, 0);

            // Initialize context menu strip
            _textboxOldContextMenuStrip = _valueTextBox.ContextMenuStrip;
            _contextMenuStrip = new ContextMenuStrip();
            _nameLabel.ContextMenuStrip = _contextMenuStrip;
            _valueTextBox.ContextMenuStrip = _contextMenuStrip;
            base.ContextMenuStrip = _contextMenuStrip;

            // Create var x
            _varX = VarX.CreateVarX(addressHolder, this, varXSubclcass, invertBool);

            // Set whether to start as a checkbox
            SetUseCheckbox(_varX.StartsAsCheckbox());

            // Add functions
            _valueTextBox.KeyDown += (sender, e) => OnTextValueKeyDown(e);
            _nameLabel.Click += (sender, e) => _varX.ShowVarInfo();
            _valueCheckBox.Click += (sender, e) => _varX.SetValueFromCheckbox(_valueCheckBox.CheckState);
            _valueTextBox.DoubleClick += (sender, e) => { EditMode = true; };
            _valueTextBox.Leave += (sender, e) => { EditMode = false; };

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

        private Label CreateNameLabel()
        {
            Label nameLabel = new Label();
            nameLabel.Size = new Size(210, nameLabelHeight);
            nameLabel.Text = VarName;
            nameLabel.Margin = new Padding(3, 3, 3, 3);
            nameLabel.ImageAlign = ContentAlignment.MiddleRight;
            nameLabel.BackColor = Color.Transparent;
            return nameLabel;
        }

        private TextBox CreateValueTextBox()
        {
            TextBox valueTextBox = new TextBox();
            valueTextBox.ReadOnly = true;
            valueTextBox.BorderStyle = BorderStyle.None;
            valueTextBox.TextAlign = HorizontalAlignment.Right;
            valueTextBox.Width = 200;
            valueTextBox.Margin = new Padding(6, 3, 6, 3);
            return valueTextBox;
        }

        private CheckBox CreateValueCheckBox()
        {
            CheckBox valueCheckBox = new CheckBox();
            valueCheckBox.CheckAlign = ContentAlignment.MiddleRight;
            valueCheckBox.CheckState = CheckState.Unchecked;
            valueCheckBox.BackColor = Color.Transparent;
            return valueCheckBox;
        }

        public void SetUseCheckbox(bool useCheckbox)
        {
            if (useCheckbox)
            {
                _valueTextBox.Visible = false;
                _valueCheckBox.Visible = true;
            }
            else
            {
                _valueTextBox.Visible = true;
                _valueCheckBox.Visible = false;
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
        */

        private void OnTextValueKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                EditMode = false;
                return;
            }

            if (e.KeyData == Keys.Enter)
            {
                bool success = _varX.SetValueFromTextbox(_valueTextBox.Text);
                EditMode = false;
                if (!success) InvokeFailure();
                return;
            }
        }

        public void UpdateControl()
        {
            if (!EditMode)
            {
                _valueTextBox.Text = _varX.GetValueForTextbox();
                _valueCheckBox.CheckState = _varX.GetValueForCheckbox();
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

            BackColor = _currentColor;
            if (!EditMode) _valueTextBox.BackColor = _currentColor;
        }

        public void InvokeFailure()
        {
            _justFailed = true;
            _lastFailureTime = DateTime.Now;
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _borderPen?.Dispose();
        }
    }
}
