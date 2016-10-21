using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;

namespace SM64_Diagnostic.ManagerClasses
{
    class TriangleManager : DataManager
    {
        MaskedTextBox _addressBox;
        uint _triangleAddress = 0;
        bool _addressChangedByUser = true;

        uint TriangleAddress
        {
            get
            {
                return _triangleAddress;
            }
            set
            {
                if (_triangleAddress == value)
                    return;

                _triangleAddress = value;

                foreach (var watchVar in _dataControls)
                {
                    watchVar.OtherOffset = _triangleAddress;
                }

                _addressChangedByUser = false;
                _addressBox.Text = String.Format("0x{0:X8}", _triangleAddress);
                _addressChangedByUser = true;

                if (_triangleAddress == 0x00)
                {
                    foreach (var watchVar in _dataControls)
                    {
                        watchVar.Value = "(none)";
                    }
                }
            }
        }

        public enum TriangleMode {Floor, Wall, Ceiling, Other};

        public TriangleMode Mode = TriangleMode.Floor;

        /// <summary>
        /// Manages illumanati
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="config"></param>
        /// <param name="data"></param>
        /// <param name="variableTable"></param>
        public TriangleManager(ProcessStream stream, Config config, FlowLayoutPanel variableTable, MaskedTextBox addressBox) 
            : base(stream, config, new List<WatchVariable>(), variableTable)
        {
            _addressBox = addressBox;
            _addressBox.LostFocus += AddressBox_LostFocus;
            _addressBox.KeyDown += AddressBox_KeyDown;
            _addressBox.TextChanged += AddressBox_TextChanged;
            (_addressBox.Parent.Controls["radioButtonTriFloor"] as RadioButton).CheckedChanged 
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Floor);
            (_addressBox.Parent.Controls["radioButtonTriWall"] as RadioButton).CheckedChanged 
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Wall);
            (_addressBox.Parent.Controls["radioButtonTriCeiling"] as RadioButton).CheckedChanged 
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Ceiling);
            (_addressBox.Parent.Controls["radioButtonTriOther"] as RadioButton).CheckedChanged 
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Other);


            var data = new List<WatchVariable>();

            #region TriangleData
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.SurfaceType,
                Name = "Surface Type",
                OtherOffset = true,
                Type = typeof(UInt16)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.Flags,
                Name = "Flags",
                OtherOffset = true,
                Type = typeof(byte),
                UseHex = true
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.WindDirection,
                Name = "Wind Direction",
                OtherOffset = true,
                Type = typeof(byte)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.WallProjection,
                Name = "Wall Projection",
                OtherOffset = true,
                Type = typeof(ushort),
                UseHex = true
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.YMin,
                Name = "Y Min",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.YMax,
                Name = "Y Max",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.X1,
                Name = "X1",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.Y1,
                Name = "Y1",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.Z1,
                Name = "Z1",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.X2,
                Name = "X2",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.Y2,
                Name = "Y2",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.Z2,
                Name = "Z2",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.X3,
                Name = "X3",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.Y3,
                Name = "Y3",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.Z3,
                Name = "Z3",
                OtherOffset = true,
                Type = typeof(short)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.NormX,
                Name = "Normal X",
                OtherOffset = true,
                Type = typeof(float)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.NormY,
                Name = "Normal Y",
                OtherOffset = true,
                Type = typeof(float)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.NormZ,
                Name = "Normal Z",
                OtherOffset = true,
                Type = typeof(float)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.Offset,
                Name = "Offset",
                OtherOffset = true,
                Type = typeof(float)
            });
            data.Add(new WatchVariable()
            {
                Address = config.TriangleOffsets.AssociatedObject,
                Name = "Associated Object",
                OtherOffset = true,
                Type = typeof(uint),
                UseHex = true
            });
            #endregion

            foreach (WatchVariable watchVar in data)
            {
                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, 0);
                watchControl.Value = "(none)";
                variableTable.Controls.Add(watchControl.Control);
                _dataControls.Add(watchControl);
            }
        }

        private void Mode_CheckedChanged(object sender, EventArgs e, TriangleMode mode)
        {
            if (!(sender as RadioButton).Checked)
                return;

            Mode = mode;
        }

        private void AddressBox_TextChanged(object sender, EventArgs e)
        {
            if (!_addressChangedByUser)
                return;

            (_addressBox.Parent.Controls["radioButtonTriOther"] as RadioButton).Checked = true;
        }

        private void AddressBox_KeyDown(object sender, KeyEventArgs e)
        {
            // On "Enter" key press
            if (e.KeyData != Keys.Enter)
                return;

            _addressBox.Parent.Focus();
        }

        private void AddressBox_LostFocus(object sender, EventArgs e)
        {
            uint newAddress;
            if (!ParsingUtilities.TryParseHex(_addressBox.Text, out newAddress))
            {
                MessageBox.Show(String.Format("Address {0} is not valid!", _addressBox.Text),
                    "Address Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TriangleAddress = newAddress;
            (_addressBox.Parent.Controls["radioButtonTriOther"] as RadioButton).Checked = true;
        }

        public override void Update(bool updateView)
        {
            if (updateView)
            {
                switch (Mode)
                {
                    case TriangleMode.Ceiling:
                        TriangleAddress = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.MarioStructAddress + _config.Mario.CeilingTriangleOffset, 4), 0);
                        break;

                    case TriangleMode.Floor:
                        TriangleAddress = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.MarioStructAddress + _config.Mario.FloorTriangleOffset, 4), 0);
                        break;

                    case TriangleMode.Wall:
                        TriangleAddress = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.MarioStructAddress + _config.Mario.WallTriangleOffset, 4), 0);
                        break;
                }
            }

            if (_triangleAddress != 0x00)
                base.Update(updateView);
        }
    }
}
