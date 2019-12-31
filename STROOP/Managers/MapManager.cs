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
using STROOP.Map;
using STROOP.Map.Map3D;

namespace STROOP.Managers
{
    public class MapManager : DataManager
    {
        private List<int> _currentObjIndexes = new List<int>();

        private bool _isLoaded2D = false;
        private bool _isLoaded3D = false;

        public MapManager(string varFilePath)
            : base(varFilePath, Config.MapGui.watchVariablePanel3DVars)
        {
        }

        public void Load2D()
        {
            // Create new graphics control
            Config.MapGraphics = new MapGraphics();
            Config.MapGraphics.Load();
            _isLoaded2D = true;

            InitializeControls();
            InitializeSemaphores();
        }

        public void Load3D()
        {
            // Create new graphics control
            Config.Map3DGraphics = new Map3DGraphics();
            Config.Map3DGraphics.Load();
            _isLoaded3D = true;
        }

        private void InitializeControls()
        {
            // FlowLayoutPanel
            Config.MapGui.flowLayoutPanelMap3Trackers.Initialize(
                new MapCurrentMapObject(), new MapCurrentBackgroundObject(), new MapHitboxHackTriangleObject());

            // ComboBox for Level
            List<MapLayout> mapLayouts = Config.MapAssociations.GetAllMaps();
            List<object> mapLayoutChoices = new List<object>() { "Recommended" };
            mapLayouts.ForEach(mapLayout => mapLayoutChoices.Add(mapLayout));
            Config.MapGui.comboBoxMap3OptionsLevel.DataSource = mapLayoutChoices;

            // ComboBox for Background
            List<BackgroundImage> backgroundImages = Config.MapAssociations.GetAllBackgroundImages();
            List<object> backgroundImageChoices = new List<object>() { "Recommended" };
            backgroundImages.ForEach(backgroundImage => backgroundImageChoices.Add(backgroundImage));
            Config.MapGui.comboBoxMap3OptionsBackground.DataSource = backgroundImageChoices;

            // Buttons on Options
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.buttonMap3OptionsAddNewTracker,
                new List<string>()
                {
                    "Add Tracker for Custom Points",
                    "Add Tracker for Custom Floor Tris",
                    "Add Tracker for Custom Wall Tris",
                    "Add Tracker for Custom Ceiling Tris",
                    "Add Tracker for Level Floor Tris",
                    "Add Tracker for Level Wall Tris",
                    "Add Tracker for Level Ceiling Tris",
                    "Add Tracker for Hitbox Hack Tris",
                    "Add Tracker for Custom Map",
                    "Add Tracker for Custom Background",
                    "Add Tracker for Custom Gridlines",
                    "Add Tracker for Iwerlipses",
                },
                new List<Action>()
                {
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter points as pairs of integers.");
                        MapObject mapObj = MapCustomPointsObject.Create(text);
                        if (mapObj == null) return;
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        MapObject mapObj = MapCustomFloorObject.Create(text);
                        if (mapObj == null) return;
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        MapObject mapObj = MapCustomWallObject.Create(text);
                        if (mapObj == null) return;
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        MapObject mapObj = MapCustomCeilingObject.Create(text);
                        if (mapObj == null) return;
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapLevelFloorObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapLevelWallObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapLevelCeilingObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapHitboxHackTriangleObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapCustomMapObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapCustomBackgroundObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapCustomGridlinesObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapIwerlipsesObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                });
            Config.MapGui.buttonMap3OptionsAddNewTracker.Click += (sender, e) =>
                Config.MapGui.buttonMap3OptionsAddNewTracker.ContextMenuStrip.Show(Cursor.Position);
            Config.MapGui.buttonMap3OptionsClearAllTrackers.Click += (sender, e) =>
                Config.MapGui.flowLayoutPanelMap3Trackers.ClearControls();
            Config.MapGui.buttonMap3OptionsTrackAllObjects.Click += (sender, e) =>
                TrackMultipleObjects(ObjectUtilities.GetAllObjectAddresses());
            Config.MapGui.buttonMap3OptionsTrackMarkedObjects.Click += (sender, e) =>
                TrackMultipleObjects(Config.ObjectSlotsManager.MarkedSlotsAddresses);

