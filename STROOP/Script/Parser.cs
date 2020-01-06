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
    public class Parser
    {
        private Tokenizer _tokenizer;
        private Token _currentToken;

        public Parser(string text)
        {
            _tokenizer = new Tokenizer(text);
            _currentToken = _tokenizer.GetNextToken();
        }

        public void Eat(TokenType type)
        {
            if (_currentToken.Type == type)
            {
                _currentToken = _tokenizer.GetNextToken();
            }
            else
            {
                throw new Exception(
                    "cannot eat when current token type does not match given type: "
                    + _currentToken.Type + " vs " + type);
            }
        }

        public Node GetFactor()
        {
            Token token = _currentToken;
            if (token.Type == TokenType.ADD)
            {
                Eat(TokenType.ADD);
                return new UnaryOpNode(token, GetFactor());
            }
            else if (token.Type == TokenType.SUBTRACT)
            {
                Eat(TokenType.SUBTRACT);
                return new UnaryOpNode(token, GetFactor());
            }
            else if (token.Type == TokenType.NUMBER)
            {
                Eat(TokenType.NUMBER);
                return new NumberNode(token);
            }
            else if (token.Type == TokenType.LEFT_PAREN)
            {
                Eat(TokenType.LEFT_PAREN);
                Node node = GetExpression();
                Eat(TokenType.RIGHT_PAREN);
            }
            throw new Exception("cannot get factor of token with type: " + token.Type);
        }

        public Node GetTerm()
        {
            Node node = GetFactor();

            while (_currentToken.Type == TokenType.MULTIPLY ||
                _currentToken.Type == TokenType.DIVIDE)
            {
                Token token = _currentToken;
                if (token.Type == TokenType.MULTIPLY)
                {
                    Eat(TokenType.MULTIPLY);
                }
                else if (token.Type == TokenType.DIVIDE)
                {
                    Eat(TokenType.DIVIDE);
                }
                node = new BinaryOpNode(node, token, GetFactor());
            }

            return node;
        }

        public Node GetExpression()
        {
            Node node = GetTerm();

            while (_currentToken.Type == TokenType.ADD ||
                _currentToken.Type == TokenType.SUBTRACT)
            {
                Token token = _currentToken;
                if (token.Type == TokenType.ADD)
                {
                    Eat(TokenType.ADD);
                }
                else if (token.Type == TokenType.SUBTRACT)
                {
                    Eat(TokenType.SUBTRACT);
                }
                node = new BinaryOpNode(node, token, GetTerm());
            }

            return node;
        }

        public Node Parse()
        {
            return GetExpression();
        }
    }
}
