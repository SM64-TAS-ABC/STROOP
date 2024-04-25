using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using STROOP.Forms;
using STROOP.Map;
using System.Windows.Input;

namespace STROOP.Managers
{
    public class TriangleManager : DataManager
    {
        BetterTextbox _addressBox;
        CheckBox _useMisalignmentOffsetCheckbox;

        public enum TriangleMode { Floor, Wall, Ceiling, Custom, MapHover, MapAccum };
        public TriangleMode Mode = TriangleMode.Floor;

        private readonly RadioButton _radioButtonTriFloor;
        private readonly RadioButton _radioButtonTriWall;
        private readonly RadioButton _radioButtonTriCeiling;
        private readonly RadioButton _radioButtonTriCustom;
        private readonly RadioButton _radioButtonTriMapHover;
        private readonly RadioButton _radioButtonTriMapAccum;

        public HashSet<uint> AccumulatedTriangles;

        CheckBox _checkBoxNeutralizeTriangle;

        CheckBox _recordTriangleDataCheckbox;
        CheckBox _repeatFirstVertexCheckbox;
        Label _recordTriangleCountLabel;
        List<uint> _recordedTriangleAddresses;

        // the pointer to the current triangle, or null if custom is selected
        public uint? TrianglePointerAddress = null;
        // the currently selected triangles (never empty)
        public readonly List<uint> TriangleAddresses = new List<uint>();

        public void SetTriangleAddresses(uint triangleAddress)
        {
            SetTriangleAddresses(new List<uint> { triangleAddress });
        }

        public void SetTriangleAddresses(List<uint> triangleAddresses)
        {
            if (triangleAddresses.Count == 0) return;
            TriangleAddresses.Clear();
            TriangleAddresses.AddRange(triangleAddresses);
            RefreshAddressBox();
        }

        public void RefreshAddressBox()
        {
            List<string> triangleAddressStrings = TriangleAddresses.ConvertAll(
                triAddress => HexUtilities.FormatValue(triAddress, 8));
            string newText = string.Join(",", triangleAddressStrings);
            _addressBox.SubmitTextLoosely(newText);
        }

        public void SetCustomTriangleAddresses(uint triangleAddress)
        {
            SetCustomTriangleAddresses(new List<uint> { triangleAddress });
        }

        public void SetCustomTriangleAddresses(List<uint> triangleAddresses)
        {
            if (triangleAddresses.Count == 0) return;
            _radioButtonTriCustom.Checked = true;
            Mode = TriangleMode.Custom;
            SetTriangleAddresses(triangleAddresses);
        }

        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
                VariableGroup.Self,
                VariableGroup.ExtendedLevelBoundaries,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
            };

