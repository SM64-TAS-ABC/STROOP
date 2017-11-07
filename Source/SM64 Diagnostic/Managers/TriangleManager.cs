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
using SM64_Diagnostic.Structs.Configurations;
using static SM64_Diagnostic.Controls.AngleDataContainer;
using static SM64_Diagnostic.Utilities.ControlUtilities;

namespace SM64_Diagnostic.Managers
{
    public class TriangleManager : DataManager
    {
        public static TriangleManager Instance = null;

        MaskedTextBox _addressBox;
        uint _triangleAddress = 0;
        CheckBox _useMisalignmentOffsetCheckbox;
        int _closestVertex = 0;

        public enum TriangleMode { Floor, Wall, Ceiling, Other };
        public TriangleMode Mode = TriangleMode.Floor;

        CheckBox _checkBoxNeutralizeTriangle;

        CheckBox _recordTriangleDataCheckbox;
        CheckBox _repeatFirstVertexCheckbox;
        Label _recordTriangleCountLabel;
        List<short[]> _triangleData;

        public uint TriangleAddress
        {
            get
            {
                return _triangleAddress;
            }
            private set
            {
                if (_triangleAddress == value)
                    return;

                _triangleAddress = value;

                _addressBox.Text = String.Format("0x{0:X8}", _triangleAddress);
            }
        }

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("DistanceAboveFloor"),
                new DataContainer("DistanceBelowCeiling"),
                new DataContainer("ClosestVertex"),
                new DataContainer("ClosestVertexX"),
                new DataContainer("ClosestVertexY"),
                new DataContainer("ClosestVertexZ"),
                new AngleDataContainer("UpHillAngle"),
                new AngleDataContainer("DownHillAngle"),
                new AngleDataContainer("LeftHillAngle"),
                new AngleDataContainer("RightHillAngle"),
                new AngleDataContainer("UpHillDeltaAngle", AngleViewModeType.Signed),
                new AngleDataContainer("DownHillDeltaAngle", AngleViewModeType.Signed),
                new AngleDataContainer("LeftHillDeltaAngle", AngleViewModeType.Signed),
                new AngleDataContainer("RightHillDeltaAngle", AngleViewModeType.Signed),
                new DataContainer("Classification"),
                new AngleDataContainer("Steepness"),
                new DataContainer("NormalDistAway"),
                new DataContainer("VerticalDistAway"),
                new DataContainer("HeightOnSlope"),

                new DataContainer("DistanceToV1"),
                new DataContainer("YDistanceToV1"),
                new DataContainer("XDistanceToV1"),
                new DataContainer("ZDistanceToV1"),
                new DataContainer("HDistanceToV1"),
                new DataContainer("DistanceToV2"),
                new DataContainer("YDistanceToV2"),
                new DataContainer("XDistanceToV2"),
                new DataContainer("ZDistanceToV2"),
                new DataContainer("HDistanceToV2"),
                new DataContainer("DistanceToV3"),
                new DataContainer("YDistanceToV3"),
                new DataContainer("XDistanceToV3"),
                new DataContainer("ZDistanceToV3"),
                new DataContainer("HDistanceToV3"),

                new DataContainer("DistanceToV2"),
                new DataContainer("HorizontalDistanceToV2"),
                new DataContainer("VerticalDistanceToV2"),
                new DataContainer("DistanceToV3"),
                new DataContainer("HorizontalDistanceToV3"),
                new DataContainer("VerticalDistanceToV3"),

                new DataContainer("DistanceToLine12"),
                new DataContainer("DistanceToLine23"),
                new DataContainer("DistanceToLine13"),

                new AngleDataContainer("AngleMarioToV1"),
                new AngleDataContainer("DeltaAngleMarioToV1", AngleViewModeType.Signed),
                new AngleDataContainer("AngleV1ToMario"),
                new AngleDataContainer("AngleMarioToV2"),
                new AngleDataContainer("DeltaAngleMarioToV2", AngleViewModeType.Signed),
                new AngleDataContainer("AngleV2ToMario"),
                new AngleDataContainer("AngleMarioToV3"),
                new AngleDataContainer("DeltaAngleMarioToV3", AngleViewModeType.Signed),
                new AngleDataContainer("AngleV3ToMario"),

