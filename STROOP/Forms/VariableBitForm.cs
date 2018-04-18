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

        public VariableBitForm(string varName, WatchVariable watchVar, List<uint> fixedAddressList)
        {
            _varName = varName;
            _watchVar = watchVar;
            _fixedAddressList = fixedAddressList;
            _timer = new Timer { Interval = 30 };

            InitializeComponent();

            _textBoxVarName.Text = _varName;
            _bytes = new BindingList<ByteModel>();
            for (int i = 0; i < watchVar.ByteCount; i++)
            {
                _bytes.Add(new ByteModel(i, 0, _dataGridViewBits, this));
            }
            _dataGridViewBits.DataSource = _bytes;

            _dataGridViewBits.CellContentClick += (sender, e) =>
                _dataGridViewBits.CommitEdit(new DataGridViewDataErrorContexts());

            _timer.Tick += (s, e) => UpdateForm();
            _timer.Start();
        }

        private void UpdateForm()
        {
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
                _bytes[i].SetByteValue(bytes[i], false);
            }

            _textBoxDecValue.Text = value.ToString();
            _textBoxHexValue.Text = HexUtilities.Format(value, _watchVar.NibbleCount.Value);
            _textBoxBinaryValue.Text = GetBinary();
        }

        private string GetBinary()
        {
            List<string> binaryStrings = _bytes.ToList().ConvertAll(b => b.GetBinary());
            binaryStrings.Reverse();
            return String.Join(" ", binaryStrings);
        }

        public void SetValueInMemory()
        {
            byte[] bytes = _bytes.ToList().ConvertAll(b => b.GetByteValue()).ToArray();
            object value = TypeUtilities.ConvertBytes(_watchVar.MemoryType, bytes);
            _watchVar.SetValue(value);
        }
    }
}
