using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Extensions;

namespace SM64_Diagnostic.ManagerClasses
{
    public class MiscManager
    {
        Config _config;
        List<WatchVariableControl> _watchVarControls;
        FlowLayoutPanel _variableTable;
        ProcessStream _stream;
        DataContainer _rngIndex, _rngPerFrame, _activeObjCnt;
        Control _puController;

        public int ActiveObjectCount = 0;

        enum PuControl { Home, PuUp, PuDown, PuLeft, PuRight, QpuUp, QpuDown, QpuLeft, QpuRight};

        public MiscManager(ProcessStream stream, Config config, List<WatchVariable> watchVariables, FlowLayoutPanel variableTable, Control puController)
        {
            _config = config;
            _variableTable = variableTable;
            _stream = stream;
            _puController = puController;

            // Initialize special variables
            _rngIndex = new DataContainer("RNG Index");
            _rngPerFrame = new DataContainer("RNG Calls/Frame");
            _activeObjCnt = new DataContainer("Num. Loaded Objs.");

            _watchVarControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in watchVariables)
            {
                if (!watchVar.Special)
                {
                    WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, 0);
                    variableTable.Controls.Add(watchControl.Control);
                    _watchVarControls.Add(watchControl);
                    continue;
                }

                switch (watchVar.SpecialType)
                {
                    case "RngIndex":
                        _rngIndex.Name = watchVar.Name;
                        variableTable.Controls.Add(_rngIndex.Control);
                        break;

                    case "RngCallsPerFrame":
                        _rngPerFrame.Name = watchVar.Name;
                        variableTable.Controls.Add(_rngPerFrame.Control);
                        break;

                    case "NumberOfLoadedObjects":
                        _activeObjCnt.Name = watchVar.Name;
                        variableTable.Controls.Add(_activeObjCnt.Control);
                        break;

                    default:
                        var failedContainer = new DataContainer(watchVar.Name);
                        failedContainer.Text = "Couldn't Find";
                        variableTable.Controls.Add(failedContainer.Control);
                        break;
                }
            }

            // Pu Controller initialize and register click events
            _puController.Controls["buttonPuConHome"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.Home);
            _puController.Controls["buttonPuConZnQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuUp);
            _puController.Controls["buttonPuConZpQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuDown);
            _puController.Controls["buttonPuConXnQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuLeft);
            _puController.Controls["buttonPuConXpQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuRight);
            _puController.Controls["buttonPuConZnPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuUp);
            _puController.Controls["buttonPuConZpPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuDown);
            _puController.Controls["buttonPuConXnPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuLeft);
            _puController.Controls["buttonPuConXpPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuRight);
        }

        private void PuControl_Click(object sender, EventArgs e, PuControl controlType)
        {
            switch(controlType)
            {
                case PuControl.Home:
                    PuUtilities.MoveToPu(_stream, _config, 0, 0, 0);
                    break;
                case PuControl.PuUp:
                    PuUtilities.MoveToRelativePu(_stream, _config, 0, 0, -1);
                    break;
                case PuControl.PuDown:
                    PuUtilities.MoveToRelativePu(_stream, _config, 0, 0, 1);
                    break;
                case PuControl.PuLeft:
                    PuUtilities.MoveToRelativePu(_stream, _config, -1, 0, 0);
                    break;
                case PuControl.PuRight:
                    PuUtilities.MoveToRelativePu(_stream, _config, 1, 0, 0);
                    break;
                case PuControl.QpuUp:
                    PuUtilities.MoveToRelativePu(_stream, _config, 0, 0, -4);
                    break;
                case PuControl.QpuDown:
                    PuUtilities.MoveToRelativePu(_stream, _config, 0, 0, 4);
                    break;
                case PuControl.QpuLeft:
                    PuUtilities.MoveToRelativePu(_stream, _config, -4, 0, 0);
                    break;
                case PuControl.QpuRight:
                    PuUtilities.MoveToRelativePu(_stream, _config, 4, 0, 0);
                    break;
            }
        }

        public void Update(bool updateView)
        {
            // Update watch variables
            foreach (var watchVar in _watchVarControls)
                watchVar.Update();

            if (!updateView)
                return;

            // Update the rng index
            int rngIndex = RngIndexer.GetRngIndex(BitConverter.ToUInt16(_stream.ReadRam(_config.RngAddress, 2), 0));
            _rngIndex.Text = (rngIndex < 0) ? "N/A [" + (-rngIndex).ToString() + "]" : rngIndex.ToString();

            _rngPerFrame.Text = GetRngCallsPerFrame().ToString();

            _activeObjCnt.Text = ActiveObjectCount.ToString();
            _puController.Controls["labelPuConPuValue"].Text = PuUtilities.GetPuPosString(_stream, _config);
            _puController.Controls["labelPuConQpuValue"].Text = PuUtilities.GetQpuPosString(_stream, _config);
        }

        private int GetRngCallsPerFrame()
        {
            var currentRng = BitConverter.ToUInt16(_stream.ReadRam(_config.RngRecordingAreaAddress + 0x0E, 2), 0);
            var preRng = BitConverter.ToUInt16(_stream.ReadRam(_config.RngRecordingAreaAddress + 0x0C, 2), 0);

            return RngIndexer.GetRngIndexDiff(preRng, currentRng);
        }

    }
}
