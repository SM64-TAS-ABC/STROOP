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
        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
                VariableGroup.Hacks,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
            };

        public MarioManager(List<WatchVariableControlPrecursor> variables, Control marioControl, WatchVariablePanel variableTable)
            : base(variables, variableTable, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
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
            // Get Mario position and rotation
            float x, y, z, rot;
            x = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            y = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            z = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort marioFacing = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.YawFacingOffset);
            rot = (float) (((Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.RotationOffset) >> 16) % 65536) / 65536f * 360f); 

            // Update Mario map object
            Config.MapManager.MarioMapObject.X = x;
            Config.MapManager.MarioMapObject.Y = y;
            Config.MapManager.MarioMapObject.Z = z;
            Config.MapManager.MarioMapObject.Rotation = rot;
            Config.MapManager.MarioMapObject.Show = true;

            // Get holp position
            float holpX, holpY, holpZ;
            holpX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPXOffset);
            holpY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPYOffset);
            holpZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPZOffset);

            // Update holp map object position
            Config.MapManager.HolpMapObject.X = holpX;
            Config.MapManager.HolpMapObject.Y = holpY;
            Config.MapManager.HolpMapObject.Z = holpZ;
            Config.MapManager.HolpMapObject.Show = true;

            // Update camera position and rotation
            float cameraX, cameraY, cameraZ , cameraRot;
            cameraX = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
            cameraY = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
            cameraZ = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);
            cameraRot = (float)((Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.YawFacingOffset) / 65536f * 360f));

            // Update floor triangle
            UInt32 floorTriangle = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            if (floorTriangle != 0x00)
            {
                Int16 x1 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.X1);
                Int16 y1 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Y1);
                Int16 z1 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Z1);
                Int16 x2 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.X2);
                Int16 y2 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Y2);
                Int16 z2 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Z2);
                Int16 x3 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.X3);
                Int16 y3 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Y3);
                Int16 z3 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Z3);
                Config.MapManager.FloorTriangleMapObject.X1 = x1;
                Config.MapManager.FloorTriangleMapObject.Z1 = z1;
                Config.MapManager.FloorTriangleMapObject.X2 = x2;
                Config.MapManager.FloorTriangleMapObject.Z2 = z2;
                Config.MapManager.FloorTriangleMapObject.X3 = x3;
                Config.MapManager.FloorTriangleMapObject.Z3 = z3;
                Config.MapManager.FloorTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            Config.MapManager.FloorTriangleMapObject.Show = (floorTriangle != 0x00);

            // Update ceiling triangle
            UInt32 ceilingTriangle = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
            if (ceilingTriangle != 0x00)
            {
                Int16 x1 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.X1);
                Int16 y1 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Y1);
                Int16 z1 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Z1);
                Int16 x2 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.X2);
                Int16 y2 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Y2);
                Int16 z2 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Z2);
                Int16 x3 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.X3);
                Int16 y3 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Y3);
                Int16 z3 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Z3);
                Config.MapManager.CeilingTriangleMapObject.X1 = x1;
                Config.MapManager.CeilingTriangleMapObject.Z1 = z1;
                Config.MapManager.CeilingTriangleMapObject.X2 = x2;
                Config.MapManager.CeilingTriangleMapObject.Z2 = z2;
                Config.MapManager.CeilingTriangleMapObject.X3 = x3;
                Config.MapManager.CeilingTriangleMapObject.Z3 = z3;
                Config.MapManager.CeilingTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            Config.MapManager.CeilingTriangleMapObject.Show = (ceilingTriangle != 0x00);

            // Update intended next position map object position
            float normY = floorTriangle == 0 ? 1 : Config.Stream.GetSingle(floorTriangle + TriangleOffsetsConfig.NormY);
            float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
            bool aboveFloor = y > floorY + 0.001;
            double multiplier = aboveFloor ? 1 : normY;
            double defactoSpeed = hSpeed * multiplier;
            double defactoSpeedQStep = defactoSpeed * 0.25;
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.YawFacingOffset);
            ushort marioAngleTruncated = MoreMath.NormalizeAngleTruncated(marioAngle);
            (double xDist, double zDist) = MoreMath.GetComponentsFromVector(defactoSpeedQStep, marioAngleTruncated);
            double intendedNextPositionX = MoreMath.MaybeNegativeModulus(x + xDist, 65536);
            double intendedNextPositionZ = MoreMath.MaybeNegativeModulus(z + zDist, 65536);
            Config.MapManager.IntendedNextPositionMapObject.X = (float)intendedNextPositionX;
            Config.MapManager.IntendedNextPositionMapObject.Z = (float)intendedNextPositionZ;
            bool marioStationary = x == intendedNextPositionX && z == intendedNextPositionZ;
            double angleToIntendedNextPosition = MoreMath.AngleTo_AngleUnits(x, z, intendedNextPositionX, intendedNextPositionZ);
            /*
            _mapManager.IntendedNextPositionMapObject.Rotation =
                marioStationary ? (float)MoreMath.AngleUnitsToDegrees(marioAngle) : (float)MoreMath.AngleUnitsToDegrees(angleToIntendedNextPosition);
                */
            Config.MapManager.IntendedNextPositionMapObject.Rotation = rot;

            // Update camera map object position
            Config.MapManager.CameraMapObject.X = cameraX;
            Config.MapManager.CameraMapObject.Y = cameraY;
            Config.MapManager.CameraMapObject.Z = cameraZ;
            Config.MapManager.CameraMapObject.Rotation = cameraRot;

            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();
        }
    }
}
