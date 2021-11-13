using STROOP.Controls;
using STROOP.Forms;
using STROOP.M64;
using STROOP.Managers;
using STROOP.Map;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Utilities
{
    public static class TestUtilities2
    {
        public static void Test()
        {
            float baseX1 = -1668.14196777344f;
            float baseY1 = 201.785629272461f;
            float baseZ1 = -917.000915527344f;

            float baseX2 = -1668.00183105469f;
            float baseY2 = 201.785629272461f;
            float baseZ2 = -917.141052246094f;

            uint tri1Address = 0x8016B010;
            uint tri2Address = 0x8016B040;
            TriangleDataModel tri1 = TriangleDataModel.Create(tri1Address);
            TriangleDataModel tri2 = TriangleDataModel.Create(tri2Address);

            List<(float x, float y, float z)> points = new List<(float x, float y, float z)>();
            for (int i = -8; i <= 1; i++)
            {
                float x1 = baseX1 + i;
                float z1 = baseZ1 - i;
                float? tri1Y1 = tri1.GetTruncatedHeightOnTriangleIfInsideTriangle(x1, z1);
                float? tri2Y1 = tri2.GetTruncatedHeightOnTriangleIfInsideTriangle(x1, z1);
                float y1 = tri1Y1 ?? tri2Y1 ?? 0;
                points.Add((x1, y1, z1));

                float x2 = baseX2 + i;
                float z2 = baseZ2 - i;
                float? tri1Y2 = tri1.GetTruncatedHeightOnTriangleIfInsideTriangle(x2, z2);
                float? tri2Y2 = tri2.GetTruncatedHeightOnTriangleIfInsideTriangle(x2, z2);
                float y2 = tri1Y2 ?? tri2Y2 ?? 0;
                points.Add((x2, y2, z2));
            }
            
            foreach (var point in points)
            {
                Config.Print(point.x + " " + (point.y + 60) + " " + point.z);
            }
        }
    }
}
