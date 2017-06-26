using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class MarioManager : DataManager
    {
        MapManager _mapManager;

        TextBox _marioPosXZTextbox;
        TextBox _marioPosYTextbox;
        Button _marioPosXpButton;
        Button _marioPosXnButton;
        Button _marioPosZpButton;
        Button _marioPosZnButton;
        Button _marioPosXpZpButton;
        Button _marioPosXpZnButton;
        Button _marioPosXnZpButton;
        Button _marioPosXnZnButton;
        Button _marioPosYpButton;
        Button _marioPosYnButton;
        bool _marioPosRelative = false;

        TextBox _marioStatsYawTextbox;
        TextBox _marioStatsHspdTextbox;
        TextBox _marioStatsVspdTextbox;
        TextBox _marioHOLPXZTextbox;
        TextBox _marioHOLPYTextbox;

        public MarioManager(ProcessStream stream, List<WatchVariable> marioData, Control marioControl, NoTearFlowLayoutPanel variableTable, MapManager mapManager)
            : base(stream, marioData, variableTable, Config.Mario.StructAddress)
        {
            _mapManager = mapManager;

            var toggleHandsfree = marioControl.Controls["buttonMarioToggleHandsfree"] as Button;
            var toggleVisibility = marioControl.Controls["buttonMarioVisibility"] as Button;

            var marioPosGroupBox = marioControl.Controls["groupBoxMarioPos"] as GroupBox;
            _marioPosXZTextbox = marioPosGroupBox.Controls["textBoxMarioPosXZ"] as TextBox;
            _marioPosYTextbox = marioPosGroupBox.Controls["textBoxMarioPosY"] as TextBox;
            _marioPosXpButton = marioPosGroupBox.Controls["buttonMarioPosXp"] as Button;
            _marioPosXnButton = marioPosGroupBox.Controls["buttonMarioPosXn"] as Button;
            _marioPosZpButton = marioPosGroupBox.Controls["buttonMarioPosZp"] as Button;
            _marioPosZnButton = marioPosGroupBox.Controls["buttonMarioPosZn"] as Button;
            _marioPosXpZpButton = marioPosGroupBox.Controls["buttonMarioPosXpZp"] as Button;
            _marioPosXpZnButton = marioPosGroupBox.Controls["buttonMarioPosXpZn"] as Button;
            _marioPosXnZpButton = marioPosGroupBox.Controls["buttonMarioPosXnZp"] as Button;
            _marioPosXnZnButton = marioPosGroupBox.Controls["buttonMarioPosXnZn"] as Button;
            _marioPosYpButton = marioPosGroupBox.Controls["buttonMarioPosYp"] as Button;
            _marioPosYnButton = marioPosGroupBox.Controls["buttonMarioPosYn"] as Button;
            var marioPosRelativeCheckbox = marioPosGroupBox.Controls["checkBoxMarioPosRelative"] as CheckBox;

            var marioStatsGroupBox = marioControl.Controls["groupBoxMarioStats"] as GroupBox;
            _marioStatsYawTextbox = marioStatsGroupBox.Controls["textBoxMarioStatsYaw"] as TextBox;
            _marioStatsHspdTextbox = marioStatsGroupBox.Controls["textBoxMarioStatsHspd"] as TextBox;
            _marioStatsVspdTextbox = marioStatsGroupBox.Controls["textBoxMarioStatsVspd"] as TextBox;
            var marioYawPButton = marioStatsGroupBox.Controls["buttonMarioStatsYawP"] as Button;
            var marioYawNButton = marioStatsGroupBox.Controls["buttonMarioStatsYawN"] as Button;
            var marioHspdPButton = marioStatsGroupBox.Controls["buttonMarioStatsHspdP"] as Button;
            var marioHspdNButton = marioStatsGroupBox.Controls["buttonMarioStatsHspdN"] as Button;
            var marioVspdPButton = marioStatsGroupBox.Controls["buttonMarioStatsVspdP"] as Button;
            var marioVspdNButton = marioStatsGroupBox.Controls["buttonMarioStatsVspdN"] as Button;

            var marioHOLPGroupBox = marioControl.Controls["groupBoxMarioHOLP"] as GroupBox;
            _marioHOLPXZTextbox = marioHOLPGroupBox.Controls["textBoxMarioHOLPXZ"] as TextBox;
            _marioHOLPYTextbox = marioHOLPGroupBox.Controls["textBoxMarioHOLPY"] as TextBox;
            var marioHOLPXpButton = marioHOLPGroupBox.Controls["buttonMarioHOLPXp"] as Button;
            var marioHOLPXnButton = marioHOLPGroupBox.Controls["buttonMarioHOLPXn"] as Button;
            var marioHOLPZpButton = marioHOLPGroupBox.Controls["buttonMarioHOLPZp"] as Button;
            var marioHOLPZnButton = marioHOLPGroupBox.Controls["buttonMarioHOLPZn"] as Button;
            var marioHOLPXpZpButton = marioHOLPGroupBox.Controls["buttonMarioHOLPXpZp"] as Button;
            var marioHOLPXpZnButton = marioHOLPGroupBox.Controls["buttonMarioHOLPXpZn"] as Button;
            var marioHOLPXnZpButton = marioHOLPGroupBox.Controls["buttonMarioHOLPXnZp"] as Button;
            var marioHOLPXnZnButton = marioHOLPGroupBox.Controls["buttonMarioHOLPXnZn"] as Button;
            var marioHOLPYpButton = marioHOLPGroupBox.Controls["buttonMarioHOLPYp"] as Button;
            var marioHOLPYnButton = marioHOLPGroupBox.Controls["buttonMarioHOLPYn"] as Button;

            toggleHandsfree.Click += ToggleHandsfree_Click;
            toggleVisibility.Click += ToggleVisibility_Click;

            _marioPosXpButton.Click += (sender, e) => marioPosXZButton_Click(sender, e, 1, 0);
            _marioPosXnButton.Click += (sender, e) => marioPosXZButton_Click(sender, e, -1, 0);
            _marioPosZpButton.Click += (sender, e) => marioPosXZButton_Click(sender, e, 0, 1);
            _marioPosZnButton.Click += (sender, e) => marioPosXZButton_Click(sender, e, 0, -1);
            _marioPosXpZpButton.Click += (sender, e) => marioPosXZButton_Click(sender, e, 1, 1);
            _marioPosXpZnButton.Click += (sender, e) => marioPosXZButton_Click(sender, e, 1, -1);
            _marioPosXnZpButton.Click += (sender, e) => marioPosXZButton_Click(sender, e, -1, 1);
            _marioPosXnZnButton.Click += (sender, e) => marioPosXZButton_Click(sender, e, -1, -1);
            _marioPosYpButton.Click += (sender, e) => marioPosYButton_Click(sender, e, 1);
            _marioPosYnButton.Click += (sender, e) => marioPosYButton_Click(sender, e, -1);
            marioPosRelativeCheckbox.CheckedChanged += marioPosRelativeCheckbox_CheckedChanged;

            marioYawPButton.Click += (sender, e) => marioStatsYawButton_Click(sender, e, 1);
            marioYawNButton.Click += (sender, e) => marioStatsYawButton_Click(sender, e, -1);
            marioHspdPButton.Click += (sender, e) => marioStatsHspdButton_Click(sender, e, 1);
            marioHspdNButton.Click += (sender, e) => marioStatsHspdButton_Click(sender, e, -1);
            marioVspdPButton.Click += (sender, e) => marioStatsVspdButton_Click(sender, e, 1);
            marioVspdNButton.Click += (sender, e) => marioStatsVspdButton_Click(sender, e, -1);

            marioHOLPXpButton.Click += (sender, e) => marioHOLPXZButton_Click(sender, e, 1, 0);
            marioHOLPXnButton.Click += (sender, e) => marioHOLPXZButton_Click(sender, e, -1, 0);
            marioHOLPZpButton.Click += (sender, e) => marioHOLPXZButton_Click(sender, e, 0, 1);
            marioHOLPZnButton.Click += (sender, e) => marioHOLPXZButton_Click(sender, e, 0, -1);
            marioHOLPXpZpButton.Click += (sender, e) => marioHOLPXZButton_Click(sender, e, 1, 1);
            marioHOLPXpZnButton.Click += (sender, e) => marioHOLPXZButton_Click(sender, e, 1, -1);
            marioHOLPXnZpButton.Click += (sender, e) => marioHOLPXZButton_Click(sender, e, -1, 1);
            marioHOLPXnZnButton.Click += (sender, e) => marioHOLPXZButton_Click(sender, e, -1, -1);
            marioHOLPYpButton.Click += (sender, e) => marioHOLPYButton_Click(sender, e, 1);
            marioHOLPYnButton.Click += (sender, e) => marioHOLPYButton_Click(sender, e, -1);
        }

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("DeFactoSpeed"),
                new DataContainer("SlidingSpeed"),
                new AngleDataContainer("SlidingAngle"),
                new DataContainer("FallHeight"),
                new DataContainer("ActionDescription"),
                new DataContainer("PrevActionDescription"),
                new DataContainer("MovementX"),
                new DataContainer("MovementY"),
                new DataContainer("MovementZ"),
                new DataContainer("MovementLateral"),
                new DataContainer("Movment"),
                new DataContainer("QFrameCountEstimate")
            };
        }

        public void ProcessSpecialVars()
        {
            UInt32 floorTriangle = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
            var floorY = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.GroundYOffset);

            float hSpeed = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HSpeedOffset);

            float slidingSpeedX = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.SlidingSpeedXOffset);
            float slidingSpeedZ = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.SlidingSpeedZOffset);

            float movementX = (_stream.GetSingle(Config.RngRecordingAreaAddress + 0x10)
                - _stream.GetSingle(Config.RngRecordingAreaAddress + 0x1C));
            float movementY = (_stream.GetSingle(Config.RngRecordingAreaAddress + 0x14)
                - _stream.GetSingle(Config.RngRecordingAreaAddress + 0x20));
            float movementZ = (_stream.GetSingle(Config.RngRecordingAreaAddress + 0x18)
                - _stream.GetSingle(Config.RngRecordingAreaAddress + 0x24));

            foreach (var specialVar in _specialWatchVars)
            {
                switch(specialVar.SpecialName)
                {
                    case "DeFactoSpeed":
                        if (floorTriangle != 0x00)
                        {
                            float normY = _stream.GetSingle(floorTriangle + Config.TriangleOffsets.NormY);
                            (specialVar as DataContainer).Text = Math.Round(hSpeed * normY, 3).ToString();
                        }
                        else
                        {
                            (specialVar as DataContainer).Text = "(No Floor)";
                        }
                        break;

                    case "SlidingSpeed":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(slidingSpeedX * slidingSpeedX + slidingSpeedZ * slidingSpeedZ), 3).ToString();
                        break;

                    case "SlidingAngle":
                        (specialVar as AngleDataContainer).AngleValue = Math.PI / 2 - Math.Atan2(slidingSpeedZ, slidingSpeedX);
                        (specialVar as AngleDataContainer).ValueExists = (slidingSpeedX != 0) || (slidingSpeedZ != 0);
                        break;

                    case "FallHeight":
                        (specialVar as DataContainer).Text = (_stream.GetSingle(Config.Mario.StructAddress + Config.Mario.PeakHeightOffset) - floorY).ToString();
                        break;

                    case "ActionDescription":
                        (specialVar as DataContainer).Text = Config.MarioActions.GetActionName(_stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.ActionOffset));
                        break;

                    case "PrevActionDescription":
                        (specialVar as DataContainer).Text = Config.MarioActions.GetActionName(_stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.PrevActionOffset));
                        break;

                    case "MovementX":
                        (specialVar as DataContainer).Text = movementX.ToString();
                        break;

                    case "MovementY":
                        (specialVar as DataContainer).Text = movementY.ToString();
                        break;

                    case "MovementZ":
                        (specialVar as DataContainer).Text = movementZ.ToString();
                        break;

                    case "MovementLateral":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(movementX * movementX + movementZ * movementZ),3).ToString();
                        break;

                    case "Movement":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(movementX * movementX + movementY * movementY + movementZ * movementZ), 3).ToString();
                        break;

                    case "QFrameCountEstimate":
                        var oldHSpeed = _stream.GetSingle(Config.RngRecordingAreaAddress + 0x28);
                        var qframes = Math.Abs(Math.Round(Math.Sqrt(movementX * movementX + movementZ * movementZ) / (oldHSpeed / 4)));
                        if (qframes > 4)
                            qframes = double.NaN;
                        (specialVar as DataContainer).Text = qframes.ToString();
                        break;
                }
            }
        }

        private void ToggleHandsfree_Click(object sender, EventArgs e)
        {
            MarioActions.ToggleHandsfree(_stream);
        }

        private void ToggleVisibility_Click(object sender, EventArgs e)
        {
            MarioActions.ToggleVisibility(_stream);
        }

        private void marioPosXZButton_Click(object sender, EventArgs e, int xSign, int zSign)
        {
            float xzValue;
            if (!float.TryParse(_marioPosXZTextbox.Text, out xzValue))
                return;

            ushort marioYaw = _stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
            MarioActions.MoveMario(_stream, xSign * xzValue, 0, zSign * xzValue, _marioPosRelative, marioYaw);
        }

        private void marioPosYButton_Click(object sender, EventArgs e, int ySign)
        {
            float yValue;
            if (!float.TryParse(_marioPosYTextbox.Text, out yValue))
                return;

            ushort marioYaw = _stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
            MarioActions.MoveMario(_stream, 0, ySign * yValue, 0, _marioPosRelative, marioYaw);
        }

        private void marioPosRelativeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _marioPosRelative = (sender as CheckBox).Checked;
            if (_marioPosRelative)
            {
                _marioPosXpButton.Text = "R";
                _marioPosXnButton.Text = "L";
                _marioPosZpButton.Text = "B";
                _marioPosZnButton.Text = "F";
                _marioPosXpZpButton.Text = "BR";
                _marioPosXpZnButton.Text = "FR";
                _marioPosXnZpButton.Text = "BL";
                _marioPosXnZnButton.Text = "FR";
                _marioPosYpButton.Text = "U";
                _marioPosYnButton.Text = "D";
            }
            else
            {
                _marioPosXpButton.Text = "X+";
                _marioPosXnButton.Text = "X-";
                _marioPosZpButton.Text = "Z+";
                _marioPosZnButton.Text = "Z-";
                _marioPosXpZpButton.Text = "X+Z+";
                _marioPosXpZnButton.Text = "X+Z-";
                _marioPosXnZpButton.Text = "X-Z+";
                _marioPosXnZnButton.Text = "X-Z-";
                _marioPosYpButton.Text = "Y+";
                _marioPosYnButton.Text = "Y-";
            }
        }

        private void marioHOLPXZButton_Click(object sender, EventArgs e, int xSign, int zSign)
        {
            float xzValue;
            if (!float.TryParse(_marioHOLPXZTextbox.Text, out xzValue))
                return;

            MarioActions.MoveHOLP(_stream, xSign * xzValue, 0, zSign * xzValue);
        }

        private void marioHOLPYButton_Click(object sender, EventArgs e, int ySign)
        {
            float yValue;
            if (!float.TryParse(_marioHOLPYTextbox.Text, out yValue))
                return;

            MarioActions.MoveHOLP(_stream, 0, ySign * yValue, 0);
        }

        private void marioStatsYawButton_Click(object sender, EventArgs e, int sign)
        {
            int yaw;
            if (!int.TryParse(_marioStatsYawTextbox.Text, out yaw))
                return;

            MarioActions.MarioChangeYaw(_stream, sign * yaw);
        }

        private void marioStatsHspdButton_Click(object sender, EventArgs e, int sign)
        {
            float hspd;
            if (!float.TryParse(_marioStatsHspdTextbox.Text, out hspd))
                return;

            MarioActions.MarioChangeHspd(_stream, sign * hspd);
        }

        private void marioStatsVspdButton_Click(object sender, EventArgs e, int sign)
        {
            float vspd;
            if (!float.TryParse(_marioStatsVspdTextbox.Text, out vspd))
                return;

            MarioActions.MarioChangeVspd(_stream, sign * vspd);
        }

        public override void Update(bool updateView)
        {
            // Get Mario position and rotation
            float x, y, z, rot;
            var marioAddress = Config.Mario.StructAddress;
            x = _stream.GetSingle(marioAddress + Config.Mario.XOffset);
            y = _stream.GetSingle(marioAddress + Config.Mario.YOffset);
            z = _stream.GetSingle(marioAddress + Config.Mario.ZOffset);
            rot = (float) (((_stream.GetUInt32(marioAddress + Config.Mario.RotationOffset) >> 16) % 65536) / 65536f * 360f); 

            // Update Mario map object
            _mapManager.MarioMapObject.X = x;
            _mapManager.MarioMapObject.Y = y;
            _mapManager.MarioMapObject.Z = z;
            _mapManager.MarioMapObject.Rotation = rot;
            _mapManager.MarioMapObject.Show = true;

            // Get holp position
            float holpX, holpY, holpZ;
            holpX = _stream.GetSingle(Config.HolpX);
            holpY = _stream.GetSingle(Config.HolpY);
            holpZ = _stream.GetSingle(Config.HolpZ);

            // Update holp map object position
            _mapManager.HolpMapObject.X = holpX;
            _mapManager.HolpMapObject.Y = holpY;
            _mapManager.HolpMapObject.Z = holpZ;
            _mapManager.HolpMapObject.Show = true;

            // Update camera position and rotation
            float cameraX, cameraY, cameraZ , cameraRot;
            cameraX = _stream.GetSingle(Config.Camera.CameraX);
            cameraY = _stream.GetSingle(Config.Camera.CameraY);
            cameraZ = _stream.GetSingle(Config.Camera.CameraZ);
            cameraRot = (float)(((UInt16)(_stream.GetUInt32(Config.Camera.CameraRot)) / 65536f * 360f));

            // Update floor triangle
            UInt32 floorTriangle = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
            if (floorTriangle != 0x00)
            {
                Int16 x1 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.X1);
                Int16 y1 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y1);
                Int16 z1 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z1);
                Int16 x2 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.X2);
                Int16 y2 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y2);
                Int16 z2 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z2);
                Int16 x3 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.X3);
                Int16 y3 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y3);
                Int16 z3 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z3);
                _mapManager.FloorTriangleMapObject.X1 = x1;
                _mapManager.FloorTriangleMapObject.Z1 = z1;
                _mapManager.FloorTriangleMapObject.X2 = x2;
                _mapManager.FloorTriangleMapObject.Z2 = z2;
                _mapManager.FloorTriangleMapObject.X3 = x3;
                _mapManager.FloorTriangleMapObject.Z3 = z3;
                _mapManager.FloorTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            _mapManager.FloorTriangleMapObject.Show = (floorTriangle != 0x00);

            // Update camera map object position
            _mapManager.CameraMapObject.X = cameraX;
            _mapManager.CameraMapObject.Y = cameraY;
            _mapManager.CameraMapObject.Z = cameraZ;
            _mapManager.CameraMapObject.Rotation = cameraRot;

            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();
            ProcessSpecialVars();
        }

        // FOR DEBUGGING
        /*
        private void marioStatsVspdButton_Click(object sender, EventArgs e, int sign)
        {
            float param1;
            if (!float.TryParse(_marioStatsYawTextbox.Text, out param1))
                return;

            float param2;
            if (!float.TryParse(_marioStatsHspdTextbox.Text, out param2))
                return;

            float param3;
            if (!float.TryParse(_marioStatsVspdTextbox.Text, out param3))
                return;

            float param4;
            if (!float.TryParse(_marioPosXZTextbox.Text, out param4))
                return;

            float param5;
            if (!float.TryParse(_marioPosYTextbox.Text, out param5))
                return;

            float param6;
            if (!float.TryParse(_marioHOLPXZTextbox.Text, out param6))
                return;

            System.Console.WriteLine("(x,y,z)=" + (param1, param2, param3) + " | (r,t,p)=" + MoreMath.EulerToSphericalRadians(param1, param2, param3));
            System.Console.WriteLine("(r,t,p)=" + (param1, param2, param3) + " | (x,y,z)=" + MoreMath.SphericalToEulerRadians(param1, param2, param3));
            System.Console.WriteLine("(x,y,z)=" + (param1, param2, param3) + " | (r,t,p)=" + MoreMath.EulerToSphericalAngleUnits(param1, param2, param3));
            System.Console.WriteLine("(r,t,p)=" + (param1, param2, param3) + " | (x,y,z)=" + MoreMath.SphericalToEulerAngleUnits(param1, param2, param3));
            System.Console.WriteLine("radians=" + param3 + " | angleUnits=" + MoreMath.RadiansToAngleUnits(param3));
            System.Console.WriteLine("angleUnits=" + param3 + " | radians=" + MoreMath.AngleUnitsToRadians(param3));
            System.Console.WriteLine("(x,y,z)=" + (param1, param2, param3) + " | (dr,dt,dp)=" + (param6, param4, param5) + " | (x,y,z)=" + MoreMath.OffsetSpherically(param1, param2, param3, param6, param4, param5));
            System.Console.WriteLine();
        }
        */
    }
}
