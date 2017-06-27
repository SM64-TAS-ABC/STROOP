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
using SM64Diagnostic.Controls;

namespace SM64_Diagnostic.Managers
{
    public class MarioManager : DataManager
    {
        MapManager _mapManager;

        public MarioManager(ProcessStream stream, List<WatchVariable> marioData, Control marioControl, NoTearFlowLayoutPanel variableTable, MapManager mapManager)
            : base(stream, marioData, variableTable, Config.Mario.StructAddress)
        {
            _mapManager = mapManager;

            var toggleHandsfree = marioControl.Controls["buttonMarioToggleHandsfree"] as Button;
            toggleHandsfree.Click += (sender, e) => MarioActions.ToggleHandsfree(_stream);

            var toggleVisibility = marioControl.Controls["buttonMarioVisibility"] as Button;
            toggleVisibility.Click += (sender, e) => MarioActions.ToggleVisibility(_stream);

            var marioPosGroupBox = marioControl.Controls["groupBoxMarioPos"] as GroupBox;
            PositionController.initialize(
                marioPosGroupBox.Controls["buttonMarioPosXn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXnZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXnZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXpZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXpZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosYp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosYn"] as Button,
                marioPosGroupBox.Controls["textBoxMarioPosXZ"] as TextBox,
                marioPosGroupBox.Controls["textBoxMarioPosY"] as TextBox,
                marioPosGroupBox.Controls["checkBoxMarioPosRelative"] as CheckBox,
                (float xOffset, float yOffset, float zOffset, bool useRelative) =>
                {
                    MarioActions.MoveMario(
                        _stream,
                        xOffset,
                        yOffset,
                        zOffset,
                        useRelative);
                });

            var marioStatsGroupBox = marioControl.Controls["groupBoxMarioStats"] as GroupBox;
            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsYawN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsYawP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsYaw"] as TextBox,
                (float yawValue) =>
                {
                    MarioActions.MarioChangeYaw(_stream, (int)yawValue);
                });
            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsHspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsHspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsHspd"] as TextBox,
                (float hspdValue) =>
                {
                    MarioActions.MarioChangeHspd(_stream, hspdValue);
                });

            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsVspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsVspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsVspd"] as TextBox,
                (float vspdValue) =>
                {
                    MarioActions.MarioChangeVspd(_stream, vspdValue);
                });

            var marioHOLPGroupBox = marioControl.Controls["groupBoxMarioHOLP"] as GroupBox;
            PositionController.initialize(
                marioHOLPGroupBox.Controls["buttonMarioHOLPXn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXnZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXnZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXpZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXpZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPYp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPYn"] as Button,
                marioHOLPGroupBox.Controls["textBoxMarioHOLPXZ"] as TextBox,
                marioHOLPGroupBox.Controls["textBoxMarioHOLPY"] as TextBox,
                marioHOLPGroupBox.Controls["checkBoxMarioHOLPRelative"] as CheckBox,
                (float xOffset, float yOffset, float zOffset, bool useRelative) =>
                {
                    MarioActions.MoveHOLP(
                        _stream,
                        xOffset,
                        yOffset,
                        zOffset,
                        useRelative);
                });
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
    }
}
