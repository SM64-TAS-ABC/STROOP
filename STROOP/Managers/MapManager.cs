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
using STROOP.Forms;
using System.Xml.Linq;

namespace STROOP.Managers
{
    public class MapManager : DataManager
    {
        private enum SaveType { MapTrackers, MapTrackersMapTabSettings, MapTrackersMapTabSettingsStroopSettings };

        private Action _checkBoxMarioAction;
        private Action _checkBoxGhostAction;
        private Action _checkBoxFloorAction;
        private Action _checkBoxUnitGridlinesAction;
        private List<int> _currentObjIndexes = new List<int>();

        public bool PauseMapUpdating = false;
        private bool _isLoaded2D = false;
        private bool _isLoaded3D = false;
        public int NumDrawingsEnabled = 0;

        private List<object> _mapLayoutChoices;
        private List<object> _backgroundImageChoices;
        private Dictionary<string, object> _mapDictionary;
        private Dictionary<string, object> _backgroundDictionary;

        public void NotifyDrawingEnabledChange(bool enabled)
        {
            NumDrawingsEnabled += enabled ? +1 : -1;
        }

        public MapManager(string varFilePath)
            : base(varFilePath, Config.MapGui.watchVariablePanelMapVars)
        {
        }

        public void Load2D()
        {
            // Create new graphics control
            Config.MapGraphics = new MapGraphics(true);
            Config.MapGraphics.Load(Config.MapGui.GLControlMap2D);
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
                new MapObjectCurrentMap(), new MapObjectCurrentBackground(), new MapObjectHitboxHackTriangle(true));

            // ComboBox for Level
            List<MapLayout> mapLayouts = Config.MapAssociations.GetAllMaps();
            _mapLayoutChoices = new List<object>() { "Recommended" };
            mapLayouts.ForEach(mapLayout => _mapLayoutChoices.Add(mapLayout));
            Config.MapGui.comboBoxMapOptionsMap.DataSource = _mapLayoutChoices;
            _mapDictionary = new Dictionary<string, object>();
            _mapLayoutChoices.ForEach(map => _mapDictionary[map.ToString()] = map);

            // ComboBox for Background
            List<BackgroundImage> backgroundImages = Config.MapAssociations.GetAllBackgroundImages();
            _backgroundImageChoices = new List<object>() { "Recommended" };
            backgroundImages.ForEach(backgroundImage => _backgroundImageChoices.Add(backgroundImage));
            Config.MapGui.comboBoxMapOptionsBackground.DataSource = _backgroundImageChoices;
            _backgroundDictionary = new Dictionary<string, object>();
            _backgroundImageChoices.ForEach(background => _backgroundDictionary[background.ToString()] = background);

            // Buttons on Options

            ToolStripMenuItem itemMapPopOut = new ToolStripMenuItem("Add Map Pop Out");
            itemMapPopOut.Click += (sender, e) =>
            {
                MapPopOutForm form = new MapPopOutForm();
                form.ShowForm();
            };

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

