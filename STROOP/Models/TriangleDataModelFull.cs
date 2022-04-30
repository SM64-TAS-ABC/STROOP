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
    public class TriangleDataModelFull : TriangleDataModel
    {
        private readonly uint _address;
        public override uint Address { get => _address; }

        private readonly short _surfaceType;
        public override short SurfaceType { get => _surfaceType; }

        private readonly byte _exertionForceIndex;
        public override byte ExertionForceIndex { get => _exertionForceIndex; }

        private readonly byte _exertionAngle;
        public override byte ExertionAngle { get => _exertionAngle; }

        private readonly byte _flags;
        public override byte Flags { get => _flags; }

        private readonly byte _room;
        public override byte Room { get => _room; }

        private readonly short _yMinMinus5;
        public override short YMinMinus5 { get => _yMinMinus5; }

        private readonly short _yMaxPlus5;
        public override short YMaxPlus5 { get => _yMaxPlus5; }

        private readonly short _x1;
        public override short X1 { get => _x1; }

        private readonly short _y1;
        public override short Y1 { get => _y1; }

        private readonly short _z1;
        public override short Z1 { get => _z1; }

        private readonly short _x2;
        public override short X2 { get => _x2; }

        private readonly short _y2;
        public override short Y2 { get => _y2; }

        private readonly short _z2;
        public override short Z2 { get => _z2; }

        private readonly short _x3;
        public override short X3 { get => _x3; }

        private readonly short _y3;
        public override short Y3 { get => _y3; }

        private readonly short _z3;
        public override short Z3 { get => _z3; }

        private readonly float _normX;
        public override float NormX { get => _normX; }

        private readonly float _normY;
        public override float NormY { get => _normY; }

        private readonly float _normZ;
        public override float NormZ { get => _normZ; }

        private readonly float _normOffset;
        public override float NormOffset { get => _normOffset; }

        private readonly uint _associatedObject;
        public override uint AssociatedObject { get => _associatedObject; }

        private readonly TriangleClassification _classification;
        public override TriangleClassification Classification { get => _classification; }

        private readonly bool _xProjection;
        public override bool XProjection { get => _xProjection; }

        private readonly bool _belongsToObject;
        public override bool BelongsToObject { get => _belongsToObject; }

        private readonly bool _noCamCollision;
        public override bool NoCamCollision { get => _noCamCollision; }

        private readonly string _description;
        public override string Description { get => _description; }

        private readonly short _slipperiness;
        public override short Slipperiness { get => _slipperiness; }

        private readonly string _slipperinessDescription;
        public override string SlipperinessDescription { get => _slipperinessDescription; }

        private readonly double _frictionMultiplier;
        public override double FrictionMultiplier { get => _frictionMultiplier; }

        private readonly double _slopeAccel;
        public override double SlopeAccel { get => _slopeAccel; }

        private readonly double _slopeDecelValue;
        public override double SlopeDecelValue { get => _slopeDecelValue; }

        private readonly bool _exertion;
        public override bool Exertion { get => _exertion; }

        private readonly List<object> _fieldValueList;
        public override List<object> FieldValueList { get => _fieldValueList; }

        public TriangleDataModelFull(uint triangleAddress)
        {
            _address = triangleAddress;

            _surfaceType = Config.Stream.GetShort(triangleAddress + TriangleOffsetsConfig.SurfaceType);
            _exertionForceIndex = Config.Stream.GetByte(triangleAddress + TriangleOffsetsConfig.ExertionForceIndex);
            _exertionAngle = Config.Stream.GetByte(triangleAddress + TriangleOffsetsConfig.ExertionAngle);
            _flags = Config.Stream.GetByte(triangleAddress + TriangleOffsetsConfig.Flags);
            _room = Config.Stream.GetByte(triangleAddress + TriangleOffsetsConfig.Room);

            _yMinMinus5 = Config.Stream.GetShort(triangleAddress + TriangleOffsetsConfig.YMinMinus5);
            _yMaxPlus5 = Config.Stream.GetShort(triangleAddress + TriangleOffsetsConfig.YMaxPlus5);

            _x1 = TriangleOffsetsConfig.GetX1(triangleAddress);
            _y1 = TriangleOffsetsConfig.GetY1(triangleAddress);
            _z1 = TriangleOffsetsConfig.GetZ1(triangleAddress);
            _x2 = TriangleOffsetsConfig.GetX2(triangleAddress);
            _y2 = TriangleOffsetsConfig.GetY2(triangleAddress);
            _z2 = TriangleOffsetsConfig.GetZ2(triangleAddress);
            _x3 = TriangleOffsetsConfig.GetX3(triangleAddress);
            _y3 = TriangleOffsetsConfig.GetY3(triangleAddress);
            _z3 = TriangleOffsetsConfig.GetZ3(triangleAddress);

            _normX = Config.Stream.GetFloat(triangleAddress + TriangleOffsetsConfig.NormX);
            _normY = Config.Stream.GetFloat(triangleAddress + TriangleOffsetsConfig.NormY);
            _normZ = Config.Stream.GetFloat(triangleAddress + TriangleOffsetsConfig.NormZ);
            _normOffset = TriangleOffsetsConfig.GetNormalOffset(triangleAddress);

            _associatedObject = Config.Stream.GetUInt(triangleAddress + TriangleOffsetsConfig.AssociatedObject);

            _classification = TriangleUtilities.CalculateClassification(NormY);

            _xProjection = (Flags & TriangleOffsetsConfig.XProjectionMask) != 0;
            _belongsToObject = (Flags & TriangleOffsetsConfig.BelongsToObjectMask) != 0;
            _noCamCollision = (Flags & TriangleOffsetsConfig.NoCamCollisionMask) != 0;

            _description = TableConfig.TriangleInfo.GetDescription(SurfaceType);
            _slipperiness = TableConfig.TriangleInfo.GetSlipperiness(SurfaceType) ?? 0;
            _slipperinessDescription = TableConfig.TriangleInfo.GetSlipperinessDescription(SurfaceType);
            _frictionMultiplier = TableConfig.TriangleInfo.GetFrictionMultiplier(SurfaceType);
            _slopeAccel = TableConfig.TriangleInfo.GetSlopeAccel(SurfaceType);
            _slopeDecelValue = TableConfig.TriangleInfo.GetSlopeDecelValue(SurfaceType);
            _exertion = TableConfig.TriangleInfo.GetExertion(SurfaceType) ?? false;

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
    }
}