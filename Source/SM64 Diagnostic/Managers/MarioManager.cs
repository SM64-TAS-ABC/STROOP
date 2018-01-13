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
using static SM64_Diagnostic.Utilities.ControlUtilities;

namespace SM64_Diagnostic.Managers
{
    public class MarioManager : DataManager
    {
        public MarioManager(List<VarXControl> variables, Control marioControl, VariablePanel variableTable)
            : base(variables, variableTable)
        {
            variableTable.SetVariableGroups(
                new List<VariableGroup>()
                {
                    VariableGroup.Basic,
                    VariableGroup.Intermediate,
                    VariableGroup.Advanced,
                    VariableGroup.Hacks,
                },
                new List<VariableGroup>()
                {
                    VariableGroup.Basic,
                    VariableGroup.Intermediate,
                    VariableGroup.Advanced,
                });

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
                (float value) =>
                {
                    ButtonUtilities.MarioChangeYaw((int)Math.Round(value));
                });
            ControlUtilities.InitializeScalarController(
                marioStatsGroupBox.Controls["buttonMarioStatsHspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsHspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsHspd"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeHspd(value);
                });
            ControlUtilities.InitializeScalarController(
                marioStatsGroupBox.Controls["buttonMarioStatsVspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsVspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsVspd"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeVspd(value);
                });

            var marioSlidingSpeedGroupBox = splitContainerMario.Panel1.Controls["groupBoxMarioSlidingSpeed"] as GroupBox;
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedXn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedXp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedX"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeSlidingSpeedX(value);
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedZn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedZp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedZ"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeSlidingSpeedZ(value);
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedHn"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedHp"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedH"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeSlidingSpeedH(value);
                });
            ControlUtilities.InitializeScalarController(
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedYawN"] as Button,
                marioSlidingSpeedGroupBox.Controls["buttonMarioSlidingSpeedYawP"] as Button,
                marioSlidingSpeedGroupBox.Controls["textBoxMarioSlidingSpeedYaw"] as TextBox,
                (float value) =>
                {
                    ButtonUtilities.MarioChangeSlidingSpeedYaw(value);
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

        public override void Update(bool updateView)
        {
            MapManager mapManager = ManagerContext.Current.MapManager;
            // Get Mario position and rotation
            float x, y, z, rot;
            var marioAddress = Config.Mario.StructAddress;
            x = Config.Stream.GetSingle(marioAddress + Config.Mario.XOffset);
            y = Config.Stream.GetSingle(marioAddress + Config.Mario.YOffset);
            z = Config.Stream.GetSingle(marioAddress + Config.Mario.ZOffset);
            ushort marioFacing = Config.Stream.GetUInt16(marioAddress + Config.Mario.YawFacingOffset);
            rot = (float) (((Config.Stream.GetUInt32(marioAddress + Config.Mario.RotationOffset) >> 16) % 65536) / 65536f * 360f); 

            // Update Mario map object
            mapManager.MarioMapObject.X = x;
            mapManager.MarioMapObject.Y = y;
            mapManager.MarioMapObject.Z = z;
            mapManager.MarioMapObject.Rotation = rot;
            mapManager.MarioMapObject.Show = true;

            // Get holp position
            float holpX, holpY, holpZ;
            holpX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPXOffset);
            holpY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPYOffset);
            holpZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPZOffset);

            // Update holp map object position
            mapManager.HolpMapObject.X = holpX;
            mapManager.HolpMapObject.Y = holpY;
            mapManager.HolpMapObject.Z = holpZ;
            mapManager.HolpMapObject.Show = true;

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
                mapManager.FloorTriangleMapObject.X1 = x1;
                mapManager.FloorTriangleMapObject.Z1 = z1;
                mapManager.FloorTriangleMapObject.X2 = x2;
                mapManager.FloorTriangleMapObject.Z2 = z2;
                mapManager.FloorTriangleMapObject.X3 = x3;
                mapManager.FloorTriangleMapObject.Z3 = z3;
                mapManager.FloorTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            mapManager.FloorTriangleMapObject.Show = (floorTriangle != 0x00);

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
                mapManager.CeilingTriangleMapObject.X1 = x1;
                mapManager.CeilingTriangleMapObject.Z1 = z1;
                mapManager.CeilingTriangleMapObject.X2 = x2;
                mapManager.CeilingTriangleMapObject.Z2 = z2;
                mapManager.CeilingTriangleMapObject.X3 = x3;
                mapManager.CeilingTriangleMapObject.Z3 = z3;
                mapManager.CeilingTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            mapManager.CeilingTriangleMapObject.Show = (ceilingTriangle != 0x00);

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
            mapManager.IntendedNextPositionMapObject.X = (float)intendedNextPositionX;
            mapManager.IntendedNextPositionMapObject.Z = (float)intendedNextPositionZ;
            bool marioStationary = x == intendedNextPositionX && z == intendedNextPositionZ;
            double angleToIntendedNextPosition = MoreMath.AngleTo_AngleUnits(x, z, intendedNextPositionX, intendedNextPositionZ);
            /*
            _mapManager.IntendedNextPositionMapObject.Rotation =
                marioStationary ? (float)MoreMath.AngleUnitsToDegrees(marioAngle) : (float)MoreMath.AngleUnitsToDegrees(angleToIntendedNextPosition);
                */
            mapManager.IntendedNextPositionMapObject.Rotation = rot;

            // Update camera map object position
            mapManager.CameraMapObject.X = cameraX;
            mapManager.CameraMapObject.Y = cameraY;
            mapManager.CameraMapObject.Z = cameraZ;
            mapManager.CameraMapObject.Rotation = cameraRot;

            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();
        }
    }
}
