using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class ObjectBehaviorAssociation
    {
        public BehaviorCriteria BehaviorCriteria;

        public string Name;
        public bool RotatesOnMap;
        public string ImagePath = "";
        public string MapImagePath = "";
        public Image Image;
        public Image TransparentImage;
        public Image MapImage;
        public Image TransparentMapImage;
        public List<WatchVariable> WatchVariables = new List<WatchVariable>();

        public bool MeetsCriteria(BehaviorCriteria behaviorCriteria)
        {
            return BehaviorCriteria.CongruentTo(behaviorCriteria);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ObjectBehaviorAssociation))
                return false;

            var otherBehavior = (ObjectBehaviorAssociation)obj;

            return otherBehavior.BehaviorCriteria == BehaviorCriteria;
        }

        public override int GetHashCode()
        {
            return BehaviorCriteria.GetHashCode();
        }

        public static bool operator ==(ObjectBehaviorAssociation a, ObjectBehaviorAssociation b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ObjectBehaviorAssociation a, ObjectBehaviorAssociation b)
        {
            return !a.Equals(b);
        }
    }
}
