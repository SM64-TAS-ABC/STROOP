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

namespace STROOP
{
    public partial class StroopMainForm : Form
    {
        const string _version = "v0.3.0d";
        
        ObjectSlotManagerGui _slotManagerGui = new ObjectSlotManagerGui();
        InputImageGui _inputImageGui = new InputImageGui();
        FileImageGui _fileImageGui = new FileImageGui();
        List<WatchVariableControlPrecursor> _watchVarControlList, _waterData, _miscData, _areaData, _inputData, _fileData,
            _debugData, _camHackData, _hudData, _cameraData, _quarterFrameData, _actionsData, _puData, _tasData,
            _triangleData, _marioData, _objectData, _gfxData;
        MapAssociations _mapAssoc;
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
                MessageBox.Show("Ambigous emulator type", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            Config.Stream.OnClose += _sm64Stream_OnClose;

            comboBoxRomVersion.DataSource = Enum.GetValues(typeof(RomVersion));
            comboBoxReadWriteMode.DataSource = Enum.GetValues(typeof(ReadWriteMode));

            ControlUtilities.AddContextMenuStripFunctions(
                labelVersionNumber,
                new List<string>() { "Enable TASer Settings", "Test Something" },
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
                    () => TestUtilities.TestSomething(),
                });

            Config.StroopMainForm = this;

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

        private void CreateManagers()
        {
            // Create map manager
            MapGui mapGui = new MapGui();

            // Main controls
            mapGui.GLControl = glControlMap;
            mapGui.MapTrackerFlowLayoutPanel = flowLayoutPanelMapTrackers;
            mapGui.TabControlView = tabControlMap;

            // Controls in options tab
            mapGui.TabPageOptions = tabPageMapOptions;
            mapGui.CheckBoxTrackMario = checkBoxMapControlsTrackMario;
            mapGui.CheckBoxTrackHolp = checkBoxMapControlsTrackHolp;
            mapGui.CheckBoxTrackCamera = checkBoxMapControlsTrackCamera;
            mapGui.CheckBoxTrackFloorTriangle = checkBoxMapControlsTrackFloorTriangle;
            mapGui.CheckBoxTrackWallTriangle = checkBoxMapControlsTrackWallTriangle;
            mapGui.CheckBoxTrackCeilingTriangle = checkBoxMapControlsTrackCeilingTriangle;
            mapGui.CheckBoxTrackAllObjects = checkBoxMapControlsTrackAllObjects;
            mapGui.CheckBoxTrackGridlines = checkBoxMapControlsTrackGridlines;

            mapGui.ButtonAddNewTracker = buttonMapControlsAddNewTracker;
            mapGui.ButtonClearAllTrackers = buttonMapControlsClearAllTrackers;
            mapGui.ButtonTrackSelectedObjects = buttonMapControlsTrackSelectedObjects;

            // Controls in 2D tab
            mapGui.TabPage2D = tabPageMap2D;
            mapGui.RadioButtonScaleCourseDefault = radioButtonMapControlsScaleCourseDefault;
            mapGui.RadioButtonScaleMaxCourseSize = radioButtonMapControlsScaleMaxCourseSize;
            mapGui.RadioButtonScaleCustom = radioButtonMapControlsScaleCustom;
            mapGui.TextBoxScaleCustom = betterTextboxMapControlsScaleCustom;

            mapGui.ButtonCenterScaleChangeMinus = buttonMapControlsScaleChangeMinus;
            mapGui.ButtonCenterScaleChangePlus = buttonMapControlsScaleChangePlus;
            mapGui.TextBoxScaleChange = betterTextboxMapControlsScaleChange;

            mapGui.RadioButtonCenterBestFit = radioButtonMapControlsCenterBestFit;
            mapGui.RadioButtonCenterOrigin = radioButtonMapControlsCenterOrigin;
            mapGui.RadioButtonCenterCustom = radioButtonMapControlsCenterCustom;
            mapGui.TextBoxCenterCustom = betterTextboxMapControlsCenterCustom;

            mapGui.ButtonCenterChangeUp = buttonMapControlsCenterChangeUp;
            mapGui.ButtonCenterChangeDown = buttonMapControlsCenterChangeDown;
            mapGui.ButtonCenterChangeLeft = buttonMapControlsCenterChangeLeft;
            mapGui.ButtonCenterChangeRight = buttonMapControlsCenterChangeRight;
            mapGui.ButtonCenterChangeUpLeft = buttonMapControlsCenterChangeUpLeft;
            mapGui.ButtonCenterChangeUpRight = buttonMapControlsCenterChangeUpRight;
            mapGui.ButtonCenterChangeDownLeft = buttonMapControlsCenterChangeDownLeft;
            mapGui.ButtonCenterChangeDownRight = buttonMapControlsCenterChangeDownRight;

            mapGui.RadioButtonAngle0 = radioButtonMapControlsAngle0;
            mapGui.RadioButtonAngle16384 = radioButtonMapControlsAngle16384;
            mapGui.RadioButtonAngle32768 = radioButtonMapControlsAngle32768;
            mapGui.RadioButtonAngle49152 = radioButtonMapControlsAngle49152;
            mapGui.RadioButtonAngleCustom = radioButtonMapControlsAngleCustom;
            mapGui.TextBoxAngleCustom = betterTextboxMapControlsAngleCustom;

            mapGui.ButtonAngleChangeCounterclockwise = buttonMapControlsAngleChangeCounterclockwise;
            mapGui.ButtonAngleChangeClockwise = buttonMapControlsAngleChangeClockwise;
            mapGui.TextBoxAngleChange = betterTextboxMapControlsAngleChange;

            // Controls in the 3D map tab
            mapGui.TabPage3D = tabPageMap3D;
            mapGui.CheckBoxMapGameCamOrientation = checkBoxMapGameCamOrientation;
            mapGui.ComboBoxMapColorMethod = comboBoxMapColorMethod;

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

                ButtonSetUsHeader = buttonM64SetUsHeader,
                ButtonSetJpHeader = buttonM64SetJpHeader,

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
            };

