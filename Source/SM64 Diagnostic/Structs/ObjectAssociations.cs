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
        Dictionary<uint, List<ObjectBehaviorAssociation>> _objAssoc = new Dictionary<uint, List<ObjectBehaviorAssociation>>();
        
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

        public Dictionary<uint, List<ObjectBehaviorAssociation>> BehaviorAssociations
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
            if (!_objAssoc.Keys.Contains(obj.Behavior))
                _objAssoc.Add(obj.Behavior, new List<ObjectBehaviorAssociation>() { obj });
            else
                _objAssoc[obj.Behavior].Add(obj);
        }

        public ObjectBehaviorAssociation FindObjectAssociation(uint behaviorAddress, uint gfxId)
        {
            if (!_objAssoc.ContainsKey(behaviorAddress))
                return null;

            if (_objAssoc[behaviorAddress].Exists(obj => obj.GfxId == gfxId))
                return _objAssoc[behaviorAddress].Find(obj => obj.GfxId == gfxId);

            if (_objAssoc[behaviorAddress].Exists(obj => obj.GfxId == null))
                return _objAssoc[behaviorAddress].Find(obj => obj.GfxId == null);

            return null;
        }

        public Image GetObjectImage(uint behaviorAddress, uint gfxId, bool transparent)
        {
            if (behaviorAddress == 0)
                return EmptyImage;

            var assoc = FindObjectAssociation(behaviorAddress, gfxId);
            if (assoc == null)
                return transparent ? _transparentDefaultImage : _defaultImage;

            return transparent ? assoc.TransparentImage : assoc.Image;
        }

        public Image GetObjectMapImage(uint behaviorAddress, uint gfxId, bool transparent)
        {
            if (behaviorAddress == 0)
                return EmptyImage;

            var assoc = FindObjectAssociation(behaviorAddress, gfxId);

            if (assoc == null)
                return _defaultImage;

            return transparent ? assoc.TransparentMapImage : assoc.MapImage;
        }

        public bool GetObjectMapRotates(uint behaviorAddress, uint gfxId)
        {
            var assoc = FindObjectAssociation(behaviorAddress, gfxId);

            if (assoc == null)
                return false;

            return assoc.RotatesOnMap;
        }

        public string GetObjectName(uint behaviorAddress, uint gfxId)
        {
            var assoc = FindObjectAssociation(behaviorAddress, gfxId);

            if (behaviorAddress == 0)
                return "Uninitialized Object";

            if (assoc == null)
                return "Unknown Object";

            return assoc.Name;
        }

        public List<WatchVariable> GetWatchVariables(uint behaviorAddress, uint gfxId)
        {
            var assoc = FindObjectAssociation(behaviorAddress, gfxId);

            if (assoc == null)
                return new List<WatchVariable>();

            else return assoc.WatchVariables;
        }

        ~ObjectAssociations()
        {
            // Unload and dispose of all images
            foreach (var objList in _objAssoc.Values)
            {
                foreach (var obj in objList)
                {
                    obj.Image?.Dispose();
                    obj.TransparentImage?.Dispose();
                    obj.MapImage?.Dispose();
                    obj.TransparentMapImage?.Dispose();
                }
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
