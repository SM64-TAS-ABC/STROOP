using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map.Semaphores
{
    public static class MapSemaphoreManager
    {
        public static MapSemaphore Mario = new MapSemaphore();
        public static MapSemaphore Holp = new MapSemaphore();
        public static MapSemaphore Camera = new MapSemaphore();
    }
}
