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

namespace STROOP.M64Editor
{
    public class M64InputFrame
    {
        public static int ClassIdIndex = 0;

        public int FrameIndex;
        public uint RawValue;

        public readonly int IdIndex;

        private readonly DataGridView _table;
        private readonly M64File _m64File;
        private readonly bool IsOriginalFrame;

        public M64InputFrame(int frameIndex, uint rawValue, bool isOriginalFrame, M64File m64File, DataGridView table)
        {
            FrameIndex = frameIndex;
            RawValue = rawValue;
            IsOriginalFrame = isOriginalFrame;
            IdIndex = ClassIdIndex;
            ClassIdIndex++;

            _m64File = m64File;
            _table = table;

            _X = X;
            _Y = Y;
            _A = A;
            _B = B;
            _Z = Z;
            _S = S;
            _R = R;
            _C_Up = C_Up;
            _C_Down = C_Down;
            _C_Left = C_Left;
            _C_Right = C_Right;
            _L = L;
            _D_Up = D_Up;
            _D_Down = D_Down;
            _D_Left = D_Left;
            _D_Right = D_Right;
        }

        public int Frame { get => M64Utilities.ConvertFrameToDisplayedValue(FrameIndex); }
        public int Id { get => M64Utilities.ConvertFrameToDisplayedValue(IdIndex); }
        public sbyte X { get => (sbyte)GetByte(2); set { SetByte(2, (byte)value); NotifyChange(); } }
        public sbyte Y { get => (sbyte)GetByte(3); set { SetByte(3, (byte)value); NotifyChange(); } }
        public bool A { get => GetBit(7); set { SetBit(7, value); NotifyChange(); } }
        public bool B { get => GetBit(6); set { SetBit(6, value); NotifyChange(); } }
        public bool Z { get => GetBit(5); set { SetBit(5, value); NotifyChange(); } }
        public bool S { get => GetBit(4); set { SetBit(4, value); NotifyChange(); } }
        public bool R { get => GetBit(12); set { SetBit(12, value); NotifyChange(); } }
        public bool C_Up { get => GetBit(11); set { SetBit(11, value); NotifyChange(); } }
        public bool C_Down { get => GetBit(10); set { SetBit(10, value); NotifyChange(); } }
        public bool C_Left { get => GetBit(9); set { SetBit(9, value); NotifyChange(); } }
        public bool C_Right { get => GetBit(8); set { SetBit(8, value); NotifyChange(); } }
        public bool L { get => GetBit(13); set { SetBit(13, value); NotifyChange(); } }
        public bool D_Up { get => GetBit(3); set { SetBit(3, value); NotifyChange(); } }
        public bool D_Down { get => GetBit(2); set { SetBit(2, value); NotifyChange(); } }
        public bool D_Left { get => GetBit(1); set { SetBit(1, value); NotifyChange(); } }
        public bool D_Right { get => GetBit(0); set { SetBit(0, value); NotifyChange(); } }

        private readonly sbyte _X;
        private readonly sbyte _Y;
        private readonly bool _A;
        private readonly bool _B;
        private readonly bool _Z;
        private readonly bool _S;
        private readonly bool _R;
        private readonly bool _C_Up;
        private readonly bool _C_Down;
        private readonly bool _C_Left;
        private readonly bool _C_Right;
        private readonly bool _L;
        private readonly bool _D_Up;
        private readonly bool _D_Down;
        private readonly bool _D_Left;
        private readonly bool _D_Right;

        private List<object> GetOriginalValues()
        {
            return new List<object>()
            {
                _X, _Y,
                _A, _B, _Z, _S, _R,
                _C_Up, _C_Down, _C_Left, _C_Right,
                _L, _D_Up, _D_Down, _D_Left, _D_Right,
            };
        }

        private List<object> GetCurrentValues()
        {
            return new List<object>()
            {
                X, Y,
                A, B, Z, S, R,
                C_Up, C_Down, C_Left, C_Right,
                L, D_Up, D_Down, D_Left, D_Right,
            };
        }

        private void NotifyChange()
        {
            _m64File.IsModified = true;
            _m64File.ModifiedFrames.Add(this);
            UpdateCellColors();
        }

        public void UpdateCellColors()
        {
            List<object> originalValues = GetOriginalValues();
            List<object> currentvalues = GetCurrentValues();
            for (int i = 0; i < 16; i++)
            {
                bool valueChanged = !Equals(originalValues[i], currentvalues[i]);
                int columnIndex = i + 2;
                DataGridViewRow row = _table.Rows[FrameIndex];
                DataGridViewColumn col = _table.Columns[columnIndex];
                DataGridViewCell cell = row.Cells[columnIndex];
                Color defaultColor = row.DefaultCellStyle.BackColor == M64Config.NewRowColor ?
                    M64Config.NewRowColor : col.DefaultCellStyle.BackColor;
                cell.Style.BackColor = valueChanged ? M64Config.EditedCellColor : defaultColor;
            }
        }

        public void UpdateRowColor()
        {
            if (!IsOriginalFrame)
            {
                DataGridViewRow row = _table.Rows[FrameIndex];
                row.DefaultCellStyle.BackColor = M64Config.NewRowColor;
            }
        }

        private void SetByte(int num, byte value)
        {
            RawValue = M64Utilities.SetByte(RawValue, num, value);
        }

        private byte GetByte(int num)
        {
            return M64Utilities.GetByte(RawValue, num);
        }

        private void SetBit(int bit, bool value)
        {
            RawValue = M64Utilities.SetBit(RawValue, bit, value);
        }

        private bool GetBit(int bit)
        {
            return M64Utilities.GetBit(RawValue, bit);
        }

        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(RawValue).ToArray();
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj is M64InputFrame input)
            {
                return IdIndex == input.IdIndex;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return IdIndex;
        }

        public override string ToString()
        {
            return String.Format("Frame={0}, Id={1}, Inputs={2}", FrameIndex, IdIndex, GetInputString());
        }

        public string GetInputString()
        {
            StringBuilder builder = new StringBuilder();

            if (X != 0) builder.Append("X" + X);
            if (Y != 0) builder.Append("Y" + Y);

            if (A) builder.Append("A");
            if (B) builder.Append("B");
            if (Z) builder.Append("Z");
            if (S) builder.Append("S");
            if (R) builder.Append("R");

            if (C_Up) builder.Append("C^");
            if (C_Down) builder.Append("Cv");
            if (C_Left) builder.Append("C<");
            if (C_Right) builder.Append("C>");

            if (L) builder.Append("L");

            if (D_Up) builder.Append("D^");
            if (D_Down) builder.Append("Dv");
            if (D_Left) builder.Append("D<");
            if (D_Right) builder.Append("D>");

            return builder.ToString();
        }
    }
}
