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
using STROOP.Map3;
using STROOP.Controls;
using STROOP.Map3.Map.Graphics;
using STROOP.Map3.Map;

namespace STROOP.Managers
{
    public class MapManager : DataManager
    {
        private List<int> _currentObjIndexes = new List<int>();

        private bool _isLoaded2D = false;
        private bool _isLoaded3D = false;

        public MapManager(string varFilePath)
            : base(varFilePath, Config.Map3Gui.watchVariablePanel3DVars)
        {
        }

        public void Load2D()
        {
            // Create new graphics control
            Config.Map3Graphics = new MapGraphics();
            Config.Map3Graphics.Load();
            _isLoaded2D = true;

            InitializeControls();
            InitializeSemaphores();
        }

        public void Load3D()
        {
            // Create new graphics control
            Config.Map4Graphics = new Map4Graphics();
            Config.Map4Graphics.Load();
            _isLoaded3D = true;
        }

        private void InitializeControls()
        {
            // FlowLayoutPanel
            Config.Map3Gui.flowLayoutPanelMap3Trackers.Initialize(
                new MapCurrentMapObject(), new MapCurrentBackgroundObject(), new MapHitboxHackTriangleObject());

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
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        MapObject mapObj = MapCustomFloorObject.Create(text);
                        if (mapObj == null) return;
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        MapObject mapObj = MapCustomWallObject.Create(text);
                        if (mapObj == null) return;
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                        MapObject mapObj = MapCustomCeilingObject.Create(text);
                        if (mapObj == null) return;
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapLevelFloorObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapLevelWallObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapLevelCeilingObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapHitboxHackTriangleObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapCustomMapObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapCustomBackgroundObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapCustomGridlinesObject();
                        MapTracker tracker = new MapTracker(mapObj);
                        Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
                    },
                    () =>
                    {
                        MapObject mapObj = new MapIwerlipsesObject();
                        MapTracker tracker = new MapTracker(mapObj);
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
            Config.Map3Gui.checkBoxMap3OptionsEnable3D.Click += (sender, e) =>
            {
                Config.Map3Gui.GLControl2D.Visible = !Config.Map3Gui.checkBoxMap3OptionsEnable3D.Checked;
                Config.Map3Gui.GLControl3D.Visible = Config.Map3Gui.checkBoxMap3OptionsEnable3D.Checked;
            };
            Config.Map3Gui.checkBoxMap3OptionsEnablePuView.Click += (sender, e) =>
                Config.Map3Graphics.MapViewEnablePuView = Config.Map3Gui.checkBoxMap3OptionsEnablePuView.Checked;
            Config.Map3Gui.checkBoxMap3OptionsScaleIconSizes.Click += (sender, e) =>
                Config.Map3Graphics.MapViewScaleIconSizes = Config.Map3Gui.checkBoxMap3OptionsScaleIconSizes.Checked;
            Config.Map3Gui.checkBoxMap3ControllersCenterChangeByPixels.Click += (sender, e) =>
                Config.Map3Graphics.MapViewCenterChangeByPixels = Config.Map3Gui.checkBoxMap3ControllersCenterChangeByPixels.Checked;

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

            // 3D Controllers
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
                Config.Map3Gui.groupBoxMapCameraPosition,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXn"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXp"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionZn"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionZp"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXnZn"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXnZp"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXpZn"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionXpZp"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionYp"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["buttonMapCameraPositionYn"] as Button,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["textBoxMapCameraPositionXZ"] as TextBox,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["textBoxMapCameraPositionY"] as TextBox,
                Config.Map3Gui.groupBoxMapCameraPosition.Controls["checkBoxMapCameraPositionRelative"] as CheckBox,
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
                Config.Map3Gui.groupBoxMapCameraSpherical,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTn"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTp"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalPn"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalPp"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTnPn"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTnPp"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTpPn"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalTpPp"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalRn"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["buttonMapCameraSphericalRp"] as Button,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["textBoxMapCameraSphericalTP"] as TextBox,
                Config.Map3Gui.groupBoxMapCameraSpherical.Controls["textBoxMapCameraSphericalR"] as TextBox,
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
                Config.Map3Gui.groupBoxMapFocusPosition,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXn"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXp"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionZn"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionZp"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXnZn"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXnZp"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXpZn"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionXpZp"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionYp"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["buttonMapFocusPositionYn"] as Button,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["textBoxMapFocusPositionXZ"] as TextBox,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["textBoxMapFocusPositionY"] as TextBox,
                Config.Map3Gui.groupBoxMapFocusPosition.Controls["checkBoxMapFocusPositionRelative"] as CheckBox,
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
                Config.Map3Gui.groupBoxMapFocusSpherical,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTn"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTp"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalPp"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalPn"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTnPp"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTnPn"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTpPp"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalTpPn"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalRp"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["buttonMapFocusSphericalRn"] as Button,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["textBoxMapFocusSphericalTP"] as TextBox,
                Config.Map3Gui.groupBoxMapFocusSpherical.Controls["textBoxMapFocusSphericalR"] as TextBox,
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
                Config.Map3Gui.groupBoxMapCameraFocus,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXn"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXp"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusZn"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusZp"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXnZn"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXnZp"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXpZn"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusXpZp"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusYp"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["buttonMapCameraFocusYn"] as Button,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["textBoxMapCameraFocusXZ"] as TextBox,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["textBoxMapCameraFocusY"] as TextBox,
                Config.Map3Gui.groupBoxMapCameraFocus.Controls["checkBoxMapCameraFocusRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateMapCameraFocus(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            // FOV
            Config.Map3Gui.trackBarMapFov.ValueChanged += (sender, e) =>
            {
                SpecialConfig.Map3DFOV = Config.Map3Gui.trackBarMapFov.Value;
                Config.Map3Gui.textBoxMapFov.Text = Config.Map3Gui.trackBarMapFov.Value.ToString();
            };

            Config.Map3Gui.textBoxMapFov.AddEnterAction(() =>
            {
                float parsed = ParsingUtilities.ParseFloat(Config.Map3Gui.textBoxMapFov.Text);
                if (parsed > 0 && parsed < 180)
                {
                    SpecialConfig.Map3DFOV = parsed;
                    ControlUtilities.SetTrackBarValueCapped(Config.Map3Gui.trackBarMapFov, parsed);
                }
            });
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
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackMario, MapSemaphoreManager.Mario, () => new MapMarioObject(), true);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackHolp, MapSemaphoreManager.Holp, () => new MapHolpObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCamera, MapSemaphoreManager.Camera, () => new MapCameraObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackFloorTri, MapSemaphoreManager.FloorTri, () => new MapMarioFloorObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCeilingTri, MapSemaphoreManager.CeilingTri, () => new MapMarioCeilingObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCellGridlines, MapSemaphoreManager.CellGridlines, () => new MapCellGridlinesObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCurrentCell, MapSemaphoreManager.CurrentCell, () => new MapCurrentCellObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackUnitGridlines, MapSemaphoreManager.UnitGridlines, () => new MapUnitGridlinesObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackCurrentUnit, MapSemaphoreManager.CurrentUnit, () => new MapCurrentUnitObject(PositionAngle.Mario), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackNextPositions, MapSemaphoreManager.NextPositions, () => new MapNextPositionsObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackSelf, MapSemaphoreManager.Self, () => new MapSelfObject(), false);
            InitializeCheckboxSemaphore(Config.Map3Gui.checkBoxMap3OptionsTrackPoint, MapSemaphoreManager.Point, () => new MapPointObject(), false);
        }

