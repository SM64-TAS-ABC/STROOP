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
using STROOP.Controls.Map.Objects;
using STROOP.Controls.Map.Semaphores;

namespace STROOP.Map3
{
    public partial class Map3Tracker : UserControl
    {
        private static readonly Image ImageEyeOpen = Properties.Resources.image_eye_open2;
        private static readonly Image ImageEyeClosed = Properties.Resources.image_eye_closed2;

        private readonly List<Map3Object> _mapObjectList;
        private readonly List<Map3Semaphore> _semaphoreList;

        private List<Image> _images;

        private bool _isVisible;
        private MapTrackerVisibilityType _currentVisiblityType;
        private bool _showTriUnits;

        public Map3Tracker(Map3Object mapObj, List<Map3Semaphore> semaphoreList = null)
            : this(new List<Map3Object>() { mapObj }, semaphoreList)
        {
        }

        public Map3Tracker(
            List<Map3Object> mapObjectList,
            List<Map3Semaphore> semaphoreList = null)
        {
            if (mapObjectList.Count < 1) throw new ArgumentOutOfRangeException();
            semaphoreList = semaphoreList ?? new List<Map3Semaphore>();

            InitializeComponent();

            _mapObjectList = new List<Map3Object>(mapObjectList);
            _semaphoreList = new List<Map3Semaphore>(semaphoreList);

            _images = new List<Image>();

            _isVisible = true;
            _currentVisiblityType = MapTrackerVisibilityType.VisibleWhenLoaded;
            _showTriUnits = false;

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
            textBoxOpacity.AddEnterAction(() => textBoxOpacity_EnterAction());
            textBoxOutlineWidth.AddEnterAction(() => textBoxOutlineWidth_EnterAction());
            colorSelector.AddColorChangeAction((Color color) => SetColor(color));
            colorSelectorOutline.AddColorChangeAction((Color color) => SetOutlineColor(color));

            pictureBoxCog.ContextMenuStrip = _mapObjectList[0].GetContextMenuStrip();
            pictureBoxCog.Click += (sender, e) => pictureBoxCog.ContextMenuStrip.Show(Cursor.Position);

            InitializePlusContextMenuStrip();

            UpdateControl();
        }

        private void InitializePlusContextMenuStrip()
        {
            ToolStripMenuItem itemHitboxCylinder = new ToolStripMenuItem("Add Tracker for Hitbox Cylinder");
            itemHitboxCylinder.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (Map3Object)new Map3HitboxCylinderObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemTangibilitySphere = new ToolStripMenuItem("Add Tracker for Tangibility Sphere");
            itemTangibilitySphere.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (Map3Object)new Map3TangibilitySphereObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemDrawDistanceSphere = new ToolStripMenuItem("Add Tracker for Draw Distance Sphere");
            itemDrawDistanceSphere.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObjectOrMario()) return null;
                    return (Map3Object)new Map3DrawDistanceSphereObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomCylinder = new ToolStripMenuItem("Add Tracker for Custom Cylinder");
            itemCustomCylinder.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (Map3Object)new Map3CustomCylinderObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomSphere = new ToolStripMenuItem("Add Tracker for Custom Sphere");
            itemCustomSphere.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    return (Map3Object)new Map3CustomSphereObject(posAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemHome = new ToolStripMenuItem("Add Tracker for Home");
            itemHome.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    return (Map3Object)new Map3HomeObject(posAngle.GetObjAddress());
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomCylinderForHome = new ToolStripMenuItem("Add Tracker for Custom Cylinder for Home");
            itemCustomCylinderForHome.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (Map3Object)new Map3CustomCylinderObject(homePosAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCustomSphereForHome = new ToolStripMenuItem("Add Tracker for Custom Sphere for Home");
            itemCustomSphereForHome.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    PositionAngle homePosAngle = PositionAngle.ObjHome(posAngle.GetObjAddress());
                    return (Map3Object)new Map3CustomSphereObject(homePosAngle);
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemFloorTriangles = new ToolStripMenuItem("Add Tracker for Floor Triangles");
            itemFloorTriangles.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    return (Map3Object)new Map3ObjectFloorObject(posAngle.GetObjAddress());
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemWallTriangles = new ToolStripMenuItem("Add Tracker for Wall Triangles");
            itemWallTriangles.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    return (Map3Object)new Map3ObjectWallObject(posAngle.GetObjAddress());
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            ToolStripMenuItem itemCeilingTriangles = new ToolStripMenuItem("Add Tracker for Ceiling Triangles");
            itemCeilingTriangles.Click += (sender, e) =>
            {
                List<Map3Object> newMapObjs = _mapObjectList.ConvertAll(mapObj =>
                {
                    PositionAngle posAngle = mapObj.GetPositionAngle();
                    if (posAngle == null) return null;
                    if (!posAngle.IsObject()) return null;
                    return (Map3Object)new Map3ObjectCeilingObject(posAngle.GetObjAddress());
                }).FindAll(mapObj => mapObj != null);
                if (newMapObjs.Count == 0) return;
                Map3Tracker tracker = new Map3Tracker(newMapObjs);
                Config.Map3Gui.flowLayoutPanelMap3Trackers.AddNewControl(tracker);
            };

            pictureBoxPlus.ContextMenuStrip = new ContextMenuStrip();
            pictureBoxPlus.ContextMenuStrip.Items.Add(itemHitboxCylinder);
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
            pictureBoxPlus.Click += (sender, e) => pictureBoxPlus.ContextMenuStrip.Show(Cursor.Position);
        }

        public List<Map3Object> GetMapObjectsToDisplay()
        {
            if (!_isVisible) return new List<Map3Object>();
            return _mapObjectList.FindAll(mapObj => mapObj.ShouldDisplay(
                (MapTrackerVisibilityType)comboBoxVisibilityType.SelectedItem));
        }

        public MapTrackerOrderType GetOrderType()
        {
            return (MapTrackerOrderType)comboBoxOrderType.SelectedItem;
        }

        private void trackBarSize_ValueChanged(object sender, EventArgs e)
        {
            SetSize(trackBarSize.Value);
        }

        private void textBoxSize_EnterAction()
        {
            SetSize(ParsingUtilities.ParseFloatNullable(textBoxSize.Text));
        }

        /** null if controls should be refreshed */
        private void SetSize(float? sizeNullable)
        {
            bool updateMapObjs = sizeNullable != null;
            float size = sizeNullable ?? _mapObjectList[0].Size;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.Size = size);
            }
            ControlUtilities.SetTrackBarValueCapped(trackBarSize, size);
            textBoxSize.Text = size.ToString();
        }

