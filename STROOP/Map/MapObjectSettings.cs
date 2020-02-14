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

        public readonly bool WallChangeRelativeHeight;
        public readonly float? WallNewRelativeHeight;

        public readonly bool ArrowChangeHeadSideLength;
        public readonly float ArrowNewHeadSideLength;

        public readonly bool PathDoReset;

        public readonly bool PathChangeResetPathOnLevelChange;
        public readonly bool PathNewResetPathOnLevelChange;

        public readonly bool PathChangeUseBlending;
        public readonly bool PathNewUseBlending;

        public readonly bool PathChangePaused;
        public readonly bool PathNewPaused;

        public readonly bool PathChangeModulo;
        public readonly int PathNewModulo;

        public MapObjectSettings(
            bool customCylinderChangeRelativeMinY = false,
            float customCylinderNewRelativeMinY = 0,

            bool customCylinderChangeRelativeMaxY = false,
            float customCylinderNewRelativeMaxY = 0,

            bool wallChangeRelativeHeight = false,
            float? wallNewRelativeHeight = null,

            bool arrowChangeHeadSideLength = false,
            float arrowNewHeadSideLength = 0,

            bool pathDoReset = false,

            bool pathChangeResetPathOnLevelChange = false,
            bool pathNewResetPathOnLevelChange = false,

            bool pathChangeUseBlending = false,
            bool pathNewUseBlending = false,

            bool pathChangePaused = false,
            bool pathNewPaused = false,

            bool pathChangeModulo = false,
            int pathNewModulo = 0)
        {
            CustomCylinderChangeRelativeMinY = customCylinderChangeRelativeMinY;
            CustomCylinderNewRelativeMinY = customCylinderNewRelativeMinY;

            CustomCylinderChangeRelativeMaxY = customCylinderChangeRelativeMaxY;
            CustomCylinderNewRelativeMaxY = customCylinderNewRelativeMaxY;

            WallChangeRelativeHeight = wallChangeRelativeHeight;
            WallNewRelativeHeight = wallNewRelativeHeight;

            ArrowChangeHeadSideLength = arrowChangeHeadSideLength;
            ArrowNewHeadSideLength = arrowNewHeadSideLength;

            PathDoReset = pathDoReset;

            PathChangeResetPathOnLevelChange = pathChangeResetPathOnLevelChange;
            PathNewResetPathOnLevelChange = pathNewResetPathOnLevelChange;

            PathChangeUseBlending = pathChangeUseBlending;
            PathNewUseBlending = pathNewUseBlending;

            PathChangePaused = pathChangePaused;
            PathNewPaused = pathNewPaused;

            PathChangeModulo = pathChangeModulo;
            PathNewModulo = pathNewModulo;
        }
    }
}
