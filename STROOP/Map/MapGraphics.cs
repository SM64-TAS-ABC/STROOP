using OpenTK;
using OpenTK.Graphics.OpenGL;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapGraphics
    {
        private GLControl _glControl;

        public enum MapScale { CourseDefault, MaxCourseSize, Custom };
        public enum MapCenter { BestFit, Origin, Mario, Custom };
        public enum MapYaw { Angle0, Angle16384, Angle32768, Angle49152, Mario, MarioSide, Camera, Centripetal, Custom };
        public enum MapDragAbility { HorizontalAndVertical, HorizontalOnly, VerticalOnly };

        public MapScale MapViewScale;
        public MapCenter MapViewCenter;
        public MapYaw MapViewYaw;
        public bool MapViewScaleWasCourseDefault = true;
        public MapDragAbility MapViewCenterDragAbility = MapDragAbility.HorizontalAndVertical;
        public MapDragAbility MapViewYawDragAbility = MapDragAbility.HorizontalAndVertical;

        private static readonly float DEFAULT_MAP_VIEW_SCALE_VALUE = 1;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_X_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_Y_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_CENTER_Z_VALUE = 0;
        private static readonly float DEFAULT_MAP_VIEW_YAW_VALUE = 32768;
        private static readonly float DEFAULT_MAP_VIEW_PITCH_VALUE = 0;
        private static float MAP_VIEW_PITCH_MIN_VALUE { get => -16384 + (float)MapConfig.Map2DOrthographicVerticalRotateSpeed; }
        private static float MAP_VIEW_PITCH_MAX_VALUE { get => 16384 - (float)MapConfig.Map2DOrthographicVerticalRotateSpeed; }

        public float MapViewScaleValue = DEFAULT_MAP_VIEW_SCALE_VALUE;
        public float MapViewCenterXValue = DEFAULT_MAP_VIEW_CENTER_X_VALUE;
        public float MapViewCenterYValue = DEFAULT_MAP_VIEW_CENTER_Y_VALUE;
        public float MapViewCenterZValue = DEFAULT_MAP_VIEW_CENTER_Z_VALUE;
        public float MapViewYawValue = DEFAULT_MAP_VIEW_YAW_VALUE;
        public float MapViewPitchValue = DEFAULT_MAP_VIEW_PITCH_VALUE;

        public float MapViewRadius { get => (float)MoreMath.GetHypotenuse(
            _glControl.Width / 2, _glControl.Height / 2) / MapViewScaleValue; }
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

        private readonly bool _isMainGraphics;
        public bool IsOrthographicViewEnabled;

        public MapGraphics(bool isMainGraphics, bool isOrthographicViewEnabled)
        {
            _isMainGraphics = isMainGraphics;
            IsOrthographicViewEnabled = isOrthographicViewEnabled;
        }

        public void Load(GLControl glControl)
        {
            _glControl = glControl;

            _glControl.MakeCurrent();
            _glControl.Context.LoadAll();

            _glControl.Paint += (sender, e) => OnPaint();

            _glControl.MouseDown += OnMouseDown;
            _glControl.MouseUp += OnMouseUp;
            _glControl.MouseMove += OnMouseMove;
            _glControl.MouseWheel += OnScroll;
            _glControl.DoubleClick += OnDoubleClick;

            GL.ClearColor(Color.FromKnownColor(KnownColor.Control));
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        private void OnPaint()
        {
            Config.CurrentMapGraphics = this;
            Config.MapGui.CurrentControl = _glControl;
            _glControl.MakeCurrent();

            UpdateCursor();
            UpdateDraggedObject();
            UpdateViewport();
            UpdateMapView();

            // Set default background color (clear drawing area)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);

            Config.MapGui.flowLayoutPanelMapTrackers.DrawOn2DControl(_isMainGraphics);

            _glControl.SwapBuffers();
        }

        private void UpdateCursor()
        {
            Cursor cursor;
            if (Config.MapManager.NumDrawingsEnabled > 0)
            {
                cursor = Cursors.Cross;
            }
            else if (Config.MapGui.checkBoxMapOptionsEnableObjectDrag.Checked)
            {
                if (Config.MapGui.flowLayoutPanelMapTrackers.ObjectDragData != null)
                {
                    cursor = Cursors.Hand;
                }
                else
                {
                    cursor = Cursors.Default;
                }
            }
            else
            {
                cursor = Cursors.Hand;
            }
            if (_glControl.Cursor != cursor)
            {
                _glControl.Cursor = cursor;
            }
        }

        private void UpdateDraggedObject()
        {
            if (DraggedObject != null)
            {
                DraggedObject = DraggedObject.MapObject?.GetHoverData(true, true) ?? DraggedObject;
            }
        }

        private void UpdateViewport()
        {
            int w = _glControl.Width;
            int h = _glControl.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(0, w, h, 0, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void UpdateMapView()
        {
            if (!_isMainGraphics) return;
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
                            x, z, 0, 0, 32768 - Config.CurrentMapGraphics.MapViewYawValue);
                        return ((float)rotatedX, (float)rotatedZ);
                    });
                    float rotatedXMax = rotatedCoordinates.Max(coord => coord.Item1);
                    float rotatedXMin = rotatedCoordinates.Min(coord => coord.Item1);
                    float rotatedZMax = rotatedCoordinates.Max(coord => coord.Item2);
                    float rotatedZMin = rotatedCoordinates.Min(coord => coord.Item2);
                    float rotatedWidth = rotatedXMax - rotatedXMin;
                    float rotatedHeight = rotatedZMax - rotatedZMin;
                    MapViewScaleValue = Math.Min(
                        _glControl.Width / rotatedWidth, _glControl.Height / rotatedHeight);
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
                    MapViewCenterXValue = 0;
                    MapViewCenterYValue = 0;
                    MapViewCenterZValue = 0;
                    break;
                case MapCenter.Mario:
                    MapViewCenterXValue = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                    MapViewCenterYValue = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    MapViewCenterZValue = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
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

            if (IsOrthographicViewEnabled)
            {
                float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                if (Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked)
                {
                    if (MapViewPitchValue == 0 && (MapViewYawValue == 0 || MapViewYawValue == 32768))
                    {
                        MapViewCenterZValue = marioZ;
                    }
                    else if (MapViewPitchValue == 0 && (MapViewYawValue == 16384 || MapViewYawValue == 49152))
                    {
                        MapViewCenterXValue = marioX;
                    }
                    else
                    {
                        (double x, double y, double z) = MoreMath.GetPlanePointAtPoint(
                            MapViewCenterXValue, MapViewCenterYValue, MapViewCenterZValue, MapViewYawValue, MapViewPitchValue, marioX, marioY, marioZ);
                        MapViewCenterXValue = (float)x;
                        MapViewCenterYValue = (float)y;
                        MapViewCenterZValue = (float)z;
                    }
                }
                else
                {
                    MapViewCenterYValue = marioY;
                }
            }

            if (MapViewCenter != MapCenter.Custom)
            {
                SetCenterTextbox(MapViewCenterXValue, MapViewCenterYValue, MapViewCenterZValue);
            }
        }

        private void UpdateAngle()
        {
            if (Config.MapGui.radioButtonMapControllersAngle0.Checked)
                MapViewYaw = MapYaw.Angle0;
            else if (Config.MapGui.radioButtonMapControllersAngle16384.Checked)
                MapViewYaw = MapYaw.Angle16384;
            else if (Config.MapGui.radioButtonMapControllersAngle32768.Checked)
                MapViewYaw = MapYaw.Angle32768;
            else if (Config.MapGui.radioButtonMapControllersAngle49152.Checked)
                MapViewYaw = MapYaw.Angle49152;
            else if (Config.MapGui.radioButtonMapControllersAngleMario.Checked)
                MapViewYaw = MapYaw.Mario;
            else if (Config.MapGui.radioButtonMapControllersAngleMarioSide.Checked)
                MapViewYaw = MapYaw.MarioSide;
            else if (Config.MapGui.radioButtonMapControllersAngleCamera.Checked)
                MapViewYaw = MapYaw.Camera;
            else if (Config.MapGui.radioButtonMapControllersAngleCentripetal.Checked)
                MapViewYaw = MapYaw.Centripetal;
            else
                MapViewYaw = MapYaw.Custom;

            switch (MapViewYaw)
            {
                case MapYaw.Angle0:
                    MapViewYawValue = 0;
                    break;
                case MapYaw.Angle16384:
                    MapViewYawValue = 16384;
                    break;
                case MapYaw.Angle32768:
                    MapViewYawValue = 32768;
                    break;
                case MapYaw.Angle49152:
                    MapViewYawValue = 49152;
                    break;
                case MapYaw.Mario:
                    MapViewYawValue = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    break;
                case MapYaw.MarioSide:
                    MapViewYawValue = MoreMath.NormalizeAngleUshort(
                        Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset) + 16384);
                    break;
                case MapYaw.Camera:
                    MapViewYawValue = Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    break;
                case MapYaw.Centripetal:
                    MapViewYawValue = (float)MoreMath.ReverseAngle(
                        Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset));
                    break;
                case MapYaw.Custom:
                    PositionAngle posAngle = PositionAngle.FromString(
                        Config.MapGui.textBoxMapControllersAngleCustom.LastSubmittedText);
                    if (posAngle != null)
                    {
                        MapViewYawValue = (float)posAngle.Angle;
                        break;
                    }

                    List<string> stringValues = ParsingUtilities.ParseStringList(
                        Config.MapGui.textBoxMapControllersAngleCustom.LastSubmittedText, replaceComma: false);
                    if (stringValues.Count == 1)
                    {
                        MapViewYawValue = ParsingUtilities.ParseFloatNullable(stringValues[0]) ?? DEFAULT_MAP_VIEW_YAW_VALUE;
                    }
                    else if (stringValues.Count == 2)
                    {
                        MapViewYawValue = ParsingUtilities.ParseFloatNullable(stringValues[0]) ?? DEFAULT_MAP_VIEW_YAW_VALUE;
                        MapViewPitchValue = ParsingUtilities.ParseFloatNullable(stringValues[1]) ?? DEFAULT_MAP_VIEW_PITCH_VALUE;
                    }
                    break;
            }

            if (MapViewYaw != MapYaw.Custom)
            {
                SetAngleTextbox(MapViewYawValue, MapViewPitchValue);
            }
        }

        public void SetScale(MapScale mapScale)
        {
            MapViewScale = mapScale;
            switch (mapScale)
            {
                case MapScale.CourseDefault:
                    Config.MapGui.radioButtonMapControllersScaleCourseDefault.Checked = true;
                    break;
                case MapScale.MaxCourseSize:
                    Config.MapGui.radioButtonMapControllersScaleMaxCourseSize.Checked = true;
                    break;
                case MapScale.Custom:
                    Config.MapGui.radioButtonMapControllersScaleCustom.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetCenter(MapCenter mapCenter)
        {
            MapViewCenter = mapCenter;
            switch (mapCenter)
            {
                case MapCenter.BestFit:
                    Config.MapGui.radioButtonMapControllersCenterBestFit.Checked = true;
                    break;
                case MapCenter.Origin:
                    Config.MapGui.radioButtonMapControllersCenterOrigin.Checked = true;
                    break;
                case MapCenter.Mario:
                    Config.MapGui.radioButtonMapControllersCenterMario.Checked = true;
                    break;
                case MapCenter.Custom:
                    Config.MapGui.radioButtonMapControllersCenterCustom.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetYaw(MapYaw mapYaw)
        {
            MapViewYaw = mapYaw;
            switch (mapYaw)
            {
                case MapYaw.Angle0:
                    Config.MapGui.radioButtonMapControllersAngle0.Checked = true;
                    break;
                case MapYaw.Angle16384:
                    Config.MapGui.radioButtonMapControllersAngle16384.Checked = true;
                    break;
                case MapYaw.Angle32768:
                    Config.MapGui.radioButtonMapControllersAngle32768.Checked = true;
                    break;
                case MapYaw.Angle49152:
                    Config.MapGui.radioButtonMapControllersAngle49152.Checked = true;
                    break;
                case MapYaw.Mario:
                    Config.MapGui.radioButtonMapControllersAngleMario.Checked = true;
                    break;
                case MapYaw.MarioSide:
                    Config.MapGui.radioButtonMapControllersAngleMarioSide.Checked = true;
                    break;
                case MapYaw.Camera:
                    Config.MapGui.radioButtonMapControllersAngleCamera.Checked = true;
                    break;
                case MapYaw.Centripetal:
                    Config.MapGui.radioButtonMapControllersAngleCentripetal.Checked = true;
                    break;
                case MapYaw.Custom:
                    Config.MapGui.radioButtonMapControllersAngleCustom.Checked = true;
                    break;
                default:
                    break;
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
            float newScaleValue = MapViewScaleValue * (float)Math.Pow(parsed.Value, power);
            if (_isMainGraphics)
            {
                Config.MapGui.radioButtonMapControllersScaleCustom.Checked = true;
                Config.MapGui.textBoxMapControllersScaleCustom.SubmitText(newScaleValue.ToString());
            }
            else
            {
                MapViewScaleValue = newScaleValue;
            }
        }

        public void ChangeCenter(int horizontalSign, int verticalSign, int depthSign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.MapGui.radioButtonMapControllersCenterCustom.Checked = true;
            if (depthSign != 0) Config.MapGui.checkBoxMapControllersCenterUseMarioDepth.Checked = false;
            float xOffset, yOffset, zOffset;
            if (IsOrthographicViewEnabled)
            {
                double yawRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewYawValue);
                double pitchRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewPitchValue);
                xOffset = (float)(
                    Math.Cos(yawRadians) * -1 * horizontalSign * parsed.Value +
                    Math.Cos(pitchRadians) * Math.Sin(yawRadians) * -1 * depthSign * parsed.Value +
                    Math.Sin(pitchRadians) * Math.Sin(yawRadians) * -1 * verticalSign * parsed.Value);
                yOffset = (float)(
                    Math.Cos(pitchRadians) * verticalSign * parsed.Value +
                    Math.Sin(pitchRadians) * -1 * depthSign * parsed.Value);
                zOffset = (float)(
                    Math.Sin(yawRadians) * horizontalSign * parsed.Value +
                    Math.Cos(pitchRadians) * Math.Cos(yawRadians) * -1 * depthSign * parsed.Value +
                    Math.Sin(pitchRadians) * Math.Cos(yawRadians) * -1 * verticalSign * parsed.Value);
            }
            else
            {
                xOffset = horizontalSign * parsed.Value;
                yOffset = depthSign * parsed.Value;
                zOffset = -1 * verticalSign * parsed.Value;
                (xOffset, zOffset) = ((float, float))MoreMath.RotatePointAboutPointAnAngularDistance(
                    xOffset, zOffset, 0, 0, Config.CurrentMapGraphics.MapViewYawValue);
            }
            float multiplier = Config.MapGui.checkBoxMapControllersCenterChangeByPixels.Checked ? 1 / MapViewScaleValue : 1;
            float newCenterXValue = MapViewCenterXValue + xOffset * multiplier;
            float newCenterYValue = MapViewCenterYValue + yOffset * multiplier;
            float newCenterZValue = MapViewCenterZValue + zOffset * multiplier;
            SetCenterTextbox(newCenterXValue, newCenterYValue, newCenterZValue);
        }

        public void ChangeYaw(int sign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            Config.MapGui.radioButtonMapControllersAngleCustom.Checked = true;
            float newYawValue = MapViewYawValue + sign * parsed.Value;
            newYawValue = (float)MoreMath.NormalizeAngleDouble(newYawValue);
            SetAngleTextbox(newYawValue, MapViewPitchValue);
        }

        public void ChangePitch(int sign, object value)
        {
            float? parsed = ParsingUtilities.ParseFloatNullable(value);
            if (!parsed.HasValue) return;
            float newPitchValue = (float)MoreMath.Clamp(
                MapViewPitchValue + sign * parsed.Value, MAP_VIEW_PITCH_MIN_VALUE, MAP_VIEW_PITCH_MAX_VALUE);
            MapViewPitchValue = newPitchValue;
            SetAngleTextbox(MapViewYawValue, newPitchValue);
        }

        public void SetCustomScale(object value)
        {
            if (_isMainGraphics) Config.MapGui.radioButtonMapControllersScaleCustom.Checked = true;
            Config.MapGui.textBoxMapControllersScaleCustom.SubmitText(value.ToString());
        }

        public void SetCustomCenter(object xValue = null, object yValue = null, object zValue = null)
        {
            if (_isMainGraphics) Config.MapGui.radioButtonMapControllersCenterCustom.Checked = true;
            xValue = xValue ?? MapViewCenterXValue;
            yValue = yValue ?? MapViewCenterYValue;
            zValue = zValue ?? MapViewCenterZValue;
            SetCenterTextbox(xValue, yValue, zValue);
        }

        private void SetCenterTextbox(object xValue, object yValue, object zValue)
        {
            MapViewCenterXValue = ParsingUtilities.ParseFloat(xValue);
            MapViewCenterYValue = ParsingUtilities.ParseFloat(yValue);
            MapViewCenterZValue = ParsingUtilities.ParseFloat(zValue);

            if (_isMainGraphics)
            {
                List<object> values = new List<object> { xValue, yValue, zValue };
                Config.MapGui.textBoxMapControllersCenterCustom.SubmitTextLoosely(string.Join(";", values));
            }
        }

        public void SetCustomYaw(object value)
        {
            if (_isMainGraphics) Config.MapGui.radioButtonMapControllersAngleCustom.Checked = true;
            SetAngleTextbox(value, MapViewPitchValue);
        }

        public void SetCustomPitch(object value)
        {
            if (_isMainGraphics) Config.MapGui.radioButtonMapControllersAngleCustom.Checked = true;
            SetAngleTextbox(MapViewYawValue, value);
        }

        public void SetCustomAngle(object yaw, object pitch)
        {
            if(_isMainGraphics) Config.MapGui.radioButtonMapControllersAngleCustom.Checked = true;
            SetAngleTextbox(yaw, pitch);
        }

        private void SetAngleTextbox(object yawValue, object pitchValue)
        {
            List<object> values = new List<object> { yawValue, pitchValue };
            if (_isMainGraphics)
            {
                Config.MapGui.textBoxMapControllersAngleCustom.SubmitTextLoosely(string.Join(";", values));
            }
            else
            {
                MapViewYawValue = ParsingUtilities.ParseFloat(yawValue);
                MapViewPitchValue = ParsingUtilities.ParseFloat(pitchValue);
            }
        }

        private bool _isTranslating = false;
        private int _translateStartMouseX = 0;
        private int _translateStartMouseY = 0;
        private float _translateStartCenterX = 0;
        private float _translateStartCenterY = 0;
        private float _translateStartCenterZ = 0;

        private bool _isRotating = false;
        private int _rotateStartMouseX = 0;
        private int _rotateStartMouseY = 0;
        private float _rotateStartYaw = 0;
        private float _rotateStartPitch = 0;

        public MapObjectHoverData DraggedObject = null;
        private int _objectDragStartMouseX = 0;
        private int _objectDragStartMouseY = 0;
        private float _objectDragStartX = 0;
        private float _objectDragStartY = 0;
        private float _objectDragStartZ = 0;

        private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Config.MapManager.NumDrawingsEnabled > 0)
            {
                Config.MapGui.flowLayoutPanelMapTrackers.NotifyMouseEvent(
                    MouseEvent.MouseDown, e.Button == MouseButtons.Left, e.X, e.Y);
                return;
            }

            if (Config.MapGui.checkBoxMapOptionsEnableObjectDrag.Checked)
            {
                if (Config.MapGui.flowLayoutPanelMapTrackers.ObjectDragData != null)
                {
                    var dragPosStart = Config.MapGui.flowLayoutPanelMapTrackers.ObjectDragData.GetDragPosition();
                    if (dragPosStart.HasValue)
                    {
                        DraggedObject = Config.MapGui.flowLayoutPanelMapTrackers.ObjectDragData;
                        _objectDragStartMouseX = e.X;
                        _objectDragStartMouseY = e.Y;
                        _objectDragStartX = (float)dragPosStart.Value.x;
                        _objectDragStartY = (float)dragPosStart.Value.y;
                        _objectDragStartZ = (float)dragPosStart.Value.z;
                    }
                    return;
                }
            }

            MouseButtons button = MapUtilities.GetMouseButton(e);
            switch (button)
            {
                case MouseButtons.Left:
                    _isTranslating = true;
                    _translateStartMouseX = e.X;
                    _translateStartMouseY = e.Y;
                    _translateStartCenterX = MapViewCenterXValue;
                    _translateStartCenterY = MapViewCenterYValue;
                    _translateStartCenterZ = MapViewCenterZValue;
                    break;
                case MouseButtons.Right:
                    _isRotating = true;
                    _rotateStartMouseX = e.X;
                    _rotateStartMouseY = e.Y;
                    _rotateStartYaw = MapViewYawValue;
                    _rotateStartPitch = MapViewPitchValue;
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

            if (DraggedObject != null)
            {
                DraggedObject = null;
                return;
            }

            MouseButtons button = MapUtilities.GetMouseButton(e);
            switch (button)
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

            if (DraggedObject != null)
            {
                if (IsOrthographicViewEnabled)
                {
                    int pixelDiffX = e.X - _objectDragStartMouseX;
                    int pixelDiffY = e.Y - _objectDragStartMouseY;
                    double unitDiffX = pixelDiffX / (double)MapViewScaleValue;
                    double unitDiffY = pixelDiffY / (double)MapViewScaleValue;
                    double newObjX, newObjY, newObjZ;
                    if (MapViewPitchValue == 0 && MapViewYawValue == 0)
                    {
                        newObjX = _objectDragStartX - unitDiffX;
                        newObjY = _objectDragStartY - unitDiffY;
                        newObjZ = _objectDragStartZ;
                    }
                    else if (MapViewPitchValue == 0 && MapViewYawValue == 16384)
                    {
                        newObjX = _objectDragStartX;
                        newObjY = _objectDragStartY - unitDiffY;
                        newObjZ = _objectDragStartZ + unitDiffX;
                    }
                    else if (MapViewPitchValue == 0 && MapViewYawValue == 32768)
                    {
                        newObjX = _objectDragStartX + unitDiffX;
                        newObjY = _objectDragStartY - unitDiffY;
                        newObjZ = _objectDragStartZ;
                    }
                    else if (MapViewPitchValue == 0 && MapViewYawValue == 49152)
                    {
                        newObjX = _objectDragStartX;
                        newObjY = _objectDragStartY - unitDiffY;
                        newObjZ = _objectDragStartZ - unitDiffX;
                    }
                    else
                    {
                        double yawRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewYawValue);
                        double pitchRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewPitchValue);
                        double xOffset =
                            Math.Cos(yawRadians) * unitDiffX +
                            Math.Sin(pitchRadians) * -1 * Math.Sin(yawRadians) * unitDiffY;
                        double yOffset =
                            Math.Cos(pitchRadians) * unitDiffY;
                        double zOffset =
                            Math.Sin(yawRadians) * -1 * unitDiffX +
                            Math.Sin(pitchRadians) * -1 * Math.Cos(yawRadians) * unitDiffY;
                        newObjX = _objectDragStartX - xOffset;
                        newObjY = _objectDragStartY - yOffset;
                        newObjZ = _objectDragStartZ - zOffset;
                    }
                    DraggedObject.SetDragPositionOrthographicView(x: newObjX, y: newObjY, z: newObjZ);
                    return;
                }
                else
                {
                    int pixelDiffX = e.X - _objectDragStartMouseX;
                    int pixelDiffY = e.Y - _objectDragStartMouseY;
                    double unitDiffX = pixelDiffX / MapViewScaleValue;
                    double unitDiffY = pixelDiffY / MapViewScaleValue;
                    (double rotatedX, double rotatedY) =
                        MoreMath.RotatePointAboutPointAnAngularDistance(
                            unitDiffX, unitDiffY, 0, 0, MapViewYawValue);
                    double newObjX = _objectDragStartX + rotatedX;
                    double newObjZ = _objectDragStartZ + rotatedY;
                    DraggedObject.SetDragPositionTopDownView(x: newObjX, z: newObjZ);
                    return;
                }
            }

            if (_isTranslating)
            {
                int pixelDiffX = HandleDragAbility(true, true, e.X - _translateStartMouseX);
                int pixelDiffY = HandleDragAbility(false, true, e.Y - _translateStartMouseY);
                pixelDiffX = MapUtilities.MaybeReverse(pixelDiffX);
                pixelDiffY = MapUtilities.MaybeReverse(pixelDiffY);
                double unitDiffX = pixelDiffX / (double)MapViewScaleValue;
                double unitDiffY = pixelDiffY / (double)MapViewScaleValue;
                double newCenterX, newCenterY, newCenterZ;
                if (IsOrthographicViewEnabled)
                {
                    if (MapViewPitchValue == 0 && MapViewYawValue == 0)
                    {
                        newCenterX = _translateStartCenterX + unitDiffX;
                        newCenterY = _translateStartCenterY + unitDiffY;
                        newCenterZ = _translateStartCenterZ;
                    }
                    else if (MapViewPitchValue == 0 && MapViewYawValue == 16384)
                    {
                        newCenterX = _translateStartCenterX;
                        newCenterY = _translateStartCenterY + unitDiffY;
                        newCenterZ = _translateStartCenterZ - unitDiffX;
                    }
                    else if (MapViewPitchValue == 0 && MapViewYawValue == 32768)
                    {
                        newCenterX = _translateStartCenterX - unitDiffX;
                        newCenterY = _translateStartCenterY + unitDiffY;
                        newCenterZ = _translateStartCenterZ;
                    }
                    else if (MapViewPitchValue == 0 && MapViewYawValue == 49152)
                    {
                        newCenterX = _translateStartCenterX;
                        newCenterY = _translateStartCenterY + unitDiffY;
                        newCenterZ = _translateStartCenterZ + unitDiffX;
                    }
                    else
                    {
                        double yawRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewYawValue);
                        double pitchRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewPitchValue);
                        double xOffset =
                            Math.Cos(yawRadians) * unitDiffX +
                            Math.Sin(pitchRadians) * -1 * Math.Sin(yawRadians) * unitDiffY;
                        double yOffset =
                            Math.Cos(pitchRadians) * unitDiffY;
                        double zOffset =
                            Math.Sin(yawRadians) * -1 * unitDiffX +
                            Math.Sin(pitchRadians) * -1 * Math.Cos(yawRadians) * unitDiffY;
                        newCenterX = _translateStartCenterX + xOffset;
                        newCenterY = _translateStartCenterY + yOffset;
                        newCenterZ = _translateStartCenterZ + zOffset;
                    }
                }
                else
                {
                    (double rotatedX, double rotatedY) =
                        MoreMath.RotatePointAboutPointAnAngularDistance(
                            unitDiffX, unitDiffY, 0, 0, MapViewYawValue);
                    newCenterX = _translateStartCenterX - rotatedX;
                    newCenterY = _translateStartCenterY;
                    newCenterZ = _translateStartCenterZ - rotatedY;
                }
                SetCustomCenter(newCenterX, newCenterY, newCenterZ);
            }

            if (_isRotating)
            {
                bool isRightClickingForHover = Config.MapGui.checkBoxMapOptionsEnableObjectHover.Checked && _isMainGraphics;
                if (!isRightClickingForHover)
                {
                    if (IsOrthographicViewEnabled)
                    {
                        int pixelDiffX = HandleDragAbility(true, false, e.X - _rotateStartMouseX);
                        int pixelDiffY = HandleDragAbility(false, false, e.Y - _rotateStartMouseY);
                        pixelDiffX = MapUtilities.MaybeReverse(pixelDiffX);
                        pixelDiffY = MapUtilities.MaybeReverse(pixelDiffY);
                        float yawDiff = (float)(pixelDiffX * MapConfig.Map2DOrthographicHorizontalRotateSpeed);
                        float pitchDiff = (float)(pixelDiffY * MapConfig.Map2DOrthographicVerticalRotateSpeed);
                        float newYaw = _rotateStartYaw - yawDiff;
                        float newPitch = _rotateStartPitch - pitchDiff;
                        newYaw = (float)MoreMath.NormalizeAngleDouble(newYaw);
                        newPitch = (float)MoreMath.Clamp(newPitch, MAP_VIEW_PITCH_MIN_VALUE, MAP_VIEW_PITCH_MAX_VALUE);
                        if (KeyboardUtilities.IsCtrlHeld())
                        {
                            newYaw = (float)MoreMath.NormalizeAngle45Degrees(newYaw);
                        }
                        SetCustomAngle(newYaw, newPitch);
                    }
                    else
                    {
                        float angleToMouse = (float)MoreMath.AngleTo_AngleUnits(
                            _rotateStartMouseX, _rotateStartMouseY, e.X, e.Y) * MapUtilities.MaybeReverse(-1) + 32768;
                        float newAngle = _rotateStartYaw + angleToMouse;
                        if (KeyboardUtilities.IsCtrlHeld())
                        {
                            newAngle = (float)MoreMath.NormalizeAngle45Degrees(newAngle);
                        }
                        SetCustomYaw(newAngle);
                    }
                }
            }
        }

        private int HandleDragAbility(bool isHorizontal, bool isCenter, int value)
        {
            MapDragAbility dragAbility = isCenter ? MapViewCenterDragAbility : MapViewYawDragAbility;
            if (isHorizontal)
            {
                return dragAbility == MapDragAbility.VerticalOnly ? 0 : value;
            }
            else
            {
                return dragAbility == MapDragAbility.HorizontalOnly ? 0 : value;
            }
        }

        private void OnScroll(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ChangeScale2(e.Delta > 0 ? 1 : -1, MapConfig.Map2DZoomSpeed);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (!_isMainGraphics) return;

            if (Config.MapManager.NumDrawingsEnabled > 0)
            {
                return;
            }

            Config.MapGui.radioButtonMapControllersScaleCourseDefault.Checked = true;
            Config.MapGui.radioButtonMapControllersCenterBestFit.Checked = true;
            Config.MapGui.radioButtonMapControllersAngle32768.Checked = true;
            MapViewPitchValue = 0;
        }
    }
}
