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
        public readonly AddressHolder AddressHolder;

        protected readonly VarXControl _varXControl;

        private bool _highlighted;

        

        private static readonly int FAILURE_DURATION_MS = 1000;
        private static readonly Color FAILURE_COLOR = Color.Red;
        private static readonly Color DEFAULT_COLOR = SystemColors.Control;

        private readonly Color _baseColor;
        private Color _currentColor;
        private bool _justFailed;
        private DateTime _lastFailureTime;

        public static VarX CreateVarX(
            string name,
            AddressHolder addressHolder,
            VarXSubclass varXSubclcass,
            Color? backgroundColor,
            bool invertBool = false)
        {
            switch (varXSubclcass)
            {
                case VarXSubclass.String:
                    return new VarX(name, addressHolder, backgroundColor);

                case VarXSubclass.Number:
                    return new VarXNumber(name, addressHolder, backgroundColor);

                case VarXSubclass.UnsignedAngle:
                    return new VarXAngle(name, addressHolder, backgroundColor, false);
                case VarXSubclass.SignedAngle:
                    return new VarXAngle(name, addressHolder, backgroundColor, true);

                case VarXSubclass.Object:
                    return new VarXObject(name, addressHolder, backgroundColor);

                case VarXSubclass.Boolean:
                    return new VarXBoolean(name, addressHolder, backgroundColor, invertBool);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public VarX(string name, AddressHolder addressHolder, Color? backgroundColor, bool useCheckbox = false)
        {
            AddressHolder = addressHolder;

            _varXControl = new VarXControl(this, name, useCheckbox);

            _baseColor = backgroundColor ?? DEFAULT_COLOR;
            _currentColor = _baseColor;

            _highlighted = false;
            _justFailed = false;
            _lastFailureTime = DateTime.Now;

            AddContextMenuStripItems();
        }



        public Control Control
        {
            get
            {
                return _varXControl;
            }
        }

        protected void AddContextMenuStripItems()
        {
            ToolStripMenuItem itemHighlight = new ToolStripMenuItem("Highlight");
            itemHighlight.Click += (sender, e) =>
            {
                _highlighted = !_highlighted;
                _varXControl.ShowBorder = _highlighted;
                itemHighlight.Checked = _highlighted;
            };
            itemHighlight.Checked = _highlighted;

            ToolStripMenuItem itemEdit = new ToolStripMenuItem("Edit");
            itemEdit.Click += (sender, e) => { _varXControl.EditMode = true; };

            ToolStripMenuItem itemCopyAsIs = new ToolStripMenuItem("Copy (As Is)");
            itemCopyAsIs.Click += (sender, e) => { Clipboard.SetText(_varXControl._textBox.Text); };

            ToolStripMenuItem itemCopyUnrounded = new ToolStripMenuItem("Copy (Unrounded)");
            itemCopyUnrounded.Click += (sender, e) => { Clipboard.SetText(GetValueForTextbox(false)); };

            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");
            itemPaste.Click += (sender, e) => { SetValueFromTextbox(Clipboard.GetText()); };

            _varXControl._contextMenuStrip.Items.Add(itemHighlight);
            _varXControl._contextMenuStrip.Items.Add(itemEdit);
            _varXControl._contextMenuStrip.Items.Add(itemCopyAsIs);
            _varXControl._contextMenuStrip.Items.Add(itemCopyUnrounded);
            _varXControl._contextMenuStrip.Items.Add(itemPaste);
        }

        public void _nameLabel_Click()
        {
            VariableViewerForm varInfo;
            string typeDescr = AddressHolder.MemoryTypeName;

            varInfo = new VariableViewerForm(_varXControl.Name, typeDescr,
                String.Format("0x{0:X8}", AddressHolder.GetRamAddress()),
                String.Format("0x{0:X8}", AddressHolder.GetProcessAddress().ToUInt64()));

            varInfo.ShowDialog();
        }

        public void _textBoxValue_DoubleClick()
        {
            _varXControl.EditMode = true;
        }

        public void InvokeFailure()
        {
            _justFailed = true;
            _lastFailureTime = DateTime.Now;
        }

        public void Update()
        {
            if (!_varXControl.EditMode)
            {
                _varXControl._textBox.Text = GetValueForTextbox();
                _varXControl._checkBoxBool.CheckState = GetValueForCheckbox();
            }

            UpdateColor();
        }

        public void UpdateColor()
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

            _varXControl.BackColor = _currentColor;
            if (!_varXControl.EditMode) _varXControl._textBox.BackColor = _currentColor;
        }

        public void OnTextValueKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                _varXControl.EditMode = false;
                return;
            }

            if (e.KeyData == Keys.Enter)
            {
                bool success = SetValueFromTextbox(_varXControl._textBox.Text);
                _varXControl.EditMode = false;
                if (!success)
                {
                    InvokeFailure();
                }
                return;
            }
        }






        public string GetValueForTextbox(bool handleRounding = true)
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

        public bool SetValueFromTextbox(string value)
        {
            value = HandleObjectUndisplaying(value);
            value = HandleHexUndisplaying(value);
            value = HandleUnnegating(value);
            value = HandleAngleUnconverting(value);

            return AddressHolder.SetValue(value);
        }


        public CheckState GetValueForCheckbox()
        {
            List<string> values = AddressHolder.GetValues();
            List<CheckState> checkStates = values.ConvertAll(value => ConvertValueToCheckState(value));
            CheckState checkState = CombineCheckStates(checkStates);
            return checkState;
        }

        public bool SetValueFromCheckbox(CheckState checkState)
        {
            string value = ConvertCheckStateToValue(checkState);
            return AddressHolder.SetValue(value);
        }




        protected (bool meaningfulValue, string stringValue) CombineValues(List<string> values)
        {
            if (values.Count == 0) return (false, "(none)");
            string firstValue = values[0];
            for (int i = 1; i < values.Count; i++)
            {
                if (values[i] != firstValue) return (false, "multiple values");
            }
            return (true, firstValue);
        }

        protected CheckState CombineCheckStates(List<CheckState> checkStates)
        {
            if (checkStates.Count == 0) return CheckState.Unchecked;
            CheckState firstCheckState = checkStates[0];
            for (int i = 1; i < checkStates.Count; i++)
            {
                if (checkStates[i] != firstCheckState) return CheckState.Indeterminate;
            }
            return firstCheckState;
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

        // Boolean methods

        public virtual CheckState ConvertValueToCheckState(string value)
        {
            return CheckState.Unchecked;
        }

        public virtual string ConvertCheckStateToValue(CheckState checkState)
        {
            return "";
        }


    }
}
