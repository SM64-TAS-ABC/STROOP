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
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;

namespace SM64_Diagnostic.Controls
{
    public class ModelGraphics
    {
        volatile float _cameraAngle = 0;
        volatile float _cameraRadius = 0;
        volatile float _cameraHeight = 0;

        Vector3 _modelCenter;

        public RectangleF MapView;
        public GLControl Control;
        Timer _timer;

        public ModelGraphics(GLControl control)
        {
            Control = control;
            _timer = new Timer();
            _timer.Interval = 1000 / 60;
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _cameraAngle += 0.01f;
        }

        public void Load()
        {
            Control.MakeCurrent();
            Control.Context.LoadAll();

            Control.Paint += OnPaint;
            Control.Resize += OnResize;
            Control.DragOver += Control_DragOver;
            Control.MouseWheel += Control_MouseWheel;

            GL.ClearColor(Color.FromKnownColor(KnownColor.Control));
            GL.Enable(EnableCap.DepthTest);

            _timer.Enabled = true;

            SetupViewport();
        }

        private void Control_MouseWheel(object sender, MouseEventArgs e)
        {
        }

        private void Control_DragOver(object sender, DragEventArgs e)
        {
            float x = e.X / Control.Width;
            float y = e.Y / Control.Height;
        }

        public void OnPaint(object sender, EventArgs e)
        {
            Control.MakeCurrent();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);

            SetupViewport();

            var cameraPos = new Vector3((float)(_cameraRadius * Math.Cos(_cameraAngle)),
                _cameraHeight, (float)(_cameraRadius * Math.Sin(_cameraAngle)));
            SetLookAtCamera(cameraPos, _modelCenter); 

            DrawModel();

            Control.SwapBuffers();
        }

        public void OnResize(object sender, EventArgs e)
        {
            Control.MakeCurrent();
            SetupViewport();
        }

        private void SetupViewport()
        {
            int w = Control.Width;
            int h = Control.Height;

            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area\

            SetPerspectiveProjection(w, h, 60f);
        }

        public Color ColorFromTri(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            float normY = Vector3.Cross(v2 - v1, v3 - v1).Normalized().Y;
            // Floor
            if (normY > 0.01)
                return Color.LightBlue;
            // Ceiling
            else if (normY < -0.01)
                return Color.Pink;
            // Wall   
            else
                return Color.LightGreen;
        }

        private void DrawModel()
        {
            lock (_modelLock)
            {
                // Draw triangles
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Begin(PrimitiveType.Triangles);
                for (int i = 0; i < _triangles.Length; i++)
                {
                    if (!_triangleSelected[i])
                        continue;

                    var t = _triangles[i];

                    GL.Color3(_triangleColors[i]);
                    GL.Vertex3(_vertices[t[0]]);
                    GL.Vertex3(_vertices[t[1]]);
                    GL.Vertex3(_vertices[t[2]]);
                }
                GL.End();

                // Draw lines
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.LineWidth(3.0f);
                GL.Begin(PrimitiveType.Triangles);
                for (int i = 0; i < _triangles.Length; i++)
                {
                    var t = _triangles[i];

                    GL.Color3(Color.Blue);
                    GL.Vertex3(_vertices[t[0]]);
                    GL.Vertex3(_vertices[t[1]]);
                    GL.Vertex3(_vertices[t[2]]);
                }
                GL.End();

                // Draw vertices
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
                GL.PointSize(8.0f);
                GL.Color3(Color.Yellow);
                GL.Begin(PrimitiveType.Points);
                for (int i = 0; i < _vertices.Length; i++)
                {
                    var v = _vertices[i];

                    // Only show vertices that are selected
                    if (!_vertexSelected[i])
                        continue;
                    
                    GL.Vertex3(v);
                }
                GL.End();
            }
        }

        Random rng = new Random();

        Vector3[] _vertices = new Vector3[0];
        Color[] _triangleColors = new Color[0];
        int[][] _triangles = new int[0][];
        bool[] _triangleSelected = new bool[0];
        bool[] _vertexSelected = new bool[0];
        object _modelLock = new object();

        public void ClearModel()
        {
            _vertices = new Vector3[0];
            _triangleColors = new Color[0];
            _triangles = new int[0][];
        }

        public void ChangeModel(List<short[]> vertices, List<int[]> triangles)
        {
            var maxRadius = vertices.Max(v => MoreMath.GetDistanceBetween(v[0], v[2], 0, 0));
            var maxHeight = vertices.Max(v => v[1]);
            var minHeight = vertices.Min(v => v[1]);

            _cameraHeight = maxHeight +  (float) (Math.Sqrt(2) * maxRadius);
            _cameraRadius = (float)maxRadius * 2f;

            _modelCenter = new Vector3(0, (maxHeight + minHeight) / 2, 0);

            lock (_modelLock)
            {
                // Create vertice point vectors
                _vertices = new Vector3[vertices.Count];
                for (int i = 0; i < _vertices.Length; i++)
                {
                    _vertices[i] = new Vector3(vertices[i][0], vertices[i][1], vertices[i][2]);
                }

                // Create triangle
                _triangles = new int[triangles.Count][];
                _triangleColors = new Color[triangles.Count];
                for (int i = 0; i < _triangles.Length; i++)
                {
                    // Make sure vertices exist
                    _triangles[i] = triangles[i].Select(t => t >= _vertices.Length || t < 0 ? 0 : t).ToArray();
                    // Find triangle colors
                    var tri = _triangles[i];
                    _triangleColors[i] = ColorFromTri(_vertices[tri[0]], _vertices[tri[1]], _vertices[tri[2]]);
                }

                // Unselect all triangle and vertices
                _vertexSelected = new bool[_vertices.Length];
                _triangleSelected = new bool[_triangles.Length];
            }
        }

        public void ChangeVertexSelection(bool[] vertexSelected)
        {
            lock (_modelLock)
            {
                for (int i = 0; i < vertexSelected.Length && i < _vertexSelected.Length; i++)
                    _vertexSelected[i] = vertexSelected[i];
            }
        }

        public void ChangeTriangleSelection(bool[] triangleSelected)
        {
            lock (_modelLock)
            {
                for (int i = 0; i < triangleSelected.Length && i < _triangleSelected.Length; i++)
                    _triangleSelected[i] = triangleSelected[i];
            }
        }

        private void SetPerspectiveProjection(int width, int height, float FOV)
        {
            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI * (FOV / 180f), width / (float)height, 0.2f, 100000.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix); // this replaces the old matrix, no need for GL.LoadIdentity()
        }

        private void SetLookAtCamera(Vector3 position, Vector3 target)
        {
            var modelViewMatrix = Matrix4.LookAt(position, target, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);
        }
    }
}
