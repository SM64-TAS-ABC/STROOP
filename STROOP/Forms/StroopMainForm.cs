using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Managers;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using STROOP.Controls;
using STROOP.Forms;
using STROOP.Models;
using STROOP.Structs.Gui;
using STROOP.Map;
using System.IO;

namespace STROOP
{
    public partial class StroopMainForm : Form
    {
        const string _version = "v1.0.2";
        
        List<InputImageGui> _inputImageGuiList = new List<Structs.InputImageGui>();
        FileImageGui _fileImageGui = new FileImageGui();
        ScriptParser _scriptParser;
        List<RomHack> _romHacks;

        DataTable _tableOtherData = new DataTable();
        Dictionary<int, DataRow> _otherDataRowAssoc = new Dictionary<int, DataRow>();

        bool _resizing = true, _objSlotResizing = false;
        int _resizeTimeLeft = 0, _resizeObjSlotTime = 0;

        public StroopMainForm()
        {
            InitializeComponent();
        }

        private bool AttachToProcess(Process process)
        {
            // Find emulator
            var emulators = Config.Emulators.Where(e => e.ProcessName.ToLower() == process.ProcessName.ToLower()).ToList();

            if (emulators.Count > 1)
            {
                MessageBox.Show("Ambiguous emulator type", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return Config.Stream.SwitchProcess(process, emulators[0]);
        }

        private void StroopMainForm_Load(object sender, EventArgs e)
        {
            Config.Stream = new ProcessStream();
            Config.Stream.OnUpdate += OnUpdate;
            Config.Stream.FpsUpdated += _sm64Stream_FpsUpdated;
            Config.Stream.OnDisconnect += _sm64Stream_OnDisconnect;
            Config.Stream.WarnReadonlyOff += _sm64Stream_WarnReadonlyOff;

            comboBoxRomVersion.DataSource = Enum.GetValues(typeof(RomVersionSelection));
            comboBoxReadWriteMode.DataSource = Enum.GetValues(typeof(ReadWriteMode));

            SetUpContextMenuStrips();

            Config.StroopMainForm = this;
            Config.TabControlMain = tabControlMain;
            Config.DebugText = labelDebugText;

            SavedSettingsConfig.StoreRecommendedTabOrder();
            SavedSettingsConfig.InvokeInitiallySavedTabOrder();
            Config.TabControlMain.SelectedIndex = 0;
            InitializeTabRemoval();
            SavedSettingsConfig.InvokeInitiallySavedRemovedTabs();

            SetupViews();

            _resizing = false;
            labelVersionNumber.Text = _version;

            // Collect garbage, we are fully loaded now!
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Load process
            buttonRefresh_Click(this, new EventArgs());
            panelConnect.Location = new Point();
            panelConnect.Size = this.Size;
        }

        private void InitializeTabRemoval()
        {
            tabControlMain.Click += (se, ev) =>
            {
                if (KeyboardUtilities.IsCtrlHeld())
                {
                    SavedSettingsConfig.RemoveTab(tabControlMain.SelectedTab);
                }
            };

            buttonTabAdd.ContextMenuStrip = new ContextMenuStrip();
            Action openingFunction = () =>
            {
                buttonTabAdd.ContextMenuStrip.Items.Clear();
                SavedSettingsConfig.GetRemovedTabItems().ForEach(
                    item => buttonTabAdd.ContextMenuStrip.Items.Add(item));
            };
            buttonTabAdd.ContextMenuStrip.Opening += (se, ev) => openingFunction();
            openingFunction();
        }

        private void SetUpContextMenuStrips()
        {
            ControlUtilities.AddContextMenuStripFunctions(
                labelVersionNumber,
                new List<string>()
                {
                    "Open Mapping",
                    "Clear Mapping",
                    "Inject Hitbox View Code",
                    "Free Movement Action",
                    "Everything in File",
                    "Go to Closest Floor Vertex",
                    "Save as Savestate",
                    "Show MHS Vars",
                    "Download Latest STROOP Release",
                    "Documentation",
                    "Show All Helpful Hints",
                    "Enable TASer Settings",
                    "Add Gfx Vertices",
                    "Show Skribblio Words",
                    "Show Image Form",
                    "Show Coin Ring Display Form",
                    "Add Chuckya Map Objs",
                    "Test Something",
                    "Test Something Else",
                    "Format Subtitles",
                },
                new List<Action>()
                {
                    () => MappingConfig.OpenMapping(),
                    () => MappingConfig.ClearMapping(),
                    () => Config.GfxManager.InjectHitboxViewCode(),
                    () => Config.Stream.SetValue(MarioConfig.FreeMovementAction, MarioConfig.StructAddress + MarioConfig.ActionOffset),
                    () => Config.FileManager.DoEverything(),
                    () => Config.TriangleManager.GoToClosestVertex(),
                    () => saveAsSavestate(),
                    () =>
                    {
                        string varFilePath = @"Config/MhsData.xml";
                        List<WatchVariableControlPrecursor> precursors =
                            XmlConfigParser.OpenWatchVariableControlPrecursors(varFilePath);
                        List<WatchVariableControl> controls = precursors.ConvertAll(
                            precursor => precursor.CreateWatchVariableControl());
                        VariablePopOutForm form = new VariablePopOutForm();
                        form.Initialize(controls);
                        form.ShowForm();
                    },
                    () => Process.Start("https://github.com/SM64-TAS-ABC/STROOP/releases/download/vDev/STROOP.zip"),
                    () => Process.Start("https://ukikipedia.net/wiki/STROOP"),
                    () => HelpfulHintUtilities.ShowAllHelpfulHints(),
                    () =>
                    {
                        SavedSettingsConfig.UseExpandedRamSize = true;
                        splitContainerTas.Panel1Collapsed = true;
                        splitContainerTas.Panel2Collapsed = false;
                        Config.TasManager.ShowTaserVariables();
                        tabControlMain.SelectedTab = tabPageTas;
                    },
                    () => TestUtilities.AddGraphicsTriangleVerticesToTriangleTab(),
                    () => SkribblioUtilities.ShowWords(),
                    () =>
                    {
                        ImageForm imageForm = new ImageForm();
                        imageForm.Show();
                    },
                    () =>
                    {
                        CoinRingDisplayForm form = new CoinRingDisplayForm();
                        form.Show();
                    },
                    () => TestUtilities.AddChuckyaMapObjects(),
                    () => TestUtilities.TestSomething(),
                    () => TestUtilities.TestSomethingElse(),
                    () => SubtitleUtilities.FormatSubtitlesFromClipboard(),
                });

            ControlUtilities.AddCheckableContextMenuStripFunctions(
                labelVersionNumber,
                new List<string>()
                {
                    "Update Cam Hack Angle",
                    "Update Floor Tri",
                },
                new List<Func<bool>>()
                {
                    () =>
                    {
                        TestingConfig.UpdateCamHackAngle = !TestingConfig.UpdateCamHackAngle;
                        return TestingConfig.UpdateCamHackAngle;
                    },
                    () =>
                    {
                        TestingConfig.UpdateFloorTri = !TestingConfig.UpdateFloorTri;
                        return TestingConfig.UpdateFloorTri;
                    },
                });

            ControlUtilities.AddContextMenuStripFunctions(
                buttonMoveTabLeft,
                new List<string>() { "Restore Recommended Tab Order" },
                new List<Action>() { () => SavedSettingsConfig.InvokeRecommendedTabOrder() });

            ControlUtilities.AddContextMenuStripFunctions(
                buttonMoveTabRight,
                new List<string>() { "Restore Recommended Tab Order" },
                new List<Action>() { () => SavedSettingsConfig.InvokeRecommendedTabOrder() });

            ControlUtilities.AddContextMenuStripFunctions(
                trackBarObjSlotSize,
                new List<string>() { "Reset to Default Object Slot Size" },
                new List<Action>() {
                    () =>
                    {
                        trackBarObjSlotSize.Value = ObjectSlotsManager.DefaultSlotSize;
                        ChangeObjectSlotSize(ObjectSlotsManager.DefaultSlotSize);
                    }
                });
        }

        private void CreateManagers()
        {
            Config.MapGui = new MapGui()
            {
                GLControlMap2D = glControlMap2D,
                GLControlMap3D = glControlMap3D,
                flowLayoutPanelMapTrackers = flowLayoutPanelMapTrackers,

                checkBoxMapOptionsTrackMario = checkBoxMapOptionsTrackMario,
                checkBoxMapOptionsTrackHolp = checkBoxMapOptionsTrackHolp,
                checkBoxMapOptionsTrackCamera = checkBoxMapOptionsTrackCamera,
                checkBoxMapOptionsTrackGhost = checkBoxMapOptionsTrackGhost,
                checkBoxMapOptionsTrackSelf = checkBoxMapOptionsTrackSelf,
                checkBoxMapOptionsTrackPoint = checkBoxMapOptionsTrackPoint,
                checkBoxMapOptionsTrackFloorTri = checkBoxMapOptionsTrackFloorTri,
                checkBoxMapOptionsTrackWallTri = checkBoxMapOptionsTrackWallTri,
                checkBoxMapOptionsTrackCeilingTri = checkBoxMapOptionsTrackCeilingTri,
                checkBoxMapOptionsTrackUnitGridlines = checkBoxMapOptionsTrackUnitGridlines,

                checkBoxMapOptionsEnable3D = checkBoxMapOptionsEnable3D,
                checkBoxMapOptionsDisable3DHitboxHackTris = checkBoxMapOptionsDisable3DHitboxHackTris,
                checkBoxMapOptionsEnableSideView = checkBoxMapOptionsEnableSideView,
                checkBoxMapOptionsEnablePuView = checkBoxMapOptionsEnablePuView,
                checkBoxMapOptionsReverseDragging = checkBoxMapOptionsReverseDragging,
                checkBoxMapOptionsScaleIconSizes = checkBoxMapOptionsScaleIconSizes,

                labelMapOptionsGlobalIconSize = labelMapOptionsGlobalIconSize,
                textBoxMapOptionsGlobalIconSize = textBoxMapOptionsGlobalIconSize,
                trackBarMapOptionsGlobalIconSize = trackBarMapOptionsGlobalIconSize,

                buttonMapOptionsAddNewTracker = buttonMapOptionsAddNewTracker,
                buttonMapOptionsClearAllTrackers = buttonMapOptionsClearAllTrackers,

                comboBoxMapOptionsLevel = comboBoxMapOptionsLevel,
                comboBoxMapOptionsBackground = comboBoxMapOptionsBackground,

                groupBoxMapControllersScale = groupBoxMapControllersScale,
                groupBoxMapControllersCenter = groupBoxMapControllersCenter,
                groupBoxMapControllersAngle = groupBoxMapControllersAngle,

                radioButtonMapControllersScaleCourseDefault = radioButtonMapControllersScaleCourseDefault,
                radioButtonMapControllersScaleMaxCourseSize = radioButtonMapControllersScaleMaxCourseSize,
                radioButtonMapControllersScaleCustom = radioButtonMapControllersScaleCustom,
                textBoxMapControllersScaleCustom = textBoxMapControllersScaleCustom,

                textBoxMapControllersScaleChange = textBoxMapControllersScaleChange,
                buttonMapControllersScaleMinus = buttonMapControllersScaleMinus,
                buttonMapControllersScalePlus = buttonMapControllersScalePlus,
                textBoxMapControllersScaleChange2 = textBoxMapControllersScaleChange2,
                buttonMapControllersScaleDivide = buttonMapControllersScaleDivide,
                buttonMapControllersScaleTimes = buttonMapControllersScaleTimes,

                radioButtonMapControllersCenterBestFit = radioButtonMapControllersCenterBestFit,
                radioButtonMapControllersCenterOrigin = radioButtonMapControllersCenterOrigin,
                radioButtonMapControllersCenterMario = radioButtonMapControllersCenterMario,
                radioButtonMapControllersCenterCustom = radioButtonMapControllersCenterCustom,
                textBoxMapControllersCenterCustom = textBoxMapControllersCenterCustom,

                checkBoxMapControllersCenterChangeByPixels = checkBoxMapControllersCenterChangeByPixels,
                textBoxMapControllersCenterChange = textBoxMapControllersCenterChange,
                buttonMapControllersCenterUp = buttonMapControllersCenterUp,
                buttonMapControllersCenterUpRight = buttonMapControllersCenterUpRight,
                buttonMapControllersCenterRight = buttonMapControllersCenterRight,
                buttonMapControllersCenterDownRight = buttonMapControllersCenterDownRight,
                buttonMapControllersCenterDown = buttonMapControllersCenterDown,
                buttonMapControllersCenterDownLeft = buttonMapControllersCenterDownLeft,
                buttonMapControllersCenterLeft = buttonMapControllersCenterLeft,
                buttonMapControllersCenterUpLeft = buttonMapControllersCenterUpLeft,

                radioButtonMapControllersAngle0 = radioButtonMapControllersAngle0,
                radioButtonMapControllersAngle16384 = radioButtonMapControllersAngle16384,
                radioButtonMapControllersAngle32768 = radioButtonMapControllersAngle32768,
                radioButtonMapControllersAngle49152 = radioButtonMapControllersAngle49152,
                radioButtonMapControllersAngleMario = radioButtonMapControllersAngleMario,
                radioButtonMapControllersAngleCamera = radioButtonMapControllersAngleCamera,
                radioButtonMapControllersAngleCentripetal = radioButtonMapControllersAngleCentripetal,
                radioButtonMapControllersAngleCustom = radioButtonMapControllersAngleCustom,
                textBoxMapControllersAngleCustom = textBoxMapControllersAngleCustom,

                textBoxMapControllersAngleChange = textBoxMapControllersAngleChange,
                buttonMapControllersAngleCCW = buttonMapControllersAngleCCW,
                buttonMapControllersAngleCW = buttonMapControllersAngleCW,

                labelMapDataMapName = labelMapDataMapName,
                labelMapDataMapSubName = labelMapDataMapSubName,
                labelMapDataPuCoordinateValues = labelMapDataPuCoordinateValues,
                labelMapDataQpuCoordinateValues = labelMapDataQpuCoordinateValues,
                labelMapDataIdValues = labelMapDataIdValues,
                labelMapDataYNormValue = labelMapDataYNormValue,

                watchVariablePanelMap3DVars = watchVariablePanelMap3DVars,

                groupBoxMapCameraPosition = groupBoxMapCameraPosition,
                groupBoxMapFocusPosition = groupBoxMapFocusPosition,
                groupBoxMapCameraSpherical = groupBoxMapCameraSpherical,
                groupBoxMapFocusSpherical = groupBoxMapFocusSpherical,
                groupBoxMapCameraFocus = groupBoxMapCameraFocus,

                textBoxMapFov = textBoxMapFov,
                trackBarMapFov = trackBarMapFov,
            };


            M64Gui m64Gui = new M64Gui()
            {
                LabelFileName = labelM64FileName,
                LabelNumInputsValue = labelM64NumInputsValue,

                ComboBoxFrameInputRelation = comboBoxM64FrameInputRelation,
                CheckBoxMaxOutViCount = checkBoxMaxOutViCount,

                ButtonSave = buttonM64Save,
                ButtonSaveAs = buttonM64SaveAs,
                ButtonResetChanges = buttonM64ResetChanges,
                ButtonOpen = buttonM64Open,
                ButtonClose = buttonM64Close,
                ButtonGoto = buttonM64Goto,
                TextBoxGoto = textBoxM64Goto,

                DataGridViewInputs = dataGridViewM64Inputs,
                PropertyGridHeader = propertyGridM64Header,
                PropertyGridStats = propertyGridM64Stats,

                TabControlDetails = tabControlM64Details,
                TabPageInputs = tabPageM64Inputs,
                TabPageHeader = tabPageM64Header,
                TabPageStats = tabPageM64Stats,

                ProgressBar = progressBarM64,
                LabelProgressBar = labelM64ProgressBar,

                ButtonSetUsRom = buttonM64SetUsRom,
                ButtonSetJpRom = buttonM64SetJpRom,
                ButtonCopyRom = buttonM64CopyRom,
                ButtonPasteRom = buttonM64PasteRom,

                TextBoxOnValue = textBoxM64OnValue,

                TextBoxSelectionStartFrame = textBoxM64SelectionStartFrame,
                TextBoxSelectionEndFrame = textBoxM64SelectionEndFrame,
                TextBoxSelectionInputs = textBoxM64SelectionInputs,

                ButtonTurnOffRowRange = buttonM64TurnOffRowRange,
                ButtonTurnOffInputRange = buttonM64TurnOffInputRange,
                ButtonTurnOffCells = buttonM64TurnOffCells,
                ButtonDeleteRowRange = buttonM64DeleteRowRange,
                ButtonTurnOnInputRange = buttonM64TurnOnInputRange,
                ButtonTurnOnCells = buttonM64TurnOnCells,
                ButtonCopyRowRange = buttonM64CopyRowRange,
                ButtonCopyInputRange = buttonM64CopyInputRange,

                ListBoxCopied = listBoxM64Copied,
                ButtonPasteInsert = buttonM64PasteInsert,
                ButtonPasteOverwrite = buttonM64PasteOverwrite,
                TextBoxPasteMultiplicity = textBoxM64PasteMultiplicity,

                TextBoxQuickDuplication1stIterationStart = textBoxM64QuickDuplication1stIterationStart,
                TextBoxQuickDuplication2ndIterationStart = textBoxM64QuickDuplication2ndIterationStart,
                TextBoxQuickDuplicationTotalIterations = textBoxM64QuickDuplicationTotalIterations,
                ButtonQuickDuplicationDuplicate = buttonM64QuickDuplicationDuplicate,

                ButtonAddPauseBufferFrames = buttonM64AddPauseBufferFrames,
            };

            // Create managers
            Config.MapManager = new MapManager(@"Config/Map3DVars.xml");

            Config.ModelManager = new ModelManager(tabPageModel);
            Config.ActionsManager = new ActionsManager(@"Config/ActionsData.xml", watchVariablePanelActions, tabPageActions);
            Config.WaterManager = new WaterManager(@"Config/WaterData.xml", watchVariablePanelWater);
            Config.SnowManager = new SnowManager(@"Config/SnowData.xml", watchVariablePanelSnow, tabPageSnow);
            Config.InputManager = new InputManager(@"Config/InputData.xml", tabPageInput, watchVariablePanelInput, _inputImageGuiList);
            Config.MarioManager = new MarioManager(@"Config/MarioData.xml", tabPageMario, WatchVariablePanelMario);
            Config.HudManager = new HudManager(@"Config/HudData.xml", tabPageHud, watchVariablePanelHud);
            Config.MiscManager = new MiscManager(@"Config/MiscData.xml", watchVariablePanelMisc, tabPageMisc);
            Config.CameraManager = new CameraManager(@"Config/CameraData.xml", tabPageCamera, watchVariablePanelCamera);
            Config.TriangleManager = new TriangleManager(tabPageTriangles, @"Config/TrianglesData.xml", watchVariablePanelTriangles);
            Config.DebugManager = new DebugManager(@"Config/DebugData.xml", tabPageDebug, watchVariablePanelDebug);
            Config.PuManager = new PuManager(@"Config/PuData.xml", tabPagePu, watchVariablePanelPu);
            Config.TasManager = new TasManager(@"Config/TasData.xml", tabPageTas, watchVariablePanelTas);
            Config.FileManager = new FileManager(@"Config/FileData.xml", tabPageFile, watchVariablePanelFile, _fileImageGui);
            Config.MainSaveManager = new MainSaveManager(@"Config/MainSaveData.xml", tabPageMainSave, watchVariablePanelMainSave);
            Config.AreaManager = new AreaManager(tabPageArea, @"Config/AreaData.xml", watchVariablePanelArea);
            Config.QuarterFrameManager = new QuarterFrameManager(@"Config/QuarterFrameData.xml", watchVariablePanelQuarterFrame);
            Config.CustomManager = new CustomManager(@"Config/CustomData.xml", tabPageCustom, watchVariablePanelCustom);
            Config.VarHackManager = new VarHackManager(tabPageVarHack, varHackPanel);
            Config.CamHackManager = new CamHackManager(@"Config/CamHackData.xml", tabPageCamHack, watchVariablePanelCamHack);
            Config.ObjectManager = new ObjectManager(@"Config/ObjectData.xml", tabPageObject, WatchVariablePanelObject);
            Config.OptionsManager = new OptionsManager(@"Config/OptionsData.xml", tabPageOptions, watchVariablePanelOptions, pictureBoxCog);
            Config.TestingManager = new TestingManager(tabPageTesting);
            Config.MemoryManager = new MemoryManager(tabPageMemory, watchVariablePanelMemory, @"Config/ObjectData.xml");
            Config.SearchManager = new SearchManager(tabPageSearch, watchVariablePanelSearch);
            Config.CellsManager = new CellsManager(@"Config/CellsData.xml", tabPageCells, watchVariablePanelCells);
            Config.CoinManager = new CoinManager(tabPageCoin);
            Config.GfxManager = new GfxManager(tabPageGfx, watchVariablePanelGfx);
            Config.PaintingManager = new PaintingManager(@"Config/PaintingData.xml", watchVariablePanelPainting, tabPagePainting);
            Config.MusicManager = new MusicManager(@"Config/MusicData2.xml", watchVariablePanelMusic, tabPageMusic);
            Config.ScriptManager = new ScriptManager(@"Config/ScriptData.xml", tabPageScript, watchVariablePanelScript);
            Config.SoundManager = new SoundManager(tabPageSound);
            Config.WarpManager = new WarpManager(@"Config/WarpData.xml", tabPageWarp, watchVariablePanelWarp);

            Config.DisassemblyManager = new DisassemblyManager(tabPageDisassembly);
            Config.InjectionManager = new InjectionManager(_scriptParser, checkBoxUseRomHack);
            Config.HackManager = new HackManager(_romHacks, Config.ObjectAssociations.SpawnHacks, tabPageHacks);
            Config.M64Manager = new M64Manager(m64Gui);

            // Create Object Slots
            Config.ObjectSlotsManager = new ObjectSlotsManager(Config.ObjectSlotManagerGui, tabControlMain);
        }

        private void _sm64Stream_WarnReadonlyOff(object sender, EventArgs e)
        {
            this.TryInvoke(new Action(() =>
                {
                var dr = MessageBox.Show("Warning! Editing variables and enabling hacks may cause the emulator to freeze. Turn off read-only mode?", 
                    "Turn Off Read-only Mode?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (dr)
                {
                    case DialogResult.Yes:
                        Config.Stream.Readonly = false;
                        Config.Stream.ShowWarning = false;
                        break;

                    case DialogResult.No:
                        Config.Stream.ShowWarning = false;
                        break;

                    case DialogResult.Cancel:
                        break;
                }
            }));
        }

        private void _sm64Stream_OnDisconnect(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() => {
                buttonRefresh_Click(this, new EventArgs());
                panelConnect.Size = this.Size;
                panelConnect.Visible = true;
            }));
        }

        public void LoadConfig(MainLoadingForm loadingForm)
        {
            Config.ObjectSlotManagerGui = new ObjectSlotManagerGui()
            {
                TabControl = tabControlMain,
                LockLabelsCheckbox = checkBoxObjLockLabels,
                FlowLayoutContainer = WatchVariablePanelObjects,
                SortMethodComboBox = comboBoxSortMethod,
                LabelMethodComboBox = comboBoxLabelMethod,
                SelectionMethodComboBox = comboBoxSelectionMethod,
            };

            int statusNum = 0;

            // Read configuration
            loadingForm.UpdateStatus("Loading main configuration", statusNum++);
            XmlConfigParser.OpenConfig(@"Config/Config.xml");
            XmlConfigParser.OpenSavedSettings(@"Config/SavedSettings.xml");
            loadingForm.UpdateStatus("Loading Miscellaneous Data", statusNum++);
            loadingForm.UpdateStatus("Loading Object Data", statusNum++);
            loadingForm.UpdateStatus("Loading Object Associations", statusNum++);
            Config.ObjectAssociations = XmlConfigParser.OpenObjectAssoc(@"Config/ObjectAssociations.xml", Config.ObjectSlotManagerGui);
            loadingForm.UpdateStatus("Loading Mario Data", statusNum++);
            loadingForm.UpdateStatus("Loading Camera Data", statusNum++);
            loadingForm.UpdateStatus("Loading Actions Data", statusNum++);
            loadingForm.UpdateStatus("Loading Water Data", statusNum++);
            loadingForm.UpdateStatus("Loading Input Data", statusNum++);
            loadingForm.UpdateStatus("Loading Input Image Associations", statusNum++);
            _inputImageGuiList = XmlConfigParser.CreateInputImageAssocList(@"Config/InputImageAssociations.xml");
            loadingForm.UpdateStatus("Loading File Data", statusNum++);
            loadingForm.UpdateStatus("Loading File Image Associations", statusNum++);
            XmlConfigParser.OpenFileImageAssoc(@"Config/FileImageAssociations.xml", _fileImageGui);
            loadingForm.UpdateStatus("Loading Area Data", statusNum++);
            loadingForm.UpdateStatus("Loading Quarter Frame Data", statusNum++);
            loadingForm.UpdateStatus("Loading Camera Hack Data", statusNum++);
            loadingForm.UpdateStatus("Loading Triangles Data", statusNum++);
            loadingForm.UpdateStatus("Loading Debug Data", statusNum++);
            loadingForm.UpdateStatus("Loading HUD Data", statusNum++);
            loadingForm.UpdateStatus("Loading Map Associations", statusNum++);
            Config.MapAssociations = XmlConfigParser.OpenMapAssoc(@"Config/MapAssociations.xml");
            loadingForm.UpdateStatus("Loading Scripts", statusNum++);
            _scriptParser = XmlConfigParser.OpenScripts(@"Config/Scripts.xml");
            loadingForm.UpdateStatus("Loading Hacks", statusNum++);
            _romHacks = XmlConfigParser.OpenHacks(@"Config/Hacks.xml");
            loadingForm.UpdateStatus("Loading Mario Actions", statusNum++);

            TableConfig.MarioActions = XmlConfigParser.OpenActionTable(@"Config/MarioActions.xml");
            TableConfig.MarioAnimations = XmlConfigParser.OpenAnimationTable(@"Config/MarioAnimations.xml");
            TableConfig.TriangleInfo = XmlConfigParser.OpenTriangleInfoTable(@"Config/TriangleInfo.xml");
            TableConfig.PendulumSwings = XmlConfigParser.OpenPendulumSwingTable(@"Config/PendulumSwings.xml");
            TableConfig.RacingPenguinWaypoints = XmlConfigParser.OpenWaypointTable(@"Config/RacingPenguinWaypoints.xml");
            TableConfig.KoopaTheQuick1Waypoints = XmlConfigParser.OpenWaypointTable(@"Config/KoopaTheQuick1Waypoints.xml");
            TableConfig.KoopaTheQuick2Waypoints = XmlConfigParser.OpenWaypointTable(@"Config/KoopaTheQuick2Waypoints.xml");
            TableConfig.TtmBowlingBallPoints = XmlConfigParser.OpenPointTable(@"Config/TtmBowlingBallPoints.xml");
            TableConfig.MusicData = XmlConfigParser.OpenMusicTable(@"Config/MusicData.xml");
            TableConfig.Missions = XmlConfigParser.OpenMissionTable(@"Config/Missions.xml");
            TableConfig.CourseData = XmlConfigParser.OpenCourseDataTable(@"Config/CourseData.xml");
            TableConfig.FlyGuyData = new FlyGuyDataTable();
            TableConfig.WdwRotatingPlatformTable = new ObjectAngleTable(1120);
            TableConfig.ElevatorAxleTable = new ObjectAngleTable(400);

            loadingForm.UpdateStatus("Creating Managers", statusNum++);
            CreateManagers();

            loadingForm.UpdateStatus("Finishing", statusNum);
        }

        private List<Process> GetAvailableProcesses()
        {
            var AvailableProcesses = Process.GetProcesses();
            List<Process> resortList = new List<Process>();
            foreach (Process p in AvailableProcesses)
            {
                try
                {
                    if (!Config.Emulators.Any(e => e.ProcessName.ToLower() == p.ProcessName.ToLower()))
                        continue;

                    if (p.HasExited)
                        continue;
                }
                catch (Win32Exception) // Access is denied
                {
                    continue;
                }

                resortList.Add(p);
            }
            return resortList;
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            this.TryInvoke(new Action(() =>
            {
                UpdateComboBoxes();
                DataModels.Update();
                FormManager.Update();
                Config.ObjectSlotsManager.Update();
                Config.ObjectManager.Update(tabControlMain.SelectedTab == tabPageObject);
                Config.MarioManager.Update(tabControlMain.SelectedTab == tabPageMario);
                Config.CameraManager.Update(tabControlMain.SelectedTab == tabPageCamera);
                Config.HudManager.Update(tabControlMain.SelectedTab == tabPageHud);
                Config.ActionsManager.Update(tabControlMain.SelectedTab == tabPageActions);
                Config.WaterManager.Update(tabControlMain.SelectedTab == tabPageWater);
                Config.SnowManager.Update(tabControlMain.SelectedTab == tabPageSnow);
                Config.InputManager.Update(tabControlMain.SelectedTab == tabPageInput);
                Config.FileManager.Update(tabControlMain.SelectedTab == tabPageFile);
                Config.MainSaveManager.Update(tabControlMain.SelectedTab == tabPageMainSave);
                Config.QuarterFrameManager.Update(tabControlMain.SelectedTab == tabPageQuarterFrame);
                Config.CustomManager.Update(tabControlMain.SelectedTab == tabPageCustom);
                Config.VarHackManager.Update(tabControlMain.SelectedTab == tabPageVarHack);
                Config.CamHackManager.Update(tabControlMain.SelectedTab == tabPageCamHack);
                Config.MiscManager.Update(tabControlMain.SelectedTab == tabPageMisc);
                Config.TriangleManager.Update(tabControlMain.SelectedTab == tabPageTriangles);
                Config.AreaManager.Update(tabControlMain.SelectedTab == tabPageArea);
                Config.DebugManager.Update(tabControlMain.SelectedTab == tabPageDebug);
                Config.PuManager.Update(tabControlMain.SelectedTab == tabPagePu);
                Config.TasManager.Update(tabControlMain.SelectedTab == tabPageTas);
                Config.TestingManager.Update(tabControlMain.SelectedTab == tabPageTesting);
                Config.GfxManager.Update(tabControlMain.SelectedTab == tabPageGfx);
                Config.PaintingManager.Update(tabControlMain.SelectedTab == tabPagePainting);
                Config.MusicManager.Update(tabControlMain.SelectedTab == tabPageMusic);
                Config.SoundManager.Update(tabControlMain.SelectedTab == tabPageSound);
                Config.OptionsManager.Update(tabControlMain.SelectedTab == tabPageOptions);
                Config.MemoryManager.Update(tabControlMain.SelectedTab == tabPageMemory);
                Config.SearchManager.Update(tabControlMain.SelectedTab == tabPageSearch);
                Config.CellsManager.Update(tabControlMain.SelectedTab == tabPageCells);
                Config.CoinManager.Update(tabControlMain.SelectedTab == tabPageCoin);
                Config.M64Manager.Update(tabControlMain.SelectedTab == tabPageM64);
                Config.MapManager.Update(tabControlMain.SelectedTab == tabPageMap);
                Config.ScriptManager.Update(tabControlMain.SelectedTab == tabPageScript);
                Config.WarpManager.Update(tabControlMain.SelectedTab == tabPageWarp);
                Config.ModelManager?.Update();
                Config.InjectionManager.Update();
                Config.HackManager.Update();
                WatchVariableLockManager.Update();
                TestUtilities.Update();
                TriangleDataModel.ClearCache();
            }));
        }

        private void UpdateComboBoxes()
        {
            // Rom Version
            RomVersionConfig.UpdateRomVersion(comboBoxRomVersion);

            // Readonly / Read+Write
            Config.Stream.Readonly = (ReadWriteMode)comboBoxReadWriteMode.SelectedItem == ReadWriteMode.ReadOnly;
        }

        private void _sm64Stream_FpsUpdated(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                labelFpsCounter.Text = "FPS: " + (int)Config.Stream.FpsInPractice;
            }));
        }

        private void SetupViews()
        {
            // Mario Image
            pictureBoxMario.Image = Config.ObjectAssociations.MarioImage;
            panelMarioBorder.BackColor = Config.ObjectAssociations.MarioColor;
            pictureBoxMario.BackColor = Config.ObjectAssociations.MarioColor.Lighten(0.5);

            // Camera Image
            pictureBoxCamera.Image = Config.ObjectAssociations.CameraImage;
            panelCameraBorder.BackColor = Config.ObjectAssociations.CameraColor;
            pictureBoxCamera.BackColor = Config.ObjectAssociations.CameraColor.Lighten(0.5);

            // Hud Image
            pictureBoxHud.Image = Config.ObjectAssociations.HudImage;
            panelHudBorder.BackColor = Config.ObjectAssociations.HudColor;
            pictureBoxHud.BackColor = Config.ObjectAssociations.HudColor.Lighten(0.5);

            // Debug Image
            pictureBoxDebug.Image = Config.ObjectAssociations.DebugImage;
            panelDebugBorder.BackColor = Config.ObjectAssociations.DebugColor;
            pictureBoxDebug.BackColor = Config.ObjectAssociations.DebugColor.Lighten(0.5);

            // Misc Image
            pictureBoxMisc.Image = Config.ObjectAssociations.MiscImage;
            panelMiscBorder.BackColor = Config.ObjectAssociations.MiscColor;
            pictureBoxMisc.BackColor = Config.ObjectAssociations.MiscColor.Lighten(0.5);

        }

        private async void glControlMap2D_Load(object sender, EventArgs e)
        {
            await Task.Run(() => {
                while (Config.MapManager == null)
                {
                    Task.Delay(1).Wait();
                }
            });
            Config.MapManager.Load2D();
        }

        private async void glControlMap3D_Load(object sender, EventArgs e)
        {
            await Task.Run(() => {
                while (Config.MapManager == null)
                {
                    Task.Delay(1).Wait();
                }
            });
            Config.MapManager.Load3D();
        }

        private async void glControlModelView_Load(object sender, EventArgs e)
        {
            await Task.Run(() => {
                while (Config.ModelManager == null)
                {
                    Task.Delay(1).Wait();
                }
            });
            Config.ModelManager.Load();
        }

        private void buttonShowTopPanel_Click(object sender, EventArgs e)
        {
            SplitContainer splitContainer =
                ControlUtilities.GetDescendantSplitContainer(
                    splitContainerMain, Orientation.Horizontal);
            if (splitContainer == null) return;
            splitContainer.Panel1Collapsed = false;
            splitContainer.Panel2Collapsed = true;
        }

        private void buttonShowBottomPanel_Click(object sender, EventArgs e)
        {
            SplitContainer splitContainer =
                ControlUtilities.GetDescendantSplitContainer(
                    splitContainerMain, Orientation.Horizontal);
            if (splitContainer == null) return;
            splitContainer.Panel1Collapsed = true;
            splitContainer.Panel2Collapsed = false;
        }

        private void buttonShowTopBottomPanel_Click(object sender, EventArgs e)
        {
            SplitContainer splitContainer =
                ControlUtilities.GetDescendantSplitContainer(
                    splitContainerMain, Orientation.Horizontal);
            if (splitContainer == null) return;
            splitContainer.Panel1Collapsed = false;
            splitContainer.Panel2Collapsed = false;
        }

        private void buttonShowLeftPanel_Click(object sender, EventArgs e)
        {
            SplitContainer splitContainer =
                ControlUtilities.GetDescendantSplitContainer(
                    splitContainerMain, Orientation.Vertical);
            if (splitContainer == null) return;
            splitContainer.Panel1Collapsed = false;
            splitContainer.Panel2Collapsed = true;
        }

        private void buttonShowRightPanel_Click(object sender, EventArgs e)
        {
            SplitContainer splitContainer =
                ControlUtilities.GetDescendantSplitContainer(
                    splitContainerMain, Orientation.Vertical);
            if (splitContainer == null) return;
            splitContainer.Panel1Collapsed = true;
            splitContainer.Panel2Collapsed = false;
        }

        private void buttonShowLeftRightPanel_Click(object sender, EventArgs e)
        {
            SplitContainer splitContainer =
                ControlUtilities.GetDescendantSplitContainer(
                    splitContainerMain, Orientation.Vertical);
            if (splitContainer == null) return;
            splitContainer.Panel1Collapsed = false;
            splitContainer.Panel2Collapsed = false;
        }

        private void buttonMoveTabLeft_Click(object sender, EventArgs e)
        {
            if (KeyboardUtilities.IsCtrlHeld() || KeyboardUtilities.IsNumberHeld())
            {
                ObjectOrderingUtilities.Move(false);
            }
            else
            {
                MoveTab(false);
            }
        }

        private void buttonMoveTabRight_Click(object sender, EventArgs e)
        {
            if (KeyboardUtilities.IsCtrlHeld() || KeyboardUtilities.IsNumberHeld())
            {
                ObjectOrderingUtilities.Move(true);
            }
            else
            {
                MoveTab(true);
            }
        }

        private void MoveTab(bool rightwards)
        {
            TabPage currentTab = tabControlMain.SelectedTab;
            int currentIndex = tabControlMain.TabPages.IndexOf(currentTab);
            int indexDiff = rightwards ? +1 : -1;
            int newIndex = currentIndex + indexDiff;
            if (newIndex < 0 || newIndex >= tabControlMain.TabCount) return;

            TabPage adjacentTab = tabControlMain.TabPages[newIndex];
            tabControlMain.TabPages.Remove(adjacentTab);
            tabControlMain.TabPages.Insert(currentIndex, adjacentTab);

            SavedSettingsConfig.Save();
        }

        private void buttonTabAdd_Click(object sender, EventArgs e)
        {
            buttonTabAdd.ContextMenuStrip.Show(Cursor.Position);
        }

        private void StroopMainForm_Resize(object sender, EventArgs e)
        {
            panelConnect.Size = this.Size;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            var selectedProcess = (ProcessSelection?)listBoxProcessesList.SelectedItem;

            // Select the only process if there is one
            if (!selectedProcess.HasValue && listBoxProcessesList.Items.Count == 1 && AttachToProcess(selectedProcess.Value.Process))
                    selectedProcess = (ProcessSelection)listBoxProcessesList.Items[0];

            if (!selectedProcess.HasValue || !AttachToProcess(selectedProcess.Value.Process))
            {
                MessageBox.Show("Could not attach to process!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            panelConnect.Visible = false;
            labelProcessSelect.Text = "Connected To: " + selectedProcess.Value.Process.ProcessName;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            // Update the process list
            listBoxProcessesList.Items.Clear();
            var processes = GetAvailableProcesses().OrderBy(p => p.StartTime).ToList();
            for (int i = 0; i < processes.Count; i++)
                listBoxProcessesList.Items.Add(new ProcessSelection(processes[i], i + 1));
            
            // Pre-select the first process
            if (listBoxProcessesList.Items.Count != 0)
                listBoxProcessesList.SelectedIndex = 0;
        }

        private void buttonBypass_Click(object sender, EventArgs e)
        {
            panelConnect.Visible = false;
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            Task.Run(() => Config.Stream.SwitchProcess(null, null));
            buttonRefresh_Click(this, new EventArgs());
            panelConnect.Size = this.Size;
            panelConnect.Visible = true;
        }

        private void buttonRefreshAndConnect_Click(object sender, EventArgs e)
        {
            buttonRefresh_Click(sender, e);
            buttonConnect_Click(sender, e);
        }

        private async void trackBarObjSlotSize_ValueChanged(object sender, EventArgs e)
        {
            ChangeObjectSlotSize(trackBarObjSlotSize.Value);
        }

        private async void ChangeObjectSlotSize(int size)
        {
            _resizeObjSlotTime = 500;
            if (_objSlotResizing)
                return;

            _objSlotResizing = true;

            await Task.Run(() =>
            {
                while (_resizeObjSlotTime > 0)
                {
                    Task.Delay(100).Wait();
                    _resizeObjSlotTime -= 100;
                }
            });

            WatchVariablePanelObjects.Visible = false;
            Config.ObjectSlotsManager.ChangeSlotSize(size);
            WatchVariablePanelObjects.Visible = true;
            _objSlotResizing = false;
        }

        private void buttonOpenSavestate_Click(object sender, EventArgs e)
        {
            if (openFileDialogSt.ShowDialog() != DialogResult.OK)
                return;
            string stextension = Path.GetExtension(openFileDialogSt.FileName);
            if (openFileDialogSt.CheckFileExists == true && stextension != ".st")
            {
                    try
                    {
                        Config.Stream.OpenSTFile(openFileDialogSt.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("Savestate is corrupted not a savestate or doesnt exist", "Invalid Savestate",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
            }
            labelProcessSelect.Text = "Connected To: " + Config.Stream.ProcessName;
            panelConnect.Visible = false;
        }

        private void saveAsSavestate()
        {
            StFileIO io = Config.Stream.IO as StFileIO;
            if (io == null)
            {
                MessageBox.Show("The current connection is not an ST file. Open an savestate file to save the savestate.", "Connection not a savestate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            saveFileDialogSt.FileName = io.Name;
            DialogResult dr = saveFileDialogSt.ShowDialog();
            if (dr != DialogResult.OK)
                return;

            io.SaveMemory(saveFileDialogSt.FileName);
        }

        public void SwitchTab(string name)
        {
            List<TabPage> tabPages = ControlUtilities.GetTabPages(tabControlMain);
            bool containsTab = tabPages.Any(tabPage => tabPage.Name == name);
            if (containsTab) tabControlMain.SelectTab(name);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Config.Stream != null)
            {
                Config.Stream.OnUpdate -= OnUpdate;
                Config.Stream.FpsUpdated -= _sm64Stream_FpsUpdated;
                Config.Stream.OnDisconnect -= _sm64Stream_OnDisconnect;
                Config.Stream.WarnReadonlyOff -= _sm64Stream_WarnReadonlyOff;
                Config.Stream.Dispose();
                Task.Run(async () =>
                {       
                    await Config.Stream.WaitForDispose();
                    Config.Stream = null;
                    Invoke(new Action(() => Close()));
                });
                e.Cancel = true;
                return;
            }
            
            base.OnFormClosing(e);
        }
    }
}
