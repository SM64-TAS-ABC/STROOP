using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Managers;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Controls;
using STROOP.Extensions;
using System.Drawing.Drawing2D;

namespace STROOP
{
    public class TrackBarEx : TrackBar
    {
        private bool _isBeingChangedByCode = false;

        public TrackBarEx()
        {
        }

        public void AddManualChangeAction(Action action)
        {
            ValueChanged += (sender, e) =>
            {
                if (!_isBeingChangedByCode) action();
            };
        }

        public void StartChangingByCode()
        {
            _isBeingChangedByCode = true;
        }

        public void StopChangingByCode()
        {
            _isBeingChangedByCode = false;
        }
    }
}
