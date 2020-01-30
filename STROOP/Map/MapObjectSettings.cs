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

        public MapObjectSettings(
            bool customCylinderChangeRelativeMinY = false,
            float customCylinderNewRelativeMinY = 0,

            bool customCylinderChangeRelativeMaxY = false,
            float customCylinderNewRelativeMaxY = 0)
        {
            CustomCylinderChangeRelativeMinY = customCylinderChangeRelativeMinY;
            CustomCylinderNewRelativeMinY = customCylinderNewRelativeMinY;

            CustomCylinderChangeRelativeMaxY = customCylinderChangeRelativeMaxY;
            CustomCylinderNewRelativeMaxY = customCylinderNewRelativeMaxY;
        }
    }
}
