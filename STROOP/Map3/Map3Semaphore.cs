using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map3
{
    public class Map3Semaphore
    {
        public bool IsUsed = false;

        public void Toggle()
        {
            IsUsed = !IsUsed;
        }
    }
}
