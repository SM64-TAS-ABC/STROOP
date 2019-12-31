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
using STROOP.Models;
using OpenTK.Input;
using STROOP.Map3.Map.Graphics;
using STROOP.Map3.Map.Graphics.Items;

namespace STROOP.Map3.Map
{
    public class Map4Controller
    {

        public enum MapCameraMode { TopDown, Fly, Game, }
        public enum MapScaleMode { CourseDefault, MaxCourseSize, Custom };
        public enum MapCenterMode { BestFit, Origin, Custom };

        public MapCameraMode CameraMode { get; set; } = MapCameraMode.Game;
        public MapScaleMode ScaleMode { get; set; } = MapScaleMode.CourseDefault;
        public MapCenterMode CenterMode { get; set; } = MapCenterMode.BestFit;

        public float MapAngle;
        public float MapScale;
        public PointF MapCenter;

        List<Map4Object> _mapObjects = new List<Map4Object>();
        Map4Graphics _graphics;
        Map4Camera _perspectiveCamera;

        public Map4Controller(Map4Graphics graphics)
        {
            _graphics = graphics;
            _perspectiveCamera = new Map4Camera(graphics);
        }
        
        public void AddMapObject(Map4Object mapObj)
        {
            _mapObjects.Add(mapObj);
            foreach (Map4GraphicsItem graphicsItem in mapObj.GraphicsItems)
                _graphics.AddMapItem(graphicsItem);
        }

        public void RemoveMapObject(Map4Object mapObj)
        {
            _mapObjects.Remove(mapObj);
            foreach (Map4GraphicsItem graphicsItem in mapObj.GraphicsItems)
                _graphics.RemoveMapObject(graphicsItem);
        }

        public void Update()
        {
            foreach (Map4Object obj in _mapObjects)
                obj.Update();

            UpdateCamera();

            _graphics.Invalidate();
        }

        public void UpdateCamera()
        {
            _graphics.Camera = _perspectiveCamera;
            CameraGameUpdate();
        }

        public void CameraGameUpdate()
        {
            void updateCameraAngles()
            {
                SpecialConfig.Map3DCameraYaw = (float)MoreMath.AngleTo_AngleUnits(
                    SpecialConfig.Map3DCameraX, SpecialConfig.Map3DCameraZ, SpecialConfig.Map3DFocusX, SpecialConfig.Map3DFocusZ);
                SpecialConfig.Map3DCameraPitch = (float)MoreMath.GetPitch(
                    SpecialConfig.Map3DCameraX, SpecialConfig.Map3DCameraY, SpecialConfig.Map3DCameraZ,
                    SpecialConfig.Map3DFocusX, SpecialConfig.Map3DFocusY, SpecialConfig.Map3DFocusZ);
            }

            if (!SpecialConfig.Map3DCameraPosPA.IsNone())
            {
                SpecialConfig.Map3DCameraX = (float)SpecialConfig.Map3DCameraPosPA.X;
                SpecialConfig.Map3DCameraY = (float)SpecialConfig.Map3DCameraPosPA.Y;
                SpecialConfig.Map3DCameraZ = (float)SpecialConfig.Map3DCameraPosPA.Z;
            }
            if (!SpecialConfig.Map3DCameraAnglePA.IsNone())
            {
                SpecialConfig.Map3DCameraYaw = (float)SpecialConfig.Map3DCameraAnglePA.Angle;
            }
            if (!SpecialConfig.Map3DFocusPosPA.IsNone())
            {
                SpecialConfig.Map3DFocusX = (float)SpecialConfig.Map3DFocusPosPA.X;
                SpecialConfig.Map3DFocusY = (float)SpecialConfig.Map3DFocusPosPA.Y;
                SpecialConfig.Map3DFocusZ = (float)SpecialConfig.Map3DFocusPosPA.Z;
            }

            switch (SpecialConfig.Map3DMode)
            {
                case Map3DMode.InGame:
                    SpecialConfig.Map3DCameraX = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.XOffset);
                    SpecialConfig.Map3DCameraY = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.YOffset);
                    SpecialConfig.Map3DCameraZ = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.ZOffset);
                    SpecialConfig.Map3DCameraYaw = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    SpecialConfig.Map3DCameraPitch = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingPitchOffset);
                    SpecialConfig.Map3DCameraRoll = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingRollOffset);
                    SpecialConfig.Map3DFocusX = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusXOffset);
                    SpecialConfig.Map3DFocusY = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusYOffset);
                    SpecialConfig.Map3DFocusZ = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusZOffset);
                    SpecialConfig.Map3DFOV = Config.Stream.GetSingle(CameraConfig.FOVAddress);
                    break;
                case Map3DMode.CameraPosAndFocus:
                    updateCameraAngles();
                    break;
                case Map3DMode.CameraPosAndAngle:
                    // do nothing, as we use whatever vars are stored
                    break;
                case Map3DMode.FollowFocusRelativeAngle:
                    double angleOffset = SpecialConfig.Map3DFocusAnglePA.IsNone() ? 0 : SpecialConfig.Map3DFocusAnglePA.Angle;
                    (SpecialConfig.Map3DCameraX, SpecialConfig.Map3DCameraZ) =
                        ((float, float))MoreMath.AddVectorToPoint(
                            SpecialConfig.Map3DFollowingRadius,
                            MoreMath.ReverseAngle(SpecialConfig.Map3DFollowingYaw + angleOffset),
                            SpecialConfig.Map3DFocusX,
                            SpecialConfig.Map3DFocusZ);
                    SpecialConfig.Map3DCameraY = SpecialConfig.Map3DFocusY + SpecialConfig.Map3DFollowingYOffset;
                    updateCameraAngles();
                    break;
                case Map3DMode.FollowFocusAbsoluteAngle:
                    (SpecialConfig.Map3DCameraX, SpecialConfig.Map3DCameraZ) =
                        ((float, float))MoreMath.AddVectorToPoint(
                            SpecialConfig.Map3DFollowingRadius,
                            MoreMath.ReverseAngle(SpecialConfig.Map3DFollowingYaw),
                            SpecialConfig.Map3DFocusX,
                            SpecialConfig.Map3DFocusZ);
                    SpecialConfig.Map3DCameraY = SpecialConfig.Map3DFocusY + SpecialConfig.Map3DFollowingYOffset;
                    updateCameraAngles();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _perspectiveCamera.Position = new Vector3(SpecialConfig.Map3DCameraX, SpecialConfig.Map3DCameraY, SpecialConfig.Map3DCameraZ);
            _perspectiveCamera.SetRotation(
                (float)MoreMath.AngleUnitsToRadians(SpecialConfig.Map3DCameraYaw), 
                (float)MoreMath.AngleUnitsToRadians(SpecialConfig.Map3DCameraPitch),
                (float)MoreMath.AngleUnitsToRadians(SpecialConfig.Map3DCameraRoll));
            _perspectiveCamera.FOV = SpecialConfig.Map3DFOV / 180 * (float) Math.PI;
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
