using STROOP.Models;
using STROOP.Utilities;
using System;
using System.Collections.Generic;

namespace STROOP.Structs.Configurations
{
    public static class MapConfig
    {
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
        public static double Map2DOrthographicHorizontalRotateSpeed = 128;
        public static double Map2DOrthographicVerticalRotateSpeed = 128;
        public static double Map3DScrollSpeed = 100;
        public static double Map3DTranslateSpeed = 20;
        public static double Map3DRotateSpeed = 50;

        public static int MapCircleNumPoints2D = 256;
        public static int MapCircleNumPoints3D = 64;

        public static double MapUnitPrecisionThreshold = 4;
        public static double MapSortOrthographicTris = 0;
        public static double MapUseNotForCeilings = 1;
        public static double MapUseXForCeilings = 0;

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
                        return Config.MapGui.CurrentControl.Width - relCenterX;
                    case CompassPosition.Center:
                        return Config.MapGui.CurrentControl.Width / 2;
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
                        return Config.MapGui.CurrentControl.Height - relCenterZ;
                    case CompassPosition.Center:
                        return Config.MapGui.CurrentControl.Height / 2;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static double CoordinateLabelsCustomSpacing = 0;
        public static double CoordinateLabelsMargin = 40;
        public static double CoordinateLabelsLabelDensity = 20;
        public static double CoordinateLabelsShowCursorPos = 1;
        public static double CoordinateLabelsShowXLabels = 1;
        public static double CoordinateLabelsShowZLabels = 1;
        public static double CoordinateLabelsUseHighX = 0;
        public static double CoordinateLabelsUseHighZ = 0;
        public static double CoordinateLabelsBoldText = 1;
    }
}
