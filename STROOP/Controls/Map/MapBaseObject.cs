using STROOP.ManagerClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map
{
    public abstract class MapBaseObject : IDisposable
    {
        public bool Draw;
        public int Depth;

        public abstract void DrawOnControl(MapGraphics graphics);
        public abstract void Load(MapGraphics graphics);
        public abstract double GetDepthScore();
        public virtual void Dispose()
        {
        }
    }
}
