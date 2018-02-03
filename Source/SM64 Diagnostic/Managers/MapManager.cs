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
        public MapAssociations MapAssoc;
        byte _currentLevel, _currentArea;
        ushort _currentLoadingPoint, _currentMissionLayout;
        Map _currentMap;
        List<Map> _currentMapList = null;
        MapGraphics _mapGraphics;
        MapObject _marioMapObj;
        MapObject _holpMapObj;
        MapObject _intendedNextPositionMapObj;
        MapObject _cameraMapObj;
        TriangleMapObject _floorTriangleMapObj;
        TriangleMapObject _ceilingTriangleMapObj;
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

        public MapObject IntendedNextPositionMapObject
        {
            get
            {
                return _intendedNextPositionMapObj;
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

        public TriangleMapObject CeilingTriangleMapObject
        {
            get
            {
                return _ceilingTriangleMapObj;
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

        public MapManager(MapAssociations mapAssoc, MapGui mapGui)
        {
            MapAssoc = mapAssoc;
            _mapGui = mapGui;

            _marioMapObj = new MapObject(Config.ObjectAssociations.MarioMapImage, 1);
            _marioMapObj.UsesRotation = true;

            _holpMapObj = new MapObject(Config.ObjectAssociations.HolpImage, 2);
            _intendedNextPositionMapObj = new MapObject(Config.ObjectAssociations.IntendedNextPositionImage, 2);
            _intendedNextPositionMapObj.UsesRotation = true;

            _cameraMapObj = new MapObject(Config.ObjectAssociations.CameraMapImage, 1);
            _cameraMapObj.UsesRotation = true;
            _floorTriangleMapObj = new TriangleMapObject(Color.FromArgb(200, Color.Cyan), 3);
            _ceilingTriangleMapObj = new TriangleMapObject(Color.FromArgb(200, Color.Red), 2);
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
            _mapGraphics.AddMapObject(_intendedNextPositionMapObj);
            _mapGraphics.AddMapObject(_cameraMapObj);
            _mapGraphics.AddMapObject(_floorTriangleMapObj);
            _mapGraphics.AddMapObject(_ceilingTriangleMapObj);

            //----- Register events ------
            // Set image
            _mapGui.MapIconSizeTrackbar.ValueChanged += (sender, e) => _mapGraphics.IconSize = _mapGui.MapIconSizeTrackbar.Value;

            _mapGui.MapBoundsUpButton.Click += (sender, e) => ChangeMapPosition(0, 1);
            _mapGui.MapBoundsDownButton.Click += (sender, e) => ChangeMapPosition(0, -1);
            _mapGui.MapBoundsLeftButton.Click += (sender, e) => ChangeMapPosition(-1, 0);
            _mapGui.MapBoundsRightButton.Click += (sender, e) => ChangeMapPosition(1, 0);
            _mapGui.MapBoundsUpLeftButton.Click += (sender, e) => ChangeMapPosition(-1, 1);
            _mapGui.MapBoundsUpRightButton.Click += (sender, e) => ChangeMapPosition(1, 1);
            _mapGui.MapBoundsDownLeftButton.Click += (sender, e) => ChangeMapPosition(-1, -1);
            _mapGui.MapBoundsDownRightButton.Click += (sender, e) => ChangeMapPosition(1, -1);

            _mapGui.MapBoundsZoomInButton.Click += (sender, e) => ChangeMapZoom(1);
            _mapGui.MapBoundsZoomOutButton.Click += (sender, e) => ChangeMapZoom(-1);

        }

        private void ChangeMapPosition(int xSign, int ySign)
        {
            int positionChange = ParsingUtilities.ParseInt(_mapGui.MapBoundsPositionTextBox.Text);
            int xChange = positionChange * xSign;
            int yChange = positionChange * ySign;
            int newX = _mapGui.GLControl.Left - xChange;
            int newY = _mapGui.GLControl.Top + yChange;
            _mapGui.GLControl.Location = new Point(newX, newY);
        }

        private void ChangeMapZoom(int sign)
        {
            int change = ParsingUtilities.ParseInt(_mapGui.MapBoundsZoomTextBox.Text);
            int zoomChange = change * sign;
            int newX = _mapGui.GLControl.Left - zoomChange;
            int newY = _mapGui.GLControl.Top - zoomChange;
            int newWidth = _mapGui.GLControl.Width + 2 * zoomChange;
            int newHeight = _mapGui.GLControl.Height + 2 * zoomChange;
            _mapGui.GLControl.SetBounds(newX, newY, newWidth, newHeight);
        }

        public void Update()
        {
            // Make sure the control has successfully loaded
            if (!_isLoaded)
                return;

            // Get level and area
            byte level = Config.Stream.GetByte(MiscConfig.LevelAddress);
            byte area = Config.Stream.GetByte(MiscConfig.AreaAddress);
            ushort loadingPoint = Config.Stream.GetUInt16(MiscConfig.LoadingPointAddress);
            ushort missionLayout = Config.Stream.GetUInt16(MiscConfig.MissionAddress);

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
            int holpPuZ = PuUtilities.GetPUFromCoord(_holpMapObj.Z);
            float holpRelX = PuUtilities.GetRelativePuPosition(_holpMapObj.X, holpPuX);
            float holpRelZ = PuUtilities.GetRelativePuPosition(_holpMapObj.Z, holpPuZ);
            var holpCoord = new PointF(holpRelX, holpRelZ);
            _holpMapObj.Draw = _mapGui.MapShowHolp.Checked;
            _holpMapObj.LocationOnContol = CalculateLocationOnControl(holpCoord, mapView);

            int intendedNextPositionPuX = PuUtilities.GetPUFromCoord(_intendedNextPositionMapObj.X);
            int intendedNextPositionPuZ = PuUtilities.GetPUFromCoord(_intendedNextPositionMapObj.Z);
            float intendedNextPositionRelX = PuUtilities.GetRelativePuPosition(_intendedNextPositionMapObj.X, intendedNextPositionPuX);
            float intendedNextPositionRelZ = PuUtilities.GetRelativePuPosition(_intendedNextPositionMapObj.Z, intendedNextPositionPuZ);
            var intendedNextPositionCoord = new PointF(intendedNextPositionRelX, intendedNextPositionRelZ);
            _intendedNextPositionMapObj.Draw = _mapGui.MapShowIntendedNextPosition.Checked;
            _intendedNextPositionMapObj.LocationOnContol = CalculateLocationOnControl(intendedNextPositionCoord, mapView);

            int cameraPuX = PuUtilities.GetPUFromCoord(_cameraMapObj.X);
            int cameraPuY = PuUtilities.GetPUFromCoord(_cameraMapObj.Y);
            int cameraPuZ = PuUtilities.GetPUFromCoord(_cameraMapObj.Z);
            float cameraRelX = PuUtilities.GetRelativePuPosition(_cameraMapObj.X, cameraPuX);
            float cameraRelZ = PuUtilities.GetRelativePuPosition(_cameraMapObj.Z, cameraPuZ);
            var cameraCoord = new PointF(cameraRelX, cameraRelZ);
            _cameraMapObj.Draw = _mapGui.MapShowCamera.Checked;
            _cameraMapObj.LocationOnContol = CalculateLocationOnControl(cameraCoord, mapView);

            float floorTrianglePuX1 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.X1);
            float floorTrianglePuZ1 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.Z1);
            float floorTrianglePuX2 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.X2);
            float floorTrianglePuZ2 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.Z2);
            float floorTrianglePuX3 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.X3);
            float floorTrianglePuZ3 = PuUtilities.GetRelativePuPosition(_floorTriangleMapObj.Z3);
            _floorTriangleMapObj.P1OnControl = CalculateLocationOnControl(new PointF(floorTrianglePuX1, floorTrianglePuZ1), mapView);
            _floorTriangleMapObj.P2OnControl = CalculateLocationOnControl(new PointF(floorTrianglePuX2, floorTrianglePuZ2), mapView);
            _floorTriangleMapObj.P3OnControl = CalculateLocationOnControl(new PointF(floorTrianglePuX3, floorTrianglePuZ3), mapView);
            _floorTriangleMapObj.Draw = _floorTriangleMapObj.Show & _mapGui.MapShowFloorTriangle.Checked;

            float ceilingTrianglePuX1 = PuUtilities.GetRelativePuPosition(_ceilingTriangleMapObj.X1);
            float ceilingTrianglePuZ1 = PuUtilities.GetRelativePuPosition(_ceilingTriangleMapObj.Z1);
            float ceilingTrianglePuX2 = PuUtilities.GetRelativePuPosition(_ceilingTriangleMapObj.X2);
            float ceilingTrianglePuZ2 = PuUtilities.GetRelativePuPosition(_ceilingTriangleMapObj.Z2);
            float ceilingTrianglePuX3 = PuUtilities.GetRelativePuPosition(_ceilingTriangleMapObj.X3);
            float ceilingTrianglePuZ3 = PuUtilities.GetRelativePuPosition(_ceilingTriangleMapObj.Z3);
            _ceilingTriangleMapObj.P1OnControl = CalculateLocationOnControl(new PointF(ceilingTrianglePuX1, ceilingTrianglePuZ1), mapView);
            _ceilingTriangleMapObj.P2OnControl = CalculateLocationOnControl(new PointF(ceilingTrianglePuX2, ceilingTrianglePuZ2), mapView);
            _ceilingTriangleMapObj.P3OnControl = CalculateLocationOnControl(new PointF(ceilingTrianglePuX3, ceilingTrianglePuZ3), mapView);
            _ceilingTriangleMapObj.Draw = _ceilingTriangleMapObj.Show & _mapGui.MapShowCeilingTriangle.Checked;

            // Calculate object slot's cooridnates
            foreach (var mapObj in _mapObjects)
            {
                mapObj.Draw = (mapObj.Show && (_mapGui.MapShowInactiveObjects.Checked || mapObj.IsActive));
                if (!mapObj.Draw)
                    continue;

                // Adjust object coordinates relative from current PU
                var objPuX = PuUtilities.GetPUFromCoord(mapObj.X);
                var objPuZ = PuUtilities.GetPUFromCoord(mapObj.Z);
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
