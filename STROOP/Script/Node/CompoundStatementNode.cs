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
    public class CompoundStatementNode : Node
    {
        private List<Token> _statementList;

        public CompoundStatementNode()
        {
            _statementList = new List<Token>();
        }

        public override object GetValue()
        {
            return null;
        }
    }
}
