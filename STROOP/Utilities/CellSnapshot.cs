using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class CellSnapshot
    {
        private readonly List<TriangleDataModel>[,,] _staticTris;
        private readonly List<TriangleDataModel>[,,] _dynamicTris;

        public CellSnapshot()
        {
            _staticTris = GetTrianglesInPartition(true);
            _dynamicTris = GetTrianglesInPartition(false);
        }

        private List<TriangleDataModel>[,,] GetTrianglesInPartition(bool staticPartition)
        {
            List<TriangleDataModel>[,,] tris = new List<TriangleDataModel>[16, 16, 3];
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    tris[z, x, 0] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Floor).ConvertAll(triAddress => TriangleDataModel.Create(triAddress));
                    tris[z, x, 1] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Ceiling).ConvertAll(triAddress => TriangleDataModel.Create(triAddress));
                    tris[z, x, 2] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Wall).ConvertAll(triAddress => TriangleDataModel.Create(triAddress));
                }
            }
            return tris;
        }
    }
}
