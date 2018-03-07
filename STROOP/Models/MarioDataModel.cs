using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;

namespace STROOP.Models
{
    public class MarioDataModel : IUpdatableDataModel
    {
        #region Position
        private float _x;
        public float X
        {
            get => _x;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.XOffset))
                    _x = value;
            }
        }

        private float _y;
        public float Y
        {
            get => _y;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.YOffset))
                    _y = value;
            }
        }

        private float _z;
        public float Z
        {
            get => _z;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.ZOffset))
                    _z = value;
            }
        }
        #endregion
        #region Rotation
        private ushort _facingYaw;
        public ushort FacingYaw
        {
            get => _facingYaw;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.FacingYawOffset))
                    _facingYaw = value;
            }
        }
        private ushort _facingPitch;
        public ushort FacingPitch
        {
            get => _facingPitch;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.FacingPitchOffset))
                    _facingPitch = value;
            }
        }
        private ushort _facingRoll;
        public ushort FacingRoll
        {
            get => _facingRoll;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.FacingRollOffset))
                    _facingRoll = value;
            }
        }
        #endregion
        #region HOLP/Held
        private float _holpX;
        public float HolpX
        {
            get => _holpX;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.HOLPXOffset))
                    _holpX = value;
            }
        }

        private float _holpY;
        public float HolpY
        {
            get => _holpY;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.HOLPYOffset))
                    _holpY = value;
            }
        }

        private float _holpZ;
        public float HolpZ
        {
            get => _holpZ;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.HOLPZOffset))
                    _holpZ = value;
            }
        }
        #endregion
        #region Speed
        private float _hSpeed;
        public float HSpeed
        {
            get => _hSpeed;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.HSpeedOffset))
                    _hSpeed = value;
            }
        }

        private double _defactoSpeed;
        public double DeFactoSpeed
        {
            get => DeFactoMultiplier * _hSpeed;
            set
            {
                float newValue = (float)(value / DeFactoMultiplier);
                if (Config.Stream.SetValue(newValue, MarioConfig.StructAddress + MarioConfig.HSpeedOffset))
                    _defactoSpeed = newValue;
            }
        }

        public float DeFactoMultiplier
        {
            get => AboveFloor ? 1.0f : _normalY;
        }

        public bool IsStationary
        {
            get;
            private set;
        }
        #endregion
        #region Floors/Ceilings/Walls
        private TriangleDataModel _floorTriangle;
        public TriangleDataModel FloorTriangle
        {
            get => _floorTriangle;
        }

        private TriangleDataModel _wallTriangle;
        public TriangleDataModel WallTriangle
        {
            get => _wallTriangle;
        }

        private TriangleDataModel _ceilingTriangle;
        public TriangleDataModel CeilingTriangle
        {
            get => _ceilingTriangle;
        }

        private float _floorY;
        public float FloorY
        {
            get => _floorY;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.FloorYOffset))
                    _floorY = value;
            }
        }

        private float _normalY;
        public float NormalY
        {
            get => _normalY;
        }

        public bool AboveFloor
        {
            get => _y > _floorY + 0.001f; // Epsilon
        }
        #endregion
        #region QStep
        public double NextIntendedQStepX
        {
            get;
            private set;
        }

        public double NextIntendedQStepZ
        {
            get;
            private set;
        }

        public double NextIntendedQStepDirection
        {
            get => MoreMath.AngleTo_AngleUnits(_x, _z, NextIntendedQStepX, NextIntendedQStepZ);
        }

        public double DeFactoSpeedQStep
        {
            get => DeFactoSpeed / 4;
        }
        #endregion
        #region PU
        public int PU_X
        {
            get => PuUtilities.GetPuIndex(X);
            set => X = (float)PuUtilities.GetCoordinateInPu(X, value);
        }

        public int PU_Y
        {
            get => PuUtilities.GetPuIndex(Y);
            set => Y = (float)PuUtilities.GetCoordinateInPu(Y, value);
        }

        public int PU_Z
        {
            get => PuUtilities.GetPuIndex(Z);
            set => Z = (float)PuUtilities.GetCoordinateInPu(Z, value);
        }

        public float PURelative_X
        {
            get => (float)PuUtilities.GetRelativeCoordinate(X);
            set => X = (float)PuUtilities.GetCoordinateInPu(value, PU_X);
        }

        public float PURelative_Y
        {
            get => (float)PuUtilities.GetRelativeCoordinate(Y);
            set => Y = (float)PuUtilities.GetCoordinateInPu(value, PU_Y);
        }

        public float PURelative_Z
        {
            get => (float)PuUtilities.GetRelativeCoordinate(Z);
            set => Z = (float)PuUtilities.GetCoordinateInPu(value, PU_Z);
        }

        public int QPU_X
        {
            get => PU_X / 4;
            set => PU_X = value * 4;
        }

        public int QPU_Y
        {
            get => PU_Y / 4;
            set => PU_Y = value * 4;
        }

        public int QPU_Z
        {
            get => PU_Z / 4;
            set => PU_Z = value * 4;
        }
        #endregion
        #region Statuses
        private uint _action;
        public uint Action
        {
            get => _action;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.ActionOffset))
                    _action = value;
            }
        }
        #endregion
        #region Objects
        private uint _heldObject;
        public uint HeldObject
        {
            get => _heldObject;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.HeldObjectPointerOffset))
                    _heldObject = value;
            }
        }

        private uint _usedObject;
        public uint UsedObject
        {
            get => _usedObject;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.UsedObjectPointerOffset))
                    _usedObject = value;
            }
        }

        private uint _stoodOnObject;
        public uint StoodOnObject
        {
            get => _stoodOnObject;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.StoodOnObjectPointerAddress))
                    _stoodOnObject = value;
            }
        }

        private uint _interactionObject;
        public uint InteractionObject
        {
            get => _interactionObject;
            set
            {
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.InteractionObjectPointerOffset))
                    _interactionObject = value;
            }
        }
        public uint ClosestObject { get; private set; }
        #endregion

        public void Update()
        {
            // Get Mario position
            _x = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            _y = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            _z = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            // Get rotation
            _facingYaw = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            _facingPitch = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingPitchOffset);
            _facingRoll = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingRollOffset);

            // Get holp position
            _holpX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPXOffset);
            _holpY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPYOffset);
            _holpZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPZOffset);

            // Update triangles
            UInt32 floorTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            _floorTriangle = floorTriangleAddress != 0x00 ? new TriangleDataModel(floorTriangleAddress) : null;
            UInt32 wallTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
            _wallTriangle = wallTriangleAddress != 0x00 ? new TriangleDataModel(wallTriangleAddress) : null;
            UInt32 ceilingTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
            _ceilingTriangle = ceilingTriangleAddress != 0x00 ? new TriangleDataModel(ceilingTriangleAddress) : null;

            _heldObject = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.HeldObjectPointerOffset);
            _stoodOnObject = Config.Stream.GetUInt32(MarioConfig.StoodOnObjectPointerAddress);
            _interactionObject = Config.Stream.GetUInt32(MarioConfig.InteractionObjectPointerOffset + MarioConfig.StructAddress);
            _usedObject = Config.Stream.GetUInt32(MarioConfig.UsedObjectPointerOffset + MarioConfig.StructAddress);

            // Find closest object
            IEnumerable<ObjectDataModel> closestObjectCandidates =
               DataModels.Objects.Where(o => o != null && o.IsActive && o.BehaviorCriteria.BehaviorAddress != MarioObjectConfig.BehaviorValue);
            if (OptionsConfig.ExcludeDustForClosestObject)
            {
                closestObjectCandidates =
                    closestObjectCandidates.Where(o =>
                        o.AbsoluteBehavior != ObjectConfig.DustSpawnerBehaviorValue
                        && o.AbsoluteBehavior != ObjectConfig.DustBallBehaviorValue
                        && o.AbsoluteBehavior != ObjectConfig.DustBehaviorValue);
            }
            ClosestObject = closestObjectCandidates.OrderBy(o => o.DistanceToMarioCalculated).FirstOrDefault()?.Address ?? 0;

            _hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);

            _action = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.ActionOffset);
           
            _floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
            _normalY = _floorTriangle == null ? 1 : _floorTriangle.NormY;

            ushort marioAngleTruncated = MoreMath.NormalizeAngleTruncated(_facingYaw);
            (double xDist, double zDist) = MoreMath.GetComponentsFromVector(DeFactoSpeedQStep, marioAngleTruncated);
            NextIntendedQStepX = MoreMath.MaybeNegativeModulus(_x + xDist, 65536);
            NextIntendedQStepZ = MoreMath.MaybeNegativeModulus(_z + zDist, 65536);
            IsStationary = _x == NextIntendedQStepX && _z == NextIntendedQStepZ;
        }

        public void Update2() { }
    }
}
