using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;

namespace SM64_Diagnostic.ManagerClasses
{
    public class MarioManager
    {
        List<WatchVariableControl> _marioDataControls;
        FlowLayoutPanel _variableTable;
        ProcessStream _stream;
        DataContainer _deFactoSpeed, _slidingSpeed, _slidingAngle, _fallHeight;
        MapManager _mapManager;

        public MarioManager(ProcessStream stream, List<WatchVariable> marioData, Control marioControl, FlowLayoutPanel variableTable, MapManager mapManager)
        {
            // Register controls on the control (for drag-and-drop)
            RegisterControlEvents(marioControl);
            foreach (Control control in marioControl.Controls)
                RegisterControlEvents(control);

            _variableTable = variableTable;
            _stream = stream;
            _mapManager = mapManager;

            _marioDataControls = new List<WatchVariableControl>();

            _deFactoSpeed = new DataContainer("De Facto Speed");
            _slidingSpeed = new DataContainer("Sliding Speed");
            _slidingAngle = new DataContainer("Sliding Angle");
            _fallHeight = new DataContainer("Fall Height");

            foreach (WatchVariable watchVar in marioData)
            {
                if (!watchVar.Special)
                {
                    WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, Config.Mario.StructAddress);
                    variableTable.Controls.Add(watchControl.Control);
                    _marioDataControls.Add(watchControl);
                    continue;
                }

                switch (watchVar.SpecialType)
                {
                    case "DeFactoSpeed":
                        _deFactoSpeed.Name = watchVar.Name;
                        variableTable.Controls.Add(_deFactoSpeed.Control);
                        break;

                    case "SlidingSpeed":
                        _slidingSpeed.Name = watchVar.Name;
                        variableTable.Controls.Add(_slidingSpeed.Control);
                        break;

                    case "SlidingAngle":
                        _slidingAngle.Name = watchVar.Name;
                        //variableTable.Controls.Add(_slidingAngle.Control);
                        break;

                    case "FallHeight":
                        _fallHeight.Name = watchVar.Name;
                        variableTable.Controls.Add(_fallHeight.Control);
                        break;

                    default:
                        var failedContainer = new DataContainer(watchVar.Name);
                        failedContainer.Text = "Couldn't Find";
                        variableTable.Controls.Add(failedContainer.Control);
                        break;
                }
            }
        }

        public void Update(bool updateView)
        {
            // Get Mario position and rotation
            float x, y, z, rot;
            var marioAddress = Config.Mario.StructAddress;
            x = BitConverter.ToSingle(_stream.ReadRam(marioAddress + Config.Mario.XOffset, 4), 0);
            y = BitConverter.ToSingle(_stream.ReadRam(marioAddress + Config.Mario.YOffset, 4), 0);
            z = BitConverter.ToSingle(_stream.ReadRam(marioAddress + Config.Mario.ZOffset, 4), 0);
            rot = (float) (((BitConverter.ToUInt32(_stream.ReadRam(marioAddress + Config.Mario.RotationOffset, 4), 0)
                >> 16) % 65536) / 65536f * 360f); 

            // Update Mario map object
            _mapManager.MarioMapObject.X = x;
            _mapManager.MarioMapObject.Y = y;
            _mapManager.MarioMapObject.Z = z;
            _mapManager.MarioMapObject.Rotation = rot;
            _mapManager.MarioMapObject.Show = true;

            // Get holp position
            float holpX, holpY, holpZ;
            holpX = BitConverter.ToSingle(_stream.ReadRam(Config.HolpX, 4), 0);
            holpY = BitConverter.ToSingle(_stream.ReadRam(Config.HolpY, 4), 0);
            holpZ = BitConverter.ToSingle(_stream.ReadRam(Config.HolpZ, 4), 0);

            // Update holp map object position
            _mapManager.HolpMapObject.X = holpX;
            _mapManager.HolpMapObject.Y = holpY;
            _mapManager.HolpMapObject.Z = holpZ;
            _mapManager.HolpMapObject.Show = true;

            // Update camera position and rotation
            float cameraX, cameraY, cameraZ , cameraRot;
            cameraX = BitConverter.ToSingle(_stream.ReadRam(Config.CameraX, 4), 0);
            cameraY = BitConverter.ToSingle(_stream.ReadRam(Config.CameraY, 4), 0);
            cameraZ = BitConverter.ToSingle(_stream.ReadRam(Config.CameraZ, 4), 0);
            cameraRot = (float)(((UInt16)(BitConverter.ToUInt32(_stream.ReadRam(Config.CameraRot, 4), 0))) / 65536f * 360f);

            // Update floor triangle
            UInt32 floorTriangle = BitConverter.ToUInt32(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset, 4), 0);
            if (floorTriangle != 0x00)
            {
                Int16 x1 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.X1, 2), 0);
                Int16 y1 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.Y1, 2), 0);
                Int16 z1 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.Z1, 2), 0);
                Int16 x2 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.X2, 2), 0);
                Int16 y2 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.Y2, 2), 0);
                Int16 z2 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.Z2, 2), 0);
                Int16 x3 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.X3, 2), 0);
                Int16 y3 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.Y3, 2), 0);
                Int16 z3 = BitConverter.ToInt16(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.Z3, 2), 0);
                _mapManager.FloorTriangleMapObject.X1 = x1;
                _mapManager.FloorTriangleMapObject.Z1 = z1;
                _mapManager.FloorTriangleMapObject.X2 = x2;
                _mapManager.FloorTriangleMapObject.Z2 = z2;
                _mapManager.FloorTriangleMapObject.X3 = x3;
                _mapManager.FloorTriangleMapObject.Z3 = z3;
                _mapManager.FloorTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            _mapManager.FloorTriangleMapObject.Show = (floorTriangle != 0x00);

            // Update camera map object position
            _mapManager.CameraMapObject.X = cameraX;
            _mapManager.CameraMapObject.Y = cameraY;
            _mapManager.CameraMapObject.Z = cameraZ;
            _mapManager.CameraMapObject.Rotation = cameraRot;

            // Update watch variables
            foreach (var watchVar in _marioDataControls)
                watchVar.Update();

            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            var floorY = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.GroundYOffset, 4), 0);

            if (floorTriangle != 0x00)
            {
                float hSpeed = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.HSpeedOffset, 4), 0);
                float normY = BitConverter.ToSingle(_stream.ReadRam(floorTriangle + Config.TriangleOffsets.NormY, 4), 0);
                _deFactoSpeed.Text = (hSpeed * normY).ToString();
            }
            else
                _deFactoSpeed.Text = "(No Floor)";

            float slidingSpeedX = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.SlidingSpeedXOffset, 4), 0);
            float slidingSpeedZ = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.SlidingSpeedZOffset, 4), 0);

            _slidingSpeed.Text = ((float)Math.Sqrt(slidingSpeedX * slidingSpeedX + slidingSpeedZ * slidingSpeedZ)).ToString();

            _fallHeight.Text = (BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.PeakHeightOffset, 4), 0) - floorY).ToString();
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
            var dropAction = new DropAction(DropAction.ActionType.Mario, Config.Mario.StructAddress);
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

            MarioActions.MoveObjectToMario(_stream, dropAction.Address);
        }
    }
}
