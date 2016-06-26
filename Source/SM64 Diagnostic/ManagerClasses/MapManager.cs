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
        public MapAssociations MapAssoc;
        byte _currentLevel, _currentArea;
        ushort _currentLoadingPoint;
        Map _currentMap;
        List<Map> _currentMapList = null;
        MapGraphicsControl _mapGraphics;
        MapObject _marioMapObj;
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
            MapAssoc = mapAssoc;
            _mapGui = mapGui;

            _marioMapObj = new MapObject(new Bitmap("Resources\\Maps\\Object Images\\Mario Top.png"), 1);
            _marioMapObj.UsesRotation = true;
        }

        public void Load()
        {
            // Create new graphics control
            _mapGraphics = new MapGraphicsControl(_mapGui.GLControl);
            _mapGraphics.Load();

            _isLoaded = true;

            // Set the default map
            ChangeCurrentMap(MapAssoc.DefaultMap);

            // Add Mario's map object
            _mapGraphics.AddMapObject(_marioMapObj);

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
            byte level = _stream.ReadRam(_config.LevelAddress, 1)[0];
            byte area = _stream.ReadRam(_config.AreaAddress, 1)[0];
            ushort loadingPoint = BitConverter.ToUInt16(_stream.ReadRam(_config.LoadingPointAddress, 2), 0);

            // Find new map list
            if (_currentMapList == null || _currentLevel != level || _currentArea != area || _currentLoadingPoint != loadingPoint)
            {
                _currentLevel = level;
                _currentArea = area;
                _currentLoadingPoint = loadingPoint;
                _currentMapList = MapAssoc.GetLevelAreaMaps(level, area);

                // Look for maps with correct loading points
                var mapListLPFiltered = _currentMapList.Where((map) => map.LoadingPoint == loadingPoint).ToList();
                if (mapListLPFiltered.Count > 0)
                    _currentMapList = mapListLPFiltered;
            }

            // Filter out all maps that are lower than Mario
            var mapListYFiltered = _currentMapList.Where((map) => map.Y >= _marioMapObj.Y).OrderByDescending((map) => map.Y).ToList();

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
                    if (map.Y < bestMap.Y)
                        bestMap = map;
                }

                ChangeCurrentMap(bestMap);
            }

            // ---- Update PU -----
            var marioCoord = new PointF(_marioMapObj.X, _marioMapObj.Z);
            var marioPu = GetPUFromCoord(marioCoord);
            int puX = marioPu.X;
            int puZ = marioPu.Y;

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
            marioCoord = GetRelativePuPosition(marioCoord, marioPu);

            // Calculate mario's location on the OpenGl control
            var mapView = _mapGraphics.MapView;
            _marioMapObj.LocationOnContol = CalculateLocationOnControl(marioCoord, mapView);
            _marioMapObj.Draw = _mapGui.MapShowMario.Checked;

            // Calculate object slot's cooridnates
            foreach (var mapObj in _mapObjects)
            {
                // Make sure the object is in the same PU as Mario
                var objCoords = new PointF(mapObj.X, mapObj.Z);
                var objPu = GetPUFromCoord(objCoords);

                // Don't draw the object if it is in a separate PU as mario
                mapObj.Draw = (mapObj.Show && objPu == marioPu && (_mapGui.MapShowInactiveObjects.Checked || mapObj.IsActive));
                if (!mapObj.Draw)
                    continue;

                // Adjust object coordinates relative from current PU
                objCoords = GetRelativePuPosition(objCoords, objPu);

                // Calculate object's location on control
                mapObj.LocationOnContol = CalculateLocationOnControl(objCoords, mapView);
            }

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _mapGraphics.Control.Invalidate();
        }

        private static Point GetPUFromCoord(PointF coord)
        {
            int puX = (int)((coord.X + 32768) / 65536);
            int puZ = (int)((coord.Y + 32768) / 65536);

            // If the object is located in the center of the (-1,-1) pu its coordinates will be (-0.5, -0.5). 
            // Because we used division this rounds down to (0,0), which is incorrect, we therefore add -1 to all negative PUs
            if (coord.X < -32768)
                puX--;
            if (coord.Y < -32768)
                puZ--;

            return new Point(puX, puZ);
        }

        private static PointF GetRelativePuPosition(PointF coord, Point puCoord)
        {
            // We find the relative object positon by subtracting the PU starting coordinates from the object
            return new PointF(coord.X - puCoord.X * 65535, coord.Y - puCoord.Y * 65535);
        }

        private void ChangeCurrentMap(Map map)
        {
            // Don't change the map if it isn't different
            if (_currentMap == map)
                return;

            // Change and set a new map
            _mapGraphics.SetMap(MapAssoc.GetMapImage(map));
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