        private void InitializeCheckboxSemaphore(
            CheckBox checkBox, MapSemaphore semaphore, Func<MapObject> mapObjFunc, bool startAsOn)
        {
            Action<bool> addTrackerAction = (bool withSemaphore) =>
            {
                MapTracker tracker = new MapTracker(
                    new List<MapObject>() { mapObjFunc() },
                    withSemaphore ? new List<MapSemaphore>() { semaphore } : new List<MapSemaphore>());
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

        public override void Update(bool updateView)
        {
            if (!_isLoaded2D) return;
            if (Config.Map3Gui.checkBoxMap3OptionsEnable3D.Checked && !_isLoaded3D) return;

            Config.Map3Gui.flowLayoutPanelMap3Trackers.UpdateControl();

            if (!updateView) return;

            base.Update(updateView);
            UpdateBasedOnObjectsSelectedOnMap();
            UpdateControlsBasedOnSemaphores();
            UpdateDataTab();
            UpdateVarColors();

            if (Config.Map3Gui.checkBoxMap3OptionsEnable3D.Checked)
            {
                Config.Map3Gui.GLControl3D.Invalidate();
            }
            else
            {
                Config.Map3Gui.GLControl2D.Invalidate();
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

            Config.Map3Gui.labelMap3DataMapName.Text = map.Name;
            Config.Map3Gui.labelMap3DataMapSubName.Text = map.SubName ?? "";
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
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            }
        }

        private void UpdateControlsBasedOnSemaphores()
        {
            // Update checkboxes when tracker is deleted
            Config.Map3Gui.checkBoxMap3OptionsTrackMario.Checked = MapSemaphoreManager.Mario.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackHolp.Checked = MapSemaphoreManager.Holp.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCamera.Checked = MapSemaphoreManager.Camera.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackFloorTri.Checked = MapSemaphoreManager.FloorTri.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCeilingTri.Checked = MapSemaphoreManager.CeilingTri.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCellGridlines.Checked = MapSemaphoreManager.CellGridlines.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCurrentCell.Checked = MapSemaphoreManager.CurrentCell.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackUnitGridlines.Checked = MapSemaphoreManager.UnitGridlines.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackCurrentUnit.Checked = MapSemaphoreManager.CurrentUnit.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackNextPositions.Checked = MapSemaphoreManager.NextPositions.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackSelf.Checked = MapSemaphoreManager.Self.IsUsed;
            Config.Map3Gui.checkBoxMap3OptionsTrackPoint.Checked = MapSemaphoreManager.Point.IsUsed;

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
            Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
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
