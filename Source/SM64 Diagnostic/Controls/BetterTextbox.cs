using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using System.Drawing.Drawing2D;

namespace SM64_Diagnostic
{
    public class BetterTextbox : TextBox
    {
        public BetterTextbox()
        {
            AddEnterAction(() => Parent.Focus());
        }

        public void AddEnterAction(Action enterAction)
        {
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Enter)
                {
                    enterAction();
                }
            };
        }

        public void AddLostFocusAction(Action lostFocusAction)
        {
            this.LostFocus += (sender, e) => lostFocusAction();
        }
    }
}
