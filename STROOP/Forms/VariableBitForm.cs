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
                _bytes.Add(new ByteModel(i, 0));
            }
            _dataGridViewBits.DataSource = _bytes;

            _timer.Tick += (s, e) => UpdateForm();
            _timer.Start();
        }

        private void UpdateForm()
        {
            //_textBoxCurrentValue.Text = _watchVarWrapper.GetValue(true, true, _fixedAddressList).ToString();
        }
    }
}
