using OpenTK;
using OpenTK.Graphics;
using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map3.Map.Semaphores
{
    public static class Map4SemaphoreManager
    {
        public static Map4Semaphore Mario = new Map4Semaphore();
        public static Map4Semaphore Holp = new Map4Semaphore();
        public static Map4Semaphore Camera = new Map4Semaphore();

        public static List<Map4Semaphore> Objects = new List<Map4Semaphore>();

        static Map4SemaphoreManager()
        {
            for (int i = 0; i < ObjectSlotsConfig.MaxSlots; i++)
            {
                Objects.Add(new Map4Semaphore());
            }
        }

        public static Map4Semaphore FloorTri = new Map4Semaphore();
        public static Map4Semaphore WallTri = new Map4Semaphore();
        public static Map4Semaphore CeilingTri = new Map4Semaphore();

    }
}
