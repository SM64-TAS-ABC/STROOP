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
using STROOP.Controls.Map.Graphics;
using STROOP.Controls.Map;
using STROOP.Controls.Map.Objects;
using STROOP.Controls.Map.Trackers;
using STROOP.Controls.Map.Semaphores;
using STROOP.Map3.Map;
using STROOP.Map3.Map.Graphics;
using STROOP.Map3.Map.Objects;
using STROOP.Map3.Map.Semaphores;
using STROOP.Map3.Map.Trackers;

namespace STROOP.Managers
{
    public class Map4Manager
    {
        public bool IsLoaded { get; private set; }
        public bool Visible { get => Config.Map4Graphics.Visible; set => Config.Map4Graphics.Visible = value; }

        #region Objects
        private Map4LevelObject _mapObjLevel;

        private Map4MarioObject _mapObjMario = new Map4MarioObject();
        private Map4HolpObject _mapObjHolp = new Map4HolpObject();
        private Map4CameraObject _mapObjCamera = new Map4CameraObject();

        private Map4FloorTriObject _mapObjFloorTri = new Map4FloorTriObject();
        private Map4WallTriObject _mapObjWallTri = new Map4WallTriObject();
        private Map4CeilingTriObject _mapObjCeilTri = new Map4CeilingTriObject();

        private List<Map4Sm64Object> _mapSm64Objs = new List<Map4Sm64Object>();
        private List<int> _currentMapSm64ObjIndexes;
        #endregion

        public Map4Manager()
        {
            _currentMapSm64ObjIndexes = new List<int>();
        }

