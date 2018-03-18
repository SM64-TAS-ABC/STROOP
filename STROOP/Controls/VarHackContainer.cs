using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Structs;
using STROOP.Utilities;
using System.Xml.Linq;
using STROOP.Structs.Configurations;
using System.Drawing.Drawing2D;

namespace STROOP.Controls
{
    public partial class VarHackContainer : UserControl
    {
        private readonly VarHackFlowLayoutPanel _varHackPanel;

        private string _specialType;
        private bool _isSpecial;
        private Func<string> _getterFunction;

        private VarHackContainer(
            VarHackFlowLayoutPanel varHackPanel,
            int creationIndex,
            bool useDefaults,
            string specialTypeIn,
            bool? noNumIn,
            string varNameIn,
            uint? addressIn,
            Type memoryTypeIn,
            bool? useHexIn,
            uint? pointerOffsetIn,
            int? xPosIn,
            int? yPosIn)
        {
            InitializeComponent();
            tableLayoutPanelVarHack.BorderWidth = 2;
            tableLayoutPanelVarHack.ShowBorder = true;

            _varHackPanel = varHackPanel;
            VarHackContainerDefaults defaults = new VarHackContainerDefaults(creationIndex);

            string specialType = useDefaults ? defaults.SpecialType : specialTypeIn;
            bool noNum = useDefaults ? defaults.NoNum : (noNumIn ?? VarHackContainerDefaults.StaticNoNum);
            string varName = useDefaults ? defaults.VarName : (varNameIn ?? VarHackContainerDefaults.StaticVarName);
            uint address = useDefaults ? defaults.Address : (addressIn ?? VarHackContainerDefaults.StaticAddres);
            Type memoryType = useDefaults ? defaults.MemoryType : (memoryTypeIn ?? VarHackContainerDefaults.StaticMemoryType);
            bool useHex = useDefaults ? defaults.UseHex : (useHexIn ?? VarHackContainerDefaults.StaticUseHex);
            uint? tempPointerOffset = useDefaults ? defaults.PointerOffset : pointerOffsetIn;
            bool usePointer = tempPointerOffset.HasValue;
            uint pointerOffset = tempPointerOffset ?? VarHackContainerDefaults.StaticPointerOffset;
            int xPos = (useDefaults || !xPosIn.HasValue) ? defaults.XPos : xPosIn.Value;
            int yPos = (useDefaults || !yPosIn.HasValue) ? defaults.YPos : yPosIn.Value;

            // Special
            _specialType = specialType;
            _isSpecial = specialType != null;
            if (_isSpecial) _getterFunction = VarHackSpecialUtilities.CreateGetterFunction(specialType);

            // Misc
            textBoxNameValue.Text = varName;
            textBoxAddressValue.Text = "0x" + String.Format("{0:X}", address);
            GetRadioButtonForType(memoryType).Checked = true;
            checkBoxUseHex.Checked = useHex;
            checkBoxNoNumber.Checked = noNum;

            // Pointer
            checkBoxUsePointer.Checked = usePointer;
            textBoxPointerOffsetValue.Enabled = usePointer;
            textBoxPointerOffsetValue.Text = "0x" + String.Format("{0:X}", pointerOffset);

            // Position
            textBoxXPosValue.Text = xPos.ToString();
            textBoxYPosValue.Text = yPos.ToString();

            // Clicking functionality
            pictureBoxUpArrow.Click += (sender, e) => _varHackPanel.MoveUpControl(this);
            pictureBoxDownArrow.Click += (sender, e) => _varHackPanel.MoveDownControl(this);
            pictureBoxRedX.Click += (sender, e) => _varHackPanel.RemoveControl(this);
            checkBoxUsePointer.Click += (sender, e) => textBoxPointerOffsetValue.Enabled = checkBoxUsePointer.Checked;

            // Pressing enter functionality
            textBoxNameValue.AddEnterAction(() => _varHackPanel.ApplyVariableToMemory(this));
            textBoxAddressValue.AddEnterAction(() => _varHackPanel.ApplyVariableToMemory(this));
            textBoxPointerOffsetValue.AddEnterAction(() => _varHackPanel.ApplyVariableToMemory(this));
            textBoxXPosValue.AddEnterAction(() => _varHackPanel.ApplyVariableToMemory(this));
            textBoxYPosValue.AddEnterAction(() => _varHackPanel.ApplyVariableToMemory(this));

            // Context menu strip
            ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem itemDuplicate = new ToolStripMenuItem("Duplicate");
            itemDuplicate.Click += (sender, e) => _varHackPanel.DuplicateControl(this);
            ContextMenuStrip.Items.Add(itemDuplicate);
        }

        public static VarHackContainer CreateDefault(
            VarHackFlowLayoutPanel varHackPanel,
            int creationIndex)
        {
            return new VarHackContainer(
                varHackPanel,
                creationIndex,
                true /* useDefaults */,
                null /* specialTypeIn */,
                null /* noNumIn */,
                null /* varNameIn */,
                null /* addressIn */,
                null /* memoryTypeIn */,
                null /* useHexIn */,
                null /* pointerOffsetIn */,
                null /* xPosIn */,
                null /* yPosIn */);
        }

        public static VarHackContainer CreateWithParameters(
            VarHackFlowLayoutPanel varHackPanel,
            int creationIndex,
            string varName,
            uint address,
            Type memoryType,
            bool useHex,
            uint? pointerOffset)
        {
            return new VarHackContainer(
                varHackPanel,
                creationIndex,
                false /* useDefaults */,
                null /* specialTypeIn */,
                false /* noNumIn */,
                varName,
                address,
                memoryType,
                useHex,
                pointerOffset,
                null /* xPosIn */,
                null /* yPosIn */);
        }

