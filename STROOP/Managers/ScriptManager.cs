using STROOP.Controls;
using STROOP.Forms;
using STROOP.Script;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class ScriptManager : DataManager
    {
        private readonly CheckBox _checkBoxScriptRunContinuously;
        private readonly Button _buttonScriptRunOnce;
        private readonly Button _buttonScriptInstructions;
        private readonly Button _buttonScriptExamples;
        private readonly RichTextBoxEx _richTextBoxScript;

        private readonly TokenScript _script;

        public ScriptManager(string varFilePath, TabPage tabPage, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel)
        {
            SplitContainer splitContainer = tabPage.Controls["splitContainerScript"] as SplitContainer;
            SplitContainer splitContainerLeft = splitContainer.Panel1.Controls["splitContainerScriptLeft"] as SplitContainer;
            _checkBoxScriptRunContinuously = splitContainerLeft.Panel1.Controls["checkBoxScriptRunContinuously"] as CheckBox;
            _buttonScriptRunOnce = splitContainerLeft.Panel1.Controls["buttonScriptRunOnce"] as Button;
            _buttonScriptInstructions = splitContainerLeft.Panel1.Controls["buttonScriptInstructions"] as Button;
            _buttonScriptExamples = splitContainerLeft.Panel1.Controls["buttonScriptExamples"] as Button;
            _richTextBoxScript = splitContainerLeft.Panel2.Controls["richTextBoxScript"] as RichTextBoxEx;

            _script = new TokenScript();

            _checkBoxScriptRunContinuously.Click += (sender, e) =>
            {
                if (_checkBoxScriptRunContinuously.Checked)
                {
                    _script.SetScript(_richTextBoxScript.Text);
                }
                _script.SetIsEnabled(_checkBoxScriptRunContinuously.Checked);
                _richTextBoxScript.ReadOnly = _checkBoxScriptRunContinuously.Checked;
            };

            _buttonScriptRunOnce.Click += (sender, e) =>
            {
                _script.SetScript(_richTextBoxScript.Text);
                _script.Run();
            };

            _buttonScriptInstructions.Click += (sender, e) =>
            {
                InfoForm.ShowValue(
                    string.Join("\r\n", _instructions),
                    "Instructions",
                    "Instructions");
            };

            _buttonScriptExamples.ContextMenuStrip = new ContextMenuStrip();
            for (int i = 0; i < _exampleNames.Count; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(_exampleNames[i]);
                string text = string.Join("\r\n", _exampleLines[i]);
                item.Click += (sender, e) => _richTextBoxScript.Text = text;
                _buttonScriptExamples.ContextMenuStrip.Items.Add(item);
            }
            _buttonScriptExamples.Click += (sender, e) =>
                _buttonScriptExamples.ContextMenuStrip.Show(Cursor.Position);
        }

        public override void Update(bool updateView)
        {
            _script.Update();

            if (!updateView) return;

            base.Update(updateView);
        }

        private readonly List<string> _instructions = new List<string>()
        {
            @"The Script Tab can be used to set variables in a custom way defined by you.",
            @"Specifically, you write JavaScript code on the left, which can both read from and write to the variables on the right.",
            @"So if you want to read from or write to a variable, you must first add it to this tab.",
            @"",
            @"Within your JavaScript code, there are 2 implicit objects that you have access to.",
            @"The first of these is INPUT, which can be used to read from the variables.",
            @"For example, to get the value for Mario’s X position, just write INPUT[""Mario X""].",
            @"The second of these is OUTPUT, which can be used to write to the variables.",
            @"For example, to write a value v to Mario’s X position, just write OUTPUT[""Mario X""] = v.",
            @"",
            @"You can run your script continuously (runs once per STROOP frame) or just once.",
            @"To see some examples of scripts you can write, click on the Examples button.",
        };

        private readonly List<string> _exampleNames = new List<string>()
        {
            "Set Mario's X value",
            "Set Mario's X value using Mario's Z value",
            "Contrain Mario's X value to within a range",
            "Set 3 scuttlebug Y speed values using a custom function",
        };

        private readonly List<List<string>> _exampleLines = new List<List<string>>()
        {
            new List<string>()
            {
                @"// Sets Mario X to 6000",
                @"OUTPUT[""Mario X""] = 6000;",
            },
            new List<string>()
            {
                @"// Sets Mario X to Mario Z",
                @"OUTPUT[""Mario X""] = INPUT[""Mario Z""];",
            },
            new List<string>()
            {
                @"// Keeps Mario's X between 6000 and 7000",
                @"if (INPUT[""Mario X""] > 7000) OUTPUT[""Mario X""] = 7000;",
                @"if (INPUT[""Mario X""] < 6000) OUTPUT[""Mario X""] = 6000;",
            },
            new List<string>()
            {
                @"// Sets 3 scuttlebug Y speeds to 20",
                @"// Assumes you have variables ""Scuttlebug Y Speed 1"",",
                @"// ""Scuttlebug Y Speed 2"", ""Scuttlebug Y Speed 3""",
                @"function setYSpeed(index) {",
                @"  OUTPUT[""Scuttlebug Y Speed "" + index] = 20;",
                @"}",
                @"for (var i = 1; i <= 3; i++) {",
                @"  setYSpeed(i);",
                @"}",
            },
        };
    }
}