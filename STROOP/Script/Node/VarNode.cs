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
    public class VarNode : Node
    {
        private Token _token;
        private string _value;

        public VarNode(Token token)
        {
            _token = token;
            _value = _token.Value.ToString();
        }

        public override object Evaluate()
        {
            if (!SymbolTable.Has(_value))
            {
                throw new Exception("SymbolTable does not have key: " + _value);
            }
            return SymbolTable.Get(_value);
        }

        public string GetName()
        {
            return _value;
        }
    }
}
