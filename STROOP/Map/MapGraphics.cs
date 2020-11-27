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
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using OpenTK.Input;

namespace STROOP.Map
{
    public class MapGraphics
    {
        private enum MapScale { CourseDefault, MaxCourseSize, Custom };
        private enum MapCenter { BestFit, Origin, Mario, Custom };
        private enum MapAngle { Angle0, Angle16384, Angle32768, Angle49152, Mario, Camera, Centripetal, Custom };
        private enum MapSideViewAngle { Angle0, Angle16384, Angle32768, Angle49152 };

        private MapScale MapViewScale;
        private MapCenter MapViewCenter;
        private MapAngle MapViewAngle;
        private MapSideViewAngle MapViewSideViewAngle;
        private bool MapViewScaleWasCourseDefault = true;

        private static readonly float DEFAULT_MAP_VIEW_SCALE_VALUE = 1;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_X_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_Y_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_Z_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_ANGLE_VALUE = 32768;

        public float MapViewScaleValue = DEFAULT_MAP_VIEW_SCALE_VALUE;
        public float MapViewCenterXValue = DEFAULT_MAP_VIEW_CENTER_X_VALUE;
        public float MapViewCenterYValue = DEFAULT_MAP_VIEW_CENTER_Y_VALUE;
        public float MapViewCenterZValue = DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
        public float MapViewAngleValue = DEFAULT_MAP_VIEW_ANGLE_VALUE;

        public bool MapViewEnablePuView = false;
        public bool MapViewScaleIconSizes = false;
        public bool MapViewCenterChangeByPixels = true;

        public float MapViewRadius { get => (float)MoreMath.GetHypotenuse(
            Config.MapGui.GLControlMap2D.Width / 2, Config.MapGui.GLControlMap2D.Height / 2) / MapViewScaleValue; } // TODO(sideview): fix this
        public float MapViewXMin { get => MapViewCenterXValue - MapViewRadius; }
        public float MapViewXMax { get => MapViewCenterXValue + MapViewRadius; }
        public float MapViewYMin { get => MapViewCenterYValue - MapViewRadius; }
        public float MapViewYMax { get => MapViewCenterYValue + MapViewRadius; }
        public float MapViewZMin { get => MapViewCenterZValue - MapViewRadius; }
        public float MapViewZMax { get => MapViewCenterZValue + MapViewRadius; }

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

        public MapGraphics()
        {
        }

