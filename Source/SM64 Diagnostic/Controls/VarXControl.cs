using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class VarXControl : TableLayoutPanel
    {
        public readonly string VarName;
        public readonly List<VariableGroup> GroupList;

        private readonly VarXPrecursor _varXPrecursor;
        private readonly VarX _varX;

        private readonly Label _nameLabel;
        private readonly TextBox _valueTextBox;
        private readonly CheckBox _valueCheckBox;
        private readonly ContextMenuStrip _contextMenuStrip;
        private readonly ContextMenuStrip _textboxOldContextMenuStrip;

        public string TextBoxValue
        {
            get { return _valueTextBox.Text; }
            set { _valueTextBox.Text = value; }
        }

        public CheckState CheckBoxValue
        {
            get { return _valueCheckBox.CheckState; }
            set { _valueCheckBox.CheckState = value; }
        }

        private static readonly Pen _borderPen = new Pen(Color.Red, 5);
        private static readonly int FAILURE_DURATION_MS = 1000;
        private static readonly Color FAILURE_COLOR = Color.Red;
        private static readonly Color DEFAULT_COLOR = SystemColors.Control;
        private readonly Color _baseColor;
        private Color _currentColor;
        private bool _justFailed;
        private DateTime _lastFailureTime;

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

        private static Image _lockedImage = new Bitmap(Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("SM64_Diagnostic.EmbeddedResources.lock.png")), new Size(16, 16));
        private static Image _someLockedImage = _lockedImage.GetOpaqueImage(0.5f);

        // TODO refactor this
        private static readonly int nameLabelHeight = 20;

        public VarXControl(
            VarXPrecursor varXPrecursor,
            string name,
            AddressHolder addressHolder,
            VarXSubclass varXSubclass,
            Color? backgroundColor,
            bool? useHex,
            bool? invertBool,
            VarXCoordinate? coordinate,
            List<VariableGroup> groupList)
        {
            // Store the precursor
            _varXPrecursor = varXPrecursor;

            // Initialize main fields
            VarName = name;
            GroupList = groupList;
            _showBorder = false;
            _editMode = false;

            // Initialize color fields
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

            // Create var x
            _varX = VarX.CreateVarX(addressHolder, this, varXSubclass, useHex, invertBool, coordinate);

            // Initialize context menu strip
            _textboxOldContextMenuStrip = _valueTextBox.ContextMenuStrip;
            _contextMenuStrip = _varX.GetContextMenuStrip();
            _nameLabel.ContextMenuStrip = _contextMenuStrip;
            _valueTextBox.ContextMenuStrip = _contextMenuStrip;
            base.ContextMenuStrip = _contextMenuStrip;

            // Set whether to start as a checkbox
            SetUseCheckbox(_varX.StartsAsCheckbox());

            // Add functions
            _nameLabel.Click += (sender, e) => _varX.ShowVarInfo();
            _valueTextBox.KeyDown += (sender, e) => OnTextValueKeyDown(e);
            _valueTextBox.DoubleClick += (sender, e) => { EditMode = true; };
            _valueTextBox.Leave += (sender, e) => { EditMode = false; };
            _valueCheckBox.Click += (sender, e) => OnCheckboxClick();

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

        private void OnCheckboxClick()
        {
            bool success = _varX.SetValueFromCheckbox(_valueCheckBox.CheckState);
            if (!success) InvokeFailure();
        }

        public void UpdateControl()
        {
            if (!EditMode)
            {
                if (_valueTextBox.Visible) _valueTextBox.Text = _varX.GetValueForTextbox();
                if (_valueCheckBox.Visible) _valueCheckBox.CheckState = _varX.GetValueForCheckbox();
            }

            _varX.UpdateItemCheckStates();
            _nameLabel.Image = GetImageForCheckState(_varX.GetLockedCheckState());

            UpdateColor();
        }

        private Image GetImageForCheckState(CheckState checkState)
        {
            switch (checkState)
            {
                case CheckState.Unchecked:
                    return null;
                case CheckState.Checked:
                    return _lockedImage;
                case CheckState.Indeterminate:
                    return _someLockedImage;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            if (!_editMode) _valueTextBox.BackColor = _currentColor;
        }

        private void InvokeFailure()
        {
            _justFailed = true;
            _lastFailureTime = DateTime.Now;
        }

        public bool BelongsToGroup(VariableGroup variableGroup)
        {
            return GroupList.Contains(variableGroup);
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
    }
}
