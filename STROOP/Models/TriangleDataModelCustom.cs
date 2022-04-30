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
    public class TriangleDataModelCustom : TriangleDataModel
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

        public TriangleDataModelCustom(int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3)
        {
            _x1 = (short)x1;
            _y1 = (short)y1;
            _z1 = (short)z1;
            _x2 = (short)x2;
            _y2 = (short)y2;
            _z2 = (short)z2;
            _x3 = (short)x3;
            _y3 = (short)y3;
            _z3 = (short)z3;

            (_normX, _normY, _normZ, _normOffset) = TriangleUtilities.GetNorms(x1, y1, z1, x2, y2, z2, x3, y3, z3);

            _yMinMinus5 = (short)(MoreMath.Min(y1, y2, y3) - 5);
            _yMaxPlus5 = (short)(MoreMath.Max(y1, y2, y3) + 5);

            _xProjection = NormX < -0.707 || NormX > 0.707;

            _classification = TriangleUtilities.CalculateClassification(NormY);
        }
    }
}