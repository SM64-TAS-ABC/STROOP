using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class PasteUtilities
    {
        public static void Paste(List<WatchVariableControl> varList)
        {
            List<string> stringList = ParsingUtilities.ParseStringList(Clipboard.GetText());
            if (stringList.Count == 0) return;

            Config.Stream.Suspend();
            for (int i = 0; i < varList.Count; i++)
            {
                bool success = varList[i].SetValue(stringList[i % stringList.Count], true);
                varList[i].FlashColor(success ? WatchVariableControl.PASTE_COLOR : WatchVariableControl.FAILURE_COLOR);
            }
            Config.Stream.Resume();
        }
    }
}