        public static VarHackContainer CreateSpecial(
            VarHackFlowLayoutPanel varHackPanel,
            int creationIndex,
            string specialType)
        {
            return new VarHackContainer(
                varHackPanel,
                creationIndex,
                false /* useDefaults */,
                specialType,
                true /* noNumIn */,
                null /* varNameIn */,
                null /* addressIn */,
                null /* memoryTypeIn */,
                null /* useHexIn */,
                null /* pointerOffsetIn */,
                null /* xPosIn */,
                null /* yPosIn */);
        }

        public static VarHackContainer CreateFromXml(
            VarHackFlowLayoutPanel varHackPanel,
            XElement element)
        {
            int xPos = ParsingUtilities.ParseInt(element.Attribute(XName.Get("xPos")).Value);
            int yPos = ParsingUtilities.ParseInt(element.Attribute(XName.Get("yPos")).Value);

            string specialType = element.Attribute(XName.Get("specialType"))?.Value;
            if (specialType != null)
            {
                return new VarHackContainer(
                    varHackPanel,
                    0 /* creationIndex */,
                    false /* useDefaults */,
                    specialType,
                    true /* noNumIn */,
                    null /* varNameIn */,
                    null /* addressIn */,
                    null /* memoryTypeIn */,
                    null /* useHexIn */,
                    null /* pointerOffsetIn */,
                    xPos,
                    yPos);
            }
            else
            {
                string varName = element.Attribute(XName.Get("name")).Value;
                uint address = ParsingUtilities.ParseHex(element.Attribute(XName.Get("address")).Value);
                Type type = TypeUtilities.StringToType[element.Attribute(XName.Get("type")).Value];
                bool useHex = ParsingUtilities.ParseBool(element.Attribute(XName.Get("useHex")).Value);
                uint? pointerOffset = ParsingUtilities.ParseHexNullable(element.Attribute(XName.Get("pointerOffset"))?.Value);
                bool noNum = ParsingUtilities.ParseBool(element.Attribute(XName.Get("noNum")).Value);

                return new VarHackContainer(
                    varHackPanel,
                    0 /* creationIndex */,
                    false /* useDefaults */,
                    null /* sepcialTypeIn */,
                    noNum,
                    varName,
                    address,
                    type,
                    useHex,
                    pointerOffset,
                    xPos,
                    yPos);
            }
        }

        public XElement ToXml()
        {
            XElement root = new XElement("Data");
            root.Add(new XAttribute("xPos", textBoxXPosValue.Text));
            root.Add(new XAttribute("yPos", textBoxYPosValue.Text));
            if (_isSpecial)
            {
                root.Add(new XAttribute("specialType", _specialType));
            }
            else
            {
                root.Add(new XAttribute("name", textBoxNameValue.Text));
                root.Add(new XAttribute("address", textBoxAddressValue.Text));
                root.Add(new XAttribute("type", TypeUtilities.TypeToString[GetCurrentType()]));
                root.Add(new XAttribute("useHex", checkBoxUseHex.Checked));
                if (checkBoxUsePointer.Checked)
                    root.Add(new XAttribute("pointerOffset", textBoxPointerOffsetValue.Text));
                root.Add(new XAttribute("noNum", checkBoxNoNumber.Checked));
            }
            return root;
        }

        public VarHackContainer Clone()
        {
            return CreateFromXml(_varHackPanel, ToXml());
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

            byte[] bytes = new byte[VarHackConfig.StructSize];

            byte[] addressBytes = BitConverter.GetBytes(address);
            WriteBytes(addressBytes, bytes, VarHackConfig.AddressOffset, true);

            byte[] xPosBytes = BitConverter.GetBytes(xPos);
            WriteBytes(xPosBytes, bytes, VarHackConfig.XPosOffset, true);

            byte[] yPosBytes = BitConverter.GetBytes(yPos);
            WriteBytes(yPosBytes, bytes, VarHackConfig.YPosOffset, true);

            name = name.Replace("\\c", VarHackConfig.CoinChar);
            name = name.Replace("\\m", VarHackConfig.MarioHeadChar);
            name = name.Replace("\\s", VarHackConfig.StarChar);
            
            string cappedName = CapString(name, !noNumber);
            string numberAddon = noNumber ? "" : (useHex ? "%x" : "%d");
            string cappedNameAndNumberAddon = cappedName + numberAddon;
            byte[] nameAndNumberAddonBytes = Encoding.ASCII.GetBytes(cappedNameAndNumberAddon);
            WriteBytes(nameAndNumberAddonBytes, bytes, VarHackConfig.StringOffset, false);

            byte[] usePointerBytes = BitConverter.GetBytes(usePointer);
            WriteBytes(usePointerBytes, bytes, VarHackConfig.UsePointerOffset, true);

            if (usePointer)
            {
                byte[] pointerOffsetBytes = BitConverter.GetBytes(pointerOffset);
                WriteBytes(pointerOffsetBytes, bytes, VarHackConfig.PointerOffsetOffset, true);
            }

            byte[] signedBytes = BitConverter.GetBytes(signed);
            WriteBytes(signedBytes, bytes, VarHackConfig.SignedOffset, true);

            byte[] typeBytes = new byte[] { typeByte };
            WriteBytes(typeBytes, bytes, VarHackConfig.TypeOffset, true);

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
            int maxLength = VarHackConfig.MaxStringLength + (factorInNumberAddon ? 0 : 2);
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
            return _isSpecial;
        }

        public void UpdateControl()
        {
            if (_isSpecial)
            {
                textBoxNameValue.Text = _getterFunction();
            }
        }
    }
}
