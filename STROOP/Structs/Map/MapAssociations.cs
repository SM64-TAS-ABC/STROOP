using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using STROOP.Structs.Configurations;

namespace STROOP.Structs
{
    public class MapAssociations
    {
        Dictionary<Tuple<byte, byte>, List<MapLayout>> _maps = new Dictionary<Tuple<byte, byte>, List<MapLayout>>();

        public MapLayout DefaultMap;

        public string FolderPath;

        public void AddAssociation(MapLayout map)
        {
            var mapKey = new Tuple<byte, byte>(map.Level, map.Area);
            if (!_maps.ContainsKey(mapKey))
                _maps.Add(mapKey, new List<MapLayout>());
            _maps[mapKey].Add(map);
        }

        public List<MapLayout> GetLevelAreaMaps(byte level, byte area)
        {
            var mapKey = new Tuple<byte, byte>(level, area);
            if (!_maps.ContainsKey(mapKey))
                return new List<MapLayout>();

            return _maps[mapKey];
        }

        public List<MapLayout> GetLevelAreaMaps(byte level, byte area, ushort loadingPoint, ushort missionLayout)
        {
            List<MapLayout> mapList = GetLevelAreaMaps(level, area);
            mapList = mapList.FindAll(map => map.LoadingPoint == null || map.LoadingPoint == loadingPoint);
            mapList = mapList.FindAll(map => map.MissionLayout == null || map.MissionLayout == missionLayout);
            return mapList;
        }

        public List<MapLayout> GetAllMaps()
        {
            List<MapLayout> maps = _maps.Values.SelectMany(list => list).ToList();
            maps.Sort();
            return maps;
        }

        public Bitmap GetMapImage(MapLayout map)
        {
            var path = Path.Combine(FolderPath, map.ImagePath);
            Bitmap image;
            using (Bitmap preLoad = Bitmap.FromFile(path) as Bitmap)
            {
                int maxSize = 1080;
                int largest = Math.Max(preLoad.Width, preLoad.Height);
                float scale = 1;
                if (largest > maxSize)
                    scale = largest / maxSize;
                
                image = new Bitmap(preLoad, new Size((int) (preLoad.Width / scale), (int) (preLoad.Height / scale)));
            }
            return image;
        }

        public Bitmap GetMapBackgroundImage(MapLayout map)
        {
            if (map.BackgroundPath == null)
                return null;

            var path = Path.Combine(FolderPath, map.BackgroundPath);
            Bitmap image;
            using (Bitmap preLoad = Image.FromFile(path) as Bitmap)
            {
                int maxSize = 1080;
                int largest = Math.Max(preLoad.Width, preLoad.Height);
                float scale = 1;
                if (largest > maxSize)
                    scale = largest / maxSize;

                image = new Bitmap(preLoad, new Size((int)(preLoad.Width / scale), (int)(preLoad.Height / scale)));
            }
            return image;
        }
    }
}