            // Create managers
            Config.MapManager = new MapManager(_mapAssoc, mapGui);

            Config.ModelManager = new ModelManager(tabPageModel);
            Config.ActionsManager = new ActionsManager(_actionsData, watchVariablePanelActions, tabPageActions);
            Config.WaterManager = new WaterManager(_waterData, watchVariablePanelWater);
            Config.InputManager = new InputManager(_inputData, tabPageInput, watchVariablePanelInput, _inputImageGui);
            Config.MarioManager = new MarioManager(_marioData, tabPageMario, WatchVariablePanelMario);
            Config.HudManager = new HudManager(_hudData, tabPageHud, watchVariablePanelHud);
            Config.MiscManager = new MiscManager(_miscData, watchVariablePanelMisc, tabPageMisc);
            Config.CameraManager = new CameraManager(_cameraData, tabPageCamera, watchVariablePanelCamera);
            Config.TriangleManager = new TriangleManager(tabPageTriangles, _triangleData, watchVariablePanelTriangles);
            Config.DebugManager = new DebugManager(_debugData, tabPageDebug, watchVariablePanelDebug);
            Config.PuManager = new PuManager(_puData, tabPagePu, watchVariablePanelPu);
            Config.TasManager = new TasManager(_tasData, tabPageTas, watchVariablePanelTas);
            Config.FileManager = new FileManager(_fileData, tabPageFile, watchVariablePanelFile, _fileImageGui);
            Config.AreaManager = new AreaManager(tabPageArea, _areaData, watchVariablePanelArea);
            Config.QuarterFrameManager = new DataManager(_quarterFrameData, watchVariablePanelQuarterFrame);
            Config.CustomManager = new CustomManager(_watchVarControlList, tabPageCustom, watchVariablePanelCustom);
            Config.VarHackManager = new VarHackManager(tabPageVarHack, varHackPanel);
            Config.CameraHackManager = new CamHackManager(_camHackData, tabPageCamHack, watchVariablePanelCamHack);
            Config.ObjectManager = new ObjectManager(_objectData, tabPageObject, WatchVariablePanelObject);
            Config.OptionsManager = new OptionsManager(tabPageOptions);
            Config.TestingManager = new TestingManager(tabPageTesting);
            Config.MemoryManager = new MemoryManager(tabPageMemory, watchVariablePanelMemory, _objectData);
            Config.ScriptManager = new ScriptManager(tabPageScripts);
            Config.GfxManager = new GfxManager(tabPageGfx, _gfxData, watchVariablePanelGfx);

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
            Invoke(new Action(() =>
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

        public void LoadConfig(LoadingForm loadingForm)
        {
            int statusNum = 0;

            // Read configuration
            loadingForm.UpdateStatus("Loading main configuration", statusNum++);
            XmlConfigParser.OpenConfig(@"Config/Config.xml");
            loadingForm.UpdateStatus("Loading Miscellaneous Data", statusNum++);
            _miscData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/MiscData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Object Data", statusNum++);
            _objectData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/ObjectData.xml", "ObjectDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Object Associations", statusNum++);
            Config.ObjectAssociations = XmlConfigParser.OpenObjectAssoc(@"Config/ObjectAssociations.xml", _slotManagerGui);
            loadingForm.UpdateStatus("Loading Mario Data", statusNum++);
            _marioData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/MarioData.xml", "MarioDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Camera Data", statusNum++);
            _cameraData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/CameraData.xml", "CameraDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Actions Data", statusNum++);
            _actionsData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/ActionsData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Water Data", statusNum++);
            _waterData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/WaterData.xml", "MiscDataSchema.xsd");
            _watchVarControlList = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/CustomData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Input Data", statusNum++);
            _inputData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/InputData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Input Image Associations", statusNum++);
            XmlConfigParser.OpenInputImageAssoc(@"Config/InputImageAssociations.xml", _inputImageGui);
            loadingForm.UpdateStatus("Loading File Data", statusNum++);
            _fileData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/FileData.xml", "FileDataSchema.xsd");
            _puData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/PuData.xml", "MiscDataSchema.xsd");
            _tasData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/TasData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading File Image Associations", statusNum++);
            XmlConfigParser.OpenFileImageAssoc(@"Config/FileImageAssociations.xml", _fileImageGui);
            loadingForm.UpdateStatus("Loading Area Data", statusNum++);
            _areaData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/AreaData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Quarter Frame Data", statusNum++);
            _quarterFrameData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/QuarterFrameData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Camera Hack Data", statusNum++);
            _camHackData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/CamHackData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Triangles Data", statusNum++);
            _triangleData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/TrianglesData.xml", "TrianglesDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Debug Data", statusNum++);
            _debugData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/DebugData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading HUD Data", statusNum++);
            _hudData = XmlConfigParser.OpenWatchVariableControlPrecursors(@"Config/HudData.xml", "HudDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Map Associations", statusNum++);
            _mapAssoc = XmlConfigParser.OpenMapAssoc(@"Config/MapAssociations.xml");
            loadingForm.UpdateStatus("Loading Scripts", statusNum++);
            _scriptParser = XmlConfigParser.OpenScripts(@"Config/Scripts.xml");
            loadingForm.UpdateStatus("Loading Hacks", statusNum++);
            _romHacks = XmlConfigParser.OpenHacks(@"Config/Hacks.xml");
            _gfxData = new List<WatchVariableControlPrecursor>();
            loadingForm.UpdateStatus("Loading Mario Actions", statusNum++);

            TableConfig.MarioActions = XmlConfigParser.OpenActionTable(@"Config/MarioActions.xml");
            TableConfig.MarioAnimations = XmlConfigParser.OpenAnimationTable(@"Config/MarioAnimations.xml");
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
            Invoke(new Action(() =>
            {
                RomVersionConfig.Version = (RomVersion)comboBoxRomVersion.SelectedItem;
                Config.Stream.Readonly = (ReadWriteMode)comboBoxReadWriteMode.SelectedItem == ReadWriteMode.ReadOnly;

                DataModels.Update();
                Config.ObjectSlotsManager.Update();
                Config.ObjectManager.Update(tabControlMain.SelectedTab == tabPageObject);
                Config.MarioManager.Update(tabControlMain.SelectedTab == tabPageMario);
                Config.CameraManager.Update(tabControlMain.SelectedTab == tabPageCamera);
                Config.HudManager.Update(tabControlMain.SelectedTab == tabPageHud);
                Config.ActionsManager.Update(tabControlMain.SelectedTab == tabPageActions);
                Config.WaterManager.Update(tabControlMain.SelectedTab == tabPageWater);
                Config.InputManager.Update(tabControlMain.SelectedTab == tabPageInput);
                Config.FileManager.Update(tabControlMain.SelectedTab == tabPageFile);
                Config.QuarterFrameManager.Update(tabControlMain.SelectedTab == tabPageQuarterFrame);
                Config.CustomManager.Update(tabControlMain.SelectedTab == tabPageCustom);
                Config.VarHackManager.Update(tabControlMain.SelectedTab == tabPageVarHack);
                Config.CameraHackManager.Update(tabControlMain.SelectedTab == tabPageCamHack);
                Config.MiscManager.Update(tabControlMain.SelectedTab == tabPageMisc);
                Config.TriangleManager.Update(tabControlMain.SelectedTab == tabPageTriangles);
                Config.AreaManager.Update(tabControlMain.SelectedTab == tabPageArea);
                Config.DebugManager.Update(tabControlMain.SelectedTab == tabPageDebug);
                Config.PuManager.Update(tabControlMain.SelectedTab == tabPagePu);
                Config.TasManager.Update(tabControlMain.SelectedTab == tabPageTas);
                Config.TestingManager.Update(tabControlMain.SelectedTab == tabPageTesting);
                Config.GfxManager.Update(tabControlMain.SelectedTab == tabPageGfx);
                Config.MemoryManager.Update(tabControlMain.SelectedTab == tabPageMemory);
                Config.M64Manager.Update(tabControlMain.SelectedTab == tabPageM64);
                Config.MapManager?.Update();
                Config.ModelManager?.Update();
                Config.InjectionManager.Update();
                Config.HackManager.Update();
                WatchVariableLockManager.Update();
            }));
        }

        private void _sm64Stream_FpsUpdated(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Config.Stream.IsRunning)
            {
                Config.Stream.OnUpdate -= OnUpdate;
                Config.Stream.FpsUpdated -= _sm64Stream_FpsUpdated;
                Config.Stream.OnDisconnect -= _sm64Stream_OnDisconnect;
                Config.Stream.WarnReadonlyOff -= _sm64Stream_WarnReadonlyOff;
                Config.Stream.OnClose -= _sm64Stream_OnClose;
                Config.Stream.Stop();
                e.Cancel = true;
                Hide();
                return;
            }
            
            base.OnFormClosing(e);
        }

        private void _sm64Stream_OnClose(object sender, EventArgs e)
        {
            Invoke(new Action(() => Close()));
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
            splitContainerMain.Panel1Collapsed = false;
            splitContainerMain.Panel2Collapsed = true;
        }

        private void buttonShowBottomPanel_Click(object sender, EventArgs e)
        {
            splitContainerMain.Panel1Collapsed = true;
            splitContainerMain.Panel2Collapsed = false;
        }

        private void buttonShowTopBottomPanel_Click(object sender, EventArgs e)
        {
            splitContainerMain.Panel1Collapsed = false;
            splitContainerMain.Panel2Collapsed = false;
        }

        private SplitContainer getSelectedTabSplitContainer()
        {
            SplitContainer selectedTabSplitContainer = null;
            TabPage selectedTabPage = tabControlMain.SelectedTab;

            if (selectedTabPage == tabPageObject)
                selectedTabSplitContainer = splitContainerObject;
            else if (selectedTabPage == tabPageMario)
                selectedTabSplitContainer = splitContainerMario;
            else if (selectedTabPage == tabPageHud)
                selectedTabSplitContainer = splitContainerHud;
            else if (selectedTabPage == tabPageCamera)
                selectedTabSplitContainer = splitContainerCamera;
            else if (selectedTabPage == tabPageTriangles)
                selectedTabSplitContainer = splitContainerTriangles;
            else if (selectedTabPage == tabPageInput)
                selectedTabSplitContainer = splitContainerInput;
            else if (selectedTabPage == tabPageFile)
                selectedTabSplitContainer = splitContainerFile;
            else if (selectedTabPage == tabPageCustom)
                selectedTabSplitContainer = splitContainerCustom;
            else if (selectedTabPage == tabPageVarHack)
                selectedTabSplitContainer = splitContainerVarHack;
            else if (selectedTabPage == tabPagePu)
                selectedTabSplitContainer = splitContainerPu;
            else if (selectedTabPage == tabPageTas)
                selectedTabSplitContainer = splitContainerTas;
            else if (selectedTabPage == tabPageMisc)
                selectedTabSplitContainer = splitContainerMisc;
            else if (selectedTabPage == tabPageDebug)
                selectedTabSplitContainer = splitContainerDebug;
            else if (selectedTabPage == tabPageMap)
                selectedTabSplitContainer = splitContainerMap;
            else if (selectedTabPage == tabPageArea)
                selectedTabSplitContainer = splitContainerArea;
            else if (selectedTabPage == tabPageModel)
                selectedTabSplitContainer = splitContainerModel;
            else if (selectedTabPage == tabPageMemory)
                selectedTabSplitContainer = splitContainerMemory;
            else if (selectedTabPage == tabPageHacks)
                selectedTabSplitContainer = splitContainerHacks;
            else if (selectedTabPage == tabPageCamHack)
                selectedTabSplitContainer = splitContainerCamHack;
            else if (selectedTabPage == tabPageM64)
                selectedTabSplitContainer = splitContainerM64;

            return selectedTabSplitContainer;
        }

        private void buttonShowLeftPanel_Click(object sender, EventArgs e)
        {
            SplitContainer selectedTabSplitContainer = getSelectedTabSplitContainer();
            if (selectedTabSplitContainer != null)
            {
                selectedTabSplitContainer.Panel1Collapsed = false;
                selectedTabSplitContainer.Panel2Collapsed = true;
            }
        }

        private void buttonShowRightPanel_Click(object sender, EventArgs e)
        {
            SplitContainer selectedTabSplitContainer = getSelectedTabSplitContainer();
            if (selectedTabSplitContainer != null)
            {
                selectedTabSplitContainer.Panel1Collapsed = true;
                selectedTabSplitContainer.Panel2Collapsed = false;
            }
        }

        private void buttonShowLeftRightPanel_Click(object sender, EventArgs e)
        {
            SplitContainer selectedTabSplitContainer = getSelectedTabSplitContainer();
            if (selectedTabSplitContainer != null)
            {
                selectedTabSplitContainer.Panel1Collapsed = false;
                selectedTabSplitContainer.Panel2Collapsed = false;
            }
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

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            Config.Stream.SwitchProcess(null, null);
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

        public void SwitchTab(string name)
        {
            tabControlMain.SelectTab(name);
        }
    }
}
