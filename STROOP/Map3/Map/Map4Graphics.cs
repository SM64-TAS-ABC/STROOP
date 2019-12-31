using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using STROOP.Structs;
using System.IO;
using System.Diagnostics;
using STROOP.Structs.Configurations;
using STROOP.Utilities;

namespace STROOP.Map3.Map.Graphics
{
    public class Map4Graphics : IDisposable
    {
        const string VertexShaderPath = @"Resources\Shaders\VertexShader.glsl";
        const string FragmentShaderPath = @"Resources\Shaders\FragmentShader.glsl";
        const string ShaderLogPath = @"Resources\Shaders\ShaderLog.txt";

        public float AspectRatio => Config.Map3Gui.GLControl3D.AspectRatio;
        public float NormalizedWidth => AspectRatio <= 1.0f ? 1.0f : (float)Config.Map3Gui.GLControl3D.Width / Config.Map3Gui.GLControl3D.Height;
        public float NormalizedHeight => AspectRatio >= 1.0f ? 1.0f : (float)Config.Map3Gui.GLControl3D.Height / Config.Map3Gui.GLControl3D.Width;
        public Size Size => Config.Map3Gui.GLControl3D.Size;
        public float Width => Config.Map3Gui.GLControl3D.Width;
        public float Height => Config.Map3Gui.GLControl3D.Height;
        public bool Visible { get => Config.Map3Gui.GLControl3D.Visible; set => Config.Map3Gui.GLControl3D.Visible = value; }

        public event EventHandler OnSizeChanged;

        object _mapItemsLock = new object();

        bool _error = false;

        int _shaderProgram;
        int _vertexShader, _fragmentShader;

        public int GLUniformView;
        int _glAttributePosition = 1;
        int _glAttributeColor = 2;
        int _glAttributeTexCoords = 3;

        public Map4Graphics()
        {
        }

        public void Load()
        {
            Config.Map4Camera = new Map4Camera();

            Config.Map3Gui.GLControl3D.MakeCurrent();
            Config.Map3Gui.GLControl3D.Context.LoadAll();

            CheckVersion();
            if (_error)
                return;

            SetupShaderProgram();
            if (_error)
                return;

            // Setup GL Properties
            GL.ClearColor(Color.FromKnownColor(KnownColor.Control));
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // Set viewport
            GL.Viewport(Config.Map3Gui.GLControl3D.DisplayRectangle);

            Config.Map3Gui.GLControl3D.Paint += OnPaint;
            Config.Map3Gui.GLControl3D.Resize += OnResize;
        }

        public void OnPaint(object sender, EventArgs e)
        {
            UpdateCamera();

            Config.Map3Gui.GLControl3D.MakeCurrent();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.CullFace);

            // Make sure we have a camera
            if (_error || Config.Map4Camera == null)
            {
                Config.Map3Gui.GLControl3D.SwapBuffers();
                return;
            }
            
            // Setup Background
            GL.Disable(EnableCap.DepthTest);

            // Draw background
            Config.Map3Gui.flowLayoutPanelMap3Trackers.DrawOn3DControl(Map3DrawType.Background);

            // Setup 3D
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            // Draw 3D
            Config.Map3Gui.flowLayoutPanelMap3Trackers.DrawOn3DControl(Map3DrawType.Perspective);

            // Setup 2D
            GL.Disable(EnableCap.DepthTest);

            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                Debugger.Break();

            // Draw 2D
            Config.Map3Gui.flowLayoutPanelMap3Trackers.DrawOn3DControl(Map3DrawType.Overlay);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
                Debugger.Break();

            // Disable Attributes
            GL.DisableVertexAttribArray(_glAttributePosition);
            GL.DisableVertexAttribArray(_glAttributeColor);
            GL.DisableVertexAttribArray(_glAttributeTexCoords);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
                Debugger.Break();

            Config.Map3Gui.GLControl3D.SwapBuffers();
        }

        public void BindVertices()
        {
            GL.EnableVertexAttribArray(_glAttributePosition);
            GL.VertexAttribPointer(_glAttributePosition, 3, VertexAttribPointerType.Float, false, Map4Vertex.Size, Map4Vertex.IndexPosition);
            GL.EnableVertexAttribArray(_glAttributeColor);
            GL.VertexAttribPointer(_glAttributeColor, 4, VertexAttribPointerType.Float, false, Map4Vertex.Size, Map4Vertex.IndexColor);
            GL.EnableVertexAttribArray(_glAttributeTexCoords);
            GL.VertexAttribPointer(_glAttributeTexCoords, 2, VertexAttribPointerType.Float, false, Map4Vertex.Size, Map4Vertex.IndexTexCoord);
        }

