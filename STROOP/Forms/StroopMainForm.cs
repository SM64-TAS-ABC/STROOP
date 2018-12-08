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
using STROOP.Map2;

namespace STROOP
{
    public partial class StroopMainForm : Form
    {
        const string _version = "v0.3.0d";
        
        ObjectSlotManagerGui _slotManagerGui = new ObjectSlotManagerGui();
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

            comboBoxRomVersion.DataSource = Enum.GetValues(typeof(RomVersion));
            comboBoxReadWriteMode.DataSource = Enum.GetValues(typeof(ReadWriteMode));

            SetUpContextMenuStrips();

            Config.StroopMainForm = this;
            Config.TabControlMain = tabControlMain;
            Config.DebugText = labelDebugText;
            SavedSettingsConfig.StoreRecommendedTabOrder();
            SavedSettingsConfig.InvokeInitiallySavedTabOrder();
            Config.TabControlMain.SelectedIndex = 0;
            InitializeTabRemoval();

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
            buttonTabAdd.ContextMenuStrip.Opening += (se, ev) =>
            {
                buttonTabAdd.ContextMenuStrip.Items.Clear();
                SavedSettingsConfig.GetRemovedTabItems().ForEach(
                    item => buttonTabAdd.ContextMenuStrip.Items.Add(item));
            };

            SavedSettingsConfig.InvokeInitiallySavedRemovedTabs();
        }

