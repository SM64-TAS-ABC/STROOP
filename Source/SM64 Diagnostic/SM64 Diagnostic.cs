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
using OpenTK;

namespace SM64_Diagnostic
{
    public partial class SM64DiagnosticForm : Form
    {
        ProcessStream _sm64Stream = null;
        Config _config;

        Dictionary<int, WatchVariable> _otherData;
        List<WatchVariable> _objectData, _marioData;
        ObjectAssociations _objectAssoc = new ObjectAssociations();
        MapAssociations _mapAssoc = new MapAssociations();

        DataTable _tableOtherData = new DataTable();
        Dictionary<int, DataRow> _otherDataRowAssoc = new Dictionary<int, DataRow>();

        ObjectSlotManager _objectSlotManager;
        DisassemblyManager _disManager;
        MarioManager _marioManager;
        ObjectManager _objectManager;
        MapManager _mapManager;
        OptionsManager _optionsManager;

        bool _resizing = true;
        int _resizeTimeLeft = 0;

        public SM64DiagnosticForm()
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

        private void Form1_Load(object sender, EventArgs e)
        {
           // Temp: Remove "Other" tab
#if RELEASE
            tabControlMain.TabPages.Remove(tabPageOther);
#endif

            // Read configuration
            _config = XmlConfigParser.OpenConfig(@"Config/Config.xml");
            _otherData = XmlConfigParser.OpenOtherData(@"Config/OtherData.xml");
            _objectData = XmlConfigParser.OpenObjectData(@"Config/ObjectData.xml");
            _objectAssoc = XmlConfigParser.OpenObjectAssoc(@"Config/ObjectAssociations.xml");
            _marioData = XmlConfigParser.OpenMarioData(_config, @"Config/MarioData.xml");
            _mapAssoc = XmlConfigParser.OpenMapAssoc(@"Config/MapAssociations.xml");

            _sm64Stream = new ProcessStream(_config);
            _sm64Stream.OnUpdate += OnUpdate;

            _disManager = new DisassemblyManager(_config, this, richTextBoxDissasembly, maskedTextBoxDisStart, _sm64Stream, buttonDisGo);

            // Create map manager
            MapGui mapGui = new MapGui();
            mapGui.GLControl = glControlMap;
            mapGui.MapIdLabel = labelMapId;
            mapGui.MapNameLabel = labelMapName;
            mapGui.MapSubNameLabel = labelMapSubName;
            mapGui.PuValueLabel = labelMapPuValue;
            mapGui.QpuValueLabel = labelMapQpuValue;
            _mapManager = new MapManager(_sm64Stream, _config, _mapAssoc, mapGui);

            _marioManager = new MarioManager(_sm64Stream, _config, _marioData, panelMarioBorder, flowLayoutPanelMario, _mapManager);

            // Create object manager
            var objectGui = new ObjectDataGui();
            objectGui.ObjectBorderPanel = panelObjectBorder;
            objectGui.ObjectFlowLayout = flowLayoutPanelObject;
            objectGui.ObjectImagePictureBox = pictureBoxObject;
            objectGui.ObjAddressLabel = labelObjAddValue;
            objectGui.ObjBehaviorLabel = labelObjBhvValue;
            objectGui.ObjectNameLabel = labelObjName;
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
            _objectSlotManager = new ObjectSlotManager(_sm64Stream, _config, _objectAssoc, _objectManager, pictureBoxTrash, pictureBoxObjClone, tabControlMain);
            _objectSlotManager.AddToControls(flowLayoutPanelObjects.Controls);

            // Add SortMethods
            foreach (var sm in Enum.GetValues(typeof(ObjectSlotManager.SortMethodType)))
                comboBoxSortMethod.Items.Add(sm);

            // Use default slot sort method
            comboBoxSortMethod.SelectedIndex = 0;

            SetupViews();

            _resizing = false;

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
            _mapManager?.Update();
            UpdateMemoryValues();
        }

        private void SetupViews()
        {
            // Mario Image
            pictureBoxMario.Image = _objectAssoc.MarioImage;
            panelMarioBorder.BackColor = _objectAssoc.MarioColor;
            pictureBoxMario.BackColor = ControlPaint.Light(ControlPaint.Light(ControlPaint.Light(_objectAssoc.MarioColor)));

            // Setup data columns
            var nameColumn = new DataColumn("Name");
            nameColumn.ReadOnly = true;
            nameColumn.DataType = typeof(string);
            _tableOtherData.Columns.Add(nameColumn);
            _tableOtherData.Columns.Add("Type", typeof(string));
            _tableOtherData.Columns.Add("Value", typeof(string));
            _tableOtherData.Columns.Add("Address", typeof(uint));

            // Setup grid view
            dataGridViewOther.DataSource = _tableOtherData;

            // Setup other data table
            for (int index = 0; index < _otherData.Count; index++)
            {
                var watchVar = _otherData[index];
                var row = _tableOtherData.Rows.Add(watchVar.Name, watchVar.Type.ToString(), "", watchVar.Address);
                _otherDataRowAssoc.Add(index, row);
            }
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
            for (int i = 0; i < _otherData.Count; i++)
            {
                WatchVariable watchVar = _otherData[i];
                // Make sure cell is not being edited
                if (dataGridViewOther.IsCurrentCellInEditMode
                    && dataGridViewOther.SelectedRows[0].Index
                    == _tableOtherData.Rows.IndexOf(_otherDataRowAssoc[i]))
                    continue;

                // Get data
                byte[] data = new byte[WatchVariableParsingExtensions.GetByteCount(watchVar)];
                _sm64Stream.ReadProcessMemory((int)(watchVar.Address & 0x0FFFFFFF), data, watchVar.AbsoluteAddressing);
                _otherDataRowAssoc[i]["Value"] = String.Join("", data);
            }
        }

        private void buttonOtherModify_Click(object sender, EventArgs e)
        {
            var row = _tableOtherData.Rows[dataGridViewOther.SelectedRows[0].Index];
            int assoc = _otherDataRowAssoc.FirstOrDefault(v => v.Value == row).Key;

            var modifyVar = new ModifyAddWatchVariableForm(_otherData[assoc]);
            modifyVar.ShowDialog();
        }

        private void buttonOtherDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete selected variables?", "Delete Variables", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == DialogResult.Yes)
            {
                // Find indexes to delete
                var deleteVars = new List<int>();
                foreach (DataGridViewRow selectedRow in dataGridViewOther.SelectedRows)
                {
                    var row = _tableOtherData.Rows[selectedRow.Index];
                    int assoc = _otherDataRowAssoc.FirstOrDefault(v => v.Value == row).Key;
                    deleteVars.Add(assoc);
                }

                // Delete rows
                foreach (int i in deleteVars)
                {
                    DataRow row = _otherDataRowAssoc[i];
                    _otherData.Remove(i);
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

            var row = _tableOtherData.Rows[dataGridViewOther.SelectedRows[0].Index];
            int assoc = _otherDataRowAssoc.FirstOrDefault(v => v.Value == row).Key;

            var modifyVar = new ModifyAddWatchVariableForm(_otherData[assoc]);
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
                int newIndex = _otherData.Count;
                _otherData.Add(newIndex, watchVar);
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
            if (_mapManager != null)
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
            if (_mapManager != null)
                _mapManager.Visible = true;

            _resizing = false;
        }

        private void glControlMap_Load(object sender, EventArgs e)
        {
            _mapManager.Load();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            throw new Exception("User is a dumb@$$");
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
