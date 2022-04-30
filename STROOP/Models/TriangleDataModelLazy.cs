using STROOP.Structs.Configurations;
using STROOP.Utilities;
using STROOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;

namespace STROOP.Models
{
    public class TriangleDataModelLazy : TriangleDataModel
    {
        private readonly uint _address;
        public override uint Address
        {
            get => _address;
        }

        private short? _surfaceType = null;
        public override short SurfaceType
        {
            get
            {
                if (!_surfaceType.HasValue)
                {
                    _surfaceType = Config.Stream.GetShort(Address + TriangleOffsetsConfig.SurfaceType);
                }
                return _surfaceType.Value;
            }
        }

        private byte? _exertionForceIndex = null;
        public override byte ExertionForceIndex
        {
            get
            {
                if (!_exertionForceIndex.HasValue)
                {
                    _exertionForceIndex = Config.Stream.GetByte(Address + TriangleOffsetsConfig.ExertionForceIndex);
                }
                return _exertionForceIndex.Value;
            }
        }

        private byte? _exertionAngle = null;
        public override byte ExertionAngle
        {
            get
            {
                if (!_exertionAngle.HasValue)
                {
                    _exertionAngle = Config.Stream.GetByte(Address + TriangleOffsetsConfig.ExertionAngle);
                }
                return _exertionAngle.Value;
            }
        }

        private byte? _flags = null;
        public override byte Flags
        {
            get
            {
                if (!_flags.HasValue)
                {
                    _flags = Config.Stream.GetByte(Address + TriangleOffsetsConfig.Flags);
                }
                return _flags.Value;
            }
        }

        private byte? _room = null;
        public override byte Room
        {
            get
            {
                if (!_room.HasValue)
                {
                    _room = Config.Stream.GetByte(Address + TriangleOffsetsConfig.Room);
                }
                return _room.Value;
            }
        }

        private short? _yMinMinus5 = null;
        public override short YMinMinus5
        {
            get
            {
                if (!_yMinMinus5.HasValue)
                {
                    _yMinMinus5 = Config.Stream.GetShort(Address + TriangleOffsetsConfig.YMinMinus5);
                }
                return _yMinMinus5.Value;
            }
        }

        private short? _yMaxPlus5 = null;
        public override short YMaxPlus5
        {
            get
            {
                if (!_yMaxPlus5.HasValue)
                {
                    _yMaxPlus5 = Config.Stream.GetShort(Address + TriangleOffsetsConfig.YMaxPlus5);
                }
                return _yMaxPlus5.Value;
            }
        }

        private short? _x1 = null;
        public override short X1
        {
            get
            {
                if (!_x1.HasValue)
                {
                    _x1 = TriangleOffsetsConfig.GetX1(Address);
                }
                return _x1.Value;
            }
        }

        private short? _y1 = null;
        public override short Y1
        {
            get
            {
                if (!_y1.HasValue)
                {
                    _y1 = TriangleOffsetsConfig.GetY1(Address);
                }
                return _y1.Value;
            }
        }

        private short? _z1 = null;
        public override short Z1
        {
            get
            {
                if (!_z1.HasValue)
                {
                    _z1 = TriangleOffsetsConfig.GetZ1(Address);
                }
                return _z1.Value;
            }
        }

        private short? _x2 = null;
        public override short X2
        {
            get
            {
                if (!_x2.HasValue)
                {
                    _x2 = TriangleOffsetsConfig.GetX2(Address);
                }
                return _x2.Value;
            }
        }

        private short? _y2 = null;
        public override short Y2
        {
            get
            {
                if (!_y2.HasValue)
                {
                    _y2 = TriangleOffsetsConfig.GetY2(Address);
                }
                return _y2.Value;
            }
        }

        private short? _z2 = null;
        public override short Z2
        {
            get
            {
                if (!_z2.HasValue)
                {
                    _z2 = TriangleOffsetsConfig.GetZ2(Address);
                }
                return _z2.Value;
            }
        }

        private short? _x3 = null;
        public override short X3
        {
            get
            {
                if (!_x3.HasValue)
                {
                    _x3 = TriangleOffsetsConfig.GetX3(Address);
                }
                return _x3.Value;
            }
        }

        private short? _y3 = null;
        public override short Y3
        {
            get
            {
                if (!_y3.HasValue)
                {
                    _y3 = TriangleOffsetsConfig.GetY3(Address);
                }
                return _y3.Value;
            }
        }

        private short? _z3 = null;
        public override short Z3
        {
            get
            {
                if (!_z3.HasValue)
                {
                    _z3 = TriangleOffsetsConfig.GetZ3(Address);
                }
                return _z3.Value;
            }
        }

        private float? _normX = null;
        public override float NormX
        {
            get
            {
                if (!_normX.HasValue)
                {
                    _normX = Config.Stream.GetFloat(Address + TriangleOffsetsConfig.NormX);
                }
                return _normX.Value;
            }
        }

        private float? _normY = null;
        public override float NormY
        {
            get
            {
                if (!_normY.HasValue)
                {
                    _normY = Config.Stream.GetFloat(Address + TriangleOffsetsConfig.NormY);
                }
                return _normY.Value;
            }
        }