        public void Load()
        {
            // Create new graphics control
            Config.Map4Graphics = new Map4Graphics(Config.Map3Gui.GLControl3D);
            Config.Map4Graphics.Load();
            Config.Map4Controller = new Map4Controller(Config.Map4Graphics);

            IsLoaded = true;

            Config.MapGui.ComboBoxMapColorMethod.DataSource = Enum.GetValues(typeof(MapLevelObject.ColorMethodType));

            Config.MapGui.TabControlView.SelectedIndexChanged += TabControlView_SelectedIndexChanged;
            Config.MapGui.CheckBoxMapGameCamOrientation.CheckedChanged += (sender, e) =>
            {
                if (Config.MapGui.CheckBoxMapGameCamOrientation.Checked)
                    Config.Map4Controller.CameraMode = Map4Controller.MapCameraMode.Game;
                else
                    Config.Map4Controller.CameraMode = Map4Controller.MapCameraMode.Fly;
            };

            Config.MapGui.RadioButtonScaleCourseDefault.Click += (sender, e) => Config.Map4Controller.ScaleMode = Map4Controller.MapScaleMode.CourseDefault;
            Config.MapGui.RadioButtonScaleMaxCourseSize.Click += (sender, e) => Config.Map4Controller.ScaleMode = Map4Controller.MapScaleMode.MaxCourseSize;
            Config.MapGui.RadioButtonScaleCustom.Click += (sender, e) => Config.Map4Controller.ScaleMode = Map4Controller.MapScaleMode.Custom;

            Config.MapGui.RadioButtonCenterBestFit.Click += (sender, e) => Config.Map4Controller.CenterMode = Map4Controller.MapCenterMode.BestFit;
            Config.MapGui.RadioButtonCenterOrigin.Click += (sender, e) => Config.Map4Controller.CenterMode = Map4Controller.MapCenterMode.Origin;
            Config.MapGui.RadioButtonCenterCustom.Click += (sender, e) => Config.Map4Controller.CenterMode = Map4Controller.MapCenterMode.Custom;

            Config.MapGui.RadioButtonAngle0.Click += (sender, e) => Config.Map4Controller.MapAngle = 0;
            Config.MapGui.RadioButtonAngle16384.Click += (sender, e) => Config.Map4Controller.MapAngle = (float)Math.PI / 2;
            Config.MapGui.RadioButtonAngle32768.Click += (sender, e) => Config.Map4Controller.MapAngle = (float)Math.PI;
            Config.MapGui.RadioButtonAngle49152.Click += (sender, e) => Config.Map4Controller.MapAngle = (float)(3 * Math.PI / 2);

            /*
            _mapGui.ButtonAddNewTracker.Click += (sender, e) =>
                _mapGui.MapTrackerFlowLayoutPanel.AddNewControl(
                    new MapTracker(_mapGui.MapTrackerFlowLayoutPanel, new List<MapIconObject>() { _mapObjMario }));
                    */
            Config.MapGui.ButtonClearAllTrackers.Click += (sender, e) => Config.MapGui.MapTrackerFlowLayoutPanel.ClearControls();

            // Test
            _mapObjLevel = new Map4LevelObject();
            Config.Map4Controller.AddMapObject(_mapObjLevel);
            Config.Map4Controller.AddMapObject(_mapObjMario);
            //Config.Map4Controller.AddMapObject(_mapObjHolp);
            //Config.Map4Controller.AddMapObject(_mapObjCamera);

            //_mapSm64Objs = Enumerable.Range(0, ObjectSlotsConfig.MaxSlots).ToList()
            //    .ConvertAll(i => new Map4Sm64Object(i));
            //_mapSm64Objs.ForEach(obj => Config.Map4Controller.AddMapObject(obj));

            Config.Map4Controller.AddMapObject(_mapObjFloorTri);
            //Config.Map4Controller.AddMapObject(_mapObjWallTri);
            //Config.Map4Controller.AddMapObject(_mapObjCeilTri);
            /*
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackMario, MapSemaphoreManager.Mario, _mapObjMario, true);
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackHolp, MapSemaphoreManager.Holp, _mapObjHolp, false);
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackCamera, MapSemaphoreManager.Camera, _mapObjCamera, false);

            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackFloorTriangle, MapSemaphoreManager.FloorTri, _mapObjFloorTri, false);
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackWallTriangle, MapSemaphoreManager.WallTri, _mapObjWallTri, false);
            InitializeCheckboxSemaphore(Config.MapGui.CheckBoxTrackCeilingTriangle, MapSemaphoreManager.CeilingTri, _mapObjCeilTri, false);
            */
            List<MapLayout> mapLayouts = Config.MapAssociations.GetAllMaps();
            List<object> mapLayoutChoices = new List<object>() { "Recommended" };
            mapLayouts.ForEach(mapLayout => mapLayoutChoices.Add(mapLayout));
            Config.MapGui.ComboBoxLevel.DataSource = mapLayoutChoices;

            List<BackgroundImage> backgroundImages = Config.MapAssociations.GetAllBackgroundImages();
            List<object> backgroundImageChoices = new List<object>() { "Recommended" };
            backgroundImages.ForEach(backgroundImage => backgroundImageChoices.Add(backgroundImage));
            Config.MapGui.ComboBoxBackground.DataSource = backgroundImageChoices;

            _mapObjMario.Shown = true;
            _mapObjMario.Displayed = true;
            _mapObjMario.Tracked = true;

            //_mapObjFloorTri.Shown = true;
            //_mapObjFloorTri.Displayed = true;
            //_mapObjFloorTri.Tracked = true;
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
            else if (Config.MapGui.TabControlView.SelectedTab == Config.MapGui.TabPage3D)
            {
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
                //Map4Sm64Object sm64Obj = _mapSm64Objs[index];
                //Map4Semaphore semaphore = Map4SemaphoreManager.Objects[index];
                //semaphore.IsUsed = true;
                //Map4Tracker tracker = new Map4Tracker(
                //    Config.MapGui.MapTrackerFlowLayoutPanel, new List<Map4Object>() { sm64Obj }, new List<Map4Semaphore>() { semaphore });
                //Config.MapGui.MapTrackerFlowLayoutPanel.AddNewControl(tracker);
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
                _mapObjLevel.ColorMethod = (Map4LevelObject.ColorMethodType)Config.MapGui.ComboBoxMapColorMethod.SelectedItem;

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            Config.Map4Controller.Update();

            // Update labels
            /*_mapGui.PuValueLabel.Text = string.Format("[{0}:{1}:{2}]", puX, puY, puZ);
            _mapGui.QpuValueLabel.Text = string.Format("[{0}:{1}:{2}]", qpuX, qpuY, qpuZ);
            _mapGui.MapIdLabel.Text = string.Format("[{0}:{1}:{2}:{3}]", level, area, loadingPoint, missionLayout);
            _mapGui.MapNameLabel.Text = _currentMap.Name;
            _mapGui.MapSubNameLabel.Text = (_currentMap.SubName != null) ? _currentMap.SubName : "";*/
        }
    }
}
