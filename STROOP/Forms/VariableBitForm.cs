using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Extensions;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Models;

namespace STROOP.Forms
{
    public partial class VariableBitForm : Form
    {
        private readonly string _varName;
        private readonly WatchVariable _watchVar;
        private readonly List<uint> _fixedAddressList;
        private readonly Timer _timer;
        private readonly BindingList<ByteModel> _bytes;
        private readonly List<ByteModel> _reversedBytes;

        private bool _hasDoneColoring = false;
        private bool _showFloatComponents = false;

        public VariableBitForm(string varName, WatchVariable watchVar, List<uint> fixedAddressList)
        {
            _varName = varName;
            _watchVar = watchVar;
            _fixedAddressList = fixedAddressList;
            _timer = new Timer { Interval = 30 };

            InitializeComponent();

            _textBoxVarName.Text = _varName;
            _bytes = new BindingList<ByteModel>();
            for (int i = 0; i < watchVar.ByteCount.Value; i++)
            {
                _bytes.Add(new ByteModel(watchVar.ByteCount.Value - 1 - i, 0, _dataGridViewBits, this));
            }
            _dataGridViewBits.DataSource = _bytes;
            _dataGridViewBits.CellContentClick += (sender, e) =>
                _dataGridViewBits.CommitEdit(new DataGridViewDataErrorContexts());
            ControlUtilities.SetTableDoubleBuffered(_dataGridViewBits, true);

            _reversedBytes = _bytes.ToList();
            _reversedBytes.Reverse();

            int effectiveTableHeight = ControlUtilities.GetTableEffectiveHeight(_dataGridViewBits);
            int totalTableHeight = _dataGridViewBits.Height;
            int emptyHeight = totalTableHeight - effectiveTableHeight + 3;
            Height -= emptyHeight;

            ControlUtilities.AddCheckableContextMenuStripItems(
                this,
                new List<string>() { "Show Value", "Show Float Components" },
                new List<bool>() { false, true },
                boolValue => _showFloatComponents = boolValue,
                false);

            _timer.Tick += (s, e) => UpdateForm();
            _timer.Start();
        }

        private void UpdateForm()
        {
            if (!_hasDoneColoring)
            {
                DoColoring();
                _hasDoneColoring = true;
            }

            List<object> values = _watchVar.GetValues();
            if (values.Count == 0) return;
            object value = values[0];
            if (!TypeUtilities.IsNumber(value))
                throw new ArgumentOutOfRangeException();

            byte[] bytes = TypeUtilities.GetBytes(value);
            if (bytes.Length != _bytes.Count)
                throw new ArgumentOutOfRangeException();

            for (int i = 0; i < _bytes.Count; i++)
            {
                _bytes[i].SetByteValue(bytes[bytes.Length - 1 - i], false);
            }

            if (_showFloatComponents && value is float floatValue)
            {
                _textBoxDecValue.Text = MoreMath.GetFloatSign(floatValue).ToString();
                _textBoxHexValue.Text = MoreMath.GetFloatExponent(floatValue).ToString();
                _textBoxBinaryValue.Text = MoreMath.GetFloatMantissa(floatValue).ToString();
            }
            else
            {
                _textBoxDecValue.Text = value.ToString();
                _textBoxHexValue.Text = HexUtilities.FormatMemory(value, _watchVar.NibbleCount.Value);
                _textBoxBinaryValue.Text = String.Join(" ", _bytes.ToList().ConvertAll(b => b.GetBinary()));
            }
        }

        public void SetValueInMemory()
        {
            byte[] bytes = _reversedBytes.ConvertAll(b => b.GetByteValue()).ToArray();
            object value = TypeUtilities.ConvertBytes(_watchVar.MemoryType, bytes);
            _watchVar.SetValue(value);
        }

        private void DoColoring()
        {
            // Color specially the differents parts of a float
            if (_watchVar.MemoryType == typeof(float))
            {
                Color signColor = Color.LightBlue;
                Color exponentColor = Color.Pink;
                Color mantissaColor = Color.LightGreen.Lighten(0.5);

                for (int i = 0; i < 32; i++)
                {
                    Color color;
                    if (i < 1) color = signColor;
                    else if (i < 9) color = exponentColor;
                    else color = mantissaColor;

                    int rowIndex = i / 8;
                    int colIndex = i % 8 + 4;
                    DataGridViewCell cell = _dataGridViewBits.Rows[rowIndex].Cells[colIndex];
                    cell.Style.BackColor = color;
                }
            }
        }
    }
}
