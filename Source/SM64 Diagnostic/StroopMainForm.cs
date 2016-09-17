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
using SM64_Diagnostic.Extensions;

namespace SM64_Diagnostic
{
    public partial class StroopMainForm : Form
    {
        const string _version = "v0.2.2";
        ProcessStream _sm64Stream = null;
        Config _config;

        List<WatchVariable> _objectData, _marioData, _cameraData, _hudData, _miscData;
        ObjectAssociations _objectAssoc;
        MapAssociations _mapAssoc;
        ScriptParser _scriptParser;
        List<RomHack> _romHacks;

        DataTable _tableOtherData = new DataTable();
        Dictionary<int, DataRow> _otherDataRowAssoc = new Dictionary<int, DataRow>();

        ObjectSlotManager _objectSlotManager;
        DisassemblyManager _disManager;
        MarioManager _marioManager;
        ObjectManager _objectManager;
        MapManager _mapManager;
        OptionsManager _optionsManager;
        ScriptManager _scriptManager;
        DataManager _hudManager;
        MiscManager _miscManager;
        CameraManager _cameraManager;
        HackManager _hackManager;

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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

            var slotManagerGui = new ObjectSlotManagerGui();

            // Read configuration
            _config = XmlConfigParser.OpenConfig(@"Config/Config.xml");
            _miscData = XmlConfigParser.OpenMiscData(@"Config/MiscData.xml");
            _objectData = XmlConfigParser.OpenObjectData(@"Config/ObjectData.xml");
            _objectAssoc = XmlConfigParser.OpenObjectAssoc(@"Config/ObjectAssociations.xml", slotManagerGui);
            _marioData = XmlConfigParser.OpenMarioData(_config, @"Config/MarioData.xml");
            _cameraData = XmlConfigParser.OpenCameraData(_config, @"Config/CameraData.xml");
            _hudData = XmlConfigParser.OpenHudData(_config, @"Config/HudData.xml");
            _mapAssoc = XmlConfigParser.OpenMapAssoc(@"Config/MapAssociations.xml");
            _scriptParser = XmlConfigParser.OpenScripts(@"Config/Scripts.xml");
            _romHacks = XmlConfigParser.OpenHacks(@"Config/Hacks.xml");

            _sm64Stream = new ProcessStream(_config);
            _sm64Stream.OnUpdate += OnUpdate;

            _disManager = new DisassemblyManager(_config, this, richTextBoxDissasembly, maskedTextBoxDisStart, _sm64Stream, buttonDisGo);
            _scriptManager = new ScriptManager(_sm64Stream, _scriptParser, checkBoxUseRomHack);
            _hackManager = new HackManager(_sm64Stream, _romHacks, checkedListBoxRomHacks);

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
            _mapManager = new MapManager(_sm64Stream, _config, _mapAssoc, _objectAssoc, mapGui);

            _marioManager = new MarioManager(_sm64Stream, _config, _marioData, panelMarioBorder, flowLayoutPanelMario, _mapManager);
            _hudManager = new DataManager(_sm64Stream, _config, _hudData, flowLayoutPanelHud);
            _miscManager = new MiscManager(_sm64Stream, _config, _miscData, flowLayoutPanelMisc, groupBoxPuController);
            _cameraManager = new CameraManager(_sm64Stream, _config, _cameraData, panelCameraBorder, flowLayoutPanelCamera);

            // Create object manager
            var objectGui = new ObjectDataGui();
            objectGui.ObjectBorderPanel = panelObjectBorder;
            objectGui.ObjectFlowLayout = flowLayoutPanelObject;
            objectGui.ObjectImagePictureBox = pictureBoxObject;
            objectGui.ObjAddressLabelValue = labelObjAddValue;
            objectGui.ObjAddressLabel = labelObjAdd;
            objectGui.ObjBehaviorLabel = labelObjBhvValue;
            objectGui.ObjectNameTextBox = textBoxObjName;
            objectGui.ObjSlotIndexLabel = labelObjSlotIndValue;
            objectGui.ObjSlotPositionLabel = labelObjSlotPosValue;
            objectGui.CloneButton = buttonObjClone;
            objectGui.MoveMarioToButton = buttonObjGoTo;
            objectGui.MoveToMarioButton = buttonObjRetrieve;
            objectGui.UnloadButton = buttonObjUnload;
            _objectManager = new ObjectManager(_sm64Stream, _config, _objectAssoc, _objectData, objectGui);

