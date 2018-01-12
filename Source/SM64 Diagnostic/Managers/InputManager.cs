using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class InputManager : DataManager
    {
        InputImageGui _gui;
        InputDisplayPanel _inputDisplayPanel;

        public InputManager(List<VarXControl> variables, Control inputControl, VariablePanel variableTable, InputImageGui gui)
            : base(variables, variableTable)
        {
            _gui = gui;

            SplitContainer splitContainerInput = inputControl.Controls["splitContainerInput"] as SplitContainer;
            _inputDisplayPanel = splitContainerInput.Panel1.Controls["inputDisplayPanel"] as InputDisplayPanel;

            _inputDisplayPanel.setInputDisplayGui(_gui);
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update();
            _inputDisplayPanel.Invalidate();
        }
    }
}
