﻿using System;
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
using STROOP.Controls.Map.Graphics;
using STROOP.Controls.Map;
using STROOP.Controls.Map.Objects;
using STROOP.Controls.Map.Trackers;
using STROOP.Controls.Map.Semaphores;

namespace STROOP.Managers
{
    public class MapManager
    {        
        public bool IsLoaded { get; private set; }
        public bool Visible { get => _graphics.Visible; set => _graphics.Visible = value; }

        private MapGraphics _graphics;

        #region Objects
        private MapLevelObject _mapObjLevel;

        private MapMarioObject _mapObjMario = new MapMarioObject();
        private MapHolpObject _mapObjHolp = new MapHolpObject();
        private MapCameraObject _mapObjCamera = new MapCameraObject();

        private MapFloorTriObject _mapObjFloorTri = new MapFloorTriObject();
        private MapWallTriObject _mapObjWallTri = new MapWallTriObject();
        private MapCeilingTriObject _mapObjCeilTri = new MapCeilingTriObject();

        private List<MapSm64Object> _mapSm64Objs = new List<MapSm64Object>();
        private List<int> _currentMapSm64ObjIndexes;
        #endregion

        public MapManager()
        {
            _currentMapSm64ObjIndexes = new List<int>();
        }

        public void Load()
        {
            // Create new graphics control
            _graphics = new MapGraphics(Config.MapGui.GLControl);
            _graphics.Load();
            Config.MapController = new MapController(_graphics);

            IsLoaded = true;

            Config.MapGui.ComboBoxMapColorMethod.DataSource = Enum.GetValues(typeof(MapLevelObject.ColorMethodType));

            Config.MapGui.TabControlView.SelectedIndexChanged += TabControlView_SelectedIndexChanged;
            Config.MapGui.CheckBoxMapGameCamOrientation.CheckedChanged += (sender, e) =>
            {
                if (Config.MapGui.CheckBoxMapGameCamOrientation.Checked)
                    Config.MapController.CameraMode = MapController.MapCameraMode.Game;
                else
                    Config.MapController.CameraMode = MapController.MapCameraMode.Fly;
            };

            Config.MapGui.RadioButtonScaleCourseDefault.Click += (sender, e) => Config.MapController.ScaleMode = MapController.MapScaleMode.CourseDefault;
            Config.MapGui.RadioButtonScaleMaxCourseSize.Click += (sender, e) => Config.MapController.ScaleMode = MapController.MapScaleMode.MaxCourseSize;
            Config.MapGui.RadioButtonScaleCustom.Click += (sender, e) => Config.MapController.ScaleMode = MapController.MapScaleMode.Custom;

            Config.MapGui.RadioButtonCenterBestFit.Click += (sender, e) => Config.MapController.CenterMode = MapController.MapCenterMode.BestFit;
            Config.MapGui.RadioButtonCenterOrigin.Click += (sender, e) => Config.MapController.CenterMode = MapController.MapCenterMode.Origin;
            Config.MapGui.RadioButtonCenterCustom.Click += (sender, e) => Config.MapController.CenterMode = MapController.MapCenterMode.Custom;

            Config.MapGui.RadioButtonAngle0.Click += (sender, e) => Config.MapController.MapAngle = 0;
            Config.MapGui.RadioButtonAngle16384.Click += (sender, e) => Config.MapController.MapAngle = (float) Math.PI / 2;
            Config.MapGui.RadioButtonAngle32768.Click += (sender, e) => Config.MapController.MapAngle = (float) Math.PI;
            Config.MapGui.RadioButtonAngle49152.Click += (sender, e) => Config.MapController.MapAngle = (float) (3 * Math.PI / 2);

            /*
            _mapGui.ButtonAddNewTracker.Click += (sender, e) =>
                _mapGui.MapTrackerFlowLayoutPanel.AddNewControl(
                    new MapTracker(_mapGui.MapTrackerFlowLayoutPanel, new List<MapIconObject>() { _mapObjMario }));
                    */
            Config.MapGui.ButtonClearAllTrackers.Click += (sender, e) => Config.MapGui.MapTrackerFlowLayoutPanel.ClearControls();

            // Test
            _mapObjLevel = new MapLevelObject();
            Config.MapController.AddMapObject(_mapObjLevel);
            Config.MapController.AddMapObject(_mapObjMario);
            Config.MapController.AddMapObject(_mapObjHolp);
            Config.MapController.AddMapObject(_mapObjCamera);

            _mapSm64Objs = Enumerable.Range(0, ObjectSlotsConfig.MaxSlots).ToList()
                .ConvertAll(i => new MapSm64Object(i));
            _mapSm64Objs.ForEach(obj => Config.MapController.AddMapObject(obj));

            Config.MapController.AddMapObject(_mapObjFloorTri);
            Config.MapController.AddMapObject(_mapObjWallTri);
            Config.MapController.AddMapObject(_mapObjCeilTri);

            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackMario, MapSemaphoreManager.Mario, _mapObjMario, true);
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackHolp, MapSemaphoreManager.Holp, _mapObjHolp, false);
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackCamera, MapSemaphoreManager.Camera, _mapObjCamera, false);

            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackFloorTriangle, MapSemaphoreManager.FloorTri, _mapObjFloorTri, false);
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackWallTriangle, MapSemaphoreManager.WallTri, _mapObjWallTri, false);
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackCeilingTriangle, MapSemaphoreManager.CeilingTri, _mapObjCeilTri, false);

            List<MapLayout> mapLayouts = Config.MapAssociations.GetAllMaps();
            List<object> mapLayoutChoices = new List<object>() { "Recommended" };
            mapLayouts.ForEach(mapLayout => mapLayoutChoices.Add(mapLayout));
            Config.MapGui.ComboBoxLevel.DataSource = mapLayoutChoices;

            List<BackgroundImage> backgroundImages = Config.MapAssociations.GetAllBackgroundImages();
            List<object> backgroundImageChoices = new List<object>() { "Recommended" };
            backgroundImages.ForEach(backgroundImage => backgroundImageChoices.Add(backgroundImage));
            Config.MapGui.ComboBoxBackground.DataSource = backgroundImageChoices;
        }

        private void InitializeCheckboxSemaphore(
            CheckBox checkBox, MapSemaphore semaphore, MapObject mapObj, bool startAsOn)
        {
            Action clickAction = () =>
            {
                semaphore.Toggle();
                if (semaphore.IsUsed)
                {
                    MapTracker tracker = new MapTracker(
                        Config.MapGui.MapTrackerFlowLayoutPanel,
                        new List<MapObject>() { mapObj },
                        new List<MapSemaphore>() { semaphore });
                    Config.MapGui.MapTrackerFlowLayoutPanel.AddNewControl(tracker);
                }
            };
            checkBox.Click += (sender, e) => clickAction();
            if (startAsOn)
            {
                checkBox.Checked = true;
                clickAction();
            }
        }

        private void TabControlView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Config.MapGui.TabControlView.SelectedTab == Config.MapGui.TabPage2D)
            {
                Config.MapController.CameraMode = MapController.MapCameraMode.TopDown;
            }
            else if (Config.MapGui.TabControlView.SelectedTab == Config.MapGui.TabPage3D) {
                if (Config.MapGui.CheckBoxMapGameCamOrientation.Checked)
                    Config.MapController.CameraMode = MapController.MapCameraMode.Game;
                else
                    Config.MapController.CameraMode = MapController.MapCameraMode.Fly;
            }
        }

        private void UpdateBasedOnObjectsSelectedOnMap()
        {
            // Determine which obj slots have been checked/unchecked since the last update
            List<int> currentSm64ObjIndexes = Config.ObjectSlotsManager.SelectedOnMapSlotsAddresses
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address))
                .FindAll(address => address.HasValue)
                .ConvertAll(address => address.Value);
            List<int> toBeRemovedIndexes = _currentMapSm64ObjIndexes.FindAll(i => !currentSm64ObjIndexes.Contains(i));
            List<int> toBeAddedIndexes = currentSm64ObjIndexes.FindAll(i => !_currentMapSm64ObjIndexes.Contains(i));
            _currentMapSm64ObjIndexes = currentSm64ObjIndexes;

            // Newly unchecked slots have their semaphore turned off
            foreach (int index in toBeRemovedIndexes)
            {
                MapSemaphore semaphore = MapSemaphoreManager.Objects[index];
                semaphore.IsUsed = false;
            }

            // Newly checked slots have their semaphore turned on and a tracker is created
            foreach (int index in toBeAddedIndexes)
            {
                MapSm64Object sm64Obj = _mapSm64Objs[index];
                MapSemaphore semaphore = MapSemaphoreManager.Objects[index];
                semaphore.IsUsed = true;
                MapTracker tracker = new MapTracker(
                    Config.MapGui.MapTrackerFlowLayoutPanel, new List<MapObject>() { sm64Obj }, new List<MapSemaphore>() { semaphore });
                Config.MapGui.MapTrackerFlowLayoutPanel.AddNewControl(tracker);
            }
        }

        private void UpdateControlsBasedOnSemaphores()
        {
            // Update checkboxes/object slots based on the current semaphore states
            // This keeps these controls consistent when the user manually exits the tracker
            Config.MapGui.CheckBoxTrackMario.Checked = MapSemaphoreManager.Mario.IsUsed;
            Config.MapGui.CheckBoxTrackHolp.Checked = MapSemaphoreManager.Holp.IsUsed;
            Config.MapGui.CheckBoxTrackCamera.Checked = MapSemaphoreManager.Camera.IsUsed;

            List<uint> toBeUnselected = Config.ObjectSlotsManager.SelectedOnMapSlotsAddresses
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address))
                .FindAll(index => index.HasValue)
                .ConvertAll(index => index.Value)
                .FindAll(index => !MapSemaphoreManager.Objects[index].IsUsed)
                .ConvertAll(index => ObjectUtilities.GetObjectAddress(index));
            toBeUnselected.ForEach(address => Config.ObjectSlotsManager.SelectedOnMapSlotsAddresses.Remove(address));

            Config.MapGui.CheckBoxTrackFloorTriangle.Checked = MapSemaphoreManager.FloorTri.IsUsed;
            Config.MapGui.CheckBoxTrackWallTriangle.Checked = MapSemaphoreManager.WallTri.IsUsed;
            Config.MapGui.CheckBoxTrackCeilingTriangle.Checked = MapSemaphoreManager.CeilingTri.IsUsed;
        }

        public void Update()
        {

            UpdateBasedOnObjectsSelectedOnMap();
            UpdateControlsBasedOnSemaphores();

            /*
            if (!currentSm64ObjIndexes.SequenceEqual(_currentMapSm64ObjIndexes))
            {
                _currentMapSm64ObjIndexes = currentSm64ObjIndexes;
                _currentMapSm64Objects.ForEach(obj => Config.MapController.RemoveMapObject(obj));
                _currentMapSm64Objects = _currentMapSm64ObjIndexes.ConvertAll(i => new MapSm64Object(i));
                _currentMapSm64Objects.ForEach(obj => Config.MapController.AddMapObject(obj));
            }
            */

            //_mapSm64Objs = Enumerable.Range(0, ObjectSlotsConfig.MaxSlots).Select(i => new MapSm64Object(i)).ToList();
            //_mapSm64Objs.ForEach(o => _controller.AddMapObject(o));

            Config.MapGui.MapTrackerFlowLayoutPanel.UpdateControls();

            // Make sure the control has successfully loaded
            if (!IsLoaded)
                return;

            if (Config.MapGui.ComboBoxMapColorMethod.SelectedItem != null)
                _mapObjLevel.ColorMethod = (MapLevelObject.ColorMethodType)Config.MapGui.ComboBoxMapColorMethod.SelectedItem;

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            Config.MapController.Update();

            // Update labels
            /*_mapGui.PuValueLabel.Text = string.Format("[{0}:{1}:{2}]", puX, puY, puZ);
            _mapGui.QpuValueLabel.Text = string.Format("[{0}:{1}:{2}]", qpuX, qpuY, qpuZ);
            _mapGui.MapIdLabel.Text = string.Format("[{0}:{1}:{2}:{3}]", level, area, loadingPoint, missionLayout);
            _mapGui.MapNameLabel.Text = _currentMap.Name;
            _mapGui.MapSubNameLabel.Text = (_currentMap.SubName != null) ? _currentMap.SubName : "";*/
        }
    }
}