            ToolStripMenuItem itemAllObjectsWithName = new ToolStripMenuItem("Add Tracker for All Objects with Name");
            itemAllObjectsWithName.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the name of the object.");
                MapObject mapObj = MapObjectAllObjectsWithName.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemLevelFloorTris = new ToolStripMenuItem("Add Tracker for Level Floor Tris");
            itemLevelFloorTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectLevelFloor();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemLevelWallTris = new ToolStripMenuItem("Add Tracker for Level Wall Tris");
            itemLevelWallTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectLevelWall();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemLevelCeilingTris = new ToolStripMenuItem("Add Tracker for Level Ceiling Tris");
            itemLevelCeilingTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectLevelCeiling();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemAllObjectFloorTris = new ToolStripMenuItem("Add Tracker for All Object Floor Tris");
            itemAllObjectFloorTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectAllObjectFloor();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemAllObjectWallTris = new ToolStripMenuItem("Add Tracker for All Object Wall Tris");
            itemAllObjectWallTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectAllObjectWall();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemAllObjectCeilingTris = new ToolStripMenuItem("Add Tracker for All Object Ceiling Tris");
            itemAllObjectCeilingTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectAllObjectCeiling();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomFloorTris = new ToolStripMenuItem("Add Tracker for Custom Floor Tris");
            itemCustomFloorTris.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                MapObject mapObj = MapObjectCustomFloor.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomWallTris = new ToolStripMenuItem("Add Tracker for Custom Wall Tris");
            itemCustomWallTris.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                MapObject mapObj = MapObjectCustomWall.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomCeilingTris = new ToolStripMenuItem("Add Tracker for Custom Ceiling Tris");
            itemCustomCeilingTris.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                MapObject mapObj = MapObjectCustomCeiling.Create(text);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomUnitPoints = new ToolStripMenuItem("Add Tracker for Custom Unit Points");
            itemCustomUnitPoints.Click += (sender, e) =>
            {
                (string, bool)? result = DialogUtilities.GetStringAndSideFromDialog(
                    labelText: "Enter points as pairs or triplets of floats.",
                    button1Text: "Pairs",
                    button2Text: "Triplets");
                if (!result.HasValue) return;
                (string text, bool side) = result.Value;
                MapObject mapObj = MapObjectCustomUnitPoints.Create(text, side);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomCylinderPoints = new ToolStripMenuItem("Add Tracker for Custom Cylinder Points");
            itemCustomCylinderPoints.Click += (sender, e) =>
            {
                (string, bool)? result = DialogUtilities.GetStringAndSideFromDialog(
                    labelText: "Enter points as pairs or triplets of floats.",
                    button1Text: "Pairs",
                    button2Text: "Triplets");
                if (!result.HasValue) return;
                (string text, bool side) = result.Value;
                MapObject mapObj = MapObjectCustomCylinderPoints.Create(text, side);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomSpherePoints = new ToolStripMenuItem("Add Tracker for Custom Sphere Points");
            itemCustomSpherePoints.Click += (sender, e) =>
            {
                (string, bool)? result = DialogUtilities.GetStringAndSideFromDialog(
                    labelText: "Enter points as pairs or triplets of floats.",
                    button1Text: "Pairs",
                    button2Text: "Triplets");
                if (!result.HasValue) return;
                (string text, bool side) = result.Value;
                MapObject mapObj = MapObjectCustomSpherePoints.Create(text, side);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomMap = new ToolStripMenuItem("Add Tracker for Custom Map");
            itemCustomMap.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCustomMap();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomBackground = new ToolStripMenuItem("Add Tracker for Custom Background");
            itemCustomBackground.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCustomBackground();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemUnitGridlines = new ToolStripMenuItem("Add Tracker for Unit Gridlines");
            itemUnitGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectUnitGridlines();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemFloatGridlines = new ToolStripMenuItem("Add Tracker for Float Gridlines");
            itemFloatGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectFloatGridlines();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCellGridlines = new ToolStripMenuItem("Add Tracker for Cell Gridlines");
            itemCellGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCellGridlines();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPuGridlines = new ToolStripMenuItem("Add Tracker for PU Gridlines");
            itemPuGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectPuGridlines();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomGridlines = new ToolStripMenuItem("Add Tracker for Custom Gridlines");
            itemCustomGridlines.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCustomGridlines();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemIwerlipses = new ToolStripMenuItem("Add Tracker for Iwerlipses");
            itemIwerlipses.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectIwerlipses();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemNextPositions = new ToolStripMenuItem("Add Tracker for Next Positions");
            itemNextPositions.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectNextPositions();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPreviousPositions = new ToolStripMenuItem("Add Tracker for Previous Positions");
            itemPreviousPositions.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectPreviousPositions();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCurrentUnit = new ToolStripMenuItem("Add Tracker for Current Unit");
            itemCurrentUnit.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCurrentUnit(PositionAngle.Mario);
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCurrentCell = new ToolStripMenuItem("Add Tracker for Current Cell");
            itemCurrentCell.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCurrentCell();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCUpFloorTris = new ToolStripMenuItem("Add Tracker for C-Up Floor Tris");
            itemCUpFloorTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCUpFloor();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPunchFloorTris = new ToolStripMenuItem("Add Tracker for Punch Floor Tris");
            itemPunchFloorTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectPunchFloor();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPunchDetector = new ToolStripMenuItem("Add Tracker for Punch Detector");
            itemPunchDetector.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectPunchDetector();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemHitboxHackTris = new ToolStripMenuItem("Add Tracker for Hitbox Hack Tris");
            itemHitboxHackTris.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectHitboxHackTriangle(false);
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemWaters = new ToolStripMenuItem("Add Tracker for Waters");
            itemWaters.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectWaters();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemAggregatedPath = new ToolStripMenuItem("Add Tracker for Aggregated Path");
            itemAggregatedPath.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectAggregatedPath();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCompass = new ToolStripMenuItem("Add Tracker for Compass");
            itemCompass.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCompass();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCoordinateLabels = new ToolStripMenuItem("Add Tracker for Coordinate Labels");
            itemCoordinateLabels.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectCoordinateLabels();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemLedgeGrabChecker = new ToolStripMenuItem("Add Tracker for Ledge Grab Checker");
            itemLedgeGrabChecker.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectLedgeGrabChecker();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomPositionAngle = new ToolStripMenuItem("Add Tracker for Custom PositionAngle");
            itemCustomPositionAngle.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter a PositionAngle.");
                PositionAngle posAngle = PositionAngle.FromString(text);
                if (posAngle == null) return;
                MapObject mapObj = new MapObjectCustomPositionAngle(posAngle);
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
                MapObject mapObj = MapObjectLineSegment.Create(text1, text2);
                if (mapObj == null) return;
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemDrawing = new ToolStripMenuItem("Add Tracker for Drawing");
            itemDrawing.Click += (sender, e) =>
            {
                MapObject mapObj = new MapObjectDrawing();
                MapTracker tracker = new MapTracker(mapObj);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip = new ContextMenuStrip();
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemMapPopOut);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemAllObjects);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemMarkedObjects);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemAllObjectsWithName);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLevelFloorTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLevelWallTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLevelCeilingTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemAllObjectFloorTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemAllObjectWallTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemAllObjectCeilingTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomFloorTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomWallTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomCeilingTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomUnitPoints);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomCylinderPoints);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomSpherePoints);
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
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemWaters);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemHitboxHackTris);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemAggregatedPath);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCompass);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCoordinateLabels);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLedgeGrabChecker);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemCustomPositionAngle);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemLineSegment);
            Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Items.Add(itemDrawing);

            Config.MapGui.buttonMapOptionsAddNewTracker.Click += (sender, e) =>
                Config.MapGui.buttonMapOptionsAddNewTracker.ContextMenuStrip.Show(Cursor.Position);

            Config.MapGui.buttonMapOptionsClearAllTrackers.Click += (sender, e) =>
                Config.MapGui.flowLayoutPanelMapTrackers.ClearControls();
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.buttonMapOptionsClearAllTrackers,
                new List<string>()
                {
                    "Reset to Initial State",
                    "Surface Triangles White",
                    "Surface Triangles Black",
                    "Enable TASer Settings",
                },
                new List<Action>()
                {
                    () => ResetToInitialState(),
                    () => DoSurfaceTriangles(true),
                    () => DoSurfaceTriangles(false),
                    () => DoTaserSettings(),
                });

            Config.MapGui.buttonMapOptionsOpen.Click += (sender, e) => Open();

            Config.MapGui.buttonMapOptionsSave.Click += (sender, e) => Save(SaveType.MapTrackers);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.buttonMapOptionsSave,
                new List<string>()
                {
                    "Save [Map Trackers]",
                    "Save [Map Trackers], [Map Tab Settings]",
                    "Save [Map Trackers], [Map Tab Settings], [STROOP Settings]",
                },
                new List<Action>()
                {
                    () => Save(SaveType.MapTrackers),
                    () => Save(SaveType.MapTrackersMapTabSettings),
                    () => Save(SaveType.MapTrackersMapTabSettingsStroopSettings),
                });

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
                Config.MapGraphics.ChangeCenter(0, 1, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterDown.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(0, -1, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, 0, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, 0, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterUpLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, 1, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterUpRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, 1, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterDownLeft.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(-1, -1, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterDownRight.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(1, -1, 0, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterIn.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(0, 0, -1, Config.MapGui.textBoxMapControllersCenterChange.Text);
            Config.MapGui.buttonMapControllersCenterOut.Click += (sender, e) =>
                Config.MapGraphics.ChangeCenter(0, 0, 1, Config.MapGui.textBoxMapControllersCenterChange.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.groupBoxMapControllersCenter,
                new List<string>() { "Center on Mario" },
                new List<Action>()
                {
                    () =>
                    {
                        float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                        float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        Config.MapGraphics.SetCustomCenter(marioX, marioY, marioZ);
                    }
                });
            Config.MapGui.groupBoxMapControllersCenter.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ControlUtilities.AddCheckableContextMenuStripItems(
                Config.MapGui.groupBoxMapControllersCenter,
                new List<string>()
                {
                    "Can Drag Horizontally and Vertically",
                    "Can Only Drag Horizontally",
                    "Can Only Drag Vertically",
                },
                new List<MapGraphics.MapDragAbility>()
                {
                    MapGraphics.MapDragAbility.HorizontalAndVertical,
                    MapGraphics.MapDragAbility.HorizontalOnly,
                    MapGraphics.MapDragAbility.VerticalOnly,
                },
                dragAbility => Config.MapGraphics.MapViewCenterDragAbility = dragAbility,
                Config.MapGraphics.MapViewCenterDragAbility);

            // Buttons for Changing Angle
            Config.MapGui.buttonMapControllersAngleCCW.Click += (sender, e) =>
                Config.MapGraphics.ChangeYaw(-1, Config.MapGui.textBoxMapControllersAngleChange.Text);
            Config.MapGui.buttonMapControllersAngleCW.Click += (sender, e) =>
                Config.MapGraphics.ChangeYaw(1, Config.MapGui.textBoxMapControllersAngleChange.Text);
            Config.MapGui.buttonMapControllersAngleUp.Click += (sender, e) =>
                Config.MapGraphics.ChangePitch(-1, Config.MapGui.textBoxMapControllersAngleChange.Text);
            Config.MapGui.buttonMapControllersAngleDown.Click += (sender, e) =>
                Config.MapGraphics.ChangePitch(1, Config.MapGui.textBoxMapControllersAngleChange.Text);
            ControlUtilities.AddContextMenuStripFunctions(
                Config.MapGui.groupBoxMapControllersAngle,
                new List<string>()
                {
                    "Use Mario Yaw",
                    "Use Camera Yaw",
                    "Use Centripetal Yaw",
                    "Use 0 Pitch",
                },
                new List<Action>()
                {
                    () =>
                    {
                        ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        Config.MapGraphics.SetCustomYaw(marioAngle);
                    },
                    () =>
                    {
                        ushort cameraAngle = Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                        Config.MapGraphics.SetCustomYaw(cameraAngle);
                    },
                    () =>
                    {
                        ushort centripetalAngle = Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset);
                        double centripetalAngleReversed = MoreMath.ReverseAngle(centripetalAngle);
                        Config.MapGraphics.SetCustomYaw(centripetalAngleReversed);
                    },
                    () =>
                    {
                        Config.MapGraphics.SetCustomPitch(0);
                    },
                });
            Config.MapGui.groupBoxMapControllersAngle.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ControlUtilities.AddCheckableContextMenuStripItems(
                Config.MapGui.groupBoxMapControllersAngle,
                new List<string>()
                {
                    "Can Drag Horizontally and Vertically",
                    "Can Only Drag Horizontally",
                    "Can Only Drag Vertically",
                },
                new List<MapGraphics.MapDragAbility>()
                {
                    MapGraphics.MapDragAbility.HorizontalAndVertical,
                    MapGraphics.MapDragAbility.HorizontalOnly,
                    MapGraphics.MapDragAbility.VerticalOnly,
                },
                dragAbility => Config.MapGraphics.MapViewYawDragAbility = dragAbility,
                Config.MapGraphics.MapViewYawDragAbility);

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
            Config.MapGui.checkBoxMapOptionsEnable3D.Click +=
                (sender, e) => SetEnable3D(Config.MapGui.checkBoxMapOptionsEnable3D.Checked);

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

        private void SetEnable3D(bool enable3D)
        {
            Config.MapGui.checkBoxMapOptionsEnable3D.Checked = enable3D;
            // Make the toBeVisible one visible first in order to avoid flicker.
            (GLControl toBeVisible, GLControl toBeInvisible) = enable3D ?
                (Config.MapGui.GLControlMap3D, Config.MapGui.GLControlMap2D) :
                (Config.MapGui.GLControlMap2D, Config.MapGui.GLControlMap3D);
            toBeVisible.Visible = true;
            toBeInvisible.Visible = false;
        }

        private void Save(SaveType saveType)
        {
            XDocument document = new XDocument();
            XElement root = new XElement(XName.Get("MapData"));
            document.Add(root);

            if (saveType == SaveType.MapTrackersMapTabSettingsStroopSettings)
            {
                XElement stroopSettings = new XElement(XName.Get("StroopSettings"));

                stroopSettings.Add(new XAttribute("stroopMainFormWidth", Config.StroopMainForm.Width));
                stroopSettings.Add(new XAttribute("stroopMainFormHeight", Config.StroopMainForm.Height));
                stroopSettings.Add(new XAttribute("stroopMainFormLocationX", Config.StroopMainForm.Location.X));
                stroopSettings.Add(new XAttribute("stroopMainFormLocationY", Config.StroopMainForm.Location.Y));

                stroopSettings.Add(new XAttribute("splitContainerMainPanel1Collapsed", Config.SplitContainerMain.Panel1Collapsed));
                stroopSettings.Add(new XAttribute("splitContainerMainPanel2Collapsed", Config.SplitContainerMain.Panel2Collapsed));
                stroopSettings.Add(new XAttribute("splitContainerMainSplitterDistance", Config.SplitContainerMain.SplitterDistance));

                stroopSettings.Add(new XAttribute("splitContainerMapPanel1Collapsed", Config.MapGui.splitContainerMap.Panel1Collapsed));
                stroopSettings.Add(new XAttribute("splitContainerMapPanel2Collapsed", Config.MapGui.splitContainerMap.Panel2Collapsed));
                stroopSettings.Add(new XAttribute("splitContainerMapSplitterDistance", Config.MapGui.splitContainerMap.SplitterDistance));

                root.Add(stroopSettings);
            }

            if (saveType == SaveType.MapTrackersMapTabSettings ||
                saveType == SaveType.MapTrackersMapTabSettingsStroopSettings)
            {
                XElement mapTabSettings = new XElement(XName.Get("MapTabSettings"));

                mapTabSettings.Add(new XAttribute("enable3D", Config.MapGui.checkBoxMapOptionsEnable3D.Checked));
                mapTabSettings.Add(new XAttribute("disableHitboxHackTris", Config.MapGui.checkBoxMapOptionsDisableHitboxHackTris.Checked));
                mapTabSettings.Add(new XAttribute("enableOrthographicView", Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked));
                mapTabSettings.Add(new XAttribute("enableCrossSection", Config.MapGui.checkBoxMapOptionsEnableCrossSection.Checked));
                mapTabSettings.Add(new XAttribute("enablePuView", Config.MapGui.checkBoxMapOptionsEnablePuView.Checked));
                mapTabSettings.Add(new XAttribute("reverseDragging", Config.MapGui.checkBoxMapOptionsReverseDragging.Checked));
                mapTabSettings.Add(new XAttribute("map", Config.MapGui.comboBoxMapOptionsMap.SelectedItem));
                mapTabSettings.Add(new XAttribute("background", Config.MapGui.comboBoxMapOptionsBackground.SelectedItem));

                mapTabSettings.Add(new XAttribute("mapViewScale", Config.MapGraphics.MapViewScale));
                if (Config.MapGraphics.MapViewScale == MapGraphics.MapScale.Custom)
                {
                    mapTabSettings.Add(new XAttribute("mapViewScaleValue", (double)Config.MapGraphics.MapViewScaleValue));
                }
                mapTabSettings.Add(new XAttribute("mapViewCenter", Config.MapGraphics.MapViewCenter));
                if (Config.MapGraphics.MapViewCenter == MapGraphics.MapCenter.Custom)
                {
                    mapTabSettings.Add(new XAttribute("mapViewCenterXValue", (double)Config.MapGraphics.MapViewCenterXValue));
                    mapTabSettings.Add(new XAttribute("mapViewCenterYValue", (double)Config.MapGraphics.MapViewCenterYValue));
                    mapTabSettings.Add(new XAttribute("mapViewCenterZValue", (double)Config.MapGraphics.MapViewCenterZValue));
                }
                mapTabSettings.Add(new XAttribute("changeByPixels", Config.MapGui.checkBoxMapControllersCenterChangeByPixels.Checked));
                mapTabSettings.Add(new XAttribute("useMarioDepth", Config.MapGui.checkBoxMapControllersCenterUseMarioDepth.Checked));
                mapTabSettings.Add(new XAttribute("mapViewYaw", Config.MapGraphics.MapViewYaw));
                if (Config.MapGraphics.MapViewYaw == MapGraphics.MapYaw.Custom)
                {
                    mapTabSettings.Add(new XAttribute("mapViewYawValue", (double)Config.MapGraphics.MapViewYawValue));
                }
                mapTabSettings.Add(new XAttribute("mapViewPitchValue", (double)Config.MapGraphics.MapViewPitchValue));

                mapTabSettings.Add(new XAttribute("map3DMode", SpecialConfig.Map3DMode));
                mapTabSettings.Add(new XAttribute("map3DCameraX", (double)SpecialConfig.Map3DCameraX));
                mapTabSettings.Add(new XAttribute("map3DCameraY", (double)SpecialConfig.Map3DCameraY));
                mapTabSettings.Add(new XAttribute("map3DCameraZ", (double)SpecialConfig.Map3DCameraZ));
                mapTabSettings.Add(new XAttribute("map3DCameraYaw", (double)SpecialConfig.Map3DCameraYaw));
                mapTabSettings.Add(new XAttribute("map3DCameraPitch", (double)SpecialConfig.Map3DCameraPitch));
                mapTabSettings.Add(new XAttribute("map3DCameraRoll", (double)SpecialConfig.Map3DCameraRoll));
                mapTabSettings.Add(new XAttribute("map3DFocusX", (double)SpecialConfig.Map3DFocusX));
                mapTabSettings.Add(new XAttribute("map3DFocusY", (double)SpecialConfig.Map3DFocusY));
                mapTabSettings.Add(new XAttribute("map3DFocusZ", (double)SpecialConfig.Map3DFocusZ));
                mapTabSettings.Add(new XAttribute("map3DCameraPosPA", SpecialConfig.Map3DCameraPosPA));
                mapTabSettings.Add(new XAttribute("map3DCameraAnglePA", SpecialConfig.Map3DCameraAnglePA));
                mapTabSettings.Add(new XAttribute("map3DFocusPosPA", SpecialConfig.Map3DFocusPosPA));
                mapTabSettings.Add(new XAttribute("map3DFocusAnglePA", SpecialConfig.Map3DFocusAnglePA));
                mapTabSettings.Add(new XAttribute("map3DFollowingRadius", (double)SpecialConfig.Map3DFollowingRadius));
                mapTabSettings.Add(new XAttribute("map3DFollowingYOffset", (double)SpecialConfig.Map3DFollowingYOffset));
                mapTabSettings.Add(new XAttribute("map3DFollowingYaw", (double)SpecialConfig.Map3DFollowingYaw));
                mapTabSettings.Add(new XAttribute("map3DFOV", (double)SpecialConfig.Map3DFOV));
                mapTabSettings.Add(new XAttribute("map2DScrollSpeed", SpecialConfig.Map2DScrollSpeed));
                mapTabSettings.Add(new XAttribute("map2DOrthographicHorizontalRotateSpeed", SpecialConfig.Map2DOrthographicHorizontalRotateSpeed));
                mapTabSettings.Add(new XAttribute("map2DOrthographicVerticalRotateSpeed", SpecialConfig.Map2DOrthographicVerticalRotateSpeed));
                mapTabSettings.Add(new XAttribute("map3DScrollSpeed", SpecialConfig.Map3DScrollSpeed));
                mapTabSettings.Add(new XAttribute("map3DTranslateSpeed", SpecialConfig.Map3DTranslateSpeed));
                mapTabSettings.Add(new XAttribute("map3DRotateSpeed", SpecialConfig.Map3DRotateSpeed));
                mapTabSettings.Add(new XAttribute("mapCircleNumPoints2D", SpecialConfig.MapCircleNumPoints2D));
                mapTabSettings.Add(new XAttribute("mapCircleNumPoints3D", SpecialConfig.MapCircleNumPoints3D));
                mapTabSettings.Add(new XAttribute("mapUnitPrecisionThreshold", SpecialConfig.MapUnitPrecisionThreshold));

                root.Add(mapTabSettings);
            }

            Config.MapGui.flowLayoutPanelMapTrackers.ToXElements().ForEach(el => root.Add(el));

            DialogUtilities.SaveXmlDocument(FileType.StroopMapData, document);
        }

        private void Open()
        {
            XDocument document = DialogUtilities.OpenDocument(FileType.StroopMapData);
            if (document == null) return;
            XElement root = document.Root;
            List<XElement> xElements = root.Elements().ToList();

            XElement stroopSettings = xElements.Find(el => el.Name == "StroopSettings");
            if (stroopSettings != null)
            {
                Config.StroopMainForm.Width = ParsingUtilities.ParseInt(stroopSettings.Attribute(XName.Get("stroopMainFormWidth")).Value);
                Config.StroopMainForm.Height = ParsingUtilities.ParseInt(stroopSettings.Attribute(XName.Get("stroopMainFormHeight")).Value);
                Config.StroopMainForm.Location = new Point(
                    ParsingUtilities.ParseInt(stroopSettings.Attribute(XName.Get("stroopMainFormLocationX")).Value),
                    ParsingUtilities.ParseInt(stroopSettings.Attribute(XName.Get("stroopMainFormLocationY")).Value));

                Config.SplitContainerMain.Panel1Collapsed = ParsingUtilities.ParseBool(stroopSettings.Attribute(XName.Get("splitContainerMainPanel1Collapsed")).Value);
                Config.SplitContainerMain.Panel2Collapsed = ParsingUtilities.ParseBool(stroopSettings.Attribute(XName.Get("splitContainerMainPanel2Collapsed")).Value);
                Config.SplitContainerMain.SplitterDistance = ParsingUtilities.ParseInt(stroopSettings.Attribute(XName.Get("splitContainerMainSplitterDistance")).Value);

                Config.MapGui.splitContainerMap.Panel1Collapsed = ParsingUtilities.ParseBool(stroopSettings.Attribute(XName.Get("splitContainerMapPanel1Collapsed")).Value);
                Config.MapGui.splitContainerMap.Panel2Collapsed = ParsingUtilities.ParseBool(stroopSettings.Attribute(XName.Get("splitContainerMapPanel2Collapsed")).Value);
                Config.MapGui.splitContainerMap.SplitterDistance = ParsingUtilities.ParseInt(stroopSettings.Attribute(XName.Get("splitContainerMapSplitterDistance")).Value);
            }

            XElement mapTabSettings = xElements.Find(el => el.Name == "MapTabSettings");
            if (mapTabSettings != null)
            {
                SetEnable3D(ParsingUtilities.ParseBool(mapTabSettings.Attribute(XName.Get("enable3D")).Value));
                Config.MapGui.checkBoxMapOptionsDisableHitboxHackTris.Checked = ParsingUtilities.ParseBool(mapTabSettings.Attribute(XName.Get("disableHitboxHackTris")).Value);
                Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked = ParsingUtilities.ParseBool(mapTabSettings.Attribute(XName.Get("enableOrthographicView")).Value);
                Config.MapGui.checkBoxMapOptionsEnableCrossSection.Checked = ParsingUtilities.ParseBool(mapTabSettings.Attribute(XName.Get("enableCrossSection")).Value);
                Config.MapGui.checkBoxMapOptionsEnablePuView.Checked = ParsingUtilities.ParseBool(mapTabSettings.Attribute(XName.Get("enablePuView")).Value);
                Config.MapGui.checkBoxMapOptionsReverseDragging.Checked = ParsingUtilities.ParseBool(mapTabSettings.Attribute(XName.Get("reverseDragging")).Value);
                Config.MapGui.comboBoxMapOptionsMap.SelectedItem = _mapDictionary[mapTabSettings.Attribute(XName.Get("map")).Value];
                Config.MapGui.comboBoxMapOptionsBackground.SelectedItem = _backgroundDictionary[mapTabSettings.Attribute(XName.Get("background")).Value];

                Config.MapGraphics.SetScale((MapGraphics.MapScale)Enum.Parse(typeof(MapGraphics.MapScale), mapTabSettings.Attribute(XName.Get("mapViewScale")).Value));
                if (Config.MapGraphics.MapViewScale == MapGraphics.MapScale.Custom)
                {
                    Config.MapGraphics.SetCustomScale(mapTabSettings.Attribute(XName.Get("mapViewScaleValue")).Value);
                }
                Config.MapGraphics.SetCenter((MapGraphics.MapCenter)Enum.Parse(typeof(MapGraphics.MapCenter), mapTabSettings.Attribute(XName.Get("mapViewCenter")).Value));
                if (Config.MapGraphics.MapViewCenter == MapGraphics.MapCenter.Custom)
                {
                    Config.MapGraphics.SetCustomCenter(
                        mapTabSettings.Attribute(XName.Get("mapViewCenterXValue")).Value,
                        mapTabSettings.Attribute(XName.Get("mapViewCenterYValue")).Value,
                        mapTabSettings.Attribute(XName.Get("mapViewCenterZValue")).Value);
                }
                Config.MapGui.checkBoxMapControllersCenterChangeByPixels.Checked = ParsingUtilities.ParseBool(mapTabSettings.Attribute(XName.Get("changeByPixels")).Value);
                Config.MapGui.checkBoxMapControllersCenterUseMarioDepth.Checked = ParsingUtilities.ParseBool(mapTabSettings.Attribute(XName.Get("useMarioDepth")).Value);
                Config.MapGraphics.SetYaw((MapGraphics.MapYaw)Enum.Parse(typeof(MapGraphics.MapYaw), mapTabSettings.Attribute(XName.Get("mapViewYaw")).Value));
                if (Config.MapGraphics.MapViewYaw == MapGraphics.MapYaw.Custom)
                {
                    Config.MapGraphics.SetCustomAngle(
                        mapTabSettings.Attribute(XName.Get("mapViewYawValue")).Value,
                        mapTabSettings.Attribute(XName.Get("mapViewPitchValue")).Value);
                }
                else
                {
                    Config.MapGraphics.MapViewPitchValue = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("mapViewPitchValue")).Value);
                }

                SpecialConfig.Map3DMode = (Map3DCameraMode)Enum.Parse(typeof(Map3DCameraMode), mapTabSettings.Attribute(XName.Get("map3DMode")).Value);
                SpecialConfig.Map3DCameraX = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DCameraX")).Value);
                SpecialConfig.Map3DCameraY = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DCameraY")).Value);
                SpecialConfig.Map3DCameraZ = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DCameraZ")).Value);
                SpecialConfig.Map3DCameraYaw = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DCameraYaw")).Value);
                SpecialConfig.Map3DCameraPitch = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DCameraPitch")).Value);
                SpecialConfig.Map3DCameraRoll = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DCameraRoll")).Value);
                SpecialConfig.Map3DFocusX = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DFocusX")).Value);
                SpecialConfig.Map3DFocusY = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DFocusY")).Value);
                SpecialConfig.Map3DFocusZ = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DFocusZ")).Value);
                SpecialConfig.Map3DCameraPosPA = PositionAngle.FromString(mapTabSettings.Attribute(XName.Get("map3DCameraPosPA")).Value);
                SpecialConfig.Map3DCameraAnglePA = PositionAngle.FromString(mapTabSettings.Attribute(XName.Get("map3DCameraAnglePA")).Value);
                SpecialConfig.Map3DFocusPosPA = PositionAngle.FromString(mapTabSettings.Attribute(XName.Get("map3DFocusPosPA")).Value);
                SpecialConfig.Map3DFocusAnglePA = PositionAngle.FromString(mapTabSettings.Attribute(XName.Get("map3DFocusAnglePA")).Value);
                SpecialConfig.Map3DFollowingRadius = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DFollowingRadius")).Value);
                SpecialConfig.Map3DFollowingYOffset = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DFollowingYOffset")).Value);
                SpecialConfig.Map3DFollowingYaw = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DFollowingYaw")).Value);
                SpecialConfig.Map3DFOV = ParsingUtilities.ParseFloat(mapTabSettings.Attribute(XName.Get("map3DFOV")).Value);
                SpecialConfig.Map2DScrollSpeed = ParsingUtilities.ParseDouble(mapTabSettings.Attribute(XName.Get("map2DScrollSpeed")).Value);
                SpecialConfig.Map2DOrthographicHorizontalRotateSpeed = ParsingUtilities.ParseDouble(mapTabSettings.Attribute(XName.Get("map2DOrthographicHorizontalRotateSpeed")).Value);
                SpecialConfig.Map2DOrthographicVerticalRotateSpeed = ParsingUtilities.ParseDouble(mapTabSettings.Attribute(XName.Get("map2DOrthographicVerticalRotateSpeed")).Value);
                SpecialConfig.Map3DScrollSpeed = ParsingUtilities.ParseDouble(mapTabSettings.Attribute(XName.Get("map3DScrollSpeed")).Value);
                SpecialConfig.Map3DTranslateSpeed = ParsingUtilities.ParseDouble(mapTabSettings.Attribute(XName.Get("map3DTranslateSpeed")).Value);
                SpecialConfig.Map3DRotateSpeed = ParsingUtilities.ParseDouble(mapTabSettings.Attribute(XName.Get("map3DRotateSpeed")).Value);
                SpecialConfig.MapCircleNumPoints2D = ParsingUtilities.ParseInt(mapTabSettings.Attribute(XName.Get("mapCircleNumPoints2D")).Value);
                SpecialConfig.MapCircleNumPoints3D = ParsingUtilities.ParseInt(mapTabSettings.Attribute(XName.Get("mapCircleNumPoints3D")).Value);
                SpecialConfig.MapUnitPrecisionThreshold = ParsingUtilities.ParseDouble(mapTabSettings.Attribute(XName.Get("mapUnitPrecisionThreshold")).Value);
            }

            xElements
                .FindAll(xElement => xElement.Name == "MapTracker")
                .ConvertAll(xElement => MapTracker.FromXElement(xElement))
                .ForEach(mapTracker => Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(mapTracker));
        }

        private void ResetToInitialState()
        {
            Config.MapGui.flowLayoutPanelMapTrackers.ClearControls();
            _checkBoxMarioAction();
            Config.MapGui.comboBoxMapOptionsMap.SelectedItem = "Recommended";
            Config.MapGui.comboBoxMapOptionsBackground.SelectedItem = "Recommended";
            Config.MapGui.radioButtonMapControllersScaleCourseDefault.Checked = true;
            Config.MapGui.radioButtonMapControllersCenterBestFit.Checked = true;
            Config.MapGui.radioButtonMapControllersAngle32768.Checked = true;
            Config.MapGraphics.MapViewPitchValue = 0;
            SpecialConfig.Map3DMode = Map3DCameraMode.InGame;
        }

        private void DoSurfaceTriangles(bool useWhiteBackground)
        {
            string backgroundName = useWhiteBackground ? "White Background" : "Black Background";
            object background = _backgroundImageChoices.Find(obj => obj.ToString() == backgroundName);
            Config.MapGui.comboBoxMapOptionsBackground.SelectedItem = background;

            object map = _mapLayoutChoices.Find(obj => obj.ToString() == "Transparent");
            Config.MapGui.comboBoxMapOptionsMap.SelectedItem = map;

            MapTracker wallTracker = new MapTracker(new MapObjectLevelWall());
            wallTracker.SetOrderType(MapTrackerOrderType.OrderOnBottom);
            Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(wallTracker);

            MapTracker floorTracker = new MapTracker(new MapObjectLevelFloor());
            floorTracker.SetOrderType(MapTrackerOrderType.OrderOnBottom);
            Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(floorTracker);

            _checkBoxUnitGridlinesAction();
        }

        public void DoTaserSettings()
        {
            Config.MapGui.flowLayoutPanelMapTrackers.ClearControls();

            _checkBoxMarioAction();
            MapTracker marioTracker = Config.MapGui.flowLayoutPanelMapTrackers.GetTrackerAtIndex(0);
            marioTracker.SetSize(15);

            _checkBoxGhostAction();
            MapTracker ghostTracker = Config.MapGui.flowLayoutPanelMapTrackers.GetTrackerAtIndex(1);
            ghostTracker.SetSize(15);

            MapTracker previousPositionsTracker = new MapTracker(new MapObjectPreviousPositions());
            Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(previousPositionsTracker);
            previousPositionsTracker.SetSize(10);

            _checkBoxUnitGridlinesAction();
            MapTracker unitGridlinesTracker = Config.MapGui.flowLayoutPanelMapTrackers.GetTrackerAtIndex(3);
            unitGridlinesTracker.SetOrderType(MapTrackerOrderType.OrderOnBottom);

            _checkBoxFloorAction();
            MapTracker floorTracker = Config.MapGui.flowLayoutPanelMapTrackers.GetTrackerAtIndex(4);
            floorTracker.SetOpacity(8);
            floorTracker.SetOrderType(MapTrackerOrderType.OrderOnBottom);

            MapObjectLevelWall mapLevelWallObject = new MapObjectLevelWall();
            MapObjectSettings settings = new MapObjectSettings(changeWallRelativeHeight: true, newWallRelativeHeight: -30);
            mapLevelWallObject.ApplySettings(settings);
            MapTracker levelWallTracker = new MapTracker(mapLevelWallObject);
            Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(levelWallTracker);
            levelWallTracker.SetOrderType(MapTrackerOrderType.OrderOnBottom);
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
            _checkBoxMarioAction = InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackMario, MapSemaphoreManager.Mario, () => new MapObjectMario(), true);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackHolp, MapSemaphoreManager.Holp, () => new MapObjectHolp(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackCamera, MapSemaphoreManager.Camera, () => new MapObjectCamera(), false);
            _checkBoxGhostAction = InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackGhost, MapSemaphoreManager.Ghost, () => new MapObjectGhost(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackSelf, MapSemaphoreManager.Self, () => new MapObjectSelf(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackPoint, MapSemaphoreManager.Point, () => new MapObjectPoint(), false);
            _checkBoxFloorAction = InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackFloorTri, MapSemaphoreManager.FloorTri, () => new MapObjectMarioFloor(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackWallTri, MapSemaphoreManager.WallTri, () => new MapObjectMarioWall(), false);
            InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackCeilingTri, MapSemaphoreManager.CeilingTri, () => new MapObjectMarioCeiling(), false);
            _checkBoxUnitGridlinesAction = InitializeCheckboxSemaphore(Config.MapGui.checkBoxMapOptionsTrackUnitGridlines, MapSemaphoreManager.UnitGridlines, () => new MapObjectUnitGridlines(), false);
        }

        private Action InitializeCheckboxSemaphore(
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
            return clickAction;
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
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);

            int puX = PuUtilities.GetPuIndex(marioX);
            int puY = PuUtilities.GetPuIndex(marioY);
            int puZ = PuUtilities.GetPuIndex(marioZ);

            double qpuX = puX / 4.0;
            double qpuY = puY / 4.0;
            double qpuZ = puZ / 4.0;

            uint floorTriangleAddress = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            float? yNorm = floorTriangleAddress == 0 ? (float?)null : Config.Stream.GetFloat(floorTriangleAddress + TriangleOffsetsConfig.NormY);

            byte level = Config.Stream.GetByte(MiscConfig.WarpDestinationAddress + MiscConfig.LevelOffset);
            byte area = Config.Stream.GetByte(MiscConfig.WarpDestinationAddress + MiscConfig.AreaOffset);
            ushort loadingPoint = Config.Stream.GetUShort(MiscConfig.LoadingPointAddress);
            ushort missionLayout = Config.Stream.GetUShort(MiscConfig.MissionAddress);

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
                MapObject mapObj = new MapObjectObject(PositionAngle.Obj(address));
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
                .ConvertAll(address => new MapObjectObject(PositionAngle.Obj(address)) as MapObject);
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

        private static readonly List<string> greyVarNames = new List<string>()
        {
            "2D Scroll Speed",
            "Orth H Rotate Speed", "Orth V Rotate Speed",
            "3D Scroll Speed", "3D Translate Speed", "3D Rotate Speed",
            "Num Circle Points", "Num Sphere Points",
            "Unit Precision Threshold",
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
                    greyVarNames.Contains(control.VarName) ? ColorUtilities.GetColorFromString("Grey") :
                    SystemColors.Control);
        }
    }
}
