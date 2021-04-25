namespace STROOP.Map
{
    public class MapObjectSettings
    {
        public readonly bool ChangeCustomCylinderRelativeMinY;
        public readonly float NewCustomCylinderRelativeMinY;

        public readonly bool ChangeCustomCylinderRelativeMaxY;
        public readonly float NewCustomCylinderRelativeMaxY;

        public readonly bool ChangeTriangleWithinDist;
        public readonly float? NewTriangleWithinDist;

        public readonly bool ChangeTriangleWithinCenter;
        public readonly float? NewTriangleWithinCenter;

        public readonly bool ChangeTriangleUseCrossSection;
        public readonly bool NewTriangleUseCrossSection;

        public readonly bool ChangeHorizontalTriangleMinHeight;
        public readonly float? NewHorizontalTriangleMinHeight;

        public readonly bool ChangeHorizontalTriangleMaxHeight;
        public readonly float? NewHorizontalTriangleMaxHeight;

        public readonly bool ChangeFloorExcludeDeathBarriers;
        public readonly bool NewFloorExcludeDeathBarriers;

        public readonly bool ChangeFloorEnableQuarterFrameLandings;
        public readonly bool NewFloorEnableQuarterFrameLandings;

        public readonly bool ChangeWallShowArrows;
        public readonly bool NewWallShowArrows;

        public readonly bool ChangeWallRelativeHeight;
        public readonly float? NewWallRelativeHeight;

        public readonly bool ChangeWallAbsoluteHeight;
        public readonly float? NewWallAbsoluteHeight;

        public readonly bool ChangeArrowUseRecommendedLength;
        public readonly bool NewArrowUseRecommendedLength;

        public readonly bool ChangeArrowUseTruncatedAngle;
        public readonly bool NewArrowUseTruncatedAngle;

        public readonly bool ChangeArrowHeadSideLength;
        public readonly float NewArrowHeadSideLength;

        public readonly bool ChangeArrowAngleOffset;
        public readonly float NewArrowAngleOffset;

        public readonly bool ChangeSectorAngleRadius;
        public readonly float NewSectorAngleRadius;

        public readonly bool DoPathReset;

        public readonly bool DoPathCopyPoints;

        public readonly bool DoPathPastePoints;

        public readonly bool ChangePathResetPathOnLevelChange;
        public readonly bool NewPathResetPathOnLevelChange;

        public readonly bool ChangePathUseBlending;
        public readonly bool NewPathUseBlending;

        public readonly bool ChangePathPaused;
        public readonly bool NewPathPaused;

        public readonly bool ChangePathUseValueAtStartOfGlobalTimer;
        public readonly bool NewPathUseValueAtStartOfGlobalTimer;

        public readonly bool ChangePathModulo;
        public readonly int NewPathModulo;

        public readonly bool ChangePathIconSize;
        public readonly float NewPathIconSize;

        public readonly bool ChangeAngleRangeUseRelativeAngles;
        public readonly bool NewAngleRangeUseRelativeAngles;

        public readonly bool ChangeAngleRangeAngleDiff;
        public readonly int NewAngleRangeAngleDiff;

        public readonly bool ChangeAngleRangeUseInGameAngles;
        public readonly bool NewAngleRangeUseInGameAngles;

        public readonly bool ChangeAutoUpdate;
        public readonly bool NewAutoUpdate;

        public readonly bool ChangeIwerlipseLockPositions;
        public readonly bool NewIwerlipseLockPositions;

        public readonly bool ChangeShowQuarterSteps;
        public readonly bool NewShowQuarterSteps;

        public MapObjectSettings(
            bool changeCustomCylinderRelativeMinY = false,
            float newCustomCylinderRelativeMinY = 0,

            bool changeCustomCylinderRelativeMaxY = false,
            float newCustomCylinderRelativeMaxY = 0,

            bool changeTriangleWithinDist = false,
            float? newTriangleWithinDist = null,

            bool changeTriangleWithinCenter = false,
            float? newTriangleWithinCenter = null,

            bool changeTriangleUseCrossSection = false,
            bool newTriangleUseCrossSection = false,

            bool changeHorizontalTriangleMinHeight = false,
            float? newHorizontalTriangleMinHeight = null,

            bool changeHorizontalTriangleMaxHeight = false,
            float? newHorizontalTriangleMaxHeight = null,

            bool changeFloorExcludeDeathBarriers = false,
            bool newFloorExcludeDeathBarriers = false,

            bool changeFloorEnableQuarterFrameLandings = false,
            bool newFloorEnableQuarterFrameLandings = false,

            bool changeWallShowArrows = false,
            bool newWallShowArrows = false,

            bool changeWallRelativeHeight = false,
            float? newWallRelativeHeight = null,

            bool changeWallAbsoluteHeight = false,
            float? newWallAbsoluteHeight = null,

            bool changeArrowUseRecommendedLength = false,
            bool newArrowUseRecommendedLength = false,

            bool changeArrowUseTruncatedAngle = false,
            bool newArrowUseTruncatedAngle = false,

            bool changeArrowHeadSideLength = false,
            float newArrowHeadSideLength = 0,

            bool changeArrowAngleOffset = false,
            float newArrowAngleOffset = 0,

            bool changeSectorAngleRadius = false,
            float newSectorAngleRadius = 0,

            bool doPathReset = false,

            bool doPathCopyPoints = false,

            bool doPathPastePoints = false,

            bool changePathResetPathOnLevelChange = false,
            bool newPathResetPathOnLevelChange = false,

            bool changePathUseBlending = false,
            bool newPathUseBlending = false,

            bool changePathPaused = false,
            bool newPathPaused = false,

            bool changePathUseValueAtStartOfGlobalTimer = false,
            bool newPathUseValueAtStartOfGlobalTimer = false,

            bool changePathModulo = false,
            int newPathModulo = 0,

            bool changePathIconSize = false,
            float newPathIconSize = 0,

            bool changeAngleRangeUseRelativeAngles = false,
            bool newAngleRangeUseRelativeAngles = false,

            bool changeAngleRangeAngleDiff = false,
            int newAngleRangeAngleDiff = 0,

            bool changeAngleRangeUseInGameAngles = false,
            bool newAngleRangeUseInGameAngles = false,

            bool changeAutoUpdate = false,
            bool newAutoUpdate = false,

            bool changeIwerlipseLockPositions = false,
            bool newIwerlipseLockPositions = false,

            bool changeShowQuarterSteps = false,
            bool newShowQuarterSteps = false)
        {
            ChangeCustomCylinderRelativeMinY = changeCustomCylinderRelativeMinY;
            NewCustomCylinderRelativeMinY = newCustomCylinderRelativeMinY;

            ChangeCustomCylinderRelativeMaxY = changeCustomCylinderRelativeMaxY;
            NewCustomCylinderRelativeMaxY = newCustomCylinderRelativeMaxY;

            ChangeTriangleWithinDist = changeTriangleWithinDist;
            NewTriangleWithinDist = newTriangleWithinDist;

            ChangeTriangleWithinCenter = changeTriangleWithinCenter;
            NewTriangleWithinCenter = newTriangleWithinCenter;

            ChangeTriangleUseCrossSection = changeTriangleUseCrossSection;
            NewTriangleUseCrossSection = newTriangleUseCrossSection;

            ChangeHorizontalTriangleMinHeight = changeHorizontalTriangleMinHeight;
            NewHorizontalTriangleMinHeight = newHorizontalTriangleMinHeight;

            ChangeHorizontalTriangleMaxHeight = changeHorizontalTriangleMaxHeight;
            NewHorizontalTriangleMaxHeight = newHorizontalTriangleMaxHeight;

            ChangeFloorExcludeDeathBarriers = changeFloorExcludeDeathBarriers;
            NewFloorExcludeDeathBarriers = newFloorExcludeDeathBarriers;

            ChangeFloorEnableQuarterFrameLandings = changeFloorEnableQuarterFrameLandings;
            NewFloorEnableQuarterFrameLandings = newFloorEnableQuarterFrameLandings;

            ChangeWallShowArrows = changeWallShowArrows;
            NewWallShowArrows = newWallShowArrows;

            ChangeWallRelativeHeight = changeWallRelativeHeight;
            NewWallRelativeHeight = newWallRelativeHeight;

            ChangeWallAbsoluteHeight = changeWallAbsoluteHeight;
            NewWallAbsoluteHeight = newWallAbsoluteHeight;

            ChangeArrowUseRecommendedLength = changeArrowUseRecommendedLength;
            NewArrowUseRecommendedLength = newArrowUseRecommendedLength;

            ChangeArrowUseTruncatedAngle = changeArrowUseTruncatedAngle;
            NewArrowUseTruncatedAngle = newArrowUseTruncatedAngle;

            ChangeArrowHeadSideLength = changeArrowHeadSideLength;
            NewArrowHeadSideLength = newArrowHeadSideLength;

            ChangeArrowAngleOffset = changeArrowAngleOffset;
            NewArrowAngleOffset = newArrowAngleOffset;

            ChangeSectorAngleRadius = changeSectorAngleRadius;
            NewSectorAngleRadius = newSectorAngleRadius;

            DoPathReset = doPathReset;

            DoPathCopyPoints = doPathCopyPoints;

            DoPathPastePoints = doPathPastePoints;

            ChangePathResetPathOnLevelChange = changePathResetPathOnLevelChange;
            NewPathResetPathOnLevelChange = newPathResetPathOnLevelChange;

            ChangePathUseBlending = changePathUseBlending;
            NewPathUseBlending = newPathUseBlending;

            ChangePathPaused = changePathPaused;
            NewPathPaused = newPathPaused;

            ChangePathUseValueAtStartOfGlobalTimer = changePathUseValueAtStartOfGlobalTimer;
            NewPathUseValueAtStartOfGlobalTimer = newPathUseValueAtStartOfGlobalTimer;

            ChangePathModulo = changePathModulo;
            NewPathModulo = newPathModulo;

            ChangePathIconSize = changePathIconSize;
            NewPathIconSize = newPathIconSize;

            ChangeAngleRangeUseRelativeAngles = changeAngleRangeUseRelativeAngles;
            NewAngleRangeUseRelativeAngles = newAngleRangeUseRelativeAngles;

            ChangeAngleRangeAngleDiff = changeAngleRangeAngleDiff;
            NewAngleRangeAngleDiff = newAngleRangeAngleDiff;

            ChangeAngleRangeUseInGameAngles = changeAngleRangeUseInGameAngles;
            NewAngleRangeUseInGameAngles = newAngleRangeUseInGameAngles;

            ChangeAutoUpdate = changeAutoUpdate;
            NewAutoUpdate = newAutoUpdate;

            ChangeIwerlipseLockPositions = changeIwerlipseLockPositions;
            NewIwerlipseLockPositions = newIwerlipseLockPositions;

            ChangeShowQuarterSteps = changeShowQuarterSteps;
            NewShowQuarterSteps = newShowQuarterSteps;
        }
    }
}