        private void SetUpContextMenuStrips()
        {
            ControlUtilities.AddContextMenuStripFunctions(
                labelVersionNumber,
                new List<string>()
                {
                    "Enable TASer Settings",
                    "Show MHS Vars",
                    "Download Latest STROOP Release",
                    "Show All Helpful Hints",
                    "Add Gfx Vertices",
                    "Test Something",
                    "Test Something Else",
                },
                new List<Action>()
                {
                    () =>
                    {
                        Config.RamSize = 0x800000;
                        checkBoxUseRomHack.Checked = true;
                        splitContainerTas.Panel1Collapsed = true;
                        splitContainerTas.Panel2Collapsed = false;
                        Config.TasManager.ShowTaserVariables();
                        tabControlMain.SelectedTab = tabPageTas;
                    },
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
                    () => HelpfulHintUtilities.ShowAllHelpfulHints(),
                    () => TestUtilities.AddGraphicsTriangleVerticesToTriangleTab(),
                    () => TestUtilities.TestSomething(),
                    () => TestUtilities.TestSomethingElse(),
                });

            ControlUtilities.AddCheckableContextMenuStripFunctions(
                labelVersionNumber,
                new List<string>()
                {
                    "Disable Locking",
                    "Show Invisible Objects as Signs",
                    "Show Cog Tris",
                    "Show Shapes",
                },
                new List<Func<bool>>()
                {
                    () =>
                    {
                        LockConfig.LockingDisabled = !LockConfig.LockingDisabled;
                        return LockConfig.LockingDisabled;
                    },
                    () =>
                    {
                        TestingConfig.ShowInvisibleObjectsAsSigns = !TestingConfig.ShowInvisibleObjectsAsSigns;
                        return TestingConfig.ShowInvisibleObjectsAsSigns;
                    },
                    () =>
                    {
                        TestingConfig.ShowCogTris = !TestingConfig.ShowCogTris;
                        return TestingConfig.ShowCogTris;
                    },
                    () =>
                    {
                        TestingConfig.ShowShapes = !TestingConfig.ShowShapes;
                        return TestingConfig.ShowShapes;
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
                buttonDisconnect,
                new List<string>() { "Save as Savestate" },
                new List<Action>() { () => saveAsSavestate() });
        }

        private void CreateManagers()
        {
            // Create map manager
            Config.MapGui = new MapGui()
            {
                // Main controls
                GLControl = glControlMap,
                MapTrackerFlowLayoutPanel = flowLayoutPanelMapTrackers,
                TabControlView = tabControlMap,

                // Controls in options tab
                TabPageOptions = tabPageMapOptions,
                CheckBoxTrackMario = checkBoxMapControlsTrackMario,
                CheckBoxTrackHolp = checkBoxMapControlsTrackHolp,
                CheckBoxTrackCamera = checkBoxMapControlsTrackCamera,
                CheckBoxTrackFloorTriangle = checkBoxMapControlsTrackFloorTriangle,
                CheckBoxTrackWallTriangle = checkBoxMapControlsTrackWallTriangle,
                CheckBoxTrackCeilingTriangle = checkBoxMapControlsTrackCeilingTriangle,
                CheckBoxTrackAllObjects = checkBoxMapControlsTrackAllObjects,
                CheckBoxTrackGridlines = checkBoxMapControlsTrackGridlines,

                ButtonAddNewTracker = buttonMapControlsAddNewTracker,
                ButtonClearAllTrackers = buttonMapControlsClearAllTrackers,
                ButtonTrackSelectedObjects = buttonMapControlsTrackSelectedObjects,

                ComboBoxLevel = comboBoxMapOptionsLevel,
                ComboBoxBackground = comboBoxMapOptionsBackground,

                // Controls in 2D tab
                TabPage2D = tabPageMap2D,
                RadioButtonScaleCourseDefault = radioButtonMapControlsScaleCourseDefault,
                RadioButtonScaleMaxCourseSize = radioButtonMapControlsScaleMaxCourseSize,
                RadioButtonScaleCustom = radioButtonMapControlsScaleCustom,
                TextBoxScaleCustom = betterTextboxMapControlsScaleCustom,

                ButtonCenterScaleChangeMinus = buttonMapControlsScaleChangeMinus,
                ButtonCenterScaleChangePlus = buttonMapControlsScaleChangePlus,
                TextBoxScaleChange = betterTextboxMapControlsScaleChange,

                RadioButtonCenterBestFit = radioButtonMapControlsCenterBestFit,
                RadioButtonCenterOrigin = radioButtonMapControlsCenterOrigin,
                RadioButtonCenterCustom = radioButtonMapControlsCenterCustom,
                TextBoxCenterCustom = betterTextboxMapControlsCenterCustom,

                ButtonCenterChangeUp = buttonMapControlsCenterChangeUp,
                ButtonCenterChangeDown = buttonMapControlsCenterChangeDown,
                ButtonCenterChangeLeft = buttonMapControlsCenterChangeLeft,
                ButtonCenterChangeRight = buttonMapControlsCenterChangeRight,
                ButtonCenterChangeUpLeft = buttonMapControlsCenterChangeUpLeft,
                ButtonCenterChangeUpRight = buttonMapControlsCenterChangeUpRight,
                ButtonCenterChangeDownLeft = buttonMapControlsCenterChangeDownLeft,
                ButtonCenterChangeDownRight = buttonMapControlsCenterChangeDownRight,

                RadioButtonAngle0 = radioButtonMapControlsAngle0,
                RadioButtonAngle16384 = radioButtonMapControlsAngle16384,
                RadioButtonAngle32768 = radioButtonMapControlsAngle32768,
                RadioButtonAngle49152 = radioButtonMapControlsAngle49152,
                RadioButtonAngleCustom = radioButtonMapControlsAngleCustom,
                TextBoxAngleCustom = betterTextboxMapControlsAngleCustom,

                ButtonAngleChangeCounterclockwise = buttonMapControlsAngleChangeCounterclockwise,
                ButtonAngleChangeClockwise = buttonMapControlsAngleChangeClockwise,
                TextBoxAngleChange = betterTextboxMapControlsAngleChange,

                // Controls in the 3D map tab
                TabPage3D = tabPageMap3D,
                CheckBoxMapGameCamOrientation = checkBoxMapGameCamOrientation,
                ComboBoxMapColorMethod = comboBoxMapColorMethod
            };

            Map2Gui map2Gui = new Map2Gui()
            {
                GLControl = glControlMap2,
                MapIdLabel = labelMap2Id,
                MapNameLabel = labelMap2Name,
                MapSubNameLabel = labelMap2SubName,
                PuValueLabel = labelMap2PuValue,
                QpuValueLabel = labelMap2QpuValue,
                MapIconSizeTrackbar = trackBarMap2IconSize,
                MapShowInactiveObjects = checkBoxMap2ShowInactive,
                MapShowMario = checkBoxMap2ShowMario,
                MapShowHolp = checkBoxMap2ShowHolp,
                MapShowIntendedNextPosition = checkBoxMap2ShowIntendedNextPosition,
                MapShowPoint = checkBoxMap2ShowPoint,
                MapShowCamera = checkBoxMap2ShowCamera,
                MapShowFloorTriangle = checkBoxMap2ShowFloor,
                MapShowCeilingTriangle = checkBoxMap2ShowCeiling,

                MapBoundsUpButton = buttonMap2BoundsUp,
                MapBoundsDownButton = buttonMap2BoundsDown,
                MapBoundsLeftButton = buttonMap2BoundsLeft,
                MapBoundsRightButton = buttonMap2BoundsRight,
                MapBoundsUpLeftButton = buttonMap2BoundsUpLeft,
                MapBoundsUpRightButton = buttonMap2BoundsUpRight,
                MapBoundsDownLeftButton = buttonMap2BoundsDownLeft,
                MapBoundsDownRightButton = buttonMap2BoundsDownRight,
                MapBoundsPositionTextBox = textBoxMap2BoundsPosition,

                MapBoundsZoomInButton = buttonMap2BoundsZoomIn,
                MapBoundsZoomOutButton = buttonMap2BoundsZoomOut,
                MapBoundsZoomTextBox = textBoxMap2BoundsZoom,

                MapArtificialMarioYLabelTextBox = textBoxMap2ArtificialMarioYLabel
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
            Config.MapManager = new MapManager();
            Config.Map2Manager = new Map2Manager(map2Gui);

            Config.ModelManager = new ModelManager(tabPageModel);
            Config.ActionsManager = new ActionsManager(@"Config/ActionsData.xml", watchVariablePanelActions, tabPageActions);
            Config.WaterManager = new WaterManager(@"Config/WaterData.xml", watchVariablePanelWater);
            Config.SnowManager = new SnowManager(@"Config/SnowData.xml", watchVariablePanelSnow);
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
            Config.QuarterFrameManager = new DataManager(@"Config/QuarterFrameData.xml", watchVariablePanelQuarterFrame);
            Config.CustomManager = new CustomManager(@"Config/CustomData.xml", tabPageCustom, watchVariablePanelCustom);
            Config.VarHackManager = new VarHackManager(tabPageVarHack, varHackPanel);
            Config.CamHackManager = new CamHackManager(@"Config/CamHackData.xml", tabPageCamHack, watchVariablePanelCamHack);
            Config.ObjectManager = new ObjectManager(@"Config/ObjectData.xml", tabPageObject, WatchVariablePanelObject);
            Config.OptionsManager = new OptionsManager(tabPageOptions, pictureBoxCog);
            Config.TestingManager = new TestingManager(tabPageTesting);
            Config.MemoryManager = new MemoryManager(tabPageMemory, watchVariablePanelMemory, @"Config/ObjectData.xml");
            Config.CoinManager = new CoinManager(tabPageCoin);
            Config.ScriptManager = new ScriptManager(tabPageScripts);
            Config.GfxManager = new GfxManager(tabPageGfx, watchVariablePanelGfx);

            Config.DisassemblyManager = new DisassemblyManager(tabPageDisassembly);
            Config.DecompilerManager = new DecompilerManager(tabPageDecompiler);
            Config.InjectionManager = new InjectionManager(_scriptParser, checkBoxUseRomHack);
            Config.HackManager = new HackManager(_romHacks, Config.ObjectAssociations.SpawnHacks, tabPageHacks);
            Config.M64Manager = new M64Manager(m64Gui);

            // Create Object Slots
            _slotManagerGui.TabControl = tabControlMain;
            _slotManagerGui.LockLabelsCheckbox = checkBoxObjLockLabels;
            _slotManagerGui.FlowLayoutContainer = WatchVariablePanelObjects;
            _slotManagerGui.SortMethodComboBox = comboBoxSortMethod;
            _slotManagerGui.LabelMethodComboBox = comboBoxLabelMethod;
            Config.ObjectSlotsManager = new ObjectSlotsManager(_slotManagerGui, tabControlMain);
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
            int statusNum = 0;

            // Read configuration
            loadingForm.UpdateStatus("Loading main configuration", statusNum++);
            XmlConfigParser.OpenConfig(@"Config/Config.xml");
            XmlConfigParser.OpenSavedSettings(@"Config/SavedSettings.xml");
            loadingForm.UpdateStatus("Loading Miscellaneous Data", statusNum++);
            loadingForm.UpdateStatus("Loading Object Data", statusNum++);
            loadingForm.UpdateStatus("Loading Object Associations", statusNum++);
            Config.ObjectAssociations = XmlConfigParser.OpenObjectAssoc(@"Config/ObjectAssociations.xml", _slotManagerGui);
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
            TableConfig.Missions = XmlConfigParser.OpenMissionTable(@"Config/Missions.xml");
            TableConfig.CourseData = XmlConfigParser.OpenCourseDataTable(@"Config/CourseData.xml");
            TableConfig.FlyGuyData = new FlyGuyDataTable();

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
                    if (!Config.Emulators.Select(e => e.ProcessName.ToLower()).Any(s => s.Contains(p.ProcessName.ToLower())))
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
                Config.OptionsManager.Update(tabControlMain.SelectedTab == tabPageOptions);
                Config.MemoryManager.Update(tabControlMain.SelectedTab == tabPageMemory);
                Config.CoinManager.Update(tabControlMain.SelectedTab == tabPageCoin);
                Config.M64Manager.Update(tabControlMain.SelectedTab == tabPageM64);
                Config.MapManager?.Update();
                Config.Map2Manager?.Update();
                Config.ModelManager?.Update();
                Config.InjectionManager.Update();
                Config.HackManager.Update();
                WatchVariableLockManager.Update();
            }));
        }