            // Create options manager
            var optionGui = new OptionsGui();
            optionGui.CheckBoxStartFromOne = checkBoxStartSlotIndexOne;
            _optionsManager = new OptionsManager(optionGui, _config);

            // Create Object Slots
            slotManagerGui.TabControl = tabControlMain;
            slotManagerGui.LockLabelsCheckbox = checkBoxObjLockLabels;
            slotManagerGui.MapObjectToggleModeComboBox = comboBoxMapToggleMode;
            slotManagerGui.FlowLayoutContainer = flowLayoutPanelObjects;
            _objectSlotManager = new ObjectSlotManager(_sm64Stream, _config, _objectAssoc, _objectManager, slotManagerGui, _mapManager, _miscManager);

            // Add SortMethods
            foreach (var sm in Enum.GetValues(typeof(ObjectSlotManager.SortMethodType)))
                comboBoxSortMethod.Items.Add(sm);

            // Use default slot sort method
            comboBoxSortMethod.SelectedIndex = 0;

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
            // Why the f--- I named this a resort list? I will never know
            // Note to self: Re-Sort not resort
            List<Process> resortList = new List<Process>();
            foreach (Process p in AvailableProcesses)
            {
                if (!p.ProcessName.ToLower().Contains(_config.ProcessName.ToLower()))
                    continue;

                resortList.Add(p);
            }
            return resortList;
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            _marioManager.Update(tabControlMain.SelectedTab == tabPageMario);
            _cameraManager.Update(tabControlMain.SelectedTab == tabPageCamera);
            _hudManager.Update(tabControlMain.SelectedTab == tabPageHud);
            _miscManager.Update(tabControlMain.SelectedTab == tabPageMisc);
            _mapManager?.Update();
            UpdateMemoryValues();
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

        private void UpdateMemoryValues()
        {
            for (int i = 0; i < _miscData.Count; i++)
            {
                WatchVariable watchVar = _miscData[i];
                // Make sure cell is not being edited
                if (dataGridViewExpressions.IsCurrentCellInEditMode
                    && dataGridViewExpressions.SelectedRows[0].Index
                    == _tableOtherData.Rows.IndexOf(_otherDataRowAssoc[i]))
                    continue;

                // Get data
                byte[] data = new byte[watchVar.GetByteCount()];
                _sm64Stream.ReadProcessMemory((int)(watchVar.Address & 0x0FFFFFFF), data, watchVar.AbsoluteAddressing);
                _otherDataRowAssoc[i]["Value"] = String.Join("", data);
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
                    : watchVar.Address + _config.RamStartAddress);

                // Add variable to lists
                int newIndex = _miscData.Count;
                _miscData.Add(watchVar);
                _otherDataRowAssoc.Add(newIndex, row);

                XmlConfigParser.AddWatchVariableOtherData(watchVar);
            }
        }

        private void comboBoxSortMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_objectSlotManager == null)
                return;

            _objectSlotManager.SortMethod = (ObjectSlotManager.SortMethodType) comboBoxSortMethod.SelectedItem;
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
            _sm64Stream.WriteRam(new byte[] { 0 }, _config.Debug.Toggle);
        }

        private void radioButtonDbgObjCnt_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, _config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 0 }, _config.Debug.Setting);
        }

        private void radioButtonDbgChkInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, _config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 1 }, _config.Debug.Setting);
        }

        private void radioButtonDbgMapInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, _config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 2 }, _config.Debug.Setting);
        }

        private void radioButtonDbgStgInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, _config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 3 }, _config.Debug.Setting);
        }

        private void checkBoxMoveCamWithPu_CheckedChanged(object sender, EventArgs e)
        {
            _config.MoveCameraWithPu = checkBoxMoveCamWithPu.Checked;
        }

        private void buttonPuConHome_Click(object sender, EventArgs e)
        {

        }

        private void radioButtonDbgFxInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _sm64Stream.WriteRam(new byte[] { 1 }, _config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 4 }, _config.Debug.Setting);
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
            _sm64Stream.WriteRam(new byte[] { 1 }, _config.Debug.Toggle);

            // Set mode
            _sm64Stream.WriteRam(new byte[] { 5 }, _config.Debug.Setting);
        }

        #endregion

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab == tabPageMap)
            {
                _objectSlotManager.UpdateSelectedObjectSlots();
                comboBoxMapToggleMode.Visible = true;
                if (_splitterIsExpanded)
                    splitContainerMain.SplitterDistance = splitContainerMain.Height;
            }
            else
            {
                _objectSlotManager.SetAllSelectedObjectSlots();
                comboBoxMapToggleMode.Visible = false;
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
