using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map3
{
    public abstract class Map3BaseObject : IDisposable
    {
        public bool Draw;
        public int Depth;

        public abstract void DrawOnControl(Map3Graphics graphics);
        public abstract void Load(Map3Graphics graphics);
        public abstract double GetDepthScore();
        public virtual void Dispose()
        {
        }
    }
}
