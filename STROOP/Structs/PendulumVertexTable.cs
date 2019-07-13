using STROOP.Models;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class PendulumVertexTable
    {
        private Dictionary<int, List<TriangleShape>> _dictionary;

        public PendulumVertexTable()
        {
            _dictionary = new Dictionary<int, List<TriangleShape>>();
        }
        
        public void Add(int angle, List<TriangleShape> tris)
        {
            _dictionary[angle] = tris;
        }

        public bool HasVertexWithY(int angle, int y)
        {
            angle = MoreMath.NormalizeAngleTruncated(angle);
            List<TriangleShape> tris = _dictionary[angle];
            return tris.Any(tri => tri.Y1 == y || tri.Y2 == y || tri.Y3 == y);
        }
    }
}
