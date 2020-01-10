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
        private readonly ScriptEngine engine = new ScriptEngine("{16d51579-a30b-4c8b-a276-0ff4dc41e755}");

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
            if (_isEnabled)
            {
                Run();
            }
        }

        public void Run()
        {
            List<(string, object)> inputData = Config.ScriptManager.GetCurrentVariableNamesAndValues();
            List<string> inputItems = new List<string>();
            foreach ((string name, object value) in inputData)
            {
                inputItems.Add("\"" + name + "\":\"" + value + "\"");
            }
            string beforeLine = "var INPUT = {" + string.Join(",", inputItems) + "}; var OUTPUT = {};" + "\r\n";
            string afterLine = "\r\n" + @"var OUTPUT_STRING = """"; for (var OUTPUT_STRING_NAME in OUTPUT) OUTPUT_STRING += OUTPUT_STRING_NAME + ""\r\n"" + OUTPUT[OUTPUT_STRING_NAME] + ""\r\n""; OUTPUT_STRING";
            string result = engine.Eval(beforeLine + _text + afterLine)?.ToString() ?? "";
            List<string> outputItems = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < outputItems.Count - 1; i += 2)
            {
                string name = outputItems[i];
                string value = outputItems[i + 1];
                Config.ScriptManager.SetVariableValueByName(name, value);
            }
            return;

            //try
            //{
            //    SymbolTable.Reset();
            //    Tokenizer tokenizer = new Tokenizer(_text);
            //    List<Token> tokens = new List<Token>();
            //    while (true)
            //    {
            //        Token token = tokenizer.GetNextToken();
            //        if (token.Type == TokenType.EOF) break;
            //        tokens.Add(token);
            //    }
            //    Parser parser = new Parser(_text);
            //    object result = parser.Parse().Evaluate();
            //    InfoForm.ShowValue(
            //        string.Join(",", tokens)
            //            + "\r\n\r\n"
            //            + result
            //            + "\r\n\r\n"
            //            + SymbolTable.GetString(),
            //        "Tokenizer Results",
            //        tokens.Count + " tokens");
            //}
            //catch (Exception e)
            //{
            //    InfoForm.ShowValue(e.Message, "Error", "Error");
            //}
        }
    }
}
