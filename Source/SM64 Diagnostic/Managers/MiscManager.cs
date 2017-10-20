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
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class MiscManager : DataManager
    {
        public int ActiveObjectCount = 0;

        BetterTextbox _betterTextboxRNGIndex;
        CheckBox _checkBoxTurnOffMusic;

        public MiscManager(List<WatchVariable> watchVariables, NoTearFlowLayoutPanel variableTable, Control miscControl)
            : base(watchVariables, variableTable)
        {
            SplitContainer splitContainerMisc = miscControl.Controls["splitContainerMisc"] as SplitContainer;
            GroupBox groupBoxRNGIndex = splitContainerMisc.Panel1.Controls["groupBoxRNGIndex"] as GroupBox;
            _betterTextboxRNGIndex = groupBoxRNGIndex.Controls["betterTextboxRNGIndex"] as BetterTextbox;

            _checkBoxTurnOffMusic = splitContainerMisc.Panel1.Controls["checkBoxTurnOffMusic"] as CheckBox;
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

        private void ProcessSpecialVars()
        {
            foreach (var specialVar in _specialWatchVars)
            {
                switch(specialVar.SpecialName)
                {
                    case "RngIndex":
                        int rngIndex = RngIndexer.GetRngIndex(Config.Stream.GetUInt16(Config.RngAddress));
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
        }

        private int GetRngCallsPerFrame()
        {
            var currentRng = Config.Stream.GetUInt16(Config.HackedAreaAddress + 0x0E);
            var preRng = Config.Stream.GetUInt16(Config.HackedAreaAddress + 0x0C);

            return RngIndexer.GetRngIndexDiff(preRng, currentRng);
        }

    }
}
