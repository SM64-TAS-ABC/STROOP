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
        public readonly List<Map3Object> MapObjectList;
        public readonly List<Map3Semaphore> SemaphoreList;

        private static readonly Image ImageEyeOpen = Properties.Resources.image_eye_open2;
        private static readonly Image ImageEyeClosed = Properties.Resources.image_eye_closed2;

        public bool IsVisible;

        public Map3Tracker(
            List<Map3Object> mapObjectList,
            List<Map3Semaphore> semaphoreList)
        {
            if (mapObjectList.Count < 1) throw new ArgumentOutOfRangeException();

            InitializeComponent();

            MapObjectList = new List<Map3Object>(mapObjectList);
            SemaphoreList = new List<Map3Semaphore>(semaphoreList);

            IsVisible = true;

            UpdateControl();

            /*
            MapObjectList.ForEach(obj =>
            {
                obj.Tracker = this;
                obj.Tracked = true;
                obj.Shown = true;
                obj.Opacity = 1;
            });

            UpdateName(MapObjectList.FirstOrDefault()?.Name);
            UpdateImage(MapObjectList.FirstOrDefault()?.BitmapImage);
            UpdateBackColor(MapObjectList.FirstOrDefault()?.BackColor);
            SetRotates(MapObjectList.FirstOrDefault()?.Rotates);
            SetColor(MapObjectList.FirstOrDefault()?.MyColor);

            textBoxOpacity.AddEnterAction(() => textBoxOpacity_EnterAction());
            textBoxSize.AddEnterAction(() => textBoxSize_EnterAction());
            comboBoxVisibilityType.SelectedValueChanged += (sender, e) =>
                SetVisibilityType((MapTrackerVisibilityType)comboBoxVisibilityType.SelectedItem);
            colorSelector.AddColorChangeAction((Color color) => SetColor(color));

            SetSize(MapObjectList.FirstOrDefault()?.DefaultSize);
            SetOpacity(MapObjectList.FirstOrDefault()?.DefaultOpacity, true);
            */
        }

        private void MapTracker_Load(object sender, EventArgs e)
        {
            tableLayoutPanel.BorderWidth = 2;
            tableLayoutPanel.ShowBorder = true;
/*
            comboBoxVisibilityType.DataSource = Enum.GetValues(typeof(MapTrackerVisibilityType));
            comboBoxVisibilityType.SelectedItem = MapTrackerVisibilityType.VisibleWhenLoaded;

            comboBoxOrderType.DataSource = Enum.GetValues(typeof(MapTrackerOrderType));
            comboBoxOrderType.SelectedItem = MapTrackerOrderType.OrderByDepth;

            comboBoxDisplayType.DataSource = Enum.GetValues(typeof(MapTrackerDisplayType));

            ControlUtilities.AddContextMenuStripFunctions(
                pictureBoxCog,
                new List<string>()
                {
                    "Hitbox Cylinder",
                    "Tangibility Radius",
                    "Draw Distance Radius",
                },
                new List<Action>()
                {
                    () => { },
                    () => { },
                    () => { },
                });
            pictureBoxCog.Click += (se, ev) => pictureBoxCog.ContextMenuStrip.Show(Cursor.Position);
            */
        }

        public MapTrackerOrderType GetOrderType()
        {
            return MapTrackerOrderType.OrderByDepth;
            /*
            return (MapTrackerOrderType) comboBoxOrderType.SelectedItem;
            */
        }

        private void trackBarSize_ValueChanged(object sender, EventArgs e)
        {
            /*
            SetSize(trackBarSize.Value);
            */
        }

        private void textBoxSize_EnterAction()
        {
            /*
            SetSize(ParsingUtilities.ParseFloatNullable(textBoxSize.Text));
            */
        }

        // sizeNullable is from 0 to 100, or null if controls should be refreshed
        private void SetSize(float? sizeNullable)
        {
            /*
            float scale = 0.2f;

            float backupValue = 50;
            float? oldValue = MapObjectList.FirstOrDefault()?.Size;
            if (oldValue.HasValue)
            {
                backupValue = oldValue.Value / scale * 100;
            }

            float size = sizeNullable ?? backupValue;
            if (size < 0) size = 0;
            float scaledSize = (size / 100) * scale;
            MapObjectList.ForEach(obj =>
            {
                obj.Size = scaledSize;
            });
            ControlUtilities.SetTrackBarValueCapped(trackBarSize, size);
            textBoxSize.Text = size.ToString();
            */
        }

        private void trackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            /*
            SetOpacity(trackBarOpacity.Value, false);
            */
        }

        private void textBoxOpacity_EnterAction()
        {
            /*
            SetOpacity(ParsingUtilities.ParseFloatNullable(textBoxOpacity.Text), false);
            */
        }

        // opacityNullable is from 0 to 100, or null if controls should be refreshed
        private void SetOpacity(float? opacityNullable, bool scaled)
        {
            /*
            if (opacityNullable.HasValue && scaled)
            {
                opacityNullable = opacityNullable.Value * 100;
            }

            float backupValue = 100;
            float? oldValue = MapObjectList.FirstOrDefault()?.Opacity;
            if (oldValue.HasValue)
            {
                backupValue = oldValue.Value * 100;
            }

            float opacity = opacityNullable ?? backupValue;
            if (opacity < 0) opacity = 0;
            if (opacity > 100) opacity = 100;
            float scaledOpacity = opacity / 100;
            MapObjectList.ForEach(obj =>
            {
                obj.Opacity = scaledOpacity;
            });
            ControlUtilities.SetTrackBarValueCapped(trackBarOpacity, opacity);
            textBoxOpacity.Text = opacity.ToString();
            */
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

        public void SetColor(Color? color)
        {
            /*
            if (!color.HasValue) return;
            colorSelector.SelectedColor = color.Value;
            MapObjectList.ForEach(obj =>
            {
                obj.MyColor = color.Value;
            });
            */
        }

        public void SetVisibilityType(MapTrackerVisibilityType visibilityType)
        {
            /*
            comboBoxVisibilityType.SelectedItem = visibilityType;
            MapObjectList.ForEach(obj =>
            {
                obj.VisibilityType = visibilityType;
            });
            */
        }

        private void pictureBoxRedX_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.RemoveControl(this);
        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {
            IsVisible = !IsVisible;
            pictureBoxEye.BackgroundImage = IsVisible ? ImageEyeOpen : ImageEyeClosed;
        }

        private void pictureBoxUpArrow_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.MoveUpControl(this);
        }

        private void pictureBoxDownArrow_Click(object sender, EventArgs e)
        {
            Config.Map3Gui.flowLayoutPanelMap3Trackers.MoveDownControl(this);
        }

        public void UpdateName(string name)
        {
            /*
            textBoxName.Text = name ?? "(Unknown)";
            */
        }

        public void UpdateImage(Bitmap image)
        {
            /*
            pictureBoxPicture.Image = image == null ? null : new Bitmap(image);
            */
        }

        public void UpdateBackColor(Color? colorNullable)
        {
            /*
            Color color = colorNullable ?? ObjectSlotsConfig.VacantSlotColor;
            panelPicture.BackColor = color;
            pictureBoxPicture.BackColor = color.Lighten(0.7);
            */
        }

        public void UpdateControl()
        {
            textBoxName.Text = string.Join(", ", MapObjectList.ConvertAll(obj => obj.GetName()));
            pictureBoxPicture.Image = MapObjectList[0].GetImage(); // TODO fix this

            if (SemaphoreList.Any(semaphore => !semaphore.IsUsed))
            {
                Config.Map3Gui.flowLayoutPanelMap3Trackers.RemoveControl(this);
            }
        }

        public void CleanUp()
        {
            SemaphoreList.ForEach(semaphore => semaphore.IsUsed = false);
        }
    }
}
