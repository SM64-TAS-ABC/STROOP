using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using OpenTK.Graphics;

namespace STROOP.Script
{
    public class TopLevelElementListNode : Node
    {
        private List<Node> _elementList;

        public TopLevelElementListNode(List<Node> elementList)
        {
            _elementList = new List<Node>(elementList);
        }

        public override object Evaluate()
        {
            List<object> values = new List<object>();
            foreach (Node element in _elementList)
            {
                values.Add(element.Evaluate());
            }
            return string.Join(",", values);
        }
    }
}
