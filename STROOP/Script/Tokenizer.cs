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
    public class Tokenizer
    {
        private string _text;
        private int _index;
        private char? _currentChar;

        public Tokenizer(string text)
        {
            _text = text;
            _index = 0;
            _currentChar = text.Length == 0 ? (char?)null : _text[0];
        }

        private void Advance()
        {
            _index++;
            if (_index >= _text.Length)
            {
                _currentChar = null;
            }
            else
            {
                _currentChar = _text[_index];
            }
        }

        private char? Peek()
        {
            int peekIndex = _index + 1;
            if (peekIndex >= _text.Length)
            {
                return null;
            }
            return _text[peekIndex];
        }

        private void SkipWhiteSpace()
        {
            while (_currentChar.HasValue && char.IsWhiteSpace(_currentChar.Value))
            {
                Advance();
            }
        }

        private double GetNumber()
        {
            string result = "";
            while (_currentChar.HasValue && char.IsDigit(_currentChar.Value))
            {
                result += _currentChar.Value;
                Advance();
            }
            double? parsed = ParsingUtilities.ParseDoubleNullable(result);
            if (!parsed.HasValue)
            {
                throw new Exception("failed to parse double");
            }
            return parsed.Value;
        }

        private static readonly Dictionary<string, Token> RESERVED_WORDS =
            new Dictionary<string, Token>()
            {
                ["if"] = new Token(TokenType.IF, "if"),
            };

        private Token GetId()
        {
            string result = "";
            while (_currentChar.HasValue && char.IsLetterOrDigit(_currentChar.Value))
            {
                result += _currentChar.Value;
                Advance();
            }

            if (RESERVED_WORDS.ContainsKey(result))
            {
                return RESERVED_WORDS[result];
            }
            return new Token(TokenType.ID, result);
        }

        public Token GetNextToken()
        {
            while (_currentChar.HasValue)
            {
                if (char.IsWhiteSpace(_currentChar.Value))
                {
                    SkipWhiteSpace();
                    if (!_currentChar.HasValue) return new Token(TokenType.EOF, null);
                }

                if (char.IsDigit(_currentChar.Value))
                {
                    return new Token(TokenType.NUMBER, GetNumber());
                }

                if (char.IsLetter(_currentChar.Value))
                {
                    return GetId();
                }

                if (_currentChar == '=')
                {
                    Advance();
                    return new Token(TokenType.ASSIGN, "=");
                }

                if (_currentChar == ';')
                {
                    Advance();
                    return new Token(TokenType.SEMI, ";");
                }

                if (_currentChar == '+')
                {
                    Advance();
                    return new Token(TokenType.ADD, "+");
                }

                if (_currentChar == '-')
                {
                    Advance();
                    return new Token(TokenType.SUBTRACT, "-");
                }

                if (_currentChar == '*')
                {
                    Advance();
                    return new Token(TokenType.MULTIPLY, "*");
                }

                if (_currentChar == '/')
                {
                    Advance();
                    return new Token(TokenType.DIVIDE, "/");
                }

                if (_currentChar == '(')
                {
                    Advance();
                    return new Token(TokenType.LEFT_PAREN, "(");
                }

                if (_currentChar == ')')
                {
                    Advance();
                    return new Token(TokenType.RIGHT_PAREN, ")");
                }

                throw new Exception("unknown char encountered: " + _currentChar);
            }

            return new Token(TokenType.EOF, null);
        }
    }
}
