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
    public class BinaryOpNode : Node
    {
        private Node _left;
        private Token _token;
        private Node _right;

        public BinaryOpNode(Node left, Token token, Node right)
        {
            _left = left;
            _token = token;
            _right = right;
        }

        public override object GetValue()
        {
            switch (_token.Type)
            {
                case TokenType.ADD:
                    {
                        (double num1, double num2) = GetBothNumbers();
                        return num1 + num2;
                    }
                case TokenType.SUBTRACT:
                    {
                        (double num1, double num2) = GetBothNumbers();
                        return num1 - num2;
                    }
                case TokenType.MULTIPLY:
                    {
                        (double num1, double num2) = GetBothNumbers();
                        return num1 * num2;
                    }
                case TokenType.DIVIDE:
                    {
                        (double num1, double num2) = GetBothNumbers();
                        return num1 / num2;
                    }
            }

            throw new Exception("did not know how to get value for type " + _token.Type);
        }

        private (double num1, double num2) GetBothNumbers()
        {
            double? num1 = ParsingUtilities.ParseDoubleNullable(_left.GetValue());
            if (!num1.HasValue) throw new Exception("could not parse into number: " + _left.GetValue());
            double? num2 = ParsingUtilities.ParseDoubleNullable(_right.GetValue());
            if (!num2.HasValue) throw new Exception("could not parse into number: " + _right.GetValue());
            return (num1.Value, num2.Value);
        }
    }
}
