using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map2
{
    public abstract class Map2BaseObject : IDisposable
    {
        public bool Draw;
        public int Depth;

        public abstract void DrawOnControl(Map2Graphics graphics);
        public abstract void Load(Map2Graphics graphics);
        public abstract double GetDepthScore();
        public virtual void Dispose()
        {
        }
    }
}
