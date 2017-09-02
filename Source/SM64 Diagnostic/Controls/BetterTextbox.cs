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
        private string lastSubmittedText;

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (lastSubmittedText == null)
                {
                    lastSubmittedText = value;
                }
                base.Text = value;
            }
        }

        public BetterTextbox()
        {
            AddLostFocusAction(() => lastSubmittedText = this.Text);
            AddDoubleClickAction(() => this.SelectAll());
            AddEnterAction(() => Parent.Focus());
            AddEscapeAction(() =>
            {
                this.Text = lastSubmittedText;
                this.Parent.Focus();
            });
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

        public void AddEscapeAction(Action escapeAction)
        {
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Escape)
                {
                    escapeAction();
                }
            };
        }

        public void AddLostFocusAction(Action lostFocusAction)
        {
            this.LostFocus += (sender, e) => lostFocusAction();
        }

        public void AddDoubleClickAction(Action doubleClickAction)
        {
            this.DoubleClick += (sender, e) => doubleClickAction();
        }
    }
}
