using System;
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

        private List<Image> _images;

        private bool _isVisible;
        private MapTrackerVisibilityType _currentVisiblityType;
        private bool _showTriUnits;

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

            _images = new List<Image>();

            _isVisible = true;
            _currentVisiblityType = MapTrackerVisibilityType.VisibleWhenLoaded;
            _showTriUnits = false;

            pictureBoxPicture.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem itemUseTopDownImage = new ToolStripMenuItem("Use Top Down Image");
            ToolStripMenuItem itemUseObjectSlotImage = new ToolStripMenuItem("Use Object Slot Image");
            ToolStripMenuItem itemUseCustomImage = new ToolStripMenuItem("Use Custom Image");
            List<ToolStripMenuItem> pictureBoxItems = new List<ToolStripMenuItem>()
            {
                itemUseTopDownImage, itemUseObjectSlotImage, itemUseCustomImage
            };
            itemUseTopDownImage.Click += (sender, e) =>
            {
                _mapObjectList.ForEach(mapObj => mapObj.SetIconType(MapTrackerIconType.TopDownImage));
                pictureBoxItems.ForEach(item => item.Checked = item == itemUseTopDownImage);
            };
            itemUseObjectSlotImage.Click += (sender, e) =>
            {
                _mapObjectList.ForEach(mapObj => mapObj.SetIconType(MapTrackerIconType.ObjectSlotImage));
                pictureBoxItems.ForEach(item => item.Checked = item == itemUseObjectSlotImage);
            };
            itemUseCustomImage.Click += (sender, e) =>
            {
                List<Image> images = DialogUtilities.GetImages();
                if (images == null || images.Count == 0) return;
                for (int i = 0; i < _mapObjectList.Count; i++)
                {
                    _mapObjectList[i].SetIconType(
                        MapTrackerIconType.CustomImage, images[i % images.Count]);
                }
                pictureBoxItems.ForEach(item => item.Checked = item == itemUseCustomImage);
            };
            itemUseTopDownImage.Checked = true;
            pictureBoxPicture.ContextMenuStrip.Items.Add(itemUseTopDownImage);
            pictureBoxPicture.ContextMenuStrip.Items.Add(itemUseObjectSlotImage);
            pictureBoxPicture.ContextMenuStrip.Items.Add(itemUseCustomImage);

            _customName = null;
            textBoxName.AddEnterAction(() => _customName = textBoxName.Text);
            textBoxName.AddLostFocusAction(() => _customName = textBoxName.Text);
            textBoxName.AddDoubleClickAction(() => textBoxName.SelectAll());
            textBoxName.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem itemResetCustomName = new ToolStripMenuItem("Reset Custom Name");
            itemResetCustomName.Click += (sender, e) => _customName = null;
            textBoxName.ContextMenuStrip.Items.Add(itemResetCustomName);

            checkBoxRotates.Click += (sender, e) =>
                _mapObjectList.ForEach(mapObj => mapObj.CustomRotates = checkBoxRotates.Checked);
            checkBoxRotates.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem itemResetCustomRotates = new ToolStripMenuItem("Reset Custom Rotates");
            itemResetCustomRotates.Click += (sender, e) => _mapObjectList.ForEach(mapObj => mapObj.CustomRotates = null);
            checkBoxRotates.ContextMenuStrip.Items.Add(itemResetCustomRotates);

            tableLayoutPanel.BorderWidth = 2;
            tableLayoutPanel.ShowBorder = true;

            comboBoxVisibilityType.DataSource = Enum.GetValues(typeof(MapTrackerVisibilityType));
            comboBoxVisibilityType.SelectedItem = MapTrackerVisibilityType.VisibleWhenLoaded;

            comboBoxOrderType.DataSource = Enum.GetValues(typeof(MapTrackerOrderType));
            comboBoxOrderType.SelectedItem = MapTrackerOrderType.OrderByY;

            SetSize(null);
            SetOpacity(null);
            SetOutlineWidth(null);
            SetColor(null);
            SetOutlineColor(null);

            textBoxSize.AddEnterAction(() => textBoxSize_EnterAction());
            trackBarSize.AddManualChangeAction(() => trackBarSize_ValueChanged());
            textBoxOpacity.AddEnterAction(() => textBoxOpacity_EnterAction());
            trackBarOpacity.AddManualChangeAction(() => trackBarOpacity_ValueChanged());
            textBoxOutlineWidth.AddEnterAction(() => textBoxOutlineWidth_EnterAction());
            trackBarOutlineWidth.AddManualChangeAction(() => trackBarOutlineWidth_ValueChanged());
            colorSelector.AddColorChangeAction((Color color) => SetColor(color));
            colorSelectorOutline.AddColorChangeAction((Color color) => SetOutlineColor(color));

            _mapObjectList.ForEach(mapObj => mapObj.GetContextMenuStrip()); // avoids null pointer exceptions
            pictureBoxCog.ContextMenuStrip = _mapObjectList[0].GetContextMenuStrip();
            pictureBoxCog.Click += (sender, e) => pictureBoxCog.ContextMenuStrip.Show(Cursor.Position);

            MapUtilities.CreateTrackBarContextMenuStrip(trackBarSize);
            MapUtilities.CreateTrackBarContextMenuStrip(trackBarOutlineWidth);
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
                    return (MapObject)new MapHitboxCylinderObject(posAngle);
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
                    return (MapObject)new MapEffectiveHitboxCylinderObject(posAngle);
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
                    return (MapObject)new MapHurtboxCylinderObject(posAngle);
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
                    return (MapObject)new MapEffectiveHurtboxCylinderObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapPushHitboxCylinderObject(posAngle);
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
                    return (MapObject)new MapTangibilitySphereObject(posAngle);
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
                    return (MapObject)new MapDrawDistanceSphereObject(posAngle);
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
                    return (MapObject)new MapCustomCylinderObject(posAngle);
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
                    return (MapObject)new MapCustomSphereObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapHomeObject(posAngle.GetObjAddress());
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
                    if (!posAngle.IsObject()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (MapObject)new MapCustomCylinderObject(homePosAngle);
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
                    if (!posAngle.IsObject()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (MapObject)new MapCustomSphereObject(homePosAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapObjectFloorObject(posAngle.GetObjAddress());
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapObjectWallObject(posAngle.GetObjAddress());
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapObjectCeilingObject(posAngle.GetObjAddress());
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
                    return (MapObject)new MapMarioFacingArrowObject(posAngle);
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
                    return (MapObject)new MapMarioMovingArrowObject(posAngle);
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
                    return (MapObject)new MapMarioIntendedArrowObject(posAngle);
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
                    return (MapObject)new MapMarioSlidingArrowObject(posAngle);
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
                    return (MapObject)new MapMarioTwirlArrowObject(posAngle);
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
                    return (MapObject)new MapMarioFloorArrowObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapObjectFacingArrowObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapObjectMovingArrowObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapObjectGraphicsArrowObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapObjectAngleToMarioArrowObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapObjectCustomArrowObject(posAngle, yawOffset, numBytes);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapSwooperEffectiveTargetArrowObject(posAngle);
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
                    return (MapObject)new MapCustomPositionAngleArrowObject(posPA, anglePA);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                MapTracker tracker = new MapTracker(newMapObjs);
                Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemArrow = new ToolStripMenuItem("Add Tracker for Arrow...");
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

            ToolStripMenuItem itemCurrentUnit = new ToolStripMenuItem("Add Tracker for Current Unit");
            itemCurrentUnit.Click += (sender, e) =>
            {
                List<MapObject> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapCurrentUnitObject(posAngle);
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
                    return (MapObject)new MapAngleRangeObject(posAngle);
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
                    return (MapObject)new MapSectorObject(posAngle);
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
                    return (MapObject)new MapFacingDividerObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapHomeLineObject(posAngle.GetObjAddress());
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
                    return (MapObject)new MapPathObject(posAngle);
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
                    return (MapObject)new MapBranchPathObject(posAngle);
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
                    if (!posAngle.IsObject()) return null;
                    return (MapObject)new MapCoffinBoxObject(posAngle.GetObjAddress());
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
                    if (!posAngle.IsObject()) continue;
                    uint address = posAngle.GetObjAddress();
                    PositionAngle chuckyaPosAngle = PositionAngle.Obj(address);
                    PositionAngle homePosAngle = PositionAngle.ObjHome(address);

                    MapObject mapObjHome = new MapHomeObject(address);
                    MapObject mapObjFacingArrow = new MapObjectFacingArrowObject(chuckyaPosAngle);
                    MapObject mapObjSector = new MapSectorObject(chuckyaPosAngle);
                    MapObject mapObjFacingDivider = new MapFacingDividerObject(chuckyaPosAngle);
                    MapObject mapObjSphere = new MapCustomSphereObject(chuckyaPosAngle);
                    MapObject mapObjCylinder = new MapCustomCylinderObject(homePosAngle);

                    mapObjFacingArrow.OutlineColor = Color.Green;
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

            ToolStripMenuItem itemObjectSpecific = new ToolStripMenuItem("Add Tracker for Object Specific...");
            itemObjectSpecific.DropDownItems.Add(itemCoffinBox);
            itemObjectSpecific.DropDownItems.Add(itemChuckyaMapObjects);

            pictureBoxPlus.ContextMenuStrip = new ContextMenuStrip();
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemHitboxCylinder);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemEffectiveHitboxCylinder);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemHurtboxCylinder);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemEffectiveHurtboxCylinder);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemPushHitboxCylinder);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemTangibilitySphere);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemDrawDistanceSphere);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemCustomCylinder);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemCustomSphere);
            pictureBoxPlus.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemHome);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemCustomCylinderForHome);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemCustomSphereForHome);
            pictureBoxPlus.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemFloorTriangles);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemWallTriangles);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemCeilingTriangles);
            pictureBoxPlus.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemArrow);
            pictureBoxPlus.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemCurrentUnit);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemAngleRange);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemSector);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemFacingDivider);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemHomeLine);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemPath);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemBranchPath);
            pictureBoxPlus.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemObjectSpecific);
            pictureBoxPlus.Click += (sender, e) => pictureBoxPlus.ContextMenuStrip.Show(Cursor.Position);
        }

        public bool ContainsMapObject(MapObject mapObject)
        {
            return _mapObjectList.Contains(mapObject);
        }

        public List<MapPathObject> GetMapPathObjects()
        {
            List<MapPathObject> output = new List<MapPathObject>();
            foreach (MapObject mapObj in _mapObjectList)
            {
                if (mapObj is MapPathObject mapPathObj) output.Add(mapPathObj);
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

        private void trackBarOutlineWidth_ValueChanged()
        {
            SetOutlineWidth(trackBarOutlineWidth.Value);
        }

        private void textBoxOutlineWidth_EnterAction()
        {
            SetOutlineWidth(ParsingUtilities.ParseFloatNullable(textBoxOutlineWidth.Text));
        }

        /** null if controls should be refreshed */
        private void SetOutlineWidth(float? outlineWidthNullable)
        {
            bool updateMapObjs = outlineWidthNullable != null;
            float outlineWidth = outlineWidthNullable ?? _mapObjectList[0].OutlineWidth;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.OutlineWidth = outlineWidth);
            }
            textBoxOutlineWidth.SubmitText(outlineWidth.ToString());
            trackBarOutlineWidth.StartChangingByCode();
            ControlUtilities.SetTrackBarValueCapped(trackBarOutlineWidth, outlineWidth);
            trackBarOutlineWidth.StopChangingByCode();
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
        public void SetOutlineColor(Color? outlineColorNullable)
        {
            bool updateMapObjs = outlineColorNullable != null;
            Color outlineColor = outlineColorNullable ?? _mapObjectList[0].OutlineColor;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.OutlineColor = outlineColor);
            }
            colorSelectorOutline.SelectedColor = outlineColor;
        }

        public void SetShowTriUnits(bool showTriUnits)
        {
            _showTriUnits = showTriUnits;
            checkBoxShowTriUnits.Checked = showTriUnits;
        }

        private void CheckBoxShowTriUnits_CheckedChanged(object sender, EventArgs e)
        {
            _showTriUnits = checkBoxShowTriUnits.Checked;
            _mapObjectList.ForEach(mapObj => mapObj.ShowTriUnits = _showTriUnits);
        }

        private void pictureBoxRedX_Click(object sender, EventArgs e)
        {
            Config.MapGui.flowLayoutPanelMapTrackers.RemoveControl(this);
        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {
            _isVisible = !_isVisible;
            pictureBoxEye.BackgroundImage = _isVisible ? ImageEyeOpen : ImageEyeClosed;
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
            xElement.Add(new XAttribute("size", _mapObjectList[0].Size));
            xElement.Add(new XAttribute("opacity", _mapObjectList[0].OpacityPercent));
            xElement.Add(new XAttribute("outlineWidth", _mapObjectList[0].OutlineWidth));
            xElement.Add(new XAttribute("orderType", comboBoxOrderType.SelectedItem.ToString()));
            xElement.Add(new XAttribute("visibilityType", comboBoxVisibilityType.SelectedItem.ToString()));
            xElement.Add(new XAttribute("color", ColorUtilities.ConvertColorToParams(_mapObjectList[0].Color)));
            xElement.Add(new XAttribute("outlineColor", ColorUtilities.ConvertColorToParams(_mapObjectList[0].OutlineColor)));
            if (_mapObjectList[0].CustomRotates.HasValue) xElement.Add(new XAttribute("customRotates", _mapObjectList[0].CustomRotates));
            xElement.Add(new XAttribute("showTriUnits", _mapObjectList[0].ShowTriUnits));
            xElement.Add(new XAttribute("isVisible", _isVisible));
            foreach (MapObject mapObj in _mapObjectList)
            {
                xElement.Add(mapObj.ToXElement());
            }
            return xElement;
        }
    }
}
