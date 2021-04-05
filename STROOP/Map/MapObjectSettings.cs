namespace STROOP.Map
{
    public class MapObjectSettings
    {
        public readonly bool CustomCylinderChangeRelativeMinY;
        public readonly float CustomCylinderNewRelativeMinY;

        public readonly bool CustomCylinderChangeRelativeMaxY;
        public readonly float CustomCylinderNewRelativeMaxY;

        public readonly bool TriangleChangeWithinDist;
        public readonly float? TriangleNewWithinDist;

        public readonly bool TriangleChangeWithinCenter;
        public readonly float? TriangleNewWithinCenter;

        public readonly bool TriangleChangeUseCrossSection;
        public readonly bool TriangleNewUseCrossSection;

        public readonly bool HorizontalTriangleChangeMinHeight;
        public readonly float? HorizontalTriangleNewMinHeight;

        public readonly bool HorizontalTriangleChangeMaxHeight;
        public readonly float? HorizontalTriangleNewMaxHeight;

        public readonly bool FloorChangeExcludeDeathBarriers;
        public readonly bool FloorNewExcludeDeathBarriers;

        public readonly bool FloorChangeEnableQuarterFrameLandings;
        public readonly bool FloorNewEnableQuarterFrameLandings;

        public readonly bool WallChangeShowArrows;
        public readonly bool WallNewShowArrows;

        public readonly bool WallChangeRelativeHeight;
        public readonly float? WallNewRelativeHeight;

        public readonly bool WallChangeAbsoluteHeight;
        public readonly float? WallNewAbsoluteHeight;

        public readonly bool ArrowChangeUseRecommendedLength;
        public readonly bool ArrowNewUseRecommendedLength;

        public readonly bool ArrowChangeUseTruncatedAngle;
        public readonly bool ArrowNewUseTruncatedAngle;

        public readonly bool ArrowChangeHeadSideLength;
        public readonly float ArrowNewHeadSideLength;

        public readonly bool ArrowChangeAngleOffset;
        public readonly float ArrowNewAngleOffset;

        public readonly bool SectorChangeAngleRadius;
        public readonly float SectorNewAngleRadius;

        public readonly bool PathDoReset;

        public readonly bool PathDoCopyPoints;

        public readonly bool PathDoPastePoints;

        public readonly bool PathChangeResetPathOnLevelChange;
        public readonly bool PathNewResetPathOnLevelChange;

        public readonly bool PathChangeUseBlending;
        public readonly bool PathNewUseBlending;

        public readonly bool PathChangePaused;
        public readonly bool PathNewPaused;

        public readonly bool PathChangeUseValueAtStartOfGlobalTimer;
        public readonly bool PathNewUseValueAtStartOfGlobalTimer;

        public readonly bool PathChangeModulo;
        public readonly int PathNewModulo;

        public readonly bool PathChangeIconSize;
        public readonly float PathNewIconSize;

        public readonly bool AngleRangeChangeUseRelativeAngles;
        public readonly bool AngleRangeNewUseRelativeAngles;

        public readonly bool AngleRangeChangeAngleDiff;
        public readonly int AngleRangeNewAngleDiff;

        public readonly bool AngleRangeChangeUseInGameAngles;
        public readonly bool AngleRangeNewUseInGameAngles;

        public MapObjectSettings(
            bool customCylinderChangeRelativeMinY = false,
            float customCylinderNewRelativeMinY = 0,

            bool customCylinderChangeRelativeMaxY = false,
            float customCylinderNewRelativeMaxY = 0,

            bool triangleChangeWithinDist = false,
            float? triangleNewWithinDist = null,

            bool triangleChangeWithinCenter = false,
            float? triangleNewWithinCenter = null,

            bool triangleChangeUseCrossSection = false,
            bool triangleNewUseCrossSection = false,

            bool horizontalTriangleChangeMinHeight = false,
            float? horizontalTriangleNewMinHeight = null,

            bool horizontalTriangleChangeMaxHeight = false,
            float? horizontalTriangleNewMaxHeight = null,

            bool floorChangeExcludeDeathBarriers = false,
            bool floorNewExcludeDeathBarriers = false,

            bool floorChangeEnableQuarterFrameLandings = false,
            bool floorNewEnableQuarterFrameLandings = false,

            bool wallChangeShowArrows = false,
            bool wallNewShowArrows = false,

            bool wallChangeRelativeHeight = false,
            float? wallNewRelativeHeight = null,

            bool wallChangeAbsoluteHeight = false,
            float? wallNewAbsoluteHeight = null,

            bool arrowChangeUseRecommendedLength = false,
            bool arrowNewUseRecommendedLength = false,

            bool arrowChangeUseTruncatedAngle = false,
            bool arrowNewUseTruncatedAngle = false,

            bool arrowChangeHeadSideLength = false,
            float arrowNewHeadSideLength = 0,

            bool arrowChangeAngleOffset = false,
            float arrowNewAngleOffset = 0,

            bool sectorChangeAngleRadius = false,
            float sectorNewAngleRadius = 0,

            bool pathDoReset = false,

            bool pathDoCopyPoints = false,

            bool pathDoPastePoints = false,

            bool pathChangeResetPathOnLevelChange = false,
            bool pathNewResetPathOnLevelChange = false,

            bool pathChangeUseBlending = false,
            bool pathNewUseBlending = false,

            bool pathChangePaused = false,
            bool pathNewPaused = false,

            bool pathChangeUseValueAtStartOfGlobalTimer = false,
            bool pathNewUseValueAtStartOfGlobalTimer = false,

            bool pathChangeModulo = false,
            int pathNewModulo = 0,

            bool pathChangeIconSize = false,
            float pathNewIconSize = 0,

            bool angleRangeChangeUseRelativeAngles = false,
            bool angleRangeNewUseRelativeAngles = false,

            bool angleRangeChangeAngleDiff = false,
            int angleRangeNewAngleDiff = 0,

            bool angleRangeChangeUseInGameAngles = false,
            bool angleRangeNewUseInGameAngles = false)
        {
            CustomCylinderChangeRelativeMinY = customCylinderChangeRelativeMinY;
            CustomCylinderNewRelativeMinY = customCylinderNewRelativeMinY;

            CustomCylinderChangeRelativeMaxY = customCylinderChangeRelativeMaxY;
            CustomCylinderNewRelativeMaxY = customCylinderNewRelativeMaxY;

            TriangleChangeWithinDist = triangleChangeWithinDist;
            TriangleNewWithinDist = triangleNewWithinDist;

            TriangleChangeWithinCenter = triangleChangeWithinCenter;
            TriangleNewWithinCenter = triangleNewWithinCenter;

            TriangleChangeUseCrossSection = triangleChangeUseCrossSection;
            TriangleNewUseCrossSection = triangleNewUseCrossSection;

            HorizontalTriangleChangeMinHeight = horizontalTriangleChangeMinHeight;
            HorizontalTriangleNewMinHeight = horizontalTriangleNewMinHeight;

            HorizontalTriangleChangeMaxHeight = horizontalTriangleChangeMaxHeight;
            HorizontalTriangleNewMaxHeight = horizontalTriangleNewMaxHeight;

            FloorChangeExcludeDeathBarriers = floorChangeExcludeDeathBarriers;
            FloorNewExcludeDeathBarriers = floorNewExcludeDeathBarriers;

            FloorChangeEnableQuarterFrameLandings = floorChangeEnableQuarterFrameLandings;
            FloorNewEnableQuarterFrameLandings = floorNewEnableQuarterFrameLandings;

            WallChangeShowArrows = wallChangeShowArrows;
            WallNewShowArrows = wallNewShowArrows;

            WallChangeRelativeHeight = wallChangeRelativeHeight;
            WallNewRelativeHeight = wallNewRelativeHeight;

            WallChangeAbsoluteHeight = wallChangeAbsoluteHeight;
            WallNewAbsoluteHeight = wallNewAbsoluteHeight;

            ArrowChangeUseRecommendedLength = arrowChangeUseRecommendedLength;
            ArrowNewUseRecommendedLength = arrowNewUseRecommendedLength;

            ArrowChangeUseTruncatedAngle = arrowChangeUseTruncatedAngle;
            ArrowNewUseTruncatedAngle = arrowNewUseTruncatedAngle;

            ArrowChangeHeadSideLength = arrowChangeHeadSideLength;
            ArrowNewHeadSideLength = arrowNewHeadSideLength;

            ArrowChangeAngleOffset = arrowChangeAngleOffset;
            ArrowNewAngleOffset = arrowNewAngleOffset;

            SectorChangeAngleRadius = sectorChangeAngleRadius;
            SectorNewAngleRadius = sectorNewAngleRadius;

            PathDoReset = pathDoReset;

            PathDoCopyPoints = pathDoCopyPoints;

            PathDoPastePoints = pathDoPastePoints;

            PathChangeResetPathOnLevelChange = pathChangeResetPathOnLevelChange;
            PathNewResetPathOnLevelChange = pathNewResetPathOnLevelChange;

            PathChangeUseBlending = pathChangeUseBlending;
            PathNewUseBlending = pathNewUseBlending;

            PathChangePaused = pathChangePaused;
            PathNewPaused = pathNewPaused;

            PathChangeUseValueAtStartOfGlobalTimer = pathChangeUseValueAtStartOfGlobalTimer;
            PathNewUseValueAtStartOfGlobalTimer = pathNewUseValueAtStartOfGlobalTimer;

            PathChangeModulo = pathChangeModulo;
            PathNewModulo = pathNewModulo;

            PathChangeIconSize = pathChangeIconSize;
            PathNewIconSize = pathNewIconSize;

            AngleRangeChangeUseRelativeAngles = angleRangeChangeUseRelativeAngles;
            AngleRangeNewUseRelativeAngles = angleRangeNewUseRelativeAngles;

            AngleRangeChangeAngleDiff = angleRangeChangeAngleDiff;
            AngleRangeNewAngleDiff = angleRangeNewAngleDiff;

            AngleRangeChangeUseInGameAngles = angleRangeChangeUseInGameAngles;
            AngleRangeNewUseInGameAngles = angleRangeNewUseInGameAngles;
        }
    }
}
