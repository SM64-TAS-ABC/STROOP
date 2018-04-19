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
        public sbyte X { get => (sbyte)GetByte(2); set { SetByte(2, (byte)value); NotifyChange(2, X != _X); } }
        public sbyte Y { get => (sbyte)GetByte(3); set { SetByte(3, (byte)value); NotifyChange(3, Y != _Y); } }
        public bool A { get => GetBit(7); set { SetBit(7, value); NotifyChange(4, A != _A); } }
        public bool B { get => GetBit(6); set { SetBit(6, value); NotifyChange(5, B != _B); } }
        public bool Z { get => GetBit(5); set { SetBit(5, value); NotifyChange(6, Z != _Z); } }
        public bool S { get => GetBit(4); set { SetBit(4, value); NotifyChange(7, S != _S); } }
        public bool R { get => GetBit(12); set { SetBit(12, value); NotifyChange(8, R != _R); } }
        public bool C_Up { get => GetBit(11); set { SetBit(11, value); NotifyChange(9, C_Up != _C_Up); } }
        public bool C_Down { get => GetBit(10); set { SetBit(10, value); NotifyChange(10, C_Down != _C_Down); } }
        public bool C_Left { get => GetBit(9); set { SetBit(9, value); NotifyChange(11, C_Left != _C_Left); } }
        public bool C_Right { get => GetBit(8); set { SetBit(8, value); NotifyChange(12, C_Right != _C_Right); } }
        public bool L { get => GetBit(13); set { SetBit(13, value); NotifyChange(13, L != _L); } }
        public bool D_Up { get => GetBit(3); set { SetBit(3, value); NotifyChange(14, D_Up != _D_Up); } }
        public bool D_Down { get => GetBit(2); set { SetBit(2, value); NotifyChange(15, D_Down != _D_Down); } }
        public bool D_Left { get => GetBit(1); set { SetBit(1, value); NotifyChange(16, D_Left != _D_Left); } }
        public bool D_Right { get => GetBit(0); set { SetBit(0, value); NotifyChange(17, D_Right != _D_Right); } }

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

        private void NotifyChange(int columnIndex, bool valueChanged)
        {
            _m64File.IsModified = true;
            DataGridViewRow row = _table.Rows[FrameIndex];
            DataGridViewColumn col = _table.Columns[columnIndex];
            DataGridViewCell cell = row.Cells[columnIndex];
            Color defaultColor = row.DefaultCellStyle.BackColor == M64Config.NewRowColor ?
                M64Config.NewRowColor : col.DefaultCellStyle.BackColor;
            cell.Style.BackColor = valueChanged ? M64Config.EditedCellColor : defaultColor;
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
    }
}
