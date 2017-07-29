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
using SM64Diagnostic.Controls;

namespace SM64_Diagnostic.Managers
{
    public class ControllerManager : DataManager
    {
        ControllerImageGui _gui;
        ControllerDisplayPanel _controllerDisplayPanel;

        public ControllerManager(ProcessStream stream, List<WatchVariable> controllerData, Control controllerControl, NoTearFlowLayoutPanel variableTable, ControllerImageGui gui)
            : base(stream, controllerData, variableTable)
        {
            _gui = gui;

            SplitContainer splitContainerController = controllerControl.Controls["splitContainerController"] as SplitContainer;
            _controllerDisplayPanel = splitContainerController.Panel1.Controls["controllerDisplayPanel"] as ControllerDisplayPanel;

            _controllerDisplayPanel.setControllerDisplayGui(_gui);
            _controllerDisplayPanel.setProcessStream(_stream);
        }

        public override void Update(bool updateView)
        {
            base.Update();
            _controllerDisplayPanel.Invalidate();
        }
    }
}
