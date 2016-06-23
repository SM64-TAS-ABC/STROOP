using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SM64_Diagnostic.ManagerClasses
{
    public class MapManager
    {
        ProcessStream _stream;
        Config _config;
        MapAssociations _assoc;
        byte _currentLevel, _currentArea;
        ushort _currentLoadingPoint;
        Map _currentMap;
        List<Map> _currentMapList = null;
        MapGraphicsControl _mapGraphics;
        MapObject _marioMapObj;
        MapGui _mapGui;
        bool _isLoaded = false;

        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
        }

        public MapObject MarioMapObject
        {
            get
            {
                return _marioMapObj;
            }
            set
            {
                _marioMapObj = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _mapGraphics.Control.Visible;
            }
            set
            {
                _mapGraphics.Control.Visible = value;
            }
        }

        public MapManager(ProcessStream stream, Config config, MapAssociations mapAssoc,
            MapGui mapGui)
        {
            _stream = stream;
            _config = config;
            _assoc = mapAssoc;
            _mapGui = mapGui;

            _marioMapObj = new MapObject(new Bitmap("Resources\\Maps\\Object Images\\Mario Top.png"));
            _marioMapObj.UsesRotation = true;
        }

        public void Load()
        {
            _mapGraphics = new MapGraphicsControl(_mapGui.GLControl);
            _mapGraphics.Load();
            _isLoaded = true;

            ChangeCurrentMap(_assoc.DefaultMap);

            _mapGraphics.AddMapObject(_marioMapObj);
        }

        public void Update()
        {
            // Make sure the control has successfully loaded
            if (!_isLoaded)
                return;

            // Get level and area
            byte level = _stream.ReadRam(_config.LevelAddress, 1)[0];
            byte area = _stream.ReadRam(_config.AreaAddress, 1)[0];
            ushort loadingPoint = BitConverter.ToUInt16(_stream.ReadRam(_config.LoadingPointAddress, 2), 0);

            // Find new map list
            if (_currentMapList == null || _currentLevel != level || _currentArea != area || _currentLoadingPoint != loadingPoint)
            {
                _currentLevel = level;
                _currentArea = area;
                _currentLoadingPoint = loadingPoint;
                _currentMapList = _assoc.GetLevelAreaMaps(level, area);

                // Look for maps with correct loading points
                var mapListLPFiltered = _currentMapList.Where((map) => map.LoadingPoint == loadingPoint).ToList();
                if (mapListLPFiltered.Count > 0)
                    _currentMapList = mapListLPFiltered;
            }

            // Filter out all maps that are lower than Mario
            var mapListYFiltered = _currentMapList.Where((map) => map.Y <= _marioMapObj.Y).ToList();

            // If no map is available display the default image
            if (mapListYFiltered.Count <= 0)
            {
                ChangeCurrentMap(_assoc.DefaultMap);
            }
            else
            {
                // Pick the map closest to mario (yet still above Mario)
                Map bestMap = mapListYFiltered[0];
                foreach (Map map in mapListYFiltered)
                {
                    if (map.Y < bestMap.Y)
                        bestMap = map;
                }

                ChangeCurrentMap(bestMap);
            }

            //---- Update PU -----
            int colAreaX = (int)((_marioMapObj.X + 8192) / 16384);
            int colAreaZ = (int)((_marioMapObj.Z + 8192) / 16384);

            if (_marioMapObj.X < -8192)
                colAreaX--;
            if (_marioMapObj.Z < -8192)
                colAreaZ--;

            // Pu's are actually 4 "collision areas" away
            int puX = colAreaX / 4;
            int puZ = colAreaZ / 4;

            // Update Qpu
            int qpuX = puX / 4;
            int qpuZ = puZ / 4;

            // Update labels
            _mapGui.PuValueLabel.Text = string.Format("[{0}:{1}]", puX, puZ);
            _mapGui.QpuValueLabel.Text = string.Format("[{0}:{1}]", qpuX, qpuZ);
            _mapGui.MapIdLabel.Text = string.Format("[{0}:{1}:{2}]", level, area, loadingPoint);
            _mapGui.MapNameLabel.Text = _currentMap.Name;
            _mapGui.MapSubNameLabel.Text = (_currentMap.SubName != null) ? _currentMap.SubName : "";

            // Adjust mario coordinates relative from current PU
            var marioCoord = new PointF(_marioMapObj.X - puX * 16384, _marioMapObj.Z - puZ * 16384);

            // Calculate mario's location on the OpenGl control
            var mapView = _mapGraphics.MapView;
            _marioMapObj.LocationOnContol = CalculateLocationOnControl(marioCoord, mapView);

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _mapGraphics.Control.Invalidate();
        }

        private void ChangeCurrentMap(Map map)
        {
            if (_currentMap == map)
                return;

            _mapGraphics.SetMap(_assoc.GetMapImage(map));
            _currentMap = map;
        }

        private PointF CalculateLocationOnControl(PointF mapLoc, RectangleF mapView)
        {
            PointF locCtrl = new PointF();
            locCtrl.X = mapView.X + (mapLoc.X - _currentMap.Coordinates.X) 
                * (mapView.Width / _currentMap.Coordinates.Width);
            locCtrl.Y = mapView.Y + (mapLoc.Y - _currentMap.Coordinates.Y) 
                * (mapView.Height / _currentMap.Coordinates.Height);
            return locCtrl;
        }

        private void SetPictureBoxLocation(PictureBox box, PointF point)
        {
            box.Location = new Point((int)point.X - box.Width / 2,
                (int)point.Y - box.Height / 2);
        }
        
    }
}
