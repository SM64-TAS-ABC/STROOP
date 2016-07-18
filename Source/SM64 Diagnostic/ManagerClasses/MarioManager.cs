using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;

namespace SM64_Diagnostic.ManagerClasses
{
    public class MarioManager
    {
        Config _config;
        List<WatchVariableControl> _marioDataControls;
        FlowLayoutPanel _variableTable;
        ProcessStream _stream;
        DataContainer _rngIndex, _rngPerFrame, _heightAboveGround, _heightBelowCeil;
        MapManager _mapManager;

        public MarioManager(ProcessStream stream, Config config, List<WatchVariable> marioData, Control marioControl, FlowLayoutPanel variableTable, MapManager mapManager)
        {
            // Register controls on the control (for drag-and-drop)
            RegisterControlEvents(marioControl);
            foreach (Control control in marioControl.Controls)
                RegisterControlEvents(control);

            _config = config;
            _variableTable = variableTable;
            _stream = stream;
            _mapManager = mapManager;

            _marioDataControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in marioData)
            {
                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, _config.Mario.MarioStructAddress);
                variableTable.Controls.Add(watchControl.Control);
                _marioDataControls.Add(watchControl);
            }

            // Add rng index
            _rngIndex = new DataContainer("RNG Index");
            variableTable.Controls.Add(_rngIndex.Control);

            _rngPerFrame = new DataContainer("RNG Calls/Frame");
            variableTable.Controls.Add(_rngPerFrame.Control);

            _heightAboveGround = new DataContainer("Dis Abv Floor");
            variableTable.Controls.Add(_heightAboveGround.Control);

            _heightBelowCeil = new DataContainer("Dis Below Ceil");
            variableTable.Controls.Add(_heightBelowCeil.Control);
        }

        public void Update(bool updateView)
        {
            // Get Mario position and rotation
            float x, y, z, rot;
            var marioAddress = _config.Mario.MarioStructAddress;
            x = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.XOffset, 4), 0);
            y = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.YOffset, 4), 0);
            z = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.ZOffset, 4), 0);
            rot = (float) (((BitConverter.ToUInt32(_stream.ReadRam(marioAddress + _config.Mario.RotationOffset, 4), 0)
                >> 16) % 65536) / 65536f * 360f); 

            // Update Mario map object
            _mapManager.MarioMapObject.X = x;
            _mapManager.MarioMapObject.Y = y;
            _mapManager.MarioMapObject.Z = z;
            _mapManager.MarioMapObject.Rotation = rot;
            _mapManager.MarioMapObject.Show = true;

            // Get holp position
            float holpX, holpY, holpZ;
            holpX = BitConverter.ToSingle(_stream.ReadRam(_config.HolpX, 4), 0);
            holpY = BitConverter.ToSingle(_stream.ReadRam(_config.HolpY, 4), 0);
            holpZ = BitConverter.ToSingle(_stream.ReadRam(_config.HolpZ, 4), 0);

            // Update holp map object position
            _mapManager.HolpMapObject.X = holpX;
            _mapManager.HolpMapObject.Y = holpY;
            _mapManager.HolpMapObject.Z = holpZ;
            _mapManager.HolpMapObject.Show = true;


            // Update camera position and rotation
            float cameraX, cameraY, cameraZ , cameraRot;
            cameraX = BitConverter.ToSingle(_stream.ReadRam(_config.CameraX, 4), 0);
            cameraY = BitConverter.ToSingle(_stream.ReadRam(_config.CameraY, 4), 0);
            cameraZ = BitConverter.ToSingle(_stream.ReadRam(_config.CameraZ, 4), 0);
            cameraRot = (float)(((UInt16)(BitConverter.ToUInt32(_stream.ReadRam(_config.CameraRot, 4), 0))) / 65536f * 360f);

            // Update camera map object position
            _mapManager.CameraMapObject.X = cameraX;
            _mapManager.CameraMapObject.Y = cameraY;
            _mapManager.CameraMapObject.Z = cameraZ;
            _mapManager.CameraMapObject.Rotation = cameraRot;

            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            // Update watch variables
            foreach (var watchVar in _marioDataControls)
                watchVar.Update();

            // Update the rng index
            int rngIndex = RngIndexer.GetRngIndex(BitConverter.ToUInt16(_stream.ReadRam(_config.RngAddress, 2), 0));
            _rngIndex.Text = (rngIndex < 0) ? "N/A [" + (-rngIndex).ToString() + "]" : rngIndex.ToString();

            _rngPerFrame.Text = GetRngCallsPerFrame().ToString();

            _heightBelowCeil.Text = (BitConverter.ToSingle(_stream.ReadRam(_config.Mario.MarioStructAddress + _config.Mario.CeilingYOffset, 4), 0) - y).ToString();
            _heightAboveGround.Text = (y - BitConverter.ToSingle(_stream.ReadRam(_config.Mario.MarioStructAddress + _config.Mario.GroundYOffset, 4), 0)).ToString();
        }

        private int GetRngCallsPerFrame()
        {
            var currentRng = BitConverter.ToUInt16(_stream.ReadRam(_config.RngRecordingAreaAddress + 0x0E, 2), 0);
            var preRng = BitConverter.ToUInt16(_stream.ReadRam(_config.RngRecordingAreaAddress + 0x0C, 2), 0);

            return RngIndexer.GetRngIndexDiff(preRng, currentRng);
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
            var dropAction = new DropAction(DropAction.ActionType.Mario, _config.Mario.MarioStructAddress);
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
            if (dropAction.Action != DropAction.ActionType.Object)
                return;

            ObjectActions.MoveObjectToMario(_stream, _config, dropAction.Address);
        }
    }
}
