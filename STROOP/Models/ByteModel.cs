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

namespace STROOP.Models
{
    public class ByteModel
    {
        private readonly int _byteIndex;
        private byte _byteValue;

        public ByteModel(int byteIndex, byte byteValue)
        {
            _byteIndex = byteIndex;
            _byteValue = byteValue;
        }

        public int Index { get => _byteIndex; }
        public byte Dec { get => _byteValue; set => _byteValue = ParsingUtilities.ParseByteRoundingWrapping(value); }
        public string Hex { get => HexUtilities.Format(_byteValue, 2, false); set => SetHex(value); }
        public string Binary { get => GetBinary(); set => SetBinary(value); }
        public bool Bit7 { get => GetBit(7); set => SetBit(7, value); }
        public bool Bit6 { get => GetBit(6); set => SetBit(6, value); }
        public bool Bit5 { get => GetBit(5); set => SetBit(5, value); }
        public bool Bit4 { get => GetBit(4); set => SetBit(4, value); }
        public bool Bit3 { get => GetBit(3); set => SetBit(3, value); }
        public bool Bit2 { get => GetBit(2); set => SetBit(2, value); }
        public bool Bit1 { get => GetBit(1); set => SetBit(1, value); }
        public bool Bit0 { get => GetBit(0); set => SetBit(0, value); }

        private bool GetBit(int bit)
        {
            return (_byteValue & (1 << bit)) != 0;
        }

        private void SetBit(int bit, bool value)
        {
            _byteValue = MoreMath.ApplyValueToMaskedByte(_byteValue, (byte)(1 << bit), value);
        }

        private string GetBinary()
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
    }
}