        public void Load()
        {
            Config.MapGui.GLControlMap2D.MakeCurrent();
            Config.MapGui.GLControlMap2D.Context.LoadAll();

            Config.MapGui.GLControlMap2D.Paint += (sender, e) => OnPaint();

            Config.MapGui.GLControlMap2D.MouseDown += OnMouseDown;
            Config.MapGui.GLControlMap2D.MouseUp += OnMouseUp;
            Config.MapGui.GLControlMap2D.MouseMove += OnMouseMove;
            Config.MapGui.GLControlMap2D.MouseWheel += OnScroll;
            Config.MapGui.GLControlMap2D.DoubleClick += OnDoubleClick;

            GL.ClearColor(Color.FromKnownColor(KnownColor.Control));
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        private void OnPaint()
        {
            Cursor cursor = Config.MapManager.NumDrawingsEnabled > 0 ? Cursors.Cross : Cursors.Hand;
            if (Config.MapGui.GLControlMap2D.Cursor != cursor)
            {
                Config.MapGui.GLControlMap2D.Cursor = cursor;
            }

            Config.MapGui.GLControlMap2D.MakeCurrent();
            UpdateViewport();
            UpdateMapView();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

            Config.MapGui.flowLayoutPanelMapTrackers.DrawOn2DControl();

            Config.MapGui.GLControlMap2D.SwapBuffers();
        }

        private void UpdateViewport()
        {
            int w = Config.MapGui.GLControlMap2D.Width;
            int h = Config.MapGui.GLControlMap2D.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void UpdateMapView()
        {
            UpdateAngle();
            UpdateScale();
            UpdateCenter();
        }

        private void UpdateScale()
        {
            if (Config.MapGui.radioButtonMapControllersScaleCourseDefault.Checked)
                MapViewScale = MapScale.CourseDefault;
            else if (Config.MapGui.radioButtonMapControllersScaleMaxCourseSize.Checked)
                MapViewScale = MapScale.MaxCourseSize;
            else
                MapViewScale = MapScale.Custom;

            if (MapViewScale == MapScale.CourseDefault) MapViewScaleWasCourseDefault = true;
            if (MapViewScale == MapScale.MaxCourseSize) MapViewScaleWasCourseDefault = false;

            switch (MapViewScale)
            {
                case MapScale.CourseDefault:
                case MapScale.MaxCourseSize:
                    RectangleF rectangle = MapViewScale == MapScale.CourseDefault ?
                        MapUtilities.GetMapLayout().Coordinates : MAX_COURSE_SIZE;
                    List<(float, float)> coordinates = new List<(float, float)>()
                    {
                        (rectangle.Left, rectangle.Top),
                        (rectangle.Right, rectangle.Top),
                        (rectangle.Left, rectangle.Bottom),
                        (rectangle.Right, rectangle.Bottom),
                    };
                    List<(float, float)> rotatedCoordinates = coordinates.ConvertAll(coord =>
                    {
                        (float x, float z) = coord;
                        (double rotatedX, double rotatedZ) = MoreMath.RotatePointAboutPointAnAngularDistance(
                            x, z, 0, 0, 32768 - Config.MapGraphics.MapViewAngleValue);
                        return ((float)rotatedX, (float)rotatedZ);
                    });
                    float rotatedXMax = rotatedCoordinates.Max(coord => coord.Item1);
                    float rotatedXMin = rotatedCoordinates.Min(coord => coord.Item1);
                    float rotatedZMax = rotatedCoordinates.Max(coord => coord.Item2);
                    float rotatedZMin = rotatedCoordinates.Min(coord => coord.Item2);
                    float rotatedWidth = rotatedXMax - rotatedXMin;
                    float rotatedHeight = rotatedZMax - rotatedZMin;
                    MapViewScaleValue = Math.Min(
                        Config.MapGui.GLControlMap2D.Width / rotatedWidth, Config.MapGui.GLControlMap2D.Height / rotatedHeight);
                    break;
                case MapScale.Custom:
                    MapViewScaleValue = ParsingUtilities.ParseFloatNullable(
                        Config.MapGui.textBoxMapControllersScaleCustom.LastSubmittedText)
                        ?? DEFAULT_MAP_VIEW_SCALE_VALUE;
                    break;
            }

            if (MapViewScale != MapScale.Custom)
            {
                Config.MapGui.textBoxMapControllersScaleCustom.SubmitTextLoosely(MapViewScaleValue.ToString());
            }
        }

        private void UpdateCenter()
        {
            if (Config.MapGui.radioButtonMapControllersCenterBestFit.Checked)
                MapViewCenter = MapCenter.BestFit;
            else if (Config.MapGui.radioButtonMapControllersCenterOrigin.Checked)
                MapViewCenter = MapCenter.Origin;
            else if (Config.MapGui.radioButtonMapControllersCenterMario.Checked)
                MapViewCenter = MapCenter.Mario;
            else
                MapViewCenter = MapCenter.Custom;

            switch (MapViewCenter)
            {
                case MapCenter.BestFit:
                    RectangleF rectangle = MapViewScaleWasCourseDefault ?
                        MapUtilities.GetMapLayout().Coordinates : MAX_COURSE_SIZE;
                    MapViewCenterXValue = rectangle.X + rectangle.Width / 2;
                    MapViewCenterYValue = 0;
                    MapViewCenterZValue = rectangle.Y + rectangle.Height / 2;
                    break;
                case MapCenter.Origin:
                    MapViewCenterXValue = 0.5f;
                    MapViewCenterYValue = 0;
                    MapViewCenterZValue = 0.5f;
                    break;
                case MapCenter.Mario:
                    MapViewCenterXValue = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    MapViewCenterYValue = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    MapViewCenterZValue = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    break;
                case MapCenter.Custom:
                    PositionAngle posAngle = PositionAngle.FromString(
                        Config.MapGui.textBoxMapControllersCenterCustom.LastSubmittedText);
                    if (posAngle != null)
                    {
                        MapViewCenterXValue = (float)posAngle.X;
                        MapViewCenterYValue = (float)posAngle.Y;
                        MapViewCenterZValue = (float)posAngle.Z;
                        break;
                    }
                    List<string> stringValues = ParsingUtilities.ParseStringList(
                        Config.MapGui.textBoxMapControllersCenterCustom.LastSubmittedText, replaceComma: false);

                    if (stringValues.Count == 2)
                    {
                        MapViewCenterXValue = ParsingUtilities.ParseFloatNullable(stringValues[0]) ?? DEFAULT_MAP_VIEW_CENTER_X_VALUE;
                        MapViewCenterZValue = ParsingUtilities.ParseFloatNullable(stringValues[1]) ?? DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
                    }
                    else if (stringValues.Count == 3)
                    {
                        MapViewCenterXValue = ParsingUtilities.ParseFloatNullable(stringValues[0]) ?? DEFAULT_MAP_VIEW_CENTER_X_VALUE;
                        MapViewCenterYValue = ParsingUtilities.ParseFloatNullable(stringValues[1]) ?? DEFAULT_MAP_VIEW_CENTER_Y_VALUE;
                        MapViewCenterZValue = ParsingUtilities.ParseFloatNullable(stringValues[2]) ?? DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
                    }
                    break;
            }

            if (MapViewCenter != MapCenter.Custom)
            {
                List<float> values = new List<float>();
                values.Add(MapViewCenterXValue);
                if (Config.MapGui.checkBoxMapOptionsEnableSideView.Checked) values.Add(MapViewCenterYValue);
                values.Add(MapViewCenterZValue);
                Config.MapGui.textBoxMapControllersCenterCustom.SubmitTextLoosely(string.Join(";", values));
            }
        }

        private void UpdateAngle()
        {
            if (Config.MapGui.radioButtonMapControllersAngle0.Checked)
                MapViewAngle = MapAngle.Angle0;
            else if (Config.MapGui.radioButtonMapControllersAngle16384.Checked)
                MapViewAngle = MapAngle.Angle16384;
            else if (Config.MapGui.radioButtonMapControllersAngle32768.Checked)
                MapViewAngle = MapAngle.Angle32768;
            else if (Config.MapGui.radioButtonMapControllersAngle49152.Checked)
                MapViewAngle = MapAngle.Angle49152;
            else if (Config.MapGui.radioButtonMapControllersAngleMario.Checked)
                MapViewAngle = MapAngle.Mario;
            else if (Config.MapGui.radioButtonMapControllersAngleCamera.Checked)
                MapViewAngle = MapAngle.Camera;
            else if (Config.MapGui.radioButtonMapControllersAngleCentripetal.Checked)
                MapViewAngle = MapAngle.Centripetal;
            else
                MapViewAngle = MapAngle.Custom;

            switch (MapViewAngle)
            {
                case MapAngle.Angle0:
                    MapViewAngleValue = 0;
                    break;
                case MapAngle.Angle16384:
                    MapViewAngleValue = 16384;
                    break;
                case MapAngle.Angle32768:
                    MapViewAngleValue = 32768;
                    break;
                case MapAngle.Angle49152:
                    MapViewAngleValue = 49152;
                    break;
                case MapAngle.Mario:
                    MapViewAngleValue = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    break;
                case MapAngle.Camera:
                    MapViewAngleValue = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    break;
                case MapAngle.Centripetal:
                    MapViewAngleValue = (float)MoreMath.ReverseAngle(
                        Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset));
                    break;
                case MapAngle.Custom:
                    PositionAngle posAngle = PositionAngle.FromString(
                        Config.MapGui.textBoxMapControllersAngleCustom.LastSubmittedText);
                    if (posAngle != null)
                    {
                        MapViewAngleValue = (float)posAngle.Angle;
                        break;
                    }
                    MapViewAngleValue = ParsingUtilities.ParseFloatNullable(
                        Config.MapGui.textBoxMapControllersAngleCustom.LastSubmittedText)
                        ?? DEFAULT_MAP_VIEW_ANGLE_VALUE;
                    break;
            }

            // set map side view angle
            {
                double added = MoreMath.NormalizeAngleDouble(MapViewAngleValue + 8192);
                int divided = (int)added / 16384;
                int multiplied = divided * 16384;
                switch (multiplied)
                {
                    case 0:
                        MapViewSideViewAngle = MapSideViewAngle.Angle0;
                        break;
                    case 16384:
                        MapViewSideViewAngle = MapSideViewAngle.Angle16384;
                        break;
                    case 32768:
                        MapViewSideViewAngle = MapSideViewAngle.Angle32768;
                        break;
                    case 49152:
                        MapViewSideViewAngle = MapSideViewAngle.Angle49152;
                        break;
                    default:
                        MapViewSideViewAngle = MapSideViewAngle.Angle32768;
                        break;
                }
            }

            if (MapViewAngle != MapAngle.Custom)
            {
                Config.MapGui.textBoxMapControllersAngleCustom.SubmitTextLoosely(MapViewAngleValue.ToString());
            }
        }

        public void ChangeScale(int sign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.MapGui.radioButtonMapControllersScaleCustom.Checked = true;
            float newScaleValue = MapViewScaleValue + sign * parsed.Value;
            Config.MapGui.textBoxMapControllersScaleCustom.SubmitText(newScaleValue.ToString());
        }

        public void ChangeScale2(int power, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.MapGui.radioButtonMapControllersScaleCustom.Checked = true;
            float newScaleValue = MapViewScaleValue * (float)Math.Pow(parsed.Value, power);
            Config.MapGui.textBoxMapControllersScaleCustom.SubmitText(newScaleValue.ToString());
        }

        public void ChangeCenter(int xSign, int zSign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.MapGui.radioButtonMapControllersCenterCustom.Checked = true;
            float xOffset = xSign * parsed.Value;
            float zOffset = zSign * parsed.Value;
            (float xOffsetRotated, float zOffsetRotated) = ((float, float)) MoreMath.RotatePointAboutPointAnAngularDistance(
                xOffset, zOffset, 0, 0, Config.MapGraphics.MapViewAngleValue);
            float multiplier = MapViewCenterChangeByPixels ? 1 / MapViewScaleValue : 1;
            float newCenterXValue = MapViewCenterXValue + xOffsetRotated * multiplier;
            float newCenterZValue = MapViewCenterZValue + zOffsetRotated * multiplier;
            Config.MapGui.textBoxMapControllersCenterCustom.SubmitText(newCenterXValue + ";" + newCenterZValue);
        }

        public void ChangeAngle(int sign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.MapGui.radioButtonMapControllersAngleCustom.Checked = true;
            float newAngleValue = MapViewAngleValue + sign * parsed.Value;
            newAngleValue = (float)MoreMath.NormalizeAngleDouble(newAngleValue);
            Config.MapGui.textBoxMapControllersAngleCustom.SubmitText(newAngleValue.ToString());
        }

        public void SetCustomScale(object value)
        {
            Config.MapGui.radioButtonMapControllersScaleCustom.Checked = true;
            Config.MapGui.textBoxMapControllersScaleCustom.SubmitText(value.ToString());
        }

        public void SetCustomCenter(object value)
        {
            Config.MapGui.radioButtonMapControllersCenterCustom.Checked = true;
            Config.MapGui.textBoxMapControllersCenterCustom.SubmitText(value.ToString());
        }

        public void SetCustomAngle(object value)
        {
            Config.MapGui.radioButtonMapControllersAngleCustom.Checked = true;
            Config.MapGui.textBoxMapControllersAngleCustom.SubmitText(value.ToString());
        }

        private bool _isTranslating = false;
        private int _translateStartMouseX = 0;
        private int _translateStartMouseY = 0;
        private float _translateStartCenterX = 0;
        private float _translateStartCenterZ = 0;

        private bool _isRotating = false;
        private int _rotateStartMouseX = 0;
        private int _rotateStartMouseY = 0;
        private float _rotateStartAngle = 0;

        private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Config.MapManager.NumDrawingsEnabled > 0)
            {
                Config.MapGui.flowLayoutPanelMapTrackers.NotifyMouseEvent(
                    MouseEvent.MouseDown, e.Button == MouseButtons.Left, e.X, e.Y);
                return;
            }

