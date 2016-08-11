using SM64_Diagnostic.Controls.Map;
using SM64_Diagnostic.ManagerClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class TriangleMapObject : MapBaseObject
    {

        public float X1, X2, X3;
        public float Z1, Z2, Z3;


        public PointF P1OnControl, P2OnControl, P3OnControl;


        public TriangleMapObject()
        {

        }

        public override void DrawOnControl(MapGraphics graphics)
        {
            
        }
    }
}
