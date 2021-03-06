﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Structs;
using STROOP.Utilities;
using System.Xml.Linq;
using STROOP.Structs.Configurations;
using System.Drawing.Drawing2D;
using STROOP.Interfaces;
using STROOP.Models;

namespace STROOP.Map
{
    public partial class MapTracker : UserControl
    {
        private static readonly Image ImageEyeOpen = Properties.Resources.image_eye_open2;
        private static readonly Image ImageEyeClosed = Properties.Resources.image_eye_closed2;

        private readonly List<MapObject> _mapObjectList;
        private readonly List<MapSemaphore> _semaphoreList;

        private MapTrackerIconType _iconType;
        private List<Image> _images;

        private ToolStripMenuItem _itemUseTopDownImage = new ToolStripMenuItem("Use Top Down Image");
        private ToolStripMenuItem _itemUseObjectSlotImage = new ToolStripMenuItem("Use Object Slot Image");
        private ToolStripMenuItem _itemUseCustomImage = new ToolStripMenuItem("Use Custom Image");

        private bool _isVisible;
        private MapTrackerVisibilityType _currentVisiblityType;

        private string _customName;

        public MapTracker(MapObject mapObj, List<MapSemaphore> semaphoreList = null)
            : this(new List<MapObject>() { mapObj }, semaphoreList)
        {
        }

        public MapTracker(
            List<MapObject> mapObjectList,
            List<MapSemaphore> semaphoreList = null)
        {
            if (mapObjectList.Count < 1) throw new ArgumentOutOfRangeException();
            semaphoreList = semaphoreList ?? new List<MapSemaphore>();

            InitializeComponent();

            _mapObjectList = new List<MapObject>(mapObjectList);
            _semaphoreList = new List<MapSemaphore>(semaphoreList);

            _iconType = MapTrackerIconType.TopDownImage;
            _images = new List<Image>();

            _isVisible = true;
            _currentVisiblityType = MapTrackerVisibilityType.VisibleWhenLoaded;

            pictureBoxPicture.ContextMenuStrip = new ContextMenuStrip();
            pictureBoxPicture.ContextMenuStrip.Items.Add(_itemUseTopDownImage);
            pictureBoxPicture.ContextMenuStrip.Items.Add(_itemUseObjectSlotImage);
            pictureBoxPicture.ContextMenuStrip.Items.Add(_itemUseCustomImage);

            _itemUseTopDownImage.Click += (sender, e) => SetIconTypeTopDownImage();
            _itemUseObjectSlotImage.Click += (sender, e) => SetIconTypeObjectSlotImage();
            _itemUseCustomImage.Click += (sender, e) => SetIconTypeCustomImage();
            _itemUseTopDownImage.Checked = true;

            _customName = null;
            textBoxName.AddEnterAction(() => _customName = textBoxName.Text);
            textBoxName.AddLostFocusAction(() => _customName = textBoxName.Text);
            textBoxName.AddDoubleClickAction(() => textBoxName.SelectAll());
            textBoxName.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem itemResetCustomName = new ToolStripMenuItem("Reset Custom Name");
            itemResetCustomName.Click += (sender, e) => _customName = null;
            textBoxName.ContextMenuStrip.Items.Add(itemResetCustomName);

            checkBoxRotates.Click += (sender, e) => SetCustomRotates(checkBoxRotates.Checked);
            checkBoxRotates.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem itemResetCustomRotates = new ToolStripMenuItem("Reset Custom Rotates");
            itemResetCustomRotates.Click += (sender, e) => SetCustomRotates(null);
            checkBoxRotates.ContextMenuStrip.Items.Add(itemResetCustomRotates);

            tableLayoutPanel.BorderWidth = 2;
            tableLayoutPanel.ShowBorder = true;

            comboBoxVisibilityType.DataSource = Enum.GetValues(typeof(MapTrackerVisibilityType));
            comboBoxVisibilityType.SelectedItem = MapTrackerVisibilityType.VisibleWhenLoaded;

            comboBoxOrderType.DataSource = Enum.GetValues(typeof(MapTrackerOrderType));
            comboBoxOrderType.SelectedItem = MapTrackerOrderType.OrderByY;

            SetSize(null);
            SetOpacity(null);
            SetLineWidth(null);
            SetColor(null);
            SetLineColor(null);
            SetScales(null);

            textBoxSize.AddEnterAction(() => textBoxSize_EnterAction());
            trackBarSize.AddManualChangeAction(() => trackBarSize_ValueChanged());
            textBoxOpacity.AddEnterAction(() => textBoxOpacity_EnterAction());
            trackBarOpacity.AddManualChangeAction(() => trackBarOpacity_ValueChanged());
            textBoxLineWidth.AddEnterAction(() => textBoxLineWidth_EnterAction());
            trackBarLineWidth.AddManualChangeAction(() => trackBarLineWidth_ValueChanged());
            colorSelector.AddColorChangeAction((Color color) => SetColor(color));
            colorSelectorLine.AddColorChangeAction((Color color) => SetLineColor(color));
            checkBoxScales.Click += (sender, e) => SetScales(checkBoxScales.Checked);
            _mapObjectList.ForEach(mapObj => mapObj.GetContextMenuStrip()); // avoids null pointer exceptions
            pictureBoxCog.ContextMenuStrip = _mapObjectList[0].GetContextMenuStrip();
            pictureBoxCog.Click += (sender, e) => pictureBoxCog.ContextMenuStrip.Show(Cursor.Position);

            MapUtilities.CreateTrackBarContextMenuStrip(trackBarSize);
            MapUtilities.CreateTrackBarContextMenuStrip(trackBarLineWidth);
            InitializePlusContextMenuStrip();

            UpdateControl();
        }