        private float? _normZ = null;
        public override float NormZ
        {
            get
            {
                if (!_normZ.HasValue)
                {
                    _normZ = Config.Stream.GetFloat(Address + TriangleOffsetsConfig.NormZ);
                }
                return _normZ.Value;
            }
        }

        private float? _normOffset = null;
        public override float NormOffset
        {
            get
            {
                if (!_normOffset.HasValue)
                {
                    _normOffset = TriangleOffsetsConfig.GetNormalOffset(Address);
                }
                return _normOffset.Value;
            }
        }

        private uint? _associatedObject = null;
        public override uint AssociatedObject
        {
            get
            {
                if (!_associatedObject.HasValue)
                {
                    _associatedObject = Config.Stream.GetUInt(Address + TriangleOffsetsConfig.AssociatedObject);
                }
                return _associatedObject.Value;
            }
        }

        private TriangleClassification? _classification = null;
        public override TriangleClassification Classification
        {
            get
            {
                if (!_classification.HasValue)
                {
                    _classification = TriangleUtilities.CalculateClassification(NormY);
                }
                return _classification.Value;
            }
        }

        private bool? _xProjection = null;
        public override bool XProjection
        {
            get
            {
                if (!_xProjection.HasValue)
                {
                    _xProjection = (Flags & TriangleOffsetsConfig.XProjectionMask) != 0;
                }
                return _xProjection.Value;
            }
        }

        private bool? _belongsToObject = null;
        public override bool BelongsToObject
        {
            get
            {
                if (!_belongsToObject.HasValue)
                {
                    _belongsToObject = (Flags & TriangleOffsetsConfig.BelongsToObjectMask) != 0;
                }
                return _belongsToObject.Value;
            }
        }

        private bool? _noCamCollision = null;
        public override bool NoCamCollision
        {
            get
            {
                if (!_noCamCollision.HasValue)
                {
                    _noCamCollision = (Flags & TriangleOffsetsConfig.NoCamCollisionMask) != 0;
                }
                return _noCamCollision.Value;
            }
        }

        private string _description = null;
        public override string Description
        {
            get
            {
                if (_description == null)
                {
                    _description = TableConfig.TriangleInfo.GetDescription(SurfaceType);
                }
                return _description;
            }
        }

        private short? _slipperiness = null;
        public override short Slipperiness
        {
            get
            {
                if (!_slipperiness.HasValue)
                {
                    _slipperiness = TableConfig.TriangleInfo.GetSlipperiness(SurfaceType) ?? 0;
                }
                return _slipperiness.Value;
            }
        }

        private string _slipperinessDescription = null;
        public override string SlipperinessDescription
        {
            get
            {
                if (_slipperinessDescription == null)
                {
                    _slipperinessDescription = TableConfig.TriangleInfo.GetSlipperinessDescription(SurfaceType);
                }
                return _slipperinessDescription;
            }
        }

        private double? _frictionMultiplier = null;
        public override double FrictionMultiplier
        {
            get
            {
                if (!_frictionMultiplier.HasValue)
                {
                    _frictionMultiplier = TableConfig.TriangleInfo.GetFrictionMultiplier(SurfaceType);
                }
                return _frictionMultiplier.Value;
            }
        }

        private double? _slopeAccel = null;
        public override double SlopeAccel
        {
            get
            {
                if (!_slopeAccel.HasValue)
                {
                    _slopeAccel = TableConfig.TriangleInfo.GetSlopeAccel(SurfaceType);
                }
                return _slopeAccel.Value;
            }
        }

        private double? _slopeDecelValue = null;
        public override double SlopeDecelValue
        {
            get
            {
                if (!_slopeDecelValue.HasValue)
                {
                    _slopeDecelValue = TableConfig.TriangleInfo.GetSlopeDecelValue(SurfaceType);
                }
                return _slopeDecelValue.Value;
            }
        }

        private bool? _exertion = null;
        public override bool Exertion
        {
            get
            {
                if (!_exertion.HasValue)
                {
                    _exertion = TableConfig.TriangleInfo.GetExertion(SurfaceType) ?? false;
                }
                return _exertion.Value;
            }
        }

        private List<object> _fieldValueList = null;
        public override List<object> FieldValueList
        {
            get
            {
                if (_fieldValueList == null)
                {
                    _fieldValueList = new List<object> {
                        HexUtilities.FormatValue(Address, 8),
                        Classification,
                        HexUtilities.FormatValue(SurfaceType, 2),
                        Description,
                        HexUtilities.FormatValue(Slipperiness, 2),
                        SlipperinessDescription,
                        Exertion,
                        ExertionForceIndex,
                        ExertionAngle,
                        HexUtilities.FormatValue(Flags, 2),
                        XProjection,
                        BelongsToObject,
                        NoCamCollision,
                        Room,
                        YMinMinus5,
                        YMaxPlus5,
                        X1,
                        Y1,
                        Z1,
                        X2,
                        Y2,
                        Z2,
                        X3,
                        Y3,
                        Z3,
                        NormX,
                        NormY,
                        NormZ,
                        NormOffset,
                        HexUtilities.FormatValue(AssociatedObject, 8),
                    };
                }
                return _fieldValueList;
            }
        }

        public TriangleDataModelLazy(uint triangleAddress)
        {
            _address = triangleAddress;
        }
    }
}
