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
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.ManagerClasses;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic
{
    public partial class StroopMainForm : Form
    {
        const string _version = "v0.2.9";
        
        ObjectSlotManagerGui _slotManagerGui = new ObjectSlotManagerGui();
        InputImageGui _inputImageGui = new InputImageGui();
        FileImageGui _fileImageGui = new FileImageGui();
        List<WatchVariable> _objectData, _marioData, _cameraData, _hudData, _miscData, _triangleData, 
            _debugData, _actionsData, _waterData, _inputData, _fileData, _quarterFrameData, _camHackData;
        MapAssociations _mapAssoc;
        ScriptParser _scriptParser;
        List<RomHack> _romHacks;

        DataTable _tableOtherData = new DataTable();
        Dictionary<int, DataRow> _otherDataRowAssoc = new Dictionary<int, DataRow>();

        ObjectSlotsManager _objectSlotManager;
        DisassemblyManager _disManager;
        MarioManager _marioManager;
        InputManager _inputManager;
        ActionsManager _actionsManager;
        ObjectManager _objectManager;
        MapManager _mapManager;
        ModelManager _modelManager;
        OptionsManager _optionsManager;
        TestingManager _testingManager;
        ScriptManager _scriptManager;
        HudManager _hudManager;
        MiscManager _miscManager;
        CameraManager _cameraManager;
        HackManager _hackManager;
        TriangleManager _triangleManager;
        DebugManager _debugManager;
        CamHackManager _cameraHackManager;
        DataManager _waterManager, _quarterFrameManager;
        FileManager _fileManager;
        PuManager _puManager;

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
            // Temp: Remove "Other" tab
#if RELEASE
            tabControlMain.TabPages.Remove(tabPageExpressions);
#endif

            // Create new manager context
            var currentContext = new ManagerContext();
            ManagerContext.Current = currentContext;

            Config.Stream = new ProcessStream();
            Config.Stream.OnUpdate += OnUpdate;
            Config.Stream.FpsUpdated += _sm64Stream_FpsUpdated;
            Config.Stream.OnDisconnect += _sm64Stream_OnDisconnect;
            Config.Stream.WarnReadonlyOff += _sm64Stream_WarnReadonlyOff;
            Config.Stream.OnClose += _sm64Stream_OnClose;

            currentContext.DisassemblyManager = _disManager = new DisassemblyManager(tabPageDisassembly);
            currentContext.ScriptManager = _scriptManager = new ScriptManager(_scriptParser, checkBoxUseRomHack);
            currentContext.HackManager = _hackManager = new HackManager(_romHacks, Config.ObjectAssociations.SpawnHacks, tabPageHacks);

            // Create map manager
            MapGui mapGui = new MapGui();
            mapGui.GLControl = glControlMap;
            mapGui.MapIdLabel = labelMapId;
            mapGui.MapNameLabel = labelMapName;
            mapGui.MapSubNameLabel = labelMapSubName;
            mapGui.PuValueLabel = labelMapPuValue;
            mapGui.QpuValueLabel = labelMapQpuValue;
            mapGui.MapIconSizeTrackbar = trackBarMapIconSize;
            mapGui.MapZoomTrackbar = trackBarMapZoom;
            mapGui.MapShowInactiveObjects = checkBoxMapShowInactive;
            mapGui.MapShowMario = checkBoxMapShowMario;
            mapGui.MapShowHolp = checkBoxMapShowHolp;
            mapGui.MapShowCamera = checkBoxMapShowCamera;
            mapGui.MapShowFloorTriangle = checkBoxMapShowFloor;
            mapGui.MapShowCeilingTriangle = checkBoxMapShowCeiling;
            currentContext.MapManager = _mapManager = new MapManager(_mapAssoc, mapGui);
            currentContext.ModelManager = _modelManager = new ModelManager(tabPageModel);

            currentContext.ActionsManager = _actionsManager = new ActionsManager(_actionsData, noTearFlowLayoutPanelActions, tabPageActions);
            currentContext.WaterManager = _waterManager = new DataManager(_waterData, noTearFlowLayoutPanelWater);
            currentContext.InputManager = _inputManager = new InputManager(_inputData, tabPageInput, NoTearFlowLayoutPanelInput, _inputImageGui);
            currentContext.MarioManager = _marioManager = new MarioManager(_marioData, tabPageMario, NoTearFlowLayoutPanelMario, _mapManager);
            currentContext.HudManager = _hudManager = new HudManager(_hudData, tabPageHud, NoTearFlowLayoutPanelHud);
            currentContext.MiscManager = _miscManager = new MiscManager(_miscData, NoTearFlowLayoutPanelMisc);
            currentContext.CameraManager = _cameraManager = new CameraManager(_cameraData, tabPageCamera, NoTearFlowLayoutPanelCamera);
            currentContext.TriangleManager = _triangleManager = new TriangleManager(tabPageTriangles, _triangleData, NoTearFlowLayoutPanelTriangles);
            currentContext.DebugManager = _debugManager = new DebugManager(_debugData, tabPageDebug, noTearFlowLayoutPanelDebug);
            currentContext.PuManager = _puManager = new PuManager(groupBoxPuController);
            currentContext.FileManager = _fileManager = new FileManager(_fileData, tabPageFile, noTearFlowLayoutPanelFile, _fileImageGui);
            currentContext.QuarterFrameManager = _quarterFrameManager = new DataManager(_quarterFrameData, noTearFlowLayoutPanelQuarterFrame);
            currentContext.CameraHackManager = _cameraHackManager = new CamHackManager(_camHackData, tabPageCamHack, noTearFlowLayoutPanelCamHack);
            currentContext.ObjectManager = _objectManager = new ObjectManager(_objectData, tabPageObjects, NoTearFlowLayoutPanelObject);
            currentContext.OptionsManager = _optionsManager = new OptionsManager(tabPageOptions);
            currentContext.TestingManager = _testingManager = new TestingManager(tabPageTesting);

            // Create Object Slots
            _slotManagerGui.TabControl = tabControlMain;
            _slotManagerGui.LockLabelsCheckbox = checkBoxObjLockLabels;
            _slotManagerGui.FlowLayoutContainer = NoTearFlowLayoutPanelObjects;
            _slotManagerGui.SortMethodComboBox = comboBoxSortMethod;
            _slotManagerGui.LabelMethodComboBox = comboBoxLabelMethod;
            currentContext.ObjectSlotManager = _objectSlotManager = new ObjectSlotsManager(_objectManager, 
                _slotManagerGui, _mapManager, _miscManager, _modelManager, tabControlMain);

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
                        buttonReadOnly.Text = "Switch to Read-Only";
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
            _miscData = XmlConfigParser.OpenWatchVarData(@"Config/MiscData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Object Data", statusNum++);
            _objectData = XmlConfigParser.OpenWatchVarData(@"Config/ObjectData.xml", "ObjectDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Object Associations", statusNum++);
            Config.ObjectAssociations = XmlConfigParser.OpenObjectAssoc(@"Config/ObjectAssociations.xml", _slotManagerGui);
            loadingForm.UpdateStatus("Loading Mario Data", statusNum++);
            _marioData = XmlConfigParser.OpenWatchVarData(@"Config/MarioData.xml", "MarioDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Camera Data", statusNum++);
            _cameraData = XmlConfigParser.OpenWatchVarData(@"Config/CameraData.xml", "CameraDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Actions Data", statusNum++);
            _actionsData = XmlConfigParser.OpenWatchVarData(@"Config/ActionsData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Water Data", statusNum++);
            _waterData = XmlConfigParser.OpenWatchVarData(@"Config/WaterData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Input Data", statusNum++);
            _inputData = XmlConfigParser.OpenWatchVarData(@"Config/InputData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Input Image Associations", statusNum++);
            XmlConfigParser.OpenInputImageAssoc(@"Config/InputImageAssociations.xml", _inputImageGui);
            loadingForm.UpdateStatus("Loading File Data", statusNum++);
            _fileData = XmlConfigParser.OpenWatchVarData(@"Config/FileData.xml", "FileDataSchema.xsd");
            loadingForm.UpdateStatus("Loading File Image Associations", statusNum++);
            XmlConfigParser.OpenFileImageAssoc(@"Config/FileImageAssociations.xml", _fileImageGui);
            loadingForm.UpdateStatus("Loading Quarter Frame Data", statusNum++);
            _quarterFrameData = XmlConfigParser.OpenWatchVarData(@"Config/QuarterFrameData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Camera Hack Data", statusNum++);
            _camHackData = XmlConfigParser.OpenWatchVarData(@"Config/CamHackData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Triangles Data", statusNum++);
            _triangleData = XmlConfigParser.OpenWatchVarData(@"Config/TrianglesData.xml", "TrianglesDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Debug Data", statusNum++);
            _debugData = XmlConfigParser.OpenWatchVarData(@"Config/DebugData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading HUD Data", statusNum++);
            _hudData = XmlConfigParser.OpenWatchVarData(@"Config/HudData.xml", "HudDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Map Associations", statusNum++);
            _mapAssoc = XmlConfigParser.OpenMapAssoc(@"Config/MapAssociations.xml");
            loadingForm.UpdateStatus("Loading Scripts", statusNum++);
            _scriptParser = XmlConfigParser.OpenScripts(@"Config/Scripts.xml");
            loadingForm.UpdateStatus("Loading Hacks", statusNum++);
            var hacksConfig = XmlConfigParser.OpenHacks(@"Config/Hacks.xml");
            Config.Hacks = hacksConfig.Item1;
            _romHacks = hacksConfig.Item2;
            loadingForm.UpdateStatus("Loading Mario Actions", statusNum++);
            Config.MarioActions = XmlConfigParser.OpenActionTable(@"Config/MarioActions.xml");
            Config.MarioAnimations = XmlConfigParser.OpenAnimationTable(@"Config/MarioAnimations.xml");
            Config.PendulumSwings = XmlConfigParser.OpenPendulumSwingTable(@"Config/PendulumSwings.xml");
            Config.Missions = XmlConfigParser.OpenMissionTable(@"Config/Missions.xml");
            Config.CourseData = XmlConfigParser.OpenCourseDataTable(@"Config/CourseData.xml");
            Config.FlyGuyData = new FlyGuyDataTable();

            loadingForm.UpdateStatus("Finishing", statusNum);
        }

        private List<Process> GetAvailableProcesses()
        {
            var AvailableProcesses = Process.GetProcesses();
            List<Process> resortList = new List<Process>();
            foreach (Process p in AvailableProcesses)
            {
                if (!Config.Emulators.Select(e => e.ProcessName.ToLower()).Any(s => s.Contains(p.ProcessName.ToLower())))
                    continue;

                resortList.Add(p);
            }
            return resortList;
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                _objectSlotManager.Update();
                _objectManager.Update(tabControlMain.SelectedTab == tabPageObjects);
                _marioManager.Update(tabControlMain.SelectedTab == tabPageMario);
                _cameraManager.Update(tabControlMain.SelectedTab == tabPageCamera);
                _hudManager.Update(tabControlMain.SelectedTab == tabPageHud);
                _actionsManager.Update(tabControlMain.SelectedTab == tabPageActions);
                _waterManager.Update(tabControlMain.SelectedTab == tabPageWater);
                _inputManager.Update(tabControlMain.SelectedTab == tabPageInput);
                _fileManager.Update(tabControlMain.SelectedTab == tabPageFile);
                _quarterFrameManager.Update(tabControlMain.SelectedTab == tabPageQuarterFrame);
                _cameraHackManager.Update(tabControlMain.SelectedTab == tabPageCamHack);
                _miscManager.Update(tabControlMain.SelectedTab == tabPageMisc);
                _triangleManager.Update(tabControlMain.SelectedTab == tabPageTriangles);
                _debugManager.Update(tabControlMain.SelectedTab == tabPageDebug);
                _puManager.Update(tabControlMain.SelectedTab == tabPagePu);
                _testingManager.Update(tabControlMain.SelectedTab == tabPageTesting);
                _mapManager?.Update();
                _modelManager?.Update();
                _scriptManager.Update();
                _hackManager.Update();
            }));
        }

        private void _sm64Stream_FpsUpdated(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                labelFpsCounter.Text = "FPS: " + (int)Config.Stream.Fps;
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

            // Setup data columns
            var nameColumn = new DataColumn("Name");
            nameColumn.ReadOnly = true;
            nameColumn.DataType = typeof(string);
            _tableOtherData.Columns.Add(nameColumn);
            _tableOtherData.Columns.Add("Type", typeof(string));
            _tableOtherData.Columns.Add("Value", typeof(string));
            _tableOtherData.Columns.Add("Address", typeof(uint));

            // Setup grid view
            dataGridViewExpressions.DataSource = _tableOtherData;

            // Setup other data table
            for (int index = 0; index < _miscData.Count; index++)
            {
                var watchVar = _miscData[index];
                if (watchVar.IsSpecial)
                    continue;
                var row = _tableOtherData.Rows.Add(watchVar.Name, watchVar.TypeName, "", watchVar.Address);
                _otherDataRowAssoc.Add(index, row);
            }

#if !DEBUG
            tabControlMain.TabPages.Remove(tabPageExpressions);
#endif
        }

        private void buttonOtherModify_Click(object sender, EventArgs e)
        {
            var row = _tableOtherData.Rows[dataGridViewExpressions.SelectedRows[0].Index];
            int assoc = _otherDataRowAssoc.FirstOrDefault(v => v.Value == row).Key;

            //var modifyVar = new ModifyAddWatchVariableForm(_miscData[assoc]);
            //modifyVar.ShowDialog();
        }

        private void buttonOtherDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete selected variables?", "Delete Variables", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == DialogResult.Yes)
            {
                // Find indexes to delete
                var deleteVars = new List<int>();
                foreach (DataGridViewRow selectedRow in dataGridViewExpressions.SelectedRows)
                {
                    var row = _tableOtherData.Rows[selectedRow.Index];
                    int assoc = _otherDataRowAssoc.FirstOrDefault(v => v.Value == row).Key;
                    deleteVars.Add(assoc);
                }

                // Delete rows
                foreach (int i in deleteVars)
                {
                    DataRow row = _otherDataRowAssoc[i];
                    _miscData.RemoveAt(i);
                    _otherDataRowAssoc.Remove(i);
                    row.Delete();
                }

                // Delete from xml file
                XmlConfigParser.DeleteWatchVariablesOtherData(deleteVars);
            }
        }

        private void dataGridViewOther_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2)
                return;

            var row = _tableOtherData.Rows[dataGridViewExpressions.SelectedRows[0].Index];
            int assoc = _otherDataRowAssoc.FirstOrDefault(v => v.Value == row).Key;

            //var modifyVar = new ModifyAddWatchVariableForm(_miscData[assoc]);
            //modifyVar.ShowDialog();
        }

        private void buttonOtherAdd_Click(object sender, EventArgs e)
        {
            /*var modifyVar = new ModifyAddWatchVariableForm();
            if(modifyVar.ShowDialog() == DialogResult.OK)
            {
                var watchVar = modifyVar.Value;

                // Create new row
                var row = _tableOtherData.Rows.Add(watchVar.Name, watchVar.Type.ToString(), "", 
                    watchVar.AbsoluteAddressing ? watchVar.Address 
                    : watchVar.Address + Config.RamStartAddress);

                // Add variable to lists
                int newIndex = _miscData.Count;
                _miscData.Add(watchVar);
                _otherDataRowAssoc.Add(newIndex, row);

                XmlConfigParser.AddWatchVariableOtherData(watchVar);
            }*/
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private async void NoTearFlowLayoutPanelObjects_Resize(object sender, EventArgs e)
        {
            _resizeTimeLeft = 500;
            if (_resizing)
                return;

            _resizing = true;
            NoTearFlowLayoutPanelObjects.Visible = false;
            NoTearFlowLayoutPanelObject.Visible = false;
            NoTearFlowLayoutPanelMario.Visible = false;
            if (_mapManager != null && _mapManager.IsLoaded)
                _mapManager.Visible = false;
            if (_modelManager != null && _modelManager.IsLoaded)
                _modelManager.Visible = false;
            await Task.Run(() =>
            {
                while (_resizeTimeLeft > 0)
                {
                    Task.Delay(100).Wait();
                    _resizeTimeLeft -= 100;
                }
            });
            NoTearFlowLayoutPanelObjects.Visible = true;
            NoTearFlowLayoutPanelObject.Visible = true;
            NoTearFlowLayoutPanelMario.Visible = true;
            if (_mapManager != null && _mapManager.IsLoaded)
                _mapManager.Visible = true;
            if (_modelManager != null && _modelManager.IsLoaded)
                _modelManager.Visible = true;

            _resizing = false;
        }

        private async void glControlMap_Load(object sender, EventArgs e)
        {
            await Task.Run(() => {
                while (_mapManager == null)
                {
                    Task.Delay(1).Wait();
                }
            });
            _mapManager.Load();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Config.Stream.IsRunning)
            {
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
                while (_modelManager == null)
                {
                    Task.Delay(1).Wait();
                }
            });
            _modelManager.Load();
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

            if (selectedTabPage == tabPageObjects)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerObject"] as SplitContainer;
            else if (selectedTabPage == tabPageMario)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerMario"] as SplitContainer;
            else if (selectedTabPage == tabPageHud)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerHud"] as SplitContainer;
            else if (selectedTabPage == tabPageCamera)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerCamera"] as SplitContainer;
            else if (selectedTabPage == tabPageTriangles)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerTriangles"] as SplitContainer;
            else if (selectedTabPage == tabPageInput)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerInput"] as SplitContainer;
            else if (selectedTabPage == tabPageFile)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerFile"] as SplitContainer;
            else if (selectedTabPage == tabPageDebug)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerDebug"] as SplitContainer;
            else if (selectedTabPage == tabPageMap)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerMap"] as SplitContainer;
            else if (selectedTabPage == tabPageModel)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerModel"] as SplitContainer;
            else if (selectedTabPage == tabPageHacks)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerHacks"] as SplitContainer;
            else if (selectedTabPage == tabPageCamHack)
                selectedTabSplitContainer = selectedTabPage.Controls["splitContainerCamHack"] as SplitContainer;
        
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

        private void buttonReadOnly_Click(object sender, EventArgs e)
        {
            Config.Stream.Readonly = !Config.Stream.Readonly;
            buttonReadOnly.Text = Config.Stream.Readonly ? "Switch to Read-Write" : "Switch to Read-Only";
            Config.Stream.ShowWarning = false;
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

            NoTearFlowLayoutPanelObjects.Visible = false;
            _objectSlotManager.ChangeSlotSize(trackBarObjSlotSize.Value);
            NoTearFlowLayoutPanelObjects.Visible = true;
            _objSlotResizing = false;
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
