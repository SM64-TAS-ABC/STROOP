﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Extensions;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class InputManager : DataManager
    {
        List<InputImageGui> _guiList;
        InputDisplayPanel _inputDisplayPanel;

        public InputManager(
            string varFilePath, Control inputControl, WatchVariableFlowLayoutPanel variableTable,
            List<InputImageGui> guiList)
            : base(varFilePath, variableTable)
        {
            _guiList = guiList;

            SplitContainer splitContainerInput = inputControl.Controls["splitContainerInput"] as SplitContainer;
            _inputDisplayPanel = splitContainerInput.Panel1.Controls["inputDisplayPanel"] as InputDisplayPanel;

            _inputDisplayPanel.SetInputDisplayGui(_guiList);
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);

            _inputDisplayPanel.UpdateInputs();
            _inputDisplayPanel.Invalidate();
        }
    }
}
