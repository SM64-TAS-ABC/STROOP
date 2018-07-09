using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class TriangleInfoTable
    {
        public struct TriangleInfoReference
        {
            public ushort Type;
            public string Description;
            public ushort Slipperiness;
            public bool Exertion;

            public override int GetHashCode()
            {
                return Type;
            }
        }

        Dictionary<ushort, TriangleInfoReference> _table = new Dictionary<ushort, TriangleInfoReference>();

        public TriangleInfoTable()
        {
        }

        public void Add(TriangleInfoReference triangleInfoRef)
        {
            _table.Add(triangleInfoRef.Type, triangleInfoRef);
        }
        
        public string GetDescription(ushort type)
        {
            if (!_table.ContainsKey(type))
                return "Unknown Type";
            return _table[type].Description;
        }

        public ushort? GetSlipperiness(ushort type)
        {
            if (!_table.ContainsKey(type))
                return null;
            return _table[type].Slipperiness;
        }

        public string GetSlipperinessDescription(ushort type)
        {
            ushort? slipperiness = GetSlipperiness(type);
            switch (slipperiness)
            {
                case 0x00:
                    return "Default";
                case 0x13:
                    return "Slide";
                case 0x14:
                    return "Slippery";
                case 0x15:
                    return "Non-Slippery";
                default:
                    return "Unknown Slipperiness";
            }
        }

        public bool? GetExertion(ushort type)
        {
            if (!_table.ContainsKey(type))
                return null;
            return _table[type].Exertion;
        }
    }
}
