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
    public class WarpManager : DataManager
    {
        public WarpManager(string varFilePath, TabPage tabPage, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(varFilePath, watchVariablePanel)
        {
            /*
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
                */
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            /*
            short numSnowParticles = Config.Stream.GetInt16(SnowConfig.CounterAddress);
            if (numSnowParticles > _numSnowParticles) // need to add controls
            {
                for (int i = _numSnowParticles; i < numSnowParticles; i++)
                {
                    List<WatchVariableControl> snowParticle = GetSnowParticleControls(i);
                    _snowParticleControls.Add(snowParticle);
                    _variablePanel.AddVariables(snowParticle);
                }
                _numSnowParticles = numSnowParticles;
            }
            else if (numSnowParticles < _numSnowParticles) // need to remove controls
            {
                for (int i = _numSnowParticles - 1; i >= numSnowParticles; i--)
                {
                    List<WatchVariableControl> snowParticle = _snowParticleControls[i];
                    _snowParticleControls.Remove(snowParticle);
                    _variablePanel.RemoveVariables(snowParticle);
                }
                _numSnowParticles = numSnowParticles;
            }
            */
            base.Update(updateView);
        }
    }
}