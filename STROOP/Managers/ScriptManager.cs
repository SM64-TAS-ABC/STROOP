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
        private CheckBox _checkBoxScriptRunScript;
        private Button _buttonScriptInstructions;
        private RichTextBoxEx _richTextBoxScript;

        private TokenScript _script;

        public ScriptManager(string varFilePath, TabPage tabPage, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel)
        {
            SplitContainer splitContainer = tabPage.Controls["splitContainerScript"] as SplitContainer;
            SplitContainer splitContainerLeft = splitContainer.Panel1.Controls["splitContainerScriptLeft"] as SplitContainer;
            _checkBoxScriptRunScript = splitContainerLeft.Panel1.Controls["checkBoxScriptRunScript"] as CheckBox;
            _buttonScriptInstructions = splitContainerLeft.Panel1.Controls["buttonScriptInstructions"] as Button;
            _richTextBoxScript = splitContainerLeft.Panel2.Controls["richTextBoxScript"] as RichTextBoxEx;

            _script = new TokenScript();

            _checkBoxScriptRunScript.Click += (sender, e) =>
            {
                if (_checkBoxScriptRunScript.Checked)
                {
                    _script.SetScript(_richTextBoxScript.Text);
                }
                _script.SetIsEnabled(_checkBoxScriptRunScript.Checked);
            };

            _buttonScriptInstructions.Click += (sender, e) =>
            {
                InfoForm.ShowValue(
                    "To use the script tab, we must first implement the script tab.",
                    "Instructions",
                    "Instructions");
            };
        }

        public override void Update(bool updateView)
        {
            _script.Update();

            if (!updateView) return;

            base.Update(updateView);
        }
    }
}