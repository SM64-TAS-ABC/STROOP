using SM64_Diagnostic.ManagerClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Controls.Map
{
    public abstract class MapBaseObject
    {
        public double DepthScore;
        public bool Draw;
        public int Depth;
        public bool Show;

        public abstract void DrawOnControl(MapGraphics graphics);
    }
}
