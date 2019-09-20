using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Models;
using STROOP.Utilities;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using STROOP.Structs.Configurations;
using STROOP.Controls.Map;
using STROOP.Map3;

namespace STROOP.Managers
{
    public class Map3Manager
    {
        public MapLayout map;
        byte _currentLevel, _currentArea;
        ushort _currentLoadingPoint, _currentMissionLayout;
        MapLayout _currentMap;
        List<MapLayout> _currentMapList = null;
        Map3Graphics _mapGraphics;
        Map3Object _marioMapObj;
        Map3Object _holpMapObj;
        Map3Object _intendedNextPositionMapObj;
        Map3Object _pointMapObj;
        Map3Object _cameraMapObj;
        TriangleMap3Object _floorTriangleMapObj;
        TriangleMap3Object _ceilingTriangleMapObj;

        List<TriangleMap3Object> _cogFloorTris;
        List<TriangleMap3Object> _cog2FloorTris;
        List<TriangleMap3Object> _cogWallTris;

        int SHAPE_MIN_SIDES = 3;
        int SHAPE_MAX_SIDSE = 8;

        List<List<TriangleMap3Object>> _triObjectWalls;
        List<List<TriangleMap3Object>> _triObjectFloors;

        List<Map3Object> _mapObjects = new List<Map3Object>();
        public Dictionary<uint, Map3Object> _mapObjectDictionary = new Dictionary<uint, Map3Object>();

        List<Map3Object> _mapObjectHomes = new List<Map3Object>();
        public Dictionary<uint, Map3Object> _mapObjectHomeDictionary = new Dictionary<uint, Map3Object>();

        Map3Gui _mapGui;
        bool _isLoaded = false;
        float? _artificialMarioY = null;

        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
        }

        public Map3Object MarioMapObject
        {
            get
            {
                return _marioMapObj;
            }
        }

        public Map3Object HolpMapObject
        {
            get
            {
                return _holpMapObj;
            }
        }

        public Map3Object IntendedNextPositionMapObject
        {
            get
            {
                return _intendedNextPositionMapObj;
            }
        }

        public Map3Object PointMapObj
        {
            get
            {
                return _pointMapObj;
            }
        }

        public Map3Object CameraMapObject
        {
            get
            {
                return _cameraMapObj;
            }
        }

        public TriangleMap3Object FloorTriangleMapObject
        {
            get
            {
                return _floorTriangleMapObj;
            }
        }

        public TriangleMap3Object CeilingTriangleMapObject
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

        public Map3Manager(Map3Gui mapGui)
        {
            _mapGui = mapGui;

            _marioMapObj = new Map3Object(Config.ObjectAssociations.MarioMapImage, 1);
            _marioMapObj.UsesRotation = true;

            _holpMapObj = new Map3Object(Config.ObjectAssociations.HolpImage, 2);

            _intendedNextPositionMapObj = new Map3Object(Config.ObjectAssociations.IntendedNextPositionImage, 2);
            _intendedNextPositionMapObj.UsesRotation = true;

            _pointMapObj = new Map3Object(Config.ObjectAssociations.IntendedNextPositionImage, 2);
            _pointMapObj.UsesRotation = true;

            _cameraMapObj = new Map3Object(Config.ObjectAssociations.CameraMapImage, 1);
            _cameraMapObj.UsesRotation = true;
            _floorTriangleMapObj = new TriangleMap3Object(Color.FromArgb(200, Color.Cyan), 3);
            _ceilingTriangleMapObj = new TriangleMap3Object(Color.FromArgb(200, Color.Red), 2);

            _cogFloorTris = new List<TriangleMap3Object>();
            for (int i = 0; i < 4; i++)
            {
                _cogFloorTris.Add(new TriangleMap3Object(Color.FromArgb(200, Color.Cyan), 3));
            }

            _cog2FloorTris = new List<TriangleMap3Object>();
            for (int i = 0; i < 4; i++)
            {
                _cog2FloorTris.Add(new TriangleMap3Object(Color.FromArgb(200, Color.Magenta), 3));
            }

            _cogWallTris = new List<TriangleMap3Object>();
            for (int i = 0; i < 12; i++)
            {
                _cogWallTris.Add(new TriangleMap3Object(Color.FromArgb(200, Color.Green), 3));
            }

            _triObjectWalls = new List<List<TriangleMap3Object>>();
            _triObjectFloors = new List<List<TriangleMap3Object>>();
            for (int numSides = SHAPE_MIN_SIDES; numSides <= SHAPE_MAX_SIDSE; numSides++)
            {
                (List<TriangleShape> floors, List<TriangleShape> walls) = GetTriShapes(numSides);

                List<TriangleMap3Object> wallTris = new List<TriangleMap3Object>();
                foreach (TriangleShape tri in walls)
                {
                    wallTris.Add(new TriangleMap3Object(Color.FromArgb(200, Color.Green), 3));
                }
                _triObjectWalls.Add(wallTris);

                List<TriangleMap3Object> floorTris = new List<TriangleMap3Object>();
                foreach (TriangleShape tri in floors)
                {
                    floorTris.Add(new TriangleMap3Object(Color.FromArgb(200, Color.Cyan), 3));
                }
                _triObjectFloors.Add(floorTris);
            }
        }

