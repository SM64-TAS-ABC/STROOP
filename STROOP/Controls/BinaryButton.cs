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
    public class BinaryButton : Button
    {
        private string _primaryText;
        private string _secondaryText;
        private Func<bool> _isSecondaryFunction;

        private bool _isSecondary;

        public BinaryButton()
        {
        }

        public void Initialize(string primaryText, string secondaryText, Action primaryAction, Action secondaryAction, Func<bool> isSecondaryFunction)
        {
            _primaryText = primaryText;
            _secondaryText = secondaryText;
            _isSecondaryFunction = isSecondaryFunction;

            base.Click += (sender, e) =>
            {
                if (_isSecondary) secondaryAction();
                else primaryAction();
            };
        }

        public void UpdateButton()
        {
            bool isSecondary = _isSecondaryFunction();
            _isSecondary = isSecondary;
            base.Text = isSecondary ? _secondaryText : _primaryText;
        }
    }
}
