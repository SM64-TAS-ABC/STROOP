using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Extensions;
using System.Reflection;
using STROOP.Managers;
using STROOP.Structs.Configurations;

namespace STROOP.Map
{
    public class MapObjectSettings
    {
        public readonly bool CustomCylinderChangeRelativeMinY;
        public readonly float CustomCylinderNewRelativeMinY;

        public readonly bool CustomCylinderChangeRelativeMaxY;
        public readonly float CustomCylinderNewRelativeMaxY;

        public readonly bool TriangleChangeMinHeight;
        public readonly float? TriangleNewMinHeight;

        public readonly bool TriangleChangeMaxHeight;
        public readonly float? TriangleNewMaxHeight;

        public readonly bool WallChangeShowArrows;
        public readonly bool WallNewShowArrows;

        public readonly bool WallChangeRelativeHeight;
        public readonly float? WallNewRelativeHeight;

        public readonly bool WallChangeAbsoluteHeight;
        public readonly float? WallNewAbsoluteHeight;

        public readonly bool ArrowChangeUseRecommendedLength;
        public readonly bool ArrowNewUseRecommendedLength;

        public readonly bool ArrowChangeHeadSideLength;
        public readonly float ArrowNewHeadSideLength;

        public readonly bool SectorChangeAngleRadius;
        public readonly float SectorNewAngleRadius;

        public readonly bool PathDoReset;

        public readonly bool PathChangeResetPathOnLevelChange;
        public readonly bool PathNewResetPathOnLevelChange;

        public readonly bool PathChangeUseBlending;
        public readonly bool PathNewUseBlending;

        public readonly bool PathChangePaused;
        public readonly bool PathNewPaused;

        public readonly bool PathChangeModulo;
        public readonly int PathNewModulo;

        public readonly bool AngleRangeChangeUseRelativeAngles;
        public readonly bool AngleRangeNewUseRelativeAngles;

        public readonly bool AngleRangeChangeAngleDiff;
        public readonly int AngleRangeNewAngleDiff;

        public MapObjectSettings(
            bool customCylinderChangeRelativeMinY = false,
            float customCylinderNewRelativeMinY = 0,

            bool customCylinderChangeRelativeMaxY = false,
            float customCylinderNewRelativeMaxY = 0,

            bool triangleChangeMinHeight = false,
            float? triangleNewMinHeight = null,

            bool triangleChangeMaxHeight = false,
            float? triangleNewMaxHeight = null,

            bool wallChangeShowArrows = false,
            bool wallNewShowArrows = false,

            bool wallChangeRelativeHeight = false,
            float? wallNewRelativeHeight = null,

            bool wallChangeAbsoluteHeight = false,
            float? wallNewAbsoluteHeight = null,

            bool arrowChangeUseRecommendedLength = false,
            bool arrowNewUseRecommendedLength = false,

            bool arrowChangeHeadSideLength = false,
            float arrowNewHeadSideLength = 0,

            bool sectorChangeAngleRadius = false,
            float sectorNewAngleRadius = 0,

            bool pathDoReset = false,

            bool pathChangeResetPathOnLevelChange = false,
            bool pathNewResetPathOnLevelChange = false,

            bool pathChangeUseBlending = false,
            bool pathNewUseBlending = false,

            bool pathChangePaused = false,
            bool pathNewPaused = false,

            bool pathChangeModulo = false,
            int pathNewModulo = 0,
            
            bool angleRangeChangeUseRelativeAngles = false,
            bool angleRangeNewUseRelativeAngles = false,

            bool angleRangeChangeAngleDiff = false,
            int angleRangeNewAngleDiff = 0)
        {
            CustomCylinderChangeRelativeMinY = customCylinderChangeRelativeMinY;
            CustomCylinderNewRelativeMinY = customCylinderNewRelativeMinY;

            CustomCylinderChangeRelativeMaxY = customCylinderChangeRelativeMaxY;
            CustomCylinderNewRelativeMaxY = customCylinderNewRelativeMaxY;

            TriangleChangeMinHeight = triangleChangeMinHeight;
            TriangleNewMinHeight = triangleNewMinHeight;

            TriangleChangeMaxHeight = triangleChangeMaxHeight;
            TriangleNewMaxHeight = triangleNewMaxHeight;

            WallChangeShowArrows = wallChangeShowArrows;
            WallNewShowArrows = wallNewShowArrows;

            WallChangeRelativeHeight = wallChangeRelativeHeight;
            WallNewRelativeHeight = wallNewRelativeHeight;

            WallChangeAbsoluteHeight = wallChangeAbsoluteHeight;
            WallNewAbsoluteHeight = wallNewAbsoluteHeight;

            ArrowChangeUseRecommendedLength = arrowChangeUseRecommendedLength;
            ArrowNewUseRecommendedLength = arrowNewUseRecommendedLength;

            ArrowChangeHeadSideLength = arrowChangeHeadSideLength;
            ArrowNewHeadSideLength = arrowNewHeadSideLength;

            SectorChangeAngleRadius = sectorChangeAngleRadius;
            SectorNewAngleRadius = sectorNewAngleRadius;

            PathDoReset = pathDoReset;

            PathChangeResetPathOnLevelChange = pathChangeResetPathOnLevelChange;
            PathNewResetPathOnLevelChange = pathNewResetPathOnLevelChange;

            PathChangeUseBlending = pathChangeUseBlending;
            PathNewUseBlending = pathNewUseBlending;

            PathChangePaused = pathChangePaused;
            PathNewPaused = pathNewPaused;

            PathChangeModulo = pathChangeModulo;
            PathNewModulo = pathNewModulo;

            AngleRangeChangeUseRelativeAngles = angleRangeChangeUseRelativeAngles;
            AngleRangeNewUseRelativeAngles = angleRangeNewUseRelativeAngles;

            AngleRangeChangeAngleDiff = angleRangeChangeAngleDiff;
            AngleRangeNewAngleDiff = angleRangeNewAngleDiff;
        }
    }
}
