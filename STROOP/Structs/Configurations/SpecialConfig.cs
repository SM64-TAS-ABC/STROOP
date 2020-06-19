using STROOP.Models;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class SpecialConfig
    {
        // Point vars

        // - Custom

        public static double CustomX = 0;
        public static double CustomY = 0;
        public static double CustomZ = 0;
        public static double CustomAngle = 0;

        public static double Custom2X = 0;
        public static double Custom2Y = 0;
        public static double Custom2Z = 0;
        public static double Custom2Angle = 0;

        // - Self pos

        public static PositionAngle SelfPosPA = PositionAngle.Mario;
        public static PositionAngle SelfAnglePA = PositionAngle.Mario;
        public static PositionAngle SelfPA
        {
            get => PositionAngle.Hybrid(SelfPosPA, SelfAnglePA);
        }

        public static double SelfX
        {
            get => SelfPA.X;
        }

        public static double SelfY
        {
            get => SelfPA.Y;
        }

        public static double SelfZ
        {
            get => SelfPA.Z;
        }

        public static double SelfAngle
        {
            get => SelfPA.Angle;
        }

        // - Point pos

        public static PositionAngle PointPosPA = PositionAngle.Custom;
        public static PositionAngle PointAnglePA = PositionAngle.Custom;
        public static PositionAngle PointPA
        {
            get => PositionAngle.Hybrid(PointPosPA, PointAnglePA);
        }

        public static double PointX
        {
            get => PointPA.X;
        }

        public static double PointY
        {
            get => PointPA.Y;
        }

        public static double PointZ
        {
            get => PointPA.Z;
        }

        public static double PointAngle
        {
            get => PointPA.Angle;
        }

        // - Functions

        public static bool IsSelectedPA
        {
            get => SelfPosPA.IsSelected ||
                SelfAnglePA.IsSelected ||
                PointPosPA.IsSelected ||
                PointAnglePA.IsSelected;
        }

        // Cam Hack vars

        private static double _numPans = 0;
        public static double NumPans
        {
            get => _numPans;
            set
            {
                _numPans = Math.Max(0, value);
                Config.CamHackManager.NotifyNumPanChange((int)_numPans);
            }
        }
        public static double CurrentPan
        {
            get
            {
                if (PanModels.Count == 0) return -1;
                uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                for (int i = 0; i < PanModels.Count; i++)
                {
                    if (globalTimer < PanModels[i].PanStartTime)
                    {
                        return Math.Max(0, i - 1);
                    }
                }
                return PanModels.Count - 1;
            }
        }

        private static double _panCamPos = 0;
        public static double PanCamPos
        {
            get => _panCamPos;
            set
            {
                _panCamPos = value;
                if (_panCamPos != 0) _panCamRotation = 0;
            }
        }

        private static double _panCamAngle = 0;
        public static double PanCamAngle
        {
            get => _panCamAngle;
            set
            {
                _panCamAngle = value;
                if (_panCamAngle != 0) _panCamRotation = 0;
            }
        }

        private static double _panCamRotation = 0;
        public static double PanCamRotation
        {
            get => _panCamRotation;
            set
            {
                _panCamRotation = value;
                if (_panCamRotation != 0)
                {
                    _panCamPos = 0;
                    _panCamAngle = 0;
                }
            }
        }

        public static double PanFOV = 0;

        public static List<PanModel> PanModels = new List<PanModel>();

        // Rng vars

        public static int GoalRngIndex
        {
            get => RngIndexer.GetRngIndex(GoalRngValue);
            set => GoalRngValue = RngIndexer.GetRngValue(value);
        }
        public static ushort GoalRngValue = 0;
        
        // PU vars

        public static int PuParam1 = 0;
        public static int PuParam2 = 1;

        public static double PuHypotenuse
        {
            get => MoreMath.GetHypotenuse(PuParam1, PuParam2);
        }

        // Mupen vars

        public static int MupenLagOffset = 0;

        // Map3D

        public static Map3DCameraMode Map3DMode = Map3DCameraMode.InGame;
        public static float Map3DCameraX = 0;
        public static float Map3DCameraY = 0;
        public static float Map3DCameraZ = 0;
        public static float Map3DCameraYaw = 0;
        public static float Map3DCameraPitch = 0;
        public static float Map3DCameraRoll = 0;
        public static float Map3DFocusX = 0;
        public static float Map3DFocusY = 0;
        public static float Map3DFocusZ = 0;
        public static PositionAngle Map3DCameraPosPA = PositionAngle.None;
        public static PositionAngle Map3DCameraAnglePA = PositionAngle.None;
        public static PositionAngle Map3DFocusPosPA = PositionAngle.None;
        public static PositionAngle Map3DFocusAnglePA = PositionAngle.None;
        public static float Map3DFollowingRadius = 1000;
        public static float Map3DFollowingYOffset = 1000;
        public static float Map3DFollowingYaw = 0;
        public static float Map3DFOV = 0;

        public static double Map2DScrollSpeed = 1.1;
        public static double Map3DScrollSpeed = 100;
        public static double Map3DTranslateSpeed = 20;
        public static double Map3DRotateSpeed = 50;

        public static CompassPosition CompassPosition;
        public static float CompassLineHeight = 50;
        public static float CompassLineWidth = 10;
        public static float CompassArrowHeight = 50;
        public static float CompassArrowWidth = 60;
        public static float CompassHorizontalMargin = 10;
        public static float CompassVerticalMargin = 10;
        public static float CompassDirectionTextSize = 80;
        public static float CompassAngleTextSize = 80;
        public static float CompassDirectionTextPosition = 0;
        public static float CompassAngleTextPosition = 50;
        public static float CompassShowDirectionText = 1;
        public static float CompassShowAngleText = 0;
        public static float CompassAngleTextSigned = 0;

        public static float CompassCenterX
        {
            get
            {
                float relCenterX = CompassHorizontalMargin + CompassArrowHeight + CompassLineHeight + CompassLineWidth / 2;
                switch (CompassPosition)
                {
                    case CompassPosition.TopLeft:
                    case CompassPosition.BottomLeft:
                        return relCenterX;
                    case CompassPosition.TopRight:
                    case CompassPosition.BottomRight:
                        return Config.MapGui.GLControlMap2D.Width - relCenterX;
                    case CompassPosition.Center:
                        return Config.MapGui.GLControlMap2D.Width / 2;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public static float CompassCenterZ
        {
            get
            {
                float relCenterZ = CompassVerticalMargin + CompassArrowHeight + CompassLineHeight + CompassLineWidth / 2;
                switch (CompassPosition)
                {
                    case CompassPosition.TopLeft:
                    case CompassPosition.TopRight:
                        return relCenterZ;
                    case CompassPosition.BottomLeft:
                    case CompassPosition.BottomRight:
                        return Config.MapGui.GLControlMap2D.Height - relCenterZ;
                    case CompassPosition.Center:
                        return Config.MapGui.GLControlMap2D.Height / 2;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        // Dummy Vars

        public static readonly List<double> DummyValues = new List<double>();
    }
}
