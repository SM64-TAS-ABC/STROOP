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

        public bool PauseMapUpdating = false;
        private bool _isLoaded2D = false;
        private bool _isLoaded3D = false;

        public MapManager(string varFilePath)
            : base(varFilePath, Config.MapGui.watchVariablePanelMap3DVars)
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
            Config.MapGui.flowLayoutPanelMapTrackers.Initialize(
                new MapCurrentMapObject(), new MapCurrentBackgroundObject(), new MapHitboxHackTriangleObject());

            // ComboBox for Level
            List<MapLayout> mapLayouts = Config.MapAssociations.GetAllMaps();
            List<object> mapLayoutChoices = new List<object>() { "Recommended" };
            mapLayouts.ForEach(mapLayout => mapLayoutChoices.Add(mapLayout));
            Config.MapGui.comboBoxMapOptionsLevel.DataSource = mapLayoutChoices;

            // ComboBox for Background
            List<BackgroundImage> backgroundImages = Config.MapAssociations.GetAllBackgroundImages();
            List<object> backgroundImageChoices = new List<object>() { "Recommended" };
            backgroundImages.ForEach(backgroundImage => backgroundImageChoices.Add(backgroundImage));
            Config.MapGui.comboBoxMapOptionsBackground.DataSource = backgroundImageChoices;

            // Buttons on Options

            ToolStripMenuItem itemAllObjects = new ToolStripMenuItem("Add Tracker for All Objects");
            itemAllObjects.Click += (sender, e) =>
            {
                TrackMultipleObjects(ObjectUtilities.GetAllObjectAddresses());
            };

            ToolStripMenuItem itemMarkedObjects = new ToolStripMenuItem("Add Tracker for Marked Objects");
            itemMarkedObjects.Click += (sender, e) =>
            {
                TrackMultipleObjects(Config.ObjectSlotsManager.MarkedSlotsAddresses);
            };

            ToolStripMenuItem itemLevelFloorTris = new ToolStripMenuItem("Add Tracker for Level Floor Tris");
            itemLevelFloorTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapLevelFloorObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemLevelWallTris = new ToolStripMenuItem("Add Tracker for Level Wall Tris");
            itemLevelWallTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapLevelWallObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemLevelCeilingTris = new ToolStripMenuItem("Add Tracker for Level Ceiling Tris");
            itemLevelCeilingTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapLevelCeilingObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomFloorTris = new ToolStripMenuItem("Add Tracker for Custom Floor Tris");
            itemCustomFloorTris.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                MapObject mapObj = MapCustomFloorObject.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomWallTris = new ToolStripMenuItem("Add Tracker for Custom Wall Tris");
            itemCustomWallTris.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                MapObject mapObj = MapCustomWallObject.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomCeilingTris = new ToolStripMenuItem("Add Tracker for Custom Ceiling Tris");
            itemCustomCeilingTris.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                MapObject mapObj = MapCustomCeilingObject.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomUnitPoints = new ToolStripMenuItem("Add Tracker for Custom Unit Points");
            itemCustomUnitPoints.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter points as pairs of floats.");
                MapObject mapObj = MapCustomUnitPointsObject.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomCirclePoints = new ToolStripMenuItem("Add Tracker for Custom Circle Points");
            itemCustomCirclePoints.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter points as pairs of floats.");
                MapObject mapObj = MapCustomCirclePointsObject.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomMap = new ToolStripMenuItem("Add Tracker for Custom Map");
            itemCustomMap.Click += (sender, e) =>
            {
                MapObject mapObj = new MapCustomMapObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomBackground = new ToolStripMenuItem("Add Tracker for Custom Background");
            itemCustomBackground.Click += (sender, e) =>
            {
                MapObject mapObj = new MapCustomBackgroundObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemUnitGridlines = new ToolStripMenuItem("Add Tracker for Unit Gridlines");
            itemUnitGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapUnitGridlinesObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemFloatGridlines = new ToolStripMenuItem("Add Tracker for Float Gridlines");
            itemFloatGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapFloatGridlinesObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCellGridlines = new ToolStripMenuItem("Add Tracker for Cell Gridlines");
            itemCellGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapCellGridlinesObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPuGridlines = new ToolStripMenuItem("Add Tracker for PU Gridlines");
            itemPuGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapPuGridlinesObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomGridlines = new ToolStripMenuItem("Add Tracker for Custom Gridlines");
            itemCustomGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapCustomGridlinesObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemIwerlipses = new ToolStripMenuItem("Add Tracker for Iwerlipses");
            itemIwerlipses.Click += (sender, e) =>
            {
                MapObject mapObj = new MapIwerlipsesObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemNextPositions = new ToolStripMenuItem("Add Tracker for Next Positions");
            itemNextPositions.Click += (sender, e) =>
            {
                MapObject mapObj = new MapNextPositionsObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPreviousPositions = new ToolStripMenuItem("Add Tracker for Previous Positions");
            itemPreviousPositions.Click += (sender, e) =>
            {
                MapObject mapObj = new MapPreviousPositionsObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCurrentUnit = new ToolStripMenuItem("Add Tracker for Current Unit");
            itemCurrentUnit.Click += (sender, e) =>
            {
                MapObject mapObj = new MapCurrentUnitObject(PositionAngle.Mario);
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCurrentCell = new ToolStripMenuItem("Add Tracker for Current Cell");
            itemCurrentCell.Click += (sender, e) =>
            {
                MapObject mapObj = new MapCurrentCellObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCUpFloorTris = new ToolStripMenuItem("Add Tracker for C-Up Floor Tris");
            itemCUpFloorTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapCUpFloorObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPunchFloorTris = new ToolStripMenuItem("Add Tracker for Punch Floor Tris");
            itemPunchFloorTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapPunchFloorObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPunchDetector = new ToolStripMenuItem("Add Tracker for Punch Detector");
            itemPunchDetector.Click += (sender, e) =>
            {
                MapObject mapObj = new MapPunchDetectorObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemHitboxHackTris = new ToolStripMenuItem("Add Tracker for Hitbox Hack Tris");
            itemHitboxHackTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapHitboxHackTriangleObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemAggregatedPath = new ToolStripMenuItem("Add Tracker for Aggregated Path");
            itemAggregatedPath.Click += (sender, e) =>
            {
                MapObject mapObj = new MapAggregatedPathObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCompass = new ToolStripMenuItem("Add Tracker for Compass");
            itemCompass.Click += (sender, e) =>
            {
                MapObject mapObj = new MapCompassObject();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomPositionAngle = new ToolStripMenuItem("Add Tracker for Custom PositionAngle");
            itemCustomPositionAngle.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter a PositionAngle.");
                PositionAngle posAngle = PositionAngle.FromString(text);
                if (posAngle == null) return;
                MapObject mapObj = new MapCustomPositionAngleObject(posAngle);
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemLineSegment = new ToolStripMenuItem("Add Tracker for Line Segment");
            itemLineSegment.Click += (sender, e) =>
            {
                string text1 = DialogUtilities.GetStringFromDialog(labelText: "Enter the first PositionAngle.");
                if (text1 == null) return;
                string text2 = DialogUtilities.GetStringFromDialog(labelText: "Enter the second PositionAngle.");
                if (text2 == null) return;
                MapObject mapObj = MapLineSegmentObject.Create(text1, text2);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip = new ContextMenuStrip();
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemAllObjects);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemMarkedObjects);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLevelFloorTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLevelWallTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLevelCeilingTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomFloorTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomWallTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomCeilingTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomUnitPoints);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomCirclePoints);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomMap);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomBackground);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemUnitGridlines);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemFloatGridlines);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCellGridlines);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemPuGridlines);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomGridlines);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemIwerlipses);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemNextPositions);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemPreviousPositions);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCurrentUnit);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCurrentCell);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCUpFloorTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemPunchFloorTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemPunchDetector);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemHitboxHackTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemAggregatedPath);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCompass);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomPositionAngle);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLineSegment);

            Config.MapGui.buttonMapOptionsAddNewTracker.Click += (sender, e) =>
                Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Show(Cursor.Position);
            Config.MapGui.buttonMapOptionsClearAllTrackers.Click += (sender, e) =>
                Config.MapGui.flowLayoutPanelMapTrackers.ClearControls();

            // Buttons for Changing Scale
            Config.MapGui.buttonMapControllersScaleMinus.Click += (sender, e) =>
                Config.MapGraphics.ChangeScale(-1, Config.MapGui.textBoxMapControllersScaleChange.Text);
            Config.MapGui.buttonMapControllersScalePlus.Click += (sender, e) =>
                Config.MapGraphics.ChangeScale(1, Config.MapGui.textBoxMapControllersScaleChange.Text);
            Config.MapGui.buttonMapControllersScaleDivide.Click += (sender, e) =>
                Config.MapGraphics.ChangeScale2(-1, Config.MapGui.textBoxMapControllersScaleChange2.Text);
            Config.MapGui.buttonMapControllersScaleTimes.Click += (sender, e) =>
                Config.MapGraphics.ChangeScale2(1, Config.MapGui.textBoxMapControllersScaleChange2.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.groupBoxMapControllersScale,
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
            Config.MapGui.buttonMapControllersCenterUp.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(0, -1, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterDown.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(0, 1, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterUpLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, -1, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterUpRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, -1, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterDownLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, 1, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterDownRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, 1, Config.MapGui.textBoxMapControllersCenterChange.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.groupBoxMapControllersCenter,
                new List<string>() { "Center on Mario" },
                new List<Action>()
                {
                    () =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        Config.MapGraphics.SetCustomCenter(marioX + ";" + marioZ);
                    }
                });

            // Buttons for Changing Angle
            Config.MapGui.buttonMapControllersAngleCCW.Click += (sender, e) =>
                Config.MapGraphics.ChangeAngle(-1, Config.MapGui.textBoxMapControllersAngleChange.Text);
            Config.MapGui.buttonMapControllersAngleCW.Click += (sender, e) =>
                Config.MapGraphics.ChangeAngle(1, Config.MapGui.textBoxMapControllersAngleChange.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.groupBoxMapControllersAngle,
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
            Config.MapGui.textBoxMapControllersScaleCustom.AddEnterAction(() =>
            {
                Config.MapGui.radioButtonMapControllersScaleCustom.Checked = true;
            });
            Config.MapGui.textBoxMapControllersCenterCustom.AddEnterAction(() =>
            {
                Config.MapGui.radioButtonMapControllersCenterCustom.Checked = true;
            });
            Config.MapGui.textBoxMapControllersAngleCustom.AddEnterAction(() =>
            {
                Config.MapGui.radioButtonMapControllersAngleCustom.Checked = true;
            });

            // Additional Checkboxes
            Config.MapGui.checkBoxMapOptionsEnable3D.Click += (sender, e) =>
            {
                // Make the toBeVisible one visible first in order to avoid flicker.
                (GLControl toBeVisible, GLControl toBeInvisible) =
                    Config.MapGui.checkBoxMapOptionsEnable3D.Checked ?
                        (Config.MapGui.GLControlMap3D, Config.MapGui.GLControlMap2D) :
                        (Config.MapGui.GLControlMap2D, Config.MapGui.GLControlMap3D);
                toBeVisible.Visible = true;
                toBeInvisible.Visible = false;
            };
            Config.MapGui.checkBoxMapOptionsEnablePuView.Click += (sender, e) =>
                Config.MapGraphics.MapViewEnablePuView = Config.MapGui.checkBoxMapOptionsEnablePuView.Checked;
            Config.MapGui.checkBoxMapOptionsScaleIconSizes.Click += (sender, e) =>
                Config.MapGraphics.MapViewScaleIconSizes = Config.MapGui.checkBoxMapOptionsScaleIconSizes.Checked;
            Config.MapGui.checkBoxMapControllersCenterChangeByPixels.Click += (sender, e) =>
                Config.MapGraphics.MapViewCenterChangeByPixels = Config.MapGui.checkBoxMapControllersCenterChangeByPixels.Checked;

            // Global Icon Size
            Config.MapGui.textBoxMapOptionsGlobalIconSize.AddEnterAction(() =>
            {
                float? parsed = ParsingUtilities.ParseFloatNullable(
                    Config.MapGui.textBoxMapOptionsGlobalIconSize.Text);
                if (!parsed.HasValue) return;
                SetGlobalIconSize(parsed.Value);
            });
            Config.MapGui.trackBarMapOptionsGlobalIconSize.AddManualChangeAction(() =>
                SetGlobalIconSize(Config.MapGui.trackBarMapOptionsGlobalIconSize.Value));
            MapUtilities.CreateTrackBarContextMenuStrip(Config.MapGui.trackBarMapOptionsGlobalIconSize);

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
                MapUtilities.MaybeChangeMapCameraMode();
                SpecialConfig.Map3DFOV = Config.MapGui.trackBarMapFov.Value;
                Config.MapGui.textBoxMapFov.Text = Config.MapGui.trackBarMapFov.Value.ToString();
            };

            Config.MapGui.textBoxMapFov.AddEnterAction(() =>
            {
                float parsed = ParsingUtilities.ParseFloat(Config.MapGui.textBoxMapFov.Text);
                if (parsed > 0 && parsed < 180)
                {
                    MapUtilities.MaybeChangeMapCameraMode();
                    SpecialConfig.Map3DFOV = parsed;
                    ControlUtilities.SetTrackBarValueCapped(Config.MapGui.trackBarMapFov, parsed);
                }
            });
        }

        private void SetGlobalIconSize(float size)
        {
            Config.MapGui.flowLayoutPanelMapTrackers.SetGlobalIconSize(size);
            Config.MapGui.textBoxMapOptionsGlobalIconSize.SubmitText(size.ToString());
            Config.MapGui.trackBarMapOptionsGlobalIconSize.StartChangingByCode();
            ControlUtilities.SetTrackBarValueCapped(Config.MapGui.trackBarMapOptionsGlobalIconSize, size);
            Config.MapGui.trackBarMapOptionsGlobalIconSize.StopChangingByCode();
        }

        private void InitializeSemaphores()
        {
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackMario, MapSemaphoreManager.Mario, () => new MapMarioObject(), true);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackHolp, MapSemaphoreManager.Holp, () => new MapHolpObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackCamera, MapSemaphoreManager.Camera, () => new MapCameraObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackGhost, MapSemaphoreManager.Ghost, () => new MapGhostObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackSelf, MapSemaphoreManager.Self, () => new MapSelfObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackPoint, MapSemaphoreManager.Point, () => new MapPointObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackFloorTri, MapSemaphoreManager.FloorTri, () => new MapMarioFloorObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackWallTri, MapSemaphoreManager.WallTri, () => new MapMarioWallObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackCeilingTri, MapSemaphoreManager.CeilingTri, () => new MapMarioCeilingObject(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackUnitGridlines, MapSemaphoreManager.UnitGridlines, () => new MapUnitGridlinesObject(), false);
        }

        private void InitializeCheckboxSemaphore(
            CheckBox checkBox, MapSemaphore semaphore, Func<MapObject> mapObjFunc, bool startAsOn)
        {
            Action<bool> addTrackerAction = (bool withSemaphore) =>
            {
                MapTracker tracker = new MapTracker(
                    new List<MapObject>() { mapObjFunc() },
                    withSemaphore ? new List<MapSemaphore>() { semaphore } : new List<MapSemaphore>());
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
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
            if (Config.MapGui.checkBoxMapOptionsEnable3D.Checked && !_isLoaded3D) return;

            Config.MapGui.flowLayoutPanelMapTrackers.UpdateControl();

            if (!updateView) return;

            base.Update(updateView);
            UpdateBasedOnObjectsSelectedOnMap();
            UpdateControlsBasedOnSemaphores();
            UpdateDataTab();
            UpdateVarColors();

            if (!PauseMapUpdating)
            {
                if (Config.MapGui.checkBoxMapOptionsEnable3D.Checked)
                {
                    Config.MapGui.GLControlMap3D.Invalidate();
                }
                else
                {
                    Config.MapGui.GLControlMap2D.Invalidate();
                }
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

            Config.MapGui.labelMapDataMapName.Text = map.Name;
            Config.MapGui.labelMapDataMapSubName.Text = map.SubName ?? "";
            Config.MapGui.labelMapDataPuCoordinateValues.Text = string.Format("[{0}:{1}:{2}]", puX, puY, puZ);
            Config.MapGui.labelMapDataQpuCoordinateValues.Text = string.Format("[{0}:{1}:{2}]", qpuX, qpuY, qpuZ);
            Config.MapGui.labelMapDataIdValues.Text = string.Format("[{0}:{1}:{2}:{3}]", level, area, loadingPoint, missionLayout);
            Config.MapGui.labelMapDataYNormValue.Text = yNorm?.ToString() ?? "(none)";
        }

        private void UpdateBasedOnObjectsSelectedOnMap()
        {
            // Determine which obj slots have been checked/unchecked since the last update
            List<int> currentObjIndexes = Config.ObjectSlotsManager.SelectedOnMapSlotsAddresses
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
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            }
        }

        private void UpdateControlsBasedOnSemaphores()
        {
            // Update checkboxes when tracker is deleted
            Config.MapGui.checkBoxMapOptionsTrackMario.Checked = MapSemaphoreManager.Mario.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackHolp.Checked = MapSemaphoreManager.Holp.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackCamera.Checked = MapSemaphoreManager.Camera.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackGhost.Checked = MapSemaphoreManager.Ghost.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackSelf.Checked = MapSemaphoreManager.Self.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackPoint.Checked = MapSemaphoreManager.Point.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackFloorTri.Checked = MapSemaphoreManager.FloorTri.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackWallTri.Checked = MapSemaphoreManager.WallTri.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackCeilingTri.Checked = MapSemaphoreManager.CeilingTri.IsUsed;
            Config.MapGui.checkBoxMapOptionsTrackUnitGridlines.Checked = MapSemaphoreManager.UnitGridlines.IsUsed;

            // Update object slots when tracker is deleted
            Config.ObjectSlotsManager.SelectedOnMapSlotsAddresses
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address))
                .FindAll(index => index.HasValue)
                .ConvertAll(index => index.Value)
                .FindAll(index => !MapSemaphoreManager.Objects[index].IsUsed)
                .ConvertAll(index => ObjectUtilities.GetObjectAddress(index))
                .ForEach(address => Config.ObjectSlotsManager.SelectedOnMapSlotsAddresses.Remove(address));
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
            Config.ObjectSlotsManager.SelectedOnMapSlotsAddresses.AddRange(addresses);
            _currentObjIndexes.AddRange(indexes);
            MapTracker tracker = new MapTracker(mapObjs, semaphores);
            Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
        }

        private static readonly List<string> speedVarNames = new List<string>()
        {
            "2D Scroll Speed", "3D Scroll Speed", "3D Translate Speed", "3D Rotate Speed",
        };
        private static readonly List<string> inGameColoredVars = new List<string>() { };
        private static readonly List<string> cameraPosAndFocusColoredVars = new List<string>()
        {
            "Camera X", "Camera Y", "Camera Z", "Focus X", "Focus Y", "Focus Z", "FOV",
        };
        private static readonly List<string> cameraPosAndAngleColoredVars = new List<string>()
        {
            "Camera X", "Camera Y", "Camera Z", "Camera Yaw", "Camera Pitch", "Camera Roll", "FOV",
        };
        private static readonly List<string> followFocusRelativeAngleColoredVars = new List<string>()
        {
            "Focus Pos PA", "Focus Angle PA", "Following Radius", "Following Y Offset", "Following Yaw", "FOV",
        };
        private static readonly List<string> followFocusAbsoluteAngleColoredVars = new List<string>()
        {
            "Focus Pos PA", "Following Radius", "Following Y Offset", "Following Yaw", "FOV",
        };
        private static readonly Dictionary<Map3DCameraMode, List<string>> coloredVarsMap =
            new Dictionary<Map3DCameraMode, List<string>>()
            {
                [Map3DCameraMode.InGame] = inGameColoredVars,
                [Map3DCameraMode.CameraPosAndFocus] = cameraPosAndFocusColoredVars,
                [Map3DCameraMode.CameraPosAndAngle] = cameraPosAndAngleColoredVars,
                [Map3DCameraMode.FollowFocusRelativeAngle] = followFocusRelativeAngleColoredVars,
                [Map3DCameraMode.FollowFocusAbsoluteAngle] = followFocusAbsoluteAngleColoredVars,
            };

        private void UpdateVarColors()
        {
            List<string> coloredVarNames = coloredVarsMap[SpecialConfig.Map3DMode];
            _variablePanel.ColorVarsUsingFunction(
                control =>
                    control.VarName == "Mode" ? ColorUtilities.GetColorFromString("Green") :
                    coloredVarNames.Contains(control.VarName) ? ColorUtilities.GetColorFromString("Red") :
                    speedVarNames.Contains(control.VarName) ? ColorUtilities.GetColorFromString("Grey") :
                    SystemColors.Control);
        }
    }
}
