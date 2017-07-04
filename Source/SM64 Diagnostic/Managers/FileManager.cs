using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;

namespace SM64_Diagnostic.Managers
{
    public class FileManager : DataManager
    {
        Control _tabControl;

        public FileManager(ProcessStream stream, List<WatchVariable> fileData, Control tabControl, NoTearFlowLayoutPanel noTearFlowLayoutPanelFile)
            : base(stream, fileData, noTearFlowLayoutPanelFile)
        {
            _tabControl = tabControl;

            SplitContainer splitContainerHud = tabControl.Controls["splitContainerFile"] as SplitContainer;

            /*
            (splitContainerHud.Panel1.Controls["buttonFillHp"] as Button).Click += buttonFill_Click;
            (splitContainerHud.Panel1.Controls["buttonDie"] as Button).Click += buttonDie_Click;
            (splitContainerHud.Panel1.Controls["buttonStandardHud"] as Button).Click += buttonStandardHud_Click;
            */
        }
    }
}
