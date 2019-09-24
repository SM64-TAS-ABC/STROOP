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
        Map3Object _backgroundMapObj;
        Map3Object _mapMapObj;
        Map3Object _currentCellMapObj;
        Map3Object _currentUnitMapObj;
        Map3Object _cellGridlinesMapObj;
        Map3Object _unitGridlinesMapObj;
        Map3Object _holpMapObj;
        Map3Object _cameraMapObj;
        Map3Object _marioMapObj;
        Map3Object _floorMapObj;
        Map3Object _ceilingMapObj;
        Map3Object _objMapObj;

        bool _isLoaded = false;

        public Map3Manager()
        {
        }

        public void Load()
        {
            // Create new graphics control
            Config.Map3Graphics = new Map3Graphics();
            Config.Map3Graphics.Load();
            _isLoaded = true;

            _backgroundMapObj = new Map3BackgroundObject();
            _mapMapObj = new Map3MapObject();
            _currentCellMapObj = new Map3CurrentCellObject();
            _currentUnitMapObj = new Map3CurrentUnitObject();
            _cellGridlinesMapObj = new Map3CellGridlinesObject();
            _unitGridlinesMapObj = new Map3UnitGridlinesObject();
            _holpMapObj = new Map3HolpObject();
            _cameraMapObj = new Map3CameraObject();
            _marioMapObj = new Map3MarioObject();
            _floorMapObj = new Map3FloorObject();
            _ceilingMapObj = new Map3CeilingObject();
            _objMapObj = new Map3ObjectObject(0x803408C8);

            // Add map objects
            Config.Map3Graphics.AddMapObject(_backgroundMapObj);
            Config.Map3Graphics.AddMapObject(_mapMapObj);
            Config.Map3Graphics.AddMapObject(_currentCellMapObj);
            Config.Map3Graphics.AddMapObject(_currentUnitMapObj);
            Config.Map3Graphics.AddMapObject(_cellGridlinesMapObj);
            Config.Map3Graphics.AddMapObject(_unitGridlinesMapObj);
            Config.Map3Graphics.AddMapObject(_holpMapObj);
            Config.Map3Graphics.AddMapObject(_cameraMapObj);
            Config.Map3Graphics.AddMapObject(_marioMapObj);
            Config.Map3Graphics.AddMapObject(_floorMapObj);
            Config.Map3Graphics.AddMapObject(_ceilingMapObj);
            Config.Map3Graphics.AddMapObject(_objMapObj);

            InitializeControls();
        }

        private void InitializeControls()
        {
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

            // Buttons for Changing Scale
            Config.Map3Gui.buttonMap3ControllersScaleMinus.Click += (sender, e) =>
                Config.Map3Graphics.ChangeScale(-1, Config.Map3Gui.textBoxMap3ControllersScaleChange.Text);
            Config.Map3Gui.buttonMap3ControllersScalePlus.Click += (sender, e) =>
                Config.Map3Graphics.ChangeScale(1, Config.Map3Gui.textBoxMap3ControllersScaleChange.Text);

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

            // Buttons for Changing Angle
            Config.Map3Gui.buttonMap3ControllersAngleCCW.Click += (sender, e) =>
                Config.Map3Graphics.ChangeAngle(-1, Config.Map3Gui.textBoxMap3ControllersAngleChange.Text);
            Config.Map3Gui.buttonMap3ControllersAngleCW.Click += (sender, e) =>
                Config.Map3Graphics.ChangeAngle(1, Config.Map3Gui.textBoxMap3ControllersAngleChange.Text);

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
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
            if (!_isLoaded) return;

            UpdateData();
            Config.Map3Gui.GLControl.Invalidate();
        }

        private void UpdateData()
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
    }
}
