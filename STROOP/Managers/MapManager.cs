using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Utilities;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using STROOP.Structs.Configurations;
using STROOP.Controls.Map.Graphics;
using STROOP.Controls.Map;
using STROOP.Controls.Map.Objects;
using STROOP.Controls.Map.Trackers;

namespace STROOP.Managers
{
    public class MapManager
    {        
        public bool IsLoaded { get; private set; }
        public bool Visible { get => _graphics.Visible; set => _graphics.Visible = value; }

        private MapGui _mapGui;
        private MapController _controller;
        private MapGraphics _graphics;
        private MapAssociations _mapAssoc;



        #region Objects
        private MapMarioObject _mapObjMario = new MapMarioObject();
        private MapHolpObject _mapObjHolp = new MapHolpObject();
        private MapCameraObject _mapObjCamera = new MapCameraObject();
        private MapWallTriObject _mapObjWallTri = new MapWallTriObject();
        private MapFloorTriObject _mapObjFloorTri = new MapFloorTriObject();
        private MapCeilingTriObject _mapObjCeilTri = new MapCeilingTriObject();
        #endregion

        public MapManager(MapAssociations mapAssoc, MapGui mapGui)
        {
            _mapAssoc = mapAssoc;
            _mapGui = mapGui;
        }

        public void Load()
        {
            // Create new graphics control
            _graphics = new MapGraphics(_mapGui.GLControl);
            _graphics.Load();
            _controller = new MapController(_graphics);

            IsLoaded = true;

            /*
            _mapGui.RadioButtonScaleCourseDefault.Click += (sender, e) => _mapScale = MapScale.CourseDefault;
            _mapGui.RadioButtonScaleMaxCourseSize.Click += (sender, e) => _mapScale = MapScale.MaxCourseSize;
            _mapGui.RadioButtonScaleCustom.Click += (sender, e) => _mapScale = MapScale.Custom;

            _mapGui.RadioButtonCenterBestFit.Click += (sender, e) => _mapCenter = MapCenter.BestFit;
            _mapGui.RadioButtonCenterOrigin.Click += (sender, e) => _mapCenter = MapCenter.Origin;
            _mapGui.RadioButtonCenterCustom.Click += (sender, e) => _mapCenter = MapCenter.Custom;

            _mapGui.RadioButtonAngle0.Click += (sender, e) => _mapAngle = MapAngle._0;
            _mapGui.RadioButtonAngle16384.Click += (sender, e) => _mapAngle = MapAngle._16384;
            _mapGui.RadioButtonAngle32768.Click += (sender, e) => _mapAngle = MapAngle._32768;
            _mapGui.RadioButtonAngle49152.Click += (sender, e) => _mapAngle = MapAngle._49152;
            _mapGui.RadioButtonAngleCustom.Click += (sender, e) => _mapAngle = MapAngle.Custom;
            */

            _mapGui.ButtonAdd.Click += (sender, e) => _mapGui.MapTrackerFlowLayoutPanel.AddNewControl(new MapTracker(_mapObjMario));
            _mapGui.ButtonClear.Click += (sender, e) => _mapGui.MapTrackerFlowLayoutPanel.ClearControls();

            // Test
            _controller.AddMapObject(new MapLevelObject(_mapAssoc));
            _controller.AddMapObject(_mapObjMario);
            _controller.AddMapObject(_mapObjHolp);
            _controller.AddMapObject(_mapObjCamera);
            _controller.AddMapObject(_mapObjWallTri);
            _controller.AddMapObject(_mapObjFloorTri);
            _controller.AddMapObject(_mapObjCeilTri);
        }

        public void Update()
        {
            _mapGui.MapTrackerFlowLayoutPanel.UpdateControls();

            // Make sure the control has successfully loaded
            if (!IsLoaded)
                return;

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _controller.Update();

            // Update labels
            /*_mapGui.PuValueLabel.Text = string.Format("[{0}:{1}:{2}]", puX, puY, puZ);
            _mapGui.QpuValueLabel.Text = string.Format("[{0}:{1}:{2}]", qpuX, qpuY, qpuZ);
            _mapGui.MapIdLabel.Text = string.Format("[{0}:{1}:{2}:{3}]", level, area, loadingPoint, missionLayout);
            _mapGui.MapNameLabel.Text = _currentMap.Name;
            _mapGui.MapSubNameLabel.Text = (_currentMap.SubName != null) ? _currentMap.SubName : "";*/
        }
    }
}
