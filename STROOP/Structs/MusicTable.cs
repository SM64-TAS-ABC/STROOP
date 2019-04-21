using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class MusicTable
    {
        private Dictionary<int, MusicEntry> _musicDictionary;

        public MusicTable(List<MusicEntry> musicEntries)
        {
            _musicDictionary = new Dictionary<int, MusicEntry>();
            foreach (MusicEntry musicEntry in musicEntries)
            {
                _musicDictionary.Add(musicEntry.Index, musicEntry);
            }
        }

        public List<MusicEntry> GetMusicEntryList()
        {
            return _musicDictionary.Values.ToList();
        }
    }
}
