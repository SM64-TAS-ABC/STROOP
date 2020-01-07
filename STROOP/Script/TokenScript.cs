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

        public TokenScript()
        {
        }

        public void SetScript(string text)
        {
            try
            {
                Tokenizer tokenizer = new Tokenizer(text);
                List<Token> tokens = new List<Token>();
                while (true)
                {
                    Token token = tokenizer.GetNextToken();
                    if (token.Type == TokenType.EOF) break;
                    tokens.Add(token);
                }
                Parser parser = new Parser(text);
                object result = parser.Parse().Evaluate();
                InfoForm.ShowValue(string.Join(",", tokens) + "\r\n\r\n" + result, "Tokenizer Results", tokens.Count + " tokens");
            }
            catch (Exception e)
            {
                InfoForm.ShowValue(e.Message, "Error", "Error");
            }
        }

        public void SetIsEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public void Update()
        {
            if (!_isEnabled) return;

            // run script
        }
    }
}
