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
using static SM64_Diagnostic.Controls.AngleDataContainer;
using static SM64_Diagnostic.Utilities.ControlUtilities;

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

            Button toggleHandsfree = splitContainerMario.Panel1.Controls["buttonMarioToggleHandsfree"] as Button;
            toggleHandsfree.Click += (sender, e) => ButtonUtilities.ToggleHandsfree();

            Button toggleVisibility = splitContainerMario.Panel1.Controls["buttonMarioVisibility"] as Button;
            toggleVisibility.Click += (sender, e) => ButtonUtilities.ToggleVisibility();

            var marioPosGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                marioPosGroupBox,
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
            ControlUtilities.InitializeScalarController(
                marioStatsGroupBox.Controls["buttonMarioStatsYawN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsYawP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsYaw"] as TextBox,
                (float yawValue) =>
                {
                    ButtonUtilities.MarioChangeYaw((int)Math.Round(yawValue));
                });
            ControlUtilities.InitializeScalarController(
                marioStatsGroupBox.Controls["buttonMarioStatsHspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsHspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsHspd"] as TextBox,
                (float hspdValue) =>
                {
                    ButtonUtilities.MarioChangeHspd(hspdValue);
                });
            ControlUtilities.InitializeScalarController(
                marioStatsGroupBox.Controls["buttonMarioStatsVspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsVspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsVspd"] as TextBox,
                (float vspdValue) =>
                {
                    ButtonUtilities.MarioChangeVspd(vspdValue);
                });

            var marioSlidingSpeedGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioSlidingSpeed"] as GroupBox;
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedXn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedXp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedX"] as TextBox,
                (float yawValue) =>
                {
                    ButtonUtilities.MarioChangeYaw((int)Math.Round(yawValue));
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedZn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedZp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedZ"] as TextBox,
                (float hspdValue) =>
                {
                    ButtonUtilities.MarioChangeHspd(hspdValue);
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedHn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedHp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedH"] as TextBox,
                (float vspdValue) =>
                {
                    ButtonUtilities.MarioChangeVspd(vspdValue);
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedYawN"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedYawP"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedYaw"] as TextBox,
                (float vspdValue) =>
                {
                    ButtonUtilities.MarioChangeVspd(vspdValue);
                });

            Button buttonMarioHOLPGoto = splitContainerMario.Panel1.Controls["buttonMarioHOLPGoto"] as Button;
            buttonMarioHOLPGoto.Click += (sender, e) => ButtonUtilities.GotoHOLP();
            ControlUtilities.AddContextMenuStripFunctions(
                buttonMarioHOLPGoto,
                new List<string>() { "Goto HOLP", "Goto HOLP Laterally", "Goto HOLP X", "Goto HOLP Y", "Goto HOLP Z" },
                new List<Action>() {
                    () => ButtonUtilities.GotoHOLP((true, true, true)),
                    () => ButtonUtilities.GotoHOLP((true, false, true)),
                    () => ButtonUtilities.GotoHOLP((true, false, false)),
                    () => ButtonUtilities.GotoHOLP((false, true, false)),
                    () => ButtonUtilities.GotoHOLP((false, false, true)),
                });

            Button buttonMarioHOLPRetrieve = splitContainerMario.Panel1.Controls["buttonMarioHOLPRetrieve"] as Button;
            buttonMarioHOLPRetrieve.Click += (sender, e) => ButtonUtilities.RetrieveHOLP();
            ControlUtilities.AddContextMenuStripFunctions(
                buttonMarioHOLPRetrieve,
                new List<string>() { "Retrieve HOLP", "Retrieve HOLP Laterally", "Retrieve HOLP X", "Retrieve HOLP Y", "Retrieve HOLP Z" },
                new List<Action>() {
                    () => ButtonUtilities.RetrieveHOLP((true, true, true)),
                    () => ButtonUtilities.RetrieveHOLP((true, false, true)),
                    () => ButtonUtilities.RetrieveHOLP((true, false, false)),
                    () => ButtonUtilities.RetrieveHOLP((false, true, false)),
                    () => ButtonUtilities.RetrieveHOLP((false, false, true)),
                });

            var marioHOLPGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioHOLP"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                marioHOLPGroupBox,
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

        protected override List<SpecialWatchVariable> _specialWatchVars { get; } = new List<SpecialWatchVariable>()
        {
            new SpecialWatchVariable("DeFactoSpeed"),
            new SpecialWatchVariable("SlidingSpeed"),
            new SpecialWatchVariable("DeltaYawIntendedFacing", true, AngleViewModeType.Signed),
            new SpecialWatchVariable("FallHeight"),
            new SpecialWatchVariable("MovementX"),
            new SpecialWatchVariable("MovementY"),
            new SpecialWatchVariable("MovementZ"),
            new SpecialWatchVariable("MovementForwards"),
            new SpecialWatchVariable("MovementSideways"),
            new SpecialWatchVariable("MovementHorizontal"),
            new SpecialWatchVariable("MovementTotal"),
            new SpecialWatchVariable("MovementAngle"),
            new SpecialWatchVariable("QFrameCountEstimate")
        };

        public void ProcessSpecialVars()
        {
            UInt32 floorTriangle = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
            var floorY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.FloorYOffset);

            float hSpeed = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HSpeedOffset);

            float slidingSpeedX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.SlidingSpeedXOffset);
            float slidingSpeedZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.SlidingSpeedZOffset);

            ushort marioYawFacing = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
            ushort marioYawFacingTruncated = MoreMath.NormalizeAngleTruncated(marioYawFacing);
            ushort marioYawIntended = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawIntendedOffset);
            ushort marioYawIntendedTruncated = MoreMath.NormalizeAngleTruncated(marioYawIntended);

            float movementX = (Config.Stream.GetSingle(Config.HackedAreaAddress + 0x10)
                - Config.Stream.GetSingle(Config.HackedAreaAddress + 0x1C));
            float movementY = (Config.Stream.GetSingle(Config.HackedAreaAddress + 0x14)
                - Config.Stream.GetSingle(Config.HackedAreaAddress + 0x20));
            float movementZ = (Config.Stream.GetSingle(Config.HackedAreaAddress + 0x18)
                - Config.Stream.GetSingle(Config.HackedAreaAddress + 0x24));
            ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);

            double movementHorizontal = Math.Sqrt(movementX * movementX + movementZ * movementZ);
            double movementAngle = MoreMath.AngleTo_AngleUnits(0, 0, movementX, movementZ);
            (double movementSideways, double movementForwards) = MoreMath.GetComponentsFromVectorRelatively(movementHorizontal, movementAngle, marioAngle);

            foreach (var specialVar in _specialDataControls)
            {
                switch(specialVar.SpecialName)
                {
                    case "DeFactoSpeed":
                        if (floorTriangle != 0x00)
                        {
                            float normY = Config.Stream.GetSingle(floorTriangle + Config.TriangleOffsets.NormY);
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

                    case "DeltaYawIntendedFacing":
                        (specialVar as AngleDataContainer).AngleValue = marioYawIntendedTruncated - marioYawFacingTruncated;
                        (specialVar as AngleDataContainer).ValueExists = true;
                        break;

                    case "FallHeight":
                        (specialVar as DataContainer).Text = (Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.PeakHeightOffset) - floorY).ToString();
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

                    case "MovementHorizontal":
                        (specialVar as DataContainer).Text = Math.Round(movementHorizontal, 3).ToString();
                        break;

                    case "MovementTotal":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(movementX * movementX + movementY * movementY + movementZ * movementZ), 3).ToString();
                        break;

                    case "MovementAngle":
                        (specialVar as DataContainer).Text = MoreMath.NormalizeAngleUshort(movementAngle).ToString();
                        break;

                    case "QFrameCountEstimate":
                        var oldHSpeed = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x28);
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
            x = Config.Stream.GetSingle(marioAddress + Config.Mario.XOffset);
            y = Config.Stream.GetSingle(marioAddress + Config.Mario.YOffset);
            z = Config.Stream.GetSingle(marioAddress + Config.Mario.ZOffset);
            ushort marioFacing = Config.Stream.GetUInt16(marioAddress + Config.Mario.YawFacingOffset);
            rot = (float) (((Config.Stream.GetUInt32(marioAddress + Config.Mario.RotationOffset) >> 16) % 65536) / 65536f * 360f); 

            // Update Mario map object
            _mapManager.MarioMapObject.X = x;
            _mapManager.MarioMapObject.Y = y;
            _mapManager.MarioMapObject.Z = z;
            _mapManager.MarioMapObject.Rotation = rot;
            _mapManager.MarioMapObject.Show = true;

            // Get holp position
            float holpX, holpY, holpZ;
            holpX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPXOffset);
            holpY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPYOffset);
            holpZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPZOffset);

            // Update holp map object position
            _mapManager.HolpMapObject.X = holpX;
            _mapManager.HolpMapObject.Y = holpY;
            _mapManager.HolpMapObject.Z = holpZ;
            _mapManager.HolpMapObject.Show = true;

            // Update camera position and rotation
            float cameraX, cameraY, cameraZ , cameraRot;
            cameraX = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.XOffset);
            cameraY = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.YOffset);
            cameraZ = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.ZOffset);
            cameraRot = (float)((Config.Stream.GetUInt16(Config.Camera.CameraStructAddress + Config.Camera.YawFacingOffset) / 65536f * 360f));

            // Update floor triangle
            UInt32 floorTriangle = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
            if (floorTriangle != 0x00)
            {
                Int16 x1 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.X1);
                Int16 y1 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y1);
                Int16 z1 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z1);
                Int16 x2 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.X2);
                Int16 y2 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y2);
                Int16 z2 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z2);
                Int16 x3 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.X3);
                Int16 y3 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y3);
                Int16 z3 = Config.Stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z3);
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
            UInt32 ceilingTriangle = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.CeilingTriangleOffset);
            if (ceilingTriangle != 0x00)
            {
                Int16 x1 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.X1);
                Int16 y1 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Y1);
                Int16 z1 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Z1);
                Int16 x2 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.X2);
                Int16 y2 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Y2);
                Int16 z2 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Z2);
                Int16 x3 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.X3);
                Int16 y3 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Y3);
                Int16 z3 = Config.Stream.GetInt16(ceilingTriangle + Config.TriangleOffsets.Z3);
                _mapManager.CeilingTriangleMapObject.X1 = x1;
                _mapManager.CeilingTriangleMapObject.Z1 = z1;
                _mapManager.CeilingTriangleMapObject.X2 = x2;
                _mapManager.CeilingTriangleMapObject.Z2 = z2;
                _mapManager.CeilingTriangleMapObject.X3 = x3;
                _mapManager.CeilingTriangleMapObject.Z3 = z3;
                _mapManager.CeilingTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            _mapManager.CeilingTriangleMapObject.Show = (ceilingTriangle != 0x00);

            // Update intended next position map object position
            float normY = floorTriangle == 0 ? 1 : Config.Stream.GetSingle(floorTriangle + Config.TriangleOffsets.NormY);
            float hSpeed = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HSpeedOffset);
            float floorY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.FloorYOffset);
            bool aboveFloor = y > floorY + 0.001;
            double multiplier = aboveFloor ? 1 : normY;
            double defactoSpeed = hSpeed * multiplier;
            double defactoSpeedQStep = defactoSpeed * 0.25;
            ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
            ushort marioAngleTruncated = MoreMath.NormalizeAngleTruncated(marioAngle);
            (double xDist, double zDist) = MoreMath.GetComponentsFromVector(defactoSpeedQStep, marioAngleTruncated);
            double intendedNextPositionX = MoreMath.MaybeNegativeModulus(x + xDist, 65536);
            double intendedNextPositionZ = MoreMath.MaybeNegativeModulus(z + zDist, 65536);
            _mapManager.IntendedNextPositionMapObject.X = (float)intendedNextPositionX;
            _mapManager.IntendedNextPositionMapObject.Z = (float)intendedNextPositionZ;
            bool marioStationary = x == intendedNextPositionX && z == intendedNextPositionZ;
            double angleToIntendedNextPosition = MoreMath.AngleTo_AngleUnits(x, z, intendedNextPositionX, intendedNextPositionZ);
            /*
            _mapManager.IntendedNextPositionMapObject.Rotation =
                marioStationary ? (float)MoreMath.AngleUnitsToDegrees(marioAngle) : (float)MoreMath.AngleUnitsToDegrees(angleToIntendedNextPosition);
                */
            _mapManager.IntendedNextPositionMapObject.Rotation = rot;

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
