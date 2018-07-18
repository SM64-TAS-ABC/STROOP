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
using System.Runtime.InteropServices;

namespace STROOP
{
    public class TabControlEx : TabControl
    {
        private TabPage _currentTab;

        private TabPage _previousTab;
        public TabPage PreviousTab
        {
            get => _previousTab ?? SelectedTab;
            private set => _previousTab = value;
        }

        public TabControlEx()
        {
            SelectedIndexChanged += (sender, e) =>
            {
                PreviousTab = _currentTab ?? TabPages[0];
                _currentTab = SelectedTab;
            };
        }
    }
}
