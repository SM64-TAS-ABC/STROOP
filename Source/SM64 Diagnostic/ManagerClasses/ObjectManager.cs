using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.ManagerClasses
{
    public class ObjectManager
    {
        Config _config;
        List<WatchVariableControl> _objectDataControls;
        List<WatchVariableControl> _behaviorDataControls = new List<WatchVariableControl>();
        ProcessStream _stream;
        ObjectAssociations _objAssoc;
        ObjectDataGui _objGui;

        DataContainer _disToMario;
        DataContainer _latDisToMario;
        DataContainer _rngCalls;

        object _watchVarLocker = new object();

        uint _currentAddress;
        int _slotIndex;
        string _slotPos;
        uint _behavior;
        bool _unclone = false;

        #region Fields
        public void SetBehaviorWatchVariables(List<WatchVariable> value, Color color)
        {
            lock (_watchVarLocker)
            {
                // Remove old watchVars from list
                foreach (var watchVar in _behaviorDataControls)
                {
                    _objectDataControls.Remove(watchVar);
                    _objGui.ObjectFlowLayout.Controls.Remove(watchVar.Control);
                }
                _behaviorDataControls.Clear();

                // Add new watchVars
                foreach (var watchVar in value)
                {
                    var newWatchVarControl = new WatchVariableControl(_stream, watchVar);
                    newWatchVarControl.Color = color;
                    _behaviorDataControls.Add(newWatchVarControl);
                    _objectDataControls.Add(newWatchVarControl);
                    _objGui.ObjectFlowLayout.Controls.Add(newWatchVarControl.Control);
                }
            }
        }

        public uint CurrentAddress
        {
            get
            {
                return _currentAddress;
            }
            set
            {
                if (_currentAddress != value)
                {
                    _currentAddress = value;
                    _objGui.ObjAddressLabelValue.Text = "0x" + _currentAddress.ToString("X8");
                }
            }
        }

        public int SlotIndex
        {
            get
            {
                return _slotIndex;
            }
            set
            {
                if (_slotIndex != value)
                {
                    _slotIndex = value;
                    _objGui.ObjSlotIndexLabel.Text = _slotIndex.ToString();
                }
            }
        }

        public string SlotPos
        {
            get
            {
                return _slotPos;
            }
            set
            {
                if (_slotPos != value)
                {
                    _slotPos = value;
                    _objGui.ObjSlotPositionLabel.Text = _slotPos;
                }
            }
        }

        public uint Behavior
        {
            get
            {
                return _behavior;
            }
            set
            {
                if (_behavior != value)
                {
                    _behavior = value;
                    _objGui.ObjBehaviorLabel.Text = "0x" + _behavior.ToString("X4");
                }
            }
        }

        public string Name
        {
            get
            {
                return _objGui.ObjectNameTextBox.Text;
            }
            set
            {
                if (_objGui.ObjectNameTextBox.Text != value)
                {
                    _objGui.ObjectNameTextBox.Text = value;
                }
            }
        }

        public Color BackColor
        {
            set
            {
                if (_objGui.ObjectBorderPanel.BackColor != value)
                {
                    _objGui.ObjectBorderPanel.BackColor = value;
                    _objGui.ObjectImagePictureBox.BackColor = value.Lighten(0.5);
                }
            }
            get
            {
                return _objGui.ObjectBorderPanel.BackColor;
            }
        }

        public Image Image
        {
            set
            {
                if (_objGui.ObjectImagePictureBox.Image != value)
                {
                    _objGui.ObjectImagePictureBox.Image = value;
                }
            }
            get
            {
                return _objGui.ObjectImagePictureBox.Image;
            }
        }

        #endregion

        public ObjectManager(ProcessStream stream, Config config, ObjectAssociations objAssoc, List<WatchVariable> objectData, ObjectDataGui objectGui)
        { 
            _config = config;
            _stream = stream;
            _objGui = objectGui;
            _objAssoc = objAssoc;

            // Register controls on the control (for drag-and-drop)
            RegisterControlEvents(_objGui.ObjectBorderPanel);
            foreach (Control control in _objGui.ObjectBorderPanel.Controls)
                RegisterControlEvents(control);

            _objectDataControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in objectData)
            {
                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar);
                objectGui.ObjectFlowLayout.Controls.Add(watchControl.Control);
                _objectDataControls.Add(watchControl);
            }
            _objGui.ObjAddressLabelValue.Click += ObjAddressLabel_Click;
            _objGui.ObjAddressLabel.Click += ObjAddressLabel_Click;

            // Add distance to mario watchvar
            _disToMario = new DataContainer("Dis. to Mario");
            objectGui.ObjectFlowLayout.Controls.Add(_disToMario.Control);

            // Add lateral distance to mario watchvar
            _latDisToMario = new DataContainer("Lat. Dis. to M");
            objectGui.ObjectFlowLayout.Controls.Add(_latDisToMario.Control);

            // Add rng calls watchvar
            _rngCalls = new DataContainer("RNG Calls/Frame");
            objectGui.ObjectFlowLayout.Controls.Add(_rngCalls.Control);

            // Register buttons
            objectGui.CloneButton.Click += CloneButton_Click;
            objectGui.UnloadButton.Click += UnloadButton_Click;
            objectGui.MoveMarioToButton.Click += MoveMarioToButton_Click;
            objectGui.MoveToMarioButton.Click += MoveToMarioButton_Click;
        }

        private void ObjAddressLabel_Click(object sender, EventArgs e)
        {
            var variableInfo = new VariableViewerForm("Object Address", "Object",
                String.Format("0x{0:X8}", _currentAddress), String.Format("0x{0:X8}", (_currentAddress & 0x0FFFFFFF) + _stream.ProcessMemoryOffset));
            variableInfo.ShowDialog();
        }

        private void MoveToMarioButton_Click(object sender, EventArgs e)
        {
            MarioActions.MoveObjectToMario(_stream, _config, CurrentAddress);
        }

        private void MoveMarioToButton_Click(object sender, EventArgs e)
        {
            MarioActions.MoveMarioToObject(_stream, _config, CurrentAddress);
        }

        private void UnloadButton_Click(object sender, EventArgs e)
        {
            MarioActions.UnloadObject(_stream, _config, CurrentAddress);
        }

        private void CloneButton_Click(object sender, EventArgs e)
        {
            if (_unclone)
                MarioActions.UnCloneObject(_stream, _config, CurrentAddress);
            else
                MarioActions.CloneObject(_stream, _config, CurrentAddress);
        }

        public void Update()
        {
            lock (_watchVarLocker)
            {
                // Update watch variables
                foreach (var watchVar in _objectDataControls)
                {
                    watchVar.OtherOffset = CurrentAddress;
                    watchVar.Update();
                }
            }

            // Get Mario position
            var marioAddress = _config.Mario.MarioStructAddress;
            float mX, mY, mZ;
            mX = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.XOffset, 4), 0);
            mY = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.YOffset, 4), 0);
            mZ = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.ZOffset, 4), 0);

            // Get object position
            float x, y, z;
            x = BitConverter.ToSingle(_stream.ReadRam(_currentAddress + _config.ObjectSlots.ObjectXOffset, 4), 0);
            y = BitConverter.ToSingle(_stream.ReadRam(_currentAddress + _config.ObjectSlots.ObjectYOffset, 4), 0);
            z = BitConverter.ToSingle(_stream.ReadRam(_currentAddress + _config.ObjectSlots.ObjectZOffset, 4), 0);

            // Calculate distances to Mario
            float latDisToMario = (float)Math.Sqrt(Math.Pow(x - mX, 2) + Math.Pow(z - mZ, 2));
            float disToMario = (float)Math.Sqrt(Math.Pow(x - mX, 2) + Math.Pow(y - mY, 2) + Math.Pow(z - mZ, 2));

            // Determine which object is being held
            uint holdingObj = BitConverter.ToUInt32(_stream.ReadRam(marioAddress + _config.Mario.HoldingObjectPointerOffset, 4),0);
            
            // Change to unclone if we are already holding the object
            if ((holdingObj == _currentAddress) != _unclone)
            {
                _unclone = !_unclone;

                // Update button text
                _objGui.CloneButton.Text = _unclone ? "UnClone" : "Clone";
            }

            // Update data container text
            _latDisToMario.Text = latDisToMario.ToString();
            _disToMario.Text = disToMario.ToString();
            _rngCalls.Text = GetNumRngCalls().ToString();
        }

        private int GetNumRngCalls()
        {
            var numberOfRngObjs = BitConverter.ToUInt32(_stream.ReadRam(_config.RngRecordingAreaAddress, 4), 0);

            int numOfCalls = 0;

            for (int i = 0; i < numberOfRngObjs; i++)
            {
                uint rngStructAdd = (uint)(_config.RngRecordingAreaAddress + 0x10 + 0x08 * i);
                var address = BitConverter.ToUInt32(_stream.ReadRam(rngStructAdd + 0x04, 4), 0);
                if (address != _currentAddress)
                    continue;

                var preRng = BitConverter.ToUInt16(_stream.ReadRam(rngStructAdd + 0x00, 2), 0);
                var postRng = BitConverter.ToUInt16(_stream.ReadRam(rngStructAdd + 0x02, 2), 0);

                numOfCalls = RngIndexer.GetRngIndexDiff(preRng, postRng);
                break;
            }

            return numOfCalls;
        }

        private void RegisterControlEvents(Control control)
        {
            control.AllowDrop = true;
            control.DragEnter += DragEnter;
            control.DragDrop += OnDrop;
            control.MouseDown += OnDrag;
        }

        private void OnDrag(object sender, EventArgs e)
        {
            // Start the drag and drop but setting the object slot index in Drag and Drop data
            var dropAction = new DropAction(DropAction.ActionType.Object, CurrentAddress);
            (sender as Control).DoDragDrop(dropAction, DragDropEffects.All);
        }

        private void DragEnter(object sender, DragEventArgs e)
        {
            // Make sure we have valid Drag and Drop data (it is an index)
            if (!e.Data.GetDataPresent(typeof(DropAction)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var dropAction = ((DropAction)e.Data.GetData(typeof(DropAction))).Action;
            if (dropAction != DropAction.ActionType.Object && dropAction != DropAction.ActionType.Mario)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            // Make sure we have valid Drag and Drop data (it is an index)
            if (!e.Data.GetDataPresent(typeof(DropAction)))
                return;

            var dropAction = ((DropAction)e.Data.GetData(typeof(DropAction)));
        }
    }
}
