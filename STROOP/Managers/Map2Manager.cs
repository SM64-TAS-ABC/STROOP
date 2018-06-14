using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
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
        public MapAssociations MapAssoc;
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
        List<Map2Object> _mapObjects = new List<Map2Object>();
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

        public Map2Manager(MapAssociations mapAssoc, Map2Gui mapGui)
        {
            MapAssoc = mapAssoc;
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
        }

        public void Load()
        {
            // Create new graphics control
            _mapGraphics = new Map2Graphics(_mapGui.GLControl);
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

            _mapGui.MapArtificialMarioYLabelTextBox.AddEnterAction(() =>
                _artificialMarioY = ParsingUtilities.ParseFloatNullable(
                    _mapGui.MapArtificialMarioYLabelTextBox.Text));
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

            // Adjust mario coordinates relative from current PU
            float marioRelX = (float)PuUtilities.GetCoordinateInPu(_marioMapObj.X, puX);
            float marioRelY = _artificialMarioY ?? (float)PuUtilities.GetCoordinateInPu(_marioMapObj.Y, puY);
            float marioRelZ = (float)PuUtilities.GetCoordinateInPu(_marioMapObj.Z, puZ);
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

            int holpPuX = PuUtilities.GetPuIndex(_holpMapObj.X);
            int holpPuZ = PuUtilities.GetPuIndex(_holpMapObj.Z);
            float holpRelX = (float)PuUtilities.GetCoordinateInPu(_holpMapObj.X, holpPuX);
            float holpRelZ = (float)PuUtilities.GetCoordinateInPu(_holpMapObj.Z, holpPuZ);
            var holpCoord = new PointF(holpRelX, holpRelZ);
            _holpMapObj.Draw = _mapGui.MapShowHolp.Checked;
            _holpMapObj.LocationOnContol = CalculateLocationOnControl(holpCoord, mapView);

            int intendedNextPositionPuX = PuUtilities.GetPuIndex(_intendedNextPositionMapObj.X);
            int intendedNextPositionPuZ = PuUtilities.GetPuIndex(_intendedNextPositionMapObj.Z);
            float intendedNextPositionRelX = (float)PuUtilities.GetCoordinateInPu(_intendedNextPositionMapObj.X, intendedNextPositionPuX);
            float intendedNextPositionRelZ = (float)PuUtilities.GetCoordinateInPu(_intendedNextPositionMapObj.Z, intendedNextPositionPuZ);
            var intendedNextPositionCoord = new PointF(intendedNextPositionRelX, intendedNextPositionRelZ);
            _intendedNextPositionMapObj.Draw = _mapGui.MapShowIntendedNextPosition.Checked;
            _intendedNextPositionMapObj.LocationOnContol = CalculateLocationOnControl(intendedNextPositionCoord, mapView);

            int cameraPuX = PuUtilities.GetPuIndex(_cameraMapObj.X);
            int cameraPuY = PuUtilities.GetPuIndex(_cameraMapObj.Y);
            int cameraPuZ = PuUtilities.GetPuIndex(_cameraMapObj.Z);
            float cameraRelX = (float)PuUtilities.GetCoordinateInPu(_cameraMapObj.X, cameraPuX);
            float cameraRelZ = (float)PuUtilities.GetCoordinateInPu(_cameraMapObj.Z, cameraPuZ);
            var cameraCoord = new PointF(cameraRelX, cameraRelZ);
            _cameraMapObj.Draw = _mapGui.MapShowCamera.Checked;
            _cameraMapObj.LocationOnContol = CalculateLocationOnControl(cameraCoord, mapView);

            float floorTrianglePuX1 = (float)PuUtilities.GetRelativeCoordinate(_floorTriangleMapObj.X1);
            float floorTrianglePuZ1 = (float)PuUtilities.GetRelativeCoordinate(_floorTriangleMapObj.Z1);
            float floorTrianglePuX2 = (float)PuUtilities.GetRelativeCoordinate(_floorTriangleMapObj.X2);
            float floorTrianglePuZ2 = (float)PuUtilities.GetRelativeCoordinate(_floorTriangleMapObj.Z2);
            float floorTrianglePuX3 = (float)PuUtilities.GetRelativeCoordinate(_floorTriangleMapObj.X3);
            float floorTrianglePuZ3 = (float)PuUtilities.GetRelativeCoordinate(_floorTriangleMapObj.Z3);
            _floorTriangleMapObj.P1OnControl = CalculateLocationOnControl(new PointF(floorTrianglePuX1, floorTrianglePuZ1), mapView);
            _floorTriangleMapObj.P2OnControl = CalculateLocationOnControl(new PointF(floorTrianglePuX2, floorTrianglePuZ2), mapView);
            _floorTriangleMapObj.P3OnControl = CalculateLocationOnControl(new PointF(floorTrianglePuX3, floorTrianglePuZ3), mapView);
            _floorTriangleMapObj.Draw = _floorTriangleMapObj.Show & _mapGui.MapShowFloorTriangle.Checked;

            float ceilingTrianglePuX1 = (float)PuUtilities.GetRelativeCoordinate(_ceilingTriangleMapObj.X1);
            float ceilingTrianglePuZ1 = (float)PuUtilities.GetRelativeCoordinate(_ceilingTriangleMapObj.Z1);
            float ceilingTrianglePuX2 = (float)PuUtilities.GetRelativeCoordinate(_ceilingTriangleMapObj.X2);
            float ceilingTrianglePuZ2 = (float)PuUtilities.GetRelativeCoordinate(_ceilingTriangleMapObj.Z2);
            float ceilingTrianglePuX3 = (float)PuUtilities.GetRelativeCoordinate(_ceilingTriangleMapObj.X3);
            float ceilingTrianglePuZ3 = (float)PuUtilities.GetRelativeCoordinate(_ceilingTriangleMapObj.Z3);
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
                var objPuX = PuUtilities.GetPuIndex(mapObj.X);
                var objPuZ = PuUtilities.GetPuIndex(mapObj.Z);
                float objPosX = (float)PuUtilities.GetCoordinateInPu(mapObj.X, objPuX);
                float objPosZ = (float)PuUtilities.GetCoordinateInPu(mapObj.Z, objPuZ);
                var objCoords = new PointF(objPosX, objPosZ);

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
