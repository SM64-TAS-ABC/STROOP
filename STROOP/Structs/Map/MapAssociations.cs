﻿using System;
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
        Dictionary<string, BackgroundImage> _backgroundImageDictionary = new Dictionary<string, BackgroundImage>();

        public MapLayout DefaultMap;

        public string MapImageFolderPath;
        public string BackgroundImageFolderPath;

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

        public MapLayout GetBestMap(byte level, byte area, ushort loadingPoint, ushort missionLayout, float y)
        {
            List<MapLayout> mapList = GetLevelAreaMaps(level, area, loadingPoint, missionLayout);
            mapList = mapList.FindAll(map => map.Y <= y);
            if (mapList.Count == 0) return Config.MapAssociations.DefaultMap;
            MapLayout bestMap = mapList.First();
            foreach (MapLayout map in mapList)
            {
                if (map.Y > bestMap.Y) bestMap = map;
            }
            return bestMap;
        }

        public List<MapLayout> GetAllMaps()
        {
            List<MapLayout> maps = _maps.Values.SelectMany(list => list).ToList();
            maps.Sort();
            return maps;
        }

        public void AddBackgroundImage(BackgroundImage backgroundImage)
        {
            _backgroundImageDictionary.Add(backgroundImage.Name, backgroundImage);
        }

        public BackgroundImage? GetBackgroundImage(string name)
        {
            if (name == null) return null;
            if (_backgroundImageDictionary.ContainsKey(name))
                return _backgroundImageDictionary[name];
            else
                return null;
        }

        public List<BackgroundImage> GetAllBackgroundImages()
        {
            return _backgroundImageDictionary.Values.ToList();
        }
    }
}
