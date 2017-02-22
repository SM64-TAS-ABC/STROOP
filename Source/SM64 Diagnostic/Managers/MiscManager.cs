using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Controls;

namespace SM64_Diagnostic.Managers
{
    public class MiscManager : DataManager
    {
        Control _puController;

        public int ActiveObjectCount = 0;

        enum PuControl { Home, PuUp, PuDown, PuLeft, PuRight, QpuUp, QpuDown, QpuLeft, QpuRight};

        public MiscManager(ProcessStream stream, List<WatchVariable> watchVariables, NoTearFlowLayoutPanel variableTable, Control puController)
            : base(stream, watchVariables, variableTable)
        {
            _puController = puController;

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

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("RngIndex"),
                new DataContainer("RngCallsPerFrame"),
                new DataContainer("NumberOfLoadedObjects")
            };
        }

        private void PuControl_Click(object sender, EventArgs e, PuControl controlType)
        {
            switch(controlType)
            {
                case PuControl.Home:
                    PuUtilities.MoveToPu(_stream, 0, 0, 0);
                    break;
                case PuControl.PuUp:
                    PuUtilities.MoveToRelativePu(_stream, 0, 0, -1);
                    break;
                case PuControl.PuDown:
                    PuUtilities.MoveToRelativePu(_stream, 0, 0, 1);
                    break;
                case PuControl.PuLeft:
                    PuUtilities.MoveToRelativePu(_stream, -1, 0, 0);
                    break;
                case PuControl.PuRight:
                    PuUtilities.MoveToRelativePu(_stream, 1, 0, 0);
                    break;
                case PuControl.QpuUp:
                    PuUtilities.MoveToRelativePu(_stream, 0, 0, -4);
                    break;
                case PuControl.QpuDown:
                    PuUtilities.MoveToRelativePu(_stream, 0, 0, 4);
                    break;
                case PuControl.QpuLeft:
                    PuUtilities.MoveToRelativePu(_stream, -4, 0, 0);
                    break;
                case PuControl.QpuRight:
                    PuUtilities.MoveToRelativePu(_stream, 4, 0, 0);
                    break;
            }
        }

        private void ProcessSpecialVars()
        {
            foreach (var specialVar in _specialWatchVars)
            {
                switch(specialVar.SpecialName)
                {
                    case "RngIndex":
                        int rngIndex = RngIndexer.GetRngIndex(_stream.GetUInt16(Config.RngAddress));
                        (specialVar as DataContainer).Text = (rngIndex < 0) ? "N/A [" + (-rngIndex).ToString() + "]" : rngIndex.ToString();
                        break;

                    case "RngCallsPerFrame":
                        (specialVar as DataContainer).Text = GetRngCallsPerFrame().ToString();
                        break;

                    case "NumberOfLoadedObjects":
                        (specialVar as DataContainer).Text = ActiveObjectCount.ToString();
                        break;
                }
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            base.Update();
            ProcessSpecialVars();

            _puController.Controls["labelPuConPuValue"].Text = PuUtilities.GetPuPosString(_stream);
            _puController.Controls["labelPuConQpuValue"].Text = PuUtilities.GetQpuPosString(_stream);
        }

        private int GetRngCallsPerFrame()
        {
            var currentRng = _stream.GetUInt16(Config.RngRecordingAreaAddress + 0x0E);
            var preRng = _stream.GetUInt16(Config.RngRecordingAreaAddress + 0x0C);

            return RngIndexer.GetRngIndexDiff(preRng, currentRng);
        }

    }
}