        public void Load()
        {
            // Create new graphics control
            _mapGraphics = new Map3Graphics(_mapGui.GLControl);
            _mapGraphics.Load();
            _mapGraphics.IconSize = _mapGui.MapIconSizeTrackbar.Value;

            _isLoaded = true;

            // Set the default map
            ChangeCurrentMap(Config.MapAssociations.DefaultMap);

            // Add Mario's map object
            _mapGraphics.AddMapObject(_marioMapObj);
            _mapGraphics.AddMapObject(_holpMapObj);
            _mapGraphics.AddMapObject(_intendedNextPositionMapObj);
            _mapGraphics.AddMapObject(_pointMapObj);
            _mapGraphics.AddMapObject(_cameraMapObj);
            _mapGraphics.AddMapObject(_floorTriangleMapObj);
            _mapGraphics.AddMapObject(_ceilingTriangleMapObj);

            _cogFloorTris.ForEach(tri => _mapGraphics.AddMapObject(tri));
            _cog2FloorTris.ForEach(tri => _mapGraphics.AddMapObject(tri));
            _cogWallTris.ForEach(tri => _mapGraphics.AddMapObject(tri));

            foreach (List<TriangleMap3Object> floorTris in _triObjectFloors)
            {
                foreach (TriangleMap3Object floorTri in floorTris)
                {
                    _mapGraphics.AddMapObject(floorTri);
                }
            }

            foreach (List<TriangleMap3Object> wallTris in _triObjectWalls)
            {
                foreach (TriangleMap3Object wallTri in wallTris)
                {
                    _mapGraphics.AddMapObject(wallTri);
                }
            }

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

            _mapGui.MapArtificialMarioYLabelTextBox.AddEnterAction(() =>
                _artificialMarioY = ParsingUtilities.ParseFloatNullable(
                    _mapGui.MapArtificialMarioYLabelTextBox.Text));

            ControlUtilities.AddContextMenuStripFunctions(
                _mapGui.GLControl.Parent,
                new List<string>() { "Fill Screen", "Copy Map Settings", "Paste Map Settings" },
                new List<Action>()
                {
                    () => ChangeMapFillScreen(),
                    () => CopyMapSettings(),
                    () => PasteMapSettings(),
                });
        }

        private void ChangeMapFillToNone()
        {
            if (_mapGui.GLControl.Dock != DockStyle.None)
            {
                Control parent = _mapGui.GLControl.Parent;
                _mapGui.GLControl.SetBounds(0, 0, parent.Width, parent.Height);
                _mapGui.GLControl.Dock = DockStyle.None;
            }
        }

