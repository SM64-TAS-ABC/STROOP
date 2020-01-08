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
    public class DeclareAssignNode : Node
    {
        private VarNode _left;
        private Token _token;
        private Node _right;

        public DeclareAssignNode(VarNode left, Token token, Node right)
        {
            _left = left;
            _token = token;
            _right = right;
        }

        public override object Evaluate()
        {
            string key = _left.GetName();
            object value = _right.Evaluate();
            if (SymbolTable.Has(key))
            {
                throw new Exception("cannot re-declare declared var: " + key);
            }
            SymbolTable.Set(key, value);
            return (key, value);
        }
    }
}
