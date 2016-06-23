﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SM64_Diagnostic.Structs
{
    public class MapAssociations
    {
        Dictionary<Tuple<byte, byte>, List<Map>> _maps = new Dictionary<Tuple<byte, byte>, List<Map>>();

        public Map DefaultMap;

        public string FolderPath;

        public void AddAssociation(Map map)
        {
            var mapKey = new Tuple<byte, byte>(map.Level, map.Area);
            if (!_maps.ContainsKey(mapKey))
                _maps.Add(mapKey, new List<Map>());
            _maps[mapKey].Add(map);
        }

        public List<Map> GetLevelAreaMaps(byte level, byte area)
        {
            var mapKey = new Tuple<byte, byte>(level, area);
            if (!_maps.ContainsKey(mapKey))
                return new List<Map>();

            return _maps[mapKey];
        }

        public Image GetMapImage(Map map)
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
    }
}
