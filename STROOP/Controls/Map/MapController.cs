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
using OpenTK.Input;

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

        public float MapAngle;
        public float MapScale;
        public PointF MapCenter;

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
                    //CameraFlyUpdate();
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

        public void CameraTopDownUpdate() {
            // Update center
            /* (CenterMode)
            {
                case MapCenterMode.BestFit:
                    break;
                case MapCenterMode.Origin:
                    // Use (0,0) as center
                    MapCenter = new PointF(0.0f, 0.0f);
                    break;
                case MapCenterMode.Custom:
                    // Center updated manually
                    break;
            }

            // Update scale
            switch (ScaleMode)
            {
                case MapScaleMode.CourseDefault:
                    break;
                case MapScaleMode.MaxCourseSize:
                    _graphics.
                    break;
                case MapScaleMode.Custom:
                    // Scale updated manually
                    break;
            }*/
        }

        volatile bool _mousePressedWithin = false;
        private void Control_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _mousePressedWithin = true;
        }

        /*
        Vector2 _pMouseCoords;
        float? _pMouseScroll = null;
        bool _mousePressed = false;
        public void CameraFlyUpdate()
        {
            KeyboardState keyState = Keyboard.GetState();
            bool anyInput = false;

            // Calculate key speed multiplier
            float speedMul = 1f;
            if (keyState.IsKeyDown(Key.ControlLeft) || keyState.IsKeyDown(Key.ControlRight))
                speedMul = 0.0f;
            else if (keyState.IsKeyDown(Key.ShiftLeft) || keyState.IsKeyDown(Key.ShiftRight))
                speedMul = 3.0f;
            else if (keyState.IsKeyDown(Key.AltLeft) || keyState.IsKeyDown(Key.AltRight))
                speedMul = 0.3f;

            // Handle mouse
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == OpenTK.Input.ButtonState.Pressed && _mousePressedWithin)
            {
                // Reset previous coordinates so no movement occurs during the initial press 
                if (!_mousePressed)
                {
                    _pMouseCoords = new Vector2(mouseState.X, mouseState.Y);
                }

                // Calcualte mouse delta
                Vector2 delta = new Vector2(mouseState.X, mouseState.Y) - _pMouseCoords;

                // Add speed multiplier
                delta *= speedMul * 0.009f;
                delta *= _perspectiveCamera.FOV / 90;

                // Trackball (add mouse deltas to angle)
                _cameraManualAngleLat += delta.X;
                _cameraManualAngleLong += -delta.Y;

                if (_cameraManualAngleLong > Math.PI / 2 - 0.001f)
                {
                    _cameraManualAngleLong = (float)(Math.PI / 2) - 0.001f;
                }
                else if (_cameraManualAngleLong < -Math.PI / 2 + 0.001f)
                {
                    _cameraManualAngleLong = (float)(-Math.PI / 2) + 0.001f;
                }

                // Update mouse coordinates for next time
                _pMouseCoords = new Vector2(mouseState.X, mouseState.Y);

                _mousePressed = true;
                anyInput = true;
            }
            else
            {
                if (_mousePressed)
                    _mousePressedWithin = false;
                _mousePressed = false;
            }

            // Don't do anything if we don't have focus
            if (_graphics.Control.Focused)
                return;

            if (!_pMouseScroll.HasValue)
                _pMouseScroll = mouseState.ScrollWheelValue;
            float deltaScroll = mouseState.ScrollWheelValue - _pMouseScroll.Value;
            //_zoom += deltaScroll * 0.1f * speedMul;
            _pMouseScroll = mouseState.ScrollWheelValue;

            Vector3 relDeltaPos = new Vector3(0, 0, 0);
            float posSpeed = speedMul * 5.0f; // Move at a rate relative to the model size

            // Handle Positional Movement 
            if (keyState.IsKeyDown(Key.W) || keyState.IsKeyDown(Key.Up))
            {
                relDeltaPos.Z += posSpeed;
                anyInput = true;
            }
            if (keyState.IsKeyDown(Key.A) || keyState.IsKeyDown(Key.Left))
            {
                relDeltaPos.X += posSpeed;
                anyInput = true;
            }
            if (keyState.IsKeyDown(Key.S) || keyState.IsKeyDown(Key.Down))
            {
                relDeltaPos.Z += -posSpeed;
                anyInput = true;
            }
            if (keyState.IsKeyDown(Key.D) || keyState.IsKeyDown(Key.Right))
            {
                relDeltaPos.X += -posSpeed;
                anyInput = true;
            }
            if (keyState.IsKeyDown(Key.Q))
            {
                relDeltaPos.Y += -posSpeed;
                anyInput = true;
            }
            if (keyState.IsKeyDown(Key.E))
            {
                relDeltaPos.Y += posSpeed;
                anyInput = true;
            }

            // Update camera position
            // This requires converting the coordinate system from the camera coordinates 
            // to the world coordinates. The camera X unit is calculate from the 
            // cross product of the camera Y unit and the camera Z unit. The camera
            // Y unit is the world Y unit since the Y coordinate is always up.
            // The Z unit is the normalized camera look vector (to move towards the look),
            // Hence, move formard.
            _perspectiveCamera.Position += Vector3.Cross(Vector3.UnitY, _cameraLook) * relDeltaPos.X
                + Vector3.UnitY * relDeltaPos.Y
                + _cameraLook * relDeltaPos.Z;
        }
        */
    }
}
