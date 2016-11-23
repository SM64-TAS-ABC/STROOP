using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;

namespace SM64_Diagnostic.ManagerClasses
{
    class TriangleManager : DataManager
    {
        MaskedTextBox _addressBox;
        uint _triangleAddress = 0;
        bool _addressChangedByUser = true;
        bool _useMisalignmentOffset = false;

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

                foreach (var dataContainer in _dataControls)
                {
                    if (dataContainer is WatchVariableControl)
                    {
                        var watchVar = dataContainer as WatchVariableControl;
                        watchVar.OtherOffsets = new List<uint>() { _triangleAddress };
                    }
                }

                _addressChangedByUser = false;
                _addressBox.Text = String.Format("0x{0:X8}", _triangleAddress);
                _addressChangedByUser = true;
            }
        }

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("DistanceAboveFloor"),
                new DataContainer("DistanceBelowCeiling"),
                new DataContainer("ClosestVertex"),
                new AngleDataContainer("UpHillAngle"),
                new AngleDataContainer("DownHillAngle"),
                new AngleDataContainer("LeftHillAngle"),
                new AngleDataContainer("RightHillAngle"),
                new DataContainer("Classification"),
                new AngleDataContainer("Steepness"),
                new DataContainer("NormalDistAway"),
                new DataContainer("VerticalDistAway"),
                new DataContainer("HeightOnSlope")
            };
        }

        public enum TriangleMode {Floor, Wall, Ceiling, Other};

        public TriangleMode Mode = TriangleMode.Floor;

        /// <summary>
        /// Manages illumanati
        /// </summary>
        public TriangleManager(ProcessStream stream, Control tabControl, List<WatchVariable> triangleWatchVars) 
            : base(stream, triangleWatchVars, tabControl.Controls["NoTearFlowLayoutPanelTriangles"] as NoTearFlowLayoutPanel)
        {
            _addressBox = tabControl.Controls["maskedTextBoxOtherTriangle"] as MaskedTextBox;
            _addressBox.LostFocus += AddressBox_LostFocus;
            _addressBox.KeyDown += AddressBox_KeyDown;
            _addressBox.TextChanged += AddressBox_TextChanged;
            (tabControl.Controls["radioButtonTriFloor"] as RadioButton).CheckedChanged 
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Floor);
            (tabControl.Controls["radioButtonTriWall"] as RadioButton).CheckedChanged 
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Wall);
            (tabControl.Controls["radioButtonTriCeiling"] as RadioButton).CheckedChanged 
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Ceiling);
            (tabControl.Controls["radioButtonTriOther"] as RadioButton).CheckedChanged 
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Other);
            (tabControl.Controls["buttonGoToV1"] as Button).Click += GoToV1Button_Click;
            (tabControl.Controls["buttonGoToV2"] as Button).Click += GoToV2Button_Click;
            (tabControl.Controls["buttonGoToV3"] as Button).Click += GoToV3Button_Click;
            (tabControl.Controls["buttonRetrieveTriangle"] as Button).Click += RetrieveTriangleButton_Click;
            (tabControl.Controls["checkBoxVertexMisalignment"] as CheckBox).CheckedChanged += checkBoxVertexMisalignment_CheckedChanged;
        }

        private void checkBoxVertexMisalignment_CheckedChanged(object sender, EventArgs e)
        {
            _useMisalignmentOffset = (sender as CheckBox).Checked;
        }

        private void ProcessSpecialVars()
        {
            var floorY = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.GroundYOffset, 4), 0);
            float marioX, marioY, marioZ;
            marioX = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.XOffset, 4), 0);
            marioY = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.YOffset, 4), 0);
            marioZ = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.ZOffset, 4), 0);

            float normX, normY, normZ, normOffset;
            normX = BitConverter.ToSingle(_stream.ReadRam(TriangleAddress + Config.TriangleOffsets.NormX, 4), 0);
            normY = BitConverter.ToSingle(_stream.ReadRam(TriangleAddress + Config.TriangleOffsets.NormY, 4), 0);
            normZ = BitConverter.ToSingle(_stream.ReadRam(TriangleAddress + Config.TriangleOffsets.NormZ, 4), 0);
            normOffset = BitConverter.ToSingle(_stream.ReadRam(TriangleAddress + Config.TriangleOffsets.Offset, 4), 0);

            var uphillAngle = Math.PI + Math.Atan2(normX, normZ);
            if (normX == 0 && normZ == 0)
                uphillAngle = double.NaN;
            if (normY < -0.01)
                uphillAngle += Math.PI;
            
            foreach (IDataContainer specialVar in _specialWatchVars)
            {
                switch (specialVar.SpecialName)
                {
                    case "DistanceAboveFloor":
                        (specialVar as DataContainer).Text = (marioY - floorY).ToString();
                        break;
                    case "DistanceBelowCeiling":
                        (specialVar as DataContainer).Text = (_stream.GetSingle(Config.Mario.StructAddress + Config.Mario.CeilingYOffset)
                            - marioY).ToString();
                        break;
                    case "ClosestVertex":
                        (specialVar as DataContainer).Text = String.Format("V{0}", MarioActions.GetClosestVertex(_stream, TriangleAddress));
                        goto case "CheckTriangleExists";
                    case "UpHillAngle":
                        (specialVar as AngleDataContainer).AngleValue = uphillAngle;
                        goto case "CheckTriangleExistsAngle";
                    case "DownHillAngle":
                        (specialVar as AngleDataContainer).AngleValue = uphillAngle + Math.PI;
                        goto case "CheckTriangleExistsAngle";
                    case "RightHillAngle":
                        (specialVar as AngleDataContainer).AngleValue = uphillAngle - Math.PI / 2;
                        goto case "CheckTriangleExistsAngle";
                    case "LeftHillAngle":
                        (specialVar as AngleDataContainer).AngleValue = uphillAngle + Math.PI / 2;
                        goto case "CheckTriangleExistsAngle";
                    case "Classification":
                        if (normY > 0.01)
                            (specialVar as DataContainer).Text = "Floor";
                        else if (normY < -0.01)
                            (specialVar as DataContainer).Text = "Ceiling";
                        else
                            (specialVar as DataContainer).Text = "Wall";
                        goto case "CheckTriangleExists";
                    case "Steepness":
                        (specialVar as AngleDataContainer).AngleValue = Math.Acos(normY);
                        goto case "CheckTriangleExistsAngle";
                    case "NormalDistAway":
                        (specialVar as DataContainer).Text = (marioX * normX + marioY * normY + marioZ * normZ + normOffset).ToString();
                        goto case "CheckTriangleExists";
                    case "VerticalDistAway":
                        (specialVar as DataContainer).Text = (marioY + (marioX * normX + marioZ * normZ + normOffset) / normY).ToString();
                        goto case "CheckTriangleExists";
                    case "HeightOnSlope":
                        (specialVar as DataContainer).Text = ((-marioX * normX - marioZ * normZ - normOffset) / normY).ToString();
                        goto case "CheckTriangleExists";

                    // Special
                    case "CheckTriangleExists":
                        if (TriangleAddress == 0x0000)
                        {
                            (specialVar as DataContainer).Text = "(none)";
                            break;
                        }
                        break;
                    case "CheckTriangleExistsAngle":
                        (specialVar as AngleDataContainer).ValueExists = (TriangleAddress != 0x0000);
                        break;
                }
            }
        }

        private double FixAngle(double angle)
        {
            angle = Math.Round(angle);
            angle %= 65536;
            if (angle > 32768)
                angle -= 65536;

            return angle;
        }

        private void RetrieveTriangleButton_Click(object sender, EventArgs e)
        {
            MarioActions.RetrieveTriangle(_stream, _triangleAddress);
        }

        private void GoToV3Button_Click(object sender, EventArgs e)
        {
            MarioActions.GoToTriangle(_stream, _triangleAddress, 3, _useMisalignmentOffset);
        }

        private void GoToV2Button_Click(object sender, EventArgs e)
        {
            MarioActions.GoToTriangle(_stream, _triangleAddress, 2, _useMisalignmentOffset);
        }

        private void GoToV1Button_Click(object sender, EventArgs e)
        {
            MarioActions.GoToTriangle(_stream, _triangleAddress, 1, _useMisalignmentOffset);
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
                        TriangleAddress = BitConverter.ToUInt32(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.CeilingTriangleOffset, 4), 0);
                        break;

                    case TriangleMode.Floor:
                        TriangleAddress = BitConverter.ToUInt32(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset, 4), 0);
                        break;

                    case TriangleMode.Wall:
                        TriangleAddress = BitConverter.ToUInt32(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.WallTriangleOffset, 4), 0);
                        break;
                }

                base.Update(updateView);
                ProcessSpecialVars();
            }
        }
    }
}