            // Buttons for Changing Scale
            Config.MapGui.buttonMap3ControllersScaleMinus.Click += (sender, e) =>
                Config.MapGraphics.ChangeScale(-1, Config.MapGui.textBoxMap3ControllersScaleChange.Text);
            Config.MapGui.buttonMap3ControllersScalePlus.Click += (sender, e) =>
                Config.MapGraphics.ChangeScale(1, Config.MapGui.textBoxMap3ControllersScaleChange.Text);
            Config.MapGui.buttonMap3ControllersScaleDivide.Click += (sender, e) =>
                Config.MapGraphics.ChangeScale2(-1, Config.MapGui.textBoxMap3ControllersScaleChange2.Text);
            Config.MapGui.buttonMap3ControllersScaleTimes.Click += (sender, e) =>
                Config.MapGraphics.ChangeScale2(1, Config.MapGui.textBoxMap3ControllersScaleChange2.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.groupBoxMap3ControllersScale,
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
                    () => Config.MapGraphics.SetCustomScale(6),
                    () => Config.MapGraphics.SetCustomScale(12),
                    () => Config.MapGraphics.SetCustomScale(18),
                    () => Config.MapGraphics.SetCustomScale(24),
                    () => Config.MapGraphics.SetCustomScale(40),
                });

            // Buttons for Changing Center
            Config.MapGui.buttonMap3ControllersCenterUp.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(0, -1, Config.MapGui.textBoxMap3ControllersCenterChange.Text);
            Config.MapGui.buttonMap3ControllersCenterDown.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(0, 1, Config.MapGui.textBoxMap3ControllersCenterChange.Text);
            Config.MapGui.buttonMap3ControllersCenterLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, 0, Config.MapGui.textBoxMap3ControllersCenterChange.Text);
            Config.MapGui.buttonMap3ControllersCenterRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, 0, Config.MapGui.textBoxMap3ControllersCenterChange.Text);
            Config.MapGui.buttonMap3ControllersCenterUpLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, -1, Config.MapGui.textBoxMap3ControllersCenterChange.Text);
            Config.MapGui.buttonMap3ControllersCenterUpRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, -1, Config.MapGui.textBoxMap3ControllersCenterChange.Text);
            Config.MapGui.buttonMap3ControllersCenterDownLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, 1, Config.MapGui.textBoxMap3ControllersCenterChange.Text);
            Config.MapGui.buttonMap3ControllersCenterDownRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, 1, Config.MapGui.textBoxMap3ControllersCenterChange.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.groupBoxMap3ControllersCenter,
                new List<string>() { "Center on Mario" },
                new List<Action>()
                {
                    () =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        Config.MapGraphics.SetCustomCenter(marioX + "," + marioZ);
                    }
                });

            // Buttons for Changing Angle
            Config.MapGui.buttonMap3ControllersAngleCCW.Click += (sender, e) =>
                Config.MapGraphics.ChangeAngle(-1, Config.MapGui.textBoxMap3ControllersAngleChange.Text);
            Config.MapGui.buttonMap3ControllersAngleCW.Click += (sender, e) =>
                Config.MapGraphics.ChangeAngle(1, Config.MapGui.textBoxMap3ControllersAngleChange.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.groupBoxMap3ControllersAngle,
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
                        Config.MapGraphics.SetCustomAngle(marioAngle);
                    },
                    () =>
                    {
                        ushort cameraAngle = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                        Config.MapGraphics.SetCustomAngle(cameraAngle);
                    },
                    () =>
                    {
                        ushort centripetalAngle = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset);
                        double centripetalAngleReversed = MoreMath.ReverseAngle(centripetalAngle);
                        Config.MapGraphics.SetCustomAngle(centripetalAngleReversed);
                    },
                });

            // TextBoxes for Custom Values
            Config.MapGui.textBoxMap3ControllersScaleCustom.AddEnterAction(() =>
            {
                Config.MapGui.radioButtonMap3ControllersScaleCustom.Checked = true;
            });
            Config.MapGui.textBoxMap3ControllersCenterCustom.AddEnterAction(() =>
            {
                Config.MapGui.radioButtonMap3ControllersCenterCustom.Checked = true;
            });
            Config.MapGui.textBoxMap3ControllersAngleCustom.AddEnterAction(() =>
            {
                Config.MapGui.radioButtonMap3ControllersAngleCustom.Checked = true;
            });

            // Additional Checkboxes
            Config.MapGui.checkBoxMap3OptionsEnable3D.Click += (sender, e) =>
            {
                Config.MapGui.GLControl2D.Visible = !Config.MapGui.checkBoxMap3OptionsEnable3D.Checked;
                Config.MapGui.GLControl3D.Visible = Config.MapGui.checkBoxMap3OptionsEnable3D.Checked;
            };
            Config.MapGui.checkBoxMap3OptionsEnablePuView.Click += (sender, e) =>
                Config.MapGraphics.MapViewEnablePuView = Config.MapGui.checkBoxMap3OptionsEnablePuView.Checked;
            Config.MapGui.checkBoxMap3OptionsScaleIconSizes.Click += (sender, e) =>
                Config.MapGraphics.MapViewScaleIconSizes = Config.MapGui.checkBoxMap3OptionsScaleIconSizes.Checked;
            Config.MapGui.checkBoxMap3ControllersCenterChangeByPixels.Click += (sender, e) =>
                Config.MapGraphics.MapViewCenterChangeByPixels = Config.MapGui.checkBoxMap3ControllersCenterChangeByPixels.Checked;

            // Global Icon Size
            Config.MapGui.textBoxMap3OptionsGlobalIconSize.AddEnterAction(() =>
            {
                float? parsed = ParsingUtilities.ParseFloatNullable(
                    Config.MapGui.textBoxMap3OptionsGlobalIconSize.Text);
                if (!parsed.HasValue) return;
                SetGlobalIconSize(parsed.Value);
            });
            Config.MapGui.trackBarMap3OptionsGlobalIconSize.AddManualChangeAction(() =>
                SetGlobalIconSize(Config.MapGui.trackBarMap3OptionsGlobalIconSize.Value));

            // 3D Controllers
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
                Config.MapGui.groupBoxMapCameraPosition,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXn"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXp"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionZn"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionZp"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXnZn"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXnZp"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXpZn"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXpZp"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionYp"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionYn"] as Button,
                Config.MapGui.groupBoxMapCameraPosition.Controls["textBoxMapCameraPositionXZ"] as TextBox,
                Config.MapGui.groupBoxMapCameraPosition.Controls["textBoxMapCameraPositionY"] as TextBox,
                Config.MapGui.groupBoxMapCameraPosition.Controls["checkBoxMapCameraPositionRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateMapCameraPosition(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Spherical,
                false,
                Config.MapGui.groupBoxMapCameraSpherical,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTn"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTp"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalPn"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalPp"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTnPn"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTnPp"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTpPn"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTpPp"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalRn"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalRp"] as Button,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["textBoxMapCameraSphericalTP"] as TextBox,
                Config.MapGui.groupBoxMapCameraSpherical.Controls["textBoxMapCameraSphericalR"] as TextBox,
                null /* checkbox */,
                (float hOffset, float vOffset, float nOffset, bool _) =>
                {
                    ButtonUtilities.TranslateMapCameraSpherical(
                        -1 * nOffset,
                        hOffset,
                        vOffset);
                });

            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
                Config.MapGui.groupBoxMapFocusPosition,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXn"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXp"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionZn"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionZp"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXnZn"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXnZp"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXpZn"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXpZp"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionYp"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionYn"] as Button,
                Config.MapGui.groupBoxMapFocusPosition.Controls["textBoxMapFocusPositionXZ"] as TextBox,
                Config.MapGui.groupBoxMapFocusPosition.Controls["textBoxMapFocusPositionY"] as TextBox,
                Config.MapGui.groupBoxMapFocusPosition.Controls["checkBoxMapFocusPositionRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateMapFocusPosition(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Spherical,
                false,
                Config.MapGui.groupBoxMapFocusSpherical,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTn"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTp"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalPp"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalPn"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTnPp"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTnPn"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTpPp"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTpPn"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalRp"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalRn"] as Button,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["textBoxMapFocusSphericalTP"] as TextBox,
                Config.MapGui.groupBoxMapFocusSpherical.Controls["textBoxMapFocusSphericalR"] as TextBox,
                null /* checkbox */,
                (float hOffset, float vOffset, float nOffset, bool _) =>
                {
                    ButtonUtilities.TranslateMapFocusSpherical(
                        nOffset,
                        hOffset,
                        vOffset);
                });

            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
                Config.MapGui.groupBoxMapCameraFocus,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXn"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXp"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusZn"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusZp"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXnZn"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXnZp"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXpZn"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXpZp"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusYp"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusYn"] as Button,
                Config.MapGui.groupBoxMapCameraFocus.Controls["textBoxMapCameraFocusXZ"] as TextBox,
                Config.MapGui.groupBoxMapCameraFocus.Controls["textBoxMapCameraFocusY"] as TextBox,
                Config.MapGui.groupBoxMapCameraFocus.Controls["checkBoxMapCameraFocusRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateMapCameraFocus(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            // FOV
            Config.MapGui.trackBarMapFov.ValueChanged += (sender, e) =>
            {
                SpecialConfig.Map3DFOV = Config.MapGui.trackBarMapFov.Value;
                Config.MapGui.textBoxMapFov.Text = Config.MapGui.trackBarMapFov.Value.ToString();
            };

            Config.MapGui.textBoxMapFov.AddEnterAction(() =>
            {
                float parsed = ParsingUtilities.ParseFloat(Config.MapGui.textBoxMapFov.Text);
                if (parsed > 0 && parsed < 180)
                {
                    SpecialConfig.Map3DFOV = parsed;
                    ControlUtilities.SetTrackBarValueCapped(Config.MapGui.trackBarMapFov, parsed);
                }
            });
        }

        private void SetGlobalIconSize(float size)
        {
            Config.MapGui.flowLayoutPanelMap3Trackers.SetGlobalIconSize(size);
            Config.MapGui.textBoxMap3OptionsGlobalIconSize.SubmitText(size.ToString());
            Config.MapGui.trackBarMap3OptionsGlobalIconSize.StartChangingByCode();
            ControlUtilities.SetTrackBarValueCapped(Config.MapGui.trackBarMap3OptionsGlobalIconSize, size);
            Config.MapGui.trackBarMap3OptionsGlobalIconSize.StopChangingByCode();
        }

        private void InitializeSemaphores()
        {
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackMario, MapSemaphoreManager.Mario, () => new MapMarioObject(), true);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackHolp, MapSemaphoreManager.Holp, () => new MapHolpObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackCamera, MapSemaphoreManager.Camera, () => new MapCameraObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackFloorTri, MapSemaphoreManager.FloorTri, () => new MapMarioFloorObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackCeilingTri, MapSemaphoreManager.CeilingTri, () => new MapMarioCeilingObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackCellGridlines, MapSemaphoreManager.CellGridlines, () => new MapCellGridlinesObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackCurrentCell, MapSemaphoreManager.CurrentCell, () => new MapCurrentCellObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackUnitGridlines, MapSemaphoreManager.UnitGridlines, () => new MapUnitGridlinesObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackCurrentUnit, MapSemaphoreManager.CurrentUnit, () => new MapCurrentUnitObject(PositionAngle.Mario), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackNextPositions, MapSemaphoreManager.NextPositions, () => new MapNextPositionsObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackSelf, MapSemaphoreManager.Self, () => new MapSelfObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMap3OptionsTrackPoint, MapSemaphoreManager.Point, () => new MapPointObject(), false);
        }

        private void InitializeCheckboxSemaphore(
            CheckBox checkBox, MapSemaphore semaphore, Func<MapObject> mapObjFunc, bool startAsOn)
        {
            Action<bool> addTrackerAction = (bool withSemaphore) =>
            {
                MapTracker tracker = new MapTracker(
                    new List<MapObject>() { mapObjFunc() },
                    withSemaphore ? new List<MapSemaphore>() { semaphore } : new List<MapSemaphore>());
                Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
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

        public override void Update(bool updateView)
        {
            if (!_isLoaded2D) return;
            if (Config.MapGui.checkBoxMap3OptionsEnable3D.Checked && !_isLoaded3D) return;

            Config.MapGui.flowLayoutPanelMap3Trackers.UpdateControl();

            if (!updateView) return;

            base.Update(updateView);
            UpdateBasedOnObjectsSelectedOnMap();
            UpdateControlsBasedOnSemaphores();
            UpdateDataTab();
            UpdateVarColors();

            if (Config.MapGui.checkBoxMap3OptionsEnable3D.Checked)
            {
                Config.MapGui.GLControl3D.Invalidate();
            }
            else
            {
                Config.MapGui.GLControl2D.Invalidate();
            }
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

            Config.MapGui.labelMap3DataMapName.Text = map.Name;
            Config.MapGui.labelMap3DataMapSubName.Text = map.SubName ?? "";
            Config.MapGui.labelMap3DataPuCoordinateValues.Text = string.Format("[{0}:{1}:{2}]", puX, puY, puZ);
            Config.MapGui.labelMap3DataQpuCoordinateValues.Text = string.Format("[{0}:{1}:{2}]", qpuX, qpuY, qpuZ);
            Config.MapGui.labelMap3DataId.Text = string.Format("[{0}:{1}:{2}:{3}]", level, area, loadingPoint, missionLayout);
            Config.MapGui.labelMap3DataYNorm.Text = yNorm?.ToString() ?? "(none)";
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
                MapSemaphore semaphore = MapSemaphoreManager.Objects[index];
                semaphore.IsUsed = false;
            }

            // Newly checked slots have their semaphore turned on and a tracker is created
            foreach (int index in toBeAddedIndexes)
            {
                uint address = ObjectUtilities.GetObjectAddress(index);
                MapObject mapObj = new MapObjectObject(address);
                MapSemaphore semaphore = MapSemaphoreManager.Objects[index];
                semaphore.IsUsed = true;
                MapTracker tracker = new MapTracker(
                    new List<MapObject>() { mapObj }, new List<MapSemaphore>() { semaphore });
                Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            }
        }

        private void UpdateControlsBasedOnSemaphores()
        {
            // Update checkboxes when tracker is deleted
            Config.MapGui.checkBoxMap3OptionsTrackMario.Checked = MapSemaphoreManager.Mario.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackHolp.Checked = MapSemaphoreManager.Holp.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackCamera.Checked = MapSemaphoreManager.Camera.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackFloorTri.Checked = MapSemaphoreManager.FloorTri.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackCeilingTri.Checked = MapSemaphoreManager.CeilingTri.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackCellGridlines.Checked = MapSemaphoreManager.CellGridlines.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackCurrentCell.Checked = MapSemaphoreManager.CurrentCell.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackUnitGridlines.Checked = MapSemaphoreManager.UnitGridlines.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackCurrentUnit.Checked = MapSemaphoreManager.CurrentUnit.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackNextPositions.Checked = MapSemaphoreManager.NextPositions.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackSelf.Checked = MapSemaphoreManager.Self.IsUsed;
            Config.MapGui.checkBoxMap3OptionsTrackPoint.Checked = MapSemaphoreManager.Point.IsUsed;

            // Update object slots when tracker is deleted
            Config.ObjectSlotsManager.SelectedOnMap3SlotsAddresses
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address))
                .FindAll(index => index.HasValue)
                .ConvertAll(index => index.Value)
                .FindAll(index => !MapSemaphoreManager.Objects[index].IsUsed)
                .ConvertAll(index => ObjectUtilities.GetObjectAddress(index))
                .ForEach(address => Config.ObjectSlotsManager.SelectedOnMap3SlotsAddresses.Remove(address));
        }

        private void TrackMultipleObjects(List<uint> addresses)
        {
            if (addresses.Count == 0) return;
            List<MapObject> mapObjs = addresses
                .ConvertAll(address => new MapObjectObject(address) as MapObject);
            List<int> indexes = addresses
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address))
                .FindAll(index => index.HasValue)
                .ConvertAll(index => index.Value);
            List<MapSemaphore> semaphores = indexes
                .ConvertAll(index => MapSemaphoreManager.Objects[index]);
            semaphores.ForEach(semaphore => semaphore.IsUsed = true);
            Config.ObjectSlotsManager.SelectedOnMap3SlotsAddresses.AddRange(addresses);
            _currentObjIndexes.AddRange(indexes);
            MapTracker tracker = new MapTracker(mapObjs, semaphores);
            Config.MapGui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
        }

        private static readonly List<string> inGameColoredVars = new List<string>() { };
        private static readonly List<string> cameraPosAndFocusColoredVars = new List<string>()
        {
            "Camera X", "Camera Y", "Camera Z", "Focus X", "Focus Y", "Focus Z",
        };
        private static readonly List<string> cameraPosAndAngleColoredVars = new List<string>()
        {
            "Camera X", "Camera Y", "Camera Z", "Camera Yaw", "Camera Pitch", "Camera Roll",
        };
        private static readonly List<string> followFocusRelativeAngleColoredVars = new List<string>()
        {
            "Focus Pos PA", "Focus Angle PA", "Following Radius", "Following Y Offset", "Following Yaw",
        };
        private static readonly List<string> followFocusAbsoluteAngleColoredVars = new List<string>()
        {
            "Focus Pos PA", "Following Radius", "Following Y Offset", "Following Yaw",
        };
        private static readonly Dictionary<Map3DMode, List<string>> coloredVarsMap =
            new Dictionary<Map3DMode, List<string>>()
            {
                [Map3DMode.InGame] = inGameColoredVars,
                [Map3DMode.CameraPosAndFocus] = cameraPosAndFocusColoredVars,
                [Map3DMode.CameraPosAndAngle] = cameraPosAndAngleColoredVars,
                [Map3DMode.FollowFocusRelativeAngle] = followFocusRelativeAngleColoredVars,
                [Map3DMode.FollowFocusAbsoluteAngle] = followFocusAbsoluteAngleColoredVars,
            };

        private void UpdateVarColors()
        {
            List<string> coloredVarNames = coloredVarsMap[SpecialConfig.Map3DMode];
            _variablePanel.ColorVarsUsingFunction(
                control => coloredVarNames.Contains(control.VarName) ?
                ColorUtilities.GetColorFromString("Red") : SystemColors.Control);
        }
    }
}
