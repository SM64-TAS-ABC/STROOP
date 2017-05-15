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
    public class ActionsManager : DataManager
    {
        public ActionsManager(ProcessStream stream, List<WatchVariable> actionsData, NoTearFlowLayoutPanel variableTable)
            : base(stream, actionsData, variableTable, Config.Mario.StructAddress)
        {
        }

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("ActionDescription"),
                new DataContainer("PrevActionDescription"),
            };
        }

        public void ProcessSpecialVars()
        {
            foreach (var specialVar in _specialWatchVars)
            {
                switch(specialVar.SpecialName)
                {                  
                    case "ActionDescription":
                        (specialVar as DataContainer).Text = Config.MarioActions.GetActionName(_stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.ActionOffset));
                        break;

                    case "PrevActionDescription":
                        (specialVar as DataContainer).Text = Config.MarioActions.GetActionName(_stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.PrevActionOffset));
                        break;
                }
            }
        }

        public override void Update(bool updateView)
        {
            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();
            ProcessSpecialVars();
        }
    }
}
