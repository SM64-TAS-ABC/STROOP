using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Extensions;

namespace SM64_Diagnostic.Structs
{
    public class ObjectAssociations
    {
        Dictionary<uint, ObjectBehaviorAssociation> _objAssoc = new Dictionary<uint, ObjectBehaviorAssociation>();
        
        Image _defaultImage;
        Image _transparentDefaultImage;

        public Image EmptyImage;
        public Image MarioImage;
        public Image HudImage;
        public Image DebugImage;
        public Image MiscImage;
        public Image CameraImage;
        public Image CameraMapImage;
        public Image HolpImage;
        public Image MarioMapImage;
        public Color MarioColor;
        public Color HudColor;
        public Color DebugColor;
        public Color MiscColor;
        public Color CameraColor;
        public uint MarioBehavior;
        public uint RamOffset;

        public Dictionary<uint, ObjectBehaviorAssociation> BehaviorAssociations
        {
            get
            {
                return _objAssoc;
            }
        }

        public Image DefaultImage
        {
            get
            {
                return _defaultImage;
            }
            set
            {
                _defaultImage = value;
                _transparentDefaultImage = value.GetOpaqueImage(0.5f);
            }
        }

        public void AddAssociation(ObjectBehaviorAssociation obj)
        {
            _objAssoc.Add(obj.Behavior, obj);
        }

        public Image GetObjectImage(uint behaviorAddress, bool transparent)
        {
            if (behaviorAddress == 0)
                return EmptyImage;

            if (!_objAssoc.ContainsKey(behaviorAddress))
                return transparent ? _transparentDefaultImage : _defaultImage;

            return transparent ? _objAssoc[behaviorAddress].TransparentImage : _objAssoc[behaviorAddress].Image;
        }

        public Image GetObjectMapImage(uint behaviorAddress, bool transparent)
        {
            if (behaviorAddress == 0)
                return EmptyImage;

            if (!_objAssoc.ContainsKey(behaviorAddress))
                return _defaultImage;

            return transparent ? _objAssoc[behaviorAddress].TransparentMapImage : _objAssoc[behaviorAddress].MapImage;
        }

        public bool GetObjectMapRotates(uint behaviorAddress)
        {
            if (!_objAssoc.ContainsKey(behaviorAddress))
                return false;

            return _objAssoc[behaviorAddress].RotatesOnMap;
        }

        public string GetObjectName(uint behaviorAddress)
        {
            if (behaviorAddress == 0)
                return "Uninitialized Object";

            if (!_objAssoc.ContainsKey(behaviorAddress))
                return "Unknown Object";

            return _objAssoc[behaviorAddress].Name;
        }

        public List<WatchVariable> GetWatchVariables(uint behaviorAddress)
        {
            if (!_objAssoc.ContainsKey(behaviorAddress))
                return new List<WatchVariable>();

            else return _objAssoc[behaviorAddress].WatchVariables;
        }

        ~ObjectAssociations()
        {
            // Unload and dipose of all images
            foreach (ObjectBehaviorAssociation obj in _objAssoc.Values)
            {
                obj.Image?.Dispose();
                obj.TransparentImage?.Dispose();
                obj.MapImage?.Dispose();
                obj.TransparentMapImage?.Dispose();
            }

            _transparentDefaultImage?.Dispose();
            _defaultImage?.Dispose();
            EmptyImage?.Dispose();
            MarioImage?.Dispose();
            MarioMapImage?.Dispose();
            HolpImage?.Dispose();
            HudImage?.Dispose();
            DebugImage?.Dispose();
            MiscImage?.Dispose();
            CameraImage?.Dispose();
            CameraMapImage?.Dispose();
        }
    }
}
