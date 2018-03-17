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
using STROOP.Controls.Map.Graphics.Items;
using System.IO;
using System.Diagnostics;

namespace STROOP.Controls.Map.Graphics
{
    public class MapGraphics : IDisposable
    {
        const string VertexShaderPath = @"Resources\Shaders\VertexShader.glsl";
        const string FragmentShaderPath = @"Resources\Shaders\FragmentShader.glsl";
        const string ShaderLogPath = @"Resources\Shaders\ShaderLog.txt";


        public IMapCamera Camera { get; set; }
        public float AspectRatio => _control.AspectRatio;
        public float NormalizedWidth => AspectRatio <= 1.0f ? 1.0f : (float) _control.Width / _control.Height;
        public float NormalizedHeight => AspectRatio >= 1.0f ? 1.0f : (float) _control.Height / _control.Width;
        public Size Size => _control.Size;
        public float Width => _control.Width;
        public float Height => _control.Height;
        public bool Visible { get => _control.Visible; set => _control.Visible = value; }

        public event EventHandler OnSizeChanged;

        Matrix4 _identityView = Matrix4.Identity;
        List <MapGraphicsItem> _mapItems = new List<MapGraphicsItem>();
        Object _mapItemsLock = new object();
        GLControl _control { get; }

        bool _error = false;

        int _shaderProgram;
        int _vertexShader, _fragmentShader;

        int _glUniformView;
        int _glAttributePosition = 1;
        int _glAttributeColor = 2;
        int _glAttributeTexCoords = 3;

        public GraphicsUtilities Utilities { get; private set; }

        public MapGraphics(GLControl control)
        {
            _control = control;
        }

        public void Load()
        {

            _control.MakeCurrent();
            _control.Context.LoadAll();

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
            GL.Viewport(_control.DisplayRectangle);

            // Create utilties for GraphicsItems to use
            Utilities = new GraphicsUtilities(this);

            _control.Paint += OnPaint;
            _control.Resize += OnResize;

            // Test
            Camera = new MapCameraTopView(this);
        }

        public void OnPaint(object sender, EventArgs e)
        {
            _control.MakeCurrent();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.CullFace);

            // Make sure we have a camera
            if (_error || Camera == null)
            {
                _control.SwapBuffers();
                return;
            }

            // Get visible map graphics items
            IEnumerable<MapGraphicsItem> drawItems;
            lock (_mapItemsLock)
            {
                drawItems = _mapItems.Where(o => o.Visible && o.DrawOnCameraTypes.Contains(Camera.GetType()));
            }
            
            IEnumerable<MapGraphicsItem> drawItemsPerspective, drawItemsOverlay, drawItemsBackground;
            drawItemsPerspective = drawItems.Where(i => i.Type == MapGraphicsItem.DrawType.Perspective);
            drawItemsOverlay = drawItems.Where(i => i.Type == MapGraphicsItem.DrawType.Overlay).OrderByDescending(i => i.Depth);
            drawItemsBackground = drawItems.Where(i => i.Type == MapGraphicsItem.DrawType.Background);

            // Setup Background
            GL.Disable(EnableCap.DepthTest);

            // Draw background
            foreach (var mapItem in drawItemsBackground)
            {
                Matrix4 viewMatrix = mapItem.GetModelMatrix(this);
                GL.UniformMatrix4(_glUniformView, false, ref viewMatrix);
                mapItem.Draw(this);
            }

            // Setup 3D
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            // Draw 3D
            foreach (var mapItem in drawItemsPerspective)
            {
                Matrix4 viewMatrix = mapItem.GetModelMatrix(this) * Camera.Matrix;
                GL.UniformMatrix4(_glUniformView, false, ref viewMatrix);
                mapItem.Draw(this);
            } 

            // Setup 2D
            GL.Disable(EnableCap.DepthTest);

            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                Debugger.Break();

            // Draw 2D
            foreach (var mapItem in drawItemsOverlay) {
                Matrix4 viewMatrix = mapItem.GetModelMatrix(this);
                GL.UniformMatrix4(_glUniformView, false, ref viewMatrix);
                 mapItem.Draw(this);
            }

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

            _control.SwapBuffers();
        }

        public void BindVertices()
        {
            GL.EnableVertexAttribArray(_glAttributePosition);
            GL.VertexAttribPointer(_glAttributePosition, 3, VertexAttribPointerType.Float, false, Vertex.Size, Vertex.IndexPosition);
            GL.EnableVertexAttribArray(_glAttributeColor);
            GL.VertexAttribPointer(_glAttributeColor, 4, VertexAttribPointerType.Float, false, Vertex.Size, Vertex.IndexColor);
            GL.EnableVertexAttribArray(_glAttributeTexCoords);
            GL.VertexAttribPointer(_glAttributeTexCoords, 2, VertexAttribPointerType.Float, false, Vertex.Size, Vertex.IndexTexCoord);
        }

        void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(_control.DisplayRectangle);
            OnSizeChanged?.Invoke(sender, e);
            Invalidate();
        }

        public void Invalidate()
        {
            _control.Invalidate();
        }

        public void AddMapItem(MapGraphicsItem mapItem)
        {
            if (_error)
                return;

            lock (_mapItemsLock)
            {
                if (disposedValue)
                    return;

                mapItem.Load(this);
                _mapItems.Add(mapItem);
            }
        }

        public void RemoveMapObject(MapGraphicsItem mapItem)
        {
            lock (_mapItemsLock)
            {
                mapItem.Unload(this);
                _mapItems.Remove(mapItem);
            }
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
            _glUniformView = GL.GetUniformLocation(_shaderProgram, "view");
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
                        foreach (MapGraphicsItem item in _mapItems)
                            item.Unload(this);

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