            switch (e.Button)
            {
                case MouseButtons.Left:
                    _isTranslating = true;
                    _translateStartMouseX = e.X;
                    _translateStartMouseY = e.Y;
                    _translateStartCenterX = MapViewCenterXValue;
                    _translateStartCenterZ = MapViewCenterZValue;
                    break;
                case MouseButtons.Right:
                    _isRotating = true;
                    _rotateStartMouseX = e.X;
                    _rotateStartMouseY = e.Y;
                    _rotateStartAngle = MapViewAngleValue;
                    break;
            }
        }

        private void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Config.MapManager.NumDrawingsEnabled > 0)
            {
                Config.MapGui.flowLayoutPanelMapTrackers.NotifyMouseEvent(
                    MouseEvent.MouseUp, e.Button == MouseButtons.Left, e.X, e.Y);
                return;
            }

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
            if (Config.MapManager.NumDrawingsEnabled > 0)
            {
                Config.MapGui.flowLayoutPanelMapTrackers.NotifyMouseEvent(
                    MouseEvent.MouseMove, e.Button == MouseButtons.Left, e.X, e.Y);
                return;
            }

            if (_isTranslating)
            {
                int pixelDiffX = e.X - _translateStartMouseX;
                int pixelDiffY = e.Y - _translateStartMouseY;
                pixelDiffX = MapUtilities.MaybeReverse(pixelDiffX);
                pixelDiffY = MapUtilities.MaybeReverse(pixelDiffY);
                float unitDiffX = pixelDiffX / MapViewScaleValue;
                float unitDiffY = pixelDiffY / MapViewScaleValue;
                (float rotatedX, float rotatedY) = ((float, float))
                    MoreMath.RotatePointAboutPointAnAngularDistance(
                        unitDiffX, unitDiffY, 0, 0, MapViewAngleValue);
                float newCenterX = _translateStartCenterX - rotatedX;
                float newCenterZ = _translateStartCenterZ - rotatedY;
                SetCustomCenter(newCenterX + ";" + newCenterZ);
            }

            if (_isRotating)
            {
                float angleToMouse = (float)MoreMath.AngleTo_AngleUnits(
                    _rotateStartMouseX, _rotateStartMouseY, e.X, e.Y) * MapUtilities.MaybeReverse(-1) + 32768;
                float newAngle = _rotateStartAngle + angleToMouse;
                SetCustomAngle(newAngle);
            }
        }

        private void OnScroll(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ChangeScale2(e.Delta > 0 ? 1 : -1, SpecialConfig.Map2DScrollSpeed);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (Config.MapManager.NumDrawingsEnabled > 0)
            {
                return;
            }

            Config.MapGui.radioButtonMapControllersScaleCourseDefault.Checked = true;
            Config.MapGui.radioButtonMapControllersCenterBestFit.Checked = true;
            Config.MapGui.radioButtonMapControllersAngle32768.Checked = true;
        }
    }
}