        void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(Config.Map3Gui.GLControl3D.DisplayRectangle);
            OnSizeChanged?.Invoke(sender, e);
            Invalidate();
        }

        public void Invalidate()
        {
            Config.Map3Gui.GLControl3D.Invalidate();
        }

        private void CheckVersion()
        {
            // Check for necessary capabilities:
            Version version = new Version(GL.GetString(StringName.Version).Substring(0, 3));
            Version target = new Version(2, 0);
            if (version < target)
            {
                MessageBox.Show($"OpenGL {target} is required (you only have {version}).", 
                    "OpenGL unsupported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _error = true;
                return;
            }
        }

        private void SetupShaderProgram()
        {
            // Create shaders 

            _vertexShader = GL.CreateShader(ShaderType.VertexShader);
            string vertexShaderSource = File.ReadAllText(VertexShaderPath);
            GL.ShaderSource(_vertexShader, vertexShaderSource);
            GL.CompileShader(_vertexShader);

            _fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            string fragmentShaderSource = File.ReadAllText(FragmentShaderPath);
            GL.ShaderSource(_fragmentShader, fragmentShaderSource);
            GL.CompileShader(_fragmentShader);

            // Check for errors

            int vertexCompileStatus;
            GL.GetShader(_vertexShader, ShaderParameter.CompileStatus, out vertexCompileStatus);
            string vertexCompileLog = GL.GetShaderInfoLog(_vertexShader);

            int fragmentCompileStatus;
            GL.GetShader(_fragmentShader, ShaderParameter.CompileStatus, out fragmentCompileStatus);
            string fragmentCompileLog = GL.GetShaderInfoLog(_fragmentShader);

            // Show and log any errors

            if (vertexCompileStatus != (int)OpenTK.Graphics.OpenGL.Boolean.True
                || fragmentCompileStatus != (int) OpenTK.Graphics.OpenGL.Boolean.True)
            {
                MessageBox.Show($"Open GL failed to compile. See {ShaderLogPath}", "OpenGL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string logFileContents = $"Vertex Shader: {Environment.NewLine}{vertexCompileLog}{Environment.NewLine}FragmentShader{Environment.NewLine}{fragmentCompileLog}";
                File.WriteAllText(ShaderLogPath, logFileContents);
                _error = true;
                return;
            }

            // Create program
            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, _vertexShader);
            GL.AttachShader(_shaderProgram, _fragmentShader);

            // Bind uniforms + attributes
            GL.BindAttribLocation(_shaderProgram, _glAttributePosition, "position");
            GL.BindAttribLocation(_shaderProgram, _glAttributeColor, "color");
            GL.BindAttribLocation(_shaderProgram, _glAttributeTexCoords, "texCoords");

            // Link program
            GL.LinkProgram(_shaderProgram);
            GL.UseProgram(_shaderProgram);

            // Get uniform locatinos
            GLUniformView = GL.GetUniformLocation(_shaderProgram, "view");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    lock (_mapItemsLock)
                    {
                        GL.DetachShader(_shaderProgram, _vertexShader);
                        GL.DetachShader(_shaderProgram, _fragmentShader);
                        GL.DeleteShader(_vertexShader);
                        GL.DeleteShader(_fragmentShader);
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

        public void UpdateCamera()
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

            Config.Map4Camera.Position = new Vector3(SpecialConfig.Map3DCameraX, SpecialConfig.Map3DCameraY, SpecialConfig.Map3DCameraZ);
            Config.Map4Camera.SetRotation(
                (float)MoreMath.AngleUnitsToRadians(SpecialConfig.Map3DCameraYaw),
                (float)MoreMath.AngleUnitsToRadians(SpecialConfig.Map3DCameraPitch),
                (float)MoreMath.AngleUnitsToRadians(SpecialConfig.Map3DCameraRoll));
            Config.Map4Camera.FOV = SpecialConfig.Map3DFOV / 180 * (float)Math.PI;
        }

        /*
        volatile bool _mousePressedWithin = false;
        private void Control_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _mousePressedWithin = true;
        }
        */

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
