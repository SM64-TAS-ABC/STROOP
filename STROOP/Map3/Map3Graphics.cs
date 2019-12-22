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
        private enum Map3Scale { CourseDefault, MaxCourseSize, Custom };
        private enum Map3Center { BestFit, Origin, Mario, Custom };
        private enum Map3Angle { Angle0, Angle16384, Angle32768, Angle49152, Mario, Camera, Centripetal, Custom };

        private Map3Scale MapViewScale;
        private Map3Center MapViewCenter;
        private Map3Angle MapViewAngle;
        private bool MapViewScaleWasCourseDefault = true;

        private static readonly float DEFAULT_MAP_VIEW_SCALE_VALUE = 1;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_X_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_Z_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_ANGLE_VALUE = 32768;

        public float MapViewScaleValue = DEFAULT_MAP_VIEW_SCALE_VALUE;
        public float MapViewCenterXValue = DEFAULT_MAP_VIEW_CENTER_X_VALUE;
        public float MapViewCenterZValue = DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
        public float MapViewAngleValue = DEFAULT_MAP_VIEW_ANGLE_VALUE;

        public bool MapViewEnablePuView = false;
        public bool MapViewScaleIconSizes = false;
        public bool MapViewCenterChangeByPixels = true;

        public float MapViewRadius { get => (float)MoreMath.GetHypotenuse(
            Config.Map3Gui.GLControl2D.Width / 2, Config.Map3Gui.GLControl2D.Height / 2) / MapViewScaleValue; }
        public float MapViewXMin { get => MapViewCenterXValue - MapViewRadius; }
        public float MapViewXMax { get => MapViewCenterXValue + MapViewRadius; }
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

        public Map3Graphics()
        {
        }

        public void Load()
        {
            Config.Map3Gui.GLControl2D.MakeCurrent();
            Config.Map3Gui.GLControl2D.Context.LoadAll();

            Config.Map3Gui.GLControl2D.Paint += (sender, e) => OnPaint();

            GL.ClearColor(Color.FromKnownColor(KnownColor.Control));
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        private void OnPaint()
        {
            Config.Map3Gui.GLControl2D.MakeCurrent();
            UpdateViewport();
            UpdateMapView();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

            Config.Map3Gui.flowLayoutPanelMap3Trackers.DrawOn2DControl();

            Config.Map3Gui.GLControl2D.SwapBuffers();
        }

        private void UpdateViewport()
        {
            int w = Config.Map3Gui.GLControl2D.Width;
            int h = Config.Map3Gui.GLControl2D.Height;
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
            if (Config.Map3Gui.radioButtonMap3ControllersScaleCourseDefault.Checked)
                MapViewScale = Map3Scale.CourseDefault;
            else if (Config.Map3Gui.radioButtonMap3ControllersScaleMaxCourseSize.Checked)
                MapViewScale = Map3Scale.MaxCourseSize;
            else
                MapViewScale = Map3Scale.Custom;

            if (MapViewScale == Map3Scale.CourseDefault) MapViewScaleWasCourseDefault = true;
            if (MapViewScale == Map3Scale.MaxCourseSize) MapViewScaleWasCourseDefault = false;

            switch (MapViewScale)
            {
                case Map3Scale.CourseDefault:
                case Map3Scale.MaxCourseSize:
                    RectangleF rectangle = MapViewScale == Map3Scale.CourseDefault ?
                        Map3Utilities.GetMapLayout().Coordinates : MAX_COURSE_SIZE;
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
                            x, z, 0, 0, 32768 - Config.Map3Graphics.MapViewAngleValue);
                        return ((float)rotatedX, (float)rotatedZ);
                    });
                    float rotatedXMax = rotatedCoordinates.Max(coord => coord.Item1);
                    float rotatedXMin = rotatedCoordinates.Min(coord => coord.Item1);
                    float rotatedZMax = rotatedCoordinates.Max(coord => coord.Item2);
                    float rotatedZMin = rotatedCoordinates.Min(coord => coord.Item2);
                    float rotatedWidth = rotatedXMax - rotatedXMin;
                    float rotatedHeight = rotatedZMax - rotatedZMin;
                    MapViewScaleValue = Math.Min(
                        Config.Map3Gui.GLControl2D.Width / rotatedWidth, Config.Map3Gui.GLControl2D.Height / rotatedHeight);
                    break;
                case Map3Scale.Custom:
                    MapViewScaleValue = ParsingUtilities.ParseFloatNullable(
                        Config.Map3Gui.textBoxMap3ControllersScaleCustom.LastSubmittedText)
                        ?? DEFAULT_MAP_VIEW_SCALE_VALUE;
                    break;
            }

            if (MapViewScale != Map3Scale.Custom)
            {
                Config.Map3Gui.textBoxMap3ControllersScaleCustom.SubmitTextLoosely(MapViewScaleValue.ToString());
            }
        }

        private void UpdateCenter()
        {
            if (Config.Map3Gui.radioButtonMap3ControllersCenterBestFit.Checked)
                MapViewCenter = Map3Center.BestFit;
            else if (Config.Map3Gui.radioButtonMap3ControllersCenterOrigin.Checked)
                MapViewCenter = Map3Center.Origin;
            else if (Config.Map3Gui.radioButtonMap3ControllersCenterMario.Checked)
                MapViewCenter = Map3Center.Mario;
            else
                MapViewCenter = Map3Center.Custom;

            switch (MapViewCenter)
            {
                case Map3Center.BestFit:
                    RectangleF rectangle = MapViewScaleWasCourseDefault ?
                        Map3Utilities.GetMapLayout().Coordinates : MAX_COURSE_SIZE;
                    MapViewCenterXValue = rectangle.X + rectangle.Width / 2;
                    MapViewCenterZValue = rectangle.Y + rectangle.Height / 2;
                    break;
                case Map3Center.Origin:
                    MapViewCenterXValue = 0.5f;
                    MapViewCenterZValue = 0.5f;
                    break;
                case Map3Center.Mario:
                    MapViewCenterXValue = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    MapViewCenterZValue = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    break;
                case Map3Center.Custom:
                    PositionAngle posAngle = PositionAngle.FromString(
                        Config.Map3Gui.textBoxMap3ControllersCenterCustom.LastSubmittedText);
                    if (posAngle != null)
                    {
                        MapViewCenterXValue = (float)posAngle.X;
                        MapViewCenterZValue = (float)posAngle.Z;
                        break;
                    }
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

            if (MapViewCenter != Map3Center.Custom)
            {
                Config.Map3Gui.textBoxMap3ControllersCenterCustom.SubmitTextLoosely(MapViewCenterXValue + "," + MapViewCenterZValue);
            }
        }

        private void UpdateAngle()
        {
            if (Config.Map3Gui.radioButtonMap3ControllersAngle0.Checked)
                MapViewAngle = Map3Angle.Angle0;
            else if (Config.Map3Gui.radioButtonMap3ControllersAngle16384.Checked)
                MapViewAngle = Map3Angle.Angle16384;
            else if (Config.Map3Gui.radioButtonMap3ControllersAngle32768.Checked)
                MapViewAngle = Map3Angle.Angle32768;
            else if (Config.Map3Gui.radioButtonMap3ControllersAngle49152.Checked)
                MapViewAngle = Map3Angle.Angle49152;
            else if (Config.Map3Gui.radioButtonMap3ControllersAngleMario.Checked)
                MapViewAngle = Map3Angle.Mario;
            else if (Config.Map3Gui.radioButtonMap3ControllersAngleCamera.Checked)
                MapViewAngle = Map3Angle.Camera;
            else if (Config.Map3Gui.radioButtonMap3ControllersAngleCentripetal.Checked)
                MapViewAngle = Map3Angle.Centripetal;
            else
                MapViewAngle = Map3Angle.Custom;

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
                case Map3Angle.Mario:
                    MapViewAngleValue = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    break;
                case Map3Angle.Camera:
                    MapViewAngleValue = Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    break;
                case Map3Angle.Centripetal:
                    MapViewAngleValue = (float)MoreMath.ReverseAngle(
                        Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset));
                    break;
                case Map3Angle.Custom:
                    PositionAngle posAngle = PositionAngle.FromString(
                        Config.Map3Gui.textBoxMap3ControllersAngleCustom.LastSubmittedText);
                    if (posAngle != null)
                    {
                        MapViewAngleValue = (float)posAngle.Angle;
                        break;
                    }
                    MapViewAngleValue = ParsingUtilities.ParseFloatNullable(
                        Config.Map3Gui.textBoxMap3ControllersAngleCustom.LastSubmittedText)
                        ?? DEFAULT_MAP_VIEW_ANGLE_VALUE;
                    break;
            }

            if (MapViewAngle != Map3Angle.Custom)
            {
                Config.Map3Gui.textBoxMap3ControllersAngleCustom.SubmitTextLoosely(MapViewAngleValue.ToString());
            }
        }

        public void ChangeScale(int sign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.Map3Gui.radioButtonMap3ControllersScaleCustom.Checked = true;
            float newScaleValue = MapViewScaleValue + sign * parsed.Value;
            Config.Map3Gui.textBoxMap3ControllersScaleCustom.SubmitText(newScaleValue.ToString());
        }

        public void ChangeScale2(int power, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.Map3Gui.radioButtonMap3ControllersScaleCustom.Checked = true;
            float newScaleValue = MapViewScaleValue * (float)Math.Pow(parsed.Value, power);
            Config.Map3Gui.textBoxMap3ControllersScaleCustom.SubmitText(newScaleValue.ToString());
        }

        public void ChangeCenter(int xSign, int zSign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.Map3Gui.radioButtonMap3ControllersCenterCustom.Checked = true;
            float xOffset = xSign * parsed.Value;
            float zOffset = zSign * parsed.Value;
            (float xOffsetRotated, float zOffsetRotated) = ((float, float)) MoreMath.RotatePointAboutPointAnAngularDistance(
                xOffset, zOffset, 0, 0, Config.Map3Graphics.MapViewAngleValue);
            float multiplier = MapViewCenterChangeByPixels ? 1 / MapViewScaleValue : 1;
            float newCenterXValue = MapViewCenterXValue + xOffsetRotated * multiplier;
            float newCenterZValue = MapViewCenterZValue + zOffsetRotated * multiplier;
            Config.Map3Gui.textBoxMap3ControllersCenterCustom.SubmitText(newCenterXValue + "," + newCenterZValue);
        }

        public void ChangeAngle(int sign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.Map3Gui.radioButtonMap3ControllersAngleCustom.Checked = true;
            float newAngleValue = MapViewAngleValue + sign * parsed.Value;
            newAngleValue = (float)MoreMath.NormalizeAngleDouble(newAngleValue);
            Config.Map3Gui.textBoxMap3ControllersAngleCustom.SubmitText(newAngleValue.ToString());
        }

        public void SetCustomScale(object value)
        {
            Config.Map3Gui.radioButtonMap3ControllersScaleCustom.Checked = true;
            Config.Map3Gui.textBoxMap3ControllersScaleCustom.SubmitText(value.ToString());
        }

        public void SetCustomCenter(object value)
        {
            Config.Map3Gui.radioButtonMap3ControllersCenterCustom.Checked = true;
            Config.Map3Gui.textBoxMap3ControllersCenterCustom.SubmitText(value.ToString());
        }

        public void SetCustomAngle(object value)
        {
            Config.Map3Gui.radioButtonMap3ControllersAngleCustom.Checked = true;
            Config.Map3Gui.textBoxMap3ControllersAngleCustom.SubmitText(value.ToString());
        }
    }
}
