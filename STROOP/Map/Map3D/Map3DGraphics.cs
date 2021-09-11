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

namespace STROOP.Map.Map3D
{
    public class Map3DGraphics : IDisposable
    {
        const string VertexShaderPath = @"Resources\Shaders\VertexShader.glsl";
        const string FragmentShaderPath = @"Resources\Shaders\FragmentShader.glsl";
        const string ShaderLogPath = @"Resources\Shaders\ShaderLog.txt";

        public float AspectRatio => Config.MapGui.GLControlMap3D.AspectRatio;
        public float NormalizedWidth => AspectRatio <= 1.0f ? 1.0f : (float)Config.MapGui.GLControlMap3D.Width / Config.MapGui.GLControlMap3D.Height;
        public float NormalizedHeight => AspectRatio >= 1.0f ? 1.0f : (float)Config.MapGui.GLControlMap3D.Height / Config.MapGui.GLControlMap3D.Width;
        public Size Size => Config.MapGui.GLControlMap3D.Size;
        public float Width => Config.MapGui.GLControlMap3D.Width;
        public float Height => Config.MapGui.GLControlMap3D.Height;
        public bool Visible { get => Config.MapGui.GLControlMap3D.Visible; set => Config.MapGui.GLControlMap3D.Visible = value; }

        public event EventHandler OnSizeChanged;

        object _mapItemsLock = new object();

        bool _error = false;

        int _shaderProgram;
        int _vertexShader, _fragmentShader;

        public int GLUniformView;
        public int GLUnifromInvfarplanecoef;
        int _glAttributePosition = 1;
        int _glAttributeColor = 2;
        int _glAttributeTexCoords = 3;

        public Map3DGraphics()
        {
        }

        public void Load()
        {
            Config.Map3DCamera = new Map3DCamera();

            Config.MapGui.GLControlMap3D.MakeCurrent();
            Config.MapGui.GLControlMap3D.Context.LoadAll();

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
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Set viewport
            GL.Viewport(Config.MapGui.GLControlMap3D.DisplayRectangle);

            Config.MapGui.GLControlMap3D.Paint += OnPaint;
            Config.MapGui.GLControlMap3D.Resize += OnResize;

            Config.MapGui.GLControlMap3D.MouseDown += OnMouseDown;
            Config.MapGui.GLControlMap3D.MouseUp += OnMouseUp;
            Config.MapGui.GLControlMap3D.MouseMove += OnMouseMove;
            Config.MapGui.GLControlMap3D.MouseWheel += OnScroll;
            Config.MapGui.GLControlMap3D.DoubleClick += OnDoubleClick;
            Config.MapGui.GLControlMap3D.Cursor = Cursors.Hand;
        }

        public void OnPaint(object sender, EventArgs e)
        {
            UpdateCamera();

            Config.MapGui.GLControlMap3D.MakeCurrent();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.CullFace);

            // Make sure we have a camera
            if (_error || Config.Map3DCamera == null)
            {
                Config.MapGui.GLControlMap3D.SwapBuffers();
                return;
            }
            
            // Note: 0x8000 needs to match the camera far plane
            GL.Uniform1(Config.Map3DGraphics.GLUnifromInvfarplanecoef, 1);

            // Setup Background
            GL.Disable(EnableCap.DepthTest);

            // Draw background
            Config.MapGui.flowLayoutPanelMapTrackers.DrawOn3DControl(MapDrawType.Background);

            // Setup 3D
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            // Draw 3D
            Config.MapGui.flowLayoutPanelMapTrackers.DrawOn3DControl(MapDrawType.Perspective);

            // Setup 2D
            GL.Disable(EnableCap.DepthTest);

            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                Debugger.Break();

            // Draw 2D
            Config.MapGui.flowLayoutPanelMapTrackers.DrawOn3DControl(MapDrawType.Overlay);

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

            Config.MapGui.GLControlMap3D.SwapBuffers();
        }

        public void BindVertices()
        {
            GL.EnableVertexAttribArray(_glAttributePosition);
            GL.VertexAttribPointer(_glAttributePosition, 3, VertexAttribPointerType.Float, false, Map3DVertex.Size, Map3DVertex.IndexPosition);
            GL.EnableVertexAttribArray(_glAttributeColor);
            GL.VertexAttribPointer(_glAttributeColor, 4, VertexAttribPointerType.Float, false, Map3DVertex.Size, Map3DVertex.IndexColor);
            GL.EnableVertexAttribArray(_glAttributeTexCoords);
            GL.VertexAttribPointer(_glAttributeTexCoords, 2, VertexAttribPointerType.Float, false, Map3DVertex.Size, Map3DVertex.IndexTexCoord);
        }