        private void trackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            SetOpacity(trackBarOpacity.Value);
        }

        private void textBoxOpacity_EnterAction()
        {
            SetOpacity(ParsingUtilities.ParseIntNullable(textBoxOpacity.Text));
        }

        /** null if controls should be refreshed */
        private void SetOpacity(int? opacityNullable)
        {
            bool updateMapObjs = opacityNullable != null;
            int opacity = opacityNullable ?? _mapObjectList[0].OpacityPercent;
            if (updateMapObjs)
            {
                _mapObjectList.ForEach(mapObj => mapObj.OpacityPercent = opacity);
            }
            ControlUtilities.SetTrackBarValueCapped(trackBarOpacity, opacity);
            textBoxOpacity.Text = opacity.ToString();
        }

        private void trackBarOutlineWidth_ValueChanged(object sender, EventArgs e)
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
            ControlUtilities.SetTrackBarValueCapped(trackBarOutlineWidth, outlineWidth);
            textBoxOutlineWidth.Text = outlineWidth.ToString();
        }

        private void checkBoxRotates_CheckedChanged(object sender, EventArgs e)
        {
            /*
            SetRotates(checkBoxRotates.Checked);
            */
        }

        public void SetRotates(bool? rotates)
        {
            /*
            if (!rotates.HasValue) return;
            checkBoxRotates.Checked = rotates.Value;
            MapObjectList.ForEach(obj =>
            {
                obj.Rotates = rotates.Value;
            });
            */
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

        private void CheckBoxShowTriUnits_CheckedChanged(object sender, EventArgs e)
        {
            _showTriUnits = checkBoxShowTriUnits.Checked;
            _mapObjectList.ForEach(mapObj => mapObj.ShowTriUnits = _showTriUnits);
        }

        private void pictureBoxRedX_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.RemoveControl(this);
        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {
            _isVisible = !_isVisible;
            pictureBoxEye.BackgroundImage = _isVisible ? ImageEyeOpen : ImageEyeClosed;
        }

        private void pictureBoxUpArrow_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.MoveUpControl(this);
        }

        private void pictureBoxDownArrow_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.MoveDownControl(this);
        }

        public void UpdateControl()
        {
            textBoxName.Text = string.Join(", ", _mapObjectList.ConvertAll(obj => obj.GetName()));
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
                    foreach (Map3Object mapObj in _mapObjectList)
                    {
                        mapObj.NotifyStoreBehaviorCritera();
                    }
                }
                _currentVisiblityType = currentVisibilityType;
            }

            if (_semaphoreList.Any(semaphore => !semaphore.IsUsed))
            {
                Config.Map3Gui.flowLayoutPanelMap3Trackers.RemoveControl(this);
            }
        }

        public void CleanUp()
        {
            _semaphoreList.ForEach(semaphore => semaphore.IsUsed = false);
        }

        public override string ToString()
        {
            return string.Join(", ", _mapObjectList);
        }
    }
}
