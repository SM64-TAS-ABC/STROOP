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
        public readonly string Name;
        public readonly string TypeName;
        public readonly Type Type;
        public readonly int ByteCount;

        public VarX(
            string name,
            BaseAddressTypeEnum offset,
            List<VariableGroup> groupList,
            string specialType,
            Color? backgroundColor,
            AddressHolder addressHolder,
            bool useHex,
            ulong? mask,
            bool isBool,
            bool isObject,
            string typeName,
            bool invertBool,
            bool isAngle)
        {
            Name = name;
            AddressHolder = addressHolder;

            if (IsSpecial) return;

            TypeName = typeName;
            Type = VarXUtilities.StringToType[TypeName];
            ByteCount = VarXUtilities.TypeSize[Type];

            CreateControls();
        }

        public bool IsSpecial
        {
            get
            {
                return AddressHolder.BaseAddressType == BaseAddressTypeEnum.Special;
            }
        }














        BorderedTableLayoutPanel _tablePanel;
        Label _nameLabel;
        TextBox _textBox;

        bool _editMode = false;



        public string Value
        {
            get
            {
                if (_textBox == null)
                    return "";

                return _textBox.Text;
            }
            set
            {
                if (_textBox == null)
                    return;

                _textBox.Text = value;
            }
        }

        public Control Control
        {
            get
            {
                return _tablePanel;
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

        private void _nameLabel_Click(object sender, EventArgs e)
        {
            VariableViewerForm varInfo;
            var typeDescr = TypeName;

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

            foreach (uint address in AddressHolder.EffectiveAddressList)
            {
                object value = Config.Stream.GetValue(Type, address, AddressHolder.UseAbsoluteAddressing);
                _textBox.Text = value.ToString();
            }

            /*
            bool firstBaseAddress = true;
            foreach (var baseAddress in AddressHolder.BaseAddressList)
            {
                string newText = GetStringValue(baseAddress);

                if (firstBaseAddress)
                {
                    _textBox.Text = newText;
                }
                else if (_textBox.Text != newText)
                {
                    _textBox.Text = "";
                    continue;
                }

                firstBaseAddress = false;
            }
            */
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
            EditMode = false;

            Config.Stream.Suspend();

            foreach (uint address in AddressHolder.EffectiveAddressList)
            {
                Config.Stream.SetValue(Type, _textBox.Text, address, AddressHolder.UseAbsoluteAddressing);
            }

            Config.Stream.Resume();
        }



















        public byte[] GetByteData(uint offset)
        {
            // Get dataBytes
            var dataBytes = Config.Stream.ReadRamLittleEndian(new UIntPtr(AddressHolder.EffectiveAddress),
                ByteCount, AddressHolder.UseAbsoluteAddressing);

            // Make sure offset is a valid pointer
            if (AddressHolder.IsAdditive && offset == 0)
                return null;

            return dataBytes;
        }

        public string GetStringValue(uint offset)
        {
            // Get dataBytes
            var dataBytes = GetByteData(offset);

            // Make sure offset is a valid pointer
            if (dataBytes == null)
                return "(none)";

            // Parse floating point
            if ((Type == typeof(float) || Type == typeof(double)))
            {
                if (Type == typeof(float))
                    return BitConverter.ToSingle(dataBytes, 0).ToString();

                if (Type == typeof(double))
                    return BitConverter.ToDouble(dataBytes, 0).ToString();
            }

            // Get Uint64 value
            var intBytes = new byte[8];
            dataBytes.CopyTo(intBytes, 0);
            UInt64 dataValue = BitConverter.ToUInt64(intBytes, 0);
            
            // Print signed
            if (Type == typeof(Int64))
                return ((Int64)dataValue).ToString();
            else if (Type == typeof(Int32))
                return ((Int32)dataValue).ToString();
            else if (Type == typeof(Int16))
                return ((Int16)dataValue).ToString();
            else if (Type == typeof(sbyte))
                return ((sbyte)dataValue).ToString();
            else
                return dataValue.ToString();
        }
        
        public byte[] GetBytesFromString(uint offset, string value)
        {
            // Get dataBytes
            var address = AddressHolder.EffectiveAddress;
            var dataBytes = new byte[8];
            Config.Stream.ReadRamLittleEndian(new UIntPtr(address), ByteCount, AddressHolder.UseAbsoluteAddressing).CopyTo(dataBytes, 0);
            UInt64 oldValue = BitConverter.ToUInt64(dataBytes, 0);
            UInt64 newValue;

            // Handle hex variable
            if (ParsingUtilities.IsHex(value))
            {
                if (!ParsingUtilities.TryParseExtHex(value, out newValue))
                    return null;
            }
            // Handle floats
            else if (Type == typeof(float))
            {
                float newFloatValue;
                if (!float.TryParse(value, out newFloatValue))
                    return null;

                // Get bytes
                newValue = BitConverter.ToUInt32(BitConverter.GetBytes(newFloatValue), 0);
            }
            else if (Type == typeof(double))
            {
                double newFloatValue;
                if (double.TryParse(value, out newFloatValue))
                    return null;

                // Get bytes
                newValue = BitConverter.ToUInt64(BitConverter.GetBytes(newFloatValue), 0);
            }
            else if (Type == typeof(UInt64))
            {
                if (!UInt64.TryParse(value, out newValue))
                {
                    Int64 newValueInt;
                    if (!Int64.TryParse(value, out newValueInt))
                        return null;

                    newValue = (UInt64)newValueInt;
                }
            }
            else
            {
                Int64 tempInt;
                if (!Int64.TryParse(value, out tempInt))
                    return null;
                newValue = (UInt64)tempInt;
            }

            var writeBytes = new byte[ByteCount];
            var valueBytes = BitConverter.GetBytes(newValue);
            Array.Copy(valueBytes, 0, writeBytes, 0, ByteCount);

            return writeBytes;
        }

        public bool SetBytes(uint offset, byte[] dataBytes)
        {
            if (dataBytes == null)
                return false;

            return Config.Stream.WriteRamLittleEndian(dataBytes, AddressHolder.EffectiveAddress, AddressHolder.UseAbsoluteAddressing);
        }
    }
}
