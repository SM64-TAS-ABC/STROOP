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
    public class FunctionParamListNode : Node
    {
        private List<FunctionParamNode> _paramList;

        public FunctionParamListNode(List<FunctionParamNode> paramList)
        {
            _paramList = paramList;
        }

        public override object Evaluate()
        {
            // TODO
            return null;
        }
    }
}
