using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Extensions;
using STROOP.Controls;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class SnowManager : DataManager
    {
        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
                VariableGroup.Snow,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.Basic,
                VariableGroup.Intermediate,
                VariableGroup.Advanced,
                VariableGroup.Snow,
            };

        private short _numSnowParticles;
        private List<List<WatchVariableControl>> _snowParticleControls;

        public SnowManager(string varFilePath, WatchVariableFlowLayoutPanel variableTable)
            : base(varFilePath, variableTable, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {
            _numSnowParticles = 0;
            _snowParticleControls = new List<List<WatchVariableControl>>();
        }

        private List<WatchVariableControl> GetSnowParticleControls(int index)
        {
            uint structOffset = (uint)index * SnowConfig.ParticleStructSize;
            List<uint> offsets = new List<uint>()
            {
                structOffset + SnowConfig.XOffset,
                structOffset + SnowConfig.YOffset,
                structOffset + SnowConfig.ZOffset,
            };
            List<string> names = new List<string>()
            {
                String.Format("Particle {0} X", index),
                String.Format("Particle {0} Y", index),
                String.Format("Particle {0} Z", index),
            };

            List<WatchVariableControl> controls = new List<WatchVariableControl>();
            for (int i = 0; i < 3; i++)
            {
                WatchVariable watchVar = new WatchVariable(
                    memoryTypeName: "int",
                    specialType: null,
                    baseAddressType: BaseAddressTypeEnum.Snow,
                    offsetUS: null,
                    offsetJP: null,
                    offsetPAL: null,
                    offsetDefault: offsets[i],
                    mask: null);
                WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(
                    name: names[i],
                    watchVar: watchVar,
                    subclass: WatchVariableSubclass.Number,
                    backgroundColor: null,
                    displayType: null,
                    roundingLimit: null,
                    useHex: null,
                    invertBool: null,
                    isYaw: null,
                    coordinate: null,
                    groupList: new List<VariableGroup>() { VariableGroup.Snow });
                controls.Add(precursor.CreateWatchVariableControl());
            }
            return controls;
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            short numSnowParticles = Config.Stream.GetInt16(SnowConfig.CounterAddress);
            if (numSnowParticles > _numSnowParticles) // need to add controls
            {
                for (int i = _numSnowParticles; i < numSnowParticles; i++)
                {
                    List<WatchVariableControl> snowParticle = GetSnowParticleControls(i);
                    _snowParticleControls.Add(snowParticle);
                    _variablePanel.AddVariables(snowParticle);
                }
                _numSnowParticles = numSnowParticles;
            }
            else if (numSnowParticles < _numSnowParticles) // need to remove controls
            {

            }

            base.Update(updateView);
        }

    }
}
