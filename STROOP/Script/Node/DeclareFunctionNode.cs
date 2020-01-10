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
    public class DeclareFunctionNode : Node
    {
        private VarNode _name;
        private FunctionParamListNode _paramList;
        private StatementListNode _statementList;

        public DeclareFunctionNode(VarNode name, FunctionParamListNode paramList, StatementListNode statementList)
        {
            _name = name;
            _paramList = paramList;
            _statementList = statementList;
        }

        public override object Evaluate()
        {
            // TODO
            return null;
        }
    }
}
