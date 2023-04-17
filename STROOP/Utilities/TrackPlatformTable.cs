using STROOP.Forms;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public class TrackPlatformTable
    {
        public Dictionary<int, TrackPlatform> _dictionary;
        public Dictionary<float, List<int>> _reverseDictionary;

        public TrackPlatformTable()
        {
            _dictionary = new Dictionary<int, TrackPlatform>();
            _reverseDictionary = new Dictionary<float, List<int>>();
        }

        public int GetNumComputedFrames()
        {
            return _dictionary.Count;
        }

        public void SetNumComputedFrames(int numFrames)
        {
            _dictionary.Clear();
            _reverseDictionary.Clear();

            TrackPlatform trackPlatform = new TrackPlatform();

            for (int i = 0; i < 21; i++)
            {
                trackPlatform.Update(true);
            }

            for (int i = 0; i < numFrames; i++)
            {
                _dictionary.Add(i, trackPlatform.Clone());

                if (_reverseDictionary.ContainsKey(trackPlatform.oPosX))
                {
                    _reverseDictionary[trackPlatform.oPosX].Add(i);
                }
                else
                {
                    _reverseDictionary[trackPlatform.oPosX] = new List<int>() { i };
                }

                trackPlatform.Update(true);
            }
        }

        public int GetFrame(uint objAddress)
        {
            TrackPlatform trackPlatform = new TrackPlatform(objAddress);
            if (_reverseDictionary.ContainsKey(trackPlatform.oPosX))
            {
                List<int> indexes = _reverseDictionary[trackPlatform.oPosX];
                foreach (int index in indexes)
                {
                    if (trackPlatform.Equals(_dictionary[index]))
                    {
                        return index;
                    }
                }
            }
            return -1;
        }

        public bool SetFrame(int frame, uint objAddress)
        {
            if (_dictionary.ContainsKey(frame))
            {
                _dictionary[frame].ApplyToObject(objAddress);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
