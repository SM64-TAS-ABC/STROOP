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
    public class UnaryOpNode : Node
    {
        private Token _token;
        private Node _child;

        public UnaryOpNode(Token token, Node child)
        {
            _token = token;
            _child = child;
        }

        public override object Evaluate()
        {
            switch (_token.Type)
            {
                case TokenType.ADD:
                    {
                        return GetNumber();
                    }
                case TokenType.SUBTRACT:
                    {
                        return -1 * GetNumber();
                    }
            }

            throw new Exception("did not know how to get value for type " + _token.Type);
        }

        private double GetNumber()
        {
            double? num = ParsingUtilities.ParseDoubleNullable(_child.Evaluate());
            if (!num.HasValue) throw new Exception("could not parse into number: " + _child.Evaluate());
            return num.Value;
        }
    }
}