        private void UpdateComboBoxes()
        {
            // Rom Version
            RomVersionConfig.UpdateRomVersionUsingTell();
            comboBoxRomVersion.SelectedItem = RomVersionConfig.Version;

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

        private async void WatchVariablePanelObjects_Resize(object sender, EventArgs e)
        {
            _resizeTimeLeft = 500;
            if (_resizing)
                return;

            _resizing = true;
            WatchVariablePanelObjects.Visible = false;
            WatchVariablePanelObject.Visible = false;
            WatchVariablePanelMario.Visible = false;
            if (Config.MapManager != null && Config.MapManager.IsLoaded)
                Config.MapManager.Visible = false;
            if (Config.ModelManager != null && Config.ModelManager.IsLoaded)
                Config.ModelManager.Visible = false;
            await Task.Run(() =>
            {
                while (_resizeTimeLeft > 0)
                {
                    Task.Delay(100).Wait();
                    _resizeTimeLeft -= 100;
                }
            });
            WatchVariablePanelObjects.Visible = true;
            WatchVariablePanelObject.Visible = true;
            WatchVariablePanelMario.Visible = true;
            if (Config.MapManager != null && Config.MapManager.IsLoaded)
                Config.MapManager.Visible = true;
            if (Config.ModelManager != null && Config.ModelManager.IsLoaded)
                Config.ModelManager.Visible = true;

            _resizing = false;
        }

        private async void glControlMap_Load(object sender, EventArgs e)
        {
            await Task.Run(() => {
                while (Config.MapManager == null)
                {
                    Task.Delay(1).Wait();
                }
            });
            Config.MapManager.Load();
        }

        private async void glControlMap2_Load(object sender, EventArgs e)
        {
            await Task.Run(() => {
                while (Config.Map2Manager == null)
                {
                    Task.Delay(1).Wait();
                }
            });
            Config.Map2Manager.Load();
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
            if (!selectedProcess.HasValue && listBoxProcessesList.Items.Count == 1)
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
            Config.ObjectSlotsManager.ChangeSlotSize(trackBarObjSlotSize.Value);
            WatchVariablePanelObjects.Visible = true;
            _objSlotResizing = false;
        }

        private void buttonOpenSavestate_Click(object sender, EventArgs e)
        {
            if (openFileDialogSt.ShowDialog() != DialogResult.OK)
                return;

            Config.Stream.OpenSTFile(openFileDialogSt.FileName);
            panelConnect.Visible = false;
        }

        private void saveAsSavestate()
        {
            // TODO(dane bouchie): Implement this
        }

        public void SwitchTab(string name)
        {
            tabControlMain.SelectTab(name);
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