        private void ChangeMapPosition(int xSign, int ySign)
        {
            ChangeMapFillToNone();
            int positionChange = ParsingUtilities.ParseInt(_mapGui.MapBoundsPositionTextBox.Text);
            int xChange = positionChange * xSign;
            int yChange = positionChange * ySign;
            int newX = _mapGui.GLControl.Left - xChange;
            int newY = _mapGui.GLControl.Top + yChange;
            _mapGui.GLControl.Location = new Point(newX, newY);
        }

        private void ChangeMapZoom(int sign)
        {
            ChangeMapFillToNone();
            int change = ParsingUtilities.ParseInt(_mapGui.MapBoundsZoomTextBox.Text);
            int zoomChange = change * sign;
            double zoomMultiply = (zoomChange + 100d) / 100d;
            if (zoomMultiply <= 0) return;
            int newWidth = (int)(_mapGui.GLControl.Width * zoomMultiply);
            int newHeight = (int)(_mapGui.GLControl.Height * zoomMultiply);
            if (newWidth > 30000 || newHeight > 30000) return;
            if (newWidth <= 1 || newHeight <= 1) return;

            Control parent = _mapGui.GLControl.Parent;
            int centerX = parent.Width / 2;
            int centerY = parent.Height / 2;
            double percentageX = (centerX - _mapGui.GLControl.Left) / (double) _mapGui.GLControl.Width;
            double percentageY = (centerY - _mapGui.GLControl.Top) / (double) _mapGui.GLControl.Height;

            int newCenterX = (int)(percentageX * newWidth);
            int newCenterY = (int)(percentageY * newHeight);
            int newX = centerX - newCenterX;
            int newY = centerY - newCenterY;

            _mapGui.GLControl.SetBounds(newX, newY, newWidth, newHeight);
        }

        private void ChangeMapFillScreen()
        {
            _mapGui.GLControl.Dock = DockStyle.Fill;
        }

        private void CopyMapSettings()
        {
            SplitContainer innerSplitContainer = ControlUtilities.GetAncestorSplitContainer(_mapGui.MapNameLabel);
            SplitContainer outerSplitContainer = ControlUtilities.GetAncestorSplitContainer(innerSplitContainer);
            List<object> values = new List<object>()
            {
                _mapGui.GLControl.Dock == DockStyle.Fill,
                _mapGui.GLControl.Bounds.X,
                _mapGui.GLControl.Bounds.Y,
                _mapGui.GLControl.Bounds.Width,
                _mapGui.GLControl.Bounds.Height,
                innerSplitContainer.Panel1Collapsed,
                innerSplitContainer.Panel2Collapsed,
                innerSplitContainer.SplitterDistance,
                outerSplitContainer.Panel1Collapsed,
                outerSplitContainer.Panel2Collapsed,
                outerSplitContainer.SplitterDistance,
                Config.StroopMainForm.Bounds.X,
                Config.StroopMainForm.Bounds.Y,
                Config.StroopMainForm.Bounds.Width,
                Config.StroopMainForm.Bounds.Height,
                _mapGui.MapIconSizeTrackbar.Value,
            };
            Clipboard.SetText(string.Join(",", values));
        }

        private void PasteMapSettings()
        {
            List<string> values = ParsingUtilities.ParseStringList(Clipboard.GetText());
            if (values.Count != 16) return;

            SplitContainer innerSplitContainer = ControlUtilities.GetAncestorSplitContainer(_mapGui.MapNameLabel);
            SplitContainer outerSplitContainer = ControlUtilities.GetAncestorSplitContainer(innerSplitContainer);

            _mapGui.GLControl.Dock = ParsingUtilities.ParseBool(values[0]) ? DockStyle.Fill : DockStyle.None;
            _mapGui.GLControl.SetBounds(
                ParsingUtilities.ParseInt(values[1]),
                ParsingUtilities.ParseInt(values[2]),
                ParsingUtilities.ParseInt(values[3]),
                ParsingUtilities.ParseInt(values[4]));
            innerSplitContainer.Panel1Collapsed = ParsingUtilities.ParseBool(values[5]);
            innerSplitContainer.Panel2Collapsed = ParsingUtilities.ParseBool(values[6]);
            innerSplitContainer.SplitterDistance = ParsingUtilities.ParseInt(values[7]);
            outerSplitContainer.Panel1Collapsed = ParsingUtilities.ParseBool(values[8]);
            outerSplitContainer.Panel2Collapsed = ParsingUtilities.ParseBool(values[9]);
            outerSplitContainer.SplitterDistance = ParsingUtilities.ParseInt(values[10]);
            Config.StroopMainForm.SetBounds(
                ParsingUtilities.ParseInt(values[11]),
                ParsingUtilities.ParseInt(values[12]),
                ParsingUtilities.ParseInt(values[13]),
                ParsingUtilities.ParseInt(values[14]));
            _mapGui.MapIconSizeTrackbar.Value = ParsingUtilities.ParseInt(values[15]);
            _mapGraphics.IconSize = _mapGui.MapIconSizeTrackbar.Value;
        }

