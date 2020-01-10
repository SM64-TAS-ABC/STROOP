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
                throw new Exception("expected " + type + " but found " + _currentToken.Type);
            }
        }

        public VarNode GetVariable()
        {
            VarNode node = new VarNode(_currentToken);
            Eat(TokenType.ID);
            return node;
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
                return node;
            }
            else if (token.Type == TokenType.ID)
            {
                return GetVariable();
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

        public Node GetAssignmentStatement()
        {
            VarNode left = GetVariable();
            Token token = _currentToken;
            Eat(TokenType.ASSIGN);
            Node right = GetExpression();
            Eat(TokenType.SEMI);
            return new AssignNode(left, token, right);
        }

        public Node GetDeclarationAssignStatement()
        {
            Eat(TokenType.VAR);
            VarNode left = GetVariable();
            Token token = _currentToken;
            Eat(TokenType.ASSIGN);
            Node right = GetExpression();
            Eat(TokenType.SEMI);
            return new DeclareAssignNode(left, token, right);
        }

        public Node GetStatement()
        {
            if (_currentToken.Type == TokenType.ID)
            {
                return GetAssignmentStatement();
            }
            if (_currentToken.Type == TokenType.VAR)
            {
                return GetDeclarationAssignStatement();
            }

            throw new Exception("cannot start a statement with type: " + _currentToken.Type);
        }

        public StatementListNode GetStatementList()
        {
            List<Node> statementList = new List<Node>();
            while (_currentToken.Type != TokenType.RIGHT_BRACE)
            {
                statementList.Add(GetStatement());
            }
            return new StatementListNode(statementList);
        }

        public FunctionParamListNode GetFunctionParamList()
        {
            // Handle empty case
            if (_currentToken.Type == TokenType.RIGHT_PAREN)
            {
                new FunctionParamListNode(new List<VarNode>());
            }

            // Handle non-empty case
            List<VarNode> paramList = new List<VarNode>() { GetVariable() };
            while (_currentToken.Type != TokenType.RIGHT_PAREN)
            {
                Eat(TokenType.COMMA);
                paramList.Add(GetVariable());
            }
            return new FunctionParamListNode(paramList);
        }

        public DeclareFunctionNode GetDeclareFunction()
        {
            Eat(TokenType.FUNCTION);
            VarNode name = GetVariable();
            Eat(TokenType.LEFT_PAREN);
            FunctionParamListNode paramList = GetFunctionParamList();
            Eat(TokenType.RIGHT_PAREN);
            Eat(TokenType.LEFT_BRACE);
            StatementListNode statementList = GetStatementList();
            Eat(TokenType.RIGHT_BRACE);
            return new DeclareFunctionNode(name, paramList, statementList);
        }

        public Node GetTopLevelElementsNode()
        {
            List<Node> elementList = new List<Node>();
            while (_currentToken.Type != TokenType.EOF)
            {
                if (_currentToken.Type == TokenType.FUNCTION)
                {
                    elementList.Add(GetDeclareFunction());
                }
                else if (_currentToken.Type == TokenType.ID)
                {
                    elementList.Add(GetAssignmentStatement());
                }
                else if (_currentToken.Type == TokenType.VAR)
                {
                    elementList.Add(GetDeclarationAssignStatement());
                }
                else
                {
                    throw new Exception("cannot get top level element with type: " + _currentToken.Type);
                }
            }
            return new TopLevelElementListNode(elementList);
        }

        public Node GetProgram()
        {
            return GetTopLevelElementsNode();
        }

        public Node Parse()
        {
            return GetProgram();
        }
    }
}
