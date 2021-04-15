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
        private static readonly string LOAD_FILE_KEY_WORD = "LOADFILE";

        private ScriptEngine _engine;

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
                string valueMark = value is string ? "\"" : "";
                inputItems.Add("\"" + name + "\":" + valueMark + value + valueMark);
            }
            string beforeLine = "var INPUT = {" + string.Join(",", inputItems) + "}; var OUTPUT = {};";
            string afterLine = @"var OUTPUT_STRING = """"; for (var OUTPUT_STRING_NAME in OUTPUT) OUTPUT_STRING += OUTPUT_STRING_NAME + ""\r\n"" + OUTPUT[OUTPUT_STRING_NAME] + ""\r\n""; OUTPUT_STRING";
            string text = PreProcess(_text);
            string result = GetEngine().Eval(beforeLine + "\r\n" + text + "\r\n" + afterLine)?.ToString() ?? "";
            List<string> outputItems = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < outputItems.Count - 1; i += 2)
            {
                string name = outputItems[i];
                string value = outputItems[i + 1];
                Config.ScriptManager.SetVariableValueByName(name, value);
            }
        }

        private string PreProcess(string text)
        {
            try
            {
                List<string> lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i];
                    List<string> parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (parts[0] == LOAD_FILE_KEY_WORD)
                    {
                        int keyWordIndex = line.IndexOf(LOAD_FILE_KEY_WORD);
                        string filePath = line.Substring(keyWordIndex + LOAD_FILE_KEY_WORD.Length).Trim();
                        string fileString = DialogUtilities.ReadFile(filePath);
                        lines[i] = fileString;
                    }
                }
                return string.Join("\r\n", lines);
            }
            catch (Exception)
            {
                return text;
            }
        }

        // Lazily create script engine because it breaks wine
        private ScriptEngine GetEngine()
        {
            if (_engine == null)
            {
                _engine = new ScriptEngine(ScriptEngine.ChakraClsid);
            }
            return _engine;
        }
    }
}