                new AngleDataContainer("AngleV1ToV2"),
                new AngleDataContainer("AngleV2ToV1"),
                new AngleDataContainer("AngleV2ToV3"),
                new AngleDataContainer("AngleV3ToV2"),
                new AngleDataContainer("AngleV1ToV3"),
                new AngleDataContainer("AngleV3ToV1"),

                new DataContainer("ObjectTriCount"),
            };
        }

        /// <summary>
        /// Manages illumanati
        /// </summary>
        public TriangleManager(Control tabControl, List<WatchVariable> triangleWatchVars, NoTearFlowLayoutPanel noTearFlowLayoutPanel) 
            : base(triangleWatchVars, noTearFlowLayoutPanel)
        {
            Instance = this;

            _triangleData = new List<short[]>();

            SplitContainer splitContainerTriangles = tabControl.Controls["splitContainerTriangles"] as SplitContainer;

            _addressBox = splitContainerTriangles.Panel1.Controls["maskedTextBoxOtherTriangle"] as MaskedTextBox;
            _useMisalignmentOffsetCheckbox = splitContainerTriangles.Panel1.Controls["checkBoxVertexMisalignment"] as CheckBox;

            _addressBox.KeyDown += AddressBox_KeyDown;
            (splitContainerTriangles.Panel1.Controls["radioButtonTriFloor"] as RadioButton).Click 
                += (sender, e) => Mode_Click(sender, e, TriangleMode.Floor);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriWall"] as RadioButton).Click
                += (sender, e) => Mode_Click(sender, e, TriangleMode.Wall);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriCeiling"] as RadioButton).Click
                += (sender, e) => Mode_Click(sender, e, TriangleMode.Ceiling);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriOther"] as RadioButton).Click
                += (sender, e) => Mode_Click(sender, e, TriangleMode.Other);

            (splitContainerTriangles.Panel1.Controls["labelTriangleSelection"] as Label).Click
                += (sender, e) => ShowTriangleCoordinates();

            (splitContainerTriangles.Panel1.Controls["buttonGotoV1"] as Button).Click
                += (sender, e) => ButtonUtilities.GotoTriangle(_triangleAddress, 1, _useMisalignmentOffsetCheckbox.Checked);
            (splitContainerTriangles.Panel1.Controls["buttonGotoV2"] as Button).Click
                += (sender, e) => ButtonUtilities.GotoTriangle(_triangleAddress, 2, _useMisalignmentOffsetCheckbox.Checked);
            (splitContainerTriangles.Panel1.Controls["buttonGotoV3"] as Button).Click
                += (sender, e) => ButtonUtilities.GotoTriangle(_triangleAddress, 3, _useMisalignmentOffsetCheckbox.Checked);
            (splitContainerTriangles.Panel1.Controls["buttonGotoVClosest"] as Button).Click += (sender, e) =>
            {
                if (_closestVertex == 0)
                    return;
                ButtonUtilities.GotoTriangle(_triangleAddress, _closestVertex, _useMisalignmentOffsetCheckbox.Checked);
            };

            (splitContainerTriangles.Panel1.Controls["buttonRetrieveTriangle"] as Button).Click
                += (sender, e) => ButtonUtilities.RetrieveTriangle(_triangleAddress);

            Button buttonNeutralizeTriangle = splitContainerTriangles.Panel1.Controls["buttonNeutralizeTriangle"] as Button;
            buttonNeutralizeTriangle.Click += (sender, e) => ButtonUtilities.NeutralizeTriangle(_triangleAddress);
            ControlUtilities.AddContextMenuStripFunctions(
                buttonNeutralizeTriangle,
                new List<string>() { "Neutralize with 0", "Neutralize with 21" },
                new List<Action>() {
                    () => ButtonUtilities.NeutralizeTriangle(_triangleAddress, false),
                    () => ButtonUtilities.NeutralizeTriangle(_triangleAddress, true),
                });

            (splitContainerTriangles.Panel1.Controls["buttonAnnihilateTriangle"] as Button).Click
                += (sender, e) => ButtonUtilities.AnnihilateTriangle(_triangleAddress);
            
            var trianglePosGroupBox = splitContainerTriangles.Panel1.Controls["groupBoxTrianglePos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                trianglePosGroupBox,
                trianglePosGroupBox.Controls["buttonTrianglePosXn"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosXp"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosZn"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosZp"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosXnZn"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosXnZp"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosXpZn"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosXpZp"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosYp"] as Button,
                trianglePosGroupBox.Controls["buttonTrianglePosYn"] as Button,
                trianglePosGroupBox.Controls["textBoxTrianglePosXZ"] as TextBox,
                trianglePosGroupBox.Controls["textBoxTrianglePosY"] as TextBox,
                trianglePosGroupBox.Controls["checkBoxTrianglePosRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.MoveTriangle(
                        _triangleAddress,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var triangleNormalGroupBox = splitContainerTriangles.Panel1.Controls["groupBoxTriangleNormal"] as GroupBox;
            ControlUtilities.InitializeScalarController(
                triangleNormalGroupBox.Controls["buttontriangleNormalN"] as Button,
                triangleNormalGroupBox.Controls["buttontriangleNormalP"] as Button,
                triangleNormalGroupBox.Controls["textBoxTriangleNormal"] as TextBox,
                (float normalValue) =>
                {
                    ButtonUtilities.MoveTriangleNormal(_triangleAddress, normalValue);
                });

            _checkBoxNeutralizeTriangle = splitContainerTriangles.Panel1.Controls["checkBoxNeutralizeTriangle"] as CheckBox;

            (splitContainerTriangles.Panel1.Controls["buttonTriangleShowCoords"] as Button).Click
                += (sender, e) => ShowTriangleCoordinates();
            (splitContainerTriangles.Panel1.Controls["buttonTriangleShowEquation"] as Button).Click
                += (sender, e) => ShowTriangleEquation();

            _recordTriangleDataCheckbox = splitContainerTriangles.Panel1.Controls["checkBoxRecordTriangleData"] as CheckBox;
            _recordTriangleCountLabel = splitContainerTriangles.Panel1.Controls["labelRecordTriangleCount"] as Label;

            (splitContainerTriangles.Panel1.Controls["buttonTriangleShowData"] as Button).Click
                += (sender, e) => ShowTriangleData();
            (splitContainerTriangles.Panel1.Controls["buttonTriangleShowVertices"] as Button).Click
                += (sender, e) => ShowTriangleVertices();
            (splitContainerTriangles.Panel1.Controls["buttonTriangleClearData"] as Button).Click
                += (sender, e) => ClearTriangleData();

            _repeatFirstVertexCheckbox = splitContainerTriangles.Panel1.Controls["checkBoxRepeatFirstVertex"] as CheckBox;

            (splitContainerTriangles.Panel1.Controls["buttonTriangleShowLevelTris"] as Button).Click
                += (sender, e) => ShowLevelTriangles();
            (splitContainerTriangles.Panel1.Controls["buttonTriangleShowObjTris"] as Button).Click
                += (sender, e) => ShowObjectTriangles();
            (splitContainerTriangles.Panel1.Controls["buttonTriangleShowAllTris"] as Button).Click
                += (sender, e) => ShowAllTriangles();
        }

        private short[] GetTriangleCoordinates(uint? nullableTriAddress = null)
        {
            uint triAddress = nullableTriAddress ?? TriangleAddress;
            short[] coordinates = new short[9];
            coordinates[0] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.X1);
            coordinates[1] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.Y1);
            coordinates[2] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.Z1);
            coordinates[3] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.X2);
            coordinates[4] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.Y2);
            coordinates[5] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.Z2);
            coordinates[6] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.X3);
            coordinates[7] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.Y3);
            coordinates[8] = Config.Stream.GetInt16(triAddress + Config.TriangleOffsets.Z3);
            return coordinates;
        }

        private void ShowTriangleCoordinates()
        {
            if (TriangleAddress == 0) return;
            var triangleInfoForm = new TriangleInfoForm();
            triangleInfoForm.SetCoordinates(GetTriangleCoordinates());
            triangleInfoForm.ShowDialog();
        }

        private void ShowTriangleEquation()
        {
            if (TriangleAddress == 0) return;

            float normX, normY, normZ, normOffset;
            normX = Config.Stream.GetSingle(TriangleAddress + Config.TriangleOffsets.NormX);
            normY = Config.Stream.GetSingle(TriangleAddress + Config.TriangleOffsets.NormY);
            normZ = Config.Stream.GetSingle(TriangleAddress + Config.TriangleOffsets.NormZ);
            normOffset = Config.Stream.GetSingle(TriangleAddress + Config.TriangleOffsets.NormOffset);

            var triangleInfoForm = new TriangleInfoForm();
            triangleInfoForm.SetEquation(normX, normY, normZ, normOffset);
            triangleInfoForm.ShowDialog();
        }

        private void ShowTriangleData()
        {
            var triangleInfoForm = new TriangleInfoForm();
            triangleInfoForm.SetData(_triangleData, _repeatFirstVertexCheckbox.Checked);
            triangleInfoForm.ShowDialog();
        }

        private void ShowTriangleVertices()
        {
            var triangleInfoForm = new TriangleInfoForm();
            triangleInfoForm.SetVertices(_triangleData);
            triangleInfoForm.ShowDialog();
        }

        private void ClearTriangleData()
        {
            _triangleData.Clear();
        }

        private void ShowLevelTriangles()
        {
            uint triangleListAddress = Config.Triangle.TriangleListPointerAddress;
            int numLevelTriangles = Config.Stream.GetInt32(Config.Triangle.LevelTriangleCountAddress);
            ShowTriangleRange(triangleListAddress, numLevelTriangles);
        }

        private void ShowObjectTriangles()
        {
            uint triangleListAddress = Config.Triangle.TriangleListPointerAddress;
            int numTotalTriangles = Config.Stream.GetInt32(Config.Triangle.TotalTriangleCountAddress);
            int numLevelTriangles = Config.Stream.GetInt32(Config.Triangle.LevelTriangleCountAddress);

            uint objectTriangleListAddress = triangleListAddress + (uint)(numLevelTriangles * Config.Triangle.TriangleStructSize);
            int numObjectTriangles = numTotalTriangles - numLevelTriangles;

            ShowTriangleRange(objectTriangleListAddress, numObjectTriangles);
        }

        private void ShowAllTriangles()
        {
            uint triangleListAddress = Config.Triangle.TriangleListPointerAddress;
            int numTotalTriangles = Config.Stream.GetInt32(Config.Triangle.TotalTriangleCountAddress);
            ShowTriangleRange(triangleListAddress, numTotalTriangles);
        }

        private void ShowTriangleRange(uint startAddress, int numTriangles)
        {
            List<TriangleStruct> triangleList = new List<TriangleStruct>();
            for (int i = 0; i < numTriangles; i++)
            {
                uint address = startAddress + (uint)(i * Config.Triangle.TriangleStructSize);
                TriangleStruct triangle = new TriangleStruct(address);
                triangleList.Add(triangle);
            }
            var triangleInfoForm = new TriangleInfoForm();
            triangleInfoForm.SetTriangles(triangleList);
            triangleInfoForm.ShowDialog();
        }

        private void ProcessSpecialVars()
        {
            var floorY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.GroundYOffset);

            // Get Mario position
            float marioX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            float marioZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            marioX = PuUtilities.GetRelativePuPosition(marioX);
            marioY = PuUtilities.GetRelativePuPosition(marioY);
            marioZ = PuUtilities.GetRelativePuPosition(marioZ);

            float normX = Config.Stream.GetSingle(TriangleAddress + Config.TriangleOffsets.NormX);
            float normY = Config.Stream.GetSingle(TriangleAddress + Config.TriangleOffsets.NormY);
            float normZ = Config.Stream.GetSingle(TriangleAddress + Config.TriangleOffsets.NormZ);
            float normOffset = Config.Stream.GetSingle(TriangleAddress + Config.TriangleOffsets.NormOffset);

            double uphillAngleRadians = Math.PI + Math.Atan2(normX, normZ);
            if (normX == 0 && normZ == 0)
                uphillAngleRadians = double.NaN;
            if (normY < -0.01)
                uphillAngleRadians += Math.PI;
            double downhillAngleRadians = uphillAngleRadians + Math.PI;
            double lefthillAngleRadians = uphillAngleRadians + Math.PI / 2;
            double righthillAngleRadians = uphillAngleRadians - Math.PI / 2;
            double uphillAngle = MoreMath.RadiansToAngleUnits(uphillAngleRadians);
            double downhillAngle = MoreMath.RadiansToAngleUnits(downhillAngleRadians);
            double lefthillAngle = MoreMath.RadiansToAngleUnits(lefthillAngleRadians);
            double righthillAngle = MoreMath.RadiansToAngleUnits(righthillAngleRadians);

            ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);

            short v1X = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.X1);
            short v1Y = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.Y1);
            short v1Z = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.Z1);
            short v2X = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.X2);
            short v2Y = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.Y2);
            short v2Z = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.Z2);
            short v3X = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.X3);
            short v3Y = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.Y3);
            short v3Z = Config.Stream.GetInt16(TriangleAddress + Config.TriangleOffsets.Z3);

            var disToV = new double[]
            {
                Math.Pow(marioX - v1X, 2) + Math.Pow(marioY - v1Y, 2) + Math.Pow(marioZ - v1Z, 2),
                Math.Pow(marioX - v2X, 2) + Math.Pow(marioY - v2Y, 2) + Math.Pow(marioZ - v2Z, 2),
                Math.Pow(marioX - v3X, 2) + Math.Pow(marioY - v3Y, 2) + Math.Pow(marioZ - v3Z, 2)
            };

            _closestVertex = disToV.IndexOfMin() + 1;

            double angleMarioToV1 = MoreMath.AngleTo_AngleUnits(marioX, marioZ, v1X, v1Z);
            double angleV1ToMario = MoreMath.AngleTo_AngleUnits(v1X, v1Z, marioX, marioZ);
            double angleMarioToV2 = MoreMath.AngleTo_AngleUnits(marioX, marioZ, v2X, v2Z);
            double angleV2ToMario = MoreMath.AngleTo_AngleUnits(v2X, v2Z, marioX, marioZ);
            double angleMarioToV3 = MoreMath.AngleTo_AngleUnits(marioX, marioZ, v3X, v3Z);
            double angleV3ToMario = MoreMath.AngleTo_AngleUnits(v3X, v3Z, marioX, marioZ);

            foreach (IDataContainer specialVar in _specialWatchVars)
            {
                switch (specialVar.SpecialName)
                {
                    case "DistanceAboveFloor":
                        (specialVar as DataContainer).Text = (marioY - floorY).ToString();
                        break;
                    case "DistanceBelowCeiling":
                        (specialVar as DataContainer).Text = (Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.CeilingYOffset)
                            - marioY).ToString();
                        break;
                    case "ClosestVertex":
                        (specialVar as DataContainer).Text = String.Format("V{0}", _closestVertex);
                        goto case "CheckTriangleExists";
                    case "ClosestVertexX":
                        short coordX = 0;
                        switch (_closestVertex)
                        {
                            case 1:
                                coordX = v1X;
                                break;
                            case 2:
                                coordX = v2X;
                                break;
                            case 3:
                                coordX = v3X;
                                break;
                        }
                        (specialVar as DataContainer).Text = coordX.ToString();
                        goto case "CheckTriangleExists";
                    case "ClosestVertexY":
                        short coordY = 0;
                        switch (_closestVertex)
                        {
                            case 1:
                                coordY = v1Y;
                                break;
                            case 2:
                                coordY = v2Y;
                                break;
                            case 3:
                                coordY = v3Y;
                                break;
                        }
                        (specialVar as DataContainer).Text = coordY.ToString();
                        goto case "CheckTriangleExists";
                    case "ClosestVertexZ":
                        short coordZ = 0;
                        switch (_closestVertex)
                        {
                            case 1:
                                coordZ = v1Z;
                                break;
                            case 2:
                                coordZ = v2Z;
                                break;
                            case 3:
                                coordZ = v3Z;
                                break;
                        }
                        (specialVar as DataContainer).Text = coordZ.ToString();
                        goto case "CheckTriangleExists";
                    case "UpHillAngle":
                        (specialVar as AngleDataContainer).AngleValue = uphillAngle;
                        goto case "CheckTriangleExistsAngle";
                    case "DownHillAngle":
                        (specialVar as AngleDataContainer).AngleValue = downhillAngle;
                        goto case "CheckTriangleExistsAngle";
                    case "LeftHillAngle":
                        (specialVar as AngleDataContainer).AngleValue = lefthillAngle;
                        goto case "CheckTriangleExistsAngle";
                    case "RightHillAngle":
                        (specialVar as AngleDataContainer).AngleValue = righthillAngle;
                        goto case "CheckTriangleExistsAngle";

                    case "UpHillDeltaAngle":
                        (specialVar as AngleDataContainer).AngleValue = marioAngle - uphillAngle;
                        goto case "CheckTriangleExistsAngle";
                    case "DownHillDeltaAngle":
                        (specialVar as AngleDataContainer).AngleValue = marioAngle - downhillAngle;
                        goto case "CheckTriangleExistsAngle";
                    case "LeftHillDeltaAngle":
                        (specialVar as AngleDataContainer).AngleValue = marioAngle - lefthillAngle;
                        goto case "CheckTriangleExistsAngle";
                    case "RightHillDeltaAngle":
                        (specialVar as AngleDataContainer).AngleValue = marioAngle - righthillAngle;
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
                        (specialVar as AngleDataContainer).AngleValue = MoreMath.RadiansToAngleUnits(Math.Acos(normY));
                        goto case "CheckTriangleExistsAngle";
                    case "NormalDistAway":
                        (specialVar as DataContainer).Text = Math.Round(marioX * normX + marioY * normY + marioZ * normZ + normOffset, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "VerticalDistAway":
                        (specialVar as DataContainer).Text = Math.Round(marioY + (marioX * normX + marioZ * normZ + normOffset) / normY, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "HeightOnSlope":
                        (specialVar as DataContainer).Text = Math.Round((-marioX * normX - marioZ * normZ - normOffset) / normY, 3).ToString();
                        goto case "CheckTriangleExists";

                    case "DistanceToV1":
                        (specialVar as DataContainer).Text = Math.Round(MoreMath.GetDistanceBetween(marioX, marioY, marioZ, v1X, v1Y, v1Z), 3).ToString();
                        goto case "CheckTriangleExists";
                    case "YDistanceToV1":
                        (specialVar as DataContainer).Text = Math.Round(marioY - v1Y, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "XDistanceToV1":
                        (specialVar as DataContainer).Text = Math.Round(marioX - v1X, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "ZDistanceToV1":
                        (specialVar as DataContainer).Text = Math.Round(marioZ - v1Z, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "HDistanceToV1":
                        (specialVar as DataContainer).Text = Math.Round(MoreMath.GetDistanceBetween(marioX, marioZ, v1X, v1Z), 3).ToString();
                        goto case "CheckTriangleExists";

                    case "DistanceToV2":
                        (specialVar as DataContainer).Text = Math.Round(MoreMath.GetDistanceBetween(marioX, marioY, marioZ, v2X, v2Y, v2Z), 3).ToString();
                        goto case "CheckTriangleExists";
                    case "YDistanceToV2":
                        (specialVar as DataContainer).Text = Math.Round(marioY - v2Y, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "XDistanceToV2":
                        (specialVar as DataContainer).Text = Math.Round(marioX - v2X, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "ZDistanceToV2":
                        (specialVar as DataContainer).Text = Math.Round(marioZ - v2Z, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "HDistanceToV2":
                        (specialVar as DataContainer).Text = Math.Round(MoreMath.GetDistanceBetween(marioX, marioZ, v2X, v2Z), 3).ToString();
                        goto case "CheckTriangleExists";

                    case "DistanceToV3":
                        (specialVar as DataContainer).Text = Math.Round(MoreMath.GetDistanceBetween(marioX, marioY, marioZ, v3X, v3Y, v3Z), 3).ToString();
                        goto case "CheckTriangleExists";
                    case "YDistanceToV3":
                        (specialVar as DataContainer).Text = Math.Round(marioY - v3Y, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "XDistanceToV3":
                        (specialVar as DataContainer).Text = Math.Round(marioX - v3X, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "ZDistanceToV3":
                        (specialVar as DataContainer).Text = Math.Round(marioZ - v3Z, 3).ToString();
                        goto case "CheckTriangleExists";
                    case "HDistanceToV3":
                        (specialVar as DataContainer).Text = Math.Round(MoreMath.GetDistanceBetween(marioX, marioZ, v3X, v3Z), 3).ToString();
                        goto case "CheckTriangleExists";

                    case "DistanceToLine12":
                        {
                            double dist = MoreMath.GetDistanceFromPointToLine(marioX, marioZ, v1X, v1Z, v2X, v2Z);
                            bool insideTri = MoreMath.IsPointInsideTriangle(marioX, marioZ, v1X, v1Z, v2X, v2Z, v3X, v3Z);
                            double signedDist = dist * (insideTri ? 1 : -1);
                            (specialVar as DataContainer).Text = Math.Round(signedDist, 3).ToString();
                        }
                        goto case "CheckTriangleExists";
                    case "DistanceToLine23":
                        {
                            double dist = MoreMath.GetDistanceFromPointToLine(marioX, marioZ, v2X, v2Z, v3X, v3Z);
                            bool insideTri = MoreMath.IsPointInsideTriangle(marioX, marioZ, v1X, v1Z, v2X, v2Z, v3X, v3Z);
                            double signedDist = dist * (insideTri ? 1 : -1);
                            (specialVar as DataContainer).Text = Math.Round(signedDist, 3).ToString();
                        }
                        goto case "CheckTriangleExists";
                    case "DistanceToLine13":
                        {
                            double dist = MoreMath.GetDistanceFromPointToLine(marioX, marioZ, v1X, v1Z, v3X, v3Z);
                            bool insideTri = MoreMath.IsPointInsideTriangle(marioX, marioZ, v1X, v1Z, v2X, v2Z, v3X, v3Z);
                            double signedDist = dist * (insideTri ? 1 : -1);
                            (specialVar as DataContainer).Text = Math.Round(signedDist, 3).ToString();
                        }
                        goto case "CheckTriangleExists";

                    case "AngleMarioToV1":
                        (specialVar as AngleDataContainer).AngleValue = angleMarioToV1;
                        goto case "CheckTriangleExistsAngle";
                    case "DeltaAngleMarioToV1":
                        (specialVar as AngleDataContainer).AngleValue = marioAngle - angleMarioToV1;
                        goto case "CheckTriangleExistsAngle";
                    case "AngleV1ToMario":
                        (specialVar as AngleDataContainer).AngleValue = angleV1ToMario;
                        goto case "CheckTriangleExistsAngle";
                    case "AngleMarioToV2":
                        (specialVar as AngleDataContainer).AngleValue = angleMarioToV2;
                        goto case "CheckTriangleExistsAngle";
                    case "DeltaAngleMarioToV2":
                        (specialVar as AngleDataContainer).AngleValue = marioAngle - angleMarioToV2;
                        goto case "CheckTriangleExistsAngle";
                    case "AngleV2ToMario":
                        (specialVar as AngleDataContainer).AngleValue = angleV2ToMario;
                        goto case "CheckTriangleExistsAngle";
                    case "AngleMarioToV3":
                        (specialVar as AngleDataContainer).AngleValue = angleMarioToV3;
                        goto case "CheckTriangleExistsAngle";
                    case "DeltaAngleMarioToV3":
                        (specialVar as AngleDataContainer).AngleValue = marioAngle - angleMarioToV3;
                        goto case "CheckTriangleExistsAngle";
                    case "AngleV3ToMario":
                        (specialVar as AngleDataContainer).AngleValue = angleV3ToMario;
                        goto case "CheckTriangleExistsAngle";

                    case "AngleV1ToV2":
                        (specialVar as AngleDataContainer).AngleValue = MoreMath.AngleTo_AngleUnits(v1X, v1Z, v2X, v2Z);
                        goto case "CheckTriangleExistsAngle";
                    case "AngleV2ToV1":
                        (specialVar as AngleDataContainer).AngleValue = MoreMath.AngleTo_AngleUnits(v2X, v2Z, v1X, v1Z);
                        goto case "CheckTriangleExistsAngle";
                    case "AngleV2ToV3":
                        (specialVar as AngleDataContainer).AngleValue = MoreMath.AngleTo_AngleUnits(v2X, v2Z, v3X, v3Z);
                        goto case "CheckTriangleExistsAngle";
                    case "AngleV3ToV2":
                        (specialVar as AngleDataContainer).AngleValue = MoreMath.AngleTo_AngleUnits(v3X, v3Z, v2X, v2Z);
                        goto case "CheckTriangleExistsAngle";
                    case "AngleV1ToV3":
                        (specialVar as AngleDataContainer).AngleValue = MoreMath.AngleTo_AngleUnits(v1X, v1Z, v3X, v3Z);
                        goto case "CheckTriangleExistsAngle";
                    case "AngleV3ToV1":
                        (specialVar as AngleDataContainer).AngleValue = MoreMath.AngleTo_AngleUnits(v3X, v3Z, v1X, v1Z);
                        goto case "CheckTriangleExistsAngle";

                    case "ObjectTriCount":
                        (specialVar as DataContainer).Text =
                            (Config.Stream.GetInt32(Config.Triangle.TotalTriangleCountAddress) - Config.Stream.GetInt32(Config.Triangle.LevelTriangleCountAddress)).ToString();
                        break;

                    // Special
                    case "CheckTriangleExists":
                        if (TriangleAddress == 0x0000)
                        {
                            (specialVar as DataContainer).Text = "(none)";
                            break;
                        }
                        break;
                    case "CheckTriangleExistsAngle":
                        (specialVar as AngleDataContainer).ValueExists = (TriangleAddress != 0);
                        break;
                }
            }
        }

        private void Mode_Click(object sender, EventArgs e, TriangleMode mode)
        {
            if (!(sender as RadioButton).Checked)
                return;

            Mode = mode;
        }

        private void AddressBox_KeyDown(object sender, KeyEventArgs e)
        {
            // On "Enter" key press
            if (e.KeyData != Keys.Enter)
                return;

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
            switch (Mode)
            {
                case TriangleMode.Ceiling:
                    TriangleAddress = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.CeilingTriangleOffset);
                    break;

                case TriangleMode.Floor:
                    TriangleAddress = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
                    break;

                case TriangleMode.Wall:
                    TriangleAddress = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.WallTriangleOffset);
                    break;
            }

            if (_checkBoxNeutralizeTriangle.Checked && TriangleAddress != 0)
            {
                ButtonUtilities.NeutralizeTriangle(TriangleAddress);
            }

            if (_recordTriangleDataCheckbox.Checked && TriangleAddress != 0)
            {
                short[] coordinates = GetTriangleCoordinates();
                bool hasAlready = _triangleData.Any(coords => Enumerable.SequenceEqual(coords, coordinates));
                if (!hasAlready) _triangleData.Add(coordinates);
            }

            if (!updateView) return;

            _recordTriangleCountLabel.Text = _triangleData.Count.ToString();

            base.Update(updateView);
            ProcessSpecialVars();
        }
    }
}
