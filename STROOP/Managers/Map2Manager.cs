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
using STROOP.Map2;

namespace STROOP.Managers
{
    public class Map2Manager
    {
        public MapLayout map;
        byte _currentLevel, _currentArea;
        ushort _currentLoadingPoint, _currentMissionLayout;
        MapLayout _currentMap;
        List<MapLayout> _currentMapList = null;
        Map2Graphics _mapGraphics;
        Map2Object _marioMapObj;
        Map2Object _holpMapObj;
        Map2Object _intendedNextPositionMapObj;
        Map2Object _cameraMapObj;
        TriangleMap2Object _floorTriangleMapObj;
        TriangleMap2Object _ceilingTriangleMapObj;

        List<TriangleMap2Object> _cogFloorTris;
        List<TriangleMap2Object> _cog2FloorTris;
        List<TriangleMap2Object> _cogWallTris;

        int SHAPE_MIN_SIDES = 3;
        int SHAPE_MAX_SIDSE = 8;

        List<List<TriangleMap2Object>> _triObjectWalls;
        List<List<TriangleMap2Object>> _triObjectFloors;

        List<Map2Object> _mapObjects = new List<Map2Object>();
        public Dictionary<uint, Map2Object> _mapObjectDictionary = new Dictionary<uint, Map2Object>();
        Map2Gui _mapGui;
        bool _isLoaded = false;
        float? _artificialMarioY = null;

        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
        }

        public Map2Object MarioMapObject
        {
            get
            {
                return _marioMapObj;
            }
        }

        public Map2Object HolpMapObject
        {
            get
            {
                return _holpMapObj;
            }
        }

        public Map2Object IntendedNextPositionMapObject
        {
            get
            {
                return _intendedNextPositionMapObj;
            }
        }

        public Map2Object CameraMapObject
        {
            get
            {
                return _cameraMapObj;
            }
        }

        public TriangleMap2Object FloorTriangleMapObject
        {
            get
            {
                return _floorTriangleMapObj;
            }
        }

        public TriangleMap2Object CeilingTriangleMapObject
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

        public Map2Manager(Map2Gui mapGui)
        {
            _mapGui = mapGui;

            _marioMapObj = new Map2Object(Config.ObjectAssociations.MarioMapImage, 1);
            _marioMapObj.UsesRotation = true;

            _holpMapObj = new Map2Object(Config.ObjectAssociations.HolpImage, 2);
            _intendedNextPositionMapObj = new Map2Object(Config.ObjectAssociations.IntendedNextPositionImage, 2);
            _intendedNextPositionMapObj.UsesRotation = true;

            _cameraMapObj = new Map2Object(Config.ObjectAssociations.CameraMapImage, 1);
            _cameraMapObj.UsesRotation = true;
            _floorTriangleMapObj = new TriangleMap2Object(Color.FromArgb(200, Color.Cyan), 3);
            _ceilingTriangleMapObj = new TriangleMap2Object(Color.FromArgb(200, Color.Red), 2);

            _cogFloorTris = new List<TriangleMap2Object>();
            for (int i = 0; i < 4; i++)
            {
                _cogFloorTris.Add(new TriangleMap2Object(Color.FromArgb(200, Color.Cyan), 3));
            }

            _cog2FloorTris = new List<TriangleMap2Object>();
            for (int i = 0; i < 4; i++)
            {
                _cog2FloorTris.Add(new TriangleMap2Object(Color.FromArgb(200, Color.Magenta), 3));
            }

            _cogWallTris = new List<TriangleMap2Object>();
            for (int i = 0; i < 12; i++)
            {
                _cogWallTris.Add(new TriangleMap2Object(Color.FromArgb(200, Color.Green), 3));
            }

            _triObjectWalls = new List<List<TriangleMap2Object>>();
            _triObjectFloors = new List<List<TriangleMap2Object>>();
            for (int numSides = SHAPE_MIN_SIDES; numSides <= SHAPE_MAX_SIDSE; numSides++)
            {
                (List<TriangleShape> floors, List<TriangleShape> walls) = GetTriShapes(numSides);

                List<TriangleMap2Object> wallTris = new List<TriangleMap2Object>();
                foreach (TriangleShape tri in walls)
                {
                    wallTris.Add(new TriangleMap2Object(Color.FromArgb(200, Color.Green), 3));
                }
                _triObjectWalls.Add(wallTris);

                List<TriangleMap2Object> floorTris = new List<TriangleMap2Object>();
                foreach (TriangleShape tri in floors)
                {
                    floorTris.Add(new TriangleMap2Object(Color.FromArgb(200, Color.Cyan), 3));
                }
                _triObjectFloors.Add(floorTris);
            }
        }

