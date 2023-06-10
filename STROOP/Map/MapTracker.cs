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
using OpenTK;
using STROOP.Controls;
using STROOP.Forms;

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
        private bool _isVisibleFor2DTopDown;
        private bool _isVisibleFor2DOrthographic;
        private bool _isVisibleFor3D;
        private MapTrackerVisibilityType _currentVisiblityType;

        private ToolStripMenuItem _itemVisibleOn2DTopDown;
        private ToolStripMenuItem _itemVisibleOn2DOrthographic;
        private ToolStripMenuItem _itemVisibleOn3D;

        private string _customName;
        public string TrackerName
        {
            get => _customName ?? string.Join(", ", _mapObjectList.ConvertAll(obj => obj.GetName()));
        }

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
            _isVisibleFor2DTopDown = true;
            _isVisibleFor2DOrthographic = true;
            _isVisibleFor3D = true;
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
            SetUseRelativeCoordinates(null);

            textBoxSize.AddEnterAction(() => textBoxSize_EnterAction());
            trackBarSize.AddManualChangeAction(() => trackBarSize_ValueChanged());
            textBoxOpacity.AddEnterAction(() => textBoxOpacity_EnterAction());
            trackBarOpacity.AddManualChangeAction(() => trackBarOpacity_ValueChanged());
            textBoxLineWidth.AddEnterAction(() => textBoxLineWidth_EnterAction());
            trackBarLineWidth.AddManualChangeAction(() => trackBarLineWidth_ValueChanged());
            colorSelector.AddColorChangeAction((Color color) => SetColor(color));
            colorSelectorLine.AddColorChangeAction((Color color) => SetLineColor(color));
            checkBoxScales.Click += (sender, e) => SetScales(checkBoxScales.Checked);
            checkBoxUseRelativeCoordinates.Click += (sender, e) => SetUseRelativeCoordinates(checkBoxUseRelativeCoordinates.Checked);
            _mapObjectList.ForEach(mapObj => mapObj.GetContextMenuStrip()); // avoids null pointer exceptions
            pictureBoxCog.ContextMenuStrip = _mapObjectList[0].GetContextMenuStrip();
            pictureBoxCog.Click += (sender, e) => pictureBoxCog.ContextMenuStrip.Show(Cursor.Position);

            AddControllerOption(labelSize, () => _mapObjectList[0].Size, (float value) => SetSize(value));
            AddControllerOption(labelOpacity, () => _mapObjectList[0].OpacityPercent, (float value) => SetOpacity((int)value));
            AddControllerOption(labelLineWidth, () => _mapObjectList[0].LineWidth, (float value) => SetLineWidth(value));

            MapUtilities.CreateTrackBarContextMenuStrip(trackBarSize, () => _mapObjectList[0].Size);
            MapUtilities.CreateTrackBarContextMenuStrip(trackBarLineWidth, () => _mapObjectList[0].LineWidth);
            InitializeEyeContextMenuStrip();
            InitializePlusContextMenuStrip();

            UpdateControl();
        }

        private void AddControllerOption(Label label, Func<float> getter, Action<float> setter)
        {
            ControlUtilities.AddContextMenuStripFunctions(
                label,
                new List<string>() { "Open Controller" },
                new List<Action>() {
                    () =>
                    {
                        string specialType = WatchVariableSpecialUtilities.AddTextboxEntry(getter, setter);
                        string name = label.Text.Substring(0, label.Text.Length - 1);

                        WatchVariable watchVariable =
                            new WatchVariable(
                                name: name,
                                memoryTypeName: null,
                                specialType: specialType,
                                baseAddressType: BaseAddressTypeEnum.None,
                                offsetUS: null,
                                offsetJP: null,
                                offsetSH: null,
                                offsetEU: null,
                                offsetDefault: null,
                                mask: null,
                                shift: null,
                                handleMapping: true);
                        WatchVariableControlPrecursor precursor =
                            new WatchVariableControlPrecursor(
                                name: name,
                                watchVar: watchVariable,
                                subclass: WatchVariableSubclass.Number,
                                backgroundColor: null,
                                displayType: null,
                                roundingLimit: null,
                                useHex: null,
                                invertBool: null,
                                isYaw: null,
                                coordinate: null,
                                groupList: new List<VariableGroup>() { VariableGroup.Custom });
                        WatchVariableControl control = precursor.CreateWatchVariableControl();

                        VariableControllerForm varController =
                            new VariableControllerForm(name, control.WatchVarWrapper, null);
                        varController.Show();
                    }
                });
        }

        private void InitializeEyeContextMenuStrip()
        {
            pictureBoxEye.ContextMenuStrip = new ContextMenuStrip();

            _itemVisibleOn2DTopDown = new ToolStripMenuItem("Visible for 2D Top Down");
            _itemVisibleOn2DTopDown.Click += (sender, e) =>
            {
                _isVisibleFor2DTopDown = !_isVisibleFor2DTopDown;
                _itemVisibleOn2DTopDown.Checked = _isVisibleFor2DTopDown;
            };
            _itemVisibleOn2DTopDown.Checked = _isVisibleFor2DTopDown;
            pictureBoxEye.ContextMenuStrip.Items.Add(_itemVisibleOn2DTopDown);

            _itemVisibleOn2DOrthographic = new ToolStripMenuItem("Visible for 2D Orthographic");
            _itemVisibleOn2DOrthographic.Click += (sender, e) =>
            {
                _isVisibleFor2DOrthographic = !_isVisibleFor2DOrthographic;
                _itemVisibleOn2DOrthographic.Checked = _isVisibleFor2DOrthographic;
            };
            _itemVisibleOn2DOrthographic.Checked = _isVisibleFor2DOrthographic;
            pictureBoxEye.ContextMenuStrip.Items.Add(_itemVisibleOn2DOrthographic);

            _itemVisibleOn3D = new ToolStripMenuItem("Visible for 3D");
            _itemVisibleOn3D.Click += (sender, e) =>
            {
                _isVisibleFor3D = !_isVisibleFor3D;
                _itemVisibleOn3D.Checked = _isVisibleFor3D;
            };
            _itemVisibleOn3D.Checked = _isVisibleFor3D;
            pictureBoxEye.ContextMenuStrip.Items.Add(_itemVisibleOn3D);
        }

        private void SetMapTypeVisibility(MapType mapType, bool value)
        {
            switch (mapType)
            {
                case MapType.Map2DTopDown:
                    _isVisibleFor2DTopDown = value;
                    _itemVisibleOn2DTopDown.Checked = value;
                    break;
                case MapType.Map2DOrthographic:
                    _isVisibleFor2DOrthographic = value;
                    _itemVisibleOn2DOrthographic.Checked = value;
                    break;
                case MapType.Map3D:
                    _isVisibleFor3D = value;
                    _itemVisibleOn3D.Checked = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializePlusContextMenuStrip()
        {
            void setUpClickAction(ToolStripMenuItem toolStripMenuItem, Func<List<MapObject>, List<MapObject>> createSubMapObjs)
            {
                if (_mapObjectList[0] is MapObjectAllObjectsWithName mapObjectAllObjectsWithName)
                {
                    toolStripMenuItem.Click += (sender, e) =>
                    {
                        List<MapObject> objs = ObjectUtilities.GetAllObjectAddresses()
                            .ConvertAll(address => new MapObjectObject(PositionAngle.Obj(address)) as MapObject);
                        List<MapObject> newMapObjs = createSubMapObjs(objs);
                        if (newMapObjs.Count == 0) return;
                        MapObject newMapObj = new MapObjectAllMapObjectsWithName(mapObjectAllObjectsWithName.GetObjName(), newMapObjs);
                        MapTracker tracker = new MapTracker(newMapObj);
                        Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
                    };
                }
                else
                {
                    toolStripMenuItem.Click += (sender, e) =>
                    {
                        List<MapObject> newMapObjs = createSubMapObjs(_mapObjectList);
                        if (newMapObjs.Count == 0) return;
                        MapTracker tracker = new MapTracker(newMapObjs);
                        Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(tracker);
                    };
                }
            }

            ToolStripMenuItem itemHitboxCylinder = new ToolStripMenuItem("Add Tracker for Hitbox Cylinder");
            setUpClickAction(itemHitboxCylinder, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectHitboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemEffectiveHitboxCylinder = new ToolStripMenuItem("Add Tracker for Effective Hitbox Cylinder");
            setUpClickAction(itemEffectiveHitboxCylinder, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectEffectiveHitboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemHurtboxCylinder = new ToolStripMenuItem("Add Tracker for Hurtbox Cylinder");
            setUpClickAction(itemHurtboxCylinder, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectHurtboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemEffectiveHurtboxCylinder = new ToolStripMenuItem("Add Tracker for Effective Hurtbox Cylinder");
            setUpClickAction(itemEffectiveHurtboxCylinder, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectEffectiveHurtboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemEffectiveHitboxHurtboxCylinder = new ToolStripMenuItem("Add Tracker for Effective Hitbox/Hurtbox Cylinder");
            setUpClickAction(itemEffectiveHitboxHurtboxCylinder, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectEffectiveHitboxHurtboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemPushHitboxCylinder = new ToolStripMenuItem("Add Tracker for Push Hitbox Cylinder");
            setUpClickAction(itemPushHitboxCylinder, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectPushHitboxCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemTangibilitySphere = new ToolStripMenuItem("Add Tracker for Tangibility Sphere");
            setUpClickAction(itemTangibilitySphere, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectTangibilitySphere(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemDrawDistanceSphere = new ToolStripMenuItem("Add Tracker for Draw Distance Sphere");
            setUpClickAction(itemDrawDistanceSphere, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectDrawDistanceSphere(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCustomCylinder = new ToolStripMenuItem("Add Tracker for Custom Cylinder");
            setUpClickAction(itemCustomCylinder, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectCustomCylinder(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCustomSphere = new ToolStripMenuItem("Add Tracker for Custom Sphere");
            setUpClickAction(itemCustomSphere, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectCustomSphere(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemHome = new ToolStripMenuItem("Add Tracker for Home");
            setUpClickAction(itemHome, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectHome(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCustomCylinderForHome = new ToolStripMenuItem("Add Tracker for Custom Cylinder for Home");
            setUpClickAction(itemCustomCylinderForHome, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (MapObject)new MapObjectCustomCylinder(homePosAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCustomSphereForHome = new ToolStripMenuItem("Add Tracker for Custom Sphere for Home");
            setUpClickAction(itemCustomSphereForHome, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (MapObject)new MapObjectCustomSphere(homePosAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemFloorTris = new ToolStripMenuItem("Add Tracker for Floor Tris");
            setUpClickAction(itemFloorTris, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectFloor(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemWallTris = new ToolStripMenuItem("Add Tracker for Wall Tris");
            setUpClickAction(itemWallTris, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectWall(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCeilingTris = new ToolStripMenuItem("Add Tracker for Ceiling Tris");
            setUpClickAction(itemCeilingTris, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectCeiling(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemGraphicsTris = new ToolStripMenuItem("Add Tracker for Gfx Tris");
            setUpClickAction(itemGraphicsTris, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectGraphicsTriangles(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemMarioFacingArrow = new ToolStripMenuItem("Add Tracker for Mario Facing Arrow");
            setUpClickAction(itemMarioFacingArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioFacingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemMarioMovingArrow = new ToolStripMenuItem("Add Tracker for Mario Moving Arrow");
            setUpClickAction(itemMarioMovingArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioMovingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemMarioIntendedArrow = new ToolStripMenuItem("Add Tracker for Mario Intended Arrow");
            setUpClickAction(itemMarioIntendedArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioIntendedArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemMarioSpeedArrow = new ToolStripMenuItem("Add Tracker for Mario Speed Arrow");
            setUpClickAction(itemMarioSpeedArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioSpeedArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemMarioSlidingArrow = new ToolStripMenuItem("Add Tracker for Mario Sliding Arrow");
            setUpClickAction(itemMarioSlidingArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioSlidingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemMarioTwirlArrow = new ToolStripMenuItem("Add Tracker for Mario Twirl Arrow");
            setUpClickAction(itemMarioTwirlArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioTwirlArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemMarioFloorArrow = new ToolStripMenuItem("Add Tracker for Mario Floor Arrow");
            setUpClickAction(itemMarioFloorArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectMarioFloorArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemObjectFacingArrow = new ToolStripMenuItem("Add Tracker for Object Facing Arrow");
            setUpClickAction(itemObjectFacingArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectFacingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemObjectMovingArrow = new ToolStripMenuItem("Add Tracker for Object Moving Arrow");
            setUpClickAction(itemObjectMovingArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectMovingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemObjectSpeedArrow = new ToolStripMenuItem("Add Tracker for Object Speed Arrow");
            setUpClickAction(itemObjectSpeedArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectSpeedArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemObjectGraphicsArrow = new ToolStripMenuItem("Add Tracker for Object Graphics Arrow");
            setUpClickAction(itemObjectGraphicsArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectGraphicsArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemObjectAngleToMarioArrow = new ToolStripMenuItem("Add Tracker for Object Angle to Mario Arrow");
            setUpClickAction(itemObjectAngleToMarioArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectAngleToMarioArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemObjectTargetArrow = new ToolStripMenuItem("Add Tracker for Object Target Arrow");
            setUpClickAction(itemObjectTargetArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectTargetArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemObjectCustomArrow = new ToolStripMenuItem("Add Tracker for Object Custom Arrow");
            setUpClickAction(itemObjectCustomArrow, mapObjectList =>
            {
                string yawOffsetString = DialogUtilities.GetStringFromDialog(labelText: "Enter the offset (in hex) of the yaw variable in the object struct:");
                if (yawOffsetString == null) return new List<MapObject>();
                uint yawOffset = ParsingUtilities.ParseHexNullable(yawOffsetString) ?? 0;
                string numBytesString = DialogUtilities.GetStringFromDialog(labelText: "Enter the number of bytes of the yaw variable:");
                if (numBytesString == null) return new List<MapObject>();
                int numBytes = ParsingUtilities.ParseInt(numBytesString);
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectObjectCustomArrow(posAngle, yawOffset, numBytes);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemSwooperTargetArrow = new ToolStripMenuItem("Add Tracker for Swooper Effective Target Arrow");
            setUpClickAction(itemSwooperTargetArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectSwooperEffectiveTargetArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemScuttlebugLungingArrow = new ToolStripMenuItem("Add Tracker for Scuttlebug Lunging Arrow");
            setUpClickAction(itemScuttlebugLungingArrow, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectScuttlebugLungingArrow(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCustomPositionAngleArrow = new ToolStripMenuItem("Add Tracker for Custom PositionAngle Arrow");
            setUpClickAction(itemCustomPositionAngleArrow, mapObjectList =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter a PositionAngle.");
                PositionAngle anglePA = PositionAngle.FromString(text);
                if (anglePA == null) return new List<MapObject>();
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posPA = mapObj.GetPositionAngle();
                    if (posPA == null) return null;
                    return (MapObject)new MapObjectCustomPositionAngleArrow(posPA, anglePA);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCurrentUnit = new ToolStripMenuItem("Add Tracker for Current Unit");
            setUpClickAction(itemCurrentUnit, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectCurrentUnit(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCurrentCell = new ToolStripMenuItem("Add Tracker for Current Cell");
            setUpClickAction(itemCurrentCell, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectCurrentCell(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemAngleRange = new ToolStripMenuItem("Add Tracker for Angle Range");
            setUpClickAction(itemAngleRange, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectAngleRange(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemSector = new ToolStripMenuItem("Add Tracker for Sector");
            setUpClickAction(itemSector, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectSector(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemFacingDivider = new ToolStripMenuItem("Add Tracker for Facing Divider");
            setUpClickAction(itemFacingDivider, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectFacingDivider(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemHomeLine = new ToolStripMenuItem("Add Tracker for Home Line");
            setUpClickAction(itemHomeLine, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectHomeLine(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemRenderTable = new ToolStripMenuItem("Add Tracker for Render Table");
            setUpClickAction(itemRenderTable, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectRenderTable(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemPath = new ToolStripMenuItem("Add Tracker for Path");
            setUpClickAction(itemPath, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectPath(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemBranchPath = new ToolStripMenuItem("Add Tracker for Branch Path");
            setUpClickAction(itemBranchPath, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectBranchPath(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemOffsetPositionAngle = new ToolStripMenuItem("Add Tracker for Offset PositionAngle");
            setUpClickAction(itemOffsetPositionAngle, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (MapObject)new MapObjectOffsetPositionAngle(
                        PositionAngle.Offset(100, 0, false, posAngle));
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemCoffinBox = new ToolStripMenuItem("Add Tracker for Coffin Box");
            setUpClickAction(itemCoffinBox, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectCoffinBox(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemFlyGuyZoneDividers = new ToolStripMenuItem("Add Tracker for Fly Guy Zone Dividers");
            setUpClickAction(itemFlyGuyZoneDividers, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectFlyGuyZoneDividers(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

            ToolStripMenuItem itemPyramidPlatformNormals = new ToolStripMenuItem("Add Tracker for Pyramid Platform Normals");
            setUpClickAction(itemPyramidPlatformNormals, mapObjectList =>
            {
                return mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (MapObject)new MapObjectPyramidPlatformNormals(posAngle);
                }).FindAll(mapObj => mapObj != null);
            });

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
                    List<MapTracker> mapTrackers = new List<MapTracker>();

                    MapTracker mapObjHome = new MapTracker(new MapObjectHome(chuckyaPosAngle));
                    mapTrackers.Add(mapObjHome);

                    MapTracker mapObjFacingArrow = new MapTracker(new MapObjectObjectFacingArrow(chuckyaPosAngle));
                    mapObjFacingArrow.SetLineColor(Color.Green);
                    mapObjFacingArrow.SetSize(3000);
                    mapTrackers.Add(mapObjFacingArrow);

                    MapTracker mapObjEffectiveHitboxCylinder = new MapTracker(new MapObjectEffectiveHitboxCylinder(chuckyaPosAngle));
                    mapTrackers.Add(mapObjEffectiveHitboxCylinder);

                    MapTracker mapObjGrabSector = new MapTracker(new MapObjectSector(chuckyaPosAngle));
                    mapObjGrabSector.SetSize(187);
                    mapObjGrabSector.ApplySettings(new MapObjectSettings(changeSectorAngleRadius: true, newSectorAngleRadius: 10912));
                    mapTrackers.Add(mapObjGrabSector);

                    MapTracker mapObjNoticeSector = new MapTracker(new MapObjectSector(chuckyaPosAngle));
                    mapObjNoticeSector.SetSize(3000);
                    mapTrackers.Add(mapObjNoticeSector);

                    MapTracker mapObjFacingDivider = new MapTracker(new MapObjectFacingDivider(chuckyaPosAngle));
                    mapObjFacingDivider.SetSize(3000);
                    mapTrackers.Add(mapObjFacingDivider);

                    MapTracker mapObjSphere = new MapTracker(new MapObjectDrawDistanceSphere(chuckyaPosAngle));
                    mapTrackers.Add(mapObjSphere);

                    MapTracker mapObjCylinder1 = new MapTracker(new MapObjectCustomCylinder(homePosAngle));
                    mapObjCylinder1.SetColor(Color.Cyan);
                    mapObjCylinder1.SetSize(1900);
                    mapObjCylinder1.ApplySettings(new MapObjectSettings(changeCustomCylinderRelativeMinY: true, newCustomCylinderRelativeMinY: -5000));
                    mapTrackers.Add(mapObjCylinder1);

                    MapTracker mapObjCylinder2 = new MapTracker(new MapObjectCustomCylinder(homePosAngle));
                    mapObjCylinder2.SetSize(500);
                    mapTrackers.Add(mapObjCylinder2);

                    MapTracker mapObjCylinder3 = new MapTracker(new MapObjectCustomCylinder(homePosAngle));
                    mapObjCylinder3.SetSize(2000);
                    mapTrackers.Add(mapObjCylinder3);

                    foreach (MapTracker mapTracker in mapTrackers)
                    {
                        mapTracker.SetOrderType(MapTrackerOrderType.OrderOnBottom);
                        Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(mapTracker);
                    }
                }
            };

            ToolStripMenuItem itemFlyGuyMapObjects = new ToolStripMenuItem("Add Trackers for Fly Guy Map Objects");
            itemFlyGuyMapObjects.Click += (sender, e) =>
            {
                foreach (MapObject mapObj in _mapObjectList)
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (!posAngle.IsObjectOrMario()) continue;
                    uint address = posAngle.GetObjAddress();
                    PositionAngle objPosAngle = PositionAngle.Obj(address);
                    PositionAngle homePosAngle = PositionAngle.ObjHome(address);
                    List<MapTracker> mapTrackers = new List<MapTracker>();

                    MapTracker effectiveHitboxHurtboxCylinder = new MapTracker(new MapObjectEffectiveHitboxHurtboxCylinder(objPosAngle));
                    mapTrackers.Add(effectiveHitboxHurtboxCylinder);

                    MapTracker movingArrow = new MapTracker(new MapObjectObjectMovingArrow(objPosAngle));
                    movingArrow.SetSize(10_000);
                    movingArrow.SetLineWidth(2);
                    mapTrackers.Add(movingArrow);

                    MapTracker homeLine = new MapTracker(new MapObjectHomeLine(objPosAngle));
                    homeLine.SetLineWidth(2);
                    mapTrackers.Add(homeLine);

                    MapTracker sphere400 = new MapTracker(new MapObjectCustomSphere(objPosAngle));
                    sphere400.SetSize(400);
                    sphere400.SetOpacity(20);
                    sphere400.SetColor(Color.Cyan);
                    mapTrackers.Add(sphere400);

                    MapTracker sphere2000 = new MapTracker(new MapObjectCustomSphere(objPosAngle));
                    sphere2000.SetSize(2000);
                    sphere2000.SetOpacity(20);
                    sphere2000.SetColor(Color.FromArgb(0, 255, 0));
                    mapTrackers.Add(sphere2000);

                    MapTracker drawDistanceSphere = new MapTracker(new MapObjectDrawDistanceSphere(objPosAngle));
                    drawDistanceSphere.SetOpacity(20);
                    drawDistanceSphere.SetColor(Color.Red);
                    mapTrackers.Add(drawDistanceSphere);

                    MapTracker drawDistanceSphere2 = new MapTracker(new MapObjectDrawDistanceSphere(objPosAngle));
                    drawDistanceSphere2.SetOpacity(20);
                    drawDistanceSphere2.SetColor(Color.Yellow);
                    drawDistanceSphere2.ApplySettings(new MapObjectSettings(changeUseCrossSection: true, newUseCrossSection: false));
                    mapTrackers.Add(drawDistanceSphere2);

                    MapTracker homeSphere = new MapTracker(new MapObjectCustomSphere(homePosAngle));
                    homeSphere.SetSize(2000);
                    homeSphere.SetOpacity(20);
                    homeSphere.SetColor(Color.Pink);
                    mapTrackers.Add(homeSphere);

                    MapTracker zoneDividers = new MapTracker(new MapObjectFlyGuyZoneDividers(objPosAngle));
                    mapTrackers.Add(zoneDividers);

                    foreach (MapTracker mapTracker in mapTrackers)
                    {
                        mapTracker.SetOrderType(MapTrackerOrderType.OrderOnBottom);
                        Config.MapGui.flowLayoutPanelMapTrackers.AddNewControl(mapTracker);
                    }
                }
            };

            ToolStripMenuItem itemCylinder = new ToolStripMenuItem("Cylinder...");
            itemCylinder.DropDownItems.Add(itemHitboxCylinder);
            itemCylinder.DropDownItems.Add(itemEffectiveHitboxCylinder);
            itemCylinder.DropDownItems.Add(itemHurtboxCylinder);
            itemCylinder.DropDownItems.Add(itemEffectiveHurtboxCylinder);
            itemCylinder.DropDownItems.Add(itemEffectiveHitboxHurtboxCylinder);
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
            itemTriangles.DropDownItems.Add(itemFloorTris);
            itemTriangles.DropDownItems.Add(itemWallTris);
            itemTriangles.DropDownItems.Add(itemCeilingTris);
            itemTriangles.DropDownItems.Add(itemGraphicsTris);

            ToolStripMenuItem itemArrow = new ToolStripMenuItem("Arrow...");
            itemArrow.DropDownItems.Add(itemMarioFacingArrow);
            itemArrow.DropDownItems.Add(itemMarioMovingArrow);
            itemArrow.DropDownItems.Add(itemMarioIntendedArrow);
            itemArrow.DropDownItems.Add(itemMarioSpeedArrow);
            itemArrow.DropDownItems.Add(itemMarioSlidingArrow);
            itemArrow.DropDownItems.Add(itemMarioTwirlArrow);
            itemArrow.DropDownItems.Add(itemMarioFloorArrow);
            itemArrow.DropDownItems.Add(new ToolStripSeparator());
            itemArrow.DropDownItems.Add(itemObjectFacingArrow);
            itemArrow.DropDownItems.Add(itemObjectMovingArrow);
            itemArrow.DropDownItems.Add(itemObjectSpeedArrow);
            itemArrow.DropDownItems.Add(itemObjectGraphicsArrow);
            itemArrow.DropDownItems.Add(itemObjectAngleToMarioArrow);
            itemArrow.DropDownItems.Add(itemObjectTargetArrow);
            itemArrow.DropDownItems.Add(itemObjectCustomArrow);
            itemArrow.DropDownItems.Add(new ToolStripSeparator());
            itemArrow.DropDownItems.Add(itemSwooperTargetArrow);
            itemArrow.DropDownItems.Add(itemScuttlebugLungingArrow);
            itemArrow.DropDownItems.Add(new ToolStripSeparator());
            itemArrow.DropDownItems.Add(itemCustomPositionAngleArrow);

            ToolStripMenuItem itemMisc = new ToolStripMenuItem("Misc...");
            itemMisc.DropDownItems.Add(itemCurrentUnit);
            itemMisc.DropDownItems.Add(itemCurrentCell);
            itemMisc.DropDownItems.Add(itemAngleRange);
            itemMisc.DropDownItems.Add(itemSector);
            itemMisc.DropDownItems.Add(itemFacingDivider);
            itemMisc.DropDownItems.Add(itemHomeLine);
            itemMisc.DropDownItems.Add(itemRenderTable);
            itemMisc.DropDownItems.Add(itemPath);
            itemMisc.DropDownItems.Add(itemBranchPath);
            itemMisc.DropDownItems.Add(itemOffsetPositionAngle);
            
            ToolStripMenuItem itemObjectSpecific = new ToolStripMenuItem("Object Specific...");
            itemObjectSpecific.DropDownItems.Add(itemCoffinBox);
            itemObjectSpecific.DropDownItems.Add(itemFlyGuyZoneDividers);
            itemObjectSpecific.DropDownItems.Add(itemPyramidPlatformNormals);

            ToolStripMenuItem itemPreset = new ToolStripMenuItem("Preset...");
            itemPreset.DropDownItems.Add(itemChuckyaMapObjects);
            itemPreset.DropDownItems.Add(itemFlyGuyMapObjects);

            pictureBoxPlus.ContextMenuStrip = new ContextMenuStrip();
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemCylinder);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemSphere);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemHomeContainer);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemTriangles);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemArrow);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemMisc);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemObjectSpecific);
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemPreset);

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
            foreach (MapObject mo in _mapObjectList)
            {
                if (mo == mapObject) return true;
                if (mo is MapObjectAllMapObjectsWithName mo2)
                {
                    if (mo2.ContainsMapObject(mapObject))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<T> GetMapPathObjectsOfType<T>()
        {
            List<T> output = new List<T>();
            foreach (MapObject mapObj in _mapObjectList)
            {
                object obj = mapObj;
                if (obj is T t) output.Add(t);
            }
            return output;
        }

        public void ApplySettings(MapObjectSettings settings)
        {
            _mapObjectList.ForEach(mapObj => mapObj.ApplySettings(settings));
        }

        public List<MapObject> GetMapObjectsToDisplay(MapType mapType)
        {
            if (!_isVisible) return new List<MapObject>();

            if (mapType == MapType.Map2DTopDown && !_isVisibleFor2DTopDown) return new List<MapObject>();
            if (mapType == MapType.Map2DOrthographic && !_isVisibleFor2DOrthographic) return new List<MapObject>();
            if (mapType == MapType.Map3D && !_isVisibleFor3D) return new List<MapObject>();

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
        public void SetLineWidth(float? lineWidthNullable)
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

        public void SetCustomName(string name)
        {
            textBoxName.Text = name;
            _customName = name;
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

        
        public void SetUseRelativeCoordinates(bool? useRelativeCoordinatesNullable)
        {
            bool updateMapObjs = useRelativeCoordinatesNullable != null;
            bool useRelativeCoordinates = useRelativeCoordinatesNullable ?? _mapObjectList[0].UseRelativeCoordinates;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.UseRelativeCoordinates = useRelativeCoordinates);
            }
            checkBoxUseRelativeCoordinates.Checked = useRelativeCoordinates;
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
            textBoxName.SubmitTextLoosely(TrackerName);

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

        public void NotifyMouseEvent(MouseEvent mouseEvent, bool isLeftButton, int mouseX, int mouseY, GLControl control)
        {
            foreach (MapObject mapObj in _mapObjectList)
            {
                mapObj.NotifyMouseEvent(mouseEvent, isLeftButton, mouseX, mouseY, control);
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
            xElement.Add(new XAttribute("useRelativeCoordinates", _mapObjectList[0].UseRelativeCoordinates));
            xElement.Add(new XAttribute("isVisible", _isVisible));
            xElement.Add(new XAttribute("isVisibleFor2DTopDown", _isVisibleFor2DTopDown));
            xElement.Add(new XAttribute("isVisibleFor2DOrthographic", _isVisibleFor2DOrthographic));
            xElement.Add(new XAttribute("isVisibleFor3D", _isVisibleFor3D));
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
            MapTrackerIconType iconType = (MapTrackerIconType)Enum.Parse(typeof(MapTrackerIconType), xElement.Attribute(XName.Get("iconType"))?.Value ?? MapTrackerIconType.TopDownImage.ToString());
            List<string> paths = null;
            if (iconType == MapTrackerIconType.CustomImage)
            {
                paths = xElement.Attribute(XName.Get("paths"))?.Value?.Split('|')?.ToList();
            }
            tracker.SetIconType(iconType, paths);
            string customName = xElement.Attribute(XName.Get("customName"))?.Value;
            if (customName != null)
            {
                tracker._customName = customName;
            }
            tracker.SetSize(ParsingUtilities.ParseFloatNullable(xElement.Attribute(XName.Get("size"))?.Value ?? (25).ToString()));
            tracker.SetOpacity(ParsingUtilities.ParseIntNullable(xElement.Attribute(XName.Get("opacity"))?.Value ?? (50).ToString()));
            tracker.SetLineWidth(ParsingUtilities.ParseFloatNullable(xElement.Attribute(XName.Get("lineWidth"))?.Value ?? (1).ToString()));
            tracker.comboBoxOrderType.SelectedItem = Enum.Parse(typeof(MapTrackerOrderType), xElement.Attribute(XName.Get("orderType"))?.Value ?? MapTrackerOrderType.OrderByY.ToString());
            tracker.comboBoxVisibilityType.SelectedItem = Enum.Parse(typeof(MapTrackerVisibilityType), xElement.Attribute(XName.Get("visibilityType"))?.Value ?? MapTrackerVisibilityType.VisibleWhenLoaded.ToString());
            tracker.SetColor(ColorUtilities.GetColorFromString(xElement.Attribute(XName.Get("color"))?.Value ?? ColorUtilities.ConvertColorToParams(SystemColors.Control)));
            tracker.SetLineColor(ColorUtilities.GetColorFromString(xElement.Attribute(XName.Get("lineColor"))?.Value ?? ColorUtilities.ConvertColorToParams(Color.Black)));
            bool? customRotates = ParsingUtilities.ParseBoolNullable(xElement.Attribute(XName.Get("customRotates"))?.Value);
            if (customRotates.HasValue)
            {
                tracker.SetCustomRotates(customRotates.Value);
            }
            tracker.SetScales(ParsingUtilities.ParseBool(xElement.Attribute(XName.Get("scales"))?.Value ?? false.ToString()));
            tracker.SetUseRelativeCoordinates(ParsingUtilities.ParseBool(xElement.Attribute(XName.Get("useRelativeCoordinates"))?.Value ?? false.ToString()));
            tracker.SetIsVisible(ParsingUtilities.ParseBool(xElement.Attribute(XName.Get("isVisible"))?.Value ?? true.ToString()));
            tracker.SetMapTypeVisibility(MapType.Map2DTopDown, ParsingUtilities.ParseBool(xElement.Attribute(XName.Get("isVisibleFor2DTopDown"))?.Value ?? true.ToString()));
            tracker.SetMapTypeVisibility(MapType.Map2DOrthographic, ParsingUtilities.ParseBool(xElement.Attribute(XName.Get("isVisibleFor2DOrthographic"))?.Value ?? true.ToString()));
            tracker.SetMapTypeVisibility(MapType.Map3D, ParsingUtilities.ParseBool(xElement.Attribute(XName.Get("isVisibleFor3D"))?.Value ?? true.ToString()));
            return tracker;
        }

        public List<T> GetAllMapObjectsOfType<T>() where T : MapObject
        {
            return _mapObjectList.FindAll(mapObj => mapObj is T).ConvertAll(mapObj => (T)mapObj);
        }
    }
}