        public TriangleManager(Control tabControl, string varFilePath, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
            _recordedTriangleAddresses = new List<uint>();

            SplitContainer splitContainerTriangles = tabControl.Controls["splitContainerTriangles"] as SplitContainer;

            _addressBox = splitContainerTriangles.Panel1.Controls["textBoxCustomTriangle"] as BetterTextbox;
            _useMisalignmentOffsetCheckbox = splitContainerTriangles.Panel1.Controls["checkBoxVertexMisalignment"] as CheckBox;

            _addressBox.AddEnterAction(() => AddressBoxEnter());

            _radioButtonTriFloor = splitContainerTriangles.Panel1.Controls["radioButtonTriFloor"] as RadioButton;
            _radioButtonTriFloor.Click += (sender, e) => Mode_Click(sender, e, TriangleMode.Floor);
            _radioButtonTriWall = splitContainerTriangles.Panel1.Controls["radioButtonTriWall"] as RadioButton;
            _radioButtonTriWall.Click += (sender, e) => Mode_Click(sender, e, TriangleMode.Wall);
            _radioButtonTriCeiling = splitContainerTriangles.Panel1.Controls["radioButtonTriCeiling"] as RadioButton;
            _radioButtonTriCeiling.Click += (sender, e) => Mode_Click(sender, e, TriangleMode.Ceiling);
            _radioButtonTriCustom = splitContainerTriangles.Panel1.Controls["radioButtonTriCustom"] as RadioButton;
            _radioButtonTriCustom.Click += (sender, e) => Mode_Click(sender, e, TriangleMode.Custom);
            _radioButtonTriMapHover = splitContainerTriangles.Panel1.Controls["radioButtonTriMapHover"] as RadioButton;
            _radioButtonTriMapHover.Click += (sender, e) => Mode_Click(sender, e, TriangleMode.MapHover);
            _radioButtonTriMapAccum = splitContainerTriangles.Panel1.Controls["radioButtonTriMapAccum"] as RadioButton;
            _radioButtonTriMapAccum.Click += (sender, e) => Mode_Click(sender, e, TriangleMode.MapAccum);

            AccumulatedTriangles = new HashSet<uint>();

            ControlUtilities.AddContextMenuStripFunctions(
                _radioButtonTriCustom,
                new List<string>()
                {
                    "Paste Addresses",
                },
                new List<Action>()
                {
                    () => EnterCustomText(Clipboard.GetText()),
                });

            Label labelTriangleSelection = splitContainerTriangles.Panel1.Controls["labelTriangleSelection"] as Label;
            ControlUtilities.AddContextMenuStripFunctions(
                labelTriangleSelection,
                new List<string>()
                {
                    "Update Based on Coordinates",
                    "Paste Triangles",
                },
                new List<Action>()
                {
                    () => UpdateBasedOnCoordinates(),
                    () => PasteTriangles(),
                });

            (splitContainerTriangles.Panel1.Controls["buttonGotoV1"] as Button).Click
                += (sender, e) => ButtonUtilities.GotoTriangleVertex(
                    TriangleAddresses[0], 1, _useMisalignmentOffsetCheckbox.Checked ? TriangleVertexOffset.O_5 : TriangleVertexOffset.NONE);
            (splitContainerTriangles.Panel1.Controls["buttonGotoV2"] as Button).Click
                += (sender, e) => ButtonUtilities.GotoTriangleVertex(
                    TriangleAddresses[0], 2, _useMisalignmentOffsetCheckbox.Checked ? TriangleVertexOffset.O_5 : TriangleVertexOffset.NONE);
            (splitContainerTriangles.Panel1.Controls["buttonGotoV3"] as Button).Click
                += (sender, e) => ButtonUtilities.GotoTriangleVertex(
                    TriangleAddresses[0], 3, _useMisalignmentOffsetCheckbox.Checked ? TriangleVertexOffset.O_5 : TriangleVertexOffset.NONE);
            (splitContainerTriangles.Panel1.Controls["buttonGotoVClosest"] as Button).Click += (sender, e) =>
                ButtonUtilities.GotoTriangleVertexClosest(
                    TriangleAddresses[0], _useMisalignmentOffsetCheckbox.Checked ? TriangleVertexOffset.O_5 : TriangleVertexOffset.NONE);

            ControlUtilities.AddContextMenuStripFunctions(
                (splitContainerTriangles.Panel1.Controls["buttonGotoVClosest"] as Button),
                new List<string>() { "Goto Closest", "Goto Closest + 0.5", "Goto Closest + 0.999" },
                new List<Action>()
                {
                    () => ButtonUtilities.GotoTriangleVertexClosest(TriangleAddresses[0], TriangleVertexOffset.NONE),
                    () => ButtonUtilities.GotoTriangleVertexClosest(TriangleAddresses[0], TriangleVertexOffset.O_5),
                    () => ButtonUtilities.GotoTriangleVertexClosest(TriangleAddresses[0], TriangleVertexOffset.O_999),
                });

            (splitContainerTriangles.Panel1.Controls["buttonRetrieveTriangle"] as Button).Click
                += (sender, e) => ButtonUtilities.RetrieveTriangle(TriangleAddresses);

            Button buttonNeutralizeTriangle = splitContainerTriangles.Panel1.Controls["buttonNeutralizeTriangle"] as Button;
            buttonNeutralizeTriangle.Click += (sender, e) => ButtonUtilities.NeutralizeTriangle(TriangleAddresses);
            ControlUtilities.AddContextMenuStripFunctions(
                buttonNeutralizeTriangle,
                new List<string>() { "Neutralize", "Neutralize with 0", "Neutralize with 0x15" },
                new List<Action>()
                {
                    () => ButtonUtilities.NeutralizeTriangle(TriangleAddresses),
                    () => ButtonUtilities.NeutralizeTriangle(TriangleAddresses, false),
                    () => ButtonUtilities.NeutralizeTriangle(TriangleAddresses, true),
                });

            Button buttonAnnihilateTriangle = splitContainerTriangles.Panel1.Controls["buttonAnnihilateTriangle"] as Button;
            buttonAnnihilateTriangle.Click += (sender, e) => ButtonUtilities.AnnihilateTriangle(TriangleAddresses);
            ControlUtilities.AddContextMenuStripFunctions(
                buttonAnnihilateTriangle,
                new List<string>()
                {
                    "Annihilate All Tri But Death Barriers",
                    "Annihilate All Ceilings",
                },
                new List<Action>()
                {
                    () => TriangleUtilities.AnnihilateAllTrianglesButDeathBarriers(),
                    () => TriangleUtilities.AnnihilateAllCeilings(),
                });

            var trianglePosGroupBox = splitContainerTriangles.Panel1.Controls["groupBoxTrianglePos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
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
                        TriangleAddresses,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative,
                        KeyboardUtilities.IsCtrlHeld());
                });

            var triangleNormalGroupBox = splitContainerTriangles.Panel1.Controls["groupBoxTriangleNormal"] as GroupBox;
            ControlUtilities.InitializeScalarController(
                triangleNormalGroupBox.Controls["buttontriangleNormalN"] as Button,
                triangleNormalGroupBox.Controls["buttontriangleNormalP"] as Button,
                triangleNormalGroupBox.Controls["textBoxTriangleNormal"] as TextBox,
                (float normalValue) =>
                {
                    ButtonUtilities.MoveTriangleNormal(TriangleAddresses, normalValue);
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
            (splitContainerTriangles.Panel1.Controls["buttonTriangleShowAddresses"] as Button).Click
                += (sender, e) => ShowTriangleAddresses();
            (splitContainerTriangles.Panel1.Controls["buttonTriangleClearData"] as Button).Click
                += (sender, e) => ClearTriangleData();

            _repeatFirstVertexCheckbox = splitContainerTriangles.Panel1.Controls["checkBoxRepeatFirstVertex"] as CheckBox;

            void CopyTriCoordinates(List<TriangleDataModel> tris)
            {
                List<string> lines = tris.ConvertAll(tri => string.Join(", ", tri.GetCoordinates()));
                string text = string.Join(",\r\n", lines);
                Clipboard.SetText(text);
            }

            Button buttonTrianglesShowLevelTris = splitContainerTriangles.Panel1.Controls["buttonTriangleShowLevelTris"] as Button;
            buttonTrianglesShowLevelTris.Click += (sender, e) => TriangleUtilities.ShowTriangles(TriangleUtilities.GetLevelTriangles());
            ControlUtilities.AddContextMenuStripFunctions(
                buttonTrianglesShowLevelTris,
                new List<string>()
                {
                    "Copy Coordinates for Level Tris",
                    "Copy Coordinates for Level Floors",
                    "Copy Coordinates for Level Walls",
                    "Copy Coordinates for Level Ceilings",
                },
                new List<Action>()
                {
                    () => CopyTriCoordinates(TriangleUtilities.GetLevelTriangles()),
                    () => CopyTriCoordinates(TriangleUtilities.GetLevelTriangles().FindAll(tri => tri.IsFloor())),
                    () => CopyTriCoordinates(TriangleUtilities.GetLevelTriangles().FindAll(tri => tri.IsWall())),
                    () => CopyTriCoordinates(TriangleUtilities.GetLevelTriangles().FindAll(tri => tri.IsCeiling())),
                });

            Button buttonTriangleShowObjTris = splitContainerTriangles.Panel1.Controls["buttonTriangleShowObjTris"] as Button;
            buttonTriangleShowObjTris.Click += (sender, e) => TriangleUtilities.ShowTriangles(TriangleUtilities.GetObjectTriangles());
            ControlUtilities.AddContextMenuStripFunctions(
                buttonTriangleShowObjTris,
                new List<string>()
                {
                    "Show All Object Tris",
                    "Show Selected Object Tris",
                    "Copy Coordinates for All Object Tris",
                    "Copy Coordinates for All Object Floors",
                    "Copy Coordinates for All Object Walls",
                    "Copy Coordinates for All Object Ceilings",
                    "Copy Coordinates for Selected Object Tris",
                    "Copy Coordinates for Selected Object Floors",
                    "Copy Coordinates for Selected Object Walls",
                    "Copy Coordinates for Selected Object Ceilings",
                },
                new List<Action>()
                {
                    () => TriangleUtilities.ShowTriangles(TriangleUtilities.GetObjectTriangles()),
                    () => TriangleUtilities.ShowTriangles(TriangleUtilities.GetSelectedObjectTriangles()),
                    () => CopyTriCoordinates(TriangleUtilities.GetObjectTriangles()),
                    () => CopyTriCoordinates(TriangleUtilities.GetObjectTriangles().FindAll(tri => tri.IsFloor())),
                    () => CopyTriCoordinates(TriangleUtilities.GetObjectTriangles().FindAll(tri => tri.IsWall())),
                    () => CopyTriCoordinates(TriangleUtilities.GetObjectTriangles().FindAll(tri => tri.IsCeiling())),
                    () => CopyTriCoordinates(TriangleUtilities.GetSelectedObjectTriangles()),
                    () => CopyTriCoordinates(TriangleUtilities.GetSelectedObjectTriangles().FindAll(tri => tri.IsFloor())),
                    () => CopyTriCoordinates(TriangleUtilities.GetSelectedObjectTriangles().FindAll(tri => tri.IsWall())),
                    () => CopyTriCoordinates(TriangleUtilities.GetSelectedObjectTriangles().FindAll(tri => tri.IsCeiling())),
                });

            Button buttonTrianglesShowAllTris = splitContainerTriangles.Panel1.Controls["buttonTriangleShowAllTris"] as Button;
            buttonTrianglesShowAllTris.Click += (sender, e) => TriangleUtilities.ShowTriangles(TriangleUtilities.GetAllTriangles());
            ControlUtilities.AddContextMenuStripFunctions(
                buttonTrianglesShowAllTris,
                new List<string>()
                {
                    "Copy Coordinates for All Tris",
                    "Copy Coordinates for All Floors",
                    "Copy Coordinates for All Walls",
                    "Copy Coordinates for All Ceilings",
                },
                new List<Action>()
                {
                    () => CopyTriCoordinates(TriangleUtilities.GetAllTriangles()),
                    () => CopyTriCoordinates(TriangleUtilities.GetAllTriangles().FindAll(tri => tri.IsFloor())),
                    () => CopyTriCoordinates(TriangleUtilities.GetAllTriangles().FindAll(tri => tri.IsWall())),
                    () => CopyTriCoordinates(TriangleUtilities.GetAllTriangles().FindAll(tri => tri.IsCeiling())),
                });

            var buttonTriangleNeutralizeAllTriangles = splitContainerTriangles.Panel1.Controls["buttonTriangleNeutralizeAllTriangles"] as Button;
            buttonTriangleNeutralizeAllTriangles.Click += (sender, e) => TriangleUtilities.NeutralizeTriangles();

            ControlUtilities.AddContextMenuStripFunctions(
                buttonTriangleNeutralizeAllTriangles,
                new List<string>() {
                    "Neutralize All Triangles",
                    "Neutralize Wall Triangles",
                    "Neutralize Floor Triangles",
                    "Neutralize Ceiling Triangles",
                    "Neutralize Death Barriers",
                    "Neutralize Lava",
                    "Neutralize Sleeping",
                    "Neutralize Loading Zones"
                },
                new List<Action>() {
                    () => TriangleUtilities.NeutralizeTriangles(),
                    () => TriangleUtilities.NeutralizeTriangles(TriangleClassification.Wall),
                    () => TriangleUtilities.NeutralizeTriangles(TriangleClassification.Floor),
                    () => TriangleUtilities.NeutralizeTriangles(TriangleClassification.Ceiling),
                    () => TriangleUtilities.NeutralizeTriangles(0x0A),
                    () => TriangleUtilities.NeutralizeTriangles(0x01),
                    () => TriangleUtilities.NeutralizeSleeping(),
                    () => {
                        TriangleUtilities.NeutralizeTriangles(0x1B);
                        TriangleUtilities.NeutralizeTriangles(0x1C);
                        TriangleUtilities.NeutralizeTriangles(0x1D);
                        TriangleUtilities.NeutralizeTriangles(0x1E);
                    },
                });

            var buttonTriangleDisableAllCamCollision = splitContainerTriangles.Panel1.Controls["buttonTriangleDisableAllCamCollision"] as Button;
            buttonTriangleDisableAllCamCollision.Click += (sender, e) => TriangleUtilities.DisableCamCollision();

            ControlUtilities.AddContextMenuStripFunctions(
                buttonTriangleDisableAllCamCollision,
                new List<string>()
                {
                    "Disable Cam Collision for All Triangles",
                    "Disable Cam Collision for Wall Triangles",
                    "Disable Cam Collision for Floor Triangles",
                    "Disable Cam Collision for Ceiling Triangles",
                },
                new List<Action>()
                {
                    () => TriangleUtilities.DisableCamCollision(),
                    () => TriangleUtilities.DisableCamCollision(TriangleClassification.Wall),
                    () => TriangleUtilities.DisableCamCollision(TriangleClassification.Floor),
                    () => TriangleUtilities.DisableCamCollision(TriangleClassification.Ceiling),
                });

            GroupBox groupBoxTriangleTypeConversion = splitContainerTriangles.Panel1.Controls["groupBoxTriangleTypeConversion"] as GroupBox;
            ComboBox comboBoxTriangleTypeConversionConvert = groupBoxTriangleTypeConversion.Controls["comboBoxTriangleTypeConversionConvert"] as ComboBox;
            TextBox textBoxTriangleTypeConversionFromType = groupBoxTriangleTypeConversion.Controls["textBoxTriangleTypeConversionFromType"] as TextBox;
            TextBox textBoxTriangleTypeConversionToType = groupBoxTriangleTypeConversion.Controls["textBoxTriangleTypeConversionToType"] as TextBox;
            Button buttonTriangleTypeConversionConvert = groupBoxTriangleTypeConversion.Controls["buttonTriangleTypeConversionConvert"] as Button;

            comboBoxTriangleTypeConversionConvert.DataSource = EnumUtilities.GetEnumValues<TriangleClassificationExtended>(typeof(TriangleClassificationExtended));

            buttonTriangleTypeConversionConvert.Click += (sender, e) =>
            {
                TriangleClassificationExtended classification = (TriangleClassificationExtended)comboBoxTriangleTypeConversionConvert.SelectedItem;
                short? fromType = (short?)ParsingUtilities.ParseHexNullable(textBoxTriangleTypeConversionFromType.Text);
                short? toType = (short?)ParsingUtilities.ParseHexNullable(textBoxTriangleTypeConversionToType.Text);
                if (!fromType.HasValue || !toType.HasValue) return;
                TriangleUtilities.ConvertSurfaceTypes(classification, fromType.Value, toType.Value);
            };
        }

        public void GoToClosestFloorVertex()
        {
            uint floorTri = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            if (floorTri == 0) return;
            ButtonUtilities.GotoTriangleVertexClosest(floorTri, TriangleVertexOffset.NONE);
        }

        private void UpdateBasedOnCoordinates()
        {
            foreach (uint triangleAddress in TriangleAddresses)
            {
                TriangleDataModel tri = TriangleDataModel.CreateLazy(triangleAddress);
                UpdateBasedOnCoordinates(triangleAddress, tri.X1, tri.Y1, tri.Z1, tri.X2, tri.Y2, tri.Z2, tri.X3, tri.Y3, tri.Z3);
            }
        }

        private void UpdateBasedOnCoordinates(
            uint triAddress, int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3)
        {
            if (triAddress == 0) return;

            // update norms
            (float normX, float normY, float normZ, float normOffset) =
                TriangleUtilities.GetNorms(x1, y1, z1, x2, y2, z2, x3, y3, z3);
            Config.Stream.SetValue(normX, triAddress + TriangleOffsetsConfig.NormX);
            Config.Stream.SetValue(normY, triAddress + TriangleOffsetsConfig.NormY);
            Config.Stream.SetValue(normZ, triAddress + TriangleOffsetsConfig.NormZ);
            TriangleOffsetsConfig.SetNormalOffset(normOffset, triAddress);

            // update y bounds
            short yMinMinus5 = (short)(MoreMath.Min(y1, y2, y3) - 5);
            short yMaxPlus5 = (short)(MoreMath.Max(y1, y2, y3) + 5);
            Config.Stream.SetValue(yMinMinus5, triAddress + TriangleOffsetsConfig.YMinMinus5);
            Config.Stream.SetValue(yMaxPlus5, triAddress + TriangleOffsetsConfig.YMaxPlus5);

            // update x projection
            bool xProjection = normX < -0.707 || normX > 0.707;
            Config.Stream.SetValue(
                (byte)(xProjection ? TriangleOffsetsConfig.XProjectionMask : 0),
                triAddress + TriangleOffsetsConfig.Flags,
                mask: TriangleOffsetsConfig.XProjectionMask);
        }

        private void PasteTriangles()
        {
            List<List<string>> lines = ParsingUtilities.ParseLines(Clipboard.GetText());
            if (lines.Count != 10) return;
            int numWords = lines[0].Count;
            if (numWords == 0) return;
            if (lines.Any(line => line.Count != numWords)) return;

            for (int wordIndex = 0; wordIndex < numWords; wordIndex++)
            {
                uint triAddress = ParsingUtilities.ParseHexNullable(lines[0][wordIndex]) ?? 0;
                List<int> coords = lines.Skip(1).ToList().ConvertAll(line => ParsingUtilities.ParseInt(line[wordIndex]));
                TriangleOffsetsConfig.SetX1((short)coords[0], triAddress);
                TriangleOffsetsConfig.SetY1((short)coords[1], triAddress);
                TriangleOffsetsConfig.SetZ1((short)coords[2], triAddress);
                TriangleOffsetsConfig.SetX2((short)coords[3], triAddress);
                TriangleOffsetsConfig.SetY2((short)coords[4], triAddress);
                TriangleOffsetsConfig.SetZ2((short)coords[5], triAddress);
                TriangleOffsetsConfig.SetX3((short)coords[6], triAddress);
                TriangleOffsetsConfig.SetY3((short)coords[7], triAddress);
                TriangleOffsetsConfig.SetZ3((short)coords[8], triAddress);
                UpdateBasedOnCoordinates(
                    triAddress,
                    coords[0], coords[1], coords[2],
                    coords[3], coords[4], coords[5],
                    coords[6], coords[7], coords[8]);
            }
        }

        private short[] GetTriangleCoordinates(uint? nullableTriAddress = null)
        {
            uint triAddress = nullableTriAddress ?? TriangleAddresses[0];
            short[] coordinates = new short[9];
            coordinates[0] = TriangleOffsetsConfig.GetX1(triAddress);
            coordinates[1] = TriangleOffsetsConfig.GetY1(triAddress);
            coordinates[2] = TriangleOffsetsConfig.GetZ1(triAddress);
            coordinates[3] = TriangleOffsetsConfig.GetX2(triAddress);
            coordinates[4] = TriangleOffsetsConfig.GetY2(triAddress);
            coordinates[5] = TriangleOffsetsConfig.GetZ2(triAddress);
            coordinates[6] = TriangleOffsetsConfig.GetX3(triAddress);
            coordinates[7] = TriangleOffsetsConfig.GetY3(triAddress);
            coordinates[8] = TriangleOffsetsConfig.GetZ3(triAddress);
            return coordinates;
        }

        private void ShowTriangleCoordinates()
        {
            if (TriangleAddresses.Count == 1 && TriangleAddresses[0] == 0) return;
            InfoForm infoForm = new InfoForm();
            infoForm.SetTriangleCoordinates(GetTriangleCoordinates());
            infoForm.Show();
        }

        private void ShowTriangleEquation()
        {
            if (TriangleAddresses.Count == 1 && TriangleAddresses[0] == 0) return;
            uint triangleAddress = TriangleAddresses[0];

            float normX, normY, normZ, normOffset;
            normX = Config.Stream.GetFloat(triangleAddress + TriangleOffsetsConfig.NormX);
            normY = Config.Stream.GetFloat(triangleAddress + TriangleOffsetsConfig.NormY);
            normZ = Config.Stream.GetFloat(triangleAddress + TriangleOffsetsConfig.NormZ);
            normOffset = TriangleOffsetsConfig.GetNormalOffset(triangleAddress);

            InfoForm infoForm = new InfoForm();
            infoForm.SetTriangleEquation(normX, normY, normZ, normOffset);
            infoForm.Show();
        }

        private void ShowTriangleData()
        {
            InfoForm infoForm = new InfoForm();
            List<short[]> triangleVertices = _recordedTriangleAddresses.ConvertAll(
                triAddress => GetTriangleCoordinates(triAddress));
            infoForm.SetTriangleData(triangleVertices, _repeatFirstVertexCheckbox.Checked);
            infoForm.Show();
        }

        private void ShowTriangleVertices()
        {
            InfoForm infoForm = new InfoForm();
            List<short[]> triangleVertices = _recordedTriangleAddresses.ConvertAll(
                triAddress => GetTriangleCoordinates(triAddress));
            infoForm.SetTriangleVertices(triangleVertices);
            infoForm.Show();
        }

        private void ShowTriangleAddresses()
        {
            InfoForm infoForm = new InfoForm();
            List<string> addressStrings = _recordedTriangleAddresses.ConvertAll(
                triAddress => HexUtilities.FormatValue(triAddress));
            infoForm.SetText(
                "Triangle Info",
                "Triangle Addresses",
                string.Join("\r\n", addressStrings));
            infoForm.Show();
        }

        private void ClearTriangleData()
        {
            _recordedTriangleAddresses.Clear();
        }

        private void Mode_Click(object sender, EventArgs e, TriangleMode mode)
        {
            if (!(sender as RadioButton).Checked)
                return;

            Mode = mode;
        }

        private void AddressBoxEnter()
        {
            EnterCustomText(_addressBox.Text);
        }

        private void EnterCustomText(string text)
        {
            List<uint> triangleAddresses = ParsingUtilities.ParseHexListNullable(text);
            if (triangleAddresses.Count > 0)
            {
                SetCustomTriangleAddresses(triangleAddresses);
            }
            else
            {
                RefreshAddressBox();
            }
            _addressBox.SelectionLength = 0;
        }

        public override void Update(bool updateView)
        {
            switch (Mode)
            {
                case TriangleMode.Floor:
                    TrianglePointerAddress = MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset;
                    SetTriangleAddresses(Config.Stream.GetUInt(TrianglePointerAddress.Value));
                    break;

                case TriangleMode.Wall:
                    TrianglePointerAddress = MarioConfig.StructAddress + MarioConfig.WallTriangleOffset;
                    SetTriangleAddresses(Config.Stream.GetUInt(TrianglePointerAddress.Value));
                    break;

                case TriangleMode.Ceiling:
                    TrianglePointerAddress = MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset;
                    SetTriangleAddresses(Config.Stream.GetUInt(TrianglePointerAddress.Value));
                    break;

                case TriangleMode.MapHover:
                    TrianglePointerAddress = null;
                    SetTriangleAddresses(MapObjectHoverData.LastTriangleAddress);
                    break;

                case TriangleMode.MapAccum:
                    TrianglePointerAddress = null;
                    if (AccumulatedTriangles.Count == 0)
                    {
                        SetTriangleAddresses(0);
                    }
                    else
                    {
                        SetTriangleAddresses(AccumulatedTriangles.ToList());
                    }
                    break;

                default:
                    TrianglePointerAddress = null;
                    break;
            }

            if (Mode != TriangleMode.MapAccum || Keyboard.IsKeyDown(Key.Escape))
            {
                AccumulatedTriangles.Clear();
            }

            if (_checkBoxNeutralizeTriangle.Checked)
            {
                ButtonUtilities.NeutralizeTriangle(TriangleAddresses);
            }

            if (_recordTriangleDataCheckbox.Checked)
            {
                foreach (uint triangleAddress in TriangleAddresses)
                {
                    bool hasAlready = _recordedTriangleAddresses.Any(recordedAddress => triangleAddress == recordedAddress);
                    if (!hasAlready) _recordedTriangleAddresses.Add(triangleAddress);
                }
            }

            if (!updateView) return;

            _recordTriangleCountLabel.Text = _recordedTriangleAddresses.Count.ToString();

            base.Update(updateView);
        }
    }
}
