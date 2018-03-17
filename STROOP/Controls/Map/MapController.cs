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
using STROOP.Controls.Map.Graphics.Items;
using STROOP.Models;

namespace STROOP.Controls.Map
{
    public class MapController
    {
        public enum MapCameraMode { TopDown, Fly, Game, }
        public enum MapScaleMode { CourseDefault, MaxCourseSize, Custom };
        public enum MapCenterMode { BestFit, Origin, Custom };

        public MapCameraMode CameraMode { get; set; } = MapCameraMode.TopDown;
        public MapScaleMode ScaleMode { get; set; } = MapScaleMode.CourseDefault;
        public MapCenterMode CenterMode { get; set; } = MapCenterMode.BestFit;

        //public float MapAngle = 0;


        List<MapObject> _mapObjects = new List<MapObject>();
        MapGraphics _graphics;
        MapCameraTopView _topCamera;
        MapPerspectiveCamera _perspectiveCamera;

        public MapController(MapGraphics graphics)
        {
            _graphics = graphics;
            _topCamera = new MapCameraTopView(graphics);
            _perspectiveCamera = new MapPerspectiveCamera(graphics);
        }
        
        public void AddMapObject(MapObject mapObj)
        {
            _mapObjects.Add(mapObj);
            foreach (MapGraphicsItem graphicsItem in mapObj.GraphicsItems)
                _graphics.AddMapItem(graphicsItem);
        }

        public void RemoveMapObject(MapObject mapObj)
        {
            _mapObjects.Remove(mapObj);
            foreach (MapGraphicsItem graphicsItem in mapObj.GraphicsItems)
                _graphics.RemoveMapObject(graphicsItem);
        }

        public void Update()
        {
            foreach (MapObject obj in _mapObjects)
                obj.Update();

            UpdateCamera();

            _graphics.Invalidate();
        }

        public void UpdateCamera()
        {
            IMapCamera camera = null;

            // Select camera type
            switch (CameraMode)
            {
                case MapCameraMode.TopDown:
                    camera = _topCamera;
                    break;

                case MapCameraMode.Fly:
                    camera = _perspectiveCamera;
                    break;

                case MapCameraMode.Game:
                    camera = _perspectiveCamera;
                    break;
            }

            // Update camera
            if (_graphics.Camera != camera)
                _graphics.Camera = camera;

            switch (CameraMode)
            {
                case MapCameraMode.TopDown:
                    CameraTopDownUpdate();
                    break;

                case MapCameraMode.Fly:
                    CameraFlyUpdate();
                    break;

                case MapCameraMode.Game:
                    CameraGameUpdate();
                    break;
            }
        }

        public void CameraGameUpdate()
        {
            _perspectiveCamera.Position = new Vector3(DataModels.Camera.X, DataModels.Camera.Y, DataModels.Camera.Z);
            _perspectiveCamera.SetRotation(
                (float)MoreMath.AngleUnitsToRadians(DataModels.Camera.FacingYaw), 
                (float)MoreMath.AngleUnitsToRadians(DataModels.Camera.FacingPitch),
                (float)MoreMath.AngleUnitsToRadians(DataModels.Camera.FacingRoll));
            //_perspectiveCamera.SetLookTarget(new Vector3(), Vector3.UnitY);
            _perspectiveCamera.FOV = DataModels.Camera.FOV / 180 * (float) Math.PI;
        }

        public void CameraTopDownUpdate() { }

        public void CameraFlyUpdate() { }
    }
}
