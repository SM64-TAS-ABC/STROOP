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

namespace SM64_Diagnostic.Managers
{
    public class MarioManager : DataManager
    {
        MapManager _mapManager;

        public MarioManager(ProcessStream stream, List<WatchVariable> marioData, Control marioControl, NoTearFlowLayoutPanel variableTable, MapManager mapManager)
            : base(stream, marioData, variableTable, Config.Mario.StructAddress)
        {
            _mapManager = mapManager;
        }

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("DeFactoSpeed"),
                new DataContainer("SlidingSpeed"),
                new AngleDataContainer("SlidingAngle"),
                new DataContainer("FallHeight"),
            };
        }

        public void ProcessSpecialVars()
        {
            UInt32 floorTriangle = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
            var floorY = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.GroundYOffset, 4), 0);

            float slidingSpeedX = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.SlidingSpeedXOffset, 4), 0);
            float slidingSpeedZ = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.SlidingSpeedZOffset, 4), 0);
            foreach (var specialVar in _specialWatchVars)
            {
                switch(specialVar.SpecialName)
                {
                    case "DeFactoSpeed":
                        if (floorTriangle != 0x00)
                        {
                            float hSpeed = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HSpeedOffset);
                            float normY = _stream.GetSingle(floorTriangle + Config.TriangleOffsets.NormY);
                            (specialVar as DataContainer).Text = Math.Round(hSpeed * normY, 3).ToString();
                        }
                        else
                        {
                            (specialVar as DataContainer).Text = "(No Floor)";
                        }
                        break;

                    case "SlidingSpeed":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(slidingSpeedX * slidingSpeedX + slidingSpeedZ * slidingSpeedZ), 3).ToString();
                        break;

                    case "SlidingAngle":
                        (specialVar as AngleDataContainer).AngleValue = Math.PI / 2 - Math.Atan2(slidingSpeedZ, slidingSpeedX);
                        (specialVar as AngleDataContainer).ValueExists = (slidingSpeedX != 0) || (slidingSpeedZ != 0);
                        break;

                    case "FallHeight":
                        (specialVar as DataContainer).Text = (_stream.GetSingle(Config.Mario.StructAddress + Config.Mario.PeakHeightOffset) - floorY).ToString();
                        break;
                }
            }
        }

        public override void Update(bool updateView)
        {
            // Get Mario position and rotation
            float x, y, z, rot;
            var marioAddress = Config.Mario.StructAddress;
            x = _stream.GetSingle(marioAddress + Config.Mario.XOffset);
            y = _stream.GetSingle(marioAddress + Config.Mario.YOffset);
            z = _stream.GetSingle(marioAddress + Config.Mario.ZOffset);
            rot = (float) (((_stream.GetUInt32(marioAddress + Config.Mario.RotationOffset) >> 16) % 65536) / 65536f * 360f); 

            // Update Mario map object
            _mapManager.MarioMapObject.X = x;
            _mapManager.MarioMapObject.Y = y;
            _mapManager.MarioMapObject.Z = z;
            _mapManager.MarioMapObject.Rotation = rot;
            _mapManager.MarioMapObject.Show = true;

            // Get holp position
            float holpX, holpY, holpZ;
            holpX = _stream.GetSingle(Config.HolpX);
            holpY = _stream.GetSingle(Config.HolpY);
            holpZ = _stream.GetSingle(Config.HolpZ);

            // Update holp map object position
            _mapManager.HolpMapObject.X = holpX;
            _mapManager.HolpMapObject.Y = holpY;
            _mapManager.HolpMapObject.Z = holpZ;
            _mapManager.HolpMapObject.Show = true;

            // Update camera position and rotation
            float cameraX, cameraY, cameraZ , cameraRot;
            cameraX = _stream.GetSingle(Config.CameraX);
            cameraY = _stream.GetSingle(Config.CameraY);
            cameraZ = _stream.GetSingle(Config.CameraZ);
            cameraRot = (float)(((UInt16)(_stream.GetUInt32(Config.CameraRot)) / 65536f * 360f));

            // Update floor triangle
            UInt32 floorTriangle = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
            if (floorTriangle != 0x00)
            {
                Int16 x1 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.X1);
                Int16 y1 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y1);
                Int16 z1 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z1);
                Int16 x2 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.X2);
                Int16 y2 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y2);
                Int16 z2 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z2);
                Int16 x3 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.X3);
                Int16 y3 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Y3);
                Int16 z3 = _stream.GetInt16(floorTriangle + Config.TriangleOffsets.Z3);
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

            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();
            ProcessSpecialVars();
        }
    }
}
