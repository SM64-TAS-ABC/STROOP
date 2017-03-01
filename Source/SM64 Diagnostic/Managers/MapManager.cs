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
using SM64_Diagnostic.ManagerClasses;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class MapManager
    {
        ProcessStream _stream;
        public MapAssociations MapAssoc;
        byte _currentLevel, _currentArea;
        ushort _currentLoadingPoint, _currentMissionLayout;
        Map _currentMap;
        List<Map> _currentMapList = null;
        MapGraphics _mapGraphics;
        MapObject _marioMapObj;
        MapObject _holpMapObj;
        MapObject _cameraMapObj;
        TriangleMapObject _floorTriangleMapObj;
        List<MapObject> _mapObjects = new List<MapObject>();
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
        }

        public MapObject HolpMapObject
        {
            get
            {
                return _holpMapObj;
            }
        }

        public MapObject CameraMapObject
        {
            get
            {
                return _cameraMapObj;
            }
        }

        public TriangleMapObject FloorTriangleMapObject
        {
            get
            {
                return _floorTriangleMapObj;
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

        public MapManager(ProcessStream stream, MapAssociations mapAssoc, ObjectAssociations objAssoc,
            MapGui mapGui)
        {
            _stream = stream;
            MapAssoc = mapAssoc;
            _mapGui = mapGui;

            _marioMapObj = new MapObject(objAssoc.MarioMapImage, 1);
            _marioMapObj.UsesRotation = true;

            _holpMapObj = new MapObject(objAssoc.HolpImage, 2);

            _cameraMapObj = new MapObject(objAssoc.CameraMapImage, 1);
            _cameraMapObj.UsesRotation = true;
            _floorTriangleMapObj = new TriangleMapObject(Color.FromArgb(200, Color.Yellow), 3);
        }

        public void Load()
        {
            // Create new graphics control
            _mapGraphics = new MapGraphics(_mapGui.GLControl);
            _mapGraphics.Load();

            _isLoaded = true;

            // Set the default map
            ChangeCurrentMap(MapAssoc.DefaultMap);

            // Add Mario's map object
            _mapGraphics.AddMapObject(_marioMapObj);
            _mapGraphics.AddMapObject(_holpMapObj);
            _mapGraphics.AddMapObject(_cameraMapObj);
            _mapGraphics.AddMapObject(_floorTriangleMapObj);

            //----- Register events ------
            // Set image
            _mapGui.MapIconSizeTrackbar.ValueChanged += (sender, e) => _mapGraphics.IconSize = _mapGui.MapIconSizeTrackbar.Value;
        }

        public void Update()
        {
            // Make sure the control has successfully loaded
            if (!_isLoaded)
                return;

            // Get level and area
            byte level = _stream.GetByte(Config.LevelAddress);
            byte area = _stream.GetByte(Config.AreaAddress);
            ushort loadingPoint = _stream.GetUInt16(Config.LoadingPointAddress);
            ushort missionLayout = _stream.GetUInt16(Config.MissionAddress);

            // Find new map list
            if (_currentMapList == null || _currentLevel != level || _currentArea != area 
                || _currentLoadingPoint != loadingPoint || _currentMissionLayout != missionLayout)
            {
                _currentLevel = level;
                _currentArea = area;
                _currentLoadingPoint = loadingPoint;
                _currentMissionLayout = missionLayout;
                _currentMapList = MapAssoc.GetLevelAreaMaps(level, area);

                // Look for maps with correct loading points
                var mapListLPFiltered = _currentMapList.Where((map) => map.LoadingPoint == loadingPoint).ToList();
                if (mapListLPFiltered.Count > 0)
                    _currentMapList = mapListLPFiltered;
                else
                    _currentMapList = _currentMapList.Where((map) => !map.LoadingPoint.HasValue).ToList();

                var mapListMLFiltered = _currentMapList.Where((map) => map.MissionLayout == missionLayout).ToList();
                if (mapListMLFiltered.Count > 0)
                    _currentMapList = mapListMLFiltered;
                else
                    _currentMapList = _currentMapList.Where((map) => !map.MissionLayout.HasValue).ToList();
            }

            // ---- Update PU -----
            int puX = PuUtilities.GetPUFromCoord(_marioMapObj.X);
            int puY = PuUtilities.GetPUFromCoord(_marioMapObj.Y);
            int puZ = PuUtilities.GetPUFromCoord(_marioMapObj.Z);

            // Update Qpu
            double qpuX = puX / 4d;
            double qpuY = puY / 4d;
            double qpuZ = puZ / 4d;

            // Update labels
            _mapGui.PuValueLabel.Text = string.Format("[{0}:{1}:{2}]", puX, puY, puZ);
            _mapGui.QpuValueLabel.Text = string.Format("[{0}:{1}:{2}]", qpuX, qpuY, qpuZ);
            _mapGui.MapIdLabel.Text = string.Format("[{0}:{1}:{2}:{3}]", level, area, loadingPoint, missionLayout);
            _mapGui.MapNameLabel.Text = _currentMap.Name;
            _mapGui.MapSubNameLabel.Text = (_currentMap.SubName != null) ? _currentMap.SubName : "";

            // Adjust mario coordinates relative from current PU
            float marioRelX = PuUtilities.GetRelativePuPosition(_marioMapObj.X, puX);
            float marioRelY = PuUtilities.GetRelativePuPosition(_marioMapObj.Y, puY);
            float marioRelZ = PuUtilities.GetRelativePuPosition(_marioMapObj.Z, puZ);
            var marioCoord = new PointF(marioRelX, marioRelZ);

            // Filter out all maps that are lower than Mario
            var mapListYFiltered = _currentMapList.Where((map) => map.Y <= marioRelY).ToList();

            // If no map is available display the default image
            if (mapListYFiltered.Count <= 0)
            {
                ChangeCurrentMap(MapAssoc.DefaultMap);
            }
            else
            {
                // Pick the map closest to mario (yet still above Mario)
                Map bestMap = mapListYFiltered[0];
                foreach (Map map in mapListYFiltered)
                {
                    if (map.Y > bestMap.Y)
                        bestMap = map;
                }

                ChangeCurrentMap(bestMap);
            }

            // Calculate mario's location on the OpenGl control
            var mapView = _mapGraphics.MapView;
            _marioMapObj.LocationOnContol = CalculateLocationOnControl(marioCoord, mapView);
            _marioMapObj.Draw = _mapGui.MapShowMario.Checked;

            int holpPuX = PuUtilities.GetPUFromCoord(_holpMapObj.X);
            int holpPuY = PuUtilities.GetPUFromCoord(_holpMapObj.Y);
            int holpPuZ = PuUtilities.GetPUFromCoord(_holpMapObj.Z);
            float holpRelX = PuUtilities.GetRelativePuPosition(_holpMapObj.X, holpPuX);
            float holpRelZ = PuUtilities.GetRelativePuPosition(_holpMapObj.Z, holpPuZ);
            var holpCoord = new PointF(holpRelX, holpRelZ);
            _holpMapObj.Draw = _mapGui.MapShowHolp.Checked && puX == holpPuX && puY == holpPuY && puZ == holpPuZ;
            _holpMapObj.LocationOnContol = CalculateLocationOnControl(holpCoord, mapView);

            int cameraPuX = PuUtilities.GetPUFromCoord(_cameraMapObj.X);
            int cameraPuY = PuUtilities.GetPUFromCoord(_cameraMapObj.Y);
            int cameraPuZ = PuUtilities.GetPUFromCoord(_cameraMapObj.Z);
            float cameraRelX = PuUtilities.GetRelativePuPosition(_cameraMapObj.X, cameraPuX);
            float cameraRelZ = PuUtilities.GetRelativePuPosition(_cameraMapObj.Z, cameraPuZ);
            var cameraCoord = new PointF(cameraRelX, cameraRelZ);
            _cameraMapObj.Draw = _mapGui.MapShowCamera.Checked && puX == cameraPuX && puY == cameraPuY && puZ == cameraPuZ;
            _cameraMapObj.LocationOnContol = CalculateLocationOnControl(cameraCoord, mapView);

            float trianglePuX1 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.X1);
            float trianglePuZ1 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.Z1);
            float trianglePuX2 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.X2);
            float trianglePuZ2 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.Z2);
            float trianglePuX3 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.X3);
            float trianglePuZ3 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.Z3);
            _floorTriangleMapObj.P1OnControl = CalculateLocationOnControl(new PointF(trianglePuX1, trianglePuZ1), mapView);
            _floorTriangleMapObj.P2OnControl = CalculateLocationOnControl(new PointF(trianglePuX2, trianglePuZ2), mapView);
            _floorTriangleMapObj.P3OnControl = CalculateLocationOnControl(new PointF(trianglePuX3, trianglePuZ3), mapView);
            _floorTriangleMapObj.Draw = _floorTriangleMapObj.Show & _mapGui.MapShowFloorTriangle.Checked;
  

            // Calculate object slot's cooridnates
            foreach (var mapObj in _mapObjects)
            {
                if (!_mapGui.MapShowObjects.Checked)
                {
                    mapObj.Draw = false;
                    continue;
                }

                // Make sure the object is in the same PU as Mario
                var objPuX = PuUtilities.GetPUFromCoord(mapObj.X);
                var objPuY = PuUtilities.GetPUFromCoord(mapObj.Y);
                var objPuZ = PuUtilities.GetPUFromCoord(mapObj.Z);

                // Don't draw the object if it is in a separate PU as mario
                mapObj.Draw = (mapObj.Show && objPuX == puX && objPuY == puY && objPuZ == puZ 
                    && (_mapGui.MapShowInactiveObjects.Checked || mapObj.IsActive));
                if (!mapObj.Draw)
                    continue;

                // Adjust object coordinates relative from current PU
                float objPosX = PuUtilities.GetRelativePuPosition(mapObj.X, objPuX);
                float objPosZ = PuUtilities.GetRelativePuPosition(mapObj.Z, objPuZ);
                var objCoords = new PointF(objPosX, objPosZ);

                // Calculate object's location on control
                mapObj.LocationOnContol = CalculateLocationOnControl(objCoords, mapView);
            }

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _mapGraphics.Control.Invalidate();
        }

        private void ChangeCurrentMap(Map map)
        {
            // Don't change the map if it isn't different
            if (_currentMap == map)
                return;

            // Change and set a new map
            using (var mapImage = MapAssoc.GetMapImage(map))
                _mapGraphics.SetMap(mapImage);

            using (var mapBackground = MapAssoc.GetMapBackgroundImage(map))
                _mapGraphics.SetBackground(mapBackground);

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
        
        public void AddMapObject(MapObject mapObj)
        {
            _mapObjects.Add(mapObj);
            _mapGraphics.AddMapObject(mapObj);
        }

        public void RemoveMapObject(MapObject mapObj)
        {
            _mapObjects.Remove(mapObj);
            _mapGraphics.RemoveMapObject(mapObj);
        }
    }
}
