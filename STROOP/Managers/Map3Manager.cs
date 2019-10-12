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
        private List<int> _currentObjIndexes = new List<int>();

        private bool _isLoaded = false;

        public Map3Manager()
        {
        }

        public void Load()
        {
            // Create new graphics control
            Config.Map3Graphics = new Map3Graphics();
            Config.Map3Graphics.Load();
            _isLoaded = true;

            InitializeControls();
            InitializeSemaphores();
        }

        private void InitializeControls()
        {
            // FlowLayoutPanel
            Config.Map3Gui.flowLayoutPanelMap3Trackers.Initialize(new Map3MapObject(), new Map3BackgroundObject());

            // ComboBox for Level
            List<MapLayout> mapLayouts = Config.MapAssociations.GetAllMaps();
            List<object> mapLayoutChoices = new List<object>() { "Recommended" };
            mapLayouts.ForEach(mapLayout => mapLayoutChoices.Add(mapLayout));
            Config.Map3Gui.comboBoxMap3OptionsLevel.DataSource = mapLayoutChoices;

            // ComboBox for Background
            List<BackgroundImage> backgroundImages = Config.MapAssociations.GetAllBackgroundImages();
            List<object> backgroundImageChoices = new List<object>() { "Recommended" };
            backgroundImages.ForEach(backgroundImage => backgroundImageChoices.Add(backgroundImage));
            Config.Map3Gui.comboBoxMap3OptionsBackground.DataSource = backgroundImageChoices;

            // Buttons on Options
            ControlUtilities.AddContextMenuStripFunctions(
                Config.Map3Gui.buttonMap3OptionsAddNewTracker,
                new List<string>()
                {
                    "Add Tracker for Custom Points",
                    "Add Tracker for Custom Floor Tris",
                    "Add Tracker for Custom Wall Tris",
                    "Add Tracker for Custom Ceiling Tris",
                    "Add Tracker for Level Floor Tris",
                    "Add Tracker for Level Wall Tris",
                    "Add Tracker for Level Ceiling Tris",
                },
                new List<Action>()
                {
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter points as pairs of integers.");
                        Map3Object mapObj = Map3CustomPointsObject.Create(text);
                        if (mapObj == null) return;
                        Map3Tracker tracker = new Map3Tracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        Map3Object mapObj = Map3CustomFloorObject.Create(text);
                        if (mapObj == null) return;
                        Map3Tracker tracker = new Map3Tracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        Map3Object mapObj = Map3CustomWallObject.Create(text);
                        if (mapObj == null) return;
                        Map3Tracker tracker = new Map3Tracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        Map3Object mapObj = Map3CustomCeilingObject.Create(text);
                        if (mapObj == null) return;
                        Map3Tracker tracker = new Map3Tracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        Map3Object mapObj = new Map3LevelFloorObject();
                        Map3Tracker tracker = new Map3Tracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        Map3Object mapObj = new Map3LevelWallObject();
                        Map3Tracker tracker = new Map3Tracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        Map3Object mapObj = new Map3LevelCeilingObject();
                        Map3Tracker tracker = new Map3Tracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                });
            Config.Map3Gui.buttonMap3OptionsAddNewTracker.Click += (sender, e) =>
                Config.Map3Gui.buttonMap3OptionsAddNewTracker.ContextMenuStrip.Show(Cursor.Position);
            Config.Map3Gui.buttonMap3OptionsClearAllTrackers.Click += (sender, e) =>
                Config.Map3Gui.flowLayoutPanelMap3Trackers.ClearControls();
            Config.Map3Gui.buttonMap3OptionsTrackAllObjects.Click += (sender, e) =>
                TrackMultipleObjects(ObjectUtilities.GetAllObjectAddresses());
            Config.Map3Gui.buttonMap3OptionsTrackMarkedObjects.Click += (sender, e) =>
                TrackMultipleObjects(Config.ObjectSlotsManager.MarkedSlotsAddresses);

            // Buttons for Changing Scale
            Config.Map3Gui.buttonMap3ControllersScaleMinus.Click += (sender, e) =>
                Config.Map3Graphics.ChangeScale(-1, Config.Map3Gui.textBoxMap3ControllersScaleChange.Text);
            Config.Map3Gui.buttonMap3ControllersScalePlus.Click += (sender, e) =>
                Config.Map3Graphics.ChangeScale(1, Config.Map3Gui.textBoxMap3ControllersScaleChange.Text);
            Config.Map3Gui.buttonMap3ControllersScaleDivide.Click += (sender, e) =>
                Config.Map3Graphics.ChangeScale2(-1, Config.Map3Gui.textBoxMap3ControllersScaleChange2.Text);
            Config.Map3Gui.buttonMap3ControllersScaleTimes.Click += (sender, e) =>
                Config.Map3Graphics.ChangeScale2(1, Config.Map3Gui.textBoxMap3ControllersScaleChange2.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.Map3Gui.groupBoxMap3ControllersScale,
                new List<string>()
                {
                    "Very Small Unit Squares",
                    "Small Unit Squares",
                    "Medium Unit Squares",
                    "Big Unit Squares",
                    "Very Big Unit Squares",
                },
                new List<Action>()
                {
                    () => Config.Map3Graphics.SetCustomScale(6),
                    () => Config.Map3Graphics.SetCustomScale(12),
                    () => Config.Map3Graphics.SetCustomScale(18),
                    () => Config.Map3Graphics.SetCustomScale(24),
                    () => Config.Map3Graphics.SetCustomScale(40),
                });

            // Buttons for Changing Center
            Config.Map3Gui.buttonMap3ControllersCenterUp.Click += (sender, e) =>
                Config.Map3Graphics.ChangeCenter(0, -1, Config.Map3Gui.textBoxMap3ControllersCenterChange.Text);
            Config.Map3Gui.buttonMap3ControllersCenterDown.Click += (sender, e) =>
                Config.Map3Graphics.ChangeCenter(0, 1, Config.Map3Gui.textBoxMap3ControllersCenterChange.Text);
            Config.Map3Gui.buttonMap3ControllersCenterLeft.Click += (sender, e) =>
                Config.Map3Graphics.ChangeCenter(-1, 0, Config.Map3Gui.textBoxMap3ControllersCenterChange.Text);
            Config.Map3Gui.buttonMap3ControllersCenterRight.Click += (sender, e) =>
                Config.Map3Graphics.ChangeCenter(1, 0, Config.Map3Gui.textBoxMap3ControllersCenterChange.Text);
            Config.Map3Gui.buttonMap3ControllersCenterUpLeft.Click += (sender, e) =>
                Config.Map3Graphics.ChangeCenter(-1, -1, Config.Map3Gui.textBoxMap3ControllersCenterChange.Text);
            Config.Map3Gui.buttonMap3ControllersCenterUpRight.Click += (sender, e) =>
                Config.Map3Graphics.ChangeCenter(1, -1, Config.Map3Gui.textBoxMap3ControllersCenterChange.Text);
            Config.Map3Gui.buttonMap3ControllersCenterDownLeft.Click += (sender, e) =>
                Config.Map3Graphics.ChangeCenter(-1, 1, Config.Map3Gui.textBoxMap3ControllersCenterChange.Text);
            Config.Map3Gui.buttonMap3ControllersCenterDownRight.Click += (sender, e) =>
                Config.Map3Graphics.ChangeCenter(1, 1, Config.Map3Gui.textBoxMap3ControllersCenterChange.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.Map3Gui.groupBoxMap3ControllersCenter,
                new List<string>() { "Center on Mario" },
                new List<Action>()
                {
                    () =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        Config.Map3Graphics.SetCustomCenter(marioX + "," + marioZ);
                    }
                });

            // Buttons for Changing Angle
            Config.Map3Gui.buttonMap3ControllersAngleCCW.Click += (sender, e) =>
                Config.Map3Graphics.ChangeAngle(-1, Config.Map3Gui.textBoxMap3ControllersAngleChange.Text);
            Config.Map3Gui.buttonMap3ControllersAngleCW.Click += (sender, e) =>
                Config.Map3Graphics.ChangeAngle(1, Config.Map3Gui.textBoxMap3ControllersAngleChange.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.Map3Gui.groupBoxMap3ControllersAngle,
                new List<string>()
                {
                    "Use Mario Angle",
                    "Use Camera Angle",
                    "Use Centripetal Angle",
                },
                new List<Action>()
                {
                    () =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        Config.Map3Graphics.SetCustomAngle(marioAngle);
                    },
                    () =>
                    {
                        ushort cameraAngle = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                        Config.Map3Graphics.SetCustomAngle(cameraAngle);
                    },
                    () =>
                    {
                        ushort centripetalAngle = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset);
                        double centripetalAngleReversed = MoreMath.ReverseAngle(centripetalAngle);
                        Config.Map3Graphics.SetCustomAngle(centripetalAngleReversed);
                    },
                });

            // TextBoxes for Custom Values
            Config.Map3Gui.textBoxMap3ControllersScaleCustom.AddEnterAction(() =>
            {
                Config.Map3Gui.radioButtonMap3ControllersScaleCustom.Checked = true;
            });
            Config.Map3Gui.textBoxMap3ControllersCenterCustom.AddEnterAction(() =>
            {
                Config.Map3Gui.radioButtonMap3ControllersCenterCustom.Checked = true;
            });
            Config.Map3Gui.textBoxMap3ControllersAngleCustom.AddEnterAction(() =>
            {
                Config.Map3Gui.radioButtonMap3ControllersAngleCustom.Checked = true;
            });

            // Additional Checkboxes
            Config.Map3Gui.checkBoxMap3OptionsEnablePuView.Click += (sender, e) =>
                Config.Map3Graphics.MapViewEnablePuView = Config.Map3Gui.checkBoxMap3OptionsEnablePuView.Checked;
            Config.Map3Gui.checkBoxMap3OptionsScaleIconSizes.Click += (sender, e) =>
                Config.Map3Graphics.MapViewScaleIconSizes = Config.Map3Gui.checkBoxMap3OptionsScaleIconSizes.Checked;

            // Global Icon Size
            Config.Map3Gui.textBoxMap3OptionsGlobalIconSize.AddEnterAction(() =>
            {
                float? parsed = ParsingUtilities.ParseFloatNullable(
                    Config.Map3Gui.textBoxMap3OptionsGlobalIconSize.Text);
                if (!parsed.HasValue) return;
                SetGlobalIconSize(parsed.Value);
            });
            Config.Map3Gui.trackBarMap3OptionsGlobalIconSize.AddManualChangeAction(() =>
                SetGlobalIconSize(Config.Map3Gui.trackBarMap3OptionsGlobalIconSize.Value));
        }

        private void SetGlobalIconSize(float size)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.SetGlobalIconSize(size);
            Config.Map3Gui.textBoxMap3OptionsGlobalIconSize.SubmitText(size.ToString());
            Config.Map3Gui.trackBarMap3OptionsGlobalIconSize.StartChangingByCode();
            ControlUtilities.SetTrackBarValueCapped(Config.Map3Gui.trackBarMap3OptionsGlobalIconSize, size);
            Config.Map3Gui.trackBarMap3OptionsGlobalIconSize.StopChangingByCode();
        }

        private void InitializeSemaphores()
        {
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackMario, Map3SemaphoreManager.Mario, () => new Map3MarioObject(), true);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackHolp, Map3SemaphoreManager.Holp, () => new Map3HolpObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCamera, Map3SemaphoreManager.Camera, () => new Map3CameraObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackFloorTri, Map3SemaphoreManager.FloorTri, () => new Map3MarioFloorObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCeilingTri, Map3SemaphoreManager.CeilingTri, () => new Map3MarioCeilingObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCellGridlines, Map3SemaphoreManager.CellGridlines, () => new Map3CellGridlinesObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCurrentCell, Map3SemaphoreManager.CurrentCell, () => new Map3CurrentCellObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackUnitGridlines, Map3SemaphoreManager.UnitGridlines, () => new Map3UnitGridlinesObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCurrentUnit, Map3SemaphoreManager.CurrentUnit, () => new Map3CurrentUnitObject(PositionAngle.Mario), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackNextPositions, Map3SemaphoreManager.NextPositions, () => new Map3NextPositionsObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackSelf, Map3SemaphoreManager.Self, () => new Map3SelfObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackPoint, Map3SemaphoreManager.Point, () => new Map3PointObject(), false);
        }

        private void InitializeCheckboxSemaphore(
            CheckBox checkBox, Map3Semaphore semaphore, Func<Map3Object> mapObjFunc, bool startAsOn)
        {
            Action<bool> addTrackerAction = (bool withSemaphore) =>
            {
                Map3Tracker tracker = new Map3Tracker(
                    new List<Map3Object>() { mapObjFunc() },
                    withSemaphore ? new List<Map3Semaphore>() { semaphore } : new List<Map3Semaphore>());
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };
            Action clickAction = () =>
            {
                semaphore.Toggle();
                if (semaphore.IsUsed)
                {
                    addTrackerAction(true);
                }
            };
            checkBox.Click += (sender, e) => clickAction();
            if (startAsOn)
            {
                checkBox.Checked = true;
                clickAction();
            }

            checkBox.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem item = new ToolStripMenuItem("Add Additional Tracker");
            item.Click += (sender, e) => addTrackerAction(false);
            checkBox.ContextMenuStrip.Items.Add(item);
        }

        public void Update(bool updateView)
        {
            if (!_isLoaded) return;

            Config.Map3Gui.flowLayoutPanelMap3Trackers.UpdateControl();

            if (!updateView) return;

            UpdateBasedOnObjectsSelectedOnMap();
            UpdateControlsBasedOnSemaphores();
            UpdateDataTab();
            Config.Map3Gui.GLControl.Invalidate();
        }

        private void UpdateDataTab()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            int puX = PuUtilities.GetPuIndex(marioX);
            int puY = PuUtilities.GetPuIndex(marioY);
            int puZ = PuUtilities.GetPuIndex(marioZ);

            double qpuX = puX / 4.0;
            double qpuY = puY / 4.0;
            double qpuZ = puZ / 4.0;

            uint floorTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            float? yNorm = floorTriangleAddress == 0 ? (float?)null : Config.Stream.GetSingle(floorTriangleAddress + TriangleOffsetsConfig.NormY);

            byte level = Config.Stream.GetByte(MiscConfig.LevelAddress);
            byte area = Config.Stream.GetByte(MiscConfig.AreaAddress);
            ushort loadingPoint = Config.Stream.GetUInt16(MiscConfig.LoadingPointAddress);
            ushort missionLayout = Config.Stream.GetUInt16(MiscConfig.MissionAddress);

            MapLayout map = Config.MapAssociations.GetBestMap();

            Config.Map3Gui.labelMap3DataMapName.Text = map.Name;
            Config.Map3Gui.labelMap3DataMapSubName.Text = (map.SubName != null) ? map.SubName : "";
            Config.Map3Gui.labelMap3DataPuCoordinateValues.Text = string.Format("[{0}:{1}:{2}]", puX, puY, puZ);
            Config.Map3Gui.labelMap3DataQpuCoordinateValues.Text = string.Format("[{0}:{1}:{2}]", qpuX, qpuY, qpuZ);
            Config.Map3Gui.labelMap3DataId.Text = string.Format("[{0}:{1}:{2}:{3}]", level, area, loadingPoint, missionLayout);
            Config.Map3Gui.labelMap3DataYNorm.Text = yNorm?.ToString() ?? "(none)";
        }

        private void UpdateBasedOnObjectsSelectedOnMap()
        {
            // Determine which obj slots have been checked/unchecked since the last update
            List<int> currentObjIndexes = Config.ObjectSlotsManager.SelectedOnMap3SlotsAddresses
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address))
                .FindAll(address => address.HasValue)
                .ConvertAll(address => address.Value);
            List<int> toBeRemovedIndexes = _currentObjIndexes.FindAll(i => !currentObjIndexes.Contains(i));
            List<int> toBeAddedIndexes = currentObjIndexes.FindAll(i => !_currentObjIndexes.Contains(i));
            _currentObjIndexes = currentObjIndexes;

            // Newly unchecked slots have their semaphore turned off
            foreach (int index in toBeRemovedIndexes)
            {
                Map3Semaphore semaphore = Map3SemaphoreManager.Objects[index];
                semaphore.IsUsed = false;
            }

            // Newly checked slots have their semaphore turned on and a tracker is created
            foreach (int index in toBeAddedIndexes)
            {
                uint address = ObjectUtilities.GetObjectAddress(index);
                Map3Object mapObj = new Map3ObjectObject(address);
                Map3Semaphore semaphore = Map3SemaphoreManager.Objects[index];
                semaphore.IsUsed = true;
                Map3Tracker tracker = new Map3Tracker(
                    new List<Map3Object>() { mapObj }, new List<Map3Semaphore>() { semaphore });
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            }
        }

        private void UpdateControlsBasedOnSemaphores()
        {
            // Update checkboxes when tracker is deleted
            Config.Map3Gui.checkBoxMap3OptionsTrackMario.Checked = Map3SemaphoreManager.Mario.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackHolp.Checked = Map3SemaphoreManager.Holp.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCamera.Checked = Map3SemaphoreManager.Camera.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackFloorTri.Checked = Map3SemaphoreManager.FloorTri.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCeilingTri.Checked = Map3SemaphoreManager.CeilingTri.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCellGridlines.Checked = Map3SemaphoreManager.CellGridlines.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCurrentCell.Checked = Map3SemaphoreManager.CurrentCell.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackUnitGridlines.Checked = Map3SemaphoreManager.UnitGridlines.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCurrentUnit.Checked = Map3SemaphoreManager.CurrentUnit.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackNextPositions.Checked = Map3SemaphoreManager.NextPositions.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackSelf.Checked = Map3SemaphoreManager.Self.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackPoint.Checked = Map3SemaphoreManager.Point.IsUsed;

            // Update object slots when tracker is deleted
            Config.ObjectSlotsManager.SelectedOnMap3SlotsAddresses
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address))
                .FindAll(index => index.HasValue)
                .ConvertAll(index => index.Value)
                .FindAll(index => !Map3SemaphoreManager.Objects[index].IsUsed)
                .ConvertAll(index => ObjectUtilities.GetObjectAddress(index))
                .ForEach(address => Config.ObjectSlotsManager.SelectedOnMap3SlotsAddresses.Remove(address));
        }

        private void TrackMultipleObjects(List<uint> addresses)
        {
            if (addresses.Count == 0) return;
            List<Map3Object> mapObjs = addresses
                .ConvertAll(address => new Map3ObjectObject(address) as Map3Object);
            List<int> indexes = addresses
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address))
                .FindAll(index => index.HasValue)
                .ConvertAll(index => index.Value);
            List<Map3Semaphore> semaphores = indexes
                .ConvertAll(index => Map3SemaphoreManager.Objects[index]);
            semaphores.ForEach(semaphore => semaphore.IsUsed = true);
            Config.ObjectSlotsManager.SelectedOnMap3SlotsAddresses.AddRange(addresses);
            _currentObjIndexes.AddRange(indexes);
            Map3Tracker tracker = new Map3Tracker(mapObjs, semaphores);
            Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
        }
    }
}
