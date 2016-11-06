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

namespace SM64_Diagnostic
{
    public partial class StroopMainForm : Form
    {
        const string _version = "v0.2.6";
        ProcessStream _sm64Stream = null;

        ObjectSlotManagerGui _slotManagerGui = new ObjectSlotManagerGui();
        List<WatchVariable> _objectData, _marioData, _cameraData, _hudData, _miscData, _triangleData;
        ObjectAssociations _objectAssoc;
        MapAssociations _mapAssoc;
        ScriptParser _scriptParser;
        List<RomHack> _romHacks;

        DataTable _tableOtherData = new DataTable();
        Dictionary<int, DataRow> _otherDataRowAssoc = new Dictionary<int, DataRow>();

        ObjectSlotsManager _objectSlotManager;
        DisassemblyManager _disManager;
        MarioManager _marioManager;
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

        bool _resizing = true, _objSlotResizing = false;
        int _resizeTimeLeft = 0, _resizeObjSlotTime = 0;

        bool _splitterIsExpanded = false;
        static int _defaultSplitValue;

        public StroopMainForm()
        {
            InitializeComponent();
        }

        private void AttachToProcess(Process process)
        {
            if (!_sm64Stream.SwitchProcess(process))
            {
                MessageBox.Show("Could not attach to process!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void comboBoxProcessSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProcessSelection.SelectedItem == null)
                return;

            AttachToProcess(((ProcessSelection)(comboBoxProcessSelection.SelectedItem)).Process);
        }

        private void StroopMainForm_Load(object sender, EventArgs e)
        {
            // Temp: Remove "Other" tab
#if RELEASE
            tabControlMain.TabPages.Remove(tabPageExpressions);
#endif

            _sm64Stream = new ProcessStream();
            _sm64Stream.OnUpdate += OnUpdate;

            _disManager = new DisassemblyManager(this, richTextBoxDissasembly, maskedTextBoxDisStart, _sm64Stream, buttonDisGo);
            _scriptManager = new ScriptManager(_sm64Stream, _scriptParser, checkBoxUseRomHack);
            _hackManager = new HackManager(_sm64Stream, _romHacks, checkedListBoxHacks);

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
            mapGui.MapShowObjects = checkBoxMapShowObj;
            mapGui.MapShowHolp = checkBoxMapShowHolp;
            mapGui.MapShowCamera = checkBoxMapShowCamera;
            mapGui.MapShowFloorTriangle = checkBoxMapShowFloor;
            _mapManager = new MapManager(_sm64Stream, _mapAssoc, _objectAssoc, mapGui);

            _marioManager = new MarioManager(_sm64Stream, _marioData, panelMarioBorder, flowLayoutPanelMario, _mapManager);
            _hudManager = new HudManager(_sm64Stream, _hudData, tabPageHud);
            _miscManager = new MiscManager(_sm64Stream, _miscData, flowLayoutPanelMisc, groupBoxPuController);
            _cameraManager = new CameraManager(_sm64Stream, _cameraData, panelCameraBorder, flowLayoutPanelCamera);
            _triangleManager = new TriangleManager(_sm64Stream, tabPageTriangles, _triangleData);
            _debugManager = new DebugManager();

            // Create object manager
            var objectGui = new ObjectDataGui()
            {
                ObjectBorderPanel = panelObjectBorder,
                ObjectFlowLayout = flowLayoutPanelObject,
                ObjectImagePictureBox = pictureBoxObject,
                ObjAddressLabelValue = labelObjAddValue,
                ObjAddressLabel = labelObjAdd,
                ObjBehaviorLabel = labelObjBhvValue,
                ObjectNameTextBox = textBoxObjName,
                ObjSlotIndexLabel = labelObjSlotIndValue,
                ObjSlotPositionLabel = labelObjSlotPosValue,
                CloneButton = buttonObjClone,
                MoveMarioToButton = buttonObjGoTo,
                MoveToMarioButton = buttonObjRetrieve,
                UnloadButton = buttonObjUnload
            };
            _objectManager = new ObjectManager(_sm64Stream, _objectAssoc, _objectData, objectGui);

            // Create options manager
            var optionGui = new OptionsGui();
            optionGui.CheckBoxStartFromOne = checkBoxStartSlotIndexOne;
            _optionsManager = new OptionsManager(optionGui);

            // Create Object Slots
            _slotManagerGui.TabControl = tabControlMain;
            _slotManagerGui.LockLabelsCheckbox = checkBoxObjLockLabels;
            _slotManagerGui.MapObjectToggleModeComboBox = comboBoxMapToggleMode;
            _slotManagerGui.FlowLayoutContainer = flowLayoutPanelObjects;
            _slotManagerGui.SortMethodComboBox = comboBoxSortMethod;
            _slotManagerGui.LabelMethodComboBox = comboBoxLabelMethod;
            _objectSlotManager = new ObjectSlotsManager(_sm64Stream, _objectAssoc, _objectManager, _slotManagerGui, _mapManager, _miscManager);

            SetupViews();

            _resizing = false;
            _defaultSplitValue = splitContainerMain.SplitterDistance;
            labelVersionNumber.Text = _version;

            // Load process
            var processes = GetAvailableProcesses();
            if (processes.Count == 1)
                if (MessageBox.Show(String.Format("Found process \"{0}\". Connect?", processes[0].ProcessName),
                    "Process Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var processSelect = new ProcessSelection(processes[0]);
                    comboBoxProcessSelection.Items.Add(processSelect);
                    comboBoxProcessSelection.SelectedIndex = 0;
                }
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
            loadingForm.UpdateStatus("Loading HUD Data", statusNum++);
            _triangleData = XmlConfigParser.OpenWatchVarData(@"Config/TrianglesData.xml", "TrianglesDataSchema.xsd", "triangleOffset");
            loadingForm.UpdateStatus("Loading Triangles Data", statusNum++);
            _hudData = XmlConfigParser.OpenWatchVarData(@"Config/HudData.xml", "HudDataSchema.xsd");
            loadingForm.UpdateStatus("Loading Map Associations", statusNum++);
            _mapAssoc = XmlConfigParser.OpenMapAssoc(@"Config/MapAssociations.xml");
            loadingForm.UpdateStatus("Loading Scripts", statusNum++);
            _scriptParser = XmlConfigParser.OpenScripts(@"Config/Scripts.xml");
            loadingForm.UpdateStatus("Loading Hacks", statusNum++);
            _romHacks = XmlConfigParser.OpenHacks(@"Config/Hacks.xml");

            loadingForm.UpdateStatus("Finishing", statusNum);
        }

        private void comboBoxProcessSelection_DropDown(object sender, EventArgs e)
        {
            comboBoxProcessSelection.Items.Clear();
            buttonPauseResume.Text = "Pause";

            foreach (Process p in GetAvailableProcesses())
            {
                comboBoxProcessSelection.Items.Add(new ProcessSelection(p));
            }
        }

        private List<Process> GetAvailableProcesses()
        {
            var AvailableProcesses = Process.GetProcesses();
            List<Process> resortList = new List<Process>();
            foreach (Process p in AvailableProcesses)
            {
                if (!p.ProcessName.ToLower().Contains(Config.ProcessName.ToLower()))
                    continue;

                resortList.Add(p);
            }
            return resortList;
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            _objectSlotManager.Update();
            _objectManager.Update(tabControlMain.SelectedTab == tabPageObjects);
            _marioManager.Update(tabControlMain.SelectedTab == tabPageMario);
            _cameraManager.Update(tabControlMain.SelectedTab == tabPageCamera);
            _hudManager.Update(tabControlMain.SelectedTab == tabPageHud);
            _miscManager.Update(tabControlMain.SelectedTab == tabPageMisc);
            _triangleManager.Update(tabControlMain.SelectedTab == tabPageTriangles);
            _mapManager?.Update();
            _scriptManager.Update();
            _hackManager.Update();
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
                var row = _tableOtherData.Rows.Add(watchVar.Name, watchVar.Type.ToString(), "", watchVar.Address);
                _otherDataRowAssoc.Add(index, row);
            }

#if !DEBUG
            tabControlMain.TabPages.Remove(tabPageExpressions);
            tabControlMain.TabPages.Remove(tabPageStars);
#endif
        }

        private void buttonPauseResume_Click(object sender, EventArgs e)
        {
            if (_sm64Stream.IsSuspended)
            {
                _sm64Stream.Resume();
                buttonPauseResume.Text = "Pause";
            }
            else
            {
                _sm64Stream.Suspend();
                buttonPauseResume.Text = "Resume";
            }
        }

        private void buttonOtherModify_Click(object sender, EventArgs e)
        {
            var row = _tableOtherData.Rows[dataGridViewExpressions.SelectedRows[0].Index];
            int assoc = _otherDataRowAssoc.FirstOrDefault(v => v.Value == row).Key;

            var modifyVar = new ModifyAddWatchVariableForm(_miscData[assoc]);
            modifyVar.ShowDialog();
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

            var modifyVar = new ModifyAddWatchVariableForm(_miscData[assoc]);
            modifyVar.ShowDialog();
        }

        private void buttonOtherAdd_Click(object sender, EventArgs e)
        {
            var modifyVar = new ModifyAddWatchVariableForm();
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
            }
        }

        private async void flowLayoutPanelObjects_Resize(object sender, EventArgs e)
        {
            _resizeTimeLeft = 500;
            if (_resizing)
                return;

            _resizing = true;
            flowLayoutPanelObjects.Visible = false;
            flowLayoutPanelObject.Visible = false;
            flowLayoutPanelMario.Visible = false;
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
            flowLayoutPanelObjects.Visible = true;
            flowLayoutPanelObject.Visible = true;
            flowLayoutPanelMario.Visible = true;
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

        private void buttonMapExpand_Click(object sender, EventArgs e)
        {
            if (!_splitterIsExpanded)
            {
                buttonMapExpand.Text = "Minimize Map";
                splitContainerMain.SplitterDistance = splitContainerMain.Height;
            }
            else
            {
                buttonMapExpand.Text = "Expand Map";
                splitContainerMain.SplitterDistance = _defaultSplitValue;
            }

            _splitterIsExpanded = !_splitterIsExpanded;
        }

        #region Debug Tab

        private void radioButtonDbgOff_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug off
            _sm64Stream.WriteRam(new byte[] { 0 }, Config.Debug.Toggle);
        }

        private void radioButtonDbgObjCnt_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 0 }, Config.Debug.Setting);
        }

        private void radioButtonDbgChkInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 1 }, Config.Debug.Setting);
        }

        private void radioButtonDbgMapInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 2 }, Config.Debug.Setting);
        }

        private void radioButtonDbgStgInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 3 }, Config.Debug.Setting);
        }

        private void checkBoxMoveCamWithPu_CheckedChanged(object sender, EventArgs e)
        {
            Config.MoveCameraWithPu = checkBoxMoveCamWithPu.Checked;
        }

        private void checkBoxUseOverlays_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowOverlays = checkBoxUseOverlays.Checked;
        }

        private void radioButtonDbgFxInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 4 }, Config.Debug.Setting);
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

            flowLayoutPanelObjects.Visible = false;
            _objectSlotManager.ChangeSlotSize(trackBarObjSlotSize.Value);
            flowLayoutPanelObjects.Visible = true;
            _objSlotResizing = false;
        }

        private void radioButtonDbgEnemyInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 5 }, Config.Debug.Setting);
        }

        #endregion

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab == tabPageMap)
            {
                _objectSlotManager.UpdateSelectedObjectSlots();
                comboBoxMapToggleMode.Visible = true;
                labelToggleMode.Visible = true;
                if (_splitterIsExpanded)
                    splitContainerMain.SplitterDistance = splitContainerMain.Height;
            }
            else
            {
                _objectSlotManager.SetAllSelectedObjectSlots();
                comboBoxMapToggleMode.Visible = false;
                labelToggleMode.Visible = false;
                if (_splitterIsExpanded)
                    splitContainerMain.SplitterDistance = _defaultSplitValue;
            }
        }

        private void tabControlMain_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            Point clientPoint = tabControlMain.PointToClient(new Point(e.X, e.Y));

            for (int i = 0; i < tabControlMain.TabCount; i++)
            {
                if (tabControlMain.GetTabRect(i).Contains(clientPoint) && tabControlMain.SelectedIndex != i)
                {
                    tabControlMain.SelectedIndex = i;
                }
            }
        }

    }
}
