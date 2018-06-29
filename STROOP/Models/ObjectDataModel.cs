using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Models
{
    public class ObjectDataModel : IUpdatableDataModel
    {
        const ushort ActiveStatus = 0x0101;
        public uint Address { get; set; }

        #region Behavior
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (Config.Stream.SetValue(value ? ActiveStatus : (ushort) 0, Address + ObjectConfig.ActiveOffset))
                    _isActive = value;
            }
        }
        public uint AbsoluteBehavior { get; private set; }
        public uint SegmentedBehavior { get; private set; }

        private UInt32 _gfxId;
        public UInt32 GraphicsID
        {
            get => _gfxId;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.BehaviorGfxOffset))
                    _gfxId = value;
            }
        }

        private UInt32 _subType;
        public UInt32 SubType
        {
            get => _subType;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.BehaviorSubtypeOffset))
                    _subType = value;
            }
        }

        private UInt32 _appearance;
        public UInt32 Appearance
        {
            get => _appearance;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.BehaviorAppearanceOffset))
                    _appearance = value;
            }
        }

        public BehaviorCriteria BehaviorCriteria { get; private set; }
        public ObjectBehaviorAssociation BehaviorAssociation { get; private set; }

        private uint BehaviorScriptStart
        { 
            get => Config.Stream.GetUInt32(Address + ObjectConfig.BehaviorScriptOffset);
            set => Config.Stream.SetValue(value, Address + ObjectConfig.BehaviorScriptOffset);
        }
        #endregion
        #region Processing/Vacancy
        public byte? CurrentProcessGroup { get; set; }
        public byte? BehaviorProcessGroup
        {
            get
            {
                if (BehaviorScriptStart == 00000000)
                    return null;
                uint firstScriptAction = Config.Stream.GetUInt32(BehaviorScriptStart);
                if ((firstScriptAction & 0xFF000000U) != 0x00000000U)
                    return null;
                return (byte)((firstScriptAction & 0x00FF0000U) >> 16);
            }
        }
        public int ProcessIndex { get; set; }
        public int? VacantSlotIndex { get; set; }
        public bool IsVacant => VacantSlotIndex.HasValue;
        #endregion
        #region Object Graph
        public uint Parent
        {
            get => Config.Stream.GetUInt32(Address + ObjectConfig.ParentOffset);
            set => Config.Stream.SetValue(value, Address + ObjectConfig.ParentOffset);
        }
        #endregion
        #region Position
        private float _x;
        public float X
        {
            get => _x;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.XOffset))
                    _x = value;
            }
        }

        private float _y;
        public float Y
        {
            get => _y;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.YOffset))
                    _y = value;
            }
        }

        private float _z;
        public float Z
        {
            get => _z;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.ZOffset))
                    _z = value;
            }
        }
        private float _homeX;
        public float HomeX
        {
            get => _homeX;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.HomeXOffset))
                    _homeX = value;
            }
        }

        private float _homeY;
        public float HomeY
        {
            get => _homeY;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.HomeYOffset))
                    _homeY = value;
            }
        }

        private float _homeZ;
        public float HomeZ
        {
            get => _homeZ;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.HomeZOffset))
                    _homeZ = value;
            }
        }

        public double DistanceToMarioCalculated { get; private set; }
        #endregion
        #region Rotation
        private ushort _facingYaw;
        public ushort FacingYaw
        {
            get => _facingYaw;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.YawFacingOffset))
                    _facingYaw = value;
            }
        }
        private ushort _facingPitch;
        public ushort FacingPitch
        {
            get => _facingPitch;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.PitchFacingOffset))
                    _facingPitch = value;
            }
        }
        private ushort _facingRoll;
        public ushort FacingRoll
        {
            get => _facingRoll;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.RollFacingOffset))
                    _facingRoll = value;
            }
        }
        #endregion
        #region Statuses
        private uint _releaseStatus;
        public uint ReleaseStatus
        {
            get => Config.Stream.GetUInt32(Address + ObjectConfig.ReleaseStatusOffset);
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.ReleaseStatusOffset))
                    _releaseStatus = value;
            }
        }

        private uint _interactionStatus;
        public uint InteractionStatus
        {
            get => Config.Stream.GetUInt32(Address + ObjectConfig.InteractionStatusOffset);
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.InteractionStatusOffset))
                    _interactionStatus = value;
            }
        }
        #endregion

        private uint _processedNextLink;
        public uint ProcessedNextLink
        {
            get => _processedNextLink;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.ProcessedNextLinkOffset))
                    _processedNextLink = value;
            }
        }

        private uint _processedPrevLink;
        public uint ProcessedPrevLink
        {
            get => _processedPrevLink;
            set
            {
                if (Config.Stream.SetValue(value, Address + ObjectConfig.ProcessedPreviousLinkOffset))
                    _processedPrevLink = value;
            }
        }

        public ObjectDataModel(uint address, bool update = true)
        {
            Address = address;
            if (update)
            {
                Update();
                Update2();
            }
        }

        public void Update()
        {
            _isActive = Config.Stream.GetUInt16(Address + ObjectConfig.ActiveOffset) != 0x0000;
            AbsoluteBehavior = Config.Stream.GetUInt32(Address + ObjectConfig.BehaviorScriptOffset) & ~0x80000000;

            _gfxId = Config.Stream.GetUInt32(Address + ObjectConfig.BehaviorGfxOffset);
            _subType = Config.Stream.GetUInt32(Address + ObjectConfig.BehaviorSubtypeOffset);
            _appearance = Config.Stream.GetUInt32(Address + ObjectConfig.BehaviorAppearanceOffset);

            long behaviorOffset = (long)AbsoluteBehavior - Config.ObjectAssociations.BehaviorBankStart;
            if (AbsoluteBehavior == 0 || behaviorOffset < 0) // Behavior is 0 or is appears to be stored below the start
                SegmentedBehavior = 0;
            else 
                SegmentedBehavior = 0x13000000 + (uint) behaviorOffset;

            BehaviorCriteria = new BehaviorCriteria()
            {
                BehaviorAddress = RomVersionConfig.Switch(SegmentedBehavior, Config.ObjectAssociations.AlignJPBehavior(SegmentedBehavior)),
                GfxId = _gfxId,
                SubType = _subType,
                Appearance = _appearance
            };

            BehaviorAssociation = Config.ObjectAssociations.FindObjectAssociation(BehaviorCriteria);

            _x = Config.Stream.GetSingle(Address + ObjectConfig.XOffset);
            _y = Config.Stream.GetSingle(Address + ObjectConfig.YOffset);
            _z = Config.Stream.GetSingle(Address + ObjectConfig.ZOffset);

            _homeX = Config.Stream.GetSingle(Address + ObjectConfig.HomeXOffset);
            _homeY = Config.Stream.GetSingle(Address + ObjectConfig.HomeYOffset);
            _homeZ = Config.Stream.GetSingle(Address + ObjectConfig.HomeZOffset);

            _facingYaw = Config.Stream.GetUInt16(Address + ObjectConfig.YawFacingOffset);
            _facingPitch = Config.Stream.GetUInt16(Address + ObjectConfig.PitchFacingOffset);
            _facingRoll = Config.Stream.GetUInt16(Address + ObjectConfig.RollFacingOffset);

            _processedNextLink = Config.Stream.GetUInt32(Address + ObjectConfig.ProcessedNextLinkOffset);
            _processedPrevLink = Config.Stream.GetUInt32(Address + ObjectConfig.ProcessedPreviousLinkOffset);
        }

        public void Update2()
        {
            DistanceToMarioCalculated = MoreMath.GetDistanceBetween(_x, _y, _z,
                DataModels.Mario.X, DataModels.Mario.Y, DataModels.Mario.Z);
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null) || !(obj is ObjectDataModel))
                return false;

            return Address == (obj as ObjectDataModel).Address;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public override string ToString()
        {
            return BehaviorAssociation.Name + " " + HexUtilities.FormatValue(Address);
        }
    }
}
