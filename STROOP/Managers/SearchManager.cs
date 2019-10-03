using STROOP.Controls;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace STROOP.Managers
{
    public class SearchManager : DataManager
    {
        public SearchManager(TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(null, watchVariablePanel)
        {
            
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
        }
    }
}