        public void Load()
        {
            // Create new graphics control
            _mapGraphics = new Map2Graphics(_mapGui.GLControl);
            _mapGraphics.Load();

            _isLoaded = true;

            // Set the default map
            ChangeCurrentMap(Config.MapAssociations.DefaultMap);

            // Add Mario's map object
            _mapGraphics.AddMapObject(_marioMapObj);
            _mapGraphics.AddMapObject(_holpMapObj);
            _mapGraphics.AddMapObject(_intendedNextPositionMapObj);
            _mapGraphics.AddMapObject(_cameraMapObj);
            _mapGraphics.AddMapObject(_floorTriangleMapObj);
            _mapGraphics.AddMapObject(_ceilingTriangleMapObj);

            _cogFloorTris.ForEach(tri => _mapGraphics.AddMapObject(tri));
            _cog2FloorTris.ForEach(tri => _mapGraphics.AddMapObject(tri));
            _cogWallTris.ForEach(tri => _mapGraphics.AddMapObject(tri));

            foreach (List<TriangleMap2Object> floorTris in _triObjectFloors)
            {
                foreach (TriangleMap2Object floorTri in floorTris)
                {
                    _mapGraphics.AddMapObject(floorTri);
                }
            }

            foreach (List<TriangleMap2Object> wallTris in _triObjectWalls)
            {
                foreach (TriangleMap2Object wallTri in wallTris)
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
        }

        private void ChangeMapPosition(int xSign, int ySign)
        {
            _mapGui.GLControl.Dock = DockStyle.None;
            int positionChange = ParsingUtilities.ParseInt(_mapGui.MapBoundsPositionTextBox.Text);
            int xChange = positionChange * xSign;
            int yChange = positionChange * ySign;
            int newX = _mapGui.GLControl.Left - xChange;
            int newY = _mapGui.GLControl.Top + yChange;
            _mapGui.GLControl.Location = new Point(newX, newY);
        }

        private void ChangeMapZoom(int sign)
        {
            _mapGui.GLControl.Dock = DockStyle.None;
            int change = ParsingUtilities.ParseInt(_mapGui.MapBoundsZoomTextBox.Text);
            int zoomChange = change * sign;
            int newX = _mapGui.GLControl.Left - zoomChange;
            int newY = _mapGui.GLControl.Top - zoomChange;
            int newWidth = _mapGui.GLControl.Width + 2 * zoomChange;
            int newHeight = _mapGui.GLControl.Height + 2 * zoomChange;
            _mapGui.GLControl.SetBounds(newX, newY, newWidth, newHeight);
        }

        private (List<TriangleShape> floors, List<TriangleShape> walls) GetTriShapes(int numSides)
        {
            double dist = 900;
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

            // Get holp position
            float holpX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpXOffset);
            float holpY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpYOffset);
            float holpZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpZOffset);

            // Update holp map object position
            HolpMapObject.X = holpX;
            HolpMapObject.Y = holpY;
            HolpMapObject.Z = holpZ;
            HolpMapObject.Show = true;

            // Update camera position and rotation
            float cameraX = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.XOffset);
            float cameraY = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.YOffset);
            float cameraZ = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.ZOffset);
            ushort cameraYaw = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
            float cameraRot = (float)MoreMath.AngleUnitsToDegrees(cameraYaw);

            // Update floor triangle
            UInt32 floorTriangle = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            if (floorTriangle != 0x00)
            {
                Int16 x1 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.X1);
                Int16 y1 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Y1);
                Int16 z1 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Z1);
                Int16 x2 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.X2);
                Int16 y2 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Y2);
                Int16 z2 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Z2);
                Int16 x3 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.X3);
                Int16 y3 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Y3);
                Int16 z3 = Config.Stream.GetInt16(floorTriangle + TriangleOffsetsConfig.Z3);
                FloorTriangleMapObject.X1 = x1;
                FloorTriangleMapObject.Z1 = z1;
                FloorTriangleMapObject.X2 = x2;
                FloorTriangleMapObject.Z2 = z2;
                FloorTriangleMapObject.X3 = x3;
                FloorTriangleMapObject.Z3 = z3;
                FloorTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            FloorTriangleMapObject.Show = (floorTriangle != 0x00);

            // Update ceiling triangle
            UInt32 ceilingTriangle = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
            if (ceilingTriangle != 0x00)
            {
                Int16 x1 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.X1);
                Int16 y1 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Y1);
                Int16 z1 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Z1);
                Int16 x2 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.X2);
                Int16 y2 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Y2);
                Int16 z2 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Z2);
                Int16 x3 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.X3);
                Int16 y3 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Y3);
                Int16 z3 = Config.Stream.GetInt16(ceilingTriangle + TriangleOffsetsConfig.Z3);
                CeilingTriangleMapObject.X1 = x1;
                CeilingTriangleMapObject.Z1 = z1;
                CeilingTriangleMapObject.X2 = x2;
                CeilingTriangleMapObject.Z2 = z2;
                CeilingTriangleMapObject.X3 = x3;
                CeilingTriangleMapObject.Z3 = z3;
                CeilingTriangleMapObject.Y = (y1 + y2 + y3) / 3;
            }
            CeilingTriangleMapObject.Show = (ceilingTriangle != 0x00);

            //List<TriangleDataModel> cogFloorTris = TriangleUtilities.GetObjectTrianglesForObject(0x80341E28)
            List<TriangleDataModel> cogFloorTris = TriangleUtilities.GetTrianglesInRange(0x8016DE30, 20)
                .FindAll(tri => tri.Classification == TriangleClassification.Floor);
            for (int i = 0; i < _cogFloorTris.Count; i++)
            {
                if (i < cogFloorTris.Count)
                {
                    _cogFloorTris[i].Update(cogFloorTris[i]);
                    _cogFloorTris[i].Show = true;
                }
                else
                {
                    _cogFloorTris[i].Show = false;
                }
            }

            //List<TriangleDataModel> cog2FloorTris = TriangleUtilities.GetObjectTrianglesForObject(0x80342088)
            List<TriangleDataModel> cog2FloorTris = TriangleUtilities.GetTrianglesInRange(0x8016E1F0, 20)
                .FindAll(tri => tri.Classification == TriangleClassification.Floor);
            for (int i = 0; i < _cog2FloorTris.Count; i++)
            {
                if (i < cog2FloorTris.Count)
                {
                    _cog2FloorTris[i].Update(cog2FloorTris[i]);
                    _cog2FloorTris[i].Show = true;
                }
                else
                {
                    _cog2FloorTris[i].Show = false;
                }
            }

            List<TriangleShape> cogWallTris = TriangleUtilities.GetWallTriangleHitboxComponents(
                TriangleUtilities.GetObjectTrianglesForObject(0x80341E28)
                    .FindAll(tri => tri.Classification == TriangleClassification.Wall));
            for (int i = 0; i < _cogWallTris.Count; i++)
            {
                if (i < cogWallTris.Count)
                {
                    _cogWallTris[i].Update(cogWallTris[i]);
                    _cogWallTris[i].Show = true;
                }
                else
                {
                    _cogWallTris[i].Show = false;
                }
            }

            for (int numSides = SHAPE_MIN_SIDES; numSides <= SHAPE_MAX_SIDSE; numSides++)
            {
                (List<TriangleShape> floors, List<TriangleShape> walls) = GetTriShapes(numSides);
                int index = numSides - SHAPE_MIN_SIDES;
                List<TriangleMap2Object> floorTris = _triObjectFloors[index];
                List<TriangleMap2Object> wallTris = _triObjectWalls[index];
                for (int i = 0; i < floorTris.Count; i++)
                {
                    floorTris[i].Update(floors[i]);
                    floorTris[i].Show = true;
                }
                for (int i = 0; i < wallTris.Count; i++)
                {
                    wallTris[i].Update(walls[i]);
                    wallTris[i].Show = true;
                }
            }

            // Update intended next position map object position
            float normY = floorTriangle == 0 ? 1 : Config.Stream.GetSingle(floorTriangle + TriangleOffsetsConfig.NormY);
            float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
            bool aboveFloor = y > floorY + 0.001;
            double multiplier = aboveFloor ? 1 : normY;
            double defactoSpeed = hSpeed * multiplier;
            double defactoSpeedQStep = defactoSpeed * 0.25;
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            ushort marioAngleTruncated = MoreMath.NormalizeAngleTruncated(marioAngle);
            (double xDist, double zDist) = MoreMath.GetComponentsFromVector(defactoSpeedQStep, marioAngleTruncated);
            double intendedNextPositionX = MoreMath.MaybeNegativeModulus(x + xDist, 65536);
            double intendedNextPositionZ = MoreMath.MaybeNegativeModulus(z + zDist, 65536);
            IntendedNextPositionMapObject.X = (float)intendedNextPositionX;
            IntendedNextPositionMapObject.Z = (float)intendedNextPositionZ;
            bool marioStationary = x == intendedNextPositionX && z == intendedNextPositionZ;
            double angleToIntendedNextPosition = MoreMath.AngleTo_AngleUnits(x, z, intendedNextPositionX, intendedNextPositionZ);
            IntendedNextPositionMapObject.Rotation =
                marioStationary ? (float)MoreMath.AngleUnitsToDegrees(marioAngle) : (float)MoreMath.AngleUnitsToDegrees(angleToIntendedNextPosition);
            IntendedNextPositionMapObject.Rotation = rot;

            // Update camera map object position
            CameraMapObject.X = cameraX;
            CameraMapObject.Y = cameraY;
            CameraMapObject.Z = cameraZ;
            CameraMapObject.Rotation = cameraRot;
        }

        public void Update()
        {
            // Make sure the control has successfully loaded
            if (!_isLoaded)
                return;

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

            // ---- Update PU -----
            int puX = PuUtilities.GetPuIndex(_marioMapObj.X);
            int puY = PuUtilities.GetPuIndex(_marioMapObj.Y);
            int puZ = PuUtilities.GetPuIndex(_marioMapObj.Z);

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

            var marioCoord = new PointF(_marioMapObj.RelX, _marioMapObj.RelZ);

            // Filter out all maps that are lower than Mario
            float marioY = _artificialMarioY ?? _marioMapObj.RelY;
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

            var holpCoord = new PointF(_holpMapObj.RelX, _holpMapObj.RelZ);
            _holpMapObj.Draw = _mapGui.MapShowHolp.Checked;
            _holpMapObj.LocationOnContol = CalculateLocationOnControl(holpCoord, mapView);

            var intendedNextPositionCoord = new PointF(_intendedNextPositionMapObj.RelX, _intendedNextPositionMapObj.RelZ);
            _intendedNextPositionMapObj.Draw = _mapGui.MapShowIntendedNextPosition.Checked;
            _intendedNextPositionMapObj.LocationOnContol = CalculateLocationOnControl(intendedNextPositionCoord, mapView);

            var cameraCoord = new PointF(_cameraMapObj.RelX, _cameraMapObj.RelZ);
            _cameraMapObj.Draw = _mapGui.MapShowCamera.Checked;
            _cameraMapObj.LocationOnContol = CalculateLocationOnControl(cameraCoord, mapView);

            _floorTriangleMapObj.P1OnControl = CalculateLocationOnControl(new PointF(_floorTriangleMapObj.RelX1, _floorTriangleMapObj.RelZ1), mapView);
            _floorTriangleMapObj.P2OnControl = CalculateLocationOnControl(new PointF(_floorTriangleMapObj.RelX2, _floorTriangleMapObj.RelZ2), mapView);
            _floorTriangleMapObj.P3OnControl = CalculateLocationOnControl(new PointF(_floorTriangleMapObj.RelX3, _floorTriangleMapObj.RelZ3), mapView);
            _floorTriangleMapObj.Draw = _floorTriangleMapObj.Show & _mapGui.MapShowFloorTriangle.Checked;

            _ceilingTriangleMapObj.P1OnControl = CalculateLocationOnControl(new PointF(_ceilingTriangleMapObj.RelX1, _ceilingTriangleMapObj.RelZ1), mapView);
            _ceilingTriangleMapObj.P2OnControl = CalculateLocationOnControl(new PointF(_ceilingTriangleMapObj.RelX2, _ceilingTriangleMapObj.RelZ2), mapView);
            _ceilingTriangleMapObj.P3OnControl = CalculateLocationOnControl(new PointF(_ceilingTriangleMapObj.RelX3, _ceilingTriangleMapObj.RelZ3), mapView);
            _ceilingTriangleMapObj.Draw = _ceilingTriangleMapObj.Show & _mapGui.MapShowCeilingTriangle.Checked;

            foreach (TriangleMap2Object cogFloorTri in _cogFloorTris)
            {
                cogFloorTri.P1OnControl = CalculateLocationOnControl(new PointF(cogFloorTri.RelX1, cogFloorTri.RelZ1), mapView);
                cogFloorTri.P2OnControl = CalculateLocationOnControl(new PointF(cogFloorTri.RelX2, cogFloorTri.RelZ2), mapView);
                cogFloorTri.P3OnControl = CalculateLocationOnControl(new PointF(cogFloorTri.RelX3, cogFloorTri.RelZ3), mapView);
                cogFloorTri.Draw = cogFloorTri.Show && TestingConfig.ShowCogTris;
            }

            foreach (TriangleMap2Object cogFloorTri in _cog2FloorTris)
            {
                cogFloorTri.P1OnControl = CalculateLocationOnControl(new PointF(cogFloorTri.RelX1, cogFloorTri.RelZ1), mapView);
                cogFloorTri.P2OnControl = CalculateLocationOnControl(new PointF(cogFloorTri.RelX2, cogFloorTri.RelZ2), mapView);
                cogFloorTri.P3OnControl = CalculateLocationOnControl(new PointF(cogFloorTri.RelX3, cogFloorTri.RelZ3), mapView);
                cogFloorTri.Draw = cogFloorTri.Show && TestingConfig.ShowCogTris;
            }

            foreach (TriangleMap2Object cogWallTri in _cogWallTris)
            {
                cogWallTri.P1OnControl = CalculateLocationOnControl(new PointF(cogWallTri.RelX1, cogWallTri.RelZ1), mapView);
                cogWallTri.P2OnControl = CalculateLocationOnControl(new PointF(cogWallTri.RelX2, cogWallTri.RelZ2), mapView);
                cogWallTri.P3OnControl = CalculateLocationOnControl(new PointF(cogWallTri.RelX3, cogWallTri.RelZ3), mapView);
                cogWallTri.Draw = cogWallTri.Show && TestingConfig.ShowCogTris;
            }

            foreach (List<TriangleMap2Object> tris in _triObjectFloors)
            {
                foreach (TriangleMap2Object tri in tris)
                {
                    tri.P1OnControl = CalculateLocationOnControl(new PointF(tri.RelX1, tri.RelZ1), mapView);
                    tri.P2OnControl = CalculateLocationOnControl(new PointF(tri.RelX2, tri.RelZ2), mapView);
                    tri.P3OnControl = CalculateLocationOnControl(new PointF(tri.RelX3, tri.RelZ3), mapView);
                    tri.Draw = tri.Show && TestingConfig.ShowShapes;
                }
            }

            foreach (List<TriangleMap2Object> tris in _triObjectWalls)
            {
                foreach (TriangleMap2Object tri in tris)
                {
                    tri.P1OnControl = CalculateLocationOnControl(new PointF(tri.RelX1, tri.RelZ1), mapView);
                    tri.P2OnControl = CalculateLocationOnControl(new PointF(tri.RelX2, tri.RelZ2), mapView);
                    tri.P3OnControl = CalculateLocationOnControl(new PointF(tri.RelX3, tri.RelZ3), mapView);
                    tri.Draw = tri.Show && TestingConfig.ShowShapes;
                }
            }

            // Calculate object slot's cooridnates
            foreach (var mapObj in _mapObjects)
            {
                mapObj.Draw = (mapObj.Show && (_mapGui.MapShowInactiveObjects.Checked || mapObj.IsActive));
                if (!mapObj.Draw)
                    continue;

                var objCoords = new PointF(mapObj.RelX, mapObj.RelZ);

                // Calculate object's location on control
                mapObj.LocationOnContol = CalculateLocationOnControl(objCoords, mapView);
            }

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

        public void AddMapObject(Map2Object mapObj)
        {
            _mapObjects.Add(mapObj);
            _mapGraphics.AddMapObject(mapObj);
        }

        public void RemoveMapObject(Map2Object mapObj)
        {
            _mapObjects.Remove(mapObj);
            _mapGraphics.RemoveMapObject(mapObj);
        }
    }
}
