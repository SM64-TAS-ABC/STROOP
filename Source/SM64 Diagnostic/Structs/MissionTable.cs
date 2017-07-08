using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class MissionTable
    {
        public struct MissionReference
        {
            public int CourseIndex;
            public int MissionIndex;
            public string MissionName;

            public override int GetHashCode()
            {
                return CourseIndex * 7 + MissionIndex;
            }
        }

        Dictionary<(int, int), MissionReference> _table = new Dictionary<(int, int), MissionReference>();

        public MissionTable()
        {
        }

        public void Add(MissionReference missionRef)
        {
            _table.Add((missionRef.CourseIndex, missionRef.MissionIndex), missionRef);
        }

        public string GetMissionName(int courseIndex, int missionIndex)
        {
            if (_table.ContainsKey((courseIndex, missionIndex)))
                return _table[(courseIndex, missionIndex)].MissionName;

            return null;
        }
    }
}
