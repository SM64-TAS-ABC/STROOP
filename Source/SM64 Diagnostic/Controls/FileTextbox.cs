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
    public abstract class FileTextbox : TextBox
    {
        protected ProcessStream _stream;
        protected uint _addressOffset;

        public FileTextbox()
        {
        }

        public virtual void Initialize(ProcessStream stream, uint addressOffset)
        {
            _stream = stream;
            _addressOffset = addressOffset;

            this.Click += (sender, e) => this.SelectAll();
            this.KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Enter)
                {
                    SubmitValue();
                    this.Parent.Focus();
                }
            };
            this.LostFocus += (sender, e) => SubmitValue();
        }

        protected abstract void SubmitValue();

        public abstract void UpdateText();
    }
}
