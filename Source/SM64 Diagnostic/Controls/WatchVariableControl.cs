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
    public class WatchVariableControl : TableLayoutPanel
    {
        public readonly string VarName;
        public readonly List<VariableGroup> GroupList;

        private readonly WatchVariableControlPrecursor _watchVarPrecursor;
        private readonly WatchVariableWrapper _watchVarWrapper;

        private readonly BetterTextbox _nameTextBox;
        private readonly TextBox _valueTextBox;
        private readonly CheckBox _valueCheckBox;
        private readonly ContextMenuStrip _valueTextboxOriginalContextMenuStrip;
        private readonly ContextMenuStrip _nameTextboxOriginalContextMenuStrip;

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
                _valueTextBox.ReadOnly = !_editMode;
                _valueTextBox.BackColor = _editMode ? Color.White : _currentColor;
                _valueTextBox.ContextMenuStrip = _editMode ? _valueTextboxOriginalContextMenuStrip : ContextMenuStrip;
                if (_editMode)
                {
                    _valueTextBox.Focus();
                    _valueTextBox.SelectAll();
                }
            }
        }

        private bool _renameMode;
        public bool RenameMode
        {
            get
            {
                return _renameMode;
            }
            set
            {
                _renameMode = value;
                _nameTextBox.ReadOnly = !_renameMode;
                _nameTextBox.BackColor = _renameMode ? Color.White : _currentColor;
                _nameTextBox.ContextMenuStrip = _renameMode ? _nameTextboxOriginalContextMenuStrip : ContextMenuStrip;
                if (_renameMode)
                {
                    _nameTextBox.Focus();
                    _nameTextBox.SelectAll();
                }
            }
        }

        public List<uint> FixedAddressList;

        private static Image _lockedImage = new Bitmap(Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("SM64_Diagnostic.EmbeddedResources.lock.png")), new Size(16, 16));
        private static Image _someLockedImage = _lockedImage.GetOpaqueImage(0.5f);

        // TODO refactor this
        private static readonly int nameLabelHeight = 20;

        public WatchVariableControl(
            WatchVariableControlPrecursor watchVarPrecursor,
            string name,
            WatchVariable watchVar,
            WatchVariableSubclass subclass,
            Color? backgroundColor,
            bool? useHex,
            bool? invertBool,
            WatchVariableCoordinate? coordinate,
            List<VariableGroup> groupList)
        {
            // Store the precursor
            _watchVarPrecursor = watchVarPrecursor;

            // Initialize main fields
            VarName = name;
            GroupList = groupList;
            _showBorder = false;
            _editMode = false;
            _renameMode = false;
            FixedAddressList = null;

            // Initialize color fields
            _baseColor = backgroundColor ?? DEFAULT_COLOR;
            _currentColor = _baseColor;
            _justFailed = false;
            _lastFailureTime = DateTime.Now;

            // Initialize control fields
            InitializeBase();
            _nameTextBox = CreateNameTextBox();
            _valueTextBox = CreateValueTextBox();
            _valueCheckBox = CreateValueCheckBox();
            base.Controls.Add(_nameTextBox, 0, 0);
            base.Controls.Add(_valueTextBox, 1, 0);
            base.Controls.Add(_valueCheckBox, 1, 0);

            // Create var x
            _watchVarWrapper = WatchVariableWrapper.CreateWatchVariableWrapper(watchVar, this, subclass, useHex, invertBool, coordinate);

            // Initialize context menu strip
            _valueTextboxOriginalContextMenuStrip = _valueTextBox.ContextMenuStrip;
            _nameTextboxOriginalContextMenuStrip = _nameTextBox.ContextMenuStrip;
            ContextMenuStrip = _watchVarWrapper.GetContextMenuStrip();
            _nameTextBox.ContextMenuStrip = ContextMenuStrip;
            _valueTextBox.ContextMenuStrip = ContextMenuStrip;

            // Set whether to start as a checkbox
            SetUseCheckbox(_watchVarWrapper.StartsAsCheckbox());

            // Add functions
            _nameTextBox.Click += (sender, e) => _watchVarWrapper.ShowVarInfo();
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

        private BetterTextbox CreateNameTextBox()
        {
            BetterTextbox nameTextBox = new BetterTextbox();
            nameTextBox.Text = VarName;
            nameTextBox.Cursor = Cursors.Default;
            nameTextBox.ReadOnly = true;
            nameTextBox.BorderStyle = BorderStyle.None;
            nameTextBox.TextAlign = HorizontalAlignment.Left;
            nameTextBox.Width = 200;
            nameTextBox.Margin = new Padding(3, 3, 3, 3);
            return nameTextBox;
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
                bool success = _watchVarWrapper.SetStringValue(_valueTextBox.Text, FixedAddressList);
                EditMode = false;
                if (!success) InvokeFailure();
                return;
            }
        }

        private void OnCheckboxClick()
        {
            bool success = _watchVarWrapper.SetCheckStateValue(_valueCheckBox.CheckState, FixedAddressList);
            if (!success) InvokeFailure();
        }

        public void UpdateControl()
        {
            if (!EditMode)
            {
                if (_valueTextBox.Visible) _valueTextBox.Text = _watchVarWrapper.GetStringValue(true, true, FixedAddressList);
                if (_valueCheckBox.Visible) _valueCheckBox.CheckState = _watchVarWrapper.GetCheckStateValue(FixedAddressList);
            }

            _watchVarWrapper.UpdateItemCheckStates();
            //_nameTextBox.Image = GetImageForCheckState(_watchVarWrapper.GetLockedCheckState(FixedAddressList));

            UpdateColor();
        }

        private static Image GetImageForCheckState(CheckState checkState)
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
            _nameTextBox.BackColor = _currentColor;
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

        public bool BelongsToAnyGroup(List<VariableGroup> variableGroups)
        {
            return variableGroups.Any(varGroup => BelongsToGroup(varGroup));
        }

        public void NotifyPanel(WatchVariablePanel panel)
        {
            _watchVarWrapper.NotifyPanel(panel);
        }

        public void NotifyInCustomTab()
        {
            _watchVarWrapper.NotifyInCustomTab();
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

        public WatchVariableControl CreateCopy()
        {
            return _watchVarPrecursor.CreateWatchVariableControl();
        }
    }
}
