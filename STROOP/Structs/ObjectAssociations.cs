using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Utilities;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using STROOP.Controls;

namespace STROOP.Structs
{
    public class ObjectAssociations
    {
        HashSet<ObjectBehaviorAssociation> _objAssoc = new HashSet<ObjectBehaviorAssociation>();
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
        public Image HomeImage;
        public Image IntendedNextPositionImage;
        public Image MarioMapImage;
        public Image BlueMarioMapImage;
        public Image GreenMarioMapImage;
        public Image OrangeMarioMapImage;
        public Image PurpleMarioMapImage;
        public Image CameraFocusMapImage;
        public Image TriangleFloorImage;
        public Image TriangleWallImage;
        public Image TriangleCeilingImage;
        public Image TriangleOtherImage;
        public Image HitboxHackTrisImage;
        public Image CellGridlinesImage;
        public Image CurrentCellImage;
        public Image UnitGridlinesImage;
        public Image CurrentUnitImage;
        public Image NextPositionsImage;
        public Image ArrowImage;
        public Image IwerlipsesImage;
        public Image CylinderImage;
        public Image SphereImage;
        public Image PathImage;
        public Image CustomPointsImage;
        public Image CustomGridlinesImage;

        public Image AggregatedPathImage;
        public Image AngleRangeImage;
        public Image BranchPathImage;
        public Image CoffinBoxImage;
        public Image CompassImage;
        public Image CoordinateLabelsImage;
        public Image FacingDividerImage;
        public Image HomeLineImage;
        public Image LedgeGrabCheckerImage;
        public Image LineSegmentImage;
        public Image SectorImage;
        public Image WatersImage;

