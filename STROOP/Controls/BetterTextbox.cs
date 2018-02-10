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
                this.Reset();
                this.Parent.Focus();
            });
        }

        public void Reset()
        {
            this.Text = lastSubmittedText;
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
