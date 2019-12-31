using OpenTK;
using OpenTK.Graphics;
using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map
{
    public static class MapSemaphoreManager
    {
        public static MapSemaphore Mario = new MapSemaphore();
        public static MapSemaphore Holp = new MapSemaphore();
        public static MapSemaphore Camera = new MapSemaphore();
        public static MapSemaphore FloorTri = new MapSemaphore();
        public static MapSemaphore CeilingTri = new MapSemaphore();
        public static MapSemaphore CellGridlines = new MapSemaphore();
        public static MapSemaphore CurrentCell = new MapSemaphore();
        public static MapSemaphore UnitGridlines = new MapSemaphore();
        public static MapSemaphore CurrentUnit = new MapSemaphore();
        public static MapSemaphore NextPositions = new MapSemaphore();
        public static MapSemaphore Self = new MapSemaphore();
        public static MapSemaphore Point = new MapSemaphore();

        public static List<MapSemaphore> Objects = new List<MapSemaphore>();

        static MapSemaphoreManager()
        {
            for (int i = 0; i < ObjectSlotsConfig.MaxSlots; i++)
            {
                Objects.Add(new MapSemaphore());
            }
        }

    }
}
