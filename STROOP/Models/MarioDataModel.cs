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
    public class MarioDataModel : UpdatableDataModel
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
                if (Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.YawFacingOffset))
                    _facingYaw = value;
            }
        }
        #endregion
        #region HOLP
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
            set => X = PuUtilities.GetRelativeCoordinate(X) + value * PuUtilities.PuSize;
        }

        public int PU_Y
        {
            get => PuUtilities.GetPuIndex(Y);
            set => Y = PuUtilities.GetRelativeCoordinate(Y) + value * PuUtilities.PuSize;
        }

        public int PU_Z
        {
            get => PuUtilities.GetPuIndex(Z);
            set => Z = PuUtilities.GetRelativeCoordinate(Z) + value * PuUtilities.PuSize;
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

        public override void Update(int dependencyLevel)
        {
            switch (dependencyLevel)
            {
                case 0:
                    // Get Mario position and rotation
                    _x = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    _y = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    _z = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    _facingYaw = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.YawFacingOffset);

                    // Get holp position
                    _holpX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPXOffset);
                    _holpY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPYOffset);
                    _holpZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HOLPZOffset);

                    _hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);

                    // Update triangles
                    UInt32 floorTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                    _floorTriangle = floorTriangleAddress != 0x00 ? new TriangleDataModel(floorTriangleAddress) : null;
                    UInt32 wallTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
                    _wallTriangle = wallTriangleAddress != 0x00 ? new TriangleDataModel(wallTriangleAddress) : null;
                    UInt32 ceilingTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
                    _ceilingTriangle = ceilingTriangleAddress != 0x00 ? new TriangleDataModel(ceilingTriangleAddress) : null;

                    _floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    _normalY = _floorTriangle == null ? 1 : _floorTriangle.NormY;

                    ushort marioAngleTruncated = MoreMath.NormalizeAngleTruncated(_facingYaw);
                    (double xDist, double zDist) = MoreMath.GetComponentsFromVector(DeFactoSpeedQStep, marioAngleTruncated);
                    NextIntendedQStepX = MoreMath.MaybeNegativeModulus(_x + xDist, 65536);
                    NextIntendedQStepZ = MoreMath.MaybeNegativeModulus(_z + zDist, 65536);
                    IsStationary = _x == NextIntendedQStepX && _z == NextIntendedQStepZ;
                    break;
            }        
        }
    }
}
