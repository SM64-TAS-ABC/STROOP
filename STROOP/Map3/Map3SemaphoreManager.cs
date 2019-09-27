using OpenTK;
using OpenTK.Graphics;
using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map3
{
    public static class Map3SemaphoreManager
    {
        public static Map3Semaphore Mario = new Map3Semaphore();
        public static Map3Semaphore Holp = new Map3Semaphore();
        public static Map3Semaphore Camera = new Map3Semaphore();
        public static Map3Semaphore FloorTri = new Map3Semaphore();
        public static Map3Semaphore CeilingTri = new Map3Semaphore();
        public static Map3Semaphore CellGridlines = new Map3Semaphore();
        public static Map3Semaphore CurrentCell = new Map3Semaphore();
        public static Map3Semaphore UnitGridlines = new Map3Semaphore();
        public static Map3Semaphore CurrentUnit = new Map3Semaphore();
        public static Map3Semaphore NextPositions = new Map3Semaphore();
        public static Map3Semaphore Self = new Map3Semaphore();
        public static Map3Semaphore Point = new Map3Semaphore();

        public static List<Map3Semaphore> Objects = new List<Map3Semaphore>();

        static Map3SemaphoreManager()
        {
            for (int i = 0; i < ObjectSlotsConfig.MaxSlots; i++)
            {
                Objects.Add(new Map3Semaphore());
            }
        }

    }
}
