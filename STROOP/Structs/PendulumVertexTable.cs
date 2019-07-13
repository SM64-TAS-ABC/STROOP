using STROOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class PendulumVertexTable
    {
        public class PendulumVertexData
        {
            public readonly int Angle;
            public readonly List<TriangleShape> Tris;

            public PendulumVertexData(int angle, List<TriangleShape> tris)
            {
                Angle = angle;
                Tris = tris;
            }
        }
        
        public void Add(PendulumVertexData data)
        {

        }
    }
}
