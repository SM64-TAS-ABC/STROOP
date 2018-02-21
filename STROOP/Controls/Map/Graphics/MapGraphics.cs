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

        Matrix4 _identityView = Matrix4.Identity;

        public IMapCamera Camera { get; set; }
        public GLControl Control { get; }

        List<MapGraphicsItem> _mapItems = new List<MapGraphicsItem>();
        Object _mapItemsLock = new object();

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
            Control = control;
        }

        public void Load()
        {

            Control.MakeCurrent();
            Control.Context.LoadAll();

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
            GL.Viewport(Control.DisplayRectangle);

            // Create utilties for GraphicsItems to use
            Utilities = new GraphicsUtilities(this);

            Control.Paint += OnPaint;
            Control.Resize += OnResize;

            // Test
            Camera = new MapCameraTopView();
            AddMapItem(new MapGraphicsBackgroundItem(Image.FromFile(@"Resources\Maps\Map Images\BBH Floor 1.png") as Bitmap));
        }

        public void OnPaint(object sender, EventArgs e)
        {
            Control.MakeCurrent();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Make sure we have a camera
            if (_error || Camera == null)
            {
                Control.SwapBuffers();
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
            drawItemsOverlay = drawItems.Where(i => i.Type == MapGraphicsItem.DrawType.Perspective).OrderByDescending(i => i.Depth);
            drawItemsBackground = drawItems.Where(i => i.Type == MapGraphicsItem.DrawType.Background);

            // Setup attributes
            GL.EnableVertexAttribArray(_glAttributePosition);
            GL.VertexAttribPointer(_glAttributePosition, 3, VertexAttribPointerType.Float, false, Vertex.Size, Vertex.IndexPosition);
            GL.EnableVertexAttribArray(_glAttributeColor);
            GL.VertexAttribPointer(_glAttributeColor, 4, VertexAttribPointerType.Float, false, Vertex.Size, Vertex.IndexColor);
            GL.EnableVertexAttribArray(_glAttributeTexCoords);
            GL.VertexAttribPointer(_glAttributeTexCoords, 2, VertexAttribPointerType.Float, false, Vertex.Size, Vertex.IndexTexCoord);

            // Setup Background
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            _identityView = Matrix4.CreateOrthographicOffCenter(-1, 1, 1, -1, -1, 1);
            GL.UniformMatrix4(_glUniformView, false, ref _identityView);

            // Draw background
            foreach (var mapItem in drawItemsBackground)
                mapItem.Draw(this);


            var error = GL.GetError();
            if (error != ErrorCode.NoError)
                Debugger.Break();

            // Setup 3D
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            Matrix4 viewMatrix = Camera.CameraMatrix;
            GL.UniformMatrix4(_glUniformView, false, ref viewMatrix);

            // Draw 3D
            foreach (var mapItem in drawItemsPerspective)
                mapItem.Draw(this);

            // Setup 2D
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
            GL.UniformMatrix4(_glUniformView, false, ref _identityView);

            // Draw 2D
            foreach (var mapItem in drawItemsOverlay)
                mapItem.Draw(this);

            // Disable Attributes
            GL.DisableVertexAttribArray(_glAttributePosition);
            GL.DisableVertexAttribArray(_glAttributeColor);
            GL.DisableVertexAttribArray(_glAttributeTexCoords);

            Control.SwapBuffers();
        }

        public void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(Control.DisplayRectangle);
            Control.Invalidate();
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
