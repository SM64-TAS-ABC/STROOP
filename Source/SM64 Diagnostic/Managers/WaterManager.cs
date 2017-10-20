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
    public class WaterManager : DataManager
    {
        public WaterManager(List<WatchVariable> watchVariables, NoTearFlowLayoutPanel variableTable)
            : base(watchVariables, variableTable)
        {
        }

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("WaterAboveMedian"),
                new DataContainer("MarioAboveWater"),
            };
        }

        private void ProcessSpecialVars()
        {
            foreach (var specialVar in _specialWatchVars)
            {
                switch(specialVar.SpecialName)
                {
                    case "WaterAboveMedian":
                        {
                            double waterLevel = Config.Stream.GetInt16(Config.Mario.StructAddress + 0x76);
                            double waterLevelMedian = Config.Stream.GetInt16(0x8036118A);
                            (specialVar as DataContainer).Text = Math.Round(waterLevel - waterLevelMedian, 3).ToString();
                            break;
                        }

                    case "MarioAboveWater":
                        {
                            double waterLevel = Config.Stream.GetInt16(Config.Mario.StructAddress + 0x76);
                            double marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                            (specialVar as DataContainer).Text = Math.Round(marioY - waterLevel, 3).ToString();
                            break;
                        }
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

    }
}
