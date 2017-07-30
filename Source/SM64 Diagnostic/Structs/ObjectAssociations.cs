using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Structs
{
    public class ObjectAssociations
    {
        HashSet<ObjectBehaviorAssociation> _objAssoc = new HashSet<ObjectBehaviorAssociation>()
        {
            new ObjectBehaviorAssociation()
            {
                Name = "Uninitialized Object",
                BehaviorCriteria = new BehaviorCriteria()
                {
                    BehaviorAddress = 0x0000,
                },
                RotatesOnMap = false
            }
        };
        List<SpawnHack> _spawnHacks = new List<SpawnHack>();

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

        Dictionary<Image, Image> _cachedBufferedObjectImages = new Dictionary<Image, Image>();
        object _cachedBufferedObjectImageLocker = new object();

        public HashSet<ObjectBehaviorAssociation> BehaviorAssociations
        {
            get
            {
                return _objAssoc;
            }
        }

        public List<SpawnHack> SpawnHacks
        {
            get
            {
                return _spawnHacks;
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

        public bool AddAssociation(ObjectBehaviorAssociation objAsooc)
        {
            return _objAssoc.Add(objAsooc);
        }

        public void AddSpawnHack(SpawnHack hack)
        {
            _spawnHacks.Add(hack);
        }

        public ObjectBehaviorAssociation FindObjectAssociation(BehaviorCriteria behaviorCriteria)
        {
            var possibleAssoc = _objAssoc.Where(objAssoc => objAssoc.MeetsCriteria(behaviorCriteria));

            if (possibleAssoc.Count() > 1 && possibleAssoc.Any(objAssoc => objAssoc.BehaviorCriteria.BehaviorOnly()))
                possibleAssoc = possibleAssoc.Where(objAssoc => !objAssoc.BehaviorCriteria.BehaviorOnly());

            return possibleAssoc.FirstOrDefault();
        }

        public Image GetObjectImage(BehaviorCriteria behaviorCriteria, bool transparent)
        {
            if (behaviorCriteria.BehaviorAddress == 0)
                return EmptyImage;

            var assoc = FindObjectAssociation(behaviorCriteria);
            if (assoc == null)
                return transparent ? _transparentDefaultImage : _defaultImage;

            return transparent ? assoc.TransparentImage : assoc.Image;
        }

        public Image GetObjectMapImage(BehaviorCriteria behaviorCriteria, bool transparent)
        {
            if (behaviorCriteria.BehaviorAddress == 0)
                return EmptyImage;

            var assoc = FindObjectAssociation(behaviorCriteria);
            if (assoc == null)
                return _defaultImage;

            return transparent ? assoc.TransparentMapImage : assoc.MapImage;
        }

        public bool GetObjectMapRotates(BehaviorCriteria behaviorCriteria)
        {
            var assoc = FindObjectAssociation(behaviorCriteria);

            if (assoc == null)
                return false;

            return assoc.RotatesOnMap;
        }

        public string GetObjectName(BehaviorCriteria behaviorCriteria)
        {
            var assoc = FindObjectAssociation(behaviorCriteria);

            if (assoc == null)
                return "Unknown Object";

            return assoc.Name;
        }

        public Image GetCachedBufferedObjectImage(Image objectImage, Size size)
        {
            lock (_cachedBufferedObjectImageLocker)
            {
                if (!_cachedBufferedObjectImages.ContainsKey(objectImage))
                    return null;

                // Make sure cached size matches
                var _bufferedImage = _cachedBufferedObjectImages[objectImage];
                if (size != _bufferedImage.Size)
                    return null;

                return _bufferedImage;
            }
        }

        public void CreateCachedBufferedObjectImage(Image objectImage, Image bufferedObjectImage)
        {
            // Dispose of previous image
            lock (_cachedBufferedObjectImageLocker)
            {
                if (_cachedBufferedObjectImages.ContainsKey(objectImage))
                    _cachedBufferedObjectImages[objectImage]?.Dispose();

                _cachedBufferedObjectImages[objectImage] = bufferedObjectImage;
            }
        }

        public List<WatchVariable> GetWatchVariables(BehaviorCriteria behaviorCriteria)
        {
            var assoc = FindObjectAssociation(behaviorCriteria);

            if (assoc == null)
                return new List<WatchVariable>();

            else return assoc.WatchVariables;
        }

        ~ObjectAssociations()
        {
            lock (_cachedBufferedObjectImageLocker)
            {
                foreach (var img in _cachedBufferedObjectImages)
                {
                    img.Value.Dispose();
                }
            }

            // Unload and dispose of all images
            foreach (var obj in _objAssoc)
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
