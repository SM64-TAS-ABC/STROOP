using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using STROOP.Structs;
using STROOP.Utilities;

namespace STROOP.Controls
{
    public class ModelGraphics
    {
        volatile float _cameraAngle = 0;
        volatile float _cameraRadius = 0;
        volatile float _cameraHeight = 0;

        Vector3 _cameraPosition;
        Vector3 _cameraLook;
        float _cameraManualAngleLat;
        float _cameraManualAngleLong;

        Vector3 _modelCenter;
        float _modelRadius;
        float _zoom = 1.0f;
        float _pov = 90f; // Calculated from Zoom

        public RectangleF MapView;
        public GLControl Control;
        Timer _timer;

        public bool ManualMode = false;

        public ModelGraphics(GLControl control)
        {
            Control = control;
            _timer = new Timer();
            _timer.Interval = 1000 / 60;
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!ManualMode)
            {
                KeyboardState keyState = Keyboard.GetState();

                float speed = 0.01f;
                if (keyState.IsKeyDown(Key.ControlLeft) || keyState.IsKeyDown(Key.ControlRight))
                    speed = 0.0f;
                else if(keyState.IsKeyDown(Key.ShiftLeft) || keyState.IsKeyDown(Key.ShiftRight))
                    speed = 0.03f;
                else if (keyState.IsKeyDown(Key.AltLeft) || keyState.IsKeyDown(Key.AltRight))
                    speed = 0.003f;

                _cameraAngle += speed;
            }

            CameraFly();
        }

        public void Load()
        {
            Control.MakeCurrent();
            Control.Context.LoadAll();

            Control.Paint += OnPaint;
            Control.Resize += OnResize;
            Control.MouseDown += Control_MouseClick;

            GL.ClearColor(Color.FromKnownColor(KnownColor.Control));
            GL.Enable(EnableCap.DepthTest);

            _timer.Enabled = true;

            SetupViewport();
        }

        volatile bool _mousePressedWithin = false;
        private void Control_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _mousePressedWithin = true;
        }

        bool _mousePressed = false;

        Vector2 _pMouseCoords;
        float? _pMouseScroll = null;
        public void CameraFly()
        {
            KeyboardState keyState = Keyboard.GetState();

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
                delta *= _pov / 90;

                // Trackball (add mouse deltas to angle)
                _cameraManualAngleLat += delta.X;
                _cameraManualAngleLong += -delta.Y;

                if (_cameraManualAngleLong > Math.PI / 2 - 0.001f)
                {
                    _cameraManualAngleLong = (float) (Math.PI / 2) - 0.001f;
                }
                else if (_cameraManualAngleLong < -Math.PI / 2 + 0.001f)
                {
                    _cameraManualAngleLong = (float) (-Math.PI / 2) + 0.001f;
                }

                // Update mouse coordinates for next time
                _pMouseCoords = new Vector2(mouseState.X, mouseState.Y);

                _mousePressed = true;
                ManualMode = true;
            }
            else
            {
                if (_mousePressed)
                    _mousePressedWithin = false;
                _mousePressed = false;
            }

            // Don't do anything if we don't have focus
            if (!Control.Focused)
                return;

            if (!_pMouseScroll.HasValue)
                _pMouseScroll = mouseState.ScrollWheelValue;
            float deltaScroll = mouseState.ScrollWheelValue - _pMouseScroll.Value;
            _zoom += deltaScroll * 0.1f * speedMul;
            _pMouseScroll = mouseState.ScrollWheelValue;

            Vector3 relDeltaPos = new Vector3(0, 0, 0);
            float posSpeed = speedMul * _modelRadius * 0.01f; // Move at a rate relative to the model size

            // Handle Positional Movement 
            if (keyState.IsKeyDown(Key.W) || keyState.IsKeyDown(Key.Up))
            {
                relDeltaPos.Z += posSpeed;
                ManualMode = true;
            }
            if (keyState.IsKeyDown(Key.A) || keyState.IsKeyDown(Key.Left))
            {
                relDeltaPos.X += posSpeed;
                ManualMode = true;
            }
            if (keyState.IsKeyDown(Key.S) || keyState.IsKeyDown(Key.Down))
            {
                relDeltaPos.Z += -posSpeed;
                ManualMode = true;
            }
            if (keyState.IsKeyDown(Key.D) || keyState.IsKeyDown(Key.Right))
            {
                relDeltaPos.X += -posSpeed;
                ManualMode = true;
            }
            if (keyState.IsKeyDown(Key.Q))
            {
                relDeltaPos.Y += -posSpeed;
                ManualMode = true;
            }
            if (keyState.IsKeyDown(Key.E))
            {
                relDeltaPos.Y += posSpeed;
                ManualMode = true;
            }

            // Update camera position
            // This requires converting the coordinate system from the camera coordinates 
            // to the world coordinates. The camera X unit is calculate from the 
            // cross product of the camera Y unit and the camera Z unit. The camera
            // Y unit is the world Y unit since the Y coordinate is always up.
            // The Z unit is the normalized camera look vector (to move towards the look),
            // Hence, move formard.
            _cameraPosition += Vector3.Cross(Vector3.UnitY, _cameraLook) * relDeltaPos.X
                + Vector3.UnitY * relDeltaPos.Y
                + _cameraLook * relDeltaPos.Z;
        }

        public void OnPaint(object sender, EventArgs e)
        {
            Control.MakeCurrent();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.DepthRange(0.0, 1.0f);

            SetupViewport();

            if (ManualMode)
            {
                // Convert the long. and lat. angles into a camera look vector
                _cameraLook.Y = (float) (Math.Sin(_cameraManualAngleLong));
                float yy = (float) Math.Sqrt(1 - _cameraLook.Y * _cameraLook.Y);
                _cameraLook.X = (float) Math.Cos(_cameraManualAngleLat) * yy;
                _cameraLook.Z = (float) Math.Sin(_cameraManualAngleLat) * yy;
            }
            else
            {
                // Rotate around model
                _cameraPosition = new Vector3((float)(_cameraRadius * Math.Cos(_cameraAngle)),
                    _cameraHeight, (float)(_cameraRadius * Math.Sin(_cameraAngle)));
                _cameraLook = (_modelCenter - _cameraPosition).Normalized();

                // Update the long. and lat. angles for switching to manual mode
                _cameraManualAngleLat = (float) Math.Atan2(_cameraLook.Z, _cameraLook.X);
                _cameraManualAngleLong = (float) Math.Asin(_cameraLook.Y);
            }

            _pov = (float)(90f + Math.Atan(_zoom) * 180f / Math.PI);
            SetLookAtCamera(_cameraPosition, _cameraPosition + _cameraLook);
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

            SetPerspectiveProjection(w, h, _pov);
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
            ManualMode = false;

            var maxRadius = vertices.Max(v => MoreMath.GetDistanceBetween(v[0], v[2], 0, 0));
            var maxHeight = vertices.Max(v => v[1]);
            var minHeight = vertices.Min(v => v[1]);

            _cameraHeight = maxHeight + (float) (Math.Sqrt(2) * maxRadius);
            _cameraRadius = (float)maxRadius * 2f;

            _modelCenter = new Vector3(0, (maxHeight + minHeight) / 2, 0);
            _modelRadius = vertices.Max(v => (new Vector3(v[0], v[1], v[2]) - _modelCenter).Length);

            _zoom = -0.57735026919f; // 60 degree FOV

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
