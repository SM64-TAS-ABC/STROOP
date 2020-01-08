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
using STROOP.Forms;

namespace STROOP.Script
{
    public class TokenScript
    {
        private bool _isEnabled = false;
        private string _text = "";

        public TokenScript()
        {
        }

        public void SetScript(string text)
        {
            _text = text;
        }

        public void SetIsEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public void Update()
        {
            if (!_isEnabled) return;
            Run();
        }

        public void Run()
        {
            try
            {
                SymbolTable.Reset();
                Tokenizer tokenizer = new Tokenizer(_text);
                List<Token> tokens = new List<Token>();
                while (true)
                {
                    Token token = tokenizer.GetNextToken();
                    if (token.Type == TokenType.EOF) break;
                    tokens.Add(token);
                }
                Parser parser = new Parser(_text);
                object result = parser.Parse().Evaluate();
                InfoForm.ShowValue(
                    string.Join(",", tokens)
                        + "\r\n\r\n"
                        + result
                        + "\r\n\r\n"
                        + SymbolTable.GetString(),
                    "Tokenizer Results",
                    tokens.Count + " tokens");
            }
            catch (Exception e)
            {
                InfoForm.ShowValue(e.Message, "Error", "Error");
            }
        }
    }
}
