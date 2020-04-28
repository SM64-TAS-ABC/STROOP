using STROOP.Controls;
using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class CopyUtilities
    {
        public static void AddContextMenuStripFunctions(
            Control control, Func<List<WatchVariableControl>> getVars)
        {
            ControlUtilities.AddContextMenuStripFunctions(
                control,
                GetCopyNames(),
                GetCopyActions(getVars));
        }

        public static void AddDropDownItems(
            ToolStripMenuItem control, Func<List<WatchVariableControl>> getVars)
        {
            ControlUtilities.AddDropDownItems(
                control,
                GetCopyNames(),
                GetCopyActions(getVars));
        }

        private static List<string> GetCopyNames()
        {
            return new List<string>()
            {
                "Copy with Commas",
                "Copy with Spaces",
                "Copy with Tabs",
                "Copy with Line Breaks",
                "Copy with Names",
                "Copy for Code",
            };
        }

        private static List<Action> GetCopyActions(Func<List<WatchVariableControl>> getVars)
        {
            return new List<Action>()
            {
                () => CopyWithSeparator(getVars(), ","),
                () => CopyWithSeparator(getVars(), " "),
                () => CopyWithSeparator(getVars(), "\t"),
                () => CopyWithSeparator(getVars(), "\r\n"),
                () => CopyWithNames(getVars()),
                () => CopyForCode(getVars()),
            };
        }

        private static void CopyWithSeparator(
            List<WatchVariableControl> controls, string separator)
        {
            if (controls.Count == 0)
            {
                Clipboard.SetText("");
                return;
            }
            Clipboard.SetText(
                string.Join(separator, controls.ConvertAll(
                    control => control.GetValue(
                        useRounding: false, handleFormatting: true))));
        }

        private static void CopyWithNames(List<WatchVariableControl> controls)
        {
            if (controls.Count == 0)
            {
                Clipboard.SetText("");
                return;
            }
            List<string> lines = controls.ConvertAll(
                watchVar => watchVar.VarName + "\t" + watchVar.GetValue(false));
            Clipboard.SetText(string.Join("\r\n", lines));
        }

        private static void CopyForCode(List<WatchVariableControl> controls)
        {
            if (controls.Count == 0)
            {
                Clipboard.SetText("");
                return;
            }
            Func<string, string> varNameFunc;
            if (KeyboardUtilities.IsCtrlHeld())
            {
                string template = DialogUtilities.GetStringFromDialog("$");
                if (template == null) return;
                varNameFunc = varName => template.Replace("$", varName);
            }
            else
            {
                varNameFunc = varName => varName;
            }
            List<string> lines = new List<string>();
            foreach (WatchVariableControl watchVar in controls)
            {
                Type type = watchVar.GetMemoryType();
                string line = string.Format(
                    "{0} {1} = {2}{3};",
                    type != null ? TypeUtilities.TypeToString[watchVar.GetMemoryType()] : "double",
                    varNameFunc(watchVar.VarName.Replace(" ", "")),
                    watchVar.GetValue(false),
                    type == typeof(float) ? "f" : "");
                lines.Add(line);
            }
            if (lines.Count > 0)
            {
                Clipboard.SetText(string.Join("\r\n", lines));
            }
        }
    }
}