        void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(Config.MapGui.GLControlMap3D.DisplayRectangle);
            OnSizeChanged?.Invoke(sender, e);
            Invalidate();
        }

        public void Invalidate()
        {
            Config.MapGui.GLControlMap3D.Invalidate();
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
            GLUnifromInvfarplanecoef = GL.GetUniformLocation(_shaderProgram, "invfarplanecoef");
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
                MapConfig.Map3DCameraYaw = (float)MoreMath.AngleTo_AngleUnits(
                    MapConfig.Map3DCameraX, MapConfig.Map3DCameraZ, MapConfig.Map3DFocusX, MapConfig.Map3DFocusZ);
                MapConfig.Map3DCameraPitch = (float)MoreMath.GetPitch(
                    MapConfig.Map3DCameraX, MapConfig.Map3DCameraY, MapConfig.Map3DCameraZ,
                    MapConfig.Map3DFocusX, MapConfig.Map3DFocusY, MapConfig.Map3DFocusZ);
            }

            if (!MapConfig.Map3DCameraPosPA.IsNone())
            {
                MapConfig.Map3DCameraX = (float)MapConfig.Map3DCameraPosPA.X;
                MapConfig.Map3DCameraY = (float)MapConfig.Map3DCameraPosPA.Y;
                MapConfig.Map3DCameraZ = (float)MapConfig.Map3DCameraPosPA.Z;
            }
            if (!MapConfig.Map3DCameraAnglePA.IsNone())
            {
                MapConfig.Map3DCameraYaw = (float)MapConfig.Map3DCameraAnglePA.Angle;
            }
            if (!MapConfig.Map3DFocusPosPA.IsNone())
            {
                MapConfig.Map3DFocusX = (float)MapConfig.Map3DFocusPosPA.X;
                MapConfig.Map3DFocusY = (float)MapConfig.Map3DFocusPosPA.Y;
                MapConfig.Map3DFocusZ = (float)MapConfig.Map3DFocusPosPA.Z;
            }

            switch (MapConfig.Map3DMode)
            {
                case Map3DCameraMode.InGame:
                    MapConfig.Map3DCameraX = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.XOffset);
                    MapConfig.Map3DCameraY = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.YOffset);
                    MapConfig.Map3DCameraZ = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.ZOffset);
                    MapConfig.Map3DCameraYaw = Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    MapConfig.Map3DCameraPitch = Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingPitchOffset);
                    MapConfig.Map3DCameraRoll = Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingRollOffset);
                    MapConfig.Map3DFocusX = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.FocusXOffset);
                    MapConfig.Map3DFocusY = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.FocusYOffset);
                    MapConfig.Map3DFocusZ = Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.FocusZOffset);
                    MapConfig.Map3DFOV = (float)MoreMath.Clamp(Config.Stream.GetFloat(CameraConfig.FOVStructAddress + CameraConfig.FOVValueOffset), 1, 179);
                    break;
                case Map3DCameraMode.CameraPosAndFocus:
                    updateCameraAngles();
                    break;
                case Map3DCameraMode.CameraPosAndAngle:
                    // do nothing, as we use whatever vars are stored
                    break;
                case Map3DCameraMode.FollowFocusRelativeAngle:
                    double angleOffset = MapConfig.Map3DFocusAnglePA.IsNone() ? 0 : MapConfig.Map3DFocusAnglePA.Angle;
                    (MapConfig.Map3DCameraX, MapConfig.Map3DCameraZ) =
                        ((float, float))MoreMath.AddVectorToPoint(
                            MapConfig.Map3DFollowingRadius,
                            MoreMath.ReverseAngle(MapConfig.Map3DFollowingYaw + angleOffset),
                            MapConfig.Map3DFocusX,
                            MapConfig.Map3DFocusZ);
                    MapConfig.Map3DCameraY = MapConfig.Map3DFocusY + MapConfig.Map3DFollowingYOffset;
                    updateCameraAngles();
                    break;
                case Map3DCameraMode.FollowFocusAbsoluteAngle:
                    (MapConfig.Map3DCameraX, MapConfig.Map3DCameraZ) =
                        ((float, float))MoreMath.AddVectorToPoint(
                            MapConfig.Map3DFollowingRadius,
                            MoreMath.ReverseAngle(MapConfig.Map3DFollowingYaw),
                            MapConfig.Map3DFocusX,
                            MapConfig.Map3DFocusZ);
                    MapConfig.Map3DCameraY = MapConfig.Map3DFocusY + MapConfig.Map3DFollowingYOffset;
                    updateCameraAngles();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Config.Map3DCamera.Position = new Vector3(MapConfig.Map3DCameraX, MapConfig.Map3DCameraY, MapConfig.Map3DCameraZ);
            Config.Map3DCamera.SetRotation(
                (float)MoreMath.AngleUnitsToRadians(MapConfig.Map3DCameraYaw),
                (float)MoreMath.AngleUnitsToRadians(MapConfig.Map3DCameraPitch),
                (float)MoreMath.AngleUnitsToRadians(MapConfig.Map3DCameraRoll));
            Config.Map3DCamera.FOV = MapConfig.Map3DFOV / 180 * (float)Math.PI;
        }

        private bool _isTranslating = false;
        private int _translateStartMouseX = 0;
        private int _translateStartMouseY = 0;
        private float _translateStartPositionX = 0;
        private float _translateStartPositionY = 0;
        private float _translateStartPositionZ = 0;

        private bool _isRotating = false;
        private int _rotateStartMouseX = 0;
        private int _rotateStartMouseY = 0;
        private float _rotateStartYaw = 0;
        private float _rotateStartPitch = 0;

        private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _isTranslating = true;
                    _translateStartMouseX = e.X;
                    _translateStartMouseY = e.Y;
                    _translateStartPositionX = MapConfig.Map3DCameraX;
                    _translateStartPositionY = MapConfig.Map3DCameraY;
                    _translateStartPositionZ = MapConfig.Map3DCameraZ;
                    break;
                case MouseButtons.Right:
                    _isRotating = true;
                    _rotateStartMouseX = e.X;
                    _rotateStartMouseY = e.Y;
                    _rotateStartYaw = MapConfig.Map3DCameraYaw;
                    _rotateStartPitch = MapConfig.Map3DCameraPitch;
                    break;
            }
        }

        private void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _isTranslating = false;
                    break;
                case MouseButtons.Right:
                    _isRotating = false;
                    break;
            }
        }

        private void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_isTranslating)
            {
                float scale = (float)MapConfig.Map3DTranslateSpeed;
                int pixelDiffX = e.X - _translateStartMouseX;
                int pixelDiffY = e.Y - _translateStartMouseY;
                pixelDiffX = MapUtilities.MaybeReverse(pixelDiffX);
                pixelDiffY = MapUtilities.MaybeReverse(pixelDiffY);
                float unitDiffX = pixelDiffX * scale;
                float unitDiffY = pixelDiffY * scale;
                (float rotX, float rotY, float rotZ) =
                    ((float, float, float))MoreMath.TranslateRelatively(
                        MapConfig.Map3DCameraYaw, MapConfig.Map3DCameraPitch, MapConfig.Map3DCameraRoll,
                        unitDiffX, unitDiffY, 0);

                MapConfig.Map3DMode = Map3DCameraMode.CameraPosAndAngle;
                MapConfig.Map3DCameraX = _translateStartPositionX - rotX;
                MapConfig.Map3DCameraY = _translateStartPositionY - rotY;
                MapConfig.Map3DCameraZ = _translateStartPositionZ - rotZ;
            }

            if (_isRotating)
            {
                float scale = (float)MapConfig.Map3DRotateSpeed;
                int pixelDiffX = e.X - _rotateStartMouseX;
                int pixelDiffY = e.Y - _rotateStartMouseY;
                pixelDiffX = MapUtilities.MaybeReverse(pixelDiffX);
                pixelDiffY = MapUtilities.MaybeReverse(pixelDiffY);
                float angleDiffX = pixelDiffX * scale;
                float angleDiffY = pixelDiffY * scale;

                MapConfig.Map3DMode = Map3DCameraMode.CameraPosAndAngle;
                MapConfig.Map3DCameraYaw = _rotateStartYaw + angleDiffX;
                MapConfig.Map3DCameraPitch = _rotateStartPitch + angleDiffY;
            }
        }

        private void OnScroll(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int multiplier = e.Delta > 0 ? 1 : -1;
            (float rotX, float rotY, float rotZ) =
                ((float, float, float))MoreMath.TranslateRelatively(
                    MapConfig.Map3DCameraYaw, MapConfig.Map3DCameraPitch, MapConfig.Map3DCameraRoll,
                    0, 0, multiplier * MapConfig.Map3DScrollSpeed);

            MapConfig.Map3DMode = Map3DCameraMode.CameraPosAndAngle;
            MapConfig.Map3DCameraX += rotX;
            MapConfig.Map3DCameraY += rotY;
            MapConfig.Map3DCameraZ += rotZ;
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            MapConfig.Map3DMode = Map3DCameraMode.InGame;
        }
    }
}
