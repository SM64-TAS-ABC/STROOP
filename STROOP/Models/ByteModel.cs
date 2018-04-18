using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Drawing;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Forms;

namespace STROOP.Models
{
    public class ByteModel
    {
        private readonly int _byteIndex;
        private readonly DataGridView _table;
        private readonly VariableBitForm _form;

        private byte _byteValue;

        public ByteModel(int byteIndex, byte byteValue, DataGridView table, VariableBitForm form)
        {
            _byteIndex = byteIndex;
            _byteValue = byteValue;
            _table = table;
            _form = form;
        }

        public int Index { get => _byteIndex; }
        public string Dec { get => _byteValue.ToString(); set { SetDec(value); NotifyChange(true); } }
        public string Hex { get => HexUtilities.Format(_byteValue, 2, false); set { SetHex(value); NotifyChange(true); } }
        public string Binary { get => GetBinary(); set { SetBinary(value); NotifyChange(true); } }
        public bool Bit7 { get => GetBit(7); set { SetBit(7, value); NotifyChange(true); } }
        public bool Bit6 { get => GetBit(6); set { SetBit(6, value); NotifyChange(true); } }
        public bool Bit5 { get => GetBit(5); set { SetBit(5, value); NotifyChange(true); } }
        public bool Bit4 { get => GetBit(4); set { SetBit(4, value); NotifyChange(true); } }
        public bool Bit3 { get => GetBit(3); set { SetBit(3, value); NotifyChange(true); } }
        public bool Bit2 { get => GetBit(2); set { SetBit(2, value); NotifyChange(true); } }
        public bool Bit1 { get => GetBit(1); set { SetBit(1, value); NotifyChange(true); } }
        public bool Bit0 { get => GetBit(0); set { SetBit(0, value); NotifyChange(true); } }

        public void SetByteValue(byte byteValue, bool userChange)
        {
            _byteValue = byteValue;
            NotifyChange(userChange);
        }

        public byte GetByteValue()
        {
            return _byteValue;
        }

        private void NotifyChange(bool userChange)
        {
            if (userChange)
            {
                _form.SetValueInMemory();
                _table.ClearSelection();
            }
            _table.Refresh();
        }

        private bool GetBit(int bit)
        {
            return (_byteValue & (1 << bit)) != 0;
        }

        private void SetBit(int bit, bool value)
        {
            _byteValue = MoreMath.ApplyValueToMaskedByte(_byteValue, (byte)(1 << bit), value);
        }

        public string GetBinary()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 7; i >= 0; i--)
            {
                bool bitBool = GetBit(i);
                string bitString = bitBool ? "1" : "0";
                builder.Append(bitString);
            }
            return builder.ToString();
        }

        private void SetBinary(string binaryString)
        {
            byte newValue = 0;
            for (int i = 0; i <= 7 && i < binaryString.Length; i++)
            {
                string binaryChar = binaryString.Substring(binaryString.Length - 1 - i, 1);
                if (binaryChar == "1" || binaryChar == "0")
                {
                    bool binaryBool = binaryChar == "1";
                    newValue = MoreMath.ApplyValueToMaskedByte(newValue, (byte)(1 << i), binaryBool);
                }
            }
            _byteValue = newValue;
        }

        private void SetHex(string hexString)
        {
            uint? uintValueNullable = ParsingUtilities.ParseHexNullable(hexString);
            if (uintValueNullable == null) return;
            uint uintValue = uintValueNullable.Value;
            _byteValue = ParsingUtilities.ParseByteRoundingWrapping(uintValue);
        }

        private void SetDec(string decValue)
        {
            byte? byteValueNullable = ParsingUtilities.ParseByteRoundingWrapping(decValue);
            if (byteValueNullable.HasValue) _byteValue = byteValueNullable.Value;
        }
    }
}
