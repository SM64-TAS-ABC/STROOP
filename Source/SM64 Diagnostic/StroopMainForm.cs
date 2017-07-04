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
        ProcessStream _sm64Stream = null;

        ObjectSlotManagerGui _slotManagerGui = new ObjectSlotManagerGui();
        ControllerImageGui _controllerImageGui = new ControllerImageGui();
        List<WatchVariable> _objectData, _marioData, _cameraData, _hudData, _miscData, _triangleData, 
            _actionsData, _waterData, _controllerData, _fileData, _levelData, _camHackData;
        ObjectAssociations _objectAssoc;
        MapAssociations _mapAssoc;
        ScriptParser _scriptParser;
        List<RomHack> _romHacks;

        DataTable _tableOtherData = new DataTable();
        Dictionary<int, DataRow> _otherDataRowAssoc = new Dictionary<int, DataRow>();

        ObjectSlotsManager _objectSlotManager;
        DisassemblyManager _disManager;
        MarioManager _marioManager;
        ControllerManager _controllerManager;
        ActionsManager _actionsManager;
        ObjectManager _objectManager;
        MapManager _mapManager;
        OptionsManager _optionsManager;
        ScriptManager _scriptManager;
        HudManager _hudManager;
        MiscManager _miscManager;
        CameraManager _cameraManager;
        HackManager _hackManager;
        TriangleManager _triangleManager;
        DebugManager _debugManager;
        CamHackManager _cameraHackManager;
        DataManager _waterManager, _fileManager, _levelManager;
        PuManager _puManager;

        TabPage _previousTabPage;

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

            return _sm64Stream.SwitchProcess(process, emulators[0]);
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

            _sm64Stream = new ProcessStream();
            _sm64Stream.OnUpdate += OnUpdate;
            _sm64Stream.FpsUpdated += _sm64Stream_FpsUpdated;
            _sm64Stream.OnDisconnect += _sm64Stream_OnDisconnect;
            _sm64Stream.WarnReadonlyOff += _sm64Stream_WarnReadonlyOff;
            _sm64Stream.OnClose += _sm64Stream_OnClose;

            currentContext.DisassemblyManager = _disManager = new DisassemblyManager(_sm64Stream, tabPageDisassembly);
            currentContext.ScriptManager = _scriptManager = new ScriptManager(_sm64Stream, _scriptParser, checkBoxUseRomHack);
            currentContext.HackManager = _hackManager = new HackManager(_sm64Stream, _romHacks, _objectAssoc.SpawnHacks, tabPageHacks);

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
            currentContext.MapManager = _mapManager = new MapManager(_sm64Stream, _mapAssoc, _objectAssoc, mapGui);

            currentContext.ActionsManager = _actionsManager = new ActionsManager(_sm64Stream, _actionsData, noTearFlowLayoutPanelActions);
            currentContext.WaterManager = _waterManager = new DataManager(_sm64Stream, _waterData, noTearFlowLayoutPanelWater);
            currentContext.ControllerManager = _controllerManager = new ControllerManager(_sm64Stream, _controllerData, tabPageController, NoTearFlowLayoutPanelController);
            currentContext.MarioManager = _marioManager = new MarioManager(_sm64Stream, _marioData, tabPageMario, NoTearFlowLayoutPanelMario, _mapManager);
            currentContext.HudManager = _hudManager = new HudManager(_sm64Stream, _hudData, tabPageHud);
            currentContext.MiscManager = _miscManager = new MiscManager(_sm64Stream, _miscData, NoTearFlowLayoutPanelMisc);
            currentContext.CameraManager = _cameraManager = new CameraManager(_sm64Stream, _cameraData, tabPageCamera, NoTearFlowLayoutPanelCamera);
            currentContext.TriangleManager = _triangleManager = new TriangleManager(_sm64Stream, tabPageTriangles, _triangleData);
            currentContext.DebugManager = _debugManager = new DebugManager(_sm64Stream, tabPageDebug);
            currentContext.PuManager = _puManager = new PuManager(_sm64Stream, groupBoxPuController);
            currentContext.FileManager = _fileManager = new DataManager(_sm64Stream, _fileData, noTearFlowLayoutPanelFile);
            currentContext.LevelManager = _levelManager = new DataManager(_sm64Stream, _levelData, noTearFlowLayoutPanelLevel);
            currentContext.CameraHackManager = _cameraHackManager = new CamHackManager(_sm64Stream, _camHackData, tabPageCamHack, noTearFlowLayoutPanelCamHack);
            currentContext.ObjectManager = _objectManager = new ObjectManager(_sm64Stream, _objectAssoc, _objectData, tabPageObjects, NoTearFlowLayoutPanelObject);
            _previousTabPage = tabPageObjects;

            // Create Object Slots
            _slotManagerGui.TabControl = tabControlMain;
            _slotManagerGui.LockLabelsCheckbox = checkBoxObjLockLabels;
            _slotManagerGui.MapObjectToggleModeComboBox = comboBoxMapToggleMode;
            _slotManagerGui.FlowLayoutContainer = NoTearFlowLayoutPanelObjects;
            _slotManagerGui.SortMethodComboBox = comboBoxSortMethod;
            _slotManagerGui.LabelMethodComboBox = comboBoxLabelMethod;
            currentContext.ObjectSlotManager = _objectSlotManager = new ObjectSlotsManager(_sm64Stream, _objectAssoc, _objectManager, _slotManagerGui, _mapManager, _miscManager, tabControlMain);

            SetupViews();

            _resizing = false;
            labelVersionNumber.Text = _version;

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
                        _sm64Stream.Readonly = false;
                        _sm64Stream.ShowWarning = false;
                        buttonReadOnly.Text = "Switch to Read-Only";
                        break;

                    case DialogResult.No:
                        _sm64Stream.ShowWarning = false;
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
            _objectData = XmlConfigParser.OpenWatchVarData(@"Config/ObjectData.xml", "ObjectDataSchema.xsd", "objectOffset");
            loadingForm.UpdateStatus("Loading Object Associations", statusNum++);
            _objectAssoc = XmlConfigParser.OpenObjectAssoc(@"Config/ObjectAssociations.xml", _slotManagerGui);
            loadingForm.UpdateStatus("Loading Mario Data", statusNum++);
            _marioData = XmlConfigParser.OpenWatchVarData(@"Config/MarioData.xml", "MarioDataSchema.xsd", "marioOffset");
            loadingForm.UpdateStatus("Loading Camera Data", statusNum++);
            _cameraData = XmlConfigParser.OpenWatchVarData(@"Config/CameraData.xml", "CameraDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Actions Data", statusNum++);
            _actionsData = XmlConfigParser.OpenWatchVarData(@"Config/ActionsData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Water Data", statusNum++);
            _waterData = XmlConfigParser.OpenWatchVarData(@"Config/WaterData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Controller Data", statusNum++);
            _controllerData = XmlConfigParser.OpenWatchVarData(@"Config/ControllerData.xml", "MiscDataSchema.xsd", "marioOffset");
            loadingForm.UpdateStatus("Loading Controller Image Associations", statusNum++);
            XmlConfigParser.OpenControllerImageAssoc(@"Config/ControllerImageAssociations.xml", _controllerImageGui);
            loadingForm.UpdateStatus("Loading File Data", statusNum++);
            _fileData = XmlConfigParser.OpenWatchVarData(@"Config/FileData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Level Data", statusNum++);
            _levelData = XmlConfigParser.OpenWatchVarData(@"Config/LevelData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Camera Hack Data", statusNum++);
            _camHackData = XmlConfigParser.OpenWatchVarData(@"Config/CamHackData.xml", "MiscDataSchema.xsd");
            loadingForm.UpdateStatus("Loading HUD Data", statusNum++);
            _triangleData = XmlConfigParser.OpenWatchVarData(@"Config/TrianglesData.xml", "TrianglesDataSchema.xsd", "triangleOffset");
            loadingForm.UpdateStatus("Loading Triangles Data", statusNum++);
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
                _controllerManager.Update(tabControlMain.SelectedTab == tabPageController);
                _fileManager.Update(tabControlMain.SelectedTab == tabPageFile);
                _levelManager.Update(tabControlMain.SelectedTab == tabPageLevel);
                _cameraHackManager.Update(tabControlMain.SelectedTab == tabPageCamHack);
                _miscManager.Update(tabControlMain.SelectedTab == tabPageMisc);
                _triangleManager.Update(tabControlMain.SelectedTab == tabPageTriangles);
                _debugManager.Update(tabControlMain.SelectedTab == tabPageDebug);
                _puManager.Update(tabControlMain.SelectedTab == tabPagePu);
                _mapManager?.Update();
                _scriptManager.Update();
                _hackManager.Update();
            }));
        }

        private void _sm64Stream_FpsUpdated(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                labelFpsCounter.Text = "FPS: " + (int)_sm64Stream.Fps;
            }));
        }

        private void SetupViews()
        {
            // Mario Image
            pictureBoxMario.Image = _objectAssoc.MarioImage;
            panelMarioBorder.BackColor = _objectAssoc.MarioColor;
            pictureBoxMario.BackColor = _objectAssoc.MarioColor.Lighten(0.5);

            // Camera Image
            pictureBoxCamera.Image = _objectAssoc.CameraImage;
            panelCameraBorder.BackColor = _objectAssoc.CameraColor;
            pictureBoxCamera.BackColor = _objectAssoc.CameraColor.Lighten(0.5);

            // Hud Image
            pictureBoxHud.Image = _objectAssoc.HudImage;
            panelHudBorder.BackColor = _objectAssoc.HudColor;
            pictureBoxHud.BackColor = _objectAssoc.HudColor.Lighten(0.5);

            // Debug Image
            pictureBoxDebug.Image = _objectAssoc.DebugImage;
            panelDebugBorder.BackColor = _objectAssoc.DebugColor;
            pictureBoxDebug.BackColor = _objectAssoc.DebugColor.Lighten(0.5);

            // Controller Image
            pictureBoxController.Image = _objectAssoc.ControllerImage;

            // Misc Image
            pictureBoxMisc.Image = _objectAssoc.MiscImage;
            panelMiscBorder.BackColor = _objectAssoc.MiscColor;
            pictureBoxMisc.BackColor = _objectAssoc.MiscColor.Lighten(0.5);

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
                if (watchVar.Special)
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
            if (_sm64Stream.IsRunning)
            {
                _sm64Stream.Stop();
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

        private void buttonShowLeftPanel_Click(object sender, EventArgs e)
        {
        }

        private void buttonShowRightPanel_Click(object sender, EventArgs e)
        {
        }

        private void buttonShowLeftRightPanel_Click(object sender, EventArgs e)
        {
        }

        private void buttonReadOnly_Click(object sender, EventArgs e)
        {
            _sm64Stream.Readonly = !_sm64Stream.Readonly;
            buttonReadOnly.Text = _sm64Stream.Readonly ? "Switch to Read-Write" : "Switch to Read-Only";
            _sm64Stream.ShowWarning = false;
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
            _sm64Stream.SwitchProcess(null, null);
            panelConnect.Size = this.Size;
            panelConnect.Visible = true;
        }

        private void buttonRefreshAndConnect_Click(object sender, EventArgs e)
        {
            buttonRefresh_Click(sender, e);
            buttonConnect_Click(sender, e);
        }

        private void checkBoxStartSlotIndexOne_CheckedChanged(object sender, EventArgs e)
        {
            Config.SlotIndexsFromOne = checkBoxStartSlotIndexOne.Checked;
        }

        private void checkBoxMoveCamWithPu_CheckedChanged(object sender, EventArgs e)
        {
            Config.MoveCameraWithPu = checkBoxMoveCamWithPu.Checked;
        }

        private void checkBoxScaleDiagonalPositionControllerButtons_CheckedChanged(object sender, EventArgs e)
        {
            Config.ScaleDiagonalPositionControllerButtons = checkBoxScaleDiagonalPositionControllerButtons.Checked;
        }

        private void checkBoxDisableActionUpdateWhenCloning_CheckedChanged(object sender, EventArgs e)
        {
            Config.DisableActionUpdateWhenCloning = checkBoxDisableActionUpdateWhenCloning.Checked;
        }

        private void checkBoxNeutralizeTriangleWith21_CheckedChanged(object sender, EventArgs e)
        {
            Config.NeutralizeTriangleWith21 = checkBoxNeutralizeTriangleWith21.Checked;
        }

        private void checkBoxShowOverlayHeldObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayHeldObject = checkBoxShowOverlayHeldObject.Checked;
        }

        private void checkBoxShowOverlayStoodOnObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayStoodOnObject = checkBoxShowOverlayStoodOnObject.Checked;
        }

        private void checkBoxShowOverlayInteractionObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayInteractionObject = checkBoxShowOverlayInteractionObject.Checked;
        }

        private void checkBoxShowOverlayUsedObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayUsedObject = checkBoxShowOverlayUsedObject.Checked;
        }

        private void checkBoxShowOverlayCameraObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayCameraObject = checkBoxShowOverlayCameraObject.Checked;
        }

        private void checkBoxShowOverlayCameraHackObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayCameraHackObject = checkBoxShowOverlayCameraHackObject.Checked;
        }

        private void checkBoxShowOverlayClosestObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayClosestObject = checkBoxShowOverlayClosestObject.Checked;
        }

        private void checkBoxShowOverlayFloorObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayFloorObject = checkBoxShowOverlayFloorObject.Checked;
        }

        private void checkBoxShowOverlayWallObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayWallObject = checkBoxShowOverlayWallObject.Checked;
        }

        private void checkBoxShowOverlayCeilingObject_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlayCeilingObject = checkBoxShowOverlayCeilingObject.Checked;
        }

        private void textBoxGotoAbove_LostFocus(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse((sender as TextBox).Text, out value))
            {
                Config.GotoRetrieve.GotoAboveOffset = value;
            }
            else
            {
                Config.GotoRetrieve.GotoAboveOffset = Config.GotoRetrieve.GotoAboveDefault;
                (sender as TextBox).Text = Config.GotoRetrieve.GotoAboveDefault.ToString();
            }
        }

        private void textBoxGotoInfront_LostFocus(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse((sender as TextBox).Text, out value))
            {
                Config.GotoRetrieve.GotoInfrontOffset = value;
            }
            else
            {
                Config.GotoRetrieve.GotoInfrontOffset = Config.GotoRetrieve.GotoInfrontDefault;
                (sender as TextBox).Text = Config.GotoRetrieve.GotoInfrontDefault.ToString();
            }
        }

        private void textBoxRetrieveAbove_LostFocus(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse((sender as TextBox).Text, out value))
            {
                Config.GotoRetrieve.RetrieveAboveOffset = value;
            }
            else
            {
                Config.GotoRetrieve.RetrieveAboveOffset = Config.GotoRetrieve.RetrieveAboveDefault;
                (sender as TextBox).Text = Config.GotoRetrieve.RetrieveAboveDefault.ToString();
            }
        }

        private void textBoxRetrieveInfront_LostFocus(object sender, EventArgs e)
        {
            float value;
            if (float.TryParse((sender as TextBox).Text, out value))
            {
                Config.GotoRetrieve.RetrieveInfrontOffset = value;
            }
            else
            {
                Config.GotoRetrieve.RetrieveInfrontOffset = Config.GotoRetrieve.RetrieveInfrontDefault;
                (sender as TextBox).Text = Config.GotoRetrieve.RetrieveInfrontDefault.ToString();
            }
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
            if (tabControlMain.SelectedTab == tabPageMap)
            {
                comboBoxMapToggleMode.Visible = true;
                labelToggleMode.Visible = true;
            }
            else
            {
                comboBoxMapToggleMode.Visible = false;
                labelToggleMode.Visible = false;
            }
            
            // TODO remove this once slots automatically update when their selection changes
            if (_previousTabPage == tabPageMap || tabControlMain.SelectedTab == tabPageMap || _previousTabPage == tabPageCamHack || tabControlMain.SelectedTab == tabPageCamHack)
            {
                _objectSlotManager.updateSlotColors();
            }
            _previousTabPage = tabControlMain.SelectedTab;
        }
    }
}
