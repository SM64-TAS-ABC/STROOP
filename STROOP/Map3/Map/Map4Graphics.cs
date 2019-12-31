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
    }
}
