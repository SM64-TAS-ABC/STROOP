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
        private readonly List<string> _consoleStrings;

        public TokenScript(List<string> consoleStrings)
        {
            _consoleStrings = consoleStrings;
        }

        public void Run(string text)
        {
            List<(string, object, string)> inputData = Config.ScriptManager.GetCurrentVariableInfo();
            List<string> inputItems = new List<string>();
            foreach ((string name, object value, string clazz) in inputData)
            {
                string valueMark = clazz == "String" ? "\"" : "";
                inputItems.Add("\"" + name + "\":" + valueMark + value + valueMark);
            }
            List<string> consoleItems = new List<string>();
            foreach (string s in _consoleStrings)
            {
                consoleItems.Add("\"" + s + "\"");
            }
            string beforeLine = "var INPUT = {" + string.Join(",", inputItems) + "}; var OUTPUT = {}; var CONSOLE = [" + string.Join(",", consoleItems) + "];";
            string afterLine1 = @"var OUTPUT_STRING = """"; for (var OUTPUT_STRING_NAME in OUTPUT) OUTPUT_STRING += OUTPUT_STRING_NAME + ""\r\n"" + OUTPUT[OUTPUT_STRING_NAME] + ""\r\n"";";
            string afterLine2 = @"var CONSOLE_STRING = """"; for (var CONSOLE_INDEX in CONSOLE) CONSOLE_STRING += CONSOLE[CONSOLE_INDEX] + ""\r\n"";";
            string afterLine3 = @"OUTPUT_STRING + ""\0"" + CONSOLE_STRING";
            string processedText = PreProcess(text);
            string result = GetEngine().Eval(beforeLine + "\r\n" + processedText + "\r\n" + afterLine1 + "\r\n" + afterLine2 + "\r\n" + afterLine3)?.ToString() ?? "";

            int index = result.IndexOf("\0");
            if (index == -1) return;
            string outputString = result.Substring(0, index);
            string consoleString = result.Substring(index + 1);

            List<string> outputItems = outputString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < outputItems.Count - 1; i += 2)
            {
                string name = outputItems[i];
                string value = outputItems[i + 1];
                Config.ScriptManager.SetVariableValueByName(name, value);
            }

            List<string> newConsoleStrings = consoleString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            _consoleStrings.Clear();
            _consoleStrings.AddRange(newConsoleStrings);
        }

        private string PreProcess(string text)
        {
            try
            {
                List<string> lines = text.Split(new string[] { "\n" }, StringSplitOptions.None).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i].Trim();
                    List<string> parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (parts.Count > 0 && parts[0] == LOAD_FILE_KEY_WORD)
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
