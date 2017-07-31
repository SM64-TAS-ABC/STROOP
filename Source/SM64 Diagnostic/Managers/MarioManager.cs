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
using static SM64_Diagnostic.Controls.AngleDataContainer;

namespace SM64_Diagnostic.Managers
{
    public class MarioManager : DataManager
    {
        MapManager _mapManager;

        public MarioManager(List<WatchVariable> marioData, Control marioControl, NoTearFlowLayoutPanel variableTable, MapManager mapManager)
            : base(marioData, variableTable)
        {
            _mapManager = mapManager;

            SplitContainer splitContainerMario = marioControl.Controls["splitContainerMario"] as SplitContainer;

            var toggleHandsfree = splitContainerMario.Panel1.Controls["buttonMarioToggleHandsfree"] as Button;
            toggleHandsfree.Click += (sender, e) => ButtonUtilities.ToggleHandsfree();

            var toggleVisibility = splitContainerMario.Panel1.Controls["buttonMarioVisibility"] as Button;
            toggleVisibility.Click += (sender, e) => ButtonUtilities.ToggleVisibility();

            var marioPosGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioPos"] as GroupBox;
            ThreeDimensionController.initialize(
                CoordinateSystem.Euler,
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
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateMario(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var marioStatsGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioStats"] as GroupBox;
            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsYawN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsYawP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsYaw"] as TextBox,
                (float yawValue) =>
                {
                    ButtonUtilities.MarioChangeYaw((int)Math.Round(yawValue));
                });
            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsHspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsHspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsHspd"] as TextBox,
                (float hspdValue) =>
                {
                    ButtonUtilities.MarioChangeHspd(hspdValue);
                });

            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsVspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsVspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsVspd"] as TextBox,
                (float vspdValue) =>
                {
                    ButtonUtilities.MarioChangeVspd(vspdValue);
                });

            var marioHOLPGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioHOLP"] as GroupBox;
            ThreeDimensionController.initialize(
                CoordinateSystem.Euler,
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
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateHOLP(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
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
                new AngleDataContainer("DeltaYawIntendedFacing", AngleViewModeType.Signed),
                new DataContainer("FallHeight"),
                new DataContainer("MovementX"),
                new DataContainer("MovementY"),
                new DataContainer("MovementZ"),
                new DataContainer("MovementForwards"),
                new DataContainer("MovementSideways"),
                new DataContainer("MovementLateral"),
                new DataContainer("MovementTotal"),
                new DataContainer("MovementAngle"),
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

            ushort marioYawFacing = _stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
            ushort marioYawFacingTruncated = (ushort)(marioYawFacing / 16 * 16);
            ushort marioYawIntended = _stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawIntendedOffset);
            ushort marioYawIntendedTruncated = (ushort)(marioYawIntended / 16 * 16);

            float movementX = (_stream.GetSingle(Config.HackedAreaAddress + 0x10)
                - _stream.GetSingle(Config.HackedAreaAddress + 0x1C));
            float movementY = (_stream.GetSingle(Config.HackedAreaAddress + 0x14)
                - _stream.GetSingle(Config.HackedAreaAddress + 0x20));
            float movementZ = (_stream.GetSingle(Config.HackedAreaAddress + 0x18)
                - _stream.GetSingle(Config.HackedAreaAddress + 0x24));
            ushort marioAngle = _stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);

            double movementLateral = Math.Sqrt(movementX * movementX + movementZ * movementZ);
            double movementAngle = MoreMath.AngleTo_AngleUnits(0, 0, movementX, movementZ);
            (double movementSideways, double movementForwards) = MoreMath.GetComponentsFromVectorRelatively(movementLateral, movementAngle, marioAngle);

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

                    case "DeltaYawIntendedFacing":
                        (specialVar as AngleDataContainer).AngleValue = MoreMath.AngleUnitsToRadians(marioYawIntendedTruncated - marioYawFacingTruncated);
                        (specialVar as AngleDataContainer).ValueExists = true;
                        break;

                    case "FallHeight":
                        (specialVar as DataContainer).Text = (_stream.GetSingle(Config.Mario.StructAddress + Config.Mario.PeakHeightOffset) - floorY).ToString();
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

                    case "MovementForwards":
                        (specialVar as DataContainer).Text = Math.Round(movementForwards, 3).ToString();
                        break;

                    case "MovementSideways":
                        (specialVar as DataContainer).Text = Math.Round(movementSideways, 3).ToString();
                        break;

                    case "MovementLateral":
                        (specialVar as DataContainer).Text = Math.Round(movementLateral, 3).ToString();
                        break;

                    case "MovementTotal":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(movementX * movementX + movementY * movementY + movementZ * movementZ), 3).ToString();
                        break;

                    case "MovementAngle":
                        (specialVar as DataContainer).Text = MoreMath.FormatAngleUshort(movementAngle).ToString();
                        break;

                    case "QFrameCountEstimate":
                        var oldHSpeed = _stream.GetSingle(Config.HackedAreaAddress + 0x28);
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
            holpX = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPXOffset);
            holpY = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPYOffset);
            holpZ = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPZOffset);

            // Update holp map object position
            _mapManager.HolpMapObject.X = holpX;
            _mapManager.HolpMapObject.Y = holpY;
            _mapManager.HolpMapObject.Z = holpZ;
            _mapManager.HolpMapObject.Show = true;

            // Update camera position and rotation
            float cameraX, cameraY, cameraZ , cameraRot;
            cameraX = _stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.XOffset);
            cameraY = _stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.YOffset);
            cameraZ = _stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.ZOffset);
            cameraRot = (float)((_stream.GetUInt16(Config.Camera.CameraStructAddress + Config.Camera.YawFacingOffset) / 65536f * 360f));

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

            // Update ceiling triangle
            UInt32 ceilingTriangle = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.CeilingTriangleOffset);
            if (ceilingTriangle != 0x00)
            {
                Int16 x1 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.X1);
                Int16 y1 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Y1);
                Int16 z1 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Z1);
                Int16 x2 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.X2);
                Int16 y2 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Y2);
                Int16 z2 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Z2);
                Int16 x3 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.X3);
                Int16 y3 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Y3);
                Int16 z3 = _stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Z3);
                _mapManager.CeilingTriangleMapObject.X1 = x1;
                _mapManager.CeilingTriangleMapObject.Z1 = z1;
                _mapManager.CeilingTriangleMapObject.X2 = x2;
                _mapManager.CeilingTriangleMapObject.Z2 = z2;
                _mapManager.CeilingTriangleMapObject.X3 = x3;
                _mapManager.CeilingTriangleMapObject.Z3 = z3;
                _mapManager.CeilingTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            _mapManager.CeilingTriangleMapObject.Show = (ceilingTriangle != 0x00);

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
