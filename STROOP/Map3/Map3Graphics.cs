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
using STROOP.Structs.Configurations;
using STROOP.Utilities;

namespace STROOP.Map3
{
    public class Map3Graphics
    {
        public readonly GLControl Control;
        private readonly List<Map3Object> _mapObjects = new List<Map3Object>();

        private enum Map3Scale { CourseDefault, MaxCourseSize, Custom };
        private enum Map3Center { BestFit, Origin, Custom };
        private enum Map3Angle { Angle0, Angle16384, Angle32768, Angle49152, Custom };

        private Map3Scale MapViewScale;
        private Map3Center MapViewCenter;
        private Map3Angle MapViewAngle;

        private static readonly float DEFAULT_MAP_VIEW_SCALE_VALUE = 1;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_X_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_Z_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_ANGLE_VALUE = 32768;

        public float MapViewScaleValue = DEFAULT_MAP_VIEW_SCALE_VALUE;
        public float MapViewCenterXValue = DEFAULT_MAP_VIEW_CENTER_X_VALUE;
        public float MapViewCenterZValue = DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
        public float MapViewAngleValue = DEFAULT_MAP_VIEW_ANGLE_VALUE;

        public static readonly int MAX_COURSE_SIZE_X_MIN = -8191;
        public static readonly int MAX_COURSE_SIZE_X_MAX = 8192;
        public static readonly int MAX_COURSE_SIZE_Z_MIN = -8191;
        public static readonly int MAX_COURSE_SIZE_Z_MAX = 8192;
        public static readonly RectangleF MAX_COURSE_SIZE =
            new RectangleF(
                MAX_COURSE_SIZE_X_MIN,
                MAX_COURSE_SIZE_Z_MIN,
                MAX_COURSE_SIZE_X_MAX - MAX_COURSE_SIZE_X_MIN,
                MAX_COURSE_SIZE_Z_MAX - MAX_COURSE_SIZE_Z_MIN);

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
            if (Config.Map3Gui.radioButtonMap3ControllersScaleCourseDefault.Checked)
                MapViewScale = Map3Scale.CourseDefault;
            else if (Config.Map3Gui.radioButtonMap3ControllersScaleMaxCourseSize.Checked)
                MapViewScale = Map3Scale.MaxCourseSize;
            else
                MapViewScale = Map3Scale.Custom;

            if (Config.Map3Gui.radioButtonMap3ControllersCenterBestFit.Checked)
                MapViewCenter = Map3Center.BestFit;
            else if (Config.Map3Gui.radioButtonMap3ControllersCenterOrigin.Checked)
                MapViewCenter = Map3Center.Origin;
            else
                MapViewCenter = Map3Center.Custom;

            if (Config.Map3Gui.radioButtonMap3ControllersAngle0.Checked)
                MapViewAngle = Map3Angle.Angle0;
            else if (Config.Map3Gui.radioButtonMap3ControllersAngle16384.Checked)
                MapViewAngle = Map3Angle.Angle16384;
            else if(Config.Map3Gui.radioButtonMap3ControllersAngle32768.Checked)
                MapViewAngle = Map3Angle.Angle32768;
            else if(Config.Map3Gui.radioButtonMap3ControllersAngle49152.Checked)
                MapViewAngle = Map3Angle.Angle49152;
            else
                MapViewAngle = Map3Angle.Custom;

            switch (MapViewScale)
            {
                case Map3Scale.CourseDefault:
                case Map3Scale.MaxCourseSize:
                    RectangleF coordinates = MapViewScale == Map3Scale.CourseDefault ?
                        Map3Utilities.GetMapLayout().Coordinates : MAX_COURSE_SIZE;
                    MapViewScaleValue = Math.Min(
                        Control.Width / coordinates.Width, Control.Height / coordinates.Height);
                    break;
                case Map3Scale.Custom:
                    MapViewScaleValue = ParsingUtilities.ParseFloatNullable(
                        Config.Map3Gui.textBoxMap3ControllersScaleCustom.LastSubmittedText)
                        ?? DEFAULT_MAP_VIEW_SCALE_VALUE;
                    break;
            }

            switch (MapViewCenter)
            {
                case Map3Center.BestFit:
                    RectangleF coordinates = MapViewScale == Map3Scale.CourseDefault ?
                        Map3Utilities.GetMapLayout().Coordinates : MAX_COURSE_SIZE;
                    MapViewCenterXValue = coordinates.X + coordinates.Width / 2;
                    MapViewCenterZValue = coordinates.Y + coordinates.Height / 2;
                    break;
                case Map3Center.Origin:
                    MapViewCenterXValue = 0;
                    MapViewCenterZValue = 0;
                    break;
                case Map3Center.Custom:
                    List<string> stringValues = ParsingUtilities.ParseStringList(
                        Config.Map3Gui.textBoxMap3ControllersCenterCustom.LastSubmittedText);
                    if (stringValues.Count >= 2)
                    {
                        MapViewCenterXValue = ParsingUtilities.ParseFloatNullable(stringValues[0]) ?? DEFAULT_MAP_VIEW_CENTER_X_VALUE;
                        MapViewCenterZValue = ParsingUtilities.ParseFloatNullable(stringValues[1]) ?? DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
                    }
                    else if (stringValues.Count == 1)
                    {
                        MapViewCenterXValue = ParsingUtilities.ParseFloatNullable(stringValues[0]) ?? DEFAULT_MAP_VIEW_CENTER_X_VALUE;
                        MapViewCenterZValue = ParsingUtilities.ParseFloatNullable(stringValues[0]) ?? DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
                    }
                    else
                    {
                        MapViewCenterXValue = DEFAULT_MAP_VIEW_CENTER_X_VALUE;
                        MapViewCenterZValue = DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
                    }
                    break;
            }

            switch (MapViewAngle)
            {
                case Map3Angle.Angle0:
                    MapViewAngleValue = 0;
                    break;
                case Map3Angle.Angle16384:
                    MapViewAngleValue = 16384;
                    break;
                case Map3Angle.Angle32768:
                    MapViewAngleValue = 32768;
                    break;
                case Map3Angle.Angle49152:
                    MapViewAngleValue = 49152;
                    break;
                case Map3Angle.Custom:
                    MapViewAngleValue = ParsingUtilities.ParseFloatNullable(
                        Config.Map3Gui.textBoxMap3ControllersAngleCustom.LastSubmittedText)
                        ?? DEFAULT_MAP_VIEW_ANGLE_VALUE;
                    break;
            }






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