        private (List<TriangleShape> floors, List<TriangleShape> walls) GetTriShapes(int numSides)
        {
            double dist = 850;
            double radius = 300;
            double xOffset = 1300;
            double zOffset = 0;
            double angleScale = 256;

            double globalTimerAngle;
            if (Config.Stream != null)
            {
                uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                ushort globalTimerUShort = (ushort)(globalTimer % 65536);
                globalTimerAngle = -1 * globalTimerUShort * angleScale;
            }
            else
            {
                globalTimerAngle = 0;
            }

            int index = numSides - SHAPE_MIN_SIDES;
            double x = (index % 3) * dist + xOffset;
            double z = (index / 3) * dist + zOffset;
            return TriangleUtilities.GetWallFoorTrianglesForShape(numSides, radius, globalTimerAngle, x, z);
        }

        public void UpdateFromMarioTab()
        {
            // Get Mario position and rotation
            float x = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float y = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float z = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort marioFacing = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            float rot = (float)MoreMath.AngleUnitsToDegrees(marioFacing);

            // Update Mario map object
            MarioMapObject.X = x;
            MarioMapObject.Y = y;
            MarioMapObject.Z = z;
            MarioMapObject.Rotation = rot;
            MarioMapObject.Show = true;
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
            if (!_isLoaded) return;

            UpdateFromMarioTab();

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
                _currentMapList = Config.MapAssociations.GetLevelAreaMaps(level, area);

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

            var marioCoord = new PointF(_marioMapObj.RelX, _marioMapObj.RelZ);

            // Filter out all maps that are lower than Mario
            float marioY = _marioMapObj.RelY;
            var mapListYFiltered = _currentMapList.Where((map) => map.Y <= marioY).ToList();

            // If no map is available display the default image
            if (mapListYFiltered.Count <= 0)
            {
                ChangeCurrentMap(Config.MapAssociations.DefaultMap);
            }
            else
            {
                // Pick the map closest to mario (yet still above Mario)
                MapLayout bestMap = mapListYFiltered[0];
                foreach (MapLayout map in mapListYFiltered)
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

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _mapGraphics.Control.Invalidate();
        }

        private void ChangeCurrentMap(MapLayout map)
        {
            // Don't change the map if it isn't different
            if (_currentMap == map)
                return;

            // Change and set a new map
            _mapGraphics.SetMap(map.MapImage);
            _mapGraphics.SetBackground(map.BackgroundImage);

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

        public void AddMapObjectHome(Map3Object mapObj)
        {
            _mapObjectHomes.Add(mapObj);
            _mapGraphics.AddMapObject(mapObj);
        }

        public void RemoveMapObjectHome(Map3Object mapObj)
        {
            _mapObjectHomes.Remove(mapObj);
            _mapGraphics.RemoveMapObject(mapObj);
        }

        public void AddMapObject(Map3Object mapObj)
        {
            _mapObjects.Add(mapObj);
            _mapGraphics.AddMapObject(mapObj);
        }

        public void RemoveMapObject(Map3Object mapObj)
        {
            _mapObjects.Remove(mapObj);
            _mapGraphics.RemoveMapObject(mapObj);
        }
    }
}
