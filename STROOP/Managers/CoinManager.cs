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
    public class CoinManager
    {

        private static readonly int _memorySize = (int)ObjectConfig.StructSize;

        public CoinManager(TabPage tabControl)
        {
           
        }
        
        public void Update(bool updateView)
        {
            if (!updateView) return;
            
        }
    }
}
