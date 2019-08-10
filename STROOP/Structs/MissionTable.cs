using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class MissionTable
    {
        public struct MissionReference
        {
            public int CourseIndex;
            public int MissionIndex;
            public int InGameCourseIndex;
            public int InGameMissionIndex;
            public string MissionName;

            public override int GetHashCode()
            {
                return CourseIndex * 7 + MissionIndex;
            }
        }

        Dictionary<(int, int), MissionReference> _table = new Dictionary<(int, int), MissionReference>();
        Dictionary<(int, int), MissionReference> _inGameTable = new Dictionary<(int, int), MissionReference>();

        public MissionTable()
        {
        }

        public void Add(MissionReference missionRef)
        {
            _table.Add((missionRef.CourseIndex, missionRef.MissionIndex), missionRef);
            _inGameTable.Add((missionRef.InGameCourseIndex, missionRef.InGameMissionIndex), missionRef);
        }

        public string GetMissionName(int courseIndex, int missionIndex)
        {
            if (_table.ContainsKey((courseIndex, missionIndex)))
                return _table[(courseIndex, missionIndex)].MissionName;

            return null;
        }

        public string GetInGameMissionName(int inGameCourseIndex, int inGameMissionIndex)
        {
            if (_inGameTable.ContainsKey((inGameCourseIndex, inGameMissionIndex)))
                return _inGameTable[(inGameCourseIndex, inGameMissionIndex)].MissionName;

            return "(unknown mission name)";
        }
    }
}