        private void InitializePlusContextMenuStrip()
        {
            ToolStripMenuItem itemHitboxCylinder = new ToolStripMenuItem("Add Tracker for Hitbox Cylinder");
            itemHitboxCylinder.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectHitboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemEffectiveHitboxCylinder = new ToolStripMenuItem("Add Tracker for Effective Hitbox Cylinder");
            itemEffectiveHitboxCylinder.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectEffectiveHitboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemHurtboxCylinder = new ToolStripMenuItem("Add Tracker for Hurtbox Cylinder");
            itemHurtboxCylinder.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectHurtboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemEffectiveHurtboxCylinder = new ToolStripMenuItem("Add Tracker for Effective Hurtbox Cylinder");
            itemEffectiveHurtboxCylinder.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectEffectiveHurtboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPushHitboxCylinder = new ToolStripMenuItem("Add Tracker for Push Hitbox Cylinder");
            itemPushHitboxCylinder.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectPushHitboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemTangibilitySphere = new ToolStripMenuItem("Add Tracker for Tangibility Sphere");
            itemTangibilitySphere.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectTangibilitySphere(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemDrawDistanceSphere = new ToolStripMenuItem("Add Tracker for Draw Distance Sphere");
            itemDrawDistanceSphere.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectDrawDistanceSphere(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomCylinder = new ToolStripMenuItem("Add Tracker for Custom Cylinder");
            itemCustomCylinder.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectCustomCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomSphere = new ToolStripMenuItem("Add Tracker for Custom Sphere");
            itemCustomSphere.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectCustomSphere(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemHome = new ToolStripMenuItem("Add Tracker for Home");
            itemHome.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectHome(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomCylinderForHome = new ToolStripMenuItem("Add Tracker for Custom Cylinder for Home");
            itemCustomCylinderForHome.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (MapObject)new MapObjectCustomCylinder(homePosAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomSphereForHome = new ToolStripMenuItem("Add Tracker for Custom Sphere for Home");
            itemCustomSphereForHome.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (MapObject)new MapObjectCustomSphere(homePosAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemFloorTriangles = new ToolStripMenuItem("Add Tracker for Floor Triangles");
            itemFloorTriangles.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectFloor(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemWallTriangles = new ToolStripMenuItem("Add Tracker for Wall Triangles");
            itemWallTriangles.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectWall(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCeilingTriangles = new ToolStripMenuItem("Add Tracker for Ceiling Triangles");
            itemCeilingTriangles.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectCeiling(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemMarioFacingArrow = new ToolStripMenuItem("Add Tracker for Mario Facing Arrow");
            itemMarioFacingArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioFacingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemMarioMovingArrow = new ToolStripMenuItem("Add Tracker for Mario Moving Arrow");
            itemMarioMovingArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioMovingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemMarioIntendedArrow = new ToolStripMenuItem("Add Tracker for Mario Intended Arrow");
            itemMarioIntendedArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioIntendedArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemMarioSlidingArrow = new ToolStripMenuItem("Add Tracker for Mario Sliding Arrow");
            itemMarioSlidingArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioSlidingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemMarioTwirlArrow = new ToolStripMenuItem("Add Tracker for Mario Twirl Arrow");
            itemMarioTwirlArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioTwirlArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemMarioFloorArrow = new ToolStripMenuItem("Add Tracker for Mario Floor Arrow");
            itemMarioFloorArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioFloorArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemObjectFacingArrow = new ToolStripMenuItem("Add Tracker for Object Facing Arrow");
            itemObjectFacingArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectFacingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemObjectMovingArrow = new ToolStripMenuItem("Add Tracker for Object Moving Arrow");
            itemObjectMovingArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectMovingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemObjectGraphicsArrow = new ToolStripMenuItem("Add Tracker for Object Graphics Arrow");
            itemObjectGraphicsArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectGraphicsArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemObjectAngleToMarioArrow = new ToolStripMenuItem("Add Tracker for Object Angle to Mario Arrow");
            itemObjectAngleToMarioArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectAngleToMarioArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemObjectCustomArrow = new ToolStripMenuItem("Add Tracker for Object Custom Arrow");
            itemObjectCustomArrow.Click += (sender, e) =>
            {
                string yawOffsetString = DialogUtilities.GetStringFromDialog(labelText: "Enter the offset (in hex) of the yaw variable in the object struct:");
                if (yawOffsetString == null) return;
                uint yawOffset = ParsingUtilities.ParseHexNullable(yawOffsetString) ?? 0;
                string numBytesString = DialogUtilities.GetStringFromDialog(labelText: "Enter the number of bytes of the yaw variable:");
                if (numBytesString == null) return;
                int numBytes = ParsingUtilities.ParseInt(numBytesString);
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectCustomArrow(posAngle, yawOffset, numBytes);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemSwooperTargetArrow = new ToolStripMenuItem("Add Tracker for Swooper Effective Target Arrow");
            itemSwooperTargetArrow.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectSwooperEffectiveTargetArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomPositionAngleArrow = new ToolStripMenuItem("Add Tracker for Custom PositionAngle Arrow");
            itemCustomPositionAngleArrow.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter a PositionAngle.");
                PositionAngle anglePA = PositionAngle.FromString(text);
                if (anglePA == null) return;
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posPA = mapObj.GetPositionAngle();
                    if (posPA == null) return null;
                    return (MapObject)new MapObjectCustomPositionAngleArrow(posPA, anglePA);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCurrentUnit = new ToolStripMenuItem("Add Tracker for Current Unit");
            itemCurrentUnit.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectCurrentUnit(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCurrentCell = new ToolStripMenuItem("Add Tracker for Current Cell");
            itemCurrentCell.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectCurrentCell(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemAngleRange = new ToolStripMenuItem("Add Tracker for Angle Range");
            itemAngleRange.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectAngleRange(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemSector = new ToolStripMenuItem("Add Tracker for Sector");
            itemSector.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectSector(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemFacingDivider = new ToolStripMenuItem("Add Tracker for Facing Divider");
            itemFacingDivider.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectFacingDivider(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemHomeLine = new ToolStripMenuItem("Add Tracker for Home Line");
            itemHomeLine.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectHomeLine(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemPath = new ToolStripMenuItem("Add Tracker for Path");
            itemPath.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectPath(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemBranchPath = new ToolStripMenuItem("Add Tracker for Branch Path");
            itemBranchPath.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectBranchPath(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCoffinBox = new ToolStripMenuItem("Add Tracker for Coffin Box");
            itemCoffinBox.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectCoffinBox(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemChuckyaMapObjects = new ToolStripMenuItem("Add Trackers for Chuckya Map Objects");
            itemChuckyaMapObjects.Click += (sender, e) =>
            {
                foreach (MapObject mapObj in _mapObjectList)
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (!posAngle.IsObjectOrMario()) continue;
                    uint address = posAngle.GetObjAddress();
                    PositionAngle chuckyaPosAngle = PositionAngle.Obj(address);
                    PositionAngle homePosAngle = PositionAngle.ObjHome(address);

                    MapObject mapObjHome = new MapObjectHome(chuckyaPosAngle);
                    MapObject mapObjFacingArrow = new MapObjectObjectFacingArrow(chuckyaPosAngle);
                    MapObject mapObjSector = new MapObjectSector(chuckyaPosAngle);
                    MapObject mapObjFacingDivider = new MapObjectFacingDivider(chuckyaPosAngle);
                    MapObject mapObjSphere = new MapObjectCustomSphere(chuckyaPosAngle);
                    MapObject mapObjCylinder = new MapObjectCustomCylinder(homePosAngle);

                    mapObjFacingArrow.LineColor = Color.Green;
                    mapObjFacingArrow.Size = 3000;
                    mapObjSector.Size = 3000;
                    mapObjFacingDivider.Size = 3000;
                    mapObjSphere.Size = 4000;
                    mapObjCylinder.Color = Color.Cyan;
                    mapObjCylinder.Size = 1900;
                    mapObjCylinder.ApplySettings(new MapObjectSettings(changeCustomCylinderRelativeMinY: true, newCustomCylinderRelativeMinY: -5000));

                    Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(new MapTracker(mapObjHome));
                    Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(new MapTracker(mapObjFacingArrow));
                    Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(new MapTracker(mapObjSector));
                    Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(new MapTracker(mapObjFacingDivider));
                    Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(new MapTracker(mapObjSphere));
                    Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(new MapTracker(mapObjCylinder));
                }
            };

            ToolStripMenuItem itemCylinder = new ToolStripMenuItem("Cylinder...");
            itemCylinder.DropDownItems.Add(itemHitboxCylinder);
            itemCylinder.DropDownItems.Add(itemEffectiveHitboxCylinder);
            itemCylinder.DropDownItems.Add(itemHurtboxCylinder);
            itemCylinder.DropDownItems.Add(itemEffectiveHurtboxCylinder);
            itemCylinder.DropDownItems.Add(itemPushHitboxCylinder);
            itemCylinder.DropDownItems.Add(itemCustomCylinder);

            ToolStripMenuItem itemSphere = new ToolStripMenuItem("Sphere...");
            itemSphere.DropDownItems.Add(itemTangibilitySphere);
            itemSphere.DropDownItems.Add(itemDrawDistanceSphere);
            itemSphere.DropDownItems.Add(itemCustomSphere);

            ToolStripMenuItem itemHomeContainer = new ToolStripMenuItem("Home...");
            itemHomeContainer.DropDownItems.Add(itemHome);
            itemHomeContainer.DropDownItems.Add(itemCustomCylinderForHome);
            itemHomeContainer.DropDownItems.Add(itemCustomSphereForHome);

            ToolStripMenuItem itemTriangles = new ToolStripMenuItem("Triangles...");
            itemTriangles.DropDownItems.Add(itemFloorTriangles);
            itemTriangles.DropDownItems.Add(itemWallTriangles);
            itemTriangles.DropDownItems.Add(itemCeilingTriangles);

            ToolStripMenuItem itemArrow = new ToolStripMenuItem("Arrow...");
            itemArrow.DropDownItems.Add(itemMarioFacingArrow);
            itemArrow.DropDownItems.Add(itemMarioMovingArrow);
            itemArrow.DropDownItems.Add(itemMarioIntendedArrow);
            itemArrow.DropDownItems.Add(itemMarioSlidingArrow);
            itemArrow.DropDownItems.Add(itemMarioTwirlArrow);
            itemArrow.DropDownItems.Add(itemMarioFloorArrow);
            itemArrow.DropDownItems.Add(new ToolStripSeparator());
            itemArrow.DropDownItems.Add(itemObjectFacingArrow);
            itemArrow.DropDownItems.Add(itemObjectMovingArrow);
            itemArrow.DropDownItems.Add(itemObjectGraphicsArrow);
            itemArrow.DropDownItems.Add(itemObjectAngleToMarioArrow);
            itemArrow.DropDownItems.Add(itemObjectCustomArrow);
            itemArrow.DropDownItems.Add(new ToolStripSeparator());
            itemArrow.DropDownItems.Add(itemSwooperTargetArrow);
            itemArrow.DropDownItems.Add(new ToolStripSeparator());
            itemArrow.DropDownItems.Add(itemCustomPositionAngleArrow);

            ToolStripMenuItem itemMisc = new ToolStripMenuItem("Misc...");
            itemMisc.DropDownItems.Add(itemCurrentUnit);
            itemMisc.DropDownItems.Add(itemCurrentCell);
            itemMisc.DropDownItems.Add(itemAngleRange);
            itemMisc.DropDownItems.Add(itemSector);
            itemMisc.DropDownItems.Add(itemFacingDivider);
            itemMisc.DropDownItems.Add(itemHomeLine);
            itemMisc.DropDownItems.Add(itemPath);
            itemMisc.DropDownItems.Add(itemBranchPath);

            ToolStripMenuItem itemObjectSpecific = new ToolStripMenuItem("Object Specific...");
            itemObjectSpecific.DropDownItems.Add(itemCoffinBox);
            itemObjectSpecific.DropDownItems.Add(itemChuckyaMapObjects);

            pictureBoxPlus.ContextMenuStrip = new ContextMenuStrip();
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemCylinder);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemSphere);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemHomeContainer);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemTriangles);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemArrow);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemMisc);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemObjectSpecific);

            pictureBoxPlus.Click += (sender, e) => pictureBoxPlus.ContextMenuStrip.Show(Cursor.Position);
        }

        private void SetIconType(MapTrackerIconType iconType, List<string> paths = null)
        {
            switch (iconType)
            {
                case MapTrackerIconType.TopDownImage:
                    SetIconTypeTopDownImage();
                    break;
                case MapTrackerIconType.ObjectSlotImage:
                    SetIconTypeObjectSlotImage();
                    break;
                case MapTrackerIconType.CustomImage:
                    SetIconTypeCustomImage(paths);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetIconTypeTopDownImage()
        {
            _iconType = MapTrackerIconType.TopDownImage;
            _mapObjectList.ForEach(mapObj => mapObj.SetIconType(MapTrackerIconType.TopDownImage));
            _itemUseTopDownImage.Checked = true;
            _itemUseObjectSlotImage.Checked = false;
            _itemUseCustomImage.Checked = false;
        }

        private void SetIconTypeObjectSlotImage()
        {
            _iconType = MapTrackerIconType.ObjectSlotImage;
            _mapObjectList.ForEach(mapObj => mapObj.SetIconType(MapTrackerIconType.ObjectSlotImage));
            _itemUseTopDownImage.Checked = false;
            _itemUseObjectSlotImage.Checked = true;
            _itemUseCustomImage.Checked = false;
        }

        private void SetIconTypeCustomImage(List<string> paths = null)
        {
            _iconType = MapTrackerIconType.CustomImage;
            List<(Image image, string path)> data;
            if (paths != null)
            {
                data = paths.ConvertAll(path => (Image.FromFile(path), path));
            }
            else
            {
                data = DialogUtilities.GetImagesAndPaths();
            }
            if (data == null || data.Count == 0) return;
            for (int i = 0; i < _mapObjectList.Count; i++)
            {
                (Image image, string path) = data[i % data.Count];
                _mapObjectList[i].SetIconType(
                    MapTrackerIconType.CustomImage, image, path);
            }
            _itemUseTopDownImage.Checked = false;
            _itemUseObjectSlotImage.Checked = false;
            _itemUseCustomImage.Checked = true;
        }

        public bool ContainsMapObject(MapObject mapObject)
        {
            return _mapObjectList.Contains(mapObject);
        }

        public List<MapObjectPath> GetMapPathObjects()
        {
            List<MapObjectPath> output = new List<MapObjectPath>();
            foreach (MapObject mapObj in _mapObjectList)
            {
                if (mapObj is MapObjectPath mapPathObj) output.Add(mapPathObj);
            }
            return output;
        }

        public void ApplySettings(MapObjectSettings settings)
        {
            _mapObjectList.ForEach(mapObj => mapObj.ApplySettings(settings));
        }

        public List<MapObject> GetMapObjectsToDisplay()
        {
            if (!_isVisible) return new List<MapObject>();
            return _mapObjectList.FindAll(mapObj => mapObj.ShouldDisplay(
                (MapTrackerVisibilityType)comboBoxVisibilityType.SelectedItem));
        }

        public MapTrackerOrderType GetOrderType()
        {
            return (MapTrackerOrderType)comboBoxOrderType.SelectedItem;
        }

        public void SetOrderType(MapTrackerOrderType type)
        {
            comboBoxOrderType.SelectedItem = type;
        }

        private void trackBarSize_ValueChanged()
        {
            SetSize(trackBarSize.Value);
        }

        private void textBoxSize_EnterAction()
        {
            SetSize(ParsingUtilities.ParseFloatNullable(textBoxSize.Text));
        }

        /** null if controls should be refreshed */
        public void SetSize(float? sizeNullable)
        {
            bool updateMapObjs = sizeNullable != null;
            float size = sizeNullable ?? _mapObjectList[0].Size;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.Size = size);
            }
            textBoxSize.SubmitText(size.ToString());
            trackBarSize.StartChangingByCode();
            ControlUtilities.SetTrackBarValueCapped(trackBarSize, size);
            trackBarSize.StopChangingByCode();
        }

        private void trackBarOpacity_ValueChanged()
        {
            SetOpacity(trackBarOpacity.Value);
        }

        private void textBoxOpacity_EnterAction()
        {
            SetOpacity(ParsingUtilities.ParseIntNullable(textBoxOpacity.Text));
        }

        /** null if controls should be refreshed */
        public void SetOpacity(int? opacityNullable)
        {
            bool updateMapObjs = opacityNullable != null;
            int opacity = opacityNullable ?? _mapObjectList[0].OpacityPercent;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.OpacityPercent = opacity);
            }
            textBoxOpacity.SubmitText(opacity.ToString());
            trackBarOpacity.StartChangingByCode();
            ControlUtilities.SetTrackBarValueCapped(trackBarOpacity, opacity);
            trackBarOpacity.StopChangingByCode();
        }

        private void trackBarLineWidth_ValueChanged()
        {
            SetLineWidth(trackBarLineWidth.Value);
        }

        private void textBoxLineWidth_EnterAction()
        {
            SetLineWidth(ParsingUtilities.ParseFloatNullable(textBoxLineWidth.Text));
        }

        /** null if controls should be refreshed */
        private void SetLineWidth(float? lineWidthNullable)
        {
            bool updateMapObjs = lineWidthNullable != null;
            float lineWidth = lineWidthNullable ?? _mapObjectList[0].LineWidth;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.LineWidth = lineWidth);
            }
            textBoxLineWidth.SubmitText(lineWidth.ToString());
            trackBarLineWidth.StartChangingByCode();
            ControlUtilities.SetTrackBarValueCapped(trackBarLineWidth, lineWidth);
            trackBarLineWidth.StopChangingByCode();
        }

        /** null if controls should be refreshed */
        public void SetColor(Color? colorNullable)
        {
            bool updateMapObjs = colorNullable != null;
            Color color = colorNullable ?? _mapObjectList[0].Color;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.Color = color);
            }
            colorSelector.SelectedColor = color;
        }

        /** null if controls should be refreshed */
        public void SetLineColor(Color? lineColorNullable)
        {
            bool updateMapObjs = lineColorNullable != null;
            Color lineColor = lineColorNullable ?? _mapObjectList[0].LineColor;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.LineColor = lineColor);
            }
            colorSelectorLine.SelectedColor = lineColor;
        }

        public void SetCustomRotates(bool? customRotates)
        {
            _mapObjectList.ForEach(mapObj => mapObj.CustomRotates = customRotates);
        }

        public void SetScales(bool? scalesNullable)
        {
            bool updateMapObjs = scalesNullable != null;
            bool scales = scalesNullable ?? _mapObjectList[0].Scales;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.Scales = scales);
            }
            checkBoxScales.Checked = scales;
        }

        private void pictureBoxRedX_Click(object sender, EventArgs e)
        {
            Config.MapGui.flowLayoutPanelMapTrackers.RemoveControl(this);
        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {
            SetIsVisible(!_isVisible);
        }

        public void SetIsVisible(bool isVisible)
        {
            _isVisible = isVisible;
            pictureBoxEye.BackgroundImage = isVisible ? ImageEyeOpen : ImageEyeClosed;
        }

        private void pictureBoxUpArrow_Click(object sender, EventArgs e)
        {
            int numMoves = KeyboardUtilities.GetCurrentlyInputtedNumber() ?? 1;
            if (KeyboardUtilities.IsCtrlHeld()) numMoves = 0;
            Config.MapGui.flowLayoutPanelMapTrackers.MoveUpControl(this, numMoves);
        }

        private void pictureBoxDownArrow_Click(object sender, EventArgs e)
        {
            int numMoves = KeyboardUtilities.GetCurrentlyInputtedNumber() ?? 1;
            if (KeyboardUtilities.IsCtrlHeld()) numMoves = 0;
            Config.MapGui.flowLayoutPanelMapTrackers.MoveDownControl(this, numMoves);
        }

        public void SetGlobalIconSize(float size)
        {
            if (_mapObjectList.Any(mapObj => mapObj.ParticipatesInGlobalIconSize()))
            {
                SetSize(size);
            }
        }

        public void UpdateControl()
        {
            textBoxName.SubmitTextLoosely(_customName ?? string.Join(", ", _mapObjectList.ConvertAll(obj => obj.GetName())));

            List<Image> images = _mapObjectList.ConvertAll(mapObj => mapObj.GetImage());
            if (!images.SequenceEqual(_images))
            {
                _images = images;
                pictureBoxPicture.Image = ImageUtilities.CreateMultiImage(images, 256, 256);
            }

            MapTrackerVisibilityType currentVisibilityType = (MapTrackerVisibilityType)comboBoxVisibilityType.SelectedValue;
            if (currentVisibilityType != _currentVisiblityType)
            {
                if (currentVisibilityType == MapTrackerVisibilityType.VisibleWhenThisBhvrIsLoaded)
                {
                    foreach (MapObject mapObj in _mapObjectList)
                    {
                        mapObj.NotifyStoreBehaviorCritera();
                    }
                }
                _currentVisiblityType = currentVisibilityType;
            }

            checkBoxRotates.CheckState = BoolUtilities.GetCheckState(
                _mapObjectList.ConvertAll(mapObj => mapObj.Rotates));

            _mapObjectList.ForEach(mapObj => mapObj.Update());

            if (_semaphoreList.Any(semaphore => !semaphore.IsUsed))
            {
                Config.MapGui.flowLayoutPanelMapTrackers.RemoveControl(this);
            }
        }

        public void CleanUp()
        {
            _semaphoreList.ForEach(semaphore => semaphore.IsUsed = false);
            _mapObjectList.ForEach(mapObj => mapObj.CleanUp());
        }

        public override string ToString()
        {
            return string.Join(", ", _mapObjectList);
        }

        public void NotifyMouseEvent(MouseEvent mouseEvent, bool isLeftButton, int mouseX, int mouseY)
        {
            foreach (MapObject mapObj in _mapObjectList)
            {
                mapObj.NotifyMouseEvent(mouseEvent, isLeftButton, mouseX, mouseY);
            }
        }

        public XElement ToXElement()
        {
            XElement xElement = new XElement("MapTracker");
            xElement.Add(new XAttribute("iconType", _iconType));
            if (_iconType == MapTrackerIconType.CustomImage)
            {
                List<string> paths = _mapObjectList.ConvertAll(mapObj => mapObj._customImagePath);
                xElement.Add(new XAttribute("paths", string.Join("|", paths)));
            }
            if (_customName != null)
            {
                xElement.Add(new XAttribute("customName", _customName));
            }
            xElement.Add(new XAttribute("size", _mapObjectList[0].Size));
            xElement.Add(new XAttribute("opacity", _mapObjectList[0].OpacityPercent));
            xElement.Add(new XAttribute("lineWidth", _mapObjectList[0].LineWidth));
            xElement.Add(new XAttribute("orderType", comboBoxOrderType.SelectedItem));
            xElement.Add(new XAttribute("visibilityType", comboBoxVisibilityType.SelectedItem));
            xElement.Add(new XAttribute("color", ColorUtilities.ConvertColorToParams(_mapObjectList[0].Color)));
            xElement.Add(new XAttribute("lineColor", ColorUtilities.ConvertColorToParams(_mapObjectList[0].LineColor)));
            if (_mapObjectList[0].CustomRotates.HasValue)
            {
                xElement.Add(new XAttribute("customRotates", _mapObjectList[0].CustomRotates.Value));
            }
            xElement.Add(new XAttribute("scales", _mapObjectList[0].Scales));
            xElement.Add(new XAttribute("isVisible", _isVisible));
            foreach (MapObject mapObj in _mapObjectList)
            {
                xElement.Add(mapObj.ToXElement());
            }
            return xElement;
        }

        public static MapTracker FromXElement(XElement xElement)
        {
            List<XElement> subElements = xElement.Elements().ToList();
            List<MapObject> mapObjs = subElements.ConvertAll(el => MapObject.FromXElement(el));
            MapTracker tracker = new MapTracker(mapObjs);
            MapTrackerIconType iconType = (MapTrackerIconType)Enum.Parse(typeof(MapTrackerIconType), xElement.Attribute(XName.Get("iconType")).Value);
            List<string> paths = null;
            if (iconType == MapTrackerIconType.CustomImage)
            {
                paths = xElement.Attribute(XName.Get("paths")).Value.Split('|').ToList();
            }
            tracker.SetIconType(iconType, paths);
            string customName = xElement.Attribute(XName.Get("customName"))?.Value;
            if (customName != null)
            {
                tracker._customName = customName;
            }
            tracker.SetSize(ParsingUtilities.ParseFloatNullable(xElement.Attribute(XName.Get("size")).Value));
            tracker.SetOpacity(ParsingUtilities.ParseIntNullable(xElement.Attribute(XName.Get("opacity")).Value));
            tracker.SetLineWidth(ParsingUtilities.ParseFloatNullable(xElement.Attribute(XName.Get("lineWidth")).Value));
            tracker.comboBoxOrderType.SelectedItem = Enum.Parse(typeof(MapTrackerOrderType), xElement.Attribute(XName.Get("orderType")).Value);
            tracker.comboBoxVisibilityType.SelectedItem = Enum.Parse(typeof(MapTrackerVisibilityType), xElement.Attribute(XName.Get("visibilityType")).Value);
            tracker.SetColor(ColorUtilities.GetColorFromString(xElement.Attribute(XName.Get("color")).Value));
            tracker.SetLineColor(ColorUtilities.GetColorFromString(xElement.Attribute(XName.Get("lineColor")).Value));
            bool? customRotates = ParsingUtilities.ParseBoolNullable(xElement.Attribute(XName.Get("customRotates"))?.Value);
            if (customRotates.HasValue)
            {
                tracker.SetCustomRotates(customRotates.Value);
            }
            tracker.SetScales(ParsingUtilities.ParseBool(xElement.Attribute(XName.Get("scales")).Value));
            tracker.SetIsVisible(ParsingUtilities.ParseBool(xElement.Attribute(XName.Get("isVisible")).Value));
            return tracker;
        }
    }
}
