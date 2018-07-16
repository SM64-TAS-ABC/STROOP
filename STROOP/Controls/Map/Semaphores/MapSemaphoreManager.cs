using OpenTK;
using OpenTK.Graphics;
using STROOP.Structs;
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

        public static List<MapSemaphore> Objects = new List<MapSemaphore>();

        static MapSemaphoreManager()
        {
            for (int i = 0; i < ObjectSlotsConfig.MaxSlots; i++)
            {
                Objects.Add(new MapSemaphore());
            }
        }

        public static MapSemaphore FloorTri = new MapSemaphore();
        public static MapSemaphore WallTri = new MapSemaphore();
        public static MapSemaphore CeilingTri = new MapSemaphore();

    }
}
