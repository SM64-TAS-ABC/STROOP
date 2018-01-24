using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
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
    public class VarHackContainer : TableLayoutPanel
    {
        private CheckBox checkBoxUsePointer;
        private CheckBox checkBoxNoNumber;
        private BetterTextbox textBoxNameValue;
        private BetterTextbox textBoxNameLabel;
        private BetterTextbox textBoxAddressLabel;
        private BetterTextbox textBoxAddressValue;
        private BetterTextbox textBoxPointerOffsetLabel;
        private BetterTextbox textBoxPointerOffsetValue;
        private BetterTextbox textBoxXPosLabel;
        private BetterTextbox textBoxXPosValue;
        private BetterTextbox textBoxYPosValue;
        private BetterTextbox textBoxYPosLabel;
        private CheckBox checkBoxUseHex;
        private RadioButton radioButtonSByte;
        private RadioButton radioButtonByte;
        private RadioButton radioButtonShort;
        private RadioButton radioButtonUShort;
        private RadioButton radioButtonInt;
        private RadioButton radioButtonUInt;
        private RadioButton radioButtonFloat;
        private PictureBox pictureBoxUpArrow;
        private PictureBox pictureBoxDownArrow;
        private PictureBox pictureBoxRedX;

        private Func<string> _getterFunction;

        private static readonly Pen _borderPen = new Pen(Color.Black, 3);

        private readonly VarHackPanel _varHackPanel;

        public VarHackContainer(VarHackPanel varHackPanel, int creationIndex, bool usePreWrittenVar)
        {
            InitializeComponent();
            _varHackPanel = varHackPanel;
            _getterFunction = null;

            pictureBoxUpArrow.Click += (sender, e) => _varHackPanel.MoveUpControl(this);
            pictureBoxDownArrow.Click += (sender, e) => _varHackPanel.MoveDownControl(this);
            pictureBoxRedX.Click += (sender, e) => _varHackPanel.RemoveControl(this);
            checkBoxUsePointer.Click += (sender, e) => textBoxPointerOffsetValue.Enabled = checkBoxUsePointer.Checked;

            SetDefaultValues(creationIndex, usePreWrittenVar);
        }

        public VarHackContainer(
            VarHackPanel varHackPanel,
            int creationIndex,
            string varName,
            uint address,
            Type memoryType,
            bool useHex,
            uint? pointerOffset)
            : this(varHackPanel, creationIndex, false)
        {
            textBoxNameValue.Text = varName + " ";
            textBoxAddressValue.Text = "0x" + String.Format("{0:X}", address);
            GetRadioButtonForType(memoryType).Checked = true;
            checkBoxUseHex.Checked = useHex;

            if (pointerOffset.HasValue)
            {
                checkBoxUsePointer.Checked = true;
                textBoxPointerOffsetValue.Enabled = true;
                textBoxPointerOffsetValue.Text = "0x" + String.Format("{0:X}", pointerOffset.Value);
            }
        }

        public VarHackContainer(
            VarHackPanel varHackPanel,
            int creationIndex,
            Func<string> getterFunction)
            : this(varHackPanel, creationIndex, false)
        {
            _getterFunction = getterFunction;
        }

        private void SetDefaultValues(int creationIndex, bool usePreWrittenVar)
        {
            int xPos = 10;
            int yPos = 192 - creationIndex * 17;
            textBoxXPosValue.Text = xPos.ToString();
            textBoxYPosValue.Text = yPos.ToString();
            if (!usePreWrittenVar) return;

            string name;
            uint address;
            RadioButton typeRadioButton;
            bool useHex = false;
            bool usePointer = false;
            uint pointerOffset = 0;

            switch (creationIndex)
            {
                case 0:
                    name = "HSPD ";
                    address = Config.Mario.StructAddress + Config.Mario.HSpeedOffset;
                    typeRadioButton = radioButtonFloat;
                    break;
                case 1:
                    name = "Angle ";
                    address = Config.Mario.StructAddress + Config.Mario.YawFacingOffset;
                    typeRadioButton = radioButtonUShort;
                    break;
                case 2:
                    name = "HP ";
                    address = Config.Mario.StructAddress + Config.Hud.HpCountOffset;
                    typeRadioButton = radioButtonShort;
                    useHex = true;
                    break;
                case 3:
                    name = "Floor Room ";
                    address = Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset;
                    typeRadioButton = radioButtonByte;
                    usePointer = true;
                    pointerOffset = 0x05;
                    break;
                case 4:
                    name = "X ";
                    address = Config.Mario.StructAddress + Config.Mario.XOffset;
                    typeRadioButton = radioButtonFloat;
                    break;
                case 5:
                    name = "Y ";
                    address = Config.Mario.StructAddress + Config.Mario.YOffset;
                    typeRadioButton = radioButtonFloat;
                    break;
                case 6:
                    name = "Z ";
                    address = Config.Mario.StructAddress + Config.Mario.ZOffset;
                    typeRadioButton = radioButtonFloat;
                    break;
                case 7:
                    name = "HOLP X ";
                    address = Config.Mario.StructAddress + Config.Mario.HOLPXOffset;
                    typeRadioButton = radioButtonFloat;
                    break;
                case 8:
                    name = "HOLP Y ";
                    address = Config.Mario.StructAddress + Config.Mario.HOLPYOffset;
                    typeRadioButton = radioButtonFloat;
                    break;
                case 9:
                default:
                    name = "HOLP Z ";
                    address = Config.Mario.StructAddress + Config.Mario.HOLPZOffset;
                    typeRadioButton = radioButtonFloat;
                    break;
            }

            textBoxNameValue.Text = name;
            textBoxAddressValue.Text = "0x" + String.Format("{0:X}", address);
            typeRadioButton.Checked = true;

            if (useHex) checkBoxUseHex.Checked = true;
            if (usePointer)
            {
                checkBoxUsePointer.Checked = true;
                textBoxPointerOffsetValue.Enabled = true;
                textBoxPointerOffsetValue.Text = "0x" + String.Format("{0:X}", pointerOffset);
            }

        }

        public byte[] GetBigEndianByteArray()
        {
            string name = GetCurrentName();
            uint? addressNullable = GetCurrentAddress();
            bool usePointer = GetCurrentUsePointer();
            ushort? pointerOffsetNullable = GetCurrentPointerOffset();
            byte typeByte = GetCurrentTypeByte();
            bool signed = GetCurrentSigned();
            bool useHex = GetCurrentUseHex();
            bool noNumber = GetCurrentNoNumber();
            ushort? xPosNullable = GetCurrentXPosition();
            ushort? yPosNullable = GetCurrentYPosition();

            if (!addressNullable.HasValue) return null;
            uint address = addressNullable.Value;
            if (!pointerOffsetNullable.HasValue && usePointer) return null;
            ushort pointerOffset = usePointer ? pointerOffsetNullable.Value : (ushort)0;
            if (!xPosNullable.HasValue) return null;
            ushort xPos = xPosNullable.Value;
            if (!yPosNullable.HasValue) return null;
            ushort yPos = yPosNullable.Value;

            byte[] bytes = new byte[Config.VarHack.StructSize];

            byte[] addressBytes = BitConverter.GetBytes(address);
            WriteBytes(addressBytes, bytes, Config.VarHack.AddressOffset, true);

            byte[] xPosBytes = BitConverter.GetBytes(xPos);
            WriteBytes(xPosBytes, bytes, Config.VarHack.XPosOffset, true);

            byte[] yPosBytes = BitConverter.GetBytes(yPos);
            WriteBytes(yPosBytes, bytes, Config.VarHack.YPosOffset, true);

            string cappedName = CapString(name, !noNumber);
            string numberAddon = noNumber ? "" : (useHex ? "%x" : "%d");
            string cappedNameAndNumberAddon = cappedName + numberAddon;
            byte[] nameAndNumberAddonBytes = Encoding.ASCII.GetBytes(cappedNameAndNumberAddon);
            WriteBytes(nameAndNumberAddonBytes, bytes, Config.VarHack.StringOffset, false);

            byte[] usePointerBytes = BitConverter.GetBytes(usePointer);
            WriteBytes(usePointerBytes, bytes, Config.VarHack.UsePointerOffset, true);

            if (usePointer)
            {
                byte[] pointerOffsetBytes = BitConverter.GetBytes(pointerOffset);
                WriteBytes(pointerOffsetBytes, bytes, Config.VarHack.PointerOffsetOffset, true);
            }

            byte[] signedBytes = BitConverter.GetBytes(signed);
            WriteBytes(signedBytes, bytes, Config.VarHack.SignedOffset, true);

            byte[] typeBytes = new byte[] { typeByte };
            WriteBytes(typeBytes, bytes, Config.VarHack.TypeOffset, true);

            return bytes;
        }

        public byte[] GetLittleEndianByteArray()
        {
            byte[] bigEndianArray = GetBigEndianByteArray();
            if (bigEndianArray == null) return null;
            byte[] littleEndianArray = new byte[bigEndianArray.Length];

            for (int i = 0; i < bigEndianArray.Length; i++)
            {
                int baseValue = (i / 4) * 4;
                int offsetValue = 3 - (i % 4);
                int newIndex = baseValue + offsetValue;
                littleEndianArray[newIndex] = bigEndianArray[i];
            }

            return littleEndianArray;
        }

        private void WriteBytes(byte[] bytesToWrite, byte[] byteHolder, uint offset, bool reversedOrder)
        {
            for (int i = 0; i < bytesToWrite.Length; i++)
            {
                int byteHolderOffset = (int)(reversedOrder ? offset + bytesToWrite.Length - 1 - i : offset + i);
                byteHolder[byteHolderOffset] = bytesToWrite[i];
            }
        }

        public static string ConvertBytesToString(byte[] bytes)
        {
            if (bytes == null) return "";
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                stringBuilder.Append(String.Format("{0:X2}", bytes[i]));
                stringBuilder.Append(" ");
                if (i % 16 == 15) stringBuilder.Append("\r\n");
            }
            return stringBuilder.ToString();
        }

        private string CapString(string text, bool factorInNumberAddon = true)
        {
            int maxLength = Config.VarHack.MaxStringLength + (factorInNumberAddon ? 0 : 2);
            return text.Length > maxLength ? text.Substring(0, maxLength) : text;
        }

        public void SetPosition(int xPos, int yPos)
        {
            textBoxXPosValue.Text = xPos.ToString();
            textBoxYPosValue.Text = yPos.ToString();
        }

        private string GetCurrentName()
        {
            return textBoxNameValue.Text;
        }

        private uint? GetCurrentAddress()
        {
            return ParsingUtilities.ParseHexNullable(textBoxAddressValue.Text);
        }

        private bool GetCurrentUsePointer()
        {
            return checkBoxUsePointer.Checked;
        }

        private ushort? GetCurrentPointerOffset()
        {
            return ParsingUtilities.ParseUShortNullable(
                ParsingUtilities.ParseHexNullable(textBoxPointerOffsetValue.Text));
        }

        private byte GetCurrentTypeByte()
        {
            Type type = GetCurrentType();
            if (type == typeof(sbyte)) return 0x08;
            if (type == typeof(byte)) return 0x08;
            if (type == typeof(short)) return 0x10;
            if (type == typeof(ushort)) return 0x10;
            if (type == typeof(int)) return 0x20;
            if (type == typeof(uint)) return 0x20;
            if (type == typeof(float)) return 0x40;
            throw new ArgumentOutOfRangeException();
        }

        private bool GetCurrentSigned()
        {
            Type type = GetCurrentType();
            if (type == typeof(sbyte)) return true;
            if (type == typeof(byte)) return false;
            if (type == typeof(short)) return true;
            if (type == typeof(ushort)) return false;
            if (type == typeof(int)) return true;
            if (type == typeof(uint)) return false;
            if (type == typeof(float)) return true;
            throw new ArgumentOutOfRangeException();
        }

        private Type GetCurrentType()
        {
            if (radioButtonSByte.Checked) return typeof(sbyte);
            if (radioButtonByte.Checked) return typeof(byte);
            if (radioButtonShort.Checked) return typeof(short);
            if (radioButtonUShort.Checked) return typeof(ushort);
            if (radioButtonInt.Checked) return typeof(int);
            if (radioButtonUInt.Checked) return typeof(uint);
            if (radioButtonFloat.Checked) return typeof(float);
            throw new ArgumentOutOfRangeException();
        }

        private RadioButton GetRadioButtonForType(Type type)
        {
            if (type == typeof(sbyte)) return radioButtonSByte;
            if (type == typeof(byte)) return radioButtonByte;
            if (type == typeof(short)) return radioButtonShort;
            if (type == typeof(ushort)) return radioButtonUShort;
            if (type == typeof(int)) return radioButtonInt;
            if (type == typeof(uint)) return radioButtonUInt;
            if (type == typeof(float)) return radioButtonFloat;
            return radioButtonFloat;
        }

        private bool GetCurrentUseHex()
        {
            return checkBoxUseHex.Checked;
        }

        private bool GetCurrentNoNumber()
        {
            return checkBoxNoNumber.Checked;
        }

        private ushort? GetCurrentXPosition()
        {
            return ParsingUtilities.ParseUShortNullable(textBoxXPosValue.Text);
        }

        private ushort? GetCurrentYPosition()
        {
            return ParsingUtilities.ParseUShortNullable(textBoxYPosValue.Text);
        }

        public bool UpdatesContinuously()
        {
            return _getterFunction != null;
        }

        public void UpdateControl()
        {
            if (_getterFunction != null)
            {
                textBoxNameValue.Text = _getterFunction();
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VarHackContainerForm));
            this.checkBoxUsePointer = new System.Windows.Forms.CheckBox();
            this.checkBoxNoNumber = new System.Windows.Forms.CheckBox();
            this.textBoxNameValue = new SM64_Diagnostic.BetterTextbox();
            this.textBoxNameLabel = new SM64_Diagnostic.BetterTextbox();
            this.textBoxAddressLabel = new SM64_Diagnostic.BetterTextbox();
            this.textBoxAddressValue = new SM64_Diagnostic.BetterTextbox();
            this.textBoxPointerOffsetLabel = new SM64_Diagnostic.BetterTextbox();
            this.textBoxPointerOffsetValue = new SM64_Diagnostic.BetterTextbox();
            this.textBoxXPosLabel = new SM64_Diagnostic.BetterTextbox();
            this.textBoxYPosLabel = new SM64_Diagnostic.BetterTextbox();
            this.textBoxXPosValue = new SM64_Diagnostic.BetterTextbox();
            this.textBoxYPosValue = new SM64_Diagnostic.BetterTextbox();
            this.checkBoxUseHex = new System.Windows.Forms.CheckBox();
            this.radioButtonSByte = new System.Windows.Forms.RadioButton();
            this.radioButtonByte = new System.Windows.Forms.RadioButton();
            this.radioButtonShort = new System.Windows.Forms.RadioButton();
            this.radioButtonUShort = new System.Windows.Forms.RadioButton();
            this.radioButtonInt = new System.Windows.Forms.RadioButton();
            this.radioButtonUInt = new System.Windows.Forms.RadioButton();
            this.radioButtonFloat = new System.Windows.Forms.RadioButton();
            this.pictureBoxUpArrow = new System.Windows.Forms.PictureBox();
            this.pictureBoxDownArrow = new System.Windows.Forms.PictureBox();
            this.pictureBoxRedX = new System.Windows.Forms.PictureBox();
            /*
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUpArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRedX)).BeginInit();
            this.SuspendLayout();
            */
            // 
            // tableLayoutPanel1
            // 
            this.ColumnCount = 6;
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.Controls.Add(this.checkBoxUsePointer, 1, 2);
            this.Controls.Add(this.checkBoxNoNumber, 0, 2);
            this.Controls.Add(this.textBoxNameValue, 1, 0);
            this.Controls.Add(this.textBoxNameLabel, 0, 0);
            this.Controls.Add(this.textBoxAddressLabel, 0, 1);
            this.Controls.Add(this.textBoxAddressValue, 1, 1);
            this.Controls.Add(this.textBoxPointerOffsetLabel, 0, 3);
            this.Controls.Add(this.textBoxPointerOffsetValue, 1, 3);
            this.Controls.Add(this.radioButtonSByte, 2, 0);
            this.Controls.Add(this.radioButtonByte, 3, 0);
            this.Controls.Add(this.radioButtonShort, 2, 1);
            this.Controls.Add(this.radioButtonUShort, 3, 1);
            this.Controls.Add(this.radioButtonInt, 2, 2);
            this.Controls.Add(this.radioButtonUInt, 3, 2);
            this.Controls.Add(this.radioButtonFloat, 2, 3);
            this.Controls.Add(this.checkBoxUseHex, 3, 3);
            this.Controls.Add(this.textBoxXPosLabel, 4, 2);
            this.Controls.Add(this.textBoxXPosValue, 5, 2);
            this.Controls.Add(this.textBoxYPosLabel, 4, 3);
            this.Controls.Add(this.textBoxYPosValue, 5, 3);
            this.Controls.Add(this.pictureBoxUpArrow, 4, 0);
            this.Controls.Add(this.pictureBoxDownArrow, 4, 1);
            this.Controls.Add(this.pictureBoxRedX, 5, 0);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "tableLayoutPanel1";
            this.RowCount = 4;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.Size = new System.Drawing.Size(401, 99);
            this.TabIndex = 39;
            // 
            // checkBoxUsePointer
            // 
            this.checkBoxUsePointer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxUsePointer.AutoSize = true;
            this.checkBoxUsePointer.Location = new System.Drawing.Point(81, 51);
            this.checkBoxUsePointer.Name = "checkBoxUsePointer";
            this.checkBoxUsePointer.Size = new System.Drawing.Size(81, 17);
            this.checkBoxUsePointer.TabIndex = 4;
            this.checkBoxUsePointer.Text = "Use Pointer";
            this.checkBoxUsePointer.UseVisualStyleBackColor = true;
            // 
            // checkBoxNoNumber
            // 
            this.checkBoxNoNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxNoNumber.AutoSize = true;
            this.checkBoxNoNumber.Location = new System.Drawing.Point(81, 51);
            this.checkBoxNoNumber.Name = "checkBoxNoNumber";
            this.checkBoxNoNumber.Size = new System.Drawing.Size(81, 17);
            this.checkBoxNoNumber.TabIndex = 4;
            this.checkBoxNoNumber.Text = "No Num";
            this.checkBoxNoNumber.UseVisualStyleBackColor = true;
            this.checkBoxNoNumber.ForeColor = Color.DarkRed;
            // 
            // textBoxNameValue
            // 
            this.textBoxNameValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxNameValue.BackColor = System.Drawing.Color.White;
            this.textBoxNameValue.Location = new System.Drawing.Point(81, 3);
            this.textBoxNameValue.Name = "textBoxNameValue";
            this.textBoxNameValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxNameValue.TabIndex = 10;
            this.textBoxNameValue.Text = "Mario X";
            this.textBoxNameValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //this.textBoxNameValue.MaxLength = Config.VarHack.MaxStringLength;
            // 
            // textBoxNameLabel
            // 
            this.textBoxNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxNameLabel.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxNameLabel.Location = new System.Drawing.Point(3, 5);
            this.textBoxNameLabel.Name = "textBoxNameLabel";
            this.textBoxNameLabel.ReadOnly = true;
            this.textBoxNameLabel.Size = new System.Drawing.Size(72, 13);
            this.textBoxNameLabel.TabIndex = 10;
            this.textBoxNameLabel.Text = "Name:";
            this.textBoxNameLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxAddressLabel
            // 
            this.textBoxAddressLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxAddressLabel.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxAddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAddressLabel.Location = new System.Drawing.Point(3, 29);
            this.textBoxAddressLabel.Name = "textBoxAddressLabel";
            this.textBoxAddressLabel.ReadOnly = true;
            this.textBoxAddressLabel.Size = new System.Drawing.Size(72, 13);
            this.textBoxAddressLabel.TabIndex = 10;
            this.textBoxAddressLabel.Text = "Address:";
            this.textBoxAddressLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxAddressValue
            // 
            this.textBoxAddressValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxAddressValue.BackColor = System.Drawing.Color.White;
            this.textBoxAddressValue.Location = new System.Drawing.Point(81, 27);
            this.textBoxAddressValue.Name = "textBoxAddressValue";
            this.textBoxAddressValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxAddressValue.TabIndex = 10;
            this.textBoxAddressValue.Text = "0x8033B1AC";
            this.textBoxAddressValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxPointerOffsetLabel
            // 
            this.textBoxPointerOffsetLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxPointerOffsetLabel.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxPointerOffsetLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPointerOffsetLabel.Location = new System.Drawing.Point(3, 79);
            this.textBoxPointerOffsetLabel.Name = "textBoxPointerOffsetLabel";
            this.textBoxPointerOffsetLabel.ReadOnly = true;
            this.textBoxPointerOffsetLabel.Size = new System.Drawing.Size(72, 13);
            this.textBoxPointerOffsetLabel.TabIndex = 10;
            this.textBoxPointerOffsetLabel.Text = "Pointer Offset:";
            this.textBoxPointerOffsetLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxPointerOffsetValue
            // 
            this.textBoxPointerOffsetValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxPointerOffsetValue.BackColor = System.Drawing.Color.White;
            this.textBoxPointerOffsetValue.Enabled = false;
            this.textBoxPointerOffsetValue.Location = new System.Drawing.Point(81, 75);
            this.textBoxPointerOffsetValue.Name = "textBoxPointerOffsetValue";
            this.textBoxPointerOffsetValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxPointerOffsetValue.TabIndex = 10;
            this.textBoxPointerOffsetValue.Text = "0x10";
            this.textBoxPointerOffsetValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxXPosLabel
            // 
            this.textBoxXPosLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxXPosLabel.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxXPosLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxXPosLabel.Location = new System.Drawing.Point(317, 5);
            this.textBoxXPosLabel.Name = "textBoxXPosLabel";
            this.textBoxXPosLabel.ReadOnly = true;
            this.textBoxXPosLabel.Size = new System.Drawing.Size(30, 13);
            this.textBoxXPosLabel.TabIndex = 10;
            this.textBoxXPosLabel.Text = "X Pos:";
            this.textBoxXPosLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxYPosLabel
            // 
            this.textBoxYPosLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxYPosLabel.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxYPosLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxYPosLabel.Location = new System.Drawing.Point(317, 29);
            this.textBoxYPosLabel.Name = "textBoxYPosLabel";
            this.textBoxYPosLabel.ReadOnly = true;
            this.textBoxYPosLabel.Size = new System.Drawing.Size(30, 13);
            this.textBoxYPosLabel.TabIndex = 10;
            this.textBoxYPosLabel.Text = "Y Pos:";
            this.textBoxYPosLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxXPosValue
            // 
            this.textBoxXPosValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxXPosValue.BackColor = System.Drawing.Color.White;
            this.textBoxXPosValue.Location = new System.Drawing.Point(353, 3);
            this.textBoxXPosValue.Name = "textBoxXPosValue";
            this.textBoxXPosValue.Size = new System.Drawing.Size(45, 20);
            this.textBoxXPosValue.TabIndex = 10;
            this.textBoxXPosValue.Text = "100";
            this.textBoxXPosValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxYPosValue
            // 
            this.textBoxYPosValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxYPosValue.BackColor = System.Drawing.Color.White;
            this.textBoxYPosValue.Location = new System.Drawing.Point(353, 27);
            this.textBoxYPosValue.Name = "textBoxYPosValue";
            this.textBoxYPosValue.Size = new System.Drawing.Size(45, 20);
            this.textBoxYPosValue.TabIndex = 10;
            this.textBoxYPosValue.Text = "200";
            this.textBoxYPosValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxUseHex
            // 
            this.checkBoxUseHex.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxUseHex.AutoSize = true;
            this.checkBoxUseHex.Location = new System.Drawing.Point(244, 77);
            this.checkBoxUseHex.Name = "checkBoxUseHex";
            this.checkBoxUseHex.Size = new System.Drawing.Size(67, 17);
            this.checkBoxUseHex.TabIndex = 4;
            this.checkBoxUseHex.Text = "Use Hex";
            this.checkBoxUseHex.UseVisualStyleBackColor = true;
            // 
            // radioButtonSByte
            // 
            this.radioButtonSByte.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonSByte.AutoSize = true;
            this.radioButtonSByte.Location = new System.Drawing.Point(188, 3);
            this.radioButtonSByte.Name = "radioButtonSByte";
            this.radioButtonSByte.Size = new System.Drawing.Size(50, 17);
            this.radioButtonSByte.TabIndex = 11;
            this.radioButtonSByte.Text = "sbyte";
            this.radioButtonSByte.UseVisualStyleBackColor = true;
            // 
            // radioButtonByte
            // 
            this.radioButtonByte.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonByte.AutoSize = true;
            this.radioButtonByte.Location = new System.Drawing.Point(244, 3);
            this.radioButtonByte.Name = "radioButtonByte";
            this.radioButtonByte.Size = new System.Drawing.Size(45, 17);
            this.radioButtonByte.TabIndex = 11;
            this.radioButtonByte.Text = "byte";
            this.radioButtonByte.UseVisualStyleBackColor = true;
            // 
            // radioButtonShort
            // 
            this.radioButtonShort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonShort.AutoSize = true;
            this.radioButtonShort.Location = new System.Drawing.Point(188, 27);
            this.radioButtonShort.Name = "radioButtonShort";
            this.radioButtonShort.Size = new System.Drawing.Size(48, 17);
            this.radioButtonShort.TabIndex = 11;
            this.radioButtonShort.Text = "short";
            this.radioButtonShort.UseVisualStyleBackColor = true;
            // 
            // radioButtonUShort
            // 
            this.radioButtonUShort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonUShort.AutoSize = true;
            this.radioButtonUShort.Location = new System.Drawing.Point(244, 27);
            this.radioButtonUShort.Name = "radioButtonUShort";
            this.radioButtonUShort.Size = new System.Drawing.Size(54, 17);
            this.radioButtonUShort.TabIndex = 11;
            this.radioButtonUShort.Text = "ushort";
            this.radioButtonUShort.UseVisualStyleBackColor = true;
            // 
            // radioButtonInt
            // 
            this.radioButtonInt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonInt.AutoSize = true;
            this.radioButtonInt.Location = new System.Drawing.Point(188, 51);
            this.radioButtonInt.Name = "radioButtonInt";
            this.radioButtonInt.Size = new System.Drawing.Size(36, 17);
            this.radioButtonInt.TabIndex = 11;
            this.radioButtonInt.Text = "int";
            this.radioButtonInt.UseVisualStyleBackColor = true;
            // 
            // radioButtonUInt
            // 
            this.radioButtonUInt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonUInt.AutoSize = true;
            this.radioButtonUInt.Location = new System.Drawing.Point(244, 51);
            this.radioButtonUInt.Name = "radioButtonUInt";
            this.radioButtonUInt.Size = new System.Drawing.Size(42, 17);
            this.radioButtonUInt.TabIndex = 11;
            this.radioButtonUInt.Text = "uint";
            this.radioButtonUInt.UseVisualStyleBackColor = true;
            // 
            // radioButtonFloat
            // 
            this.radioButtonFloat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonFloat.AutoSize = true;
            this.radioButtonFloat.Checked = true;
            this.radioButtonFloat.Location = new System.Drawing.Point(188, 77);
            this.radioButtonFloat.Name = "radioButtonFloat";
            this.radioButtonFloat.Size = new System.Drawing.Size(45, 17);
            this.radioButtonFloat.TabIndex = 11;
            this.radioButtonFloat.TabStop = true;
            this.radioButtonFloat.Text = "float";
            this.radioButtonFloat.UseVisualStyleBackColor = true;
            // 
            // pictureBoxUpArrow
            // 
            this.pictureBoxUpArrow.BackgroundImage = global::SM64_Diagnostic.Properties.Resources.Up_Arrow;
            this.pictureBoxUpArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            //this.pictureBoxUpArrow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxUpArrow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxUpArrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxUpArrow.Location = new System.Drawing.Point(317, 51);
            this.pictureBoxUpArrow.Name = "pictureBoxUpArrow";
            this.pictureBoxUpArrow.Size = new System.Drawing.Size(30, 18);
            this.pictureBoxUpArrow.TabIndex = 12;
            this.pictureBoxUpArrow.TabStop = false;
            // 
            // pictureBoxDownArrow
            // 
            this.pictureBoxDownArrow.BackgroundImage = global::SM64_Diagnostic.Properties.Resources.Down_Arrow;
            this.pictureBoxDownArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            //this.pictureBoxDownArrow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxDownArrow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxDownArrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDownArrow.Location = new System.Drawing.Point(317, 75);
            this.pictureBoxDownArrow.Name = "pictureBoxDownArrow";
            this.pictureBoxDownArrow.Size = new System.Drawing.Size(30, 21);
            this.pictureBoxDownArrow.TabIndex = 12;
            this.pictureBoxDownArrow.TabStop = false;
            // 
            // pictureBoxRedX
            // 
            this.pictureBoxRedX.BackgroundImage = global::SM64_Diagnostic.Properties.Resources.Red_X;
            this.pictureBoxRedX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            //this.pictureBoxRedX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxRedX.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxRedX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxRedX.Location = new System.Drawing.Point(353, 51);
            this.pictureBoxRedX.Name = "pictureBoxRedX";
            this.SetRowSpan(this.pictureBoxRedX, 2);
            this.pictureBoxRedX.Size = new System.Drawing.Size(45, 45);
            this.pictureBoxRedX.TabIndex = 12;
            this.pictureBoxRedX.TabStop = false;
            // 
            // VarHackContainerForm
            // 
            /*
            this.ResumeLayout(false);
            this.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUpArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRedX)).EndInit();
            */
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var rec = DisplayRectangle;
            rec.Width -= 1;
            rec.Height -= 1;
            e.Graphics.DrawRectangle(_borderPen, rec);
        }

    }
}