        public Color MarioColor;
        public Color HudColor;
        public Color DebugColor;
        public Color MiscColor;
        public Color CameraColor;
        public uint MarioBehavior;
        public uint SegmentTable { get => RomVersionConfig.SwitchMap(SegmentTableUS, SegmentTableJP, SegmentTableSH, SegmentTableEU); }
        public uint SegmentTableUS = 0x8033B400;
        public uint SegmentTableJP = 0x8033A090;
        public uint SegmentTableSH = 0x8031DC58;
        public uint SegmentTableEU = 0x803096C8;
        public uint BehaviorBankStart;

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
                _defaultImage?.Dispose();
                _transparentDefaultImage?.Dispose();
                _defaultImage = value;
                _transparentDefaultImage = value.GetOpaqueImage(0.5f);
            }
        }

        public bool AddAssociation(ObjectBehaviorAssociation objAsooc)
        {
            return _objAssoc.Add(objAsooc);
        }

        public bool AddEmptyAssociation()
        {
            return AddAssociation(
                new ObjectBehaviorAssociation()
                {
                    Name = "Uninitialized Object",
                    Criteria = new BehaviorCriteria()
                    {
                        BehaviorAddress = 0x0000,
                    },
                    RotatesOnMap = false,
                    Image = new LazyImage(EmptyImage),
                    MapImage = new LazyImage(EmptyImage),
                });
        }

        public void AddSpawnHack(SpawnHack hack)
        {
            _spawnHacks.Add(hack);
        }

        private Dictionary<BehaviorCriteria, ObjectBehaviorAssociation> _cachedObjAssoc = new Dictionary<BehaviorCriteria, ObjectBehaviorAssociation>();
        public ObjectBehaviorAssociation FindObjectAssociation(BehaviorCriteria behaviorCriteria)
        {
            if (_cachedObjAssoc.ContainsKey(behaviorCriteria))
            {
                return _cachedObjAssoc[behaviorCriteria];
            }

            var possibleAssoc = _objAssoc.Where(objAssoc => objAssoc.MeetsCriteria(behaviorCriteria));

            if (possibleAssoc.Count() > 1 && possibleAssoc.Any(objAssoc => objAssoc.Criteria.BehaviorOnly()))
                possibleAssoc = possibleAssoc.Where(objAssoc => !objAssoc.Criteria.BehaviorOnly());

            var behaviorAssoc = possibleAssoc.FirstOrDefault();

            _cachedObjAssoc[behaviorCriteria] = behaviorAssoc;

            return behaviorAssoc;
        }

        public Image GetObjectImage(BehaviorCriteria behaviorCriteria, bool transparent = false)
        {
            if (behaviorCriteria.BehaviorAddress == 0)
                return EmptyImage;

            var assoc = FindObjectAssociation(behaviorCriteria);
            if (assoc == null)
                return transparent ? _transparentDefaultImage : _defaultImage;

            return transparent ? assoc.TransparentImage.Image : assoc.Image.Image;
        }

        public Image GetObjectImage(string objName)
        {
            ObjectBehaviorAssociation assoc = GetObjectAssociation(objName);
            if (assoc == null) return EmptyImage;
            return assoc.Image.Image;
        }

        public ObjectBehaviorAssociation GetObjectAssociation(string objName)
        {
            return _objAssoc.FirstOrDefault(a => a.Name.ToLower() == objName.ToLower());
        }

        public Image GetObjectMapImage(BehaviorCriteria behaviorCriteria)
        {
            if (behaviorCriteria.BehaviorAddress == 0)
                return EmptyImage;

            var assoc = FindObjectAssociation(behaviorCriteria);
            if (assoc == null)
                return _defaultImage;

            return assoc.MapImage.Image;
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

        public List<WatchVariableControl> GetWatchVarControls(BehaviorCriteria behaviorCriteria)
        {
            var assoc = FindObjectAssociation(behaviorCriteria);

            if (assoc == null)
                return new List<WatchVariableControl>();

            else return assoc.WatchVariableControls;
        }

        public uint AlignJPBehavior(uint segmented)
        {
            if (segmented >= 0x13002ea0)
                return segmented + 32;
            if (segmented >= 0x13002c6c)
                return segmented + 36;
            if (segmented >= 0x13002998)
                return segmented + 24;
            return segmented;
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
                obj.Image?.Image?.Dispose();
                obj.TransparentImage?.Image?.Dispose();
                obj.MapImage?.Image?.Dispose();
            }

            _transparentDefaultImage?.Dispose();
            _defaultImage?.Dispose();
            EmptyImage?.Dispose();
            MarioImage?.Dispose();
            MarioMapImage?.Dispose();
            BlueMarioMapImage?.Dispose();
            GreenMarioMapImage?.Dispose();
            OrangeMarioMapImage?.Dispose();
            PurpleMarioMapImage?.Dispose();
            CameraFocusMapImage?.Dispose();
            HolpImage?.Dispose();
            HomeImage?.Dispose();
            HudImage?.Dispose();
            DebugImage?.Dispose();
            MiscImage?.Dispose();
            CameraImage?.Dispose();
            CameraMapImage?.Dispose();
            TriangleFloorImage?.Dispose();
            TriangleWallImage?.Dispose();
            TriangleCeilingImage?.Dispose();
            TriangleOtherImage?.Dispose();
            HitboxHackTrisImage?.Dispose();
            CellGridlinesImage?.Dispose();
            CurrentCellImage?.Dispose();
            UnitGridlinesImage?.Dispose();
            CurrentUnitImage?.Dispose();
            NextPositionsImage?.Dispose();
            ArrowImage?.Dispose();
            IwerlipsesImage?.Dispose();
            CylinderImage?.Dispose();
            SphereImage?.Dispose();
            PathImage?.Dispose();
            CustomPointsImage?.Dispose();
            CustomGridlinesImage?.Dispose();
            AggregatedPathImage?.Dispose();
            AngleRangeImage?.Dispose();
            BranchPathImage?.Dispose();
            CoffinBoxImage?.Dispose();
            CompassImage?.Dispose();
            CoordinateLabelsImage?.Dispose();
            FacingDividerImage?.Dispose();
            HomeLineImage?.Dispose();
            LedgeGrabCheckerImage?.Dispose();
            LineSegmentImage?.Dispose();
            SectorImage?.Dispose();
            WatersImage?.Dispose();
        }
    }
}
