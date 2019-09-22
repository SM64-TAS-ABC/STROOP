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
using STROOP.Controls.Map;

namespace STROOP.Map3
{
    public class Map3Graphics
    {
        public readonly GLControl Control;
        private readonly List<Map3Object> _mapObjects = new List<Map3Object>();

        public RectangleF MapView;
        public int XMin = -8191;
        public int XMax = 8192;
        public int ZMin = -8191;
        public int ZMax = 8192;
        public float ConversionScale
        {
            get => MapView.Width / (XMax - XMin);
        }

        public Map3Graphics(GLControl control)
        {
            Control = control;
        }

        public void Load()
        {
            Control.MakeCurrent();
            Control.Context.LoadAll();

            Control.Paint += (sender, e) => OnPaint();

            GL.ClearColor(Color.FromKnownColor(KnownColor.Control));
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        private void OnPaint()
        {
            Control.MakeCurrent();
            UpdateViewport();
            UpdateMapView();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

            // Loop through and draw all map objects
            foreach (Map3Object mapObj in _mapObjects)
            {
                // Draw the map object
                mapObj.DrawOnControl();
            }

            Control.SwapBuffers();
        }

        private void UpdateViewport()
        {
            int w = Control.Width;
            int h = Control.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void UpdateMapView()
        {
            // Calculate scale of "zoom" view (make sure image fits fully within the region, 
            // it is at a maximum size, and the aspect ration is maintained 
            float minLength = Math.Min(Control.Width, Control.Height);

            float marginV = 0;
            float marginH = 0;
            if (Control.Width > Control.Height)
                marginH = Control.Width - minLength;
            else
                marginV = Control.Height - minLength;

            // Calculate where the map image should be drawn
            MapView = new RectangleF(marginH / 2, marginV / 2, Control.Width - marginH, Control.Height - marginV);
        }

        public void AddMapObject(Map3Object mapObj)
        {
            _mapObjects.Add(mapObj);
        }

        public void RemoveMapObject(Map3Object mapObj)
        {
            _mapObjects.Remove(mapObj);
            mapObj?.Dispose();
        }
    }
}
